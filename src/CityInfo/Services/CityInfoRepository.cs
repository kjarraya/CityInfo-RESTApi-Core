using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private CityInfoContext _context;
        public CityInfoRepository(CityInfoContext context)
        {
            _context = context;
        }

        public bool CitiesExists(int cityId)
        {
            return _context.cities.Any(c => c.Id == cityId);
        }

        public IEnumerable<City> GetCities()
        {
            return _context.cities.OrderBy(c => c.Name).ToList();
        }

        public City GetCity(int id, bool includePointOfInterest)
        {
            if (includePointOfInterest)
            {
                return _context.cities.Include(c => c.PointsOfIntersest).Where(c => c.Id == id).FirstOrDefault();

            }
            return _context.cities.Where(c => c.Id == id).FirstOrDefault();
        }

        public PointOfIntersect GetPointOfInterestForCity(int cityId, int pointOfIntersectId)
        {
            return _context.PointsOfIntersect.Where(p => p.CityId == cityId && p.Id == pointOfIntersectId).FirstOrDefault();
        }

        public IEnumerable<PointOfIntersect> GetPointsOfInterestForCity(int cityId)
        {
            return _context.PointsOfIntersect.Where(p => p.CityId == cityId).ToList();
        }
        public void AddPointOfInterestForCity(int cityId, PointOfIntersect pointOfIntersect)
        {
            var city = GetCity(cityId, false);
            city.PointsOfIntersest.Add(pointOfIntersect);

        }
        public bool Save()
        {
            return _context.SaveChanges() >= 0;
        }

        public void DeletePointOfInterest(PointOfIntersect pointOfIntersect)
        {
            _context.PointsOfIntersect.Remove(pointOfIntersect);
        }
    }
}
