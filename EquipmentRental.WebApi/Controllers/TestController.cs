using Microsoft.AspNetCore.Mvc;
using Persistence.Repositories;

namespace EquipmentRental.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController
{
    private readonly IEquipmentRepository _equipmentRepository;

    public TestController(IEquipmentRepository equipmentRepository)
    {
        _equipmentRepository = equipmentRepository;
    }

    [HttpGet]
    public IActionResult GetEquipmentTypes()
    {
        var types = _equipmentRepository.GetEquipmentTypes();
        return new OkObjectResult(types);
    }
}