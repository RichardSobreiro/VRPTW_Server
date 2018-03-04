using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace VRPTW.Domain.Interface.Repository
{
	public abstract class RepositoryBase
	{
		protected IDbConnection OpenConnection()
		{
			return new SqlConnection(ConfigurationManager.ConnectionStrings["VehicleRoutingProblem"].ConnectionString);
		}									
	}
}
