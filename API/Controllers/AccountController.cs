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
using API._Helpers;

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
        private readonly IUnitOfWork unitOfWork;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManger, ITokenService tokenService,
        IPhotoService photoService, IConfiguration config, IEmailSender emailSender, IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
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
            if (await UserExisit(registerDto.UserName, registerDto.Email)) return BadRequest("user name/email is exisit, please try another name/email");
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
            if (!result.Succeeded) return BadRequest(result.Errors);

            await this.sendEmailActivationLink(user.UserName);

            var roleResult = await this.userManager.AddToRoleAsync(user, "Member");
            if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);

            return Ok();

        }



        [HttpPost("confirmemail")]
        public async Task<ActionResult> ConfirmEmail(ConfirmEmailViewModel model)
        {

            var user = await this.userManager.FindByIdAsync(model.UserId);
            if(user.EmailConfirmed)return Ok();

            var result = await this.userManager.ConfirmEmailAsync(user, model.Token);

            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
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



        private async Task<bool> UserExisit(string userName, string email)
        {
            return await this.userManager.Users.AnyAsync(x => x.UserName == userName.ToLower() || x.Email == email);
        }

        [HttpPut("manage-password/{userEmail}")]
        public async Task<ActionResult> ManagePassword(string userEmail)
        {
            var user = await this.userManager.FindByEmailAsync(userEmail);
            if (user is null) return BadRequest("No such user with " + userEmail + "email");
            sendPasswordResetLinkToEmail(user);
            return NoContent();
        }

        [HttpPut("reset-password")]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await this.userManager.FindByIdAsync(model.UserId);
            if(!user.EmailConfirmed){
                user.EmailConfirmed = true;
                await this.unitOfWork.Complete();
            }

            var result = await this.userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (result.Succeeded)
            {
            return Ok();
            }
            return BadRequest(user);
        }

        private async void sendPasswordResetLinkToEmail(AppUser user)
        {
            var token = await this.userManager.GeneratePasswordResetTokenAsync(user);
            var uriBuilder = new UriBuilder(this.config["ReturnPaths:ResetPassword"]);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["token"] = token;
            query["userId"] = user.Id.ToString();
            uriBuilder.Query = query.ToString();
            var urlString = uriBuilder.ToString();
            var senderEmail = this.config["ReturnPaths:SenderEmail"];
            await this.emailSender.SendEmailAsync(senderEmail, user.Email, "Click the link below to reset your password", urlString);

        }

        private async Task sendEmailActivationLink(string userName)
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