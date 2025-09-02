using Microsoft.AspNetCore.Mvc;
using Resturant.Domain.Entity;
using Resturant.Infrastructure.Context;

namespace API.Controllers;


[ApiController]
public class MiscController : Controller
{
    
    private readonly RestaurantContext _context;
    
    public MiscController(RestaurantContext context)
    {
        _context = context;
    }
    
    [HttpPost("BookingInformation")]
    public async Task<IActionResult> BookingInformation(BookingInformation bookingInformation)
    {
        await _context.BookingInformation.AddAsync(bookingInformation);
        await _context.SaveChangesAsync();
        return Ok(bookingInformation);
    }
    
        
    [HttpPost("CustomerInquiry")]
    public async Task<IActionResult> PostContactAsync(CustomerInquiryInformation contact)
    {
        await _context.CustomerInquiryInformation.AddAsync(contact);
        await _context.SaveChangesAsync();
        return Ok(contact);
    }
    
    
    [HttpGet("CreateGuideForFrontend")]
    public IActionResult MakeGuid()
    {
        var guid = Guid.NewGuid();
        return Ok(guid);
    }

    
}