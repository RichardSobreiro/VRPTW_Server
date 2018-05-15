using System.Collections.Generic;
using VRPTW.Domain.Dto;
using VRPTW.Domain.Entity;

namespace VRPTW.Business.Mapper
{
	public static class SubRouteMapper
	{
		public static SubRouteDto CreateDto(this SubRoute entity)
		{
			return new SubRouteDto()
			{
				subRouteId = entity.SubRouteId,
				vehicleRouteId = entity.VehicleRouteId,
				addressOrigin = entity.AddressOrigin.CreateDto(),
				addressDestiny = entity.AddressDestiny.CreateDto(),
				distance = entity.Distance,
				duration = entity.Duration,
				sequenceNumber = entity.SequenceNumber
			};
		}

		public static List<SubRouteDto> CreateDto(this List<SubRoute> entities)
		{
			return entities.ConvertAll(entity => entity.CreateDto());
		}
	}
}
