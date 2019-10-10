using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using java.lang;

namespace TeamListAPI.Models
{
    //declare a Player model which contains an Id, firstName(required), and lastName(required)
    public class Player
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Player FirstName is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Team Name is required")]

        public string LastName { get; set; }
        public Long TeamId { get; set; }

    }
}
