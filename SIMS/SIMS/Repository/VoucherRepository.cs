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
        public List<Voucher> vouchers;
        public VoucherRepository()
        {
            _serializer = new Serializer<Voucher>();
            vouchers = _serializer.FromCSV(_filePath);
        }

        public void Delete(Voucher voucher)
        {

            Voucher founded = vouchers.Find(v => v.Id == voucher.Id);
            vouchers.Remove(founded);
            _serializer.ToCSV(_filePath, vouchers);

        }

        public List<Voucher> GetAll()
        {
            return _serializer.FromCSV(_filePath);
        }

        public void useIt(int voucherId)
        {
            foreach (Voucher voucher in vouchers)
            {
                if (voucher.Id == voucherId)
                {
                    voucher.Status = true;
                }
            }
            _serializer.ToCSV(_filePath, vouchers);
        }

        public void DontUseIt(int voucherId)
        {
            foreach (Voucher voucher in vouchers)
            {
                if (voucher.Id == voucherId)
                {
                    voucher.Status = false;
                }
            }
            _serializer.ToCSV(_filePath, vouchers);
        }
        
        public void Save(Voucher voucher)
        {
            if (vouchers.Count == 0)
            {
                voucher.Id = 1;
            }
            else
            {
                voucher.Id = vouchers.Max(t => t.Id) + 1;
            }
            vouchers.Add(voucher);
            _serializer.ToCSV(_filePath, vouchers);
        }
    }
}
