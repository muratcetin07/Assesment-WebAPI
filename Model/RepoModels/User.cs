using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model
{
    public class User : Core.MongoBaseModel
    {
        [Required]
        public string FirstName { get; set; } 

        [Required]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
