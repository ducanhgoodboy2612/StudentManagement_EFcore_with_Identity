using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManageApp_Codef.Data;
using StudentManageApp_Codef.Data.Models;
using StudentManageApp_Codef.Data.R_IRepository;
using StudentManageApp_Codef.Data.Repository;
namespace StudentManageApp_Codef.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly AppDbContext _context;

        private readonly IStudentRepository _studentRepository;

        public StudentController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchStudents(string? firstName, string? lastName, string? phone, int page = 1, int pageSize = 10)
        {
            var result = await _studentRepository.SearchStudentsAsync(firstName, lastName, phone, page, pageSize);

            return Ok(new
            {
                TotalRecords = result.TotalRecords,
                Students = result.Students
            });
        }


        [HttpGet("all")]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _context.Students
                .Include(s => s.Enrollments)   // Nếu bạn muốn lấy luôn các quan hệ liên quan
                .Include(s => s.TuitionFees)
                .Include(s => s.Attendances)
                .Include(s => s.Grades)
                .ToListAsync();

            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById(int studentId)
        {
            

            try
            {
                var student = await _studentRepository.GetbyId(studentId);
                return Ok(student);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Sinh viên không tồn tại.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] StudentDTO studentDTO)
        {
            if (studentDTO == null)
            {
                return BadRequest("Dữ liệu sinh viên không hợp lệ.");
            }

            var newStudent = await _studentRepository.CreateStudent(studentDTO);
            return Ok(newStudent);
        }

        [HttpPut("{studentId}")]
        public async Task<IActionResult> UpdateStudent(int studentId, [FromBody] StudentDTO studentDTO)
        {
            if (studentId != studentDTO.StudentID)
            {
                return BadRequest("ID sinh viên không khớp.");
            }

            try
            {
                var updatedStudent = await _studentRepository.UpdateStudent(studentDTO);
                return Ok(updatedStudent);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Sinh viên không tồn tại.");
            }
        }

        [HttpGet("student-exam-info")]
        public async Task<IActionResult> GetStudentExamInfo(int studentId, DateTime startDate, DateTime endDate)
        {
            var result = await _studentRepository.GetStudentExamInfo(studentId, startDate, endDate);

            if (result == null)
            {
                return NotFound($"No student found with ID {studentId}");
            }

            return Ok(result);
        }

        [HttpGet("GetExamsSchedule")]
        public async Task<IActionResult> GetExamsSchedule(int studentId, int n)
        {
            var exams = await _studentRepository.GetExamsSchedule(studentId, n);

            if (exams == null || exams.Count == 0)
            {
                return NotFound("Không có bài kiểm tra nào.");
            }

            return Ok(exams);
        }
    }
}
