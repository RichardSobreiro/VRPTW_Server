using System.Collections.Generic;
using VRPTW.Domain.Entity;

namespace VRPTW.Domain.Interface.Repository
{
	public interface IDepotRepository
	{
		List<Depot> SelectDepots();
	}
}
