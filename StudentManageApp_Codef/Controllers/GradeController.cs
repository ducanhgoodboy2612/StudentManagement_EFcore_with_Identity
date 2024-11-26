using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManageApp_Codef.Data.R_IRepository;
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
    }
}
