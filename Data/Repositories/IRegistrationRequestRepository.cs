using InvesTime.BackEnd.Models;

namespace InvesTime.BackEnd.Data.Repositories;

public interface IRegistrationRequestRepository
{
    Task<IList<RegistrationRequest>> GetRegistrationRequestsByManagerName(string managerUsername);
    RegistrationRequest GetRegistrationRequest(string requestId);
    RegistrationRequest AddRegistrationRequest(RegistrationRequest request);
    Task<bool> DeleteRegistrationRequest(string requestId);
    Task<bool> ApproveRegistrationRequest(string requestId);
}