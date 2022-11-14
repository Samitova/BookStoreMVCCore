﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookStore.Data.Models.ModelsDTO
{
    public class PublisherDTO : BaseEntity
    {        
        [Required]
        [MaxLength(50), MinLength(2)]
        public string PublisherName { get; set; }

        [MaxLength(50)]
        public string City { get; set; }

        [MaxLength(50)]
        public string Phone { get; set; }

    }
}
