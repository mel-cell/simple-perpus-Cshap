using pr.Models;

namespace pr.Services;

public interface ISessionService
{
    User? CurrentUser { get; }
    void SetCurrentUser(User user);
    void ClearCurrentUser();
}
