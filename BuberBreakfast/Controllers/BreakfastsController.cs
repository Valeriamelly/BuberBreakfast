using BuberBreakfast.Contracts.Breakfast;
using BuberBreakfast.Models;
using BuberBreakfast.ServiceErrors;
using BuberBreakfast.Services.Breakfasts;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace BuberBreakfast.Controllers;

public class BreakfastController : ApiController
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
        ErrorOr<Breakfast> getBreakfastResult = _breakfastService.GetBreakfast(id);

        return getBreakfastResult.Match(
            breakfast => Ok(MapBreakfastResponse(breakfast)), // acción que se ejecutará si la operación fue exitosa.
            errors => Problem(errors)); // acción que se ejecutará si ocurrió un error durante la operación

        //if (getBreakfastResult.IsError &&
        //   getBreakfastResult.FirstError == Errors.Breakfast.NotFound)
        //{
        //    return NotFound();
        //}
        //var breakfast = getBreakfastResult.Value; //Si no se produjo un error y se encontró el desayuno, se obtiene el valor del desayuno 

        //BreakfastResponse response = MapBreakfastResponse(breakfast);
        //return Ok(response);
    }

    private static BreakfastResponse MapBreakfastResponse(Breakfast breakfast)
    {
        return new BreakfastResponse(
                    breakfast.Id,
                    breakfast.Name,
                    breakfast.Description,
                    breakfast.StartDateTime,
                    breakfast.EndDateTime,
                    breakfast.LastModifiedDateTime,
                    breakfast.Savory,
                    breakfast.Sweet
                );
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