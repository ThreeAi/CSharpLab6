using CSharpLab6.DbConnection;
using CSharpLab6.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLab6.ViewModel.Repositories
{
    internal class VisitRepository
    {
        private readonly MyDbContext _context;

        public VisitRepository()
        {
            _context = new MyDbContext();
        }

        public void AddVisit(Visit visit)
        {
            var client = _context.Clients.FirstOrDefault(c => c.LastName == visit.Client.LastName);
            var building = _context.ClimbingBuildings.FirstOrDefault(b => b.Address == visit.ClimbingBuilding.Address);

            if (client == null || building == null)
            {
                return;
            }

            Visit newVisit = new Visit
            {
                ClientId = client.ClientId,
                BuildingId = building.BuildingId,
                VisitDate = visit.VisitDate.ToUniversalTime(),
                Client = client,
                ClimbingBuilding = building
            };

            _context.Visits.Add(newVisit);
            _context.SaveChanges();
        }

        public List<Visit> GetAllVisits()
        {
            return _context.GetVisitsWithClientsAndBuildings().ToList();
        }

        public void UpdateVisit(Visit visit)
        {
            _context.Entry(visit).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteVisit(Visit visit)
        {
            if (_context.Visits.Contains(visit))
            {
                _context.Visits.Remove(visit);
            }
            _context.SaveChanges();
        }
    }
}
