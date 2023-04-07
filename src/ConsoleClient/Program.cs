using IdentityModel.Client;
using System.Text.Json;

var client = new HttpClient();

var discoveryResult = await client.GetDiscoveryDocumentAsync("http://localhost:5273");
if (discoveryResult.IsError)
{
    Console.WriteLine(discoveryResult.Error);
    Environment.Exit(1);
}

var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
{
    Address = discoveryResult.TokenEndpoint,
    ClientId = "console-client",
    ClientSecret = "console-client-secret",
    Scope = "api:read"
});

if (tokenResponse.IsError)
{
    Console.WriteLine(tokenResponse.Error);
    Environment.Exit(1);
}

var apiClient = new HttpClient();
apiClient.SetBearerToken(tokenResponse.AccessToken);

var response = await apiClient.GetAsync("http://localhost:5298/resource");

if (!response.IsSuccessStatusCode)
{
    Console.WriteLine(response.StatusCode);
}
else
{
    var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
    Console.WriteLine(JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true }));
}