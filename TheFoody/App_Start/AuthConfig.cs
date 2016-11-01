using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using TheFoody.Models;

namespace TheFoody.App_Start
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            OAuthWebSecurity.RegisterFacebookClient(
                appId: "1044738812290783",
                appSecret: "e6c6d04db394c62c31de705eed5dcc6c"
                );
        }
    }
}