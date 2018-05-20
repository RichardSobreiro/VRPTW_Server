using System;
using System.Collections.Generic;
using System.Linq;
using VRPTW.Business.Mapper;
using VRPTW.CrossCutting.Extensions;
using VRPTW.Domain.Dto;
using VRPTW.Domain.Entity;
using VRPTW.Domain.Interface.Repository;

namespace VRPTW.Business
{
	public class VehicleRouteBusiness : IVehicleRouteBusiness
	{
		public List<VehicleRouteDto> GetVehicleRoutes(VehicleRouteFilterDto filter)
		{
			var vehicleRoutes = _vehicleRouteRepository.GetVehicleRoutes(filter.CreateEntity());
			foreach (var vehicleRoute in vehicleRoutes)
			{
				foreach (var subRoute in vehicleRoute.SubRoutes)
				{
					subRoute.AddressOrigin = _addressRepository.GetAddressByAddressId(subRoute.AddressOriginId);
					subRoute.AddressDestiny = _addressRepository.GetAddressByAddressId(subRoute.AddressDestinyId);
				}
			}
			return vehicleRoutes.CreateDto();
		}

		public void ScheduleDeliveries(List<DeliveryDto> deliveriesToBeScheduled)
		{
			if (ValidateSchedulingOfDeliveries(deliveriesToBeScheduled))
			{
				ClusterDeliveriesOfTheSameClient(deliveriesToBeScheduled);
				var deliveriesToBeScheduledEntity = deliveriesToBeScheduled.CreateEntity();
				var depot = GetBestDepotToAttend(deliveriesToBeScheduled);
				var routes = ClusterFractionedTrips(depot, deliveriesToBeScheduledEntity);
				//InsertVehicleRoutes(routes, depot, deliveriesToBeScheduled[0].productType);
			}
		}

		public bool ValidateSchedulingOfDeliveries(List<DeliveryDto> deliveriesToBeScheduled)
		{
			return true;
		}

		public Depot GetBestDepotToAttend(List<DeliveryDto> deliveriesToBeScheduled)
		{
			var depots = _depotRepository.SelectDepots();
			depots[0].Address = _addressRepository.GetAddressByDepotId(depots[0].DepotId);
			return depots[0];
		}

		public VehicleRouteBusiness(IAddressRepository addressRepository, IDepotRepository depotRepository,
			IGoogleMapsRepository googleMapsRepository, ICeplexRepository ceplexRepository,
			IVehicleRepository vehicleRepository, IVehicleRouteRepository vehicleRouteRepository)
		{
			_addressRepository = addressRepository;
			_depotRepository = depotRepository;
			_googleMapsRepository = googleMapsRepository;
			_ceplexRepository = ceplexRepository;
			_vehicleRepository = vehicleRepository;
			_vehicleRouteRepository = vehicleRouteRepository;
		}

		private List<VehicleRoute> ClusterFractionedTrips(Depot depot,
			List<Delivery> deliveriesTobeScheduled)
		{
			var fractionedScheduledTrips = new List<DeliveryTruckTrip>();
			bool optimalSolution = false;
			foreach (var delivery in deliveriesTobeScheduled)
			{
				var deliveryTruckTrips = AllocateQuantityOfProductToTruckTrips(delivery);
				GetAddressByClientId(deliveryTruckTrips);
				fractionedScheduledTrips.AddRange(deliveryTruckTrips);
			}						 
			var vehicleRoutes = FindRoutes(depot, fractionedScheduledTrips, out optimalSolution);
			if (!optimalSolution)
			{
				PostProcessSubRoutes(depot, vehicleRoutes);
			}	
			return vehicleRoutes;
		}

		private List<VehicleRoute> FindRoutes(Depot depot, List<DeliveryTruckTrip> fractionedScheduledTrips, 
			out bool optimalSolution)
		{
			var ceplexParameters = GetCeplexParametersMultipleVehicleRoutingProblem(depot, fractionedScheduledTrips);
			int[][][] routeMatrix = _ceplexRepository.SolveFractionedTrips(ceplexParameters, out optimalSolution);
			var routes = GetRoutesFindedMultipleVehicleRouteProblem(depot, fractionedScheduledTrips, ceplexParameters, routeMatrix);
			FindSequenceNumberOfRoutes(routes, depot);
			return routes;
		}
		
		private void PostProcessSubRoutes(Depot depot, List<VehicleRoute> vehicleRoutes)
		{
			foreach(var vehicleRoute in vehicleRoutes)
			{
				var ceplexParameters = GetCeplexParametersVehicleRoutingProblem(depot, vehicleRoute);
				int[] sequenceVector = _ceplexRepository.FindOptimalSequenceForSubRoutes(ceplexParameters);
				GetOptimalSequenceOfClientsToVisit(depot, vehicleRoute, sequenceVector);
			}
		}

		private List<DeliveryTruckTrip> AllocateQuantityOfProductToTruckTrips(Delivery delivery)
		{
			var deliveriesTruckTrips = new List<DeliveryTruckTrip>();

			int numberOfTrips = (int)Math.Ceiling(delivery.QuantityProduct / 10);
			double quantityOfProductToDelivery = delivery.QuantityProduct;

			for (int i = 0; i < numberOfTrips; i++)
			{
				var newTrip = new DeliveryTruckTrip()
				{
					DeliveryId = delivery.DeliveryId,
					ClientId = delivery.Client.ClientId,
					ProductType = delivery.ProductType,
					QuantityProduct = GetDeliveryTruckTripQuantityOfProductToDelivery(ref quantityOfProductToDelivery)
				};
				deliveriesTruckTrips.Add(newTrip);
			}

			return deliveriesTruckTrips;
		}

		private double GetDeliveryTruckTripQuantityOfProductToDelivery(ref double remainingQuantityOfProductToDelivery)
		{
			double quantityOfProductToDelivery = 0;

			if (remainingQuantityOfProductToDelivery <= 10)
			{
				quantityOfProductToDelivery = remainingQuantityOfProductToDelivery;
			}
			else
			{
				quantityOfProductToDelivery = 10;
				remainingQuantityOfProductToDelivery -= quantityOfProductToDelivery;
			}

			return quantityOfProductToDelivery;
		}

		private void GetAddressByClientId(List<DeliveryTruckTrip> deliveryTruckTrips)
		{
			foreach (var deliveryTruckTrip in deliveryTruckTrips)
			{
				deliveryTruckTrip.Address = _addressRepository.GetAddressByClientId(deliveryTruckTrip.ClientId);
			}
		}

		private List<Depot> GetDepots()
		{
			var depots = _depotRepository.SelectDepots();
			foreach (var depot in depots)
			{
				depot.Address = _addressRepository.GetAddressByDepotId(depot.DepotId);
			}
			return depots;
		}   

		private CeplexParameters GetCeplexParametersMultipleVehicleRoutingProblem(Depot depot, 
			List<DeliveryTruckTrip> fractionedScheduledTrips)
		{
			var ceplexParameters = new CeplexParameters();

			var availablesVehicles = _vehicleRepository.GetAvailableVehiclesByDepot(depot.DepotId);
			ceplexParameters.QuantityOfVehiclesAvailable = availablesVehicles.Count;

			ceplexParameters.QuantityOfClients = fractionedScheduledTrips.Count;

			ceplexParameters.VehiclesGreatestPossibleDemand = 11;

			ceplexParameters.GreatestPossibleDemand = (int)fractionedScheduledTrips.Max(trip => trip.QuantityProduct) + 1;

			var numberOfPoints = fractionedScheduledTrips.Count + 1;
			var distancesAndDurations = GetDistancesAndDurations(depot, fractionedScheduledTrips, ceplexParameters.QuantityOfClients);
			ceplexParameters.Distance = distancesAndDurations.Item1;
			ceplexParameters.Duration = distancesAndDurations.Item2;

			ceplexParameters.VehicleCapacity = new int[ceplexParameters.QuantityOfVehiclesAvailable];
			for (int i = 0; i < ceplexParameters.QuantityOfVehiclesAvailable; i++)
			{
				ceplexParameters.VehicleCapacity[i] = 10;
			}

			ceplexParameters.ClientsDemand = new double[ceplexParameters.QuantityOfClients];
			for (int i = 0; i < ceplexParameters.QuantityOfClients; i++)
			{
				ceplexParameters.ClientsDemand[i] = fractionedScheduledTrips[i].QuantityProduct;
			}

			return ceplexParameters;
		}

		private CeplexParameters GetCeplexParametersVehicleRoutingProblem(Depot depot, VehicleRoute vehicleRoute)
		{
			var ceplexParametersVRP = new CeplexParameters();
			ceplexParametersVRP.QuantityOfClients = vehicleRoute.SubRoutes.Count - 1;
			ceplexParametersVRP.Distance = new double[vehicleRoute.SubRoutes.Count][];
			ceplexParametersVRP.Duration = new long[vehicleRoute.SubRoutes.Count][];
			vehicleRoute.SubRoutes = vehicleRoute.SubRoutes.OrderBy(s => s.SequenceNumber).ToList();

			for (int i = 0; i < vehicleRoute.SubRoutes.Count; i++)
			{
				ceplexParametersVRP.Distance[i] = new double[vehicleRoute.SubRoutes.Count];
				for (int j = 0; j < vehicleRoute.SubRoutes.Count; j++)
				{	
					vehicleRoute.SubRoutes[i].indexVRPDistanceMatrix = i;
					if (i == 0 && i != j)
					{
						var tupleDistanceDuration = _googleMapsRepository.GetDistanceBetweenTwoAddressesWithCache(depot.Address,
							vehicleRoute.SubRoutes[j].AddressOrigin);
						ceplexParametersVRP.Distance[i][j] = tupleDistanceDuration.Item1.Value;
					}
					else if (i != 0 && i < j && i != j)
					{
						var tupleDistanceDuration = _googleMapsRepository.GetDistanceBetweenTwoAddressesWithCache(
							vehicleRoute.SubRoutes[i].AddressOrigin, vehicleRoute.SubRoutes[j].AddressDestiny);
						ceplexParametersVRP.Distance[i][j] = tupleDistanceDuration.Item1.Value;
					}
					else if (i != 0 && i > j && i != j)
					{
						ceplexParametersVRP.Distance[i][j] = ceplexParametersVRP.Distance[j][i];
					}
					else 
					{
						ceplexParametersVRP.Distance[i][j] = 0;
					}	 	
				}
			}
			return ceplexParametersVRP;
		}

