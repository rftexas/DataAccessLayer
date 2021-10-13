namespace DataAccessLayer.Tests
{
    public class TestCommand: DataAccess.Command
    {
        public TestCommand(string command)
        {
            QueryText = command;
        }
    }
}
