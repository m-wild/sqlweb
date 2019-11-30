namespace SqlWeb.Types
{
    public class RowsOptions
    {
        // Number of rows to skip
        public string Where { get; set; }

        // Number of rows to skip
        public int Offset { get; set; }

        // Number of rows to fetch
        public int Limit { get; set; }

        // Column to sort by
        public string SortColumn { get; set; }

        // Sort direction (ASC, DESC)
        public string SortOrder { get; set; }
    }
}