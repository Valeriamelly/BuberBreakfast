using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BuberBreakfast.Controllers;

[ApiController] //definir que esta clase es un controlador de API
[Route("breakfasts")] // las rutas para acceder a los métodos del controlador estarán basadas en el nombre del controlador
public class ApiController : ControllerBase
{
    protected IActionResult Problem(List<Error> errors)
    {
        if (errors.All(e => e.Type == ErrorType.Validation))
        {   
            //agregan todos los errores de validación que ocurrieron
            var modelStateDictionary = new ModelStateDictionary();
            
            foreach (var error in errors)
            {
                modelStateDictionary.AddModelError(error.Code, error.Description);
            }
            //dvolver las solicitudes incorrectas con todos los detalles
            return ValidationProblem(modelStateDictionary);
        }

        if (errors.Any (e => e.Type == ErrorType.Unexpected))
        {
            return Problem();
        }

        //de lo contrario se toma el primer error y se devuelve la respuesta adecuada
        var firstError = errors[0];
        
        var statusCode = firstError.Type switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _=> StatusCodes.Status500InternalServerError
        };
        
        return Problem(statusCode: statusCode, title: firstError.Description);
    }
}