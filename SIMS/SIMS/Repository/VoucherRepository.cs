using SIMS.Domain.Model;
using SIMS.Domain.RepositoryInterface;
using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Repository
{
    class VoucherRepository : IVoucherRepository
    {
        private const string _filePath = "../../../../SIMS/Resources/Data/Voucher.csv";

        private readonly Serializer<Voucher> _serializer;

        public VoucherRepository()
        {
            _serializer = new Serializer<Voucher>();
        }

        public List<Voucher> GetByUserId(int id)
        {
            List<Voucher> vouchers = _serializer.FromCSV(_filePath);
            List<Voucher> filteredVouchers = new List<Voucher>();

            foreach (Voucher voucher in vouchers)
            {
                if (voucher.IdUser == id)
                {
                    filteredVouchers.Add(voucher);
                }

            }

            return filteredVouchers;
        }

        public void useIt(int voucherId)
        {
            List<Voucher> vouchers = _serializer.FromCSV(_filePath);
            foreach (Voucher voucher in vouchers)
            {
                if (voucher.Id == voucherId)
                {
                    voucher.Status = true;
                }
            }
            _serializer.ToCSV(_filePath, vouchers);
        }
    }
}
