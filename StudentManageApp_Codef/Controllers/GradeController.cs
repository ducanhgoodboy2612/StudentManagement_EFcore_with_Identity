using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManageApp_Codef.Data.R_IRepository;
using StudentManageApp_Codef.Data.Repository;
using StudentManageApp_Codef.Service;

namespace StudentManageApp_Codef.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradeController : ControllerBase
    {
        private readonly IGradeRepository _repo;
        private readonly GradeService _service;


        public GradeController(IGradeRepository enrollmentRepo, GradeService service)
        {
            _repo = enrollmentRepo;
            _service = service;
        }

        [HttpGet("GetGrades")]
        public async Task<IActionResult> GetGradesByStudent([FromQuery] int studentId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var result = await _service.GetGradesAndAverages(studentId, startDate, endDate);
            return Ok(result);
        }

        [HttpGet("GetClassExamResults")]
        public async Task<IActionResult> GetClassExamResults(int classId)
        {
            try
            {
                var results = await _repo.GetExamResultsByClassId(classId);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateGrade(GradeDTO gradeDTO)
        {
            var grade = await _repo.CreateGradeAsync(gradeDTO);
            return CreatedAtAction(nameof(CreateGrade), new { id = grade.GradeID }, grade);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateGrade(GradeDTO gradeDTO)
        {
            var grade = await _repo.UpdateGradeAsync(gradeDTO);
            return Ok(grade);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGrade(int id)
        {
            var isDeleted = await _repo.DeleteGradeAsync(id);
            if (!isDeleted) return NotFound();

            return NoContent();
        }

        [HttpGet("average-score")]
        public async Task<IActionResult> GetAverageScore(int classId, int studentId)
        {
            var averageScore = await _repo.CalculateAverageScoreAsync(classId, studentId);

            return Ok(new
            {
                ClassId = classId,
                StudentId = studentId,
                AverageScore = averageScore
            });
        }
    }
}
