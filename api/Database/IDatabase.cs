using SqlWeb.Types;

namespace SqlWeb.Database
{
    public interface IDatabase
    {
        SchemaObjects Objects();

        TableInfo TableInfo(string table);

        Result TableDefinition(string table);

        Result TableRows(string table, RowsOptions opts);

        long TableRowsCount(string table, RowsOptions opts);
    }
}