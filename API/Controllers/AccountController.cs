using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API._Data;
using API._DTOs;
using API._Entities;
using API._Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly ITokenService tokenService;
        private readonly IPhotoService photoService;
        private readonly IPostsRepo postsRepo;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManger;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManger, ITokenService tokenService,
        IPhotoService photoService, IPostsRepo postsRepo)
        {
            this.userManager = userManager;
            this.signInManger = signInManger;
            this.tokenService = tokenService;
            this.photoService = photoService;
            this.postsRepo = postsRepo;
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteUsersPhoto()
        {
            var users = await this.userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                if (user.PhotoPublicId != null)
                {
                    var deletionResult = await this.photoService.DeletePhotoAsync(user.PhotoPublicId);
                    if (deletionResult.Error != null) return BadRequest();


                }
            }
            return Ok();
        }


        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromForm] RegisterDto registerDto)
        {
            if (await UserExisit(registerDto.UserName)) return BadRequest("user-name is exisit, please try another name");
            var user = new AppUser
            {
                UserName = registerDto.UserName.ToLower(),
                Email = registerDto.Email,
                KnownAs = registerDto.KnownAs

            };

            if (registerDto.Photo != null)
            {
                var uploadPhotoResult = await this.photoService.AddCloudPhotoAsync(registerDto.Photo);
                if (uploadPhotoResult.Error != null) return BadRequest("Photo cannot be uploaded");

                user.Photo = uploadPhotoResult.Url.AbsoluteUri;
                user.PhotoPublicId = uploadPhotoResult.PublicId;

            }

            var result = await this.userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest("failed to create new user");
            var roleResult = await this.userManager.AddToRoleAsync(user, "Member");
            if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);

            var userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Token = await this.tokenService.CreateToken(user),
                Photo = user.Photo
            };

            return Ok(userDto);

        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await this.userManager.Users
            .FirstOrDefaultAsync(x => x.UserName == loginDto.UserName.ToLower());
            if (user is null)
                return Unauthorized("User is not exisit");

            var result = await this.signInManger.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) return BadRequest("Password is not correct");

            var userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Token = await this.tokenService.CreateToken(user),
                Photo = user.Photo
            };
            return Ok(userDto);


        }

        private async Task<bool> UserExisit(string userName)
        {
            return await this.userManager.Users.AnyAsync(x => x.UserName == userName.ToLower());
        }
    }
}