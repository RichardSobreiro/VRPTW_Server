using ILOG.OPL;
using System;

namespace Ceplex
{
	class Program
	{
		static int Main(string[] args)
		{
			int status = 0;

			try
			{
				OplFactory oplF = new OplFactory();
				OplErrorHandler handler = oplF.CreateOplErrorHandler(Console.Out);
				//OplModelSource modelSource = oplF.CreateOplModelSource(DATADIR + "/Modelo3.mod");
				OplModelSource modelSource;
				OplDataSource dataSource;
				if (args[0].Contains("1"))
				{
					modelSource = oplF.CreateOplModelSource("C://Users//Richard Sobreiro//Desktop//PFCCodigos//Backend//VRPTW_Server//Ceplex//MultipleVehicleRoutingProblem.mod");
					dataSource = oplF.CreateOplDataSource("C://Users//Richard Sobreiro//Desktop//PFCCodigos//Backend//VRPTW_Server//Ceplex//MultipleVehicleRoutingProblem.dat");
				}
				else
				{
					modelSource = oplF.CreateOplModelSource("C://Users//Richard Sobreiro//Desktop//PFCCodigos//Backend//VRPTW_Server//Ceplex//VRP.mod");
					dataSource = oplF.CreateOplDataSource("C://Users//Richard Sobreiro//Desktop//PFCCodigos//Backend//VRPTW_Server//Ceplex//VRP.dat");
				}
				OplSettings settings = oplF.CreateOplSettings(handler);
				OplModelDefinition def = oplF.CreateOplModelDefinition(modelSource, settings);
				Cplex cplex = oplF.CreateCplex();

				OplModel opl = oplF.CreateOplModel(def, cplex);
				opl.AddDataSource(dataSource);
				opl.Generate();

				if (cplex.Solve())
				{
					Console.Out.WriteLine();
					Console.Out.WriteLine("OBJECTIVE: " + opl.Cplex.ObjValue);
					opl.PostProcess();
					status = 0;
				}
				else
				{
					Console.Out.WriteLine("No solution!");
					status = 1;
				}
				oplF.End();
			}
			catch (OplException ex)
			{
				Console.WriteLine(ex.Message);
				status = 2;
			}
			catch (ILOG.Concert.Exception ex)
			{
				Console.WriteLine(ex.Message);
				status = 3;
			}
			catch (System.Exception ex)
			{
				Console.WriteLine(ex.Message);
				status = 4;
			}

			//Console.WriteLine("--Press <Enter> to exit--");
			//Console.ReadLine();

			return status;
		}
	}
}
