using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookStore.Data.Models.ModelsDTO
{
    public class Publisher
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50), MinLength(2)]
        public string Name { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }

    }
}
