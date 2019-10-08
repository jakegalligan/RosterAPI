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
        //----------------------POST ROUTES--------------------

        //post route to create a team
        [HttpPost("team")]
        public async Task<ActionResult<Team>> CreateTeam(Team team)
        {
            //save team to Teams set in teamlistcontext 
            context.Teams.Add(team);
            await context.SaveChangesAsync();
            return team;

        }
        //post route to create a player
        [HttpPost("player")]
        public async Task<ActionResult<Player>> CreatePlayer(Player player)
        {
            //save player to Players set in playerlistcontext 
            context.Players.Add(player);
            await context.SaveChangesAsync();
            return player;
        }
        //----------------------GET ROUTES--------------------

        //get route to get list of all teams in sorted or nonsorted order
        [HttpGet("team")]
        public async Task<ActionResult<IEnumerable<Team>>> GetTeams(string sortBy="N/A") 
        {
            switch (sortBy.ToLower())
            {
                //return all teams regardless of order
                case "n/a":
                    return await context.Teams.ToListAsync();
                //return all teams ordered by location
                case "location":
                    return await context.Teams.OrderBy(team => team.Location).ToListAsync();
                //return all teams ordered by Name
                case "name":
                    return await context.Teams.OrderBy(team => team.Name).ToListAsync();
                default:
                    return NotFound("Improper search query");
            }
            
        }

        //get route to get a specific team
        [HttpGet("team/{id}")]
       public async Task<ActionResult<Team>> GetTeamById (long id)
        {
            //get specific team from set of Teams
            var team = await context.Teams.FindAsync(id);
            //if the team is undefined return an error as team isn't in Team List
            if (team == null)
            {
                return NotFound();
            }
            //else return the team
            return team;
        }

        //get route for all players
        [HttpGet("player")]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers(string lastName)
        {   
            //if there is no lastname query return all players
            if (lastName == null)
            {
                return await context.Players.ToListAsync();
       
            //if lastName query is specified return all players that match the given lastName
            } else {
                return await context.Players.Where(player => player.LastName == lastName).ToListAsync();
            }
        }

        //get route to get a specific player
        [HttpGet("player/{id}")]
        public async Task<ActionResult<Player>> GetPlayerById(long id)
        {
            //get specific palyer from set of players
            var player = await context.Players.FindAsync(id);
            //if the player is undefined return an error 
            if (player == null)
            {
                return NotFound("No player matching given id");
            }
            //else return the player
            return player;
        }

  


    }
}
