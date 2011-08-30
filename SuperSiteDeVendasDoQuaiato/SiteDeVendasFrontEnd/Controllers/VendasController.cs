using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using RepositorioDeVendas;

namespace SiteDeVendasFrontEnd.Controllers
{
    public class VendasController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Vender(FormCollection valores)
        {
            var produto = valores["produto"];
            var quantidade = Int16.Parse(valores["quantidade"]);
            var valor = Decimal.Parse(valores["valor"]);
            //formas de pagamento diversas

            var paraProcessar = new RepositorioDePagamentosParaProcessar();

            for(int i=0;i<quantidade;i++)
                paraProcessar.NovaVendaParaProcessar(new Venda(produto, 1, valor));

            ViewBag.Venda = string.Format("Venda de {0} {1} realizada. Aguarde Confirmação de pagamento.",quantidade,produto);
            return RedirectToAction("Index");
        }
    }
}
