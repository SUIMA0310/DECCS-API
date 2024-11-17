namespace Deccs.Api.Model.Auth0;
public record SignupResponse(
    string _Id, 
    bool EmailVerified, 
    string Email, 
    string Username, 
    string? GivenName, 
    string? FamilyName, 
    string? Name, 
    string? Nickname, 
    string Picture
);