		private List<VehicleRoute> GetRoutesFindedMultipleVehicleRouteProblem(Depot depot, List<DeliveryTruckTrip> fractionedScheduledTrips,
			CeplexParameters ceplexParameters, int[][][] routeMatrix)
		{
			var routes = new List<VehicleRoute>();
			for (int k = 0; k < ceplexParameters.QuantityOfVehiclesAvailable; k++)
			{
				var route = new VehicleRoute();
				route.DepotId = depot.DepotId;
				route.DateCreation = DateTime.Now.Date;
				route.DateScheduled = DateTime.Now.Date;
				for(int j = 0; j <= ceplexParameters.QuantityOfClients; j++)
				{
					for (int i = 0; i <= ceplexParameters.QuantityOfClients; i++)
					{
						if(routeMatrix[k][j][i] == 1 && j != i && j == 0)
						{
							var subRoute = new SubRoute();	
							subRoute.AddressOriginId = depot.Address.AddressId;
							var clientDestiny = fractionedScheduledTrips.FirstOrDefault(c => c.ColumnIndex == i);
							subRoute.AddressDestinyId = clientDestiny.Address.AddressId;
							subRoute.Distance = ceplexParameters.Distance[j][i];
							subRoute.Duration = ceplexParameters.Duration[j][i].ConvertMinutesToDateTime();
							subRoute.AddressOrigin = depot.Address;
							subRoute.AddressDestiny = clientDestiny.Address;
							route.SubRoutes.Add(subRoute);
						}
						else if(routeMatrix[k][j][i] == 1 && j != i && j > 0 && i != 0)
						{
							var subRoute = new SubRoute();
							var clientOrigin = fractionedScheduledTrips.FirstOrDefault(c => c.ColumnIndex == j);
							subRoute.AddressOriginId = clientOrigin.Address.AddressId;
							var clientDestiny = fractionedScheduledTrips.FirstOrDefault(c => c.ColumnIndex == i);
							subRoute.AddressDestinyId = clientDestiny.Address.AddressId;
							subRoute.Distance = ceplexParameters.Distance[j][i];
							subRoute.Duration = ceplexParameters.Duration[j][i].ConvertMinutesToDateTime();
							subRoute.AddressOrigin = clientOrigin.Address;
							subRoute.AddressDestiny = clientDestiny.Address;
							route.SubRoutes.Add(subRoute);
						}
						else if (routeMatrix[k][j][i] == 1 && j != i && j > 0 && i == 0)
						{
							var subRoute = new SubRoute();
							var clientOrigin = fractionedScheduledTrips.FirstOrDefault(c => c.ColumnIndex == j);
							subRoute.AddressOriginId = clientOrigin.Address.AddressId;
							subRoute.AddressDestinyId = depot.Address.AddressId;
							subRoute.Distance = ceplexParameters.Distance[j][i];
							subRoute.Duration = ceplexParameters.Duration[j][i].ConvertMinutesToDateTime();
							subRoute.AddressOrigin = clientOrigin.Address;
							subRoute.AddressDestiny = depot.Address;
							route.SubRoutes.Add(subRoute);
						}
					}
				}  
				if(route.SubRoutes.Count > 0)
					routes.Add(route);
			}
			return routes;
		}

		private void GetOptimalSequenceOfClientsToVisit(Depot depot, VehicleRoute vehicleRoute, int[] sequenceVector)
		{
			var depotSubRoute = vehicleRoute.SubRoutes.FirstOrDefault(s => s.AddressOriginId == depot.Address.AddressId);
			depotSubRoute.SequenceNumber = sequenceVector[0] - sequenceVector[0];
			int i = 1;
			while (i < sequenceVector.Length)
			{
				var nextSubRoute = vehicleRoute.SubRoutes.FirstOrDefault(s => s.indexVRPDistanceMatrix == i);
				nextSubRoute.SequenceNumber = sequenceVector[i];
				i++;
			}
		}

		private Tuple<double[][], long[][]> GetDistancesAndDurations(Depot depot, List<DeliveryTruckTrip> fractionedScheduledTrips,
			int numberOfPoints)
		{				
			numberOfPoints++;
			double[][] Distance = new double[numberOfPoints][];
			long[][] Duration = new long[numberOfPoints][];

			Distance[0] = new double[numberOfPoints];
			Duration[0] = new long[numberOfPoints];
			for (int j = 0; j < numberOfPoints; j++)
			{
				if ((j == 0))
				{
					Distance[0][0] = 0;
					Duration[0][0] = 0;
				}
				else
				{
					var tupleDistanceDurantion = _googleMapsRepository.GetDistanceBetweenTwoAddressesWithCache(depot.Address, 
						fractionedScheduledTrips[j - 1].Address);
					Distance[0][j] = tupleDistanceDurantion.Item1.Value;
					Duration[0][j] = tupleDistanceDurantion.Item2;
				}
			}

			for (int j = 1; j < numberOfPoints; j++)
			{
				Distance[j] = new double[numberOfPoints];
				Duration[j] = new long[numberOfPoints];
				fractionedScheduledTrips[j - 1].ColumnIndex = j;
				for (int i = 0; i < numberOfPoints; i++)
				{
					if ((j == i))
					{
						Distance[j][i] = 0;
						Duration[j][i] = 0;
					}
					else if (j < i)
					{
						var tupleDistanceDurantion = _googleMapsRepository.GetDistanceBetweenTwoAddressesWithCache(
							fractionedScheduledTrips[j - 1].Address, fractionedScheduledTrips[i - 1].Address);
						Distance[j][i] = tupleDistanceDurantion.Item1.Value;
						Duration[j][i] = tupleDistanceDurantion.Item2;
					}
					else if (j > i)
					{
						Distance[j][i] = Distance[i][j];
						Duration[j][i] = Duration[i][j];
					}
				}
			}

			Tuple<double[][],long[][]> DistancesAndDurations = Tuple.Create<double[][],long[][]>(Distance, Duration);
			return DistancesAndDurations;
		}  

		private void FindSequenceNumberOfRoutes(List<VehicleRoute> routes, Depot depot)
		{
			foreach(var vehicleRoute in routes)
			{
				FindSequenceNumberOfSubRoutes(vehicleRoute, depot);
			}
		}

		private void FindSequenceNumberOfSubRoutes(VehicleRoute vehicleRoute, Depot depot)
		{
			int indexOfDepot = vehicleRoute.SubRoutes.FindIndex(s => s.AddressOriginId == depot.Address.AddressId);
			vehicleRoute.SubRoutes[indexOfDepot].SequenceNumber = 1;
			int addressIdPreviousDestiny = vehicleRoute.SubRoutes[indexOfDepot].AddressDestinyId;
			int nextSequenceNumber = 2;
			while (nextSequenceNumber <= vehicleRoute.SubRoutes.Count)
			{
				var nextIndex = vehicleRoute.SubRoutes.FindIndex(s => s.AddressOriginId == addressIdPreviousDestiny);
				vehicleRoute.SubRoutes[nextIndex].SequenceNumber = nextSequenceNumber;
				addressIdPreviousDestiny = vehicleRoute.SubRoutes[nextIndex].AddressDestinyId;
				nextSequenceNumber++;
			}
		}

		private void InsertVehicleRoutes(List<VehicleRoute> vehicleRoutes, Depot depot, int productType)
		{
			var availableVehicles = _vehicleRepository.GetAvailableVehiclesByDepot(depot.DepotId);
			foreach (var vehicleRoute in vehicleRoutes)
			{
				vehicleRoute.productType = productType;
				vehicleRoute.VehicleId = availableVehicles[0].VehicleId;
				vehicleRoute.VehicleRouteId = _vehicleRouteRepository.InsertVehicleRoute(vehicleRoute);
				foreach (var subRoute in vehicleRoute.SubRoutes)
				{
					subRoute.VehicleRouteId = vehicleRoute.VehicleRouteId;
					_vehicleRouteRepository.InsertSubRoute(subRoute);
				}
			}
		}

		private void ClusterDeliveriesOfTheSameClient(List<DeliveryDto> deliveriesToBeScheduled)
		{
			for(var i = 0; i < deliveriesToBeScheduled.Count; i++)
			{
				if(deliveriesToBeScheduled.Count(d => d.client.clientId == deliveriesToBeScheduled[i].client.clientId) > 1)
				{
					deliveriesToBeScheduled[i].quantityProduct = deliveriesToBeScheduled
						.FindAll(d => d.client.clientId == deliveriesToBeScheduled[i].client.clientId)
						.Sum(dSameClient => dSameClient.quantityProduct);
					deliveriesToBeScheduled.RemoveAll(d => 
						d.client.clientId == deliveriesToBeScheduled[i].client.clientId && 
						d.deliveryId != deliveriesToBeScheduled[i].deliveryId);
				}
			}
		}

		private readonly IAddressRepository _addressRepository;
		private readonly IDepotRepository _depotRepository;
		private readonly ICeplexRepository _ceplexRepository;
		private readonly IVehicleRepository _vehicleRepository;
		private readonly IGoogleMapsRepository _googleMapsRepository;
		private readonly IVehicleRouteRepository _vehicleRouteRepository;
	}
}
