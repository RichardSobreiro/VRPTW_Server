using System.Collections.Generic;
using VRPTW.Domain.Entity;

namespace VRPTW.Domain.Interface.Repository
{
	public interface IClientRepository
	{
		int CreateClient(Client client);
		void EditClient(Client client);
		List<Client> GetClients();
	}
}
