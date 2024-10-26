namespace JamesBot;

class Program
{
    private static string? _token;
    private static readonly string _path = new DirectoryInfo(Environment.CurrentDirectory)
    .FullName ?? "";
    public const string PREFIX = "?";

    public static void GetToken()
    {
        var envFile = _path + "/.env";
        if (!File.Exists(envFile))
            throw new Exception("can't found .env file");
        var sr = new StreamReader(envFile);
        var line = sr.ReadLine();
        if (string.IsNullOrEmpty(line))
            throw new Exception("can't found token");
        _token = line.ToString();
    }

    static void Main()
    {
        GetToken();
        var bot = new Bot(_token ?? "", PREFIX, _path);
        bot.RunAsync().GetAwaiter().GetResult();
    }
}