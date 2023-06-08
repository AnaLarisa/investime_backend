using backend.Models;
using backend.Models.DTO;
using backend.Repositories;
using Microsoft.AspNetCore.Identity;

namespace backend.Services;

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
        var registrationRequest = CreateRegistrationRequest(request);
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

    private RegistrationRequest CreateRegistrationRequest(RegistrationRequestDto requestDto)
    {
        return new RegistrationRequest()
        {
            FirstName = requestDto.FirstName,
            LastName = requestDto.LastName,
            Username = requestDto.Username,
            ManagerUsername = requestDto.ManagerUsername,
            Email = requestDto.Email
        };
    }
}