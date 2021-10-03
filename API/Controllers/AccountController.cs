using System;
using System.Threading.Tasks;
using API._DTOs;
using API._Entities;
using API._Interfaces;
using Microsoft.Extensions.Configuration;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Web;
using API.Helpers;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly ITokenService tokenService;
        private readonly IPhotoService photoService;
        private readonly IConfiguration config;
        private readonly IEmailSender emailSender;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManger;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManger, ITokenService tokenService,
        IPhotoService photoService, IConfiguration config, IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.signInManger = signInManger;
            this.tokenService = tokenService;
            this.photoService = photoService;
            this.config = config;
            this.emailSender = emailSender;
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

            await this.EmailAsync(user.UserName);

            var roleResult = await this.userManager.AddToRoleAsync(user, "Member");
            if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);

            return Ok();

        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            Console.WriteLine("Middle");
            var user = await this.userManager.Users
            .FirstOrDefaultAsync(x => x.UserName == loginDto.UserName.ToLower());
            if (user is null)
                return Unauthorized("User is not exisit");

            var result = await this.signInManger.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) return BadRequest("confirm your email and write the correct password");

            var userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Token = await this.tokenService.CreateToken(user),
                Photo = user.Photo
            };
            return Ok(userDto);


        }

        [HttpPost("confirmemail")]
        public async Task<ActionResult> ConfirmEmail(ConfirmEmailViewModel model)
        {

            var user = await this.userManager.FindByIdAsync(model.UserId);

            var result = await this.userManager.ConfirmEmailAsync(user, model.Token);

            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest();
        }

        private async Task<bool> UserExisit(string userName)
        {
            return await this.userManager.Users.AnyAsync(x => x.UserName == userName.ToLower());
        }


        private async Task EmailAsync(string userName)
        {
            var userFromDb = await this.userManager.FindByNameAsync(userName);
            var token = await this.userManager.GenerateEmailConfirmationTokenAsync(userFromDb);
            var uriBuilder = new UriBuilder(this.config["ReturnPaths:ConfirmEmail"]);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["token"] = token;
            query["userId"] = userFromDb.Id.ToString();
            uriBuilder.Query = query.ToString();
            var urlString = uriBuilder.ToString();
            var senderEmail = this.config["ReturnPaths:SenderEmail"];
            await this.emailSender.SendEmailAsync(senderEmail, userFromDb.Email, "Click the link below to activate your account", urlString);

        }
    }
}