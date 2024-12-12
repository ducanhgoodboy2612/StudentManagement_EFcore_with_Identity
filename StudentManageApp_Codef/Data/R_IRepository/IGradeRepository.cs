using StudentManageApp_Codef.Data.Models;
using StudentManageApp_Codef.Data.Repository;

namespace StudentManageApp_Codef.Data.R_IRepository
{
    public partial interface IGradeRepository
    {
        Task<bool> CheckQuizAndMidtermScores(int classId, int enrollmentId);
        Task<Grade> CreateGradeAsync(GradeDTO gradeDTO);
        Task<Grade> UpdateGradeAsync(GradeDTO gradeDTO);
        Task<bool> DeleteGradeAsync(int gradeId);
        Task<decimal> GetAverageGrade(int enrollmentId);
        Task<IEnumerable<Grade>> GetGradesByStudentId(int studentId, DateTime startDate, DateTime endDate);
        Task<List<GradeSummaryDto>> GetExamResultsByClassId(int classId);
        Task<decimal> CalculateAverageScoreAsync(int classId, int studentId);
        Task<bool> DeleteGradesByStudentAndClassAsync(int studentId, int classId);
    }
}
