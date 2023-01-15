using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookStore.ViewModelData
{
    public class CreateRoleViewModel
    {
        [Required]
        [DisplayName("Role Name")]
        public string RoleName { get; set; }
    }
}
