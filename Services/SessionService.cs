using pr.Models;

namespace pr.Services;

public class SessionService : ISessionService
{
    public User? CurrentUser { get; private set; }

    public void SetCurrentUser(User user)
    {
        CurrentUser = user;
    }

    public void ClearCurrentUser()
    {
        CurrentUser = null;
    }
}
