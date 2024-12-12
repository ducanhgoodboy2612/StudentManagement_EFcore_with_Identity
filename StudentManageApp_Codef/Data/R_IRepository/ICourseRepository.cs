using StudentManageApp_Codef.Data.Models;

namespace StudentManageApp_Codef.Data.R_IRepository
{
    public partial interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllCoursesAsync();
    }
}
