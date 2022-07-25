using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Crudapp.Models;

namespace Crudapp.Data
{
    public class CrudappContext : DbContext
    {
        public CrudappContext (DbContextOptions<CrudappContext> options)
            : base(options)
        {
        }

        public DbSet<Crudapp.Models.StudentView> StudentView { get; set; }
    }
}
