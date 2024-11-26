using StudentManageApp_Codef.Data.Models;
using StudentManageApp_Codef.Data.Repository;

namespace StudentManageApp_Codef.Data.R_IRepository
{
    public partial interface IGradeRepository
    {
        Task<bool> CheckQuizAndMidtermScores(int classId, int enrollmentId);
        Task AddGradeAsync(Grade grade);
        Task<decimal> GetAverageGrade(int enrollmentId);
        Task<IEnumerable<Grade>> GetGradesByStudentId(int studentId, DateTime startDate, DateTime endDate);
        Task<List<GradeSummaryDto>> GetExamResultsByClassId(int classId);
    }
}
