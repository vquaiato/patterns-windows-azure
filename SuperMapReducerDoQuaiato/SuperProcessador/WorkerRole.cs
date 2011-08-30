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
using Storage;

namespace SuperProcessador
{
    public class WorkerRole : RoleEntryPoint
    {
        public override void Run()
        {
            Trace.WriteLine("SuperProcessador entry point called", "Information");

            var queuePalavras = new QueuePalavras();
            var tableResultado = new TableResultado();

            while (true)
            {
                var mensagem = queuePalavras.ProximaPalavraParaProcessar();
                if (mensagem != null)
                {
                    var campos = mensagem.AsString.Split(new[] {"@"}, StringSplitOptions.RemoveEmptyEntries);
                    var palavra = campos[0];
                    var letraParaContar = char.Parse(campos[1].Trim(new []{' '}));
                    var identificador = campos[2];

                    var qtdLetras = 0;
                    foreach (char letra in palavra)
                    {
                        if (letra.Equals(letraParaContar))
                            qtdLetras++;
                    }

                    tableResultado.ArmazenarResultado(identificador,qtdLetras);
                    queuePalavras.PalavraProcessada(mensagem);

                    Thread.Sleep(1000);    
                }
                else
                {
                    Thread.Sleep(30000);
                }
                
                Trace.WriteLine("Working", "Information");
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
