using System.Security.Claims;
using Backend.Dtos;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Authorize]
[Route("api/purchase-bills")]
public class PurchaseBillsController(PurchaseBillService bills) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(PurchaseBillRequest request)
    {
        var user = User.FindFirstValue(ClaimTypes.Email) ?? "";
        var bill = await bills.CreateAsync(request, user);
        return Ok(new { id = bill.Id, message = "Purchase bill saved successfully." });
    }
}
