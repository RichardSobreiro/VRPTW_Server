using VRPTW.Domain.Entity;

namespace VRPTW.Domain.Interface.Repository
{
	public interface ICeplexRepository
	{
		int[][][] SolveFractionedTrips(CeplexParameters ceplexParameters);
	}
}
