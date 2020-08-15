using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class AuthOptions
    {
        public string ApiKey { get; set; }
        public string Audience { get; set; }
        public string Authority { get; set; }
        public ClientOptions[] Clients { get; set; }
    }
}
