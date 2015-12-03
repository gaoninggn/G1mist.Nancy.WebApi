using System.Collections.Generic;
using Nancy.Security;

namespace G1mist.Nancy.API.Model
{
    public class UserIdentify : IUserIdentity
    {
        public IEnumerable<string> Claims
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }
    }
}