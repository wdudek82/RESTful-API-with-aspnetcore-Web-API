using System.Collections.Generic;
using ParkyAPIApp.Models;

namespace ParkyAPIApp.Repository.IRepository
{
    public interface ITrailRepository
    {
        ICollection<Trail> GetTrails();

        ICollection<Trail> GetTrailsInNationalPark(int nationalParkId);

        Trail GetTrail(int trailId);

        bool TrailExists(int trailId);

        bool TrailExists(string name);

        bool CreateTrail(Trail trail);

        bool UpdateTrail(Trail trail);

        bool DeleteTrail(Trail trail);

        bool Save();
    }
}
