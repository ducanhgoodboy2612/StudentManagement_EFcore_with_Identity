using StudentManageApp_Codef.Data.Models;

namespace StudentManageApp_Codef.Data.R_IRepository
{
    public interface IUserRepository
    {
        User Login(string email, string password);
    }
}
