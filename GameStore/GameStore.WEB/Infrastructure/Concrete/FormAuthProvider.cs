using GameStore.WEB.Infrastructure.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;

namespace GameStore.WEB.Infrastructure.Concrete
{
    public class FormAuthProvider : IAuthProvider
    {

        public bool Authenticate(string userName, string password)
        {
            bool result = FormsAuthentication.Authenticate(userName, password);

            if (result)
            {
                FormsAuthentication.SetAuthCookie(userName, false);
            }
            return result;
        }
    }
}