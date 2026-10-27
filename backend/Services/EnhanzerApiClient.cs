using System.Text.Json.Nodes;
using Backend.Dtos;

namespace Backend.Services;

public class EnhanzerApiClient(HttpClient http, IConfiguration config)
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
        if (!response.IsSuccessStatusCode) return null;

        var json = JsonNode.Parse(await response.Content.ReadAsStringAsync());
        var locations = FindArray(json, "User_Locations");
        if (locations is null) return null;

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
