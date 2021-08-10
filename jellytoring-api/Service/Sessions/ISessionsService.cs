using System.Threading.Tasks;
using jellytoring_api.Models.Users;

namespace jellytoring_api.Service.Users
{
    public interface ISessionsService
    {
        Task<string> CreateAsync(CreateSessionUser createSessionUser);
    }
}