using System.Collections.Generic;
using VRPTW.Domain.Dto;
using VRPTW.Domain.Entity;

namespace VRPTW.Business.Mapper
{
	public static class AddressMapper
	{
		public static Address CreateEntity(this AddressDto addressDto)
		{
			return new Address()
			{
				AddressId = addressDto.addressId,
				Street = addressDto.street,
				Number = addressDto.number,
				Neighborhood = addressDto.neighborhood,
				City = addressDto.city,
				State = addressDto.state,
				ProductProviderId = addressDto.productProviderId,
				ClientId = addressDto.clientId	
			};
		}

		public static AddressDto CreateDto(this Address address)
		{
			return new AddressDto()
			{
				addressId = address.AddressId,
				street = address.Street,
				number = address.Number,
				neighborhood = address.Neighborhood,
				city = address.City,
				state = address.State,
				productProviderId = address.ProductProviderId,
				clientId = address.ClientId
			};
		}

		public static List<AddressDto> CreateDto(this List<Address> entities)
		{
			return entities.ConvertAll(entity => entity.CreateDto());
		}
		 
		public static List<Address> CreateEntity(this List<AddressDto> dtos)
		{
			return dtos.ConvertAll(dto => dto.CreateEntity());
		}
	}
}
