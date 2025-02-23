using System.ComponentModel.DataAnnotations;

namespace Resturant.Domain.Entity
{
    public class NameVO
    {


        [RegularExpression("^([a-zA-Z\\s]*$)")]
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
