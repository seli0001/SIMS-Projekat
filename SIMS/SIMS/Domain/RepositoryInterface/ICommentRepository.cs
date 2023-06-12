using SIMS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Domain.RepositoryInterface
{
    internal interface ICommentRepository
    {
        List<Comment> GetAll();
        Comment Update(Comment comment);
    }
}
