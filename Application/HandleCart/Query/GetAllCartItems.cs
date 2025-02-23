using MediatR;
using Resturant.Application.DTO;
using Resturant.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resturant.Application.HandleCart.Query
{
    public class GetAllCartItems : IRequest<List<CartDTO>> {}
}
