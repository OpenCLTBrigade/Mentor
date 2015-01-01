using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace Mentor
{
    public class UserService
    {
        private readonly MentorDb _db;

        public UserService(MentorDb db)
        {
            _db = db;
        }

        public User Find(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            return _db.Users
                      .SingleOrDefault(x => x.Email == email);
        }

        public void Login(string email, string password)
        {
            var user = Find(email);
            if (user == null || !user.Active || password == null || user.Password != password)
                throw new ApplicationException("Invalid login");

            var roles = new string[0];
            if (user.IsAdmin == true)
            {
                roles = new[] { "Admin" };
            }

            SetPrincipal(user.Email, roles);
        }

        public void Logout()
        {
            HttpContext.Current.Session.Abandon();
            FormsAuthentication.SignOut();
        }

        public static void SetPrincipal(string username, string[] roles)
        {
            var authTicket = new FormsAuthenticationTicket(
                1,
                username,
                DateTime.Now,
                DateTime.Now.AddMinutes(20),  // expiry
                true,  //do not remember
                String.Join(",", roles),
                "/");

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static GenericPrincipal GetPrincipal()
        {
            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null)
                return null;

            var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            if (authTicket == null)
                return null;

            var roles = authTicket.UserData.Split(new[] { ',' });
            return new GenericPrincipal(new GenericIdentity(authTicket.Name), roles);
        }
    };
}
