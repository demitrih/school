using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Soccer.Models
{
    public class SoccerTeam : Team
    {

        public int goalsFor { get; set; }
        public int goalsAgainst { get; set; }


        public SoccerTeam()
        {

        }

        public SoccerTeam(string teamName, int teamPoints)
        {
            base.teamName = teamName;
            base.teamPoints = teamPoints;
        }
    }
}