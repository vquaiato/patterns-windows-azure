using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;

namespace Storage
{
    public class QueuePalavras
    {
        private static CloudStorageAccount storageAccount;
        private CloudQueueClient queueClient;
        private CloudQueue palavrasParaProcessasQueue;

        static QueuePalavras()
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
        public QueuePalavras()
        {
            queueClient = storageAccount.CreateCloudQueueClient();

            this.palavrasParaProcessasQueue = this.queueClient.GetQueueReference("palavrasparaprocessar");
            this.palavrasParaProcessasQueue.CreateIfNotExist();
        }

        public void NovaPalavraParaProcessar(string palavra, string letra, string identificador)
        {
            var mensagem = string.Format("{0}@{1}@{2}", palavra, letra, identificador);

            this.palavrasParaProcessasQueue.AddMessage(new CloudQueueMessage(mensagem));
        }

        public CloudQueueMessage ProximaPalavraParaProcessar()
        {
            return this.palavrasParaProcessasQueue.GetMessage();
        }
        public void PalavraProcessada(CloudQueueMessage palavra)
        {
            this.palavrasParaProcessasQueue.DeleteMessage(palavra);
        }
    }
}