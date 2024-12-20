using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HrManager.Rest;

[Route("hr_manager")]
[ApiController]
public class EmployeeController(ILogger<EmployeeController> logger, HrManagerService service) : ControllerBase
{
    [Route("wishlist")]
    [HttpPost]
    public IActionResult ReceiveWishList([FromBody] EmployeeInfo? employeeInfo)
    {
        if (employeeInfo == null)
        {
            logger.LogWarning("Received null employee information.");
            return BadRequest("Employee information cannot be null.");
        }

        logger.LogInformation($"Received wish list for employee ID: {employeeInfo.WishList.EmployeeId}");

        service.Handle(employeeInfo);

        return Ok(new { status = "Employee information received successfully", employeeId = employeeInfo.Employee.Id });
    }
}