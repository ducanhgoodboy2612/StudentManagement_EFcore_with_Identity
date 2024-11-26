using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StudentManageApp_Codef.Data.R_IRepository;
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
        [Route("create")]
        public async Task<IActionResult> CreateEnrollment(int studentId, int classId, DateTime enrollmentDate)
        {
            var result = await _service.CreateEnrollment(studentId, classId, enrollmentDate);
            if (result == "Enrollment successful.")
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
