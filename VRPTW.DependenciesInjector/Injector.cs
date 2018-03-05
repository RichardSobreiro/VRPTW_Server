using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using System;
using System.Web.Http;
using VRPTW.Business;
using VRPTW.Domain.Interface.Business;
using VRPTW.Domain.Interface.Repository;
using VRPTW.Repository;
using VRPTW.Repository.CEPLEX;
using VRPTW.Repository.Scheduler;

namespace VRPTW.DependenciesInjector
{
	public class Injector
    {		
		public static void Begins()
		{
			
		}

		private static void BeginsHttpDepencies(HttpConfiguration global, Action<Container> RelateDependencies)
		{
			RelateDependencies(container);
			global.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
		}
		
		private static void RelateDependencies(Container container)
		{
			// Repository
			container.Register<IDeliveryRepository, DeliveryRepository>();
			container.Register<IFractionedTripRepository, FractionedTripSchedulerRepository>();
			container.Register<IGoogleMapsRepository, GoogleMapsRepository>();
			container.Register<IDeliveryTruckTripRepository, DeliveryTruckTripRepository>();
			container.Register<IAddressRepository, AddressRepository>();
			container.Register<ICeplexRepository, CeplexRepository>();

			// Business
			container.Register<IDeliveryBusiness, DeliveryBusiness>();
			container.Register<ICreateDeliveryBusiness, CreateDeliveryBusiness>();
		}

		private static Container container = new Container();
    }
}
