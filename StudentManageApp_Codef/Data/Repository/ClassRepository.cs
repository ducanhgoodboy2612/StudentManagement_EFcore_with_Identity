using Microsoft.EntityFrameworkCore;
using StudentManageApp_Codef.Data.Models;
using StudentManageApp_Codef.Data.R_IRepository;

namespace StudentManageApp_Codef.Data.Repository
{
    public class ClassRepository : IClassRepository
    {
        private readonly AppDbContext _context;

        public ClassRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Class>> SearchClasses(string courseName, string semester, int? year)
        {
            var query = _context.Classes
                .Include(c => c.Course) // Include course
                .AsQueryable();

            if (!string.IsNullOrEmpty(courseName))
            {
                query = query.Where(c => c.Course.CourseName.Contains(courseName));
            }

            if (!string.IsNullOrEmpty(semester))
            {
                query = query.Where(c => c.Semester.Contains(semester));
            }

            if (year.HasValue)
            {
                query = query.Where(c => c.Year == year.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<Class> GetClassById(int classId)
        {
            var res = await _context.Classes
                //.Include(c => c.Course)
                //.Include(c => c.Lecturer)
                .FirstOrDefaultAsync(c => c.ClassID == classId);
            return res;
        }

        public async Task<ClassDto> AddClass(ClassDto classDto)
        {
            var classEntity = new Class
            {
                ClassName = classDto.ClassName,
                CourseID = classDto.CourseID,
                LecturerID = classDto.LecturerID,
                Semester = classDto.Semester,
                Year = classDto.Year
            };

            _context.Classes.Add(classEntity);
            await _context.SaveChangesAsync();

            classDto.ClassID = classEntity.ClassID; 
            return classDto;
        }

        public async Task<ClassDto> UpdateClass(ClassDto classDto)
        {
            var classEntity = await _context.Classes.FindAsync(classDto.ClassID);
            if (classEntity == null)
            {
                return null;
            }

            classEntity.ClassName = classDto.ClassName;
            classEntity.CourseID = classDto.CourseID;
            classEntity.LecturerID = classDto.LecturerID;
            classEntity.Semester = classDto.Semester;
            classEntity.Year = classDto.Year;

            await _context.SaveChangesAsync();

            return classDto;
        }

        public async Task<bool> DeleteClass(int id)
        {
            var classEntity = await _context.Classes
                .Include(c => c.Enrollments)
                .FirstOrDefaultAsync(c => c.ClassID == id);

            if (classEntity == null)
            {
                return false;
            }

            _context.Enrollments.RemoveRange(classEntity.Enrollments);
            _context.Classes.Remove(classEntity);

            await _context.SaveChangesAsync();
            return true;
        }
    }

    public class ClassDto
    {
        public int ClassID { get; set; } 
        public string ClassName { get; set; }
        public int CourseID { get; set; }
        public int LecturerID { get; set; }
        public string Semester { get; set; }
        public int Year { get; set; }
    }

}
