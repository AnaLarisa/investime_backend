using backend.Models;
using backend.Models.DTO;

namespace backend.Services
{
    public interface IRegistrationRequestService
    {
        RegistrationRequest AddRegistrationRequest(RegistrationRequestDto request);
        Task<bool> ApproveRegistrationRequest(string requestId);
        Task<bool> DeleteRegistrationRequest(string requestId);
        Task<IList<RegistrationRequest>> GetRegistrationRequestsByManagerName(string managerUsername);
        RegistrationRequest GetRegistrationRequest(string requestId);

    }
}