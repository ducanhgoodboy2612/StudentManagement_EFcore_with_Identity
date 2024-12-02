﻿using Microsoft.EntityFrameworkCore;
using StudentManageApp_Codef.Data.Models;
using StudentManageApp_Codef.Data.R_IRepository;

namespace StudentManageApp_Codef.Data.Repository
{
    public class GradeRepository : IGradeRepository
    {
        private readonly AppDbContext _context;

        public GradeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckQuizAndMidtermScores(int classId, int enrollmentId)
        {
            var exams = await _context.Exams
                .Where(e => e.ClassID == classId && (e.ExamType == "Quiz" || e.ExamType == "Midterm"))
                .Select(e => e.ExamID)
                .ToListAsync();

            if (exams.Count < 2) return false; 

            var grades = await _context.Grades
                .Where(g => exams.Contains(g.ExamID) && g.EnrollmentID == enrollmentId)
                .Select(g => g.MarksObtained)
                .ToListAsync();

            if (grades.Count < 2) return false; 

            var average = grades.Average();
            return average >= 5; 
        }

        public async Task<decimal> CalculateAverageScoreAsync(int classId, int studentId)
        {
            var exams = await (from grade in _context.Grades
                               join exam in _context.Exams on grade.ExamID equals exam.ExamID
                               where exam.ClassID == classId && grade.StudentID == studentId
                               select new
                               {
                                   grade.MarksObtained,
                                   exam.TotalMarks,
                                   exam.ExamType
                               }).ToListAsync();

            if (exams == null || exams.Count == 0)
                return 0;

            decimal totalWeightedScore = exams.Sum(e =>
                (e.MarksObtained / e.TotalMarks * 10) * (e.ExamType == "Final" ? 2 : 1));
            int totalExams = exams.Count + exams.Count(e => e.ExamType == "Final");

            return totalWeightedScore / totalExams;
        }

        public async Task<Grade> CreateGradeAsync(GradeDTO gradeDTO)
        {
            var grade = new Grade
            {
                EnrollmentID = gradeDTO.EnrollmentID,
                ExamID = gradeDTO.ExamID,
                MarksObtained = gradeDTO.MarksObtained,
                StudentID = gradeDTO.StudentID,
                Note = gradeDTO.Note
            };

            await _context.Grades.AddAsync(grade);
            await _context.SaveChangesAsync();
            return grade;
        }

        public async Task<Grade> UpdateGradeAsync(GradeDTO gradeDTO)
        {
            var grade = await _context.Grades.FirstOrDefaultAsync(g => g.GradeID == gradeDTO.GradeID);
            if (grade == null) throw new KeyNotFoundException("Grade not found.");

            grade.EnrollmentID = gradeDTO.EnrollmentID;
            grade.ExamID = gradeDTO.ExamID;
            grade.MarksObtained = gradeDTO.MarksObtained;
            grade.StudentID = gradeDTO.StudentID;
            grade.Note = gradeDTO.Note;

            _context.Grades.Update(grade);
            await _context.SaveChangesAsync();
            return grade;
        }

        public async Task<bool> DeleteGradeAsync(int gradeId)
        {
            var grade = await _context.Grades.FirstOrDefaultAsync(g => g.GradeID == gradeId);
            if (grade == null) return false;

            _context.Grades.Remove(grade);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> GetAverageGrade(int enrollmentId)
        {
            var grades = await _context.Grades
                .Where(g => g.EnrollmentID == enrollmentId)
                .Select(g => g.MarksObtained)
                .ToListAsync();

            return grades.Count > 0 ? grades.Average() : 0;
        }

        public async Task<IEnumerable<Grade>> GetGradesByStudentId(int studentId, DateTime startDate, DateTime endDate)
        {
            return await (
                from grade in _context.Grades
                join enrollment in _context.Enrollments
                on grade.EnrollmentID equals enrollment.EnrollmentID
                where enrollment.StudentID == studentId &&
                      enrollment.EnrollmentDate >= startDate &&
                      enrollment.EnrollmentDate <= endDate
                select new Grade
                {
                    GradeID = grade.GradeID,
                    EnrollmentID = grade.EnrollmentID,
                    ExamID = grade.ExamID,
                    MarksObtained = grade.MarksObtained,
                    Note = grade.Note,
                    Exam = new Exam
                    {
                        ExamID = grade.Exam.ExamID,
                        ClassID = grade.Exam.ClassID,
                        ExamDate = grade.Exam.ExamDate,
                        ExamType = grade.Exam.ExamType,
                        TotalMarks = grade.Exam.TotalMarks
                    }
                }
            ).ToListAsync();
        }

        public async Task<List<GradeSummaryDto>> GetExamResultsByClassId(int classId)
        {
            return await _context.Set<Grade>()
                .Where(g => g.Exam.ClassID == classId)
                .GroupBy(g => g.StudentID)
                .Select(g => new GradeSummaryDto
                {
                    StudentID = g.Key,
                    Grades = g.Select(x => new GradeDetailsDto
                    {
                        ExamID = x.ExamID,
                        ExamType = x.Exam.ExamType,
                        ExamDate = x.Exam.ExamDate,
                        TotalMarks = x.Exam.TotalMarks,
                        MarksObtained = x.MarksObtained,
                    }).ToList(),
                    AverageMarks = g.Average(x => x.MarksObtained)
                })
                .OrderByDescending(x => x.AverageMarks)
                .ToListAsync();
        }

    }

    public class GradeDTO
    {
        public int? GradeID { get; set; } // Optional for Update
        public int EnrollmentID { get; set; }
        public int ExamID { get; set; }
        public decimal MarksObtained { get; set; }
        public int StudentID { get; set; }
        public string Note { get; set; }
    }



    public class GradeDetailsDto
    {
        public int GradeID { get; set; }
        public decimal MarksObtained { get; set; }
        public string Note { get; set; }
        public int ExamID { get; set; }
        public string ExamType { get; set; }
        public DateTime? ExamDate { get; set; }
        public decimal TotalMarks { get; set; }
    }

    public class GradeSummaryDto
    {
        public int StudentID { get; set; }
        public List<GradeDetailsDto> Grades { get; set; }
        public decimal AverageMarks { get; set; }
    }

}
