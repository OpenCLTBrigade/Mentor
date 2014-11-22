using System;
using System.Linq;
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
            if (user == null || !user.Active ||  password == null || user.Password != password)
                throw new ApplicationException("Invalid login");

            FormsAuthentication.SetAuthCookie(user.Email, true);
        }

        public void Logout()
        {
            HttpContext.Current.Session.Abandon();
            FormsAuthentication.SignOut();
        }
    };
}
