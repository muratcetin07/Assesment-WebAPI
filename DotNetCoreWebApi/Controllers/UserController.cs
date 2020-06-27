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
    public class UserController : ControllerBase
    {
        IUserSubscriptionDataRepo _userSubscriptionDataRepo;

        public UserController(IUserSubscriptionDataRepo userSubscriptionDataRepo)
        {
            _userSubscriptionDataRepo = userSubscriptionDataRepo;
        }

        /// <summary>
        /// Only A registered user can purchase a subscription to any book available in the product catalogue by access token.
        /// </summary>
        /// <param name="subscribeReq"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("Subscribe")]
        public ActionResult<DataResult<SubscribeResponse>> Subscribe([FromBody] SubscribeRequest subscribeReq)
        {
            var response = new DataResult<SubscribeResponse>(null) { Code = ResponseCode.BadRequest, Message = "unsuccessful" };

            if (ModelState.IsValid)
            {

                System.Security.Claims.ClaimsPrincipal currentUser = this.User;
                var userId = currentUser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                                   .Select(c => c.Value).SingleOrDefault();

                #region checkSubscriptionExist
                var currentUserSubscription = _userSubscriptionDataRepo.GetBy(x => x.BookId == subscribeReq.BookId && x.UserId == userId)?.FirstOrDefault();
                if (currentUserSubscription != null)
                {
                    response.Code = ResponseCode.OK;
                    response.Message = "subscription has already exsist";
                    response.Result = new SubscribeResponse { UserSubscriptionId = currentUserSubscription.Id };
                    return response;
                }
                #endregion

                #region InsertSubscription

                var newUserSubscription = new UserSubscription
                {
                    BookId = subscribeReq.BookId,
                    UserId = userId,
                    IsActive = true
                };

                var result = _userSubscriptionDataRepo.Insert(newUserSubscription);
                if (result.Code == ResponseCode.OK)
                {
                    response.Code = ResponseCode.OK;
                    response.Message = "subscription was created";
                    response.Result = new SubscribeResponse { UserSubscriptionId = newUserSubscription.Id };
                }

                #endregion
            }

            return response;
        }

        /// <summary>
        /// A user can unsubscribe to any book on their subscription by access token. 
        /// </summary>
        /// <param name="subscribeReq"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Unsubscribe")]
        [Authorize]
        public ActionResult<DataResult<SubscribeResponse>> Unsubscribe([FromBody] SubscribeRequest subscribeReq)
        {
            var response = new DataResult<SubscribeResponse>(null) { Code = ResponseCode.BadRequest, Message = "unsuccessful" };

            if (ModelState.IsValid)
            {
                System.Security.Claims.ClaimsPrincipal currentUser = this.User;
                var userId = currentUser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                                   .Select(c => c.Value).SingleOrDefault();

                #region ValidateSubscription
                var checkSubscriptionExist = _userSubscriptionDataRepo.GetBy(x => x.BookId == subscribeReq.BookId && x.UserId == userId)?.FirstOrDefault();

                if (checkSubscriptionExist == null)
                {
                    response.Code = ResponseCode.OK;
                    response.Message = "subscription couldn't found";
                    response.Result = new SubscribeResponse { };
                    return response;
                }

                #endregion

                #region SoftDeleteSubscription
                var currentSubscription = new UserSubscription
                {
                    BookId = subscribeReq.BookId,
                    UserId = userId,
                    IsActive = false
                };

                var result = _userSubscriptionDataRepo.Update(currentSubscription,
                                                                x => x.UserId == currentSubscription.UserId &&
                                                                x.BookId == currentSubscription.BookId);
                if (result.Code == ResponseCode.OK)
                {
                    response.Code = ResponseCode.OK;
                    response.Message = "subscription was deleted";
                }
                #endregion

            }

            return response;
        }

        /// <summary>
        /// Get user subscription list by access token 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("GetSubscriptions")]
        public ActionResult<DataResult<UserSubscriptionListResponse>> GetSubscriptions()
        {
            var response = new DataResult<UserSubscriptionListResponse>(null) { Code = ResponseCode.BadRequest, Message = "unsuccessful" };

            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var userId = currentUser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                               .Select(c => c.Value).SingleOrDefault();

            var userSubscribedBooks = _userSubscriptionDataRepo.GetSubscriptions(userId);
            if(userSubscribedBooks != null && userSubscribedBooks.Count > 0)
            {
                response.Code = ResponseCode.OK;
                response.Result.Books = userSubscribedBooks;
            }
            else
            {
                response.Code = ResponseCode.OK;
                response.Message = "user hasn't subscribed any book yet.";
            }

            return response;
        }

    }
}
