using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly ILogger<BuggyController> _logger;

        public BuggyController(ILogger<BuggyController> logger)
        {
            _logger = logger;
        }

        [HttpGet("auth")]
        public IActionResult GetAuth()
        {
            return Unauthorized();
        }

        [HttpGet("not-found")]
        public IActionResult GetNotFound()
        {
            return NotFound();
        }

        [HttpGet("server-error")]
        public IActionResult GetServerError()
        {
            throw new Exception("Server error");
        }


        [HttpGet("bad-request")]
        public IActionResult GetBadRequest()
        {
            return BadRequest("bad request");
        }

    }
}