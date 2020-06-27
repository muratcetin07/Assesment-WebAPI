using Model.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model
{
    public class Book : MongoBaseModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public decimal PurchasePrice { get; set; }
    }
}
