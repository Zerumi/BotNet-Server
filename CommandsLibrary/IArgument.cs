namespace CommandsLibrary
{
    public interface IArgument
    {
        public string Command { get; set; }
        public int ArgumentCount { get; set; }
        public string[] ArgumentsList { get; set; }
    }
}
