using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class ClientOptions
    {
        public string Authority { get; set; }
        public string ClientId { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}, Authority: {Authority}, ClientId: {ClientId}";
        }
    }
}
