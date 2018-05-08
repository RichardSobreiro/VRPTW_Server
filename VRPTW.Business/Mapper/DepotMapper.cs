using VRPTW.Domain.Dto;
using VRPTW.Domain.Entity;

namespace VRPTW.Business.Mapper
{
	public static class DepotMapper
	{
		public static DepotDto CreateDto(this Depot entity)
		{
			return new DepotDto
			{
				depotId = entity.DepotId,
				depotDescription = entity.DepotDescription
			};
		}
	}
}
