using ModelLayer.DTOs.Email;

namespace BusinessLogicLayer.Interface
{
    public interface IMessagePublisher
    {
        void PublishEmail(EmailMessageDTO message);
    }
}
