using System.Collections.Generic;
using VRPTW.Domain.Dto;
using VRPTW.Domain.Entity;

namespace VRPTW.Business.Mapper
{
	public static class VehicleRouteMapper
	{
		public static VehicleRouteDto CreateDto(this VehicleRoute entity)
		{
			return new VehicleRouteDto()
			{
				vehicleRouteId = entity.VehicleRouteId,
				dateCreation = entity.DateCreation,
				dateScheduled = entity.DateScheduled,
				departureTime = entity.DepartureTime,
				estimatedTimeReturn = entity.EstimatedTimeReturn,
				vehicleId = entity.VehicleId,
				depot = entity.Depot.CreateDto(),
				subRoutes = entity.SubRoutes.CreateDto()
			};
		}

		public static List<VehicleRouteDto> CreateDto(this List<VehicleRoute> entities)
		{
			return entities.ConvertAll(entity => entity.CreateDto());
		}

		public static VehicleRouteFilter CreateEntity(this VehicleRouteFilterDto dto)
		{
			return new VehicleRouteFilter
			{
				DesiredDateInitial = dto.desiredDateInitial,
				DesiredDateFinal = dto.desiredDateFinal,
				ProductType = dto.productType
			};
		}
	}
}
