using Microsoft.AspNetCore.Mvc.ModelBinding;

public static class ModelStateExtensions
{

    public static BadRequestObjectResult As<BadRequestObjectResult>(this ModelStateDictionary modelState)
    {
        var message = string.Join("\r\n", modelState.Values
                                           .SelectMany(v => v.Errors)
                                           .Select(e => e.ErrorMessage));
        return (BadRequestObjectResult)Activator.CreateInstance(typeof(BadRequestObjectResult), message)!;
    }
}