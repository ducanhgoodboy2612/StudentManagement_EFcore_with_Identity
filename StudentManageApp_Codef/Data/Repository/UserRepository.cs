using StudentManageApp_Codef.Data.Models;
using StudentManageApp_Codef.Data.R_IRepository;
using System.ComponentModel.DataAnnotations;

namespace StudentManageApp_Codef.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public User Login(string email, string password)
        {
            var user = _context.Users
                               .SingleOrDefault(u => u.Email == email && u.Password == password);

            return user;
        }
    }

    public class AuthenticateModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class AppSettings
    {
        public string Secret { get; set; }

    }
}
