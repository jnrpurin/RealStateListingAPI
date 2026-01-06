using RealEstateListingApi.Repo;

namespace RealEstateListingApi.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IListingRepo Listings { get; }
        int Commit();
    }
}
