using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace ProductManagment.Api.Helpers
{
    public static class ResultExtensions
    {
        public static void AddErrorToModelState(this Result result, ModelStateDictionary modelState)
        {
            if (result.Success)
            {
                return;
            }

            foreach (var error in result.Errors)
            {
                modelState.AddModelError(error.PropertyName, error.Message);
            }
        }

        public static IActionResult Process<T>(this Result<T> result, ModelStateDictionary modelState, string actionName)
        {
            if (!result.Success)
            {
                result.AddErrorToModelState(modelState);
                return new BadRequestObjectResult(modelState);
            }

            return new CreatedResult(actionName, result.Value);
        }

        public static IActionResult Process<T>(this Result<T> result, ModelStateDictionary modelState)
        {
            if (!result.Success)
            {
                result.AddErrorToModelState(modelState);
                return new BadRequestObjectResult(modelState);
            }

            return new OkObjectResult(result.Value);
        }
    }
}
