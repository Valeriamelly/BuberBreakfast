using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace BuberBreakfast.Controllers;
[ApiController] //definir que esta clase es un controlador de API
[Route("breakfasts")] // las rutas para acceder a los métodos del controlador estarán basadas en el nombre del controlador
public class ApiController : ControllerBase
{
    protected IActionResult Problem(List<Error> errors)
    {
        var FirstError = errors[0];
        var statusCode = FirstError.Type switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _=> StatusCodes.Status500InternalServerError
        };
        
        return Problem(statusCode: statusCode, title: FirstError.Description);
    }
}