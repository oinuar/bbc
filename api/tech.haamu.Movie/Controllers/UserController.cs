using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using tech.haamu.Movie.Services;

namespace tech.haamu.Movie.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly Users users;

        public UserController(IConfiguration configuration, Users users)
        {
            this.configuration = configuration;
            this.users = users;
        }

        [Route("[action]/{id}")]
        [HttpPost]
        public string Token(string id)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "Dummy user"),
                new Claim("userId", id)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        [Route("[action]")]
        [HttpPost]
        [User]
        public IEnumerable<string> MoviePreference([FromBody] string[] movieIds)
        {
            var user = users.GetById(this.GetUserId());

            var likes = (user.LikedMovies ?? Enumerable.Empty<Models.Movie>()).ToLookup(x => x.Id);

            return movieIds.Where(x => likes.Contains(x));
        }
    }
}