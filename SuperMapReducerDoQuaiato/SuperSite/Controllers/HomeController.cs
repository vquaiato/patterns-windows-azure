using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Storage;

namespace SuperSite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var data = TempData["identificador"];
            if (data != null)
                ViewBag.Identificador = data;

            return View();
        }

        public ActionResult Novafrase(FormCollection fraseEletra)
        {
            var queue = new QueueFrases();
            var identificador = Guid.NewGuid();

            queue.NovaFraseParaProcessar(fraseEletra["frase"], fraseEletra["letra"], identificador);

            TempData["identificador"] = identificador.ToString();

            return RedirectToAction("Index");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Consultar(string identificador)
        {
            var tableResultado = new TableResultado();
            var total = tableResultado.Consultar(identificador);

            return View("consultar", total);
        }
    }
}
