using System.ComponentModel.DataAnnotations;

namespace Resturant.Domain.Entity
{
    public class NameVO
    {


        [RegularExpression("^([a-zA-Z\\s]*$)")]
        [Required(ErrorMessage = "Name is required")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters long")]
        public string? FirstName { get; set;  }

        public NameVO()
        {
            
        }

        public NameVO(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("the value cannot be null");
            }
            if (name.Length > 10)
            {
                throw new ArgumentException($"the length of {nameof(name)} cannot be grater then 10");
            }
            FirstName = name;
        }
    }

}
