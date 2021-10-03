using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API._Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepo UserRepo { get; }
        IPostsRepo PostRepo { get; }

        Task<bool> Complete();

        bool HasChanges();
    }
}