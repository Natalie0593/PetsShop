using Pesiko.Domain.Abstract;
using Pesiko.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pesiko.Domain.Concrete
{
    public class EFPesikoRepository : IPesikoRepository
    {
        EFDbContext context = new EFDbContext();

        public IEnumerable<Pesik> Pesiks
        {
            get { return context.Pesiks; }
        }
    }
}
