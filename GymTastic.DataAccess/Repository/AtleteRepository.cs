using GymTastic.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymTastic.DataAccess.Data;
using GymTastic.DataAccess.Repository.IRepository;

namespace GymTastic.DataAccess.Repository
{
    public class AtleteRepository : Repository<Atlete>, IAtleteRepository
    {
        private readonly ApplicationDbContext _db;
        public AtleteRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Atlete atlete)
        {
            _db.Atletes.Update(atlete);
        }
    }
}
