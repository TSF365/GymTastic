using GymTastic.DataAccess.Data;
using GymTastic.DataAccess.Repository.IRepository;
using GymTastic.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTastic.DataAccess.Repository
{
    public class PreferenceRepository : Repository<Preferences>, IPreferenceRepository
    {
        private readonly ApplicationDbContext _db;
        public PreferenceRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Preferences preferences)
        {
            _db.Preferences.Update(preferences);
        }
    }
}
