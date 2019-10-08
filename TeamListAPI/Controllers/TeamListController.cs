using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamListAPI.Models;

namespace TeamListAPI.Controllers
{
    //create route for api and make api respond to webApi requests
    [Route("api/")]
    [ApiController]
    public class TeamListController : ControllerBase
    {
        private readonly TeamListContext context;

        public TeamListController(TeamListContext _context)
        {
            context = _context;
        }

        //post route to create a team
        [HttpPost("team")]
        public async Task<ActionResult<Team>> CreateTeam(Team team)
        {
            //save team to teams set in teamlistcontext 
            context.Teams.Add(team);
            await context.SaveChangesAsync();
            return team;

        }
        //post route to create a player
        [HttpPost("player")]
        public async Task<ActionResult<Player>> CreatePlayer(Player player)
        {
            //save player to players set in playerlistcontext 
            context.Players.Add(player);
            await context.SaveChangesAsync();
            return player;
        }
    }
}
