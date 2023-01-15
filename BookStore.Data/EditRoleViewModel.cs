using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text;

namespace BookStore.ViewModelData
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<string>();
        }
        public string Id { get; set; }

        [Required(ErrorMessage ="Role name is required")]
        [DisplayName("Role Name")]
        public string RoleName { get; set; }
        public List<string> Users { get; set; }
    }
}
