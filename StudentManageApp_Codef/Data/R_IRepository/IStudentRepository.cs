﻿using StudentManageApp_Codef.Data.Models;
using StudentManageApp_Codef.Data.Repository;

namespace StudentManageApp_Codef.Data.R_IRepository
{
    public partial interface IStudentRepository
    {
        Task<(IEnumerable<Student> Students, int TotalRecords)> SearchStudentsAsync(
            string? firstName, string? lastName, string? phone, int page, int pageSize);

        IEnumerable<Student> SearchStudentsAsync(string? name, string? phone, int page, int pageSize, out int total);
        Task<StudentExamInfoDto> GetStudentExamInfo(int studentId, DateTime startDate, DateTime endDate);
        Task<List<Exam>> GetExamsSchedule(int studentId, int n);
        Task<Student> CreateStudent(StudentDTO studentDTO);
        Task<Student> UpdateStudent(StudentDTO studentDTO);
        Task<bool> SoftDeleteStudentAsync(int id);
        Task<Student> GetbyId(int id);
        Task<List<Student>> GetAllStudentsAsync();
    }
}
