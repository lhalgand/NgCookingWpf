﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Newtonsoft.Json;

namespace apis.Models
{
    public class User : IdentityUser<Int32>
    {
        [JsonProperty("firstname")]
        public String FirstName { get; set; }

        [JsonProperty("surname")]
        public String LastName { get; set; }

        [JsonProperty("level")]
        public Int32 Level { get; set; }

        [JsonProperty("city")]
        public String City { get; set; }

        [JsonProperty("birth")]
        public Int16 BirthYear { get; set; }

        [JsonProperty("bio")]
        public String Bio { get; set; }

        [JsonProperty("picture")]
        public String Picture { get; set; }

        public Int32 JsonId { get; set; }

        public String Password { get; set; }
    }
}
