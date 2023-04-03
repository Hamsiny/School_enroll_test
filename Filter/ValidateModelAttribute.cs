using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace UxtrataTask.Filter;

public class ValidateModelAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        if (!filterContext.ModelState.IsValid)
        {
            filterContext.Result = new ViewResult
            {
                ViewName = filterContext.RouteData.Values["action"].ToString(),
                ViewData = (filterContext.Controller as Controller)?.ViewData, // Change type to ViewDataDictionary
                TempData = (filterContext.Controller as Controller)?.TempData
            };
        }
    }
}