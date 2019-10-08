using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamListAPI.Models
{
    //declare a Player model which contains an Id, firstName, and lastName
    public class Player
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
