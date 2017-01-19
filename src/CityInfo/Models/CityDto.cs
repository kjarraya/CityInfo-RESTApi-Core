using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.Models
{
    public class CityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description{ get; set; }
        public int NumberOfPointsOfInterest { get { return PointsOfIntersest.Count; } }
        public ICollection<PointOfIntersectDto> PointsOfIntersest { get; set; } = new List<PointOfIntersectDto>();
    }


    public class CityWithoutPointsOfIntersestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
