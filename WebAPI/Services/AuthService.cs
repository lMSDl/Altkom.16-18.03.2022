﻿using Microsoft.IdentityModel.Tokens;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Services
{
    public class AuthService
    {
        private static readonly string _secret = Guid.NewGuid().ToString();
        public static byte[] Key = Encoding.Default.GetBytes(_secret);

        private IUsersService _service;
        public AuthService(IUsersService service)
        {
            _service = service;
        }

        public async Task<string> AuthenticateAsync(string login, string password)
        {
            var user = await _service.ReadByLoginAsync(login);
            if (user?.Password != password)
                return null;

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.Login));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.DateOfBirth, user.BirthDate.ToShortDateString()));
            claims.AddRange(user.Roles.ToString().Split(", ").Select(x => new Claim(ClaimTypes.Role, x)));

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor();
            tokenDescriptor.Subject = new ClaimsIdentity(claims);
            tokenDescriptor.Expires = DateTime.Now.AddMinutes(5);
            tokenDescriptor.SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Key), SecurityAlgorithms.HmacSha256Signature);

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
