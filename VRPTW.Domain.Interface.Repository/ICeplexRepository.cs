using VRPTW.Domain.Entity;

namespace VRPTW.Domain.Interface.Repository
{
	public interface ICeplexRepository
	{
		int[][][] SolveFractionedTrips(CeplexParameters ceplexParameters, out bool optimalSolution);
		int[][] FindOptimalSequenceForSubRoutes(CeplexParameters ceplexParameters);
	}
}
