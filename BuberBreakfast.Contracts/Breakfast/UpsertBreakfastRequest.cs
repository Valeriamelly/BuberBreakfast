/*
Los registros son similares a las clases, pero
están diseñados específicamente para almacenar datos y no tienen comportamientos definidos (métodos) como las clases.
*/
namespace BuberBreakfast.Contracts.Breakfast;

public record UpsertBreakfastRequest(
    string Name,
    string Description,
    DateTime StartDateTime,
    DateTime EndDateTime,
    List<string> Savory,
    List<string> Sweet
);
