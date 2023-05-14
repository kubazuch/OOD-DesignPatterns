namespace ConsoleProject.CLI.Exception
{
    internal class MissingArgumentException : System.ArgumentException
    {
        public MissingArgumentException(Command command, int id, string name)
            : base($"Missing required argument #{id}: `§l{name}§r`.\n\tUsage: §l{command}")
        {
        }
        public MissingArgumentException(string command, int id, string name)
            : base($"Missing required argument #{id}: `§l{name}§r`.\n\tUsage: §l{command}")
        {
        }
    }
}
