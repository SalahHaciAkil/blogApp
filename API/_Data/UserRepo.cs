using System.Collections.Generic;
using System.Threading.Tasks;
using API._DTOs;
using API._Entities;
using API._Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API._Data
{
    public class UserRepo : IUserRepo
    {
        private readonly DataContext context;
        private readonly IMapper autoMapper;

        public UserRepo(DataContext context, IMapper autoMapper)
        {
            this.autoMapper = autoMapper;
            this.context = context;
        }

        public async Task AddUserPostLike(UserPostLikes userPostLikes)
        {
            await this.context.UsersPostLikes.AddAsync(userPostLikes);


        }

        public async Task<AppUser> GetUserAsync(string userName)
        {
            return await this.context.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            var users = await this.context.Users.Include(x => x.Posts)
            .ToListAsync();
            return users;
        }

        // public async Task<bool> SaveChangesAsync()
        // {
        //     if (await this.context.SaveChangesAsync() > 0) return true;
        //     return false;
        // }
    }
}