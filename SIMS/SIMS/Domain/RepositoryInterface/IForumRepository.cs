using SIMS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Domain.RepositoryInterface
{
    internal interface IForumRepository
    {
        List<Forum> GetAll();
        Forum Save(Forum forum);
        void AddComment(Comment comment, Forum forumm);
        public void CloseForum(Forum forumm);

    }
}
