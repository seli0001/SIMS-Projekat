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
        List<Voucher> GetAll();
        void useIt(int voucherId);
        void DontUseIt(int voucherId);
        void Delete(Voucher voucher);
        void Save(Voucher voucher);
        public List<Voucher> GetVouchers(int id);
        public List<Voucher> GetAvailable(int id);

    }
}
