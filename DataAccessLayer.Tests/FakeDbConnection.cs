using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace DataAccessLayer.Tests
{
    public class FakeDbConnection : DbConnection
    {
        private ConnectionState _state;

        private readonly FakeDbDataReader _reader = new();

        public FakeDbConnection(string connectionString) {
            ConnectionString = connectionString;
            _state = ConnectionState.Closed;
        }

        public override string ConnectionString { get; set; }

        public override string Database { get; } = "";

        public override string DataSource { get; } = "";

        public override string ServerVersion { get; } = "";

        public override ConnectionState State => _state;

        public override void ChangeDatabase(string databaseName)
        {
            
        }

        public override void Close()
        {
            _state = ConnectionState.Closed;
        }

        public override void Open()
        {
            _state = ConnectionState.Open;
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            return new FakeDbTransaction(this, isolationLevel);
        }

        protected override DbCommand CreateDbCommand()
        {
            return new FakeDbCommand(_reader) { Connection = this };
        }

        public void AddResultSet<T>(params T[] values)
        {
            _reader.AddResultSet(values.Select(t => new FakeResult<T>(t)).ToArray());
        }

        
    }
    public abstract class FakeResult
    {
        protected PropertyInfo[] _properties;

        public abstract object Get(int ordinal);
        public abstract object Get(string name);
        public string GetName(int ordinal) => _properties[ordinal].Name;
        public int FieldCount => _properties.Length;
    }

    public class FakeResult<T> : FakeResult
    {
        private readonly T _value;
        public FakeResult(T value)
        {
            _value = value;

            _properties = typeof(T).GetProperties();
        }


        public override object Get(int ordinal) => _properties[ordinal].GetValue(_value);

        public override object Get(string name) => _properties.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase))?.GetValue(_value);
    }

    public class FakeDbDataReader : DbDataReader
    {
        private readonly List<FakeResult[]> _resultSets=new();
        private int _currentResultSet = 0;
        private int _currentResult = -1;
        public void AddResultSet(FakeResult[] results) {
            _resultSets.Add(results);
        }

        private FakeResult Current => _resultSets[(_currentResultSet < 0 ? 0 : _currentResultSet)][(_currentResult < 0 ? 0 : _currentResult)];

        public override object this[int ordinal] => Current.Get(ordinal);

        public override object this[string name] => Current.Get(name);

        public override int Depth => _currentResult;

        public override int FieldCount => Current.FieldCount;

        public override bool HasRows => _resultSets[_currentResultSet].Length > 0;

        public override bool IsClosed => false;

        public override int RecordsAffected => 0;

        public override bool GetBoolean(int ordinal) => Current.Get(ordinal) as bool? ?? throw new InvalidOperationException();

        public override byte GetByte(int ordinal) => Current.Get(ordinal) as byte? ?? throw new InvalidOperationException();

        public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            throw new NotImplementedException();
        }

        public override char GetChar(int ordinal) => Current.Get(ordinal) as char? ?? throw new InvalidOperationException();

        public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            throw new NotImplementedException();
        }

        public override string GetDataTypeName(int ordinal)
        {
            return Current.Get(ordinal).GetType().Name;
        }

        public override DateTime GetDateTime(int ordinal) => Current.Get(ordinal) as DateTime? ?? throw new InvalidOperationException();

        public override decimal GetDecimal(int ordinal) => Current.Get(ordinal) as decimal? ?? throw new InvalidOperationException();

        public override double GetDouble(int ordinal) => Current.Get(ordinal) as double? ?? throw new InvalidOperationException();

        public override IEnumerator GetEnumerator()
        {
            return _resultSets[_currentResultSet].GetEnumerator();
        }

        public override Type GetFieldType(int ordinal)
        {
            return Current.Get(ordinal).GetType();
        }

        public override float GetFloat(int ordinal) => Current.Get(ordinal) as float? ?? throw new InvalidOperationException();

        public override Guid GetGuid(int ordinal) => Current.Get(ordinal) as Guid? ?? throw new InvalidOperationException();

        public override short GetInt16(int ordinal) => Current.Get(ordinal) as short? ?? throw new InvalidOperationException();

        public override int GetInt32(int ordinal) => Current.Get(ordinal) as int? ?? throw new InvalidOperationException();

        public override long GetInt64(int ordinal) => Current.Get(ordinal) as long? ?? throw new InvalidOperationException();

        public override string GetName(int ordinal) => Current.GetName(ordinal);

        public override int GetOrdinal(string name)
        {
            for(var i =0;i < Current.FieldCount; ++i)
            {
                if (Current.GetName(i).Equals(name, StringComparison.OrdinalIgnoreCase))
                    return i;
            }
            return -1;
        }

        public override string GetString(int ordinal) => Current.Get(ordinal) as string;

        public override object GetValue(int ordinal) => Current.Get(ordinal);

        public override int GetValues(object[] values)
        {
            values = new object[Current.FieldCount];
            for(var i =0; i<values.Length; ++i)
            {
                values[i] = Current.Get(i);
            }

            return values.Length;
        }

        public override bool IsDBNull(int ordinal) => Current.Get(ordinal) == null;

        public override bool NextResult()
        {
            _currentResultSet++;
            _currentResult = -1;
            return _currentResultSet < _resultSets.Count;
        }

        public override bool Read()
        {
            _currentResult++;
            return _currentResult < _resultSets[_currentResultSet].Length;
        }

        public override DataTable GetSchemaTable()
        {
            var schemaTable = new DataTable();
            schemaTable.Columns.Add("ColumnName", typeof(string));
            for(var i=0; i < Current.FieldCount; ++i)
            {
                var row = schemaTable.NewRow();
                row.SetField("ColumnName", Current.GetName(i));
                schemaTable.Rows.Add(row);
            }
            return schemaTable;
        }

        public override Task<DataTable> GetSchemaTableAsync(CancellationToken cancellationToken = default)
        {
            return Task.Run(GetSchemaTable, cancellationToken);
        }
    }

    public class FakeDbCommand : DbCommand
    {
        private readonly FakeDbDataReader _reader;
        public FakeDbCommand(FakeDbDataReader reader)
        {
            _reader = reader;
        }

        public override string CommandText { get; set; }
        public override int CommandTimeout { get; set; }
        public override CommandType CommandType { get; set; }
        public override bool DesignTimeVisible { get; set; }
        public override UpdateRowSource UpdatedRowSource { get; set; }
        protected override DbConnection DbConnection { get; set; }

        protected override DbParameterCollection DbParameterCollection { get; }

        protected override DbTransaction DbTransaction { get; set; }

        public override void Cancel()
        {
        }

        public override int ExecuteNonQuery()
        {
            return 1;
        }

        public override object ExecuteScalar()
        {
            throw new NotImplementedException();
        }

        public override void Prepare()
        {
        }

        protected override DbParameter CreateDbParameter()
        {
            return new FakeDbParameter();
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            return _reader;
        }

        private class FakeDbParameter : DbParameter
        {
            public override DbType DbType { get; set; }
            public override ParameterDirection Direction { get; set; }
            public override bool IsNullable { get; set; }
            public override string ParameterName { get; set; }
            public override int Size { get; set; }
            public override string SourceColumn { get; set; }
            public override bool SourceColumnNullMapping { get; set; }
            public override object Value { get; set; }

            public override void ResetDbType()
            {
            }
        }
    }
}
