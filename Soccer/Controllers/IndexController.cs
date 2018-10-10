using Soccer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Soccer.Controllers
{
    public class IndexController : Controller
    {
        // GET: Index
        public ActionResult Index()
        {

            //Initialize variables
            List<SoccerTeam> listSoccerTeams = new List<SoccerTeam>();
            List<SoccerTeam> listSorted = new List<SoccerTeam>();
            string addtoDOM = "";
            int teamRank = 0;

            //Load data
            listSoccerTeams.Add(new SoccerTeam("Denver", 20));
            listSoccerTeams.Add(new SoccerTeam("Salt Lake City", 25));
            listSoccerTeams.Add(new SoccerTeam("San Jose", 10));
            listSoccerTeams.Add(new SoccerTeam("Dallas", 25));

            //Sort List
            listSorted = listSoccerTeams.OrderByDescending(x => x.teamPoints).ToList();

            //Print data
            addtoDOM += "<table>";
            addtoDOM += "<tr>";
            addtoDOM += "<td>Rank</td>";
            addtoDOM += "<td>Team Name</td>";
            addtoDOM += "<td>Points</td>";
            addtoDOM += "</tr>";

            foreach ( SoccerTeam team in listSorted)
            {

                addtoDOM += "<tr>";
                addtoDOM += "<td>"+ ++teamRank +"</td>";
                addtoDOM += "<td>" + team.teamName + "</td>";
                addtoDOM += "<td>" + team.teamPoints + "</td>";
                addtoDOM += "</tr>";

            }

            //Add data to ViewBag
            ViewBag.printer = addtoDOM;

            return View();
        }
    }
}