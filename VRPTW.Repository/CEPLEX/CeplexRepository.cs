﻿using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using VRPTW.CrossCutting.Configuration;
using VRPTW.CrossCutting.Enumerations;
using VRPTW.Domain.Entity;
using VRPTW.Domain.Interface.Repository;

namespace VRPTW.Repository.CEPLEX
{
	public class CeplexRepository : ICeplexRepository
	{
		public int[][][] SolveFractionedTrips(CeplexParameters ceplexParameters, out bool optimalSolution)
		{
			CreateDataFileMultipleVehicleRoutingProblem(ceplexParameters);
			CallSolver(LinearProgrammingProblems.MVRP, out optimalSolution);
			return GetRouteMultipleVehicleRoutingProblem(ceplexParameters);
		}

		public int[][] FindOptimalSequenceForSubRoutes(CeplexParameters ceplexParameters)
		{
			bool optimalSolution;
			CreateDataFileVehicleRoutingProblem(ceplexParameters);
			CallSolver(LinearProgrammingProblems.VRP, out optimalSolution);
			return GetRouteVehicleRoutingProblem(ceplexParameters);
		}

		private void CreateDataFileMultipleVehicleRoutingProblem(CeplexParameters ceplexParameters)
		{
			using (var writer = new StreamWriter(ConfigurationManager.AppSettings["DATA_FILE_MULTIPLE_VEHICLE_ROUTING_PROBLEM"]))
			{
				writer.Flush();

				writer.WriteLine("QuantityOfVehiclesAvailable = " + ceplexParameters.QuantityOfVehiclesAvailable + ";");
				writer.WriteLine("QuantityOfClients = " + ceplexParameters.QuantityOfClients + ";");
				writer.WriteLine("Time = [");
				for (int j = 0; j < (ceplexParameters.QuantityOfClients + 1); j++)
				{
					writer.Write("[");
					for (int i = 0; i < (ceplexParameters.QuantityOfClients + 1); i++)
					{
						if(i == ceplexParameters.QuantityOfClients)
						{
							writer.Write(ceplexParameters.Distance[j][i]);
						}
						else
						{
							writer.Write(ceplexParameters.Distance[j][i] + ",");
						}
					}
					if(j == ceplexParameters.QuantityOfClients)
					{
						writer.Write("]" + Environment.NewLine);
					}
					else
					{
						writer.Write("]," + Environment.NewLine);
					}
				}
				writer.WriteLine("];");
				writer.Write("VehiclesCapacity = [");
				for (int i = 0; i < ceplexParameters.QuantityOfVehiclesAvailable; i++)
				{
					if (i == (ceplexParameters.QuantityOfVehiclesAvailable - 1))
					{
						writer.Write(ceplexParameters.VehicleCapacity[i]);
					}
					else
					{
						writer.Write(ceplexParameters.VehicleCapacity[i] + ",");
					}
				}
				writer.WriteLine("];");
				writer.WriteLine("VehiclesGreatestPossibleDemand = " + ceplexParameters.VehiclesGreatestPossibleDemand + ";");
				writer.WriteLine("GreatestPossibleDemand = " + ceplexParameters.GreatestPossibleDemand + ";");
				writer.Write("ClientsDemand = [0,");
				for (int i = 0; i < ceplexParameters.QuantityOfClients; i++)
				{
					if (i == (ceplexParameters.QuantityOfClients - 1))
					{
						writer.Write(ceplexParameters.ClientsDemand[i]);
					}
					else
					{
						writer.Write(ceplexParameters.ClientsDemand[i] + ",");
					}
				}
				writer.WriteLine("];");
				writer.WriteLine("VehicleCost = 100000;");
			}
		}

