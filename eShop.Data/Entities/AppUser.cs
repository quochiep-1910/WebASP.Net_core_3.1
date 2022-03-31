using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace eShop.Data.Entities
{
    public class AppUser : IdentityUser<string>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime Dob { get; set; }

        public List<Cart> Carts { get; set; }

        public List<Order> Orders { get; set; }

        public List<Transaction> Transactions { get; set; }

        public bool IsDelete { get; set; }
        public string VerificationToken { get; set; }
        public string ResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public DateTime? PasswordReset { get; set; }

        public string ProfileImageUrl { get; set; }
    }
}