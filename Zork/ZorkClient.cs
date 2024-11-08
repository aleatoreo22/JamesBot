using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace JamesBot.Zork;

public class ZorkClient
{
    const string BASEURL = "http://one:8443/";

    private static void NormalizeText(ref Model.Zork zork)
    {
        zork.cmdOutput = Regex.Replace(zork.cmdOutput, @"\x1b\[[0-9;]*[A-Za-z]", "");
        zork.lookOutput.firstLine = Regex.Replace(zork.lookOutput.firstLine, @"\x1b\[[0-9;]*[A-Za-z]", "");
        zork.firstLine = Regex.Replace(zork.firstLine, @"\x1b\[[0-9;]*[A-Za-z]", "");
    }

    public static async Task<Model.Zork?> Action(string email, string game_title, string action)
    {
        var paramters = new Dictionary<string, object>
        {
            {"email",email},
            {"game_title",game_title},
            {"action",action},
        };
        return await ExecuteRequest("action", paramters);
    }
    public static async Task<Model.Zork?> Start(string email, string game_title, string save = "")
    {
        var paramters = new Dictionary<string, object>
        {
            {"email",email},
            {"game_title",game_title},
         };
        return await ExecuteRequest("start", paramters);
    }

    public static async Task<Model.Zork?> User(string email)
    {
        var paramters = new Dictionary<string, object>
        {
            {"email",email}
        };
        return await ExecuteRequest("user", paramters);
    }

    private static async Task<Model.Zork?> ExecuteRequest(string endpoint, Dictionary<string, object>? paramters = null)
    {
        var url = BASEURL + endpoint;
        if (paramters != null)
        {
            url += "?" + string.Join('&', paramters.Select(x => $"{x.Key}={x.Value}"));
        }
        string responseBody;

        using (var client = new HttpClient())
        {
            var response = await client.GetAsync(url);
            responseBody = await response.Content.ReadAsStringAsync();
        }
        var zork = JsonConvert.DeserializeObject<Model.Zork>(responseBody);
        if (zork == null)
            return default;
        NormalizeText(ref zork);
        return zork;
    }
}
