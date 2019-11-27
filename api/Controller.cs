using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SqlWeb.Types;

namespace SqlWeb
{
    [ApiController]
    [Route("/api")]
    public class Controller : ControllerBase
    {
        private readonly ILogger logger;

        public Controller(ILoggerFactory logger)
        {
            this.logger = logger.CreateLogger("sqlweb");
        }
       
        [HttpPost]
        [Route("query")]
        public IActionResult RunQuery([FromBody] RunQueryRequest request)
        {
            logger.LogInformation(request?.Query);

            var results = new[] {
                new FakeData(1, "Hello", "world"),
                new FakeData(2, "Something", "blah"),
            };

            return Ok(results);
        }


        [HttpGet, Route("objects")]
        public Dictionary<string, Objects> GetObjects()
        {
            using (var conn = new SqlConnection("Server=localhost,1433; Database=Testing; User ID=sa; Password=SQLServer2017;"))
            {
                conn.Open();
                
                var objects = new Dialect.SqlServer.SqlServer()
                    .GetAllObjects(conn)
                    .ToObjects();

                return objects;
            }
            
        }
    }



    public class FakeData
    {
        public FakeData(int id, string name, string foo)
        {
            Id = id; Name = name; Foo = foo;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Foo { get; set; }
    }


    public class RunQueryRequest {
        public string Query { get; set; }
    }

    
}
