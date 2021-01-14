using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace project.handlers
{
    // BasicAuthenticationHandler class
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly DBvegetableContext _context;
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            DBvegetableContext context)
            :base(options,logger, encoder, clock)
        {
            _context = context;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            //if not request in header "Login" show fail message
            if(!Request.Headers.ContainsKey("Login"))
                return AuthenticateResult.Fail("Login header was not found");
            try
            {
                //else get the "Login" request from header
                var authenticationHeaderValue = AuthenticationHeaderValue.Parse(Request.Headers["Login"]);
                //parse enctype username and password from base64String to UTF8 then split by : and save into variables
                var bytes = Convert.FromBase64String(authenticationHeaderValue.Parameter);
                string[] credentials = Encoding.UTF8.GetString(bytes).Split(":");
                string username = credentials[0];
                string password = credentials[1];
                //create new "user" object and then check if username and password are valid
                User user = _context.Users.Where(user => user.Username == username && user.Password == password).FirstOrDefault();
                //if invalid
                if(user==null)
                    AuthenticateResult.Fail("Invalid username or password");
                else
                {
                    //else return success result (the user can use the Authorize function in vegetable Controller)
                    var claims = new[] { new Claim(ClaimTypes.Name,user.Username) };
                    var identity = new ClaimsIdentity(claims, Scheme.Name);
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);
                    return AuthenticateResult.Success(ticket);
                }
            }
            catch(Exception)
            {
                return AuthenticateResult.Fail("Error has occured");
            }
            return AuthenticateResult.Fail("");

        }
    }
}
