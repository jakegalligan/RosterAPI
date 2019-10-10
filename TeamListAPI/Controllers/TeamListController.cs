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
            //check to see if team with location and name already exists
            var duplicateTeam = context.Teams.Where(t => t.Name == team.Name && t.Location == team.Location).FirstOrDefault();
            //if there is a duplicateTeam return an error
            if (duplicateTeam != null)
            {
                return BadRequest("Team already exists");
            } else
            {
                //otherwise save team to database 
                context.Teams.Add(team);
                await context.SaveChangesAsync();
                return team;
            }

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
                    return BadRequest("Improper search query");
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
                return BadRequest();
            }
            //else return the team
            return team;
        }

        //get all plyers on a team
        [HttpGet("team/{id}/players")]
        public async Task<ActionResult<List<Player>>> GetPlayersOnTeam (long id)
        {
            //get all players that play for a specified team
            var players = context.Players.Where(x => x.TeamId == id);

            //if the team is undefined return an error as team isn't in Team List
            if (players == null)
            {
                return BadRequest();
            }
            //else return the players on the team
            return Ok(players);
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
                return BadRequest("No player matching given id");
            }
            //else return the player
            return player;
        }
        //----------------------PUT ROUTES--------------------
        //put route to remove or add players to a team
        [HttpPut("team/{teamId}/player/{playerId}")]
        public async Task<ActionResult> AddOrRemovePlayerFromTeam (long teamId, long playerId, string action)
        {

            //see if player and teams exist
            Team team =  await context.Teams.FindAsync(teamId);
            Player player =  await context.Players.FindAsync(playerId);
            if (team == null || player == null)
            {
                return BadRequest("Unable to find team or player");
            }

            //check if user wants to add or remove player
            switch (action.ToLower())
            {
                case "remove":
                    //if player isn't on team return error
                    if (player.TeamId != team.Id)
                        {
                            return BadRequest("Player not found on team");
                        }
                    //remove player from team
                     player.TeamId = 0;
                     team.PlayerCount -= 1;
                     context.SaveChanges();
                    //return a response showing the player and team which the player was removed from
                     return Ok("Removed " + player.FirstName + " " + player.LastName + " from " + team.Name);

                case "add":
                    //check if the player is currently on another team
                    if (player.TeamId != null && player.TeamId != team.Id)
                    {
                        return BadRequest("Player already on team, please remove player from current team");
                    }
                    //if player is already on team return error
                    if (player.TeamId == team.Id) 
                        {
                            return BadRequest("Player already on team");
                        }
                    //if team has more 8 players already return error
                        if (team.PlayerCount == 8)
                        {
                            return BadRequest("Unable to add player, roster limit of 8 exceeded");
                        }
                    //add player to team
                    player.TeamId= team.Id;
                    //increment count for team
                    team.PlayerCount += 1;
                    await context.SaveChangesAsync();
                    return Ok("Added " + player.FirstName + " " + player.LastName + " to " + team.Name);

                default:
                    return BadRequest("Improper search query");
            }
        }
    }
}
