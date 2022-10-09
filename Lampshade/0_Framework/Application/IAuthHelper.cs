namespace _0_Framework.Application
{
    public interface IAuthHelper
    {
        void Signin(AuthViewModel account);
        void SignOut();
        bool IsAuthenticated();
        string CurrentAccountRole();
        AuthViewModel CurrentAccountInfo();

    }
}
