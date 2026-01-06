using RealEstateListingApi.Data;
using RealEstateListingApi.Repo;

namespace RealEstateListingApi.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IListingRepo Listings { get; }

        public UnitOfWork(ApplicationDbContext context, IListingRepo listings)
        {
            _context = context;
            Listings = listings;
        }

        public int Commit()
            => _context.SaveChanges();

        public void Dispose()
            => _context.Dispose();
    }
}
