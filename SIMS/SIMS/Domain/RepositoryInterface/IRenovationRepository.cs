using SIMS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Domain.RepositoryInterface
{
    internal interface IRenovationRepository
    {
        List<Renovation> GetAll();
        List<Renovation> GetByOwnerId(int id);
        Renovation Save(Renovation renovation);
        Renovation Update(Renovation renovation);
        void Delete(Renovation renovation);
    }
}
