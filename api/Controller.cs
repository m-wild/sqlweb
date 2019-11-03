using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace sqlweb.Controllers
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
