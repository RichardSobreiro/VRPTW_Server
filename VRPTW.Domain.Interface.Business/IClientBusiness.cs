using System.Collections.Generic;
using VRPTW.Domain.Dto;

namespace VRPTW.Domain.Interface.Business
{
	public interface IClientBusiness
	{
		int CreateClient(ClientDto clientDto);
		void EditClient(ClientDto clientDto);
		List<ClientDto> GetClientsByName(string clientName);
		ClientDto GetClientById(int clientId);
	}
}
