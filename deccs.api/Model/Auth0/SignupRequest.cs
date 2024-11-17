using System.Diagnostics;

namespace Deccs.Api.Model.Auth0;

[DebuggerDisplay("{Username} / {Email}")]
public record SignupRequest(
    string ClientId, 
    string Email, 
    string Password, 
    string Connection, 
    string Username, 
    string? GivenName, 
    string? FamilyName, 
    string? Name, 
    string? Nickname, 
    string? Picture, 
    UserMetadata UserMetadata
)
{
    public static string DefaultConnection = "Username-Password-Authentication";
}