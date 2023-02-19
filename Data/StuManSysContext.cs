using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentManagementSys.Model;

namespace StuManSys.Data
{
    public class StuManSysContext : IdentityDbContext
    {
        public StuManSysContext (DbContextOptions<StuManSysContext> options)
            : base(options)
        {
        }

        public DbSet<StudentManagementSys.Model.Student> Student { get; set; } = default!;

        public DbSet<StudentManagementSys.Model.Item> Item { get; set; } = default!;

        public DbSet<StudentManagementSys.Model.Staff> Staff { get; set; } = default!;
    }
}
