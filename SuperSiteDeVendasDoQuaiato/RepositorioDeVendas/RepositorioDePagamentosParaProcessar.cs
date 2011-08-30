using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using Models;

namespace RepositorioDeVendas
{
    public class RepositorioDePagamentosParaProcessar
    {
        private static CloudStorageAccount storageAccount;
        private CloudQueueClient queueClient;
        private CloudQueue vendasParaProcessasQueue;

        static RepositorioDePagamentosParaProcessar()
        {
            CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
                                                                     {
                                                                         string connectionString = string.Empty;

                                                                         if (RoleEnvironment.IsAvailable)
                                                                         {
                                                                             connectionString = RoleEnvironment.GetConfigurationSettingValue(configName);
                                                                         }

                                                                         configSetter(connectionString);
                                                                     });

            storageAccount = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
        }
        public RepositorioDePagamentosParaProcessar()
        {
            queueClient = storageAccount.CreateCloudQueueClient();

            this.vendasParaProcessasQueue = this.queueClient.GetQueueReference("vendasparaprocessar");
            this.vendasParaProcessasQueue.CreateIfNotExist();
        }

        public void NovaVendaParaProcessar(Venda venda)
        {
            this.vendasParaProcessasQueue.AddMessage(new CloudQueueMessage(venda.ToString()));
        }

        public CloudQueueMessage ProximoPagamentoParaProcessar()
        {
            return this.vendasParaProcessasQueue.GetMessage();
        }
        public void PagamentoProcessado(CloudQueueMessage pagamento)
        {
            this.vendasParaProcessasQueue.DeleteMessage(pagamento);
        }
    }
}