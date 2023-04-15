using SIMS.Domain.Model;
using SIMS.Domain.RepositoryInterface;
using SIMS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Service.Services
{
    class VoucherService
    {
        private  IVoucherRepository _voucherRepository;
        public VoucherService() { 
        _voucherRepository = Injector.Injector.CreateInstance<IVoucherRepository>();
        }


        public List<Voucher> GetAvailable(int userId)
        {
            return _voucherRepository
                .GetByUserId(userId)
                .Where(v => !v.Status && v.ValiUntl > DateTime.Now)
                .ToList();
        }

        public List<Voucher> GetByUserId(int userId)
        {
            return _voucherRepository
                .GetByUserId(userId)
                .ToList();
        }


        public void useVoucher(int voucherId)
        {
            _voucherRepository.useIt(voucherId);
        }
    }
}
