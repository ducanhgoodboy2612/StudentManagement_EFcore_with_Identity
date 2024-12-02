using StudentManageApp_Codef.Data.Models;
using StudentManageApp_Codef.Data.Repository;

namespace StudentManageApp_Codef.Data.R_IRepository
{
    public interface ILecturerRepository
    {
        Task<Lecturer> GetByIdAsync(int id);
        Task<Lecturer> AddLecturerAsync(LecturerDTO lecturerDTO);
        Task<Lecturer> UpdateLecturerAsync(int id, LecturerDTO lecturerDTO);
        Task<bool> DeleteLecturerAsync(int id);
    }
}
