using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLab6.Model
{
    internal class Visit
    {
        public int ClientId { get; set; }
        public Client Client { get; set; } = new Client();

        public int BuildingId { get; set; }
        public ClimbingBuilding ClimbingBuilding { get; set; } = new ClimbingBuilding();
        public DateTime VisitDate { get; set; } = DateTime.Now;
    }
}
