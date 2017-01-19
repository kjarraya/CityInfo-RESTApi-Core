using CityInfo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo
{
    public static class CityInfoExtensions
    {
        public static void EnsureSeedDataForContext(this CityInfoContext context)
        {
            if (context.cities.Any())
            {
                return;
            }
            var cities = new List<City>()
            {
                new City()
                {
                    Name="Sfax",
                    Description="Ville de Sfax",
                    PointsOfIntersest = new List<PointOfIntersect>()
                    {
                        new PointOfIntersect()
                        {
                            Name="Sfax Point1",
                            Description = "Description Sfax Point1"
                                                },
                        new PointOfIntersect()
                        {
                             Name="Sfax Point2",
                            Description = "Description Sfax Point2"
                        }

                    }
                },
                new City()
                {
                    Name="Tunis",
                    Description="Ville de Tunis",
                    PointsOfIntersest = new List<PointOfIntersect>()
                    {
                        new PointOfIntersect()
                        {
                            Name="Tunis Point1",
                            Description = "Description Tunis Point1"
                                                },
                        new PointOfIntersect()
                        {
                             Name="Tunis Point2",
                            Description = "Description Tunis Point2"
                        }

                    }
                }
            };
            context.cities.AddRange(cities);
            context.SaveChanges();
        }
    }
}
