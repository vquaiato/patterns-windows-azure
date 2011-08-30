using System;
using Microsoft.WindowsAzure.StorageClient;

namespace Storage
{
    public class Resultado : TableServiceEntity
    {
        public int Quantidade { get; set; }
        public Resultado(){}
        public Resultado(string identificador, int quantidade)
        {
            this.PartitionKey = identificador;
            this.RowKey = Guid.NewGuid().ToString();

            Quantidade = quantidade;
        }
    }
}