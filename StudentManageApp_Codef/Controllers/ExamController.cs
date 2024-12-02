using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManageApp_Codef.Data.Repository;
using StudentManageApp_Codef.Data.R_IRepository;

namespace StudentManageApp_Codef.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        private readonly IExamRepository _examRepository;

        public ExamController(IExamRepository examRepository)
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


        [HttpPost]
        public async Task<IActionResult> CreateExam([FromBody] ExamDTO examDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdExam = await _examRepository.AddExamAsync(examDTO);
            return CreatedAtAction(nameof(CreateExam), new { id = createdExam.ExamID }, createdExam);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExam(int id, [FromBody] ExamDTO examDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updatedExam = await _examRepository.UpdateExamAsync(id, examDTO);
            if (updatedExam == null) return NotFound();

            return Ok(updatedExam);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExam(int id)
        {
            var isDeleted = await _examRepository.DeleteExamAsync(id);
            if (!isDeleted) return NotFound();

            return NoContent();
        }
    }
}
