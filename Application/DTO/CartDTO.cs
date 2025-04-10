using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Resturant.Application.DTO
{
    
    //a dto can have some sort of validation 
    public class CartDTO 
    {
        public CartDTO(List<string> items, double prices, string name)
        {
            Items = items;
            Prices = prices;
            Name = name;
        }

        //parameterless constructor is always required
        public CartDTO()
        {
            
        }
        
        [Required]
        [MinLength(2)]
        public List<string> Items { get; set;  }
        [Required]
        [Range(0.1 , double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        public double Prices { get; set; }
        [Required]
        [MinLength(2)]
        public string  Name { get; set;  }   
    }
}
