using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace TeamListAPI.Models
{
    //declare a team model with an id, name(required), location(required), and a list of all players on the team.
    public class Team 
    {
        public long Id { get; set; }
        [Required(ErrorMessage ="Team Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Team Location is required")]
        public string Location { get; set; }
        public int playerCount { get; set; }
        public ICollection<Player> PlayerList { get; set; }

        public Team()
        {
            PlayerList = new List<Player>();
        }
    }
}
