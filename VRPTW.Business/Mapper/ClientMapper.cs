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
				legalPerson = client.LegalPerson,
				documentNumber = client.DocumentNumber,
				documentType = client.DocumentType
			};
		}

		public static Client CreateEntity(this ClientDto clientDto)
		{
			return new Client()
			{
				ClientId = clientDto.clientId,
				DateCreation = clientDto.dateCreation,
				Name = clientDto.name,
				LegalPerson = clientDto.legalPerson,
				DocumentNumber = clientDto.documentNumber,
				DocumentType = clientDto.documentType
			};
		}
	}
}
