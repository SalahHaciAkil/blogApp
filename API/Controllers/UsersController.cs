using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API._Data;
using API._DTOs;
using API._Entities;
using API._Extensions;
using API._Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class UsersController : BaseApiController
    {

        private readonly IMapper autoMapper;
        private readonly IUnitOfWork unitOfWork;

        public UsersController(IUnitOfWork unitOfWork, IMapper autoMapper)
        {
            this.unitOfWork = unitOfWork;
            this.autoMapper = autoMapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await this.unitOfWork.UserRepo.GetUsersAsync();
            return Ok(this.autoMapper.Map<MemberDto[]>(users));
        }



        [HttpGet("{userName}")]

        public async Task<ActionResult<MemberDto>> GetUser(string userName)
        {
            var user = await this.unitOfWork.UserRepo.GetUserAsync(userName);
            if (user is null) { return NotFound("user is not exisit"); }
            return Ok(this.autoMapper.Map<MemberDto>(user));
        }



    }
}