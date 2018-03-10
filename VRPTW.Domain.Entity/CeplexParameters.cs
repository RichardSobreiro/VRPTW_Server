﻿namespace VRPTW.Domain.Entity
{
	public class CeplexParameters
	{
		public int QuantityOfVehiclesAvailable { get; set; }
		public int QuantityOfClients { get; set; }
		public int VehiclesGreatestPossibleDemand { get; set; }
		public int GreatestPossibleDemand { get; set; }
		public double[][] Time { get; set; }
		public int[] VehicleCapacity { get; set; }
		public int[] ClientsDemand { get; set; }
	}
}
