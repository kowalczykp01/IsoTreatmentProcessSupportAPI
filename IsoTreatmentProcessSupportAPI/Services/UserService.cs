using IsoTreatmentProcessSupportAPI.Entities;
using IsoTreatmentProcessSupportAPI.Exceptions;
using IsoTreatmentProcessSupportAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IsoTreatmentProcessSupportAPI.Services
{
    public interface IUserService
    {
        void RegisterUser(RegisterUserDto dto);
        void ConfirmEmail(string emailConfirmationToken);
        string GenerateEmailConfirmationToken(string userEmail);
        string GenerateLoginToken(LoginDto dto);
    }
    public class UserService : IUserService
    {
        private readonly IsoSupportDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IMailkitService _mailkitService;
        private readonly AuthenticationSettings _authenticationSettings;

        public UserService(IsoSupportDbContext dbContext, IPasswordHasher<User> passwordHasher, IMailkitService mailkitService, AuthenticationSettings authenticationSettings)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _mailkitService = mailkitService;
            _authenticationSettings = authenticationSettings;
        }

        public void RegisterUser(RegisterUserDto dto)
        {
            var newUser = new User()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Weight = dto.Weight,
                ClimaxDoseInMiligramsPerKilogramOfBodyWeight = dto.ClimaxDoseInMiligramsPerKilogramOfBodyWeight,
                DailyDose = dto.DailyDose,
                MedicationStartDate = dto.MedicationStartDate
            };
            var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);
            
            newUser.PasswordHash = hashedPassword;
            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();

            var emailConfirmationToken = GenerateEmailConfirmationToken(dto.Email);

            _mailkitService.Send(newUser.Email, emailConfirmationToken);
        }

        public void ConfirmEmail(string emailConfirmationToken)
        {
            var userEmail = DecodeUserEmailFromToken(emailConfirmationToken);

            var user = _dbContext
                .Users
                .FirstOrDefault(e => e.Email == userEmail);

            user.EmailConfirmed = true;
            _dbContext.SaveChanges();
        }

        public string GenerateEmailConfirmationToken(string userEmail)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, $"{userEmail}")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        public string GenerateLoginToken(LoginDto dto)
        {
            var user = _dbContext.Users
                .FirstOrDefault(u => u.Email == dto.Email);

            if (user is null)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Email, $"{user.Email}")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddHours(1);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        public string DecodeUserEmailFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            if (securityToken != null)
            {
                var userEmailClaim = securityToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email);

                if (userEmailClaim != null)
                {
                    return userEmailClaim.Value;
                }
            }
            return null;
        }
    }
}
