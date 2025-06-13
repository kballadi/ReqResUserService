using ReqRes.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReqRes.Core.Interfaces;

public interface IExternalUserService
{
    Task<User?> GetUserByIdAsync(int id);
    Task<IEnumerable<User>> GetAllUsersAsync();
}
