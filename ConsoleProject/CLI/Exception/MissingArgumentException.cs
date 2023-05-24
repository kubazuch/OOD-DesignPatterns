namespace ConsoleProject.CLI.Exception
{
    internal class MissingArgumentException : System.ArgumentException
    {
        public MissingArgumentException(CommandParser command, int id, string name)
            : base($"Missing required argument #{id}: `{name}`.\n\tUsage: §l{command}")
        { }

        public MissingArgumentException(string command, int id, string name)
            : base($"Missing required argument #{id}: `{name}`.\n\tUsage: §l{command}")
        {
        }
    }
}
