using SIMS.Domain.RepositoryInterface;
using SIMS.Repository;
using SIMS.WPF.ViewModel;
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
            TourRequestRepository tourRequestRepository = new TourRequestRepository();
            ComplexTourRepository complexTourRepository = new ComplexTourRepository();
            CommentRepository commentRepository = new CommentRepository();
            ForumRepository forumRepository = new ForumRepository();
            RenovationRepository renovationRepository = new RenovationRepository();

            _implementations.Add(typeof(IGuestRatingRepository), guestRatingRepository);
            _implementations.Add(typeof(IVoucherRepository), voucherRepository);
            _implementations.Add(typeof(ITourRatingRepository), tourRatingRepository);
            _implementations.Add(typeof(ITourRequestRepository), tourRequestRepository);
            _implementations.Add(typeof(IMessageBoxService), new MessageBoxService());
            _implementations.Add(typeof(IComplexTourRepository), complexTourRepository);
            _implementations.Add(typeof(IForumRepository), forumRepository);
            _implementations.Add(typeof(ICommentRepository), commentRepository);
            _implementations.Add(typeof(IRenovationRepository), renovationRepository);
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
