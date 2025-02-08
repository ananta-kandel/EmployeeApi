using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using BCr = BCrypt.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Text; 

[Route("[Controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ApplicationDbContext applicationDbContext;
    private const string JwtSecretKey = "this_is_my_super_secure_and_custom_secret_key_for_authentication_123!";
    private const string JwtIssuer = "yourdomain.com";
    private const string JwtAudience = "yourdomain.com";

    public UserController(ApplicationDbContext dbContext)
    {
        this.applicationDbContext = dbContext;
    }

    [Authorize]
    [HttpGet]
    public IActionResult GetAllUsers()
    {
        var result = applicationDbContext.Users.ToList();
        return Ok(result);
    }

    [HttpPost]
    public IActionResult RegisterUser(AddUserDto addUserDto)
    {
        var password = BCr.BCrypt.HashPassword(addUserDto.Password);
        var user = new Users
        {
            Name = addUserDto.Name,
            Email = addUserDto.Email,
            Password = password,
            Roles = addUserDto.Roles
        };

        applicationDbContext.Users.Add(user);
        applicationDbContext.SaveChanges();
        return Ok();
    }

    [HttpPost("/login")]
    public IActionResult LoginUser(LoginUserDto loginUserDto)
    {
        var name = loginUserDto.Name;
        var password = loginUserDto.Password;

        var user = applicationDbContext.Users.FirstOrDefault(u => u.Name == name);
        if (user == null)
        {
            return Ok("No user found");
        }

        if (BCr.BCrypt.Verify(password, user.Password))
        {
            var token = GenerateJwtToken(user.Name);
            return Ok(new { token });
        }
        else
        {
            return Ok("Password incorrect");
        }
    }

    private string GenerateJwtToken(string username)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            // new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: JwtIssuer,
            audience: JwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
