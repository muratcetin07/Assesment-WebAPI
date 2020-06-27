using Model;
using Model.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Abstract
{
    public interface IUserSubscriptionDataRepo : IData<UserSubscription>
    {
        List<Book> GetSubscriptions(string userId);
    }
}
