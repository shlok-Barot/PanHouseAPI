using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PanHouse.Model;
using PanHouse.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace PanHouse.WebAPI.Filters
{
    public class TokenAuthenticationHandler : AuthenticationHandler<TokenAuthenticationOptions>
    {
        public const string SchemeName = "TokenAuth";
        public IConfiguration Configurations { get; }
        public TokenAuthenticationHandler(IOptionsMonitor<TokenAuthenticationOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IConfiguration configuration)
                : base(options, logger, encoder, clock)
        {
            Configurations = configuration;
        }
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            return Task.Run(() => Authenticate());
        }
        private AuthenticateResult Authenticate()
        {
            string token = Context.Request.Headers["X-Authorized-Header"];
            string userId = Context.Request.Headers["X-Authorized-userId"];
            if (token == null) return AuthenticateResult.Fail("No Authorization token provided");
            try
            {
                //var DecryptStringAES = SecurityService.DecryptStringAES(token);
                string DecryptStringAES = SecurityService.DecryptStringAESJSON(token);
                string url = Context.Request.Host.Value;
                Authenticate authenticate = new Authenticate();
                authenticate = JsonConvert.DeserializeObject<Authenticate>(DecryptStringAES);

                if (!string.IsNullOrEmpty(authenticate.Message))
                {
                    if (authenticate.Message.Equals("Valid User"))
                    {
                        var Date_time = DateTime.Now.AddDays(-1);
                        if (authenticate.CurrentDatetime >= Date_time)
                        {
                            Configurations["JSONData"] = DecryptStringAES;
                            string token_Date = SecurityService.DecryptStringAES(authenticate.Token.Split('.')[0]);
                            byte[] base64toByte = Convert.FromBase64String(token_Date);
                            string byteToString = Encoding.ASCII.GetString(base64toByte);

                            var claims = new[] { new Claim(ClaimTypes.Name, "1") };
                            var identity = new ClaimsIdentity(claims, Scheme.Name);
                            var principal = new ClaimsPrincipal(identity);
                            var ticket = new AuthenticationTicket(principal, Scheme.Name);
                            return AuthenticateResult.Success(ticket);
                        }
                        else
                        {
                            return AuthenticateResult.Fail("Token Expired");
                        }
                    }
                    else
                    {
                        return AuthenticateResult.Fail("Invalid User");
                    }
                }
                else
                {
                    return AuthenticateResult.Fail("Invalid Authorization");
                }
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail("Failed to validate token");
            }
        }

        private string generatetoken(string Domain_Name, string ApplicationId, string UserEmailId)
        {
            try
            {
                byte[] bytes = Encoding.ASCII.GetBytes(Domain_Name + "_" + ApplicationId + "_" + UserEmailId);
                string token = Convert.ToBase64String(bytes);
                byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
                byte[] key = Guid.NewGuid().ToByteArray();
                string SecreateToken = token + "." + Convert.ToBase64String(time.Concat(key).ToArray());

                return SecreateToken;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
    public static class SchemesNamesConst
    {
        public const string TokenAuthenticationDefaultScheme = "TokenAuthenticationScheme";
    }
    public class TokenAuthenticationOptions : AuthenticationSchemeOptions
    {

    }
}
