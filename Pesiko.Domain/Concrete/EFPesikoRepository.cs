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

        public void SavePesik(Pesik pesik)
        {
            if (pesik.PesikId == 0)
                context.Pesiks.Add(pesik);
            else
            {
                Pesik dbEntry = context.Pesiks.Find(pesik.PesikId);
                if (dbEntry != null)
                {
                    dbEntry.Name = pesik.Name;
                    dbEntry.Description = pesik.Description;
                    dbEntry.Price = pesik.Price;
                    dbEntry.Category = pesik.Category;
                    dbEntry.ImageData = pesik.ImageData;
                    dbEntry.ImageMimeType = pesik.ImageMimeType;
                }
            }
            context.SaveChanges();
        }

        public Pesik DeletePesik(int pesikId)
        {
            Pesik dbEntry = context.Pesiks.Find(pesikId);
            if (dbEntry != null)
            {
                context.Pesiks.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
