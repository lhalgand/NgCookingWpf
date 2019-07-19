﻿using System;
using System.Linq;
using System.Threading.Tasks;

using apis.Models;
using apis.Data;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace apis.Controllers
{
    [Route("api/[controller]")]
    public class AuthenticateController : Controller
    {
        UserManager<User> _userManager;
        SignInManager<User> _signInManager;
        IRepository<User> _communityRepository;

        public AuthenticateController(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IRepository<User> communityRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _communityRepository = communityRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult LoggedInStatus()
        {
            if (!User.Identity.IsAuthenticated)
                return new StatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);

            String userName = _userManager.GetUserName(User);
            String userId = _userManager.GetUserId(User);

            User user = _communityRepository.Get().Where(u => u.UserName == userName).FirstOrDefault();

            if (user == null)
                return new StatusCodeResult((int)System.Net.HttpStatusCode.Unauthorized);

            return new JsonResult(GetUser(user));
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody]LogginUser user)
        {
            var result = await _signInManager.PasswordSignInAsync(user.email, user.password, true, false);

            if (result == Microsoft.AspNetCore.Identity.SignInResult.Success)
                return new StatusCodeResult(200);
            else
                return new StatusCodeResult(401);
        }

        [HttpDelete]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return new StatusCodeResult(200);
        }

        private User GetUser(User user)
        {
            return new User
            {
                Bio = user.Bio,
                BirthYear = user.BirthYear,
                City = user.City,
                FirstName = user.FirstName,
                Id = user.Id,
                LastName = user.LastName,
                Level = user.Level,
                Picture = user.Picture
            };
        }
    }
}
