using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text;

namespace BookStore.ViewModelData
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DisplayName("Current Password")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required]
        [DisplayName("New Password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match")]
        public string ConfirmPassword { get; set; }
    }
}
