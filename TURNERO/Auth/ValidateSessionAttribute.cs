using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TURNERO.Auth
{
    public class ValidateSessionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Session.GetString("user") == null)
            {
                context.Result = new RedirectResult("~/Access/Login");
            }
            base.OnActionExecuting(context);
        }
    }
}
