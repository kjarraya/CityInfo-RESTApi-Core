using AutoMapper;
using CityInfo.Entities;
using CityInfo.Models;
using CityInfo.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.Controllers
{
    [Route("api/cities")]
    public class PointsOfInterestController : Controller
    {
        private ILogger<PointsOfInterestController> _logger;
        private IMailServiceInterface _mailService;
        private ICityInfoRepository _cityInfoRepository;
        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailServiceInterface mailService, ICityInfoRepository cityInfoRepository)
        {
            _logger = logger;
            _mailService = mailService;
            _cityInfoRepository = cityInfoRepository;
        }

        [HttpGet("{cityid}/PointsOfInterest")]
        public IActionResult GetPointsOfInterest(int cityid)
        {
            try
            {
                if (!_cityInfoRepository.CitiesExists(cityid))
                {
                    _logger.LogInformation($"city with id {cityid} wasn't found when accessing points of interest.");
                    return NotFound();
                }
                var pointsOfInterest = _cityInfoRepository.GetPointsOfInterestForCity(cityid);
                var pointsOfInterestResut = Mapper.Map<IEnumerable<PointOfIntersectDto>>(pointsOfInterest);

                return Ok(pointsOfInterestResut);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting  points of interest for city with id {cityid}.", ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }

        }

        [HttpGet("{cityid}/PointsOfInterest/{id}", Name = "GetPointsOfInterest")]
        public IActionResult GetPointsOfInterest(int cityid, int id)
        {
            if (!_cityInfoRepository.CitiesExists(cityid))
            {
                return NotFound();
            }

            var pointsOfIntersest = _cityInfoRepository.GetPointOfInterestForCity(cityid, id);
            if (pointsOfIntersest == null)
            {
                return NotFound();
            }

            var pointsOfIntersestResult = Mapper.Map<PointOfIntersectDto>(pointsOfIntersest);
            return Ok(pointsOfIntersestResult);

        }
        [HttpPost("{cityid}/PointsOfInterest")]
        public IActionResult CreatePointsOfInterest(int cityid, [FromBody] PointOfIntersectForCreationDto pointsOfInterest)
        {
            if (pointsOfInterest == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_cityInfoRepository.CitiesExists(cityid))
            {
                return NotFound();
            }

            var finalpointsOfInterest = Mapper.Map<PointOfIntersect>(pointsOfInterest);
            _cityInfoRepository.AddPointOfInterestForCity(cityid, finalpointsOfInterest);
            if (!_cityInfoRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling uour request.");
            }

            var createdPointOfInterestToReturn = Mapper.Map<PointOfIntersectDto>(finalpointsOfInterest);
            return CreatedAtRoute("GetPointsOfInterest", new { cityid = cityid, id = createdPointOfInterestToReturn.Id }, createdPointOfInterestToReturn);

        }

        [HttpPatch("{cityid}/PointsOfInterest/{id}")]
        public IActionResult PartiallyUpdatePointsOfInterest(int cityid, int id, [FromBody] JsonPatchDocument<PointOfIntersectForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }
            if (!_cityInfoRepository.CitiesExists(cityid))
            {
                return NotFound();
            }
            var pointOfInterestToEntity = _cityInfoRepository.GetPointOfInterestForCity(cityid, id);
            if (pointOfInterestToEntity == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = Mapper.Map<PointOfIntersectForUpdateDto>(pointOfInterestToEntity);

            patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (pointOfInterestToEntity.Description == pointOfInterestToEntity.Name)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the name.");
            }

            TryValidateModel(pointOfInterestToEntity);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mapper.Map(pointOfInterestToPatch, pointOfInterestToEntity);
            if (!_cityInfoRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling uour request.");
            }
            return NoContent();
        }

        [HttpPut("{cityid}/PointsOfInterest/{id}")]
        public IActionResult UpdatePointsOfInterest(int cityid, int id, [FromBody] PointOfIntersectForCreationDto pointsOfInterest)
        {

            if (pointsOfInterest == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (pointsOfInterest.Description == pointsOfInterest.Name)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the name.");
            }

            if (_cityInfoRepository.CitiesExists(cityid))
            {
                return NotFound();
            }

            var pointOfInterestFromStore = _cityInfoRepository.GetPointOfInterestForCity(cityid, id);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }
            Mapper.Map(pointsOfInterest, pointOfInterestFromStore);
            if (!_cityInfoRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling uour request.");
            }

            return NoContent();

        }

        [HttpDelete("{cityid}/PointsOfInterest/{id}")]
        public IActionResult DeletePointsOfInterest(int cityid, int id)
        {
            if (!_cityInfoRepository.CitiesExists(cityid))
            {
                return NotFound();
            }

            var pointsOfInterestEntity = _cityInfoRepository.GetPointOfInterestForCity(cityid, id);
            if (pointsOfInterestEntity == null)
            {
                return NotFound();
            }
            _cityInfoRepository.DeletePointOfInterest(pointsOfInterestEntity);

            if (!_cityInfoRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling uour request.");
            }

            _mailService.send("point of ineterset deleted", $"point of interest {pointsOfInterestEntity.Name} with id {pointsOfInterestEntity.Id} was deleted.");
            return NoContent();
        }
    }
}
