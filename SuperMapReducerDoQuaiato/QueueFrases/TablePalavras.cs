using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;

namespace Storage
{
    public class TablePalavrasDataContext : TableServiceContext
    {
        public TablePalavrasDataContext(string baseAddress, StorageCredentials credentials)
            : base(baseAddress, credentials) { }
    }

    public class TableResultado
    {
        private static CloudStorageAccount storageAccount;
        private TablePalavrasDataContext context;

        static TableResultado()
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

            storageAccount.CreateCloudTableClient().CreateTableIfNotExist("Resultado");
        }

        public TableResultado()
        {
            this.context = new TablePalavrasDataContext(storageAccount.TableEndpoint.AbsoluteUri, storageAccount.Credentials);
            this.context.RetryPolicy = RetryPolicies.Retry(3, TimeSpan.FromSeconds(1));
        }

        public void ArmazenarResultado(string identificador, int quantidade)
        {
            this.context.AddObject("Resultado", new Resultado(identificador, quantidade));
            this.context.SaveChanges();
        }

        public int Consultar(string identificador)
        {
            var totais = this.context
                .CreateQuery<Resultado>("Resultado")
                .Where(r => r.PartitionKey == identificador)
                .ToList();

            return totais.Sum(t=>t.Quantidade);
        }
    }
}
