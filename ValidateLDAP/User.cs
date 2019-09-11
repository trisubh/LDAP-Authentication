using System;
using System.Collections.Generic;
using System.Text;

namespace ValidateLDAP
{
    class User
    {
        public User(string userid, string password)
        {
            _userid = userid;
            _password = password;
        }
        private string _userid;
        private string _password;



        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }


        public string Message { get; set; }

        public string Userid { get { return _userid; } set { value = _userid; } }

        public string Password { get { return _password; } set { value = _password; } }
    }
}
