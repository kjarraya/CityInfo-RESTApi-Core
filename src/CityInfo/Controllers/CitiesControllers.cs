using AutoMapper;
using CityInfo.Models;
using CityInfo.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.Controllers
{
    [Route("api/cities")]
    public class CitiesControllers : Controller
    {
        private ICityInfoRepository _cityInfoRepository;
        public CitiesControllers(ICityInfoRepository cityInfoRepository)
        {
            _cityInfoRepository = cityInfoRepository;
        }

        [HttpGet()]
        public IActionResult GetCities()
        {

            var cityEntities = _cityInfoRepository.GetCities();

            var results = Mapper.Map<IEnumerable<CityWithoutPointsOfIntersestDto>>(cityEntities);
            return Ok(results);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id, bool includePointsOfIntersest = false)
        {
            var city = _cityInfoRepository.GetCity(id, includePointsOfIntersest);

            if (city == null)
            {
                return NotFound();
            }

            if (includePointsOfIntersest)
            {
                var cityResult = Mapper.Map<CityDto>(city);
                return Ok(cityResult);
            }

            var result = Mapper.Map<CityWithoutPointsOfIntersestDto>(city); 
            return Ok(result);

        }



    }
}
