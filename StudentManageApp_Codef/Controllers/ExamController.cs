using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManageApp_Codef.Data.Repository;

namespace StudentManageApp_Codef.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        private readonly ExamRepository _examRepository;

        public ExamController(ExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        [HttpGet("GetExamsByStudentId")]
        public async Task<IActionResult> GetExamsByStudentId(int studentId, DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest("Start date cannot be later than end date.");
            }

            var exams = await _examRepository.GetExamsByStudentId(studentId, startDate, endDate);
            return Ok(exams);
        }
    }
}
