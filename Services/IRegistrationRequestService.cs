using InvesTime.BackEnd.Models;
using InvesTime.BackEnd.Models.DTO;

namespace InvesTime.BackEnd.Services;

public interface IRegistrationRequestService
{
    RegistrationRequest AddRegistrationRequest(RegistrationRequestDto request);
    Task<bool> ApproveRegistrationRequest(string requestId);
    Task<bool> DeleteRegistrationRequest(string requestId);
    Task<IList<RegistrationRequest>> GetRegistrationRequestsByManagerName();
    RegistrationRequest GetRegistrationRequest(string requestId);

}