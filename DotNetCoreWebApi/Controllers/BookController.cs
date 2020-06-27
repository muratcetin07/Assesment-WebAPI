using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Abstract;
using Data.Core;
using Data;
using Microsoft.AspNetCore.Mvc;
using Model;
using DTO;
using Microsoft.AspNetCore.Authorization;
using Utilities;
using System.Security.Claims;

namespace DotNetCoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        IBookDataRepo _bookDataRepo;

        public BookController(IBookDataRepo bookDataRepo)
        {
            _bookDataRepo = bookDataRepo;
        }

        /// <summary>
        /// get all books on database by access token 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("GetBooks")]
        public ActionResult<DataResult<BookResponse>> GetBooks()
        {
            var response = new DataResult<BookResponse>(null) { Code = ResponseCode.BadRequest, Message = "unsuccessful" };


            var books = _bookDataRepo.GetAll();
            if (books != null && books.Count > 0)
            {
                response.Code = ResponseCode.OK;
                response.Result.Books = books;
            }
            else
            {
                response.Code = ResponseCode.OK;
                response.Message = "couldn't find any book";
            }

            return response;
        }
    }
}
