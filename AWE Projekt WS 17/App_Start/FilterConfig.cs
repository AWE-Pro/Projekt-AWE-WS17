using System.Web;
using System.Web.Mvc;

namespace AWE_Projekt_WS_17
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
