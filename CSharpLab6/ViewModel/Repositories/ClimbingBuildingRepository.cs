using CSharpLab6.DbConnection;
using CSharpLab6.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace CSharpLab6.ViewModel.Repositories
{
    internal class ClimbingBuildingRepository
    {
        private readonly MyDbContext _context;

        public ClimbingBuildingRepository()
        {
            _context = new MyDbContext();
        }

        public void AddClimbingBuilding(ClimbingBuilding building)
        {
            _context.ClimbingBuildings.Add(building);
            _context.SaveChanges();
        }

        public List<ClimbingBuilding> GetAllClimbingBuildings()
        {
            return _context.ClimbingBuildings.ToList();
        }

        public void UpdateClimbingBuilding(ClimbingBuilding building)
        {
            _context.Entry(building).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteClimbingBuilding(ClimbingBuilding building)
        {
            if(_context.ClimbingBuildings.Contains(building))
            {
                _context.ClimbingBuildings.Remove(building);
            }
            _context.SaveChanges();
        }

        public async Task SaveClimbingBuildingsAsync(List<ClimbingBuilding> clients)
        {
            var newBuildings = clients.Select(c => new ClimbingBuilding
            {
                Type = c.Type,
                Address = c.Address,
                City = c.City
            }).ToList();

            await _context.ClimbingBuildings.AddRangeAsync(newBuildings);
            await _context.SaveChangesAsync();
        }
    }
}
