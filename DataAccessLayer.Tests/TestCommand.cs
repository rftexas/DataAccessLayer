using System.Data;

namespace DataAccessLayer.Tests
{
    public class TestCommand: DataAccess.ICommand
    {
        public TestCommand(string command)
        {
            QueryText = command;
        }

        public string QueryText { get; }
        public object Parameters { get; }
        public CommandType CommandType { get; } = CommandType.Text;
    }
}
