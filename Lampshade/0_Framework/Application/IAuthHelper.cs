using System.Collections.Generic;
using _0_Framework.Infrastructure;

namespace _0_Framework.Application
{
    public interface IAuthHelper
    {
        void Signin(AuthViewModel account);
        void SignOut();
        bool IsAuthenticated();
        string CurrentAccountRole();
        AuthViewModel CurrentAccountInfo();
        List<int> GetPermissions();
        long CurrentAccountId();
    }
}
