using Microsoft.AspNetCore.Http;

namespace TuyenDungWeb.Utility.Helpers
{
    public interface IFileValidator
    {
        bool IsValid(IFormFile file);
    }
}
