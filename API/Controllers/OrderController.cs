﻿
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using API.DTO;
using API.Repository;
using Resturant.Domain.Entity;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using Resturant.Infrastructure.Context;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        //private readonly IMediator _mediator;

        private readonly  ToDoContext _context;

        private readonly IRepository _temporaryCartRepository; 
          
        public OrderController(ToDoContext context,  IRepository temporaryCartRepository)
        {
            //   _mediator = mediatR ?? throw  new ArgumentNullException(nameof(mediatR));
            _context = context; 
           _temporaryCartRepository = temporaryCartRepository;
        
        }
        
        [HttpGet("TempItemsTable")]
        public async Task<IActionResult> TempCartItems()
        {

            var tempCartItems = await _temporaryCartRepository.ReturnListItemsAsync();
            return tempCartItems.Count == 0 ? NotFound() : Ok(tempCartItems);
        }

        [HttpGet("GetTotalPrice")]
        public async Task<IActionResult> GetTempsItemsTableByGuid(string guid = "3fa85f64-5717-4562-b3fc-2c963f66afa")
        {
            var tempItemPrice = await _temporaryCartRepository.ReturnCartItemsByGuidAsync(guid);
            if (tempItemPrice == null)
            {
                return NotFound();
            }
            return Ok(tempItemPrice);
        }
        
         [HttpGet("GetAllTempItems")]

          public async Task<ActionResult<IEnumerable<MenuDTO>>> TemporaryCartItems()
         {
             var menuDTO = await _temporaryCartRepository.ReturnMenuDtoListAsync(); 
              if(menuDTO.Any()) {
                 return Ok(menuDTO);
              }
              return NotFound("no value was found");
         }


         [HttpGet("GetMenuItemByGuid")]
         public async Task<ActionResult<IEnumerable<MenuDTO>>> TemporaryCartItemByGuid(
             [FromQuery] string guidId = "3fa85f64-5717-4562-b3fc-2c963f66afa6")
         {
        
             var guidCheck= Guid.TryParse(guidId, out var guidValue);
     
             if (guidCheck)
             {
                 var menuDto = await _context.TemporaryCartItems.Include("MenuItems")
                     .Where(x => x.Indentity.ToString() == guidValue.ToString())
                     .SelectMany(x => x.MenuItems)
                     .Select(x => new MenuDTO()
                         { Id = x.Id, Name = x.Name, Price = x.Price, GuidId = x.TemporaryCartItemsIndentity.ToString() })
                     .ToListAsync();

                 if (menuDto.Any())
                 {
                     return menuDto;
                 }
             }
             else
             {
                 return Ok(0);
             }
             //even if nothing is found still return zero
             int zero = 0; 
             return Ok(zero); 
         }
         
         
         [HttpGet("GetMenuItemByGuidSize")]
         public async Task<ActionResult<int>> TemporaryCartItemByGuidSize(
             [FromQuery] string guidId = "3fa85f64-5717-4562-b3fc-2c963f66afa6")
         {
        
             var guidCheck  = Guid.TryParse(guidId, out var guidValue);
     
             if (guidCheck)
             {
                 var menuDto = await _context.TemporaryCartItems.Include("MenuItems")
                     .Where(x => x.Indentity.ToString() == guidValue.ToString())
                     .SelectMany(x => x.MenuItems)
                     .Select(x => new MenuDTO()
                         { Id = x.Id, Name = x.Name, Price = x.Price, GuidId = x.TemporaryCartItemsIndentity.ToString() })
                     .ToListAsync();

                 if (menuDto.Any())
                 {
                     int menusize = menuDto.Count;
                     return Ok(menusize);
                 }
             }
             else
             {
                 return Ok(null);
             }
             
             return Ok(null); 
         }
         
         [HttpGet("CreateGuide")]
         public IActionResult MakeGuid()
         {
             var guid = Guid.NewGuid();
             return Ok(guid);
         }
         
         [ProducesResponseType(200)]
         [HttpDelete("DeleteMenuItem")]
         public async Task<ActionResult<MenuItemsVO>> RemoveMenuItem(int id, Guid guidId)
         {
          //  var menuItem =  await _context.MenuItems.FindAsync(id);
            MenuItemsVO? menuItem = await _temporaryCartRepository.FindByPrimaryKey(id); 
            
            if (menuItem != null)
            {
                _context.MenuItems.Remove(menuItem);
            }
            await _temporaryCartRepository.SaveCartItemsAsync();
   
            var results = await UpdateTotalPrice(guidId);
            return results;
            
         }

         private async Task<ActionResult<MenuItemsVO>> UpdateTotalPrice(Guid guidId)
         {

             var tempCartItem = await _temporaryCartRepository.ReturnCartItemsByGuidAsync(guidId.ToString());
             var totalPriceMenuItems = 
                 _context.TemporaryCartItems.Include("MenuItems").
                     Where(x => x.Indentity == guidId).
                     SelectMany(x=>x.MenuItems).
                     Sum(x => x.Price);
             
             tempCartItem.TotalPrice = totalPriceMenuItems;

             await _context.SaveChangesAsync();
             return Ok();
         }


         [HttpPost("TemporaryCartItems")]
         
         public async Task<ActionResult<TempDto>> AddTempItems(TempDto dto)
         {
             var checkNewMenuItem =
                 await _context.TemporaryCartItems.SingleOrDefaultAsync(x => x.Indentity == dto.GuidId);
             
             //if paid exists means you have already paid
             var paid = 
                 await _context.OrderInformation.SingleOrDefaultAsync(x => x.TempCartsIdentity == dto.GuidId);
             
             //create a new menu item
           if (checkNewMenuItem is null && paid is null )
           {
       
               string name = dto.Name!;
               decimal price = CheckItemPrices(name);
              
               TemporaryCartItems temporaryCartItems = new TemporaryCartItems();
               temporaryCartItems.Indentity = dto.GuidId;
               temporaryCartItems.Created = DateTime.UtcNow;
               temporaryCartItems.TotalPrice = price;

               temporaryCartItems.MenuItems.Add(new MenuItemsVO() { Name = name, Price = price });
               await _context.AddAsync(temporaryCartItems);
               await _context.SaveChangesAsync();
             
              // return CreatedAtAction("TemporaryCartItemByGuid", new {dto.GuidId}, temporaryCartItems);
              return Ok(new { Name = name, Price = price });
      
           }
    
           else if (checkNewMenuItem != null && paid is null)
           {
               
               string name = dto.Name!;
               decimal price = CheckItemPrices(name);
               
               var menuItems = new MenuItemsVO() { Name = name, Price = price , TemporaryCartItemsIndentity = dto.GuidId };
               _context.MenuItems.Add(menuItems);
              await _context.SaveChangesAsync();
            
             //update totalprice-AFTER the menu Item has been saved to get the most up to date price
               var totalPriceMenuItems = 
                   _context.TemporaryCartItems.Include("MenuItems").
                       Where(x => x.Indentity == dto.GuidId).
                       SelectMany(x=>x.MenuItems).
                       Sum(x => x.Price);

               checkNewMenuItem.TotalPrice = totalPriceMenuItems;
               await _context.SaveChangesAsync();
         
             //  return CreatedAtAction("TemporaryCartItemByGuid", new {dto.Name}, new {Name = name, Price = price});
             return Ok(new { Name = name, Price = price });
           }
           else
           {
               return BadRequest("This user has already paid!!!");
           }

         }
         
         struct ItemPrices
         {
             public const decimal TofuStirFry = (decimal) 10.5;
             public const decimal EggRollPlatter = (decimal) 14.95;
             public const decimal PapayaSalad = (decimal) 8.95;
             public const decimal CaesarSalad = (decimal) 8.95;
             public const decimal ChoppedBeef = (decimal) 12.95;
             public const decimal VeggiePlatter = (decimal) 8.95;
         }

         static decimal CheckItemPrices(string itemName)
         {
             switch (itemName)
             {
                 case "Egg Roll Platter":
                     return ItemPrices.EggRollPlatter;
                 case "Papaya Salad":
                     return ItemPrices.PapayaSalad;
                 case "Tofu":
                     return ItemPrices.TofuStirFry;
                 case "Caesar Salad":
                     return ItemPrices.CaesarSalad;
                 case "Chopped Beef":    
                     return ItemPrices.ChoppedBeef;
                 case "Veggie Platter":
                      return ItemPrices.VeggiePlatter;  
                 default: throw new InvalidItemException("That is not a valid menu item- " +  itemName);
             }
         }
    
        
    }
}
