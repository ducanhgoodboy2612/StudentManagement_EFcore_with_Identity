using StudentManageApp_Codef.Data.Models;
using StudentManageApp_Codef.Data.Repository;

namespace StudentManageApp_Codef.Data.R_IRepository
{
    public partial interface IExamRepository
    {
        Task<List<Exam>> GetExamsByStudentId(int studentId, DateTime startDate, DateTime endDate);
        Task<Exam> GetExamByIdAsync(int examId);
        Task<List<Exam>> GetExamsByClassIdAsync(int classId);
        Task<Exam> AddExamAsync(ExamDTO examDTO);
        Task<Exam> UpdateExamAsync(ExamDTO examDTO);
        Task<bool> DeleteExamAsync(int id);
    }
}
