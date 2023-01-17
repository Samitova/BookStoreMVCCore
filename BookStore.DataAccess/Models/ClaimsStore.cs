using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace BookStore.DataAccess.Models
{
    public static class ClaimsStore
    {
        public static List<Claim> AllClaims { get; set; } = new List<Claim>()
        { 
            new Claim("Create Role", "Create Role"),
            new Claim("Edit Role", "Edit Role"),
            new Claim("Delete Role", "Delete Role")
        };
    }
}
