﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resturant.Application.DTO
{
    public record CartDTO (Guid Id, string items, int? prices, string name)
    {
    }
}
