using System;
using System.Collections.Generic;

#nullable disable

namespace project.Models
{
    public partial class User
    {
        private string username, email, password;
        public string Username { get { return username; } set { if (value != null && value.Length <= 50) username = value; } }
        public string Email { get { return email; } set { if (value != null && value.Length <= 50) email = value; } }
        public string Password { get { return password; } set { if (value != null && value.Length <= 50) password = value; } }
    }
}
