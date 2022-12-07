using Pustok.DAL;
using Pustok.Models;
using System.Collections.Generic;
using System.Linq;

namespace Pustok.Sevices
{
    public class LayoutService
    {
        private readonly PustokDbContext _context;
        public LayoutService(PustokDbContext context)
        {
            _context = context;
        }
        public Dictionary<string, string> GetSettings()
        {
            return _context.Settings.ToDictionary(x=> x.Key, x=> x.Value);
        }
        public List<Genre> GetGenres()
        {
            return _context.Genres.ToList();
        }
    }
}
