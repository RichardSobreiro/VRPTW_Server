﻿using System.Collections.Generic;
using VRPTW.Domain.Entity;

namespace VRPTW.Domain.Interface.Repository
{
	public interface IDeliveryRepository
    {
		int InsertDelivery(Delivery delivery);
		List<Delivery> GetDeliveries();
	}
}
