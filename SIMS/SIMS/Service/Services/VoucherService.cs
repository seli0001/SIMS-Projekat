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
            List<Voucher> vouchers = _voucherRepository.GetAll();
            List<Voucher> filteredVouchers = new List<Voucher>();

            foreach (Voucher voucher in vouchers)
            {
                if (voucher.IdUser == id && voucher.ValiUntl > DateTime.Now)
                {
                    filteredVouchers.Add(voucher);
                }
                else if (voucher.ValiUntl < DateTime.Now)
                {
                    _voucherRepository.Delete(voucher);
                }

            }
            return filteredVouchers;
        }

        public List<Voucher> GetAvailable(int id)
        {
            List<Voucher> vouchers = GetVouchers(id);
            return vouchers.Where(v => v.Status == false).ToList();
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
