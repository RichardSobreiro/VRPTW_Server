using System;
using VRPTW.Domain.Entity;

namespace VRPTW.Domain.Interface.Repository
{
	public interface IGoogleMapsRepository
	{
		Tuple<double?, long> GetDistanceBetweenTwoAddressesWithCache(Address addressOrigin, Address addressDestination);
		void GetLatirudeAndLogitudeOfAnAddress(Address address);
	}
}
