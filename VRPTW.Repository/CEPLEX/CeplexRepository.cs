using ILOG.CP;
using ILOG.OPL;
using System;
using VRPTW.CrossCutting.Configuration;
using VRPTW.Domain.Entity;
using VRPTW.Domain.Interface.Repository;

namespace VRPTW.Repository.CEPLEX
{
	public class CeplexRepository : ICeplexRepository
	{
		public void SolveFractionedTrips(CeplexParameters ceplexParameters)
		{
			OplFactory.DebugMode = GeneralConfigurations.OplDebugMode;
			OplFactory oplFactory = new OplFactory();

			OplErrorHandler errHandler = null;
			ConfigureErrorHandler(oplFactory, ref errHandler);

			OplModelSource modelSource = oplFactory.CreateOplModelSourceFromString(GetModelText(), "FractionedTrips");

			OplSettings settings = oplFactory.CreateOplSettings(errHandler);

			OplModelDefinition def = oplFactory.CreateOplModelDefinition(modelSource, settings);

			CP cp = oplFactory.CreateCP();

			OplModel opl = oplFactory.CreateOplModel(def, cp);

			OplDataSource dataSource = new MyData(oplFactory, ceplexParameters.QuantityOfVehiclesAvailable, ceplexParameters.QuantityOfClients,
				ceplexParameters.VehiclesGreatestPossibleDemand, ceplexParameters.GreatestPossibleDemand, ceplexParameters.Time,
				ceplexParameters.VehicleCapacity, ceplexParameters.ClientsDemand);
			opl.AddDataSource(dataSource);
			opl.Generate();

			if (cp.Solve())
			{
				Console.Out.WriteLine("OBJECTIVE: " + opl.CP.ObjValue);
				opl.PostProcess();
				opl.PrintSolution(Console.Out);
			}
			else
			{
				throw new Exception();
			}

			oplFactory.End();

		}

		private void ConfigureErrorHandler(OplFactory oplFactory, ref OplErrorHandler errHandler)
		{
			if (GeneralConfigurations.OplDebugMode)
				errHandler = oplFactory.CreateOplErrorHandler(Console.Out);
		}

		private string GetModelText()
		{
			var modelText = @"int QuantityOfVehiclesAvailable = ...;
							int QuantityOfClients = ...;
							int VehicleCost = ...;

							int M = ...;

							range Vehicles = 1..QuantityOfVehiclesAvailable;
							range Locations = 1..(QuantityOfClients + 1);

							int time[Locations][Locations] = ...;
							int VehicleCapacity[Vehicles] = ...;
							int hmax = ...;
							float ClientsDemand[Locations] = ...;

							int MPMN = QuantityOfClients + 1;
							dvar boolean u[Locations][Locations][Vehicles];

							dvar boolean x[Locations][Locations][Vehicles];
							dvar float p[Locations][Vehicles];
							dvar int v[Locations][Vehicles];
							dvar boolean vehicleIsUsed[Vehicles];

							dvar int T;

							minimize T;

							subject to
							{

								FuncaoObjetivo:
									forall(k in Vehicles, j in Locations, i in Locations){
											T >= time[i][j] * x[i][j][k] + sum(k in Vehicles) VehicleCost* vehicleIsUsed[k];
										}

								ChegadaImplicaEmSaidaDeUmCliente:
									sum(k in Vehicles, j in Locations, i in Locations : i != j) x [j][i][k] == sum(k in Vehicles, j in Locations, i in Locations : i != j) x [i][j][k];

								NoMaximoUmCaminhoChegaEmUmCliente:
									forall(j in Locations){
										sum(k in Vehicles, i in Locations: i != j) x[i][j][k] <= 1;
									}

								NoMaximoUmCaminhoSaiDeUmCliente:
									forall(i in Locations) {
										sum(k in Vehicles, j in Locations: i != j) x[i][j][k] <= 1;
									}

								EliminarSubTours:
									forall(k in Vehicles, j in Locations : j > 1) {
										v[j][k] <= MPMN;
										v[j][k] >= 2;
										forall(i in Locations: i > 1 && i != j) {
											MPMN* u[i][j][k] >= v [j][k] - v [i][k] - 1;
											MPMN - (MPMN * u [i][j][k]) >= v [i][k] - v [j][k] + 1;
											u [i][j][k] >= x [i][j][k];
										}
									}

								SeVeiculoNaoVisitaClienteEntaoNaoRecolheNadaDoCliente:
									forall(k in Vehicles, j in Locations)
									{
										sum(i in Locations: i != j) hmax* x[i][j][k] >= p[j][k];	
									}

								ImplementaRestricaoLimiteDeCargaVeiculo:
									forall(k in Vehicles)
									{
										sum(i in Locations) p[i][k] <= VehicleCapacity[k];
									}

								ImplementaRestricaoTodosMateriasDoClienteDevemSerEntregues:
									forall(i in Locations)
									{
										sum(k in Vehicles) p[i][k] == ClientsDemand[i];
									}

								VeiculoAtendeCliente:
									forall(k in Vehicles)
									{
										M* vehicleIsUsed[k] >= sum(i in Locations) p[i][k];
									}

								VariaveisNaoNegativas:
									forall(k in Vehicles, i in Locations)
									{
										p[i][k] >= 0;
									}
								}";
			return modelText;
		}

	}

