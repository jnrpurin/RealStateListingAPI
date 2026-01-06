using Microsoft.AspNetCore.Mvc;
using RealEstateListingApi.Data;
using RealEstateListingApi.Models;
using RealEstateListingApi.UnitOfWork;
using System.Collections.Generic;
using System.Linq;

namespace RealEstateListingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ListingsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public ListingsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Get all the listings 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Tags("Listings Retrieval")]
        public ActionResult<IEnumerable<Listing>> GetAllListings()
        {
            return Ok(_unitOfWork.Listings.GetAll());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listing"></param>
        /// <returns></returns>
        [HttpPost]
        [Tags("Listings Management")]
        public ActionResult<Listing> AddListing([FromBody] Listing listing)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _unitOfWork.Listings.Add(listing);
            _unitOfWork.Commit();

            return CreatedAtAction(nameof(GetListingById), new { id = listing.Id }, listing);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Tags("Listings Retrieval")]
        public ActionResult<Listing> GetListingById(Guid id)
        {
            var listing = _unitOfWork.Listings.GetById(id);
            if (listing == null)
                return NotFound();

            return Ok(listing);
        }

        /// <summary>
        /// Delete a listing
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteListing(Guid id)
        {
            if (id == null)
                return BadRequest("Listing id must be provided.");

            var listing = _unitOfWork.Listings.GetById(id);

            if (listing == null)
                return NotFound($"Listing with id '{id}' was not found.");

            _unitOfWork.Listings.Remove(listing);
            _unitOfWork.Commit();

            return NoContent();
        }

    }
}
