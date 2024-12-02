using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StudentManageApp_Codef.Data.R_IRepository;
using StudentManageApp_Codef.Data.Repository;
using StudentManageApp_Codef.Service;

namespace StudentManageApp_Codef.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentRepository _enrollmentRepo;
        private readonly EnrollmentService _service;


        public EnrollmentController(IEnrollmentRepository enrollmentRepo, EnrollmentService service)
        {
            _enrollmentRepo = enrollmentRepo;
            _service = service;
        }

        [HttpGet("GetStudentsByClass/{classId}")]
        public IActionResult GetStudentsByClassId(int classId)
        {
            try
            {
                int totalRecords;
                var students =  _enrollmentRepo.GetStudentsByClassIdAsync(classId, out totalRecords);
                if (students == null || !students.Any())
                {
                    return NotFound(new { message = "No students found for the given class ID." });
                }

                //return Ok(students);
                return Ok(new { TotalRecords = totalRecords, Students = students });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateEnrollment(EnrollmentDTO enrollmentDTO)
        {
            var enrollment = await _service.CreateEnrollment(enrollmentDTO);
            return Ok(enrollment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnrollment(int id)
        {
            var isDeleted = await _enrollmentRepo.SoftDeleteEnrollmentAsync(id);
            if (!isDeleted) return NotFound();

            return NoContent();
        }

        [HttpGet("paged/{studentId}")]
        public async Task<IActionResult> GetPagedEnrollments(int studentId, int page = 1, int pageSize = 10)
        {
            var (enrollments, total) = await _enrollmentRepo.GetPagedEnrollmentsByStudentIdAsync(studentId, page, pageSize);
            return Ok(new { Total = total, Data = enrollments });
        }
    }
}
