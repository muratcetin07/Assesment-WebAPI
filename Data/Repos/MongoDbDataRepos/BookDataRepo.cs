using Data.Abstract;
using Data.Concrete;
using Data.Core;
using Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;

namespace Data
{
    public class BookDataRepo : EntityBaseMongoData<Book>, IBookDataRepo
    {
        public BookDataRepo() : base(MongoTablename.Book)
        {
        }
    }
}