	internal class MyData : CustomOplDataSource
	{
		internal MyData(OplFactory oplF, int quantityOfVehiclesAvailable, int quantityOfClients, int vehiclesGreatestPossibleDemand,
			int greatestPossibleDemand, double[][] time, int[] vehiclesCapacity, double[] clientsDemand) : base(oplF)
		{
			QuantityOfVehiclesAvailable = quantityOfVehiclesAvailable;
			QuantityOfClientes = quantityOfClients;
			VehiclesGreatestPossibleDemand = vehiclesGreatestPossibleDemand;
			GreatestPossibleDemand = greatestPossibleDemand;
			Time = time;
			VehiclesCapacity = vehiclesCapacity;
			ClientsDemand = clientsDemand;
		}
		public override void CustomRead()
		{
			OplDataHandler handler = getDataHandler();
			handler.StartElement("QuantityOfVehiclesAvailable");
			handler.AddIntItem(QuantityOfVehiclesAvailable);
			handler.EndElement();

			handler.StartElement("QuantityOfClientes");
			handler.AddIntItem(QuantityOfClientes);
			handler.EndElement();
			  
			handler.StartElement("VehiclesGreatestPossibleDemand");
			handler.AddIntItem(VehiclesGreatestPossibleDemand);
			handler.EndElement();

			handler.StartElement("GreatestPossibleDemand");
			handler.AddIntItem(GreatestPossibleDemand);
			handler.EndElement();
														
			handler.StartElement("Time");
			handler.StartArray();
			for (int j = 0; j < (QuantityOfClientes + 1); j++)
				for (int i = 0; i < (QuantityOfClientes + 1); i++)
					handler.AddNumItem(Time[i][j]);
			handler.EndArray();
			handler.EndElement();

			handler.StartElement("VehiclesCapacity");
			handler.StartArray();
			for (int j = 0; j < VehiclesCapacity.Length; j++)	  
					handler.AddIntItem(VehiclesCapacity[j]);
			handler.EndArray();
			handler.EndElement();

			handler.StartElement("ClientsDemand");
			handler.StartArray();
			for (int j = 0; j < ClientsDemand.Length; j++)
				handler.AddNumItem(ClientsDemand[j]);
			handler.EndArray();
			handler.EndElement();
		}

		private int QuantityOfVehiclesAvailable { get; set; }
		private int QuantityOfClientes { get; set; }
		private int VehiclesGreatestPossibleDemand { get; set; }
		private int GreatestPossibleDemand { get; set; }
		private double[][] Time { get; set; }
		private int[] VehiclesCapacity { get; set; }
		private double[] ClientsDemand { get; set; }
	}
}
