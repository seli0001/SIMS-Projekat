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
        private IVoucherRepository _voucherRepository;
        public VoucherService()
        {
            _voucherRepository = Injector.Injector.CreateInstance<IVoucherRepository>();
        }

        public List<Voucher> GetVouchers(int id)
        {
            return _voucherRepository.GetVouchers(id);
        }
        
        public void AddVoucher(Voucher voucher)
        {
            _voucherRepository.Save(voucher);
        }

        public List<Voucher> GetAvailable(int id)
        {
           return _voucherRepository.GetAvailable(id);
        }

        public List<Voucher> GetAll(int id)
        {
            List<Voucher> vouchers = GetVouchers(id);
            return vouchers.ToList();
        }

        public void useVoucher(int voucherId)
        {
            _voucherRepository.useIt(voucherId);
        }

        public void DontUseIt(int voucherId)
        {
            _voucherRepository.DontUseIt(voucherId);
        }
    }
}
