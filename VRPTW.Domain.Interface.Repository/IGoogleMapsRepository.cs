using VRPTW.Domain.Entity;

namespace VRPTW.Domain.Interface.Repository
{
	public interface IGoogleMapsRepository
	{
		double? GetDistanceBetweenTwoAddresses(Address addressOrigin, Address addressDestination);
		void GetLatirudeAndLogitudeOfAnAddress(Address address);
	}
}
