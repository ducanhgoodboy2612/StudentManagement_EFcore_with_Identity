﻿using Microsoft.EntityFrameworkCore;
using StudentManageApp_Codef.Data.Models;
using StudentManageApp_Codef.Data.R_IRepository;

namespace StudentManageApp_Codef.Data.Repository
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly AppDbContext _context;

        public ScheduleRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Schedule>> GetSchedulesByClass(int? classId, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Schedules.AsQueryable();

            if (classId.HasValue)
            {
                query = query.Where(s => s.ClassID == classId.Value);
            }

            if (startDate.HasValue && endDate.HasValue)
            {
                query = query.Where(s => s.StartTime >= startDate.Value && s.EndTime <= endDate.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<List<Schedule>> GetSchedulesByStudentId(int studentId, DateTime startDate, DateTime endDate)
        {
            return await (from enrollment in _context.Enrollments
                          join schedule in _context.Schedules
                          on enrollment.ClassID equals schedule.ClassID
                          where enrollment.StudentID == studentId
                                && schedule.StartTime >= startDate
                                && schedule.EndTime <= endDate
                          select schedule) // Lấy toàn bộ các cột từ bảng Schedule
                         .ToListAsync();
        }


    }
}
