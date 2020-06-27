using Data.Abstract;
using Data.Concrete;
using Data.Core;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;

namespace Data
{
    public class UserSubscriptionDataRepo : EntityBaseMongoData<UserSubscription>, IUserSubscriptionDataRepo
    {
        public UserSubscriptionDataRepo() : base(MongoTablename.UserSubscription)
        {
        }

        public List<Book> GetSubscriptions(string userId)
        {
            var subscrtionBookIds = this.GetBy(x => x.UserId == userId && x.IsActive).Select(x => x.BookId).ToList();
            if(subscrtionBookIds != null && subscrtionBookIds.Count > 0)
            {
                var bookList = new BookDataRepo().GetBy(x => subscrtionBookIds.Equals(x.Id)).ToList();
                return bookList;
            }

            return null;
                
        }
    }
}
