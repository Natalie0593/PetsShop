using Pesiko.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pesiko.Domain.Concrete
{
    public class EFDbContext : DbContext
    {
        public DbSet<Pesik> Pesiks { get; set; }
    }
}
