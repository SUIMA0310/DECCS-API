using Deccs.Api.Auth0.Model;

namespace Deccs.Api.Auth0;
internal interface IAuth0SignupService
{
    Task<SignupResponse> SignupAsync(SignupRequest request);
}
