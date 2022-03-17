using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Filters;
using WebAPI.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    //[ServiceFilter(typeof(LimitAsyncFilter))]
    public class UsersController : CrudController<User>
    {
        private IUsersService _service;
        private AuthService _authService;

        public UsersController(IUsersService service, AuthService authService) : base(service)
        {
            _service = service;
            _authService = authService;
        }

        public override async Task<IActionResult> Put(int id, [FromBody] User entity)
        {
            var user = await _service.ReadAsync(id);
            if(user?.Password == entity.Password)
            {
                ModelState.AddModelError(nameof(Models.User.Password), "Hasło nie może być takie samo jak poprzednie");
                //return BadRequest(ModelState);
            }

            return await base.Put(id, entity);
        }

        [ServiceFilter(typeof(LimitAsyncFilter))]
        [HttpGet("{id}/resetPassword")]
        public async Task<IActionResult> ResetPassword(int id)
        {
            if (await _service.ReadAsync(id) == null)
                return NotFound();

            var password = await _service.ResetPasswordAsync(id);

            return Ok(password);
        } 

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            var token = await _authService.AuthenticateAsync(user.Login, user.Password);
            if (token == null)
                return BadRequest();
            return Ok(token);
        }
    }
}
