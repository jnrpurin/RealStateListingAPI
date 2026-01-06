using RealEstateListingApi.Models;

namespace RealEstateListingApi.Repo
{
    public interface IListingRepo
    {
        IEnumerable<Listing> GetAll();
        Listing? GetById(Guid id);
        void Add(Listing listing);
        void Remove(Listing listing);
    }
}