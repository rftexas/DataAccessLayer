using System.Data;
using System.Data.Common;

namespace DataAccessLayer.Tests
{
    public class FakeDbTransaction : DbTransaction
    {
        public FakeDbTransaction(DbConnection connection, IsolationLevel isolationLevel) => (DbConnection, IsolationLevel) = (connection, isolationLevel);
        public override IsolationLevel IsolationLevel { get; }

        protected override DbConnection DbConnection { get; }

        public override void Commit()
        {
            
        }

        public override void Rollback()
        {
            
        }
    }
}
