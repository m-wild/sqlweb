using System.Data.SqlClient;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SqlWeb.Database;
using SqlWeb.Database.SqlServer;
using SqlWeb.Persistence;
using SqlWeb.Types;

namespace SqlWeb
{
    [ApiController]
    [Route("/api")]
    public class Controller : ControllerBase
    {
        private readonly ISessionStore sessionStore;
        private readonly IDatabaseFactory databaseFactory;
        private readonly IResourceStoreFactory resourceStoreFactory;
        private readonly ISessionFactory sessionFactory;
        private readonly ILogger logger;

        public Controller(ILoggerFactory logger, 
            ISessionStore sessionStore, 
            IDatabaseFactory databaseFactory, 
            IResourceStoreFactory resourceStoreFactory, 
            ISessionFactory sessionFactory)
        {
            this.sessionStore = sessionStore;
            this.databaseFactory = databaseFactory;
            this.resourceStoreFactory = resourceStoreFactory;
            this.sessionFactory = sessionFactory;
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
        private IActionResult QueryRequired() => BadRequest(new {status = 400, error = "Query parameter is required"});
        private IActionResult DbRequired() => BadRequest(new {status = 400, error = "Db parameter is required"});
        private IActionResult Error(string error) => BadRequest(new {status = 400, error});
        private IActionResult OkSessionId(SessionId id) => Ok(new {session_id = id.ToString()});
        
        [HttpPost, Route("connect")]
        public IActionResult Connect([FromBody] ConnectRequest request)
        {
            var session = sessionFactory.Connect(request.Engine, request.ConnectionString);            
            var id = sessionStore.SaveSession(session);

            return OkSessionId(id);
        }

        [HttpGet, Route("databases")]
        public IActionResult Databases()
        {
            var session = GetSession();
            if (session == null)
            {
                return NotConnected();
            }
            
            var database = databaseFactory.Database(session);

            var databases = database.Databases();

            return Ok(databases);
        }

        [HttpPost, Route("switchdb")]
        public IActionResult SwitchDatabase([FromQuery] string db)
        {
            if (string.IsNullOrWhiteSpace(db))
            {
                return DbRequired();
            }
            
            var session = GetSession();
            if (session == null)
            {
                return NotConnected();
            }
            
            session.SwitchDatabase(db);

            var database = databaseFactory.Database(session);

            var err = database.Test();
            if (err != null)
            {
                return Error(err);
            }
            
            return Ok();
        }
       
        [HttpGet, HttpPost, Route("query")]
        public IActionResult RunQuery([FromBody] QueryRequest request)
        {
            var query = request?.Query;
            if (string.IsNullOrWhiteSpace(query))
            {
                return QueryRequired();
            }
            
            var session = GetSession();
            if (session == null)
            {
                return NotConnected();
            }
            
            logger.LogInformation(query);

            var database = databaseFactory.Database(session);

            var (results, error) = database.RunQuery(query);
            if (error != null)
            {
                return Error(error);
            }
            
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

        [HttpGet, Route("resources")]
        public IActionResult GetResources()
        {
            // todo: ensure authenticated
            
            var resourceStore = resourceStoreFactory.ResourceStore();
            var resources = resourceStore.GetAllResources();
            return Ok(resources);
        }

        [HttpPost, Route("connect/{resourceId}")]
        public IActionResult ConnectResource([FromRoute] string resourceId)
        {
            // todo: ensure authenticated
            
            var resourceStore = resourceStoreFactory.ResourceStore();
            var resource = resourceStore.GetResource(resourceId);
            if (resource == null)
            {
                return Error($"cannot connect to resource '{resourceId}'");
            }

            var session = sessionFactory.ConnectResource(resource);
            var id = sessionStore.SaveSession(session);

            return OkSessionId(id);
        }
        
    }
}
