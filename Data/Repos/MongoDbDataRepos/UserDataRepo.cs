using Data.Abstract;
using Data.Concrete;
using Data.Core;
using Model;
using System;
using System.Collections.Generic;
using System.Text;
using Utilities;

namespace Data
{
    public class UserDataRepo : EntityBaseMongoData<User>, IUserDataRepo
    {
        public UserDataRepo() : base(MongoTablename.User)
        {
        }
    }
}
