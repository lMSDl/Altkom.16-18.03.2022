using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    public class UsersController : CrudController<User>
    {
        private IUsersService _service;

        public UsersController(IUsersService service) : base(service)
        {
            _service = service;
        }

        [HttpGet("{id}/resetPassword")]
        public async Task<IActionResult> ResetPassword(int id)
        {
            if (await _service.ReadAsync(id) == null)
                return NotFound();

            var password = await _service.ResetPasswordAsync(id);

            return Ok(password);
        } 
    }
}
