using ILOG.CP;
using ILOG.OPL;
using System;
using VRPTW.CrossCutting.Configuration;
using VRPTW.Domain.Interface.Repository;

namespace VRPTW.Repository.CEPLEX
{
	public class CeplexRepository : ICeplexRepository
	{
		public void SolveFractionedTrips(int quantityOfVehiclesAvailable, int quantityOfClients, int[][] time, int[] vehicleCapacity, 
			int[] clientsDemand)
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

			OplDataSource dataSource = new MyData(oplFactory);
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
				Console.Out.WriteLine("No solution!");
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
			return "";
		}

	}

	internal class MyData : CustomOplDataSource
	{

		internal MyData(OplFactory oplF) : base(oplF)
		{
		}
		public override void CustomRead()
		{

			int _nbConfs = 7;
			int _nbOptions = 5;

			OplDataHandler handler = getDataHandler();
			handler.StartElement("nbConfs");
			handler.AddIntItem(_nbConfs);
			handler.EndElement();
			handler.StartElement("nbOptions");
			handler.AddIntItem(_nbOptions);
			handler.EndElement();

			int[] _demand = { 5, 5, 10, 10, 10, 10, 5 };
			handler.StartElement("demand");
			handler.StartArray();
			for (int i = 0; i < _nbConfs; i++)
				handler.AddIntItem(_demand[i]);
			handler.EndArray();
			handler.EndElement();

			int[,] _option = {{1, 0, 0, 0, 1, 1, 0},
							   {0, 0, 1, 1, 0, 1, 0},
							   {1, 0, 0, 0, 1, 0, 0},
							   {1, 1, 0, 1, 0, 0, 0},
							   {0, 0, 1, 0, 0, 0, 0}};
			handler.StartElement("option");
			handler.StartArray();
			for (int i = 0; i < _nbOptions; i++)
			{
				handler.StartArray();
				for (int j = 0; j < _nbConfs; j++)
					handler.AddIntItem(_option[i, j]);
				handler.EndArray();
			}
			handler.EndArray();
			handler.EndElement();

			int[,] _capacity = { { 1, 2 }, { 2, 3 }, { 1, 3 }, { 2, 5 }, { 1, 5 } };
			handler.StartElement("capacity");
			handler.StartArray();
			for (int i = 0; i < _nbOptions; i++)
			{
				handler.StartTuple();
				for (int j = 0; j <= 1; j++)
					handler.AddIntItem(_capacity[i, j]);
				handler.EndTuple();
			}
			handler.EndArray();
			handler.EndElement();
		}
	}
}
