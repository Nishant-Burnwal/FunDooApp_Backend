using DatabaseLayer.Data;
using DatabaseLayer.Exceptions;
using DatabaseLayer.Interface;
using ModelLayer.DTOs;
using ModelLayer.Entity;

namespace DatabaseLayer.Repository
{
    public class UserRepository : IUserRepository
    {/*
      * This class does ONLY ONE JOB:
       Talk to the database for User-related operations
      */
        private readonly FunDooContext _context;

        public UserRepository(FunDooContext context)
        {
            _context = context;
        }

        public User RegisterUser(RegisterUserDTO registerUserDTO)
        {
            // Check if user already exists
            var existingUser = _context.Users
                .FirstOrDefault(u => u.Email == registerUserDTO.Email);

            if (existingUser != null)
            {
                throw new UserAlreadyExistsException("User already registered with this email");
            }

            var user = new User
            {
                FirstName = registerUserDTO.FirstName,
                LastName = registerUserDTO.LastName,
                Email = registerUserDTO.Email,
                Password = registerUserDTO.Password, // hashing later
                CreatedAt = DateTime.Now,
                ChangedAt = DateTime.Now
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }
        public bool SaveResetToken(string email, string token, DateTime expiry)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) return false;

            user.ResetToken = token;
            user.ResetTokenExpiry = expiry;
            user.ChangedAt = DateTime.Now;

            _context.SaveChanges();
            return true;
        }

        public bool UpdatePassword(User user, string newPassword)
        {
            user.Password = newPassword;
            user.ResetToken = null;
            user.ResetTokenExpiry = null;
            user.ChangedAt = DateTime.Now;

            _context.SaveChanges();
            return true;
        }
        public User GetUserByResetToken(string token)
        {
            return _context.Users.FirstOrDefault(u => u.ResetToken == token);
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

    }
}
