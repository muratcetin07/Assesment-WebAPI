using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model
{
    public class UserSubscription : Core.MongoBaseModel
    {
        [Required]
        public string UserId { get; set; } 

        [Required]
        public string BookId { get; set; }

        public bool IsActive { get; set; }

    }
}
