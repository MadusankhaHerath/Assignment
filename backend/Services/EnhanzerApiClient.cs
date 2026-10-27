using System.Text.Json.Nodes;
using Backend.Dtos;

namespace Backend.Services;

public class EnhanzerApiClient(HttpClient http, IConfiguration config, ILogger<EnhanzerApiClient> logger)
{
    public async Task<List<LocationDto>?> LoginAsync(string email, string password)
    {
        var body = new
        {
            API_Action = "GetLoginData",
            Device_Id = "D001",
            Sync_Time = "",
            Company_Code = email,
            API_Body = new { Username = email, Pw = password }
        };

        var response = await http.PostAsJsonAsync(config["EnhanzerApi:Url"], body);
        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            logger.LogWarning("Enhanzer login failed with {Status}: {Body}", response.StatusCode, content);
            return null;
        }

        var locations = FindArray(JsonNode.Parse(content), "User_Locations");
        if (locations is null)
        {
            logger.LogWarning("Enhanzer login returned no User_Locations: {Body}", content);
            return null;
        }

        return locations
            .Where(l => l is not null)
            .Select(l => new LocationDto(
                l!["Location_Code"]?.ToString() ?? "",
                l["Location_Name"]?.ToString() ?? ""))
            .Where(l => l.LocationCode != "")
            .ToList();
    }

    private static JsonArray? FindArray(JsonNode? node, string name)
    {
        switch (node)
        {
            case JsonObject obj:
                foreach (var (key, value) in obj)
                {
                    if (key == name && value is JsonArray arr) return arr;
                    var found = FindArray(value, name);
                    if (found is not null) return found;
                }
                break;
            case JsonArray array:
                foreach (var item in array)
                {
                    var found = FindArray(item, name);
                    if (found is not null) return found;
                }
                break;
        }
        return null;
    }
}
