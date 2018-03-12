using VRPTW.Domain.Entity;

namespace VRPTW.Domain.Interface.Repository
{
	public interface ICeplexRepository
	{
		void SolveFractionedTrips(CeplexParameters ceplexParameters);
	}
}
