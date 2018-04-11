using System.Collections.Generic;
using VRPTW.Domain.Dto;
using VRPTW.Domain.Entity;

namespace VRPTW.Business.Mapper
{
	public static class ClientMapper
	{
		public static ClientDto CreateDto(this Client client)
		{
			return new ClientDto()
			{
				clientId = client.ClientId,
				dateCreation = client.DateCreation,
				name = client.Name,						
				documentNumber = client.DocumentNumber,
				documentType = client.DocumentType,
				address = client.Address?.CreateDto()
			};
		}

		public static Client CreateEntity(this ClientDto clientDto)
		{
			return new Client()
			{
				ClientId = clientDto.clientId,
				DateCreation = clientDto.dateCreation,
				Name = clientDto.name,				  
				DocumentNumber = clientDto.documentNumber,
				DocumentType = clientDto.documentType,
				Address = clientDto.address?.CreateEntity()
			};
		}

		public static List<ClientDto> CreateDto(this List<Client> clients)
		{
			return clients.ConvertAll(client => client.CreateDto());
		}
	}
}
