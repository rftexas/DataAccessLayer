using System;

namespace DataAccessLayer.Exceptions
{
    public class InvalidQueryException: Exception
    {
        public InvalidQueryException(string message): base(message) { }
    }
    public class InvalidCommandException : Exception
    {
        public InvalidCommandException(string message) : base(message) { }
    }
}
