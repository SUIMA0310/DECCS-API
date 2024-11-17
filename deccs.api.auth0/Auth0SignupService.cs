using System.Text;
using System.Text.Json;
using Deccs.Api.Auth0.Model;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Deccs.Api.Auth0;

internal class Auth0SignupService : IAuth0SignupService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<Auth0SignupService> _logger;
    private readonly string _auth0Domain;
    private readonly string _auth0ClientId;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public Auth0SignupService(HttpClient httpClient, IConfiguration configuration, ILogger<Auth0SignupService> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _auth0Domain = configuration["Auth0:Domain"] ?? throw new ArgumentException("Auth0:Domain");
        _auth0ClientId = configuration["Auth0:ClientId"] ?? throw new ArgumentException("Auth0:ClientId");
    }

    public async Task<SignupResponse> SignupAsync(SignupRequest request)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        var url = $"https://{_auth0Domain}/dbconnections/signup";

        var payload = new
        {
            client_id = _auth0ClientId,
            email = request.Email,
            password = request.Password,
            connection = request.Connection
        };

        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

        try
        {
            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Auth0 Signup API failed with status code {StatusCode} and reason {ReasonPhrase}",
                    response.StatusCode, response.ReasonPhrase);
                response.EnsureSuccessStatusCode();
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<SignupResponse>(responseContent, _jsonSerializerOptions) ??
                   throw new InvalidOperationException("Failed to deserialize SignupResponse");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while calling Auth0 Signup API");
            throw;
        }
    }
}
