using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class MembersController : BaseApiController
    {
        private readonly ILogger<MembersController> _logger;
        private readonly AppDBContext context;

        public MembersController(ILogger<MembersController> logger, AppDBContext context)
        {
            _logger = logger;
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AppUser>>> GetMembers()
        {
            var members = await context.Users.ToListAsync();

            return members;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetMember(string id)
        {
            var member = await context.Users.FindAsync(id);

            if (member == null)
                return NotFound();

            return member;
        }
    }
}