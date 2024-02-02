
namespace JamesBot;

class Program
{
    private static string? TOKEN;
    public static void GetToken()
    {
        var envFile = new DirectoryInfo(Environment.CurrentDirectory).Parent?.Parent?.Parent?.FullName + @"\.env";
        if (!File.Exists(envFile))
            throw new Exception("can't found .env file");
        StreamReader sr = new StreamReader(envFile);
        var line = sr.ReadLine();
        if (string.IsNullOrEmpty(line))
            throw new Exception("can't found token");
        TOKEN = line.ToString();
    }

    public const string PREFIX = "?";
    static void Main()
    {
        GetToken();
        var bot = new Bot(TOKEN, PREFIX);
        bot.RunAsync().GetAwaiter().GetResult();
    }
}