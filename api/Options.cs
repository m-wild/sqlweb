namespace SqlWeb
{
    public class Options
    {
        public bool ReadOnly { get; set; }

        public string ResourceStoreType { get; set; } = "sqlserver";

        public string SqlServerConnectionString { get; set; } = "Data Source=localhost; Initial Catalog=sqlweb; User ID=sa; Password=SQLServer2017;";
    }
}