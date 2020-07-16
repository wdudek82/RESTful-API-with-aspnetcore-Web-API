using System.Collections.Generic;
using ParkyAPIApp.Models;

namespace ParkyAPIApp.Repository.IRepository
{
    public interface INationalParkRepository
    {
        ICollection<NationalPark> GetNationalParks();

        NationalPark GetNationalPark(int nationalParkId);

        bool NationalParkExists(string name);

        bool NationalParkExits(int id);

        bool CreateNationalPark(NationalPark nationalPark);

        bool UpdateNationalPark(NationalPark nationalPark);

        bool DeleteNationalPark(NationalPark nationalPark);

        bool Save();
    }
}
