using System.Web;
using System.Web.Mvc;

namespace Puntland_Port_Taxation
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}