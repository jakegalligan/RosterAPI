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
        public async Task<ActionResult<IEnumerable<Team>>> GetTeams(string orderBy="N/A") 
        {
            switch (orderBy.ToLower())
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

        //get a specific team
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

        //get all plyers on a team
        [HttpGet("team/{id}/players")]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayersOnTeam (long id)
        {
            //get specific team from set of Teams
            var team = await context.Teams.FindAsync(id);
            //if the team is undefined return an error as team isn't in Team List
            if (team == null)
            {
                return NotFound();
            }
            //else return the players on the team
            return team.Players;
        } 


        //get route all players with an option to query by lastName
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

        //get a specific player
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
        //----------------------PUT ROUTES--------------------
        //put route to remove or add players to a team
        [HttpPut("team/{teamId}/player{playerId}")]
        public async Task<ActionResult> AddOrRemovePlayerFromTeam (long teamId, long playerId, string action)
        {

            //see if player and teams exist
            Team team =  context.Teams.Find(teamId);
            Player player = context.Players.Find(playerId);
            //check if user wants to add or remove player
            switch (action.ToLower())
            {
                case "remove":
                //if player isn't on team return error
                if (!team.Players.Contains(player))
                    {
                        return NotFound("Player not found on team");
                    }
                //remove player from team
                 team.Players.Remove(player);
                //return a response showing the player and team which the player was removed from
                 return Ok("Removed" + player + "from" + team);

                case "add":
                //if player is already on team return error
                if (team.Players.Contains(player))
                    {
                        return NotFound("Player already on team");
                    }
                //if team has more 8 players already return error
                    if (team.Players.Count() == 8)
                    {
                        return NotFound("Unable to add player, roster limit of 8 exceeded");
                    }
                    //add player to team
                    team.Players.Add(player);
                    return Ok("Added" + player + "from" + team);

                default:
                    return NotFound("Improper search query");
            }
        }
    }
}
