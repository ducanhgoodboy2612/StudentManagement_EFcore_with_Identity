using StudentManageApp_Codef.Data.Models;
using StudentManageApp_Codef.Data.R_IRepository;

namespace StudentManageApp_Codef.Service

{
    public class EnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepo;
        private readonly IConfiguration _configuration;

        public EnrollmentService(IEnrollmentRepository enrollmentRepo, IConfiguration configuration)
        {
            _enrollmentRepo = enrollmentRepo;
            _configuration = configuration;
        }

        public async Task<string> CreateEnrollment(int studentId, int classId, DateTime enrollmentDate)
        {
            var classAvailable = await _enrollmentRepo.CheckClassSlotAvailableAsync(classId);
            if (!classAvailable)
            {
                return "Class slot is full.";
            }

            var semester = GetSemester(enrollmentDate);
            var maxCredits = _configuration.GetValue<int>("AppSettings:MaxCreditsPerSemester");

            var totalCredits = await _enrollmentRepo.GetTotalCreditsForSemesterAsync(studentId, enrollmentDate.Year, semester);
            if (totalCredits >= maxCredits)
            {
                return $"Student has exceeded the maximum allowed credits ({maxCredits}) for this semester.";
            }

            var enrollment = new Enrollment
            {
                StudentID = studentId,
                ClassID = classId,
                EnrollmentDate = enrollmentDate,
                Status = 1,
                CreatedAt = DateTime.Now
            };

            await _enrollmentRepo.AddEnrollmentAsync(enrollment);
            return "Enrollment successful.";
        }

        private string GetSemester(DateTime date)
        {
            if (date.Month >= 9 && date.Month <= 1)
            {
                return "Semester1"; // Kỳ 1 từ tháng 9 đến tháng 1
            }
            return "Semester2"; // Kỳ 2 từ tháng 2 đến tháng 6
        }
    }
}
