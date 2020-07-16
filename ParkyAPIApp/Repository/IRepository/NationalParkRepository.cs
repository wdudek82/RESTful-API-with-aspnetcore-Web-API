using System;
using System.Collections.Generic;
using System.Linq;
using ParkyAPIApp.Data;
using ParkyAPIApp.Models;

namespace ParkyAPIApp.Repository.IRepository
{
    public class NationalParkRepository : INationalParkRepository
    {
        private readonly ApplicationDbContext _db;

        public NationalParkRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public ICollection<NationalPark> GetNationalParks()
        {
            return _db.NationalParks
                .OrderBy(p => p.Name)
                .ToList();
        }

        public NationalPark GetNationalPark(int nationalParkId)
        {
            return _db.NationalParks.FirstOrDefault(p => p.Id == nationalParkId);
        }

        public bool NationalParkExists(string name)
        {
            var value = _db.NationalParks.Any(p =>
                string.Equals(p.Name.Trim(), name.Trim(), StringComparison.CurrentCultureIgnoreCase));
            return value;
        }

        public bool NationalParkExits(int id)
        {
           return _db.NationalParks.Any(p => p.Id == id);
        }

        public bool CreateNationalPark(NationalPark nationalPark)
        {
            _db.NationalParks.Add(nationalPark);
            return Save();
        }

        public bool UpdateNationalPark(NationalPark nationalPark)
        {
            _db.NationalParks.Update(nationalPark);
            return Save();
        }

        public bool DeleteNationalPark(NationalPark nationalPark)
        {
            _db.NationalParks.Remove(nationalPark);
            return Save();
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0;
        }
    }
}
