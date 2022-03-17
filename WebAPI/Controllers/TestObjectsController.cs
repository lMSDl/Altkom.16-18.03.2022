using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "Admin")]
    public class TestObjectsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var obj1 = new TestObject();
            var obj2 = new TestObject();

            obj1.CreatedAt = DateTime.Now;
            obj1.SomeInt = 10;
            obj2.SomeInt = 1099;
            obj2.CreatedAt = DateTime.Now.Date;
            obj2.SomeBool = true;

            obj1.TestObjectParent = obj2;
            obj2.TestObjectParent = obj1;


            return Ok(obj1);// (Enumerable.Repeat(obj1, 10000));

        }
    }
}
