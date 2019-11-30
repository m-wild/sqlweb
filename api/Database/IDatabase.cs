using System;
using SqlWeb.Types;

namespace SqlWeb.Database
{
    public interface IDatabase
    {
        SchemaObjects Objects();

        TableInfo TableInfo(string table);
    }
}