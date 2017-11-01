using System.Web;
using System.Web.Mvc;

namespace YUHS.WebAPI.MCare
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
