using System.Data.SqlClient;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SqlWeb.Database;
using SqlWeb.Database.SqlServer;
using SqlWeb.Types;

namespace SqlWeb
{
    [ApiController]
    [Route("/api")]
    public class Controller : ControllerBase
    {
        private readonly ISessionStore sessionStore;
        private readonly IDatabaseFactory databaseFactory;
        private readonly ILogger logger;

        public Controller(ILoggerFactory logger, ISessionStore sessionStore, IDatabaseFactory databaseFactory)
        {
            this.sessionStore = sessionStore;
            this.databaseFactory = databaseFactory;
            this.logger = logger.CreateLogger("sqlweb");
        }

        private ISession GetSession()
        {
            var sessionId = Request.Headers.TryGetValue("x-session-id", out var sessionIdValues)
                ? SessionId.Parse(sessionIdValues.FirstOrDefault())
                : null;

            return sessionStore.GetSession(sessionId);
        }

        private IActionResult NotConnected() => BadRequest(new {status = 400, error = "Not connected"});

        [HttpPost, Route("connect")]
        public IActionResult Connect()
        {
            // dummy connection
            var builder = new SqlConnectionStringBuilder("Server=localhost,1433; Database=Testing; User ID=sa; Password=SQLServer2017;");
            var session = new SqlServerSession(builder);
            var id = sessionStore.SaveSession(session);
            
            return Ok(new
            {
                SessionId = id.ToString(),
            });
        }
        
       
        [HttpPost, Route("query")]
        public IActionResult RunQuery([FromForm] string query)
        {
            var session = GetSession();
            if (session == null)
            {
                return NotConnected();
            }
            
            logger.LogInformation(query);
            
            var results = new Result();
            results.Columns.AddRange(new []{"id", "display_name"});
            results.Rows.AddRange(new []
            {
                new object[]{ 1, "Hello" },
                new object[]{ 2, "World" },
                new object[]{ 666, "Goodbye" },
            });

            return Ok(results);
        }


        [HttpGet, Route("objects")]
        public IActionResult GetObjects()
        {
            var session = GetSession();
            if (session == null)
            {
                return NotConnected();
            }

            var database = databaseFactory.Database(session);
            
            var objects = database.Objects();

            return Ok(objects);
        }

        [HttpGet, Route("tables/{table}")]
        public IActionResult GetTableDefinition([FromRoute] string table)
        {
            var session = GetSession();
            if (session == null)
            {
                return NotConnected();
            }

            var database = databaseFactory.Database(session);

            var tableDefinition = database.TableDefinition(table);

            return Ok(tableDefinition);
        }
        
        [HttpGet, Route("tables/{table}/info")]
        public IActionResult GetTableInfo([FromRoute] string table)
        {
            var session = GetSession();
            if (session == null)
            {
                return NotConnected();
            }

            var database = databaseFactory.Database(session);

            var tableInfo = database.TableInfo(table);

            return Ok(tableInfo);
        }

        [HttpGet, Route("tables/{table}/rows")]
        public IActionResult GetTableRows([FromRoute] string table, [FromQuery] RowsOptions opts)
        {
            var session = GetSession();
            if (session == null)
            {
                return NotConnected();
            }

            var database = databaseFactory.Database(session);

            var tableRows = database.TableRows(table, opts);

            var numRows = database.TableRowsCount(table, opts);

            var numPages = numRows / opts.Limit;
            if (numPages * opts.Limit < numRows)
            {
                numPages++;
            }
            
            tableRows.Pagination = new Pagination
            {
                Rows = numRows,
                Page = (opts.Offset / opts.Limit) + 1,
                Pages = numPages,
                PerPage = opts.Limit,
            };

            return Ok(tableRows);
        }
    }
}
