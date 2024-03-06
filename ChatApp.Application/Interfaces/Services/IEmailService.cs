namespace ChatApp.Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendUserActivationEmail(Guid userId);
    }
}
