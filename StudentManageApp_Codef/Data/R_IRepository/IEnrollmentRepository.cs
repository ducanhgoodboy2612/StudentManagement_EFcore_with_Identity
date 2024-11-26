using StudentManageApp_Codef.Data.Models;
using StudentManageApp_Codef.Data.Repository;

namespace StudentManageApp_Codef.Data.R_IRepository
{
    public partial interface IEnrollmentRepository
    {
        //Task<List<Student_EnrollmentDateDto>> GetStudentsByClassIdAsync(int classId);

        public IEnumerable<Student_EnrollmentDateDto> GetStudentsByClassIdAsync(int classId, out int total);

        Task<bool> CheckClassSlotAvailableAsync(int classId);
        Task<int> GetTotalCreditsForSemesterAsync(int studentId, int year, string semester);
        Task AddEnrollmentAsync(Enrollment enrollment);

    }
}