		private void CreateDataFileVehicleRoutingProblem(CeplexParameters ceplexParameters)
		{
			using (var writer = new StreamWriter(ConfigurationManager.AppSettings["DATA_FILE_VEHICLE_ROUTING_PROBLEM"]))
			{
				writer.Flush();

				writer.WriteLine("QuantityOfClients = " + ceplexParameters.QuantityOfClients + ";");
				double wtot = 0;
				writer.Write("ClientsDemand = [0,");
				for (int i = 0; i < ceplexParameters.QuantityOfClients; i++)
				{
					if (i == (ceplexParameters.QuantityOfClients - 1))
					{
						writer.Write(ceplexParameters.ClientsDemand[i]);
					}
					else
					{
						writer.Write(ceplexParameters.ClientsDemand[i] + ",");
					}
					wtot += ceplexParameters.ClientsDemand[i];
				}
				writer.WriteLine("];");
				writer.WriteLine("wtot = " + wtot + ";");
				writer.WriteLine("Distance = [");
				for (int j = 0; j < (ceplexParameters.QuantityOfClients + 1); j++)
				{
					writer.Write("[");
					for (int i = 0; i < (ceplexParameters.QuantityOfClients + 1); i++)
					{
						if (i == ceplexParameters.QuantityOfClients)
						{
							writer.Write(ceplexParameters.Distance[j][i]);
						}
						else
						{
							writer.Write(ceplexParameters.Distance[j][i] + ",");
						}
					}
					if (j == ceplexParameters.QuantityOfClients)
					{
						writer.Write("]" + Environment.NewLine);
					}
					else
					{
						writer.Write("]," + Environment.NewLine);
					}
				}
				writer.WriteLine("];");
			}
		}

		private void CallSolver(LinearProgrammingProblems problem, out bool optimalSolution)
		{
			ProcessStartInfo startInfo = new ProcessStartInfo();
			startInfo.RedirectStandardOutput = true;
			startInfo.CreateNoWindow = true;
			startInfo.UseShellExecute = false;
			startInfo.Arguments = ((int)problem).ToString();
			startInfo.FileName = ConfigurationManager.AppSettings["SOLVER_PATH"];
			startInfo.WindowStyle = ProcessWindowStyle.Hidden;
			var stopwatch = new Stopwatch();

			try
			{																 
				using (Process exeProcess = Process.Start(startInfo))
				{
					stopwatch.Start();
					string result = exeProcess.StandardOutput.ReadToEnd();
					exeProcess.WaitForExit();
					stopwatch.Stop();
				}
				if(stopwatch.Elapsed.TotalSeconds > GeneralConfigurations.TOTAL_SECONDS_LIMIT_SOLVER)
				{
					optimalSolution = false;
				}
				else
				{
					optimalSolution = true;
				}
			}
			catch(Exception e)
			{
				throw e;
			}
		}

		private int[][][] GetRouteMultipleVehicleRoutingProblem(CeplexParameters ceplexParameters)
		{
			int[][][] routeMatrix = new int[ceplexParameters.QuantityOfVehiclesAvailable][][];
			using (var reader = new StreamReader(ConfigurationManager.AppSettings["SOLUTION_MULTIPLE_VEHICLE_ROUTING_PROBLEM"]))
			{
				string solutionText = Task.Run(() => reader.ReadToEndAsync()).Result;

				for(int k = 0; k < ceplexParameters.QuantityOfVehiclesAvailable; k++)
				{
					routeMatrix[k] = new int[ceplexParameters.QuantityOfClients + 1][];
					for (int j = 0; j < ceplexParameters.QuantityOfClients+1; j++)
					{
						routeMatrix[k][j] = new int[ceplexParameters.QuantityOfClients + 1];
						for (int i = 0; i < ceplexParameters.QuantityOfClients+1; i++)
						{
							if (solutionText.Contains("x["+ (i+1) + "][" + (j+1) + "][" + (k+1) + "]"))
								routeMatrix[k][j][i] = 1;
							else
								routeMatrix[k][j][i] = 0;
						}
					}
				}
			}
			return routeMatrix;
		}

		private int[][] GetRouteVehicleRoutingProblem(CeplexParameters ceplexParameters)
		{
			int[][] routeMatrix = new int[ceplexParameters.QuantityOfClients + 1][];
			using (var reader = new StreamReader(ConfigurationManager.AppSettings["SOLUTION_VEHICLE_ROUTING_PROBLEM"]))
			{
				string solutionText = Task.Run(() => reader.ReadToEndAsync()).Result;
																						
				for (int j = 0; j < ceplexParameters.QuantityOfClients + 1; j++)
				{
					routeMatrix[j] = new int[ceplexParameters.QuantityOfClients + 1];
					for (int i = 0; i <= ceplexParameters.QuantityOfClients + 1; i++)
					{
						if (solutionText.Contains("x[" + (j + 1) + "][" + (i + 1) + "]"))
							routeMatrix[j][i%5] = 1;
						else
							routeMatrix[j][i%5] = 0;
					}
				}
			}
			return routeMatrix;
		}
	}											  
}
