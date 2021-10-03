using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API._Interfaces;
using AutoMapper;

namespace API._Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext context;
        private readonly IMapper mapper;

        public UnitOfWork(DataContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IUserRepo UserRepo => new UserRepo(this.context, this.mapper);

        public IPostsRepo PostRepo => new PostsRepo(this.context, this.mapper);


        public async Task<bool> Complete()
        {
            return (await this.context.SaveChangesAsync() > 0);
        }

        public bool HasChanges()
        {
            return this.context.ChangeTracker.HasChanges();
        }
    }
}