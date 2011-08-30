using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure.ServiceRuntime;
using Storage;

namespace Map
{
    public class WorkerRole : RoleEntryPoint
    {
        private int FRASE_INDEX = 0;
        private int LETRA_INDEX = 1;
        private int IDENTIFICADOR_INDEX = 2;

        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            Trace.WriteLine("SuperReducer entry point called", "Information");

            var queueFrases = new QueueFrases();
            var queuePalavras = new QueuePalavras();

            while (true)
            {
                var mensagem = queueFrases.ProximaFraseParaProcessar();
                if (mensagem != null)
                {
                    var fraseLetra = mensagem.AsString.Split(new[] {"@"}, StringSplitOptions.RemoveEmptyEntries);
                    var frase = fraseLetra[FRASE_INDEX];
                    var letra = fraseLetra[LETRA_INDEX];

                    var palavras = frase.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);
                    var identificador = fraseLetra[IDENTIFICADOR_INDEX];

                    foreach (var palavra in palavras)
                    {
                        queuePalavras.NovaPalavraParaProcessar(palavra, letra, identificador);
                    }
                    
                    queueFrases.FraseProcessada(mensagem);
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
