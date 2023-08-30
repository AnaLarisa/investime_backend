namespace InvesTime.BackEnd.Helpers;

public interface IUserHelper
{
    public string GetCurrentUserId();
    public string GetCurrentUserUsername();
    public bool IsCurrentUserAdmin();
}