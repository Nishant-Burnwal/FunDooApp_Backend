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
    }
}
