﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using IsatiWei.Api.Models;
using IsatiWei.Api.Models.Authentication;
using IsatiWei.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IsatiWei.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationService _authenticationService;
        private readonly IMapper _mapper;

        public AuthenticationController(AuthenticationService authenticationService, IMapper mapper)
        {
            _authenticationService = authenticationService;
            _mapper = mapper;
        }

        /// <summary>
        /// Register a user
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /register
        ///     {
        ///         "firstName": "Victor",
        ///         "lastName": "DENIS",
        ///         "email": "admin@feldrise.com",
        ///         "username": "Feldrise",
        ///         "password": "MySecurePassword"
        ///     }   
        /// </remarks>
        /// <param name="registerModel"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public IActionResult Register([FromBody]UserRegister registerModel)
        {
            var user = _mapper.Map<User>(registerModel);

            try
            {
                _authenticationService.Register(user, registerModel.Password);
            } 
            catch (Exception e)
            {
                return BadRequest($"Can't regster the user: {e.Message}");
            }

            return Ok();
        }

        /// <summary>
        /// Login a user
        /// </summary>
        /// <remarks>
        /// The username can be both the email or the actual username.
        /// </remarks>
        /// <param name="loginModel"></param>
        /// <returns>The user with all informations</returns>
        [HttpPost("login")]
        public ActionResult<User> Login([FromBody]UserLogin loginModel)
        {
            var user = _authenticationService.Login(loginModel.Username, loginModel.Password);

            if (user == null)
            {
                return BadRequest("Username or password is incorrect");
            }

            return Ok(user);
        }

        /*
         * Put
         */

        /// <summary>
        /// Update the password for current user
        /// </summary>
        /// <param name="passwordUpdateModel"></param>
        /// <param name="authorization"></param>
        /// <returns></returns>
        [HttpPut("update/password")]
        public async Task<ActionResult<PasswordUpdate>> UpdatePassword([FromBody] PasswordUpdate passwordUpdateModel, [FromHeader] string authorization)
        {
            try
            {
                passwordUpdateModel = await _authenticationService.UpdatePassword(UserUtilities.UserIdFromAuth(authorization), passwordUpdateModel.OldPassword, passwordUpdateModel.NewPassword);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(passwordUpdateModel);
        }

    }
}
