using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MotorDepot.WEB.Models
{
    public class LoginResponse
    {
        public String Name { get; set; }
        public HttpCookie Cookie { get; set; }
        public String Role { get; set; }
    }
}