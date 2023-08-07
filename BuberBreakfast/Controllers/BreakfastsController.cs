using BuberBreakfast.Contracts.Breakfast;
using BuberBreakfast.Models;
using BuberBreakfast.Services.Breakfasts;
using Microsoft.AspNetCore.Mvc;

namespace BuberBreakfast.Controllers;

[ApiController] //definir que esta clase es un controlador de API
[Route("breakfasts")] // las rutas para acceder a los métodos del controlador estarán basadas en el nombre del controlador
public class BreakfastController : ControllerBase
{   // su valor no se podrá modificar después de la inicialización.
    private readonly IBreakfastService _breakfastService;
    //inyección de dependencia
    public BreakfastController(IBreakfastService breakfastService)
    {
        _breakfastService=breakfastService;
    }

    //REST 
    [HttpPost]
    public IActionResult CreateBreakfast(CreateBreakfastRequest request)
    {
        var breakfast = new Breakfast(
            Guid.NewGuid(),
            request.Name,
            request.Description,
            request.StartDateTime,
            request.EndDateTime,
            DateTime.UtcNow,
            request.Savory,
            request.Sweet);

        _breakfastService.CreateBreakfast(breakfast);

        //guardar en BD
        var response = new BreakfastResponse(
            breakfast.Id,
            breakfast.Name,
            breakfast.Description,
            breakfast.StartDateTime,
            breakfast.EndDateTime,
            breakfast.LastModifiedDateTime,
            breakfast.Savory,
            breakfast.Sweet
        );        
        //devolver un resultado HTTP 201 (Created) junto con la URL del nuevo desayuno creado.
        return CreatedAtAction(
            actionName: nameof(GetBreakfast),
            routeValues: new { id = breakfast.Id },
            value: response
        );

    }

    [HttpGet("{id:guid}")]
    public IActionResult GetBreakfast(Guid id)
    {        
        Breakfast breakfast = _breakfastService.GetBreakfast(id);
        
        var response = new BreakfastResponse (
            breakfast.Id,
            breakfast.Name,
            breakfast.Description,
            breakfast.StartDateTime,
            breakfast.EndDateTime,
            breakfast.LastModifiedDateTime,
            breakfast.Savory,
            breakfast.Sweet
        );
        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpsertBreakfast(Guid id, UpsertBreakfastRequest request)
    {
        var breakfast = new Breakfast(
            id, // Se coloca el id que viene de la ruta 
            request.Name, 
            request.Description,
            request.StartDateTime,
            request.EndDateTime,
            DateTime.UtcNow,
            request.Savory,
            request.Sweet

        );

        _breakfastService.UpsertBreakfast(breakfast);

        return NoContent(); // en caso de que ya exista un id

    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteBreakfast(Guid id)
    {
        _breakfastService.DeleteBreakfast(id);
        return NoContent();
    }



}