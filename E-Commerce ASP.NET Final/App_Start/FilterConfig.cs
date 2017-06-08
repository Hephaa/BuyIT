using System.Web;
using System.Web.Mvc;

namespace E_Commerce_ASP.NET_Final
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
