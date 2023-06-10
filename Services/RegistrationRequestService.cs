using InvesTime.BackEnd.Data.Repositories;
using InvesTime.BackEnd.Helpers;
using InvesTime.BackEnd.Models;
using InvesTime.BackEnd.Models.DTO;

namespace InvesTime.BackEnd.Services;

public class RegistrationRequestService : IRegistrationRequestService
{
    private readonly IRegistrationRequestRepository _registrationRequestRepository;

    public RegistrationRequestService(IRegistrationRequestRepository registrationRequestRepository)
    {
        _registrationRequestRepository = registrationRequestRepository;
    }

    public Task<IList<RegistrationRequest>> GetRegistrationRequestsByManagerName(string managerUsername)
    {
        return _registrationRequestRepository.GetRegistrationRequestsByManagerName(managerUsername);
    }

    public RegistrationRequest GetRegistrationRequest(string requestId)
    {
        return _registrationRequestRepository.GetRegistrationRequest(requestId);
    }

    public RegistrationRequest AddRegistrationRequest(RegistrationRequestDto request)
    {
        var registrationRequest = ObjectConverter.Convert<RegistrationRequestDto, RegistrationRequest>(request);
        return _registrationRequestRepository.AddRegistrationRequest(registrationRequest);
    }

    public Task<bool> DeleteRegistrationRequest(string requestId)
    {
        return _registrationRequestRepository.DeleteRegistrationRequest(requestId);
    }

    public Task<bool> ApproveRegistrationRequest(string requestId)
    {
        return _registrationRequestRepository.ApproveRegistrationRequest(requestId);
    }
}