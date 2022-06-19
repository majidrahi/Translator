using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;
using System.Web;

namespace Translator.MVC.Helpers.Search
{
    /// <summary>
    /// A helper class to Get Search filter key value from querystring
    /// </summary>
    public class SearchFilter
    {
        public static string? GetSearchIndex(Controller controller)
        {
            if (controller.HttpContext.Request.Query["filter"].ToString() != null)
            {
                NameValueCollection filterQueryString = HttpUtility.ParseQueryString(controller.HttpContext.Request.Query["filter"].ToString());

                if (filterQueryString["qIndex"] != null)
                {
                    string? searchIndex =filterQueryString["qIndex"];
                    controller.ViewBag.searchIndex = searchIndex;
                    return searchIndex;
                }
            }

            return null;
        }

        public static string? GetSearchQuery(Controller controller)
        {
            if (controller.HttpContext.Request.Query["filter"].ToString() != null)
            {
                NameValueCollection filterQueryString = HttpUtility.ParseQueryString(controller.HttpContext.Request.Query["filter"].ToString());

                if (filterQueryString["q"] != null)
                {
                    string searchQuery = filterQueryString["q"]!;
                    controller.ViewBag.searchQuery = searchQuery;
                    return searchQuery;
                }
            }

            return null;
        }

        public static string? GetCustomSearchQuery(Controller controller,string queryName)
        {
            if (controller.HttpContext.Request.Query["filter"].ToString() != null)
            {
                NameValueCollection filterQueryString = HttpUtility.ParseQueryString(controller.HttpContext.Request.Query["filter"].ToString());

                if (filterQueryString[queryName] != null)
                {
                    string searchQuery = filterQueryString[queryName]!;
                    controller.ViewData[queryName] = searchQuery;
                    return searchQuery;
                }
            }

            return null;
        }
    }
}