using VRPTW.Domain.Entity;

namespace VRPTW.Domain.Interface.Repository
{
	public interface ICeplexRepository
	{
		bool[][] SolveFractionedTrips(CeplexParameters ceplexParameters);
	}
}
