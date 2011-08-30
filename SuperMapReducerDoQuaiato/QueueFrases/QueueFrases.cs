using System;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;

namespace Storage
{
    public class QueueFrases
    {
        private static CloudStorageAccount storageAccount;
        private CloudQueueClient queueClient;
        private CloudQueue frasesParaProcessasQueue;

        static QueueFrases()
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
        public QueueFrases()
        {
            queueClient = storageAccount.CreateCloudQueueClient();

            this.frasesParaProcessasQueue = this.queueClient.GetQueueReference("frasesparaprocessar");
            this.frasesParaProcessasQueue.CreateIfNotExist();
        }

        public void NovaFraseParaProcessar(string frase, string letra, Guid identificador)
        {
            this.frasesParaProcessasQueue.AddMessage(new CloudQueueMessage(frase + "@" + letra + "@" + identificador.ToString()));
        }

        public CloudQueueMessage ProximaFraseParaProcessar()
        {
            return this.frasesParaProcessasQueue.GetMessage();
        }
        public void FraseProcessada(CloudQueueMessage frase)
        {
            this.frasesParaProcessasQueue.DeleteMessage(frase);
        }
    }
}