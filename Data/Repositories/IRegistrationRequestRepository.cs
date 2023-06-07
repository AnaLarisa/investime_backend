using backend.Models;

namespace backend.Repositories;

public interface IRegistrationRequestRepository
{
    Task<IList<RegistrationRequest>> GetRegistrationRequestsByManagerName(string managerUsername);
    RegistrationRequest GetRegistrationRequest(string requestId);
    RegistrationRequest AddRegistrationRequest(RegistrationRequest request);
    Task<bool> DeleteRegistrationRequest(string requestId);
    Task<bool> ApproveRegistrationRequest(string requestId);
}