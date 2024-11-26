using StudentManageApp_Codef.Data.Models;

namespace StudentManageApp_Codef.Data.R_IRepository
{
    public partial interface IExamRepository
    {
        Task<List<Exam>> GetExamsByStudentId(int studentId, DateTime startDate, DateTime endDate);
        Task<Exam> GetExamByIdAsync(int examId);
    }
}
