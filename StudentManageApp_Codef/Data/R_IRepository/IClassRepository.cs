using StudentManageApp_Codef.Data.Models;
using StudentManageApp_Codef.Data.Repository;

namespace StudentManageApp_Codef.Data.R_IRepository
{
    public partial interface IClassRepository
    {
        Task<IEnumerable<Class>> SearchClasses(string courseName, string semester, int? year);
        Task<Class> GetClassById(int classId);
        Task<ClassDto> AddClass(ClassDto classDto);
        Task<ClassDto> UpdateClass(ClassDto classDto);
        Task<bool> DeleteClass(int id);
    }
}
