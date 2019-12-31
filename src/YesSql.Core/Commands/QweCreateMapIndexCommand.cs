using Dapper;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using YesSql.Collections;
using YesSql.Indexes;
using Newtonsoft.Json.Linq;

namespace YesSql.Commands
{
    //{{ * qwe新增修改
    public class QweCreateMapIndexCommand : QweMapIndexCommand
    {
        public override int ExecutionOrder { get; } = 2;
        private string _tableName { get; set; } 

        public QweCreateMapIndexCommand(
            List<ContentItemHelper> entity,
            string tablePrefix,
            Document document,
            string ContentType)
            :base(entity, tablePrefix, document)
        {
            _tableName = ContentType;
        }

        public override async Task ExecuteAsync(DbConnection connection, DbTransaction transaction, ISqlDialect dialect, ILogger logger)
        {
            var documentTable = CollectionHelper.Current.GetPrefixedName(Store.DocumentTable);
            var sql = Inserts(_index, dialect, _tableName) + " " + dialect.IdentitySelectString + " " + dialect.QuoteForColumnName("Id");
            logger.LogTrace(sql);
            var IndexId = await connection.ExecuteScalarAsync<int>(sql, null, transaction);
            var command = "update " + dialect.QuoteForTableName(_tablePrefix + _tableName) + " set " + dialect.QuoteForColumnName("DocumentId") + " = @mapid where " + dialect.QuoteForColumnName("Id") + " = @Id";
            logger.LogTrace(command);
            await connection.ExecuteAsync(command, new { mapid = Document.Id, Id = IndexId }, transaction);
        }
    }
    //修改结束 * }}
}
