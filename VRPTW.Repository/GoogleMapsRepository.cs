using Google.Maps;
using Google.Maps.DistanceMatrix;
using Google.Maps.Geocoding;
using System;
using System.Linq;
using VRPTW.CrossCutting.Cache;
using VRPTW.Domain.Entity;
using VRPTW.Domain.Interface.Repository;

namespace VRPTW.Repository
{
	public class GoogleMapsRepository : IGoogleMapsRepository
	{
		public Tuple<double?, long> GetDistanceBetweenTwoAddressesWithCache(Address addressOrigin, Address addressDestination)
		{
			return Cache.GetItemFromCache("Distance_" + addressOrigin.AddressId + "_" + addressDestination.AddressId,
				GetDistanceBetweenTwoAddresses, addressOrigin, addressDestination);
		}

		private Tuple<double?, long> GetDistanceBetweenTwoAddresses(Address addressOrigin, Address addressDestination)
		{
			DistanceMatrixRequest request = new DistanceMatrixRequest();

			GetLatirudeAndLogitudeOfAnAddress(addressOrigin);
			GetLatirudeAndLogitudeOfAnAddress(addressDestination);

			request.AddDestination(new LatLng(latitude: (decimal)addressOrigin.Latitude.Value, longitude: (decimal)addressOrigin.Longitude));

			request.AddOrigin(new LatLng(latitude: (decimal)addressDestination.Latitude.Value, longitude: (decimal)addressDestination.Longitude));		

			request.Mode = TravelMode.driving;
			request.Units = Units.metric;
			
			DistanceMatrixResponse response = new DistanceMatrixService().GetResponse(request);

			long duration = 0;
			double? distance = null;
			if (response.Status == ServiceResponseStatus.Ok && response.Rows.Length == 1)
			{
				duration = response.Rows[0].Elements[0].duration.Value;
				distance = response.Rows[0].Elements[0].distance.Value;
				return Tuple.Create(distance, duration);
			}
			return Tuple.Create(distance, duration);
		}

		public void GetLatirudeAndLogitudeOfAnAddress(Address address)
		{
			var request = new GeocodingRequest();
			request.Address = address.FormattedAddress;
			var response = new GeocodingService().GetResponse(request);

			if (response.Status == ServiceResponseStatus.Ok && response.Results.Count() >= 1)
			{
				var result = response.Results.First();

				address.Longitude = result.Geometry.Location.Longitude;
				address.Latitude = result.Geometry.Location.Latitude;
			}
			else if(response.Status == ServiceResponseStatus.OverQueryLimit)
			{
				Console.WriteLine("Excesso de requisições no dia!");
			}
			else
			{
				address.Longitude = null;
				address.Latitude = null;
			}
		}
	}
}
