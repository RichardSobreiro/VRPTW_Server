namespace VRPTW.Domain.Interface.Repository
{
	public interface ICeplexRepository
	{
		void SolveFractionedTrips(int quantityOfVehiclesAvailable, int quantityOfClients, int vehiclesGreatestPossibleDemand,
			int greatestPossibleDemand, int[][] time, int[] vehicleCapacity, int[] clientsDemand);
	}
}
