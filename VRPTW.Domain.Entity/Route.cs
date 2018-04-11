using System;
using System.Collections.Generic;
using System.Linq;

namespace VRPTW.Domain.Entity
{
	public class Route
	{	
		public DateTime DateCreation { get; set; }
		public double TotalDistance
		{
			get
			{
				return SubRoutes.Sum(subRoute => subRoute.Distance);
			}
		}
		public List<SubRoute> SubRoutes { get; set; } = new List<SubRoute>();
	}
}
