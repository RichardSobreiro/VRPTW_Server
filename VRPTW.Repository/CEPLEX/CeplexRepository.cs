﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VRPTW.Domain.Entity;
using VRPTW.Domain.Interface.Repository;

namespace VRPTW.Repository.CEPLEX
{
	public class CeplexRepository : ICeplexRepository
	{
		public int[][][] SolveFractionedTrips(CeplexParameters ceplexParameters)
		{
			CreateDataFile(ceplexParameters);
			CallSolver();
			return GetRoute(ceplexParameters);
		}

		private void CreateDataFile(CeplexParameters ceplexParameters)
		{
			using (var writer = new StreamWriter("C:\\Users\\Richard\\Desktop\\Mulprod\\Modelo3.dat"))
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

		private void CallSolver()
		{
			ProcessStartInfo startInfo = new ProcessStartInfo();
			startInfo.RedirectStandardOutput = true;
			startInfo.CreateNoWindow = true;
			startInfo.UseShellExecute = false;
			startInfo.FileName = "C:\\Users\\Richard\\Desktop\\Mulprod\\bin\\Debug\\Mulprod.exe";
			//startInfo.WorkingDirectory = Path.GetDirectoryName("C:\\Users\\Richard\\Desktop\\Mulprod\\bin\\Debug");
			startInfo.WindowStyle = ProcessWindowStyle.Hidden;				   

			try
			{																 
				using (Process exeProcess = Process.Start(startInfo))
				{			   
					string result = exeProcess.StandardOutput.ReadToEnd();
					exeProcess.WaitForExit();
				}
			}
			catch(Exception e)
			{
				throw e;
			}
		}

		private int[][][] GetRoute(CeplexParameters ceplexParameters)
		{
			int[][][] routeMatrix = new int[ceplexParameters.QuantityOfVehiclesAvailable][][];
			using (var reader = new StreamReader("C:\\Users\\Richard\\Desktop\\Mulprod\\Solution1.txt"))
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
							if (solutionText.Contains("x["+ (k+1) + "][" + (j+1) + "][" + (i+1) + "]"))
								routeMatrix[k][j][i] = 1;
							else
								routeMatrix[k][j][i] = 0;
						}
					}
				}
			}
			return routeMatrix;
		}

		private static string ExtractFromString(string text, string startString, string endString)
		{
			List<string> matched = new List<string>();
			int indexStart = 0, indexEnd = 0;
			bool exit = false;
			while (!exit)
			{
				indexStart = text.IndexOf(startString);
				indexEnd = text.IndexOf(endString, indexStart);
				if (indexStart != -1 && indexEnd != -1)
				{
					matched.Add(text.Substring(indexStart + startString.Length,
						indexEnd - indexStart - startString.Length));
					exit = true;										 
				}
				else
					exit = true;
			}
			return matched.FirstOrDefault();
		}
	}											  
}
