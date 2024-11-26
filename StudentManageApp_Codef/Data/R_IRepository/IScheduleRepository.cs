using StudentManageApp_Codef.Data.Models;

namespace StudentManageApp_Codef.Data.R_IRepository
{
    public partial interface IScheduleRepository
    {
        Task<List<Schedule>> GetSchedulesByClass(int? classId, DateTime? startDate, DateTime? endDate);
        Task<List<Schedule>> GetSchedulesByStudentId(int studentId, DateTime startDate, DateTime endDate);  
    }
}
