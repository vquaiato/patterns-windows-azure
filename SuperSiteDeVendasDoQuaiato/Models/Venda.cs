using System;

namespace Models
{
    public class Venda
    {
        public Guid Identificador { get; set; }
        public string Produto { get; set; }
        public int Quantidade { get; set; }
        public decimal Valor { get; set; }

        public Venda(string produto, int quantidade, decimal valor)
        {
            this.Produto = produto;
            this.Quantidade = quantidade;
            this.Valor = valor;
            this.Identificador = Guid.NewGuid();
        }

        public override string ToString()
        {
            return string.Format("{0} - {1} - {2} - {3}", this.Produto, this.Quantidade, this.Valor, this.Identificador);
        }
    }
}