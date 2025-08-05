using Easypay_App.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Easypay_App.Filters
{
    public class CustomExceptionFilter:ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            context.Result = new BadRequestObjectResult(
                new ErrorObjectDTO
                {
                    ErrorNumber = 500,
                    ErrorMessage = context.Exception.Message
                }
                );
        }
    }
}
