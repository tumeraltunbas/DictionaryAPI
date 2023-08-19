using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace DictionaryAPI.WebAPI.CustomAttributes
{
    public class RegisterJwtClaimsToItemsAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if(context.HttpContext.User.Identity.IsAuthenticated)
            {
                string id = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                string email = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
                context.HttpContext.Items["Id"] = id;
                context.HttpContext.Items["Email"] = email;
            }

            base.OnActionExecuting(context);
        }

    }
}
