using RealEstateListingApi.Data;
using RealEstateListingApi.Models;

namespace RealEstateListingApi.Repo
{
    public class ListingRepo : IListingRepo
    {
        private readonly ApplicationDbContext _context;

        public ListingRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Listing> GetAll()
            => _context.Listings.ToList();

        public Listing? GetById(Guid id)
            => _context.Listings.FirstOrDefault(l => l.Id.Equals(id));

        public void Add(Listing listing)
            => _context.Listings.Add(listing);

        public void Remove(Listing listing)
        {
            _context.Listings.Remove(listing);
        }
    }
}