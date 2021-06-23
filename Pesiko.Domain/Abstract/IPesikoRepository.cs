using Pesiko.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pesiko.Domain.Abstract
{
    public interface IPesikoRepository
    {
        IEnumerable<Pesik> Pesiks { get; }
        void SavePesik(Pesik pesik);
        Pesik DeletePesik(int pesikId);
    }
}
