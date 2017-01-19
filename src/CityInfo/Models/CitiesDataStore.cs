using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.Models
{
    public class CitiesDataStore
    {
        public static CitiesDataStore Current { get; } = new CitiesDataStore();
        public List<CityDto> Cities { get; set; }
        public CitiesDataStore()
        {
            Cities = new List<CityDto>()
            {
                new CityDto { Id = 1, Name = "tunis", Description = "Capitale de la tunisie" ,

                    PointsOfIntersest = new List<PointOfIntersectDto>()
                        {
                            new PointOfIntersectDto { Id = 1, Name = "tunis", Description = "Capitale de la tunisie" },
                            new PointOfIntersectDto { Id = 2, Name = "sfax", Description = "Capitale de la sfax" }
                    } },
                new CityDto { Id = 2, Name = "Sfax", Description = "Capitale de la tunisie"

                     },
                new CityDto { Id = 3, Name = "Sousse", Description = "Capitale de la Sousse" 

                   
                     }

            };
        }
    }
}
