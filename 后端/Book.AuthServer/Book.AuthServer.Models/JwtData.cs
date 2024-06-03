using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.AuthServer.Models
{
    public class JwtData
    {
        public string sub { get; set; }
        public string jti { get; set; }
        public DateTime iat { get; set; }
        public long nbf { get; set; }
        public long exp { get; set; }
        public string iss { get; set; }
        public string aud { get; set; }
    }
}
