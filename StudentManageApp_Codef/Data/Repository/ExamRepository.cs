using Microsoft.EntityFrameworkCore;
using StudentManageApp_Codef.Data.Models;
using StudentManageApp_Codef.Data.R_IRepository;

namespace StudentManageApp_Codef.Data.Repository
{
    public class ExamRepository : IExamRepository
    {
        private readonly AppDbContext _context;

        public ExamRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Exam>> GetExamsByStudentId(int studentId, DateTime startDate, DateTime endDate)
        {
            var exams = await (from exam in _context.Exams
                               join enrollment in _context.Enrollments on exam.ClassID equals enrollment.ClassID
                               where enrollment.StudentID == studentId
                                     && exam.ExamDate >= startDate
                                     && exam.ExamDate <= endDate
                               select exam)
                              .ToListAsync();

            return exams;
        }

        public async Task<Exam> GetExamByIdAsync(int examId)
        {
            return await _context.Exams
                .FirstOrDefaultAsync(e => e.ExamID == examId);
        }
    }
}
