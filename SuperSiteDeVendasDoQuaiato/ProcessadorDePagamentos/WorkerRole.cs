using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using RepositorioDeVendas;

namespace ProcessadorDePagamentos
{
    public class WorkerRole : RoleEntryPoint
    {
        public override void Run()
        {
            Trace.WriteLine("ProcessadorDePagamentos entry point called", "Infrmation");

            var pagamentosParaProcessar = new RepositorioDePagamentosParaProcessar();

            while (true)
            {
                Trace.TraceInformation("obtendo pagamento para processar...");
                var pagamentoParaProcessar = pagamentosParaProcessar.ProximoPagamentoParaProcessar();
                if (pagamentoParaProcessar != null)
                {
                    Trace.TraceInformation("processando pagamento");
                    Thread.Sleep(15000);
                    Trace.TraceInformation("pagamento processado");
                    pagamentosParaProcessar.PagamentoProcessado(pagamentoParaProcessar);
                }
                else
                {
                    Trace.TraceInformation("nenhum pagamento para processar, dormindo...");
                    Thread.Sleep(60000);    
                }
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }
    }
}
