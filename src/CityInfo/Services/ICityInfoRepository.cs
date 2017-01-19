using CityInfo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.Services
{
    public interface ICityInfoRepository
    {
        IEnumerable<City> GetCities();
        City GetCity(int id, bool includePointOfInterest);

        bool CitiesExists(int cityId);
        IEnumerable<PointOfIntersect> GetPointsOfInterestForCity(int cityId);
        PointOfIntersect GetPointOfInterestForCity(int cityId, int pointOfIntersectId);
        void AddPointOfInterestForCity(int cityId, PointOfIntersect pointOfIntersect);
        bool Save();
        void DeletePointOfInterest(PointOfIntersect pointOfIntersect);

    }
}
