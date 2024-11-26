using System.Diagnostics;

namespace StudentManageApp_Codef.Data.Models
{
    public class Exam
    {
        public int ExamID { get; set; }
        public int ClassID { get; set; }
        public DateTime? ExamDate { get; set; } 
        public string ExamType { get; set; } // Midterm, Final, Quiz
        public decimal TotalMarks { get; set; }

        public Class Class { get; set; }
            
        public ICollection<Grade> Grades { get; set; }
    }

}
