using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace YesSql.Commands
{

    //{{ * qwe新增修改
    public abstract class QweMapIndexCommand : IIndexCommand
    {
        protected readonly string _tablePrefix;
        public abstract int ExecutionOrder { get; }

        public Document Document { get; }

        public List<ContentItemHelper> _index { get; set; }

        public QweMapIndexCommand(List<ContentItemHelper> entity, string tablePrefix, Document document)
        {
            _index = entity;
            _tablePrefix = tablePrefix;
            Document = document;
        }

        public abstract Task ExecuteAsync(DbConnection connection, DbTransaction transaction, ISqlDialect dialect, ILogger logger);

        protected string Inserts(List<ContentItemHelper> entity, ISqlDialect dialect,string tableName)
        {
            string values = dialect.DefaultValuesInsert;

            var allProperties = entity;

            if (allProperties.Any())
            {
                var sbColumnList = new StringBuilder(null);

                for (var i = 0; i < allProperties.Count(); i++)
                {
                    var property = allProperties.ElementAt(i);
                    sbColumnList.Append(dialect.QuoteForColumnName(property.ColumnName));
                    if (i < allProperties.Count() - 1)
                    {
                        sbColumnList.Append(", ");
                    }
                }

                var sbParameterList = new StringBuilder(null);
                for (var i = 0; i < allProperties.Count(); i++)
                {
                    var property = allProperties.ElementAt(i);
                    sbParameterList.Append(property.ColumnValue);
                    if (i < allProperties.Count() - 1)
                    {
                        sbParameterList.Append(", ");
                    }
                }

                values = " (" + sbColumnList + ") VALUES (" + sbParameterList + ")";
            }

            var result = "INSERT INTO " + dialect.QuoteForTableName(_tablePrefix + tableName) + " " + values;

            return String.Format(result, _tablePrefix);
        }
    }


    //修改结束 * }}
}
