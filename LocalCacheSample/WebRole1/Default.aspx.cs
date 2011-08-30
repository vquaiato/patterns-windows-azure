using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.ApplicationServer.Caching;

namespace WebRole1
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void colocarNoCache_Click(object sender, EventArgs e)
        {
            try
            {
                var itens = new List<Tuple<string, string, string>>();

                using (DataCacheFactory dataCacheFactory = new DataCacheFactory())
                {
                    DataCache dataCache = dataCacheFactory.GetDefaultCache();

                    for (int i = 0; i < 300; i++)
                    {
                        itens.Add(new Tuple<string, string, string>("item " + i + 1, "item " + i + 1, "item " + i + 1));
                    }

                    dataCache.Put("itens", itens);
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message + " <br> " + " <br> " + ex.InnerException.Message + ex.StackTrace);
            }
        }

        protected void buscarDoCache_Click(object sender, EventArgs e)
        {
            try
            {
                var itens = new List<Tuple<string, string, string>>();

                using (DataCacheFactory dataCacheFactory = new DataCacheFactory())
                {
                    DataCache dataCache = dataCacheFactory.GetDefaultCache();

                    itens = dataCache.Get("itens") as List<Tuple<string, string, string>>;
                }

                this.GridView1.DataSource = itens;
                this.GridView1.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message + " <br> " + " <br> " + ex.InnerException.Message + ex.StackTrace);
            }
        }
    }
}
