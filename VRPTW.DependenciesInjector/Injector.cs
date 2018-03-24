using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using System;
using System.Web.Http;
using VRPTW.Business;
using VRPTW.Domain.Interface.Business;
using VRPTW.Domain.Interface.Repository;
using VRPTW.Repository;
using VRPTW.Repository.CEPLEX;

namespace VRPTW.DependenciesInjector
{
	public class Injector
    {		
		public static Container GetContainer
		{
			get
			{
				return container;
			}
		}

		//public static void Begins()
		//{
		//	Begins(RelateDependencies);
		//}

		public static void Begins(HttpConfiguration global)
		{
			Begins(global, RelateDependencies);
		}

		public static void Begins(HttpConfiguration global, Action<Container> DelegateIniciarContainer)
		{
			container = new Container();
			DelegateIniciarContainer(container);
			global.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
		}

		//public static void Begins(Action<Container> DelegateIniciarContainer)
		//{
		//	container = new Container();
		//	DelegateIniciarContainer(container);
		//	System.Web.Mvc.DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
		//}

		private static void RelateDependencies(Container container)
		{
			// Repository
			container.Register<IDeliveryRepository, DeliveryRepository>();
			container.Register<IFractionedTripRepository, FractionedTripSchedulerRepository>();
			container.Register<IGoogleMapsRepository, GoogleMapsRepository>();
			container.Register<IDeliveryTruckTripRepository, DeliveryTruckTripRepository>();
			container.Register<IAddressRepository, AddressRepository>();
			container.Register<ICeplexRepository, CeplexRepository>();
			container.Register<IDepotRepository, DepotRepository>();
			container.Register<IVehicleRepository, VehicleRepository>();
			container.Register<IProductRepository, ProductRepository>();

			// Business
			container.Register<IDeliveryBusiness, DeliveryBusiness>();
			container.Register<ICreateDeliveryBusiness, CreateDeliveryBusiness>();
			container.Register<IProductBusiness, ProductBusiness>();
		}

		private static Container container = new Container();
    }
}
