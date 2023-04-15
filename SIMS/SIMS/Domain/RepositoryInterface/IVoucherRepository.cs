using SIMS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Domain.RepositoryInterface
{
    internal interface IVoucherRepository
    {
        List<Voucher> GetByUserId(int id);
        void useIt(int voucherId);
    }
}
