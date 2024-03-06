using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NZWalks.Api.CustomValidations
{
    public class CustomValidationsAttribute:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);    
            if(context.ModelState.IsValid)
            {
                context.Result = new BadRequestResult();
            }
        }
    }
}
