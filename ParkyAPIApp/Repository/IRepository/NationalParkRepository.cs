using System.Collections.Generic;
using ParkyAPIApp.Models;

namespace ParkyAPIApp.Repository.IRepository
{
    public class NationalParkRepository : INationalParkRepository
    {
        public ICollection<NationalPark> GetNationalParks()
        {
            throw new System.NotImplementedException();
        }

        public NationalPark GetNationalPark(int nationalParkId)
        {
            throw new System.NotImplementedException();
        }

        public bool NationalParkExists(string name)
        {
            throw new System.NotImplementedException();
        }

        public bool NationalParkExits(int id)
        {
            throw new System.NotImplementedException();
        }

        public bool CreateNationalPark(NationalPark nationalPark)
        {
            throw new System.NotImplementedException();
        }

        public bool UpdateNationalPark(NationalPark nationalPark)
        {
            throw new System.NotImplementedException();
        }

        public bool DeleteNationalPark(NationalPark nationalPark)
        {
            throw new System.NotImplementedException();
        }

        public bool Save()
        {
            throw new System.NotImplementedException();
        }
    }
}
