using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSys.Controllers.Dto;
using StudentManagementSys.Model;
using StuManSys.Data;
using System.Runtime.CompilerServices;

namespace StuManSys.Services
{
    public class StudentServices
    {
        private readonly StuManSysContext _context;

        public StudentServices(StuManSysContext context)
        {
            _context = context;
        }

        //AutoMapper Configuration
        private MapperConfiguration config = new MapperConfiguration(cfg =>
                    cfg.CreateMap<Student, StudentDto>()
                    .ForMember(des => des.SubjectEnlisted, act => act.MapFrom(scr => mapStringToList(scr.SubjectEnlisted)))
        );

        private MapperConfiguration configReversed = new MapperConfiguration(cfg =>
                    cfg.CreateMap<StudentDto, Student>()
                    .ForMember(des => des.SubjectEnlisted, act => act.MapFrom(scr => String.Join(',', scr.SubjectEnlisted.Where(s => !string.IsNullOrEmpty(s)))))
        );

        private static List<String> mapStringToList(String? a)
        {
            return String.IsNullOrEmpty(a) ? new List<String>() : a.Split(",").ToList();
        }

        //Methods
        public async Task<Boolean> RegisterStudentAsync(StudentDto stDto) {

            _context.Add(new Mapper(configReversed).Map<Student>(stDto));
            await _context.SaveChangesAsync();
            return _context.SaveChangesAsync().IsCompletedSuccessfully;
        }

        public async Task<StudentDto> GetStudent(String id)
        {
            var mapper = new Mapper(config);

            if (id == null || _context.Student == null)
            {
                return null;
            }
            Student student = await _context.Student
                .FirstOrDefaultAsync(m => m.UID == id);
            if (student == null)
            {
                return null;
            }
            var rs = mapper.Map<StudentDto>(student);
            return rs;
        }

        public async Task<StudentDto> UpdateStudent(string id, StudentDto stuDto)
        {
            if (id != stuDto.UID)
            {
                return null;
            }
            var Student = new Mapper(configReversed).Map<Student>(stuDto);
            try
            {
                _context.ChangeTracker.Clear();
                _context.Update(Student);
                await _context.SaveChangesAsync();
                return stuDto;
            }
            catch (DbUpdateConcurrencyException)
            {
                return null;
                throw;
            }
        }

    }
}
