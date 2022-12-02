namespace _0_Framework.Application.Sms
{
    public interface ISmsService
    {
        void SendAsync(string number, string message);
    }
}