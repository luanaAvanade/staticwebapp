using Gicaf.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gicaf.API.Authentication
{
    public class AutResult
    {
        public bool newUser { get; set; }
        public UserAuthenticator user { get; set; }
    }

    public class UserAuthenticator
    {
        public string name { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public bool internalUser { get; set; }
        public bool emailConfirmed { get; set; }
    }
}
