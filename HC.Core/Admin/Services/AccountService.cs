using HC.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using HC.DataAccess.Models;
using HC.Core.Helpers;
using HC.Core.Admin.Interfaces;

namespace HC.Core.Admin.Services
{
    public class AccountService : IAccount
    {
        private readonly HealthcareAppointmentContext _context;
        private readonly JwtHelper _jwtHelper;

        public AccountService(HealthcareAppointmentContext context, IConfiguration configuration)
        {
            _context = context;
            _jwtHelper = new JwtHelper(configuration);
        }

        [HttpPost]
        public async Task<string> RegisterUserAsync(RegisterUserModel model)
        {
            // Validate inputs
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                throw new ArgumentException("All fields are required.");
            }

            // Check if the email is already registered
            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
            {
                throw new InvalidOperationException("Email is already registered.");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Password = hashedPassword
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Generate JWT token
            return _jwtHelper.GenerateToken(user.UserId.ToString(), user.Email);            
        }

        [HttpPost]
        public async Task<string> LoginAsync(LoginModel model)
        {
            // Validate inputs
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                throw new ArgumentException("Email and password are required.");
            }

            // Find user by email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
            {
                throw new InvalidOperationException("Invalid email or password.");
            }

            // Verify password
            if (!BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                throw new InvalidOperationException("Invalid email or password.");
            }

            // Generate JWT token
            return _jwtHelper.GenerateToken(user.UserId.ToString(), user.Email);
        }
    }
}
