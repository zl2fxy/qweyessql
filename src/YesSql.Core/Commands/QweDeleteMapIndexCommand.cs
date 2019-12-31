using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using Dapper;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace YesSql.Commands
{
    //{{ * qwe新增修改
    public class QweDeleteMapIndexCommand : IIndexCommand
    {
        private readonly int _documentId;
        private readonly string _tableName;
        private readonly string _tablePrefix;

        public int ExecutionOrder { get; } = 1;

        public QweDeleteMapIndexCommand(string tableName, int documentId, string tablePrefix, ISqlDialect dialect)
        {
            _tableName = tableName;
            _documentId = documentId;
            _tablePrefix = tablePrefix;
        }

        public Task ExecuteAsync(DbConnection connection, DbTransaction transaction, ISqlDialect dialect, ILogger logger)
        {
            var command = "delete from " + dialect.QuoteForTableName(_tablePrefix + _tableName) + " where " + dialect.QuoteForColumnName("DocumentId") + " = @Id";
            logger.LogTrace(command);
            return connection.ExecuteAsync(command, new { Id = _documentId }, transaction);
        }
    }
    //修改结束 * }}
}
