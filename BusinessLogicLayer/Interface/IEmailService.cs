namespace BusinessLogicLayer.Interface
{
    public interface IEmailService
    {
        void SendEmail(string toEmail, string subject, string body);
    }
}
