using TuyenDungWeb.Models;

namespace TuyenDungWeb.DataAccess.Services
{
    public interface IEmailService
    {
        void SendEmail(Message message);
    }
}
