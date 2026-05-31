using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SMT.API.Common;

public class ApiResponseFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Result is ObjectResult objectResult)
        {
            var value = objectResult.Value;

            if (value is ApiResponse<object>)
                return;

            if (context.HttpContext.Request.Method == "GET" && value == null)
            {
                objectResult.Value = ApiResponse<object>.Fail("NotFound", "Ресурс не найден");
                objectResult.StatusCode = 404;
                return;
            }

            if (objectResult.StatusCode >= 200 && objectResult.StatusCode < 300)
            {
                objectResult.Value = ApiResponse<object>.Ok(value);
            }
        }
    }
}