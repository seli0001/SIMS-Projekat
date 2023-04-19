using SIMS.Domain.RepositoryInterface;
using SIMS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Injector
{
    public class Injector
    {
        private static Dictionary<Type, object> _implementations = new Dictionary<Type, object>
        {
        };

        public static void BindComponents()
        {
            VoucherRepository voucherRepository = new VoucherRepository();
            TourRatingRepository tourRatingRepository = new TourRatingRepository();
            GuestRatingRepository guestRatingRepository = new GuestRatingRepository();
            _implementations.Add(typeof(IGuestRatingRepository), guestRatingRepository);
            _implementations.Add(typeof(IVoucherRepository), voucherRepository);
            _implementations.Add(typeof(ITourRatingRepository), tourRatingRepository);
        }

        public static T CreateInstance<T>()
        {
            Type type = typeof(T);

            if (_implementations.ContainsKey(type))
            {
                return (T)_implementations[type];
            }

            throw new ArgumentException($"No implementation found for type {type}");
        }

    }
}
