namespace InstagramAPI.Settings
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
    }
}
