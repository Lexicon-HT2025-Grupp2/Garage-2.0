using Microsoft.AspNetCore.Mvc.Rendering;

namespace Garage_2._0.Models;

public class RegisteredVehicleSelectionViewModel
{
    public int? SelectedVehicleId { get; set; }
    public List<SelectListItem> RegisteredVehicles { get; set; } = new();
}