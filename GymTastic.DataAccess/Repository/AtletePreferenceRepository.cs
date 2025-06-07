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
    public class AtletePreferenceRepository : Repository<AtletePreferences>, IAtletePreferenceRepository
    {
        private readonly ApplicationDbContext _db;
        public AtletePreferenceRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(AtletePreferences atletePreferences)
        {
            _db.AtletePreferences.Update(atletePreferences);
        }
    }
}
