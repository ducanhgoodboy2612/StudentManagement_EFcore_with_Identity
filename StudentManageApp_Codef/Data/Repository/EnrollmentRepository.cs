using StudentManageApp_Codef.Data.R_IRepository;
using Microsoft.EntityFrameworkCore;
using StudentManageApp_Codef.Data.Models;

namespace StudentManageApp_Codef.Data.Repository
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;


        public EnrollmentRepository(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }

        public IEnumerable<Student_EnrollmentDateDto> GetStudentsByClassIdAsync(int classId, out int total)
        {
            var query = from enrollment in _context.Enrollments
                        join student in _context.Students on enrollment.StudentID equals student.StudentID
                        where enrollment.ClassID == classId
                        select new Student_EnrollmentDateDto
                        {
                            StudentID = student.StudentID,
                            FirstName = student.FirstName,
                            LastName = student.LastName,
                            Gender = student.Gender,
                            BirthDate = student.BirthDate,
                            Email = student.Email,
                            Phone = student.Phone,
                            Address = student.Address,
                            EnrollmentDate = enrollment.EnrollmentDate
                        };
            total = query.Count();

            return query.ToList();
        }


      
        public async Task<bool> CheckClassSlotAvailableAsync(int classId)
        {
            var classItem = await _context.Classes.FirstOrDefaultAsync(c => c.ClassID == classId);
            if (classItem == null) return false;

            return classItem.RemainSlots > 0; 
        }

        public async Task<int> GetTotalCreditsForSemesterAsync(int studentId, int year, string semester)
        {
            var semesterStart = new DateTime(year, _configuration.GetValue<int>($"AppSettings:{semester}StartMonth"), 1);
            var semesterEnd = new DateTime(year, _configuration.GetValue<int>($"AppSettings:{semester}EndMonth"), 30);

            // Lấy danh sách các lớp học mà sinh viên đã đăng ký
            var enrolledClasses = await _context.Enrollments
                .Where(e => e.StudentID == studentId && e.EnrollmentDate >= semesterStart && e.EnrollmentDate <= semesterEnd)
                .Select(e => e.ClassID)
                .ToListAsync();

            // Lấy tất cả các khóa học (Course) thuộc các lớp mà sinh viên đã đăng ký
            var totalCredits = await _context.Classes
                .Where(c => enrolledClasses.Contains(c.ClassID))
                .Join(_context.Courses, c => c.CourseID, cr => cr.CourseID, (c, cr) => cr.Credits)
                .SumAsync(credits => credits);

            return totalCredits; 
        }

        public async Task AddEnrollmentAsync(Enrollment enrollment)
        {
            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();
        }
    }

    public class Student_EnrollmentDateDto
    {
        public int StudentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Gender { get; set; } // 1: Male, 0: Female
        public DateTime? BirthDate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime EnrollmentDate { get; set; }
    }

    public class EnrollmentDTO
    {
        public int EnrollmentID { get; set; }
        public int StudentID { get; set; }
        public int ClassID { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public int Status { get; set; }

    }
   }
