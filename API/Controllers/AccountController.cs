using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API._Data;
using API._DTOs;
using API._Entities;
using API._Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext context;
        private readonly ITokenService tokenService;
        private readonly IPhotoService photoService;

        public AccountController(DataContext context, ITokenService tokenService,
        IPhotoService photoService)
        {
            this.context = context;
            this.tokenService = tokenService;
            this.photoService = photoService;
        }


        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register([FromForm]RegisterDto registerDto)
        {
            if (await UserExisit(registerDto.UserName)) return BadRequest("user-name is exisit, please try another name");
            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = registerDto.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key

            };

            if (registerDto.Photo != null)
            {
                var uploadPhotoResult = await this.photoService.AddCloudPhotoAsync(registerDto.Photo);
                if(uploadPhotoResult.Error != null)return BadRequest("Photo cannot be uploaded");

                user.Photo = uploadPhotoResult.Url.AbsoluteUri;
                user.PhotoPublicId = uploadPhotoResult.PublicId;

            }




            await this.context.Users.AddAsync(user);
            if (await this.context.SaveChangesAsync() > 0)
            {
                var userDto = new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Token = this.tokenService.CreateToken(user),
                    Photo = user.Photo
                };

                return Ok(userDto);
            }

            return BadRequest("Problem while registering");

        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await this.context.Users.Where(x => x.UserName == loginDto.UserName.ToLower()).FirstOrDefaultAsync();
            if (user is null) return Unauthorized("User is not exisit");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var userHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            if (userHash.SequenceEqual(user.PasswordHash))
            {
                return new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Token = this.tokenService.CreateToken(user),
                    Photo = user.Photo
                };
            }
            return Unauthorized("Invalid Passowrd");

        }

        private async Task<bool> UserExisit(string userName)
        {
            return await this.context.Users.AnyAsync(x => x.UserName == userName.ToLower());
        }
    }
}