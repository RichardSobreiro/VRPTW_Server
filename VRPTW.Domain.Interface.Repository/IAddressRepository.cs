using VRPTW.Domain.Entity;

namespace VRPTW.Domain.Interface.Repository
{
	public interface IAddressRepository
	{
		void CreateAddres(Address address);
		void EditAddress(Address address);
		Address GetAddressByClientId(int clientId);
		Address GetAddressByDepotId(int depotId);
	}
}
