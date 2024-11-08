namespace JamesBot.Zork.Model;

public class Zork
{
    public class Profile
    {
        public List<string> hike { get; set; }
        public string lastGame { get; set; }
        public List<object> spell { get; set; }
        public string userEmail { get; set; }
        public List<object> wish { get; set; }
        public List<object> zork1 { get; set; }
        public List<object> zork2 { get; set; }
        public List<object> zork3 { get; set; }
    }
    public class LookOutput
    {
        public string firstLine { get; set; }
        public string titleInfo { get; set; }
    }
    public string cmdOutput { get; set; }
    public LookOutput lookOutput { get; set; }
    public bool newUser { get; set; }
    public Profile profile { get; set; }
    public string firstLine { get; set; }
    public string titleInfo { get; set; }
    public Profile userProfile { get; set; }
}
