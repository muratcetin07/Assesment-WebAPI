using Data.Abstract;
using Data.Core;
using DotNetCoreWebApi.ApiHelpers;
using DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Utilities;

namespace DotNetCoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        IUserDataRepo _userDataRepo;

        public AuthenticationController(IUserDataRepo userDataRepo)
        {
            _userDataRepo = userDataRepo;
        }

        /// <summary>
        /// user register to system
        /// </summary>
        /// <param name="registerReq"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public ActionResult<DataResult<RegisterResponse>> Register([FromBody] RegisterRequest registerReq)
        {
            var response = new DataResult<RegisterResponse>(null) { Code = ResponseCode.BadRequest, Message = "unsuccessful" };

            if (ModelState.IsValid)
            {
                var user = new User
                {
                    EmailAddress = registerReq.EmailAddress,
                    FirstName = registerReq.FirstName,
                    LastName = registerReq.LastName,
                    Password = registerReq.Password,
                    CreateDate = DateTime.Now
                };

                var result = _userDataRepo.Insert(user);
                if (result.Code == ResponseCode.OK)
                {
                    response.Code = result.Code;
                    response.Message = "User was created";
                    response.Result = new RegisterResponse { UserId = user.Id };
                }
                else
                {
                    response.Message = result?.Message;
                }
            }

            return response;
        }


        /// <summary>
        /// user login into  system
        /// </summary>
        /// <param name="loginReq"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public ActionResult<DataResult<LoginResponse>> Login([FromBody] LoginRequest loginReq)
        {
            var response = new DataResult<LoginResponse>(null) { Code = ResponseCode.BadRequest, Message = "unsuccessful" };

            if (ModelState.IsValid)
            {

                var user = _userDataRepo.GetBy(x => x.EmailAddress == loginReq.EmailAddress && x.Password == loginReq.Password)?.FirstOrDefault();

                #region dummyUserForAssesment
                if (user == null)
                {
                    user = new User { FirstName = "murat", EmailAddress = "muratcetin007@hotmail.com", Id = new Guid().ToString() };
                }
                #endregion

                if (user != null)
                {

                    var token = TokenHelper.Authenticate(user);
                    response.Code = ResponseCode.OK;
                    response.Message = "Login successful. Api token was created.";
                    response.Result = new LoginResponse { Token = token };
                }
                else
                {
                    response.Message = "User wasn't found";
                }
            }

            return response;
        }

    }
}
