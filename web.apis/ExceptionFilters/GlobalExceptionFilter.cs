using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using web.apis.Models;

public class GlobalExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var response = new ResponseModel("An error occurred", false, null);
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        if (context.ModelState?.IsValid == false)
        {
            var errors = context.ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            response.Status = "Some field(s) are not valid";
            response.Data = errors;
        }
        else
        {
            response.Status = context.Exception.Message;
        }

        context.Result = new JsonResult(response);
        context.ExceptionHandled = true;
    }
}
