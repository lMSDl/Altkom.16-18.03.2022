using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Authorize(Policy = "Age")]
    //[Authorize(Policy = "Admin")]
    public class ProductsController : CrudController<Product>
    {
        public ProductsController(ICrudService<Product> service) : base(service)
        {
        }
    }
}
