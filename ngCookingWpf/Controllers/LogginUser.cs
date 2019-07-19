using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apis.Controllers
{
    public class LogginUser
    {
        [JsonProperty("email")]
        public String email { get; set; }

        [JsonProperty("password")]
        public String password { get; set; }

    }
}
