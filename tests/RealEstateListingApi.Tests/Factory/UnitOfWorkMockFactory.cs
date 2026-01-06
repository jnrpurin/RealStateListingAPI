using Moq;
using RealEstateListingApi.UnitOfWork;
using RealEstateListingApi.Repo;
using RealEstateListingApi.Models;

namespace RealEstateListingApi.Tests.Factory
{
    public static class UnitOfWorkMockFactory
    {
        public static Mock<IUnitOfWork> Create(Listing listing)
        {
            var repoMock = new Mock<IListingRepo>();
            repoMock.Setup(r => r.GetById(It.IsAny<Guid>()))
                    .Returns(listing);

            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(u => u.Listings)
                   .Returns(repoMock.Object);

            return uowMock;
        }
    }
}
