using Core.DTOs;
using Core.DTOs.Requests;

namespace Core.Interfaces;

public interface IIdentityService
{
    Task<AuthenticationResponse> LoginAsync(LoginRequest command, string ipAddress, string userAgent,
        CancellationToken cancellationToken); 
}