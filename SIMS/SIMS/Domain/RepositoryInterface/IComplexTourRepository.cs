using SIMS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Domain.RepositoryInterface
{
    public interface IComplexTourRepository

    {
        public void add(string name,List<TourRequest> tours,int userId);
        public List<ComplexTourRequest> GetByUser(int userId);
        public void ChangeStatus(int userId);
    }
}
