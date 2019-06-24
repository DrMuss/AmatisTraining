using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AT.Services.Recapture
{
    public class RecaptchaConfig
    {
        public string SecretKey { get; set; }
        public string SiteKey { get; set; }
    }
}
