namespace Deccs.Api.Auth0.Model;
internal record SignupRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Connection { get; set; }
    public string UserName { get; set; }
}
