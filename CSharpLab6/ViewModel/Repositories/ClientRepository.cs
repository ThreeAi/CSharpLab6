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
    internal class ClientRepository
    {

        private readonly MyDbContext _context;

        public ClientRepository()
        {
            _context = new MyDbContext();
        }

        public void AddClient(Client client)
        {
            _context.Clients.Add(client);
            _context.SaveChanges();
        }

        public List<Client> GetAllClients()
        {
            return _context.Clients.ToList();
        }

        public void UpdateClient(Client client)
        {
            _context.Entry(client).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteClient(Client client)
        {
            if(_context.Clients.Contains(client)) { 
                _context.Clients.Remove(client);
            }
            _context.SaveChanges();
        }

        public async Task SaveClientsAsync(List<Client> clients)
        {
            var newClients = clients.Select(c => new Client
            {
                Name = c.Name,
                LastName = c.LastName,
                FurtherName = c.FurtherName
            }).ToList();

            await _context.Clients.AddRangeAsync(newClients);
            await _context.SaveChangesAsync();
        }
    }
}
