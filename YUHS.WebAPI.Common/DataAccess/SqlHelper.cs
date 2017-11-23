using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using static Dapper.SqlMapper;

namespace YUHS.WebAPI.Common.DataAccess
{
    public static class SqlHelper
    {
        //private static GridReader gridReader;

        public static string GetConnectionString(string ConnectionStringKey)
        {
            //return ConfigurationManager.AppSettings[ConnectionStringKey];
            // Change when using ConnectionString Encryption
            return ConfigurationManager.ConnectionStrings[ConnectionStringKey].ConnectionString;
        }

        private static string GetStoredProcedure(string storedProcedure)
        {
            string sp = storedProcedure;

            //if owner is exist, return original stored procedure
            if (sp.IndexOf(".") < 0)
            {
                if (ConfigurationManager.AppSettings["AppServer"] == "DEV")
                {
                    sp = "his_user." + storedProcedure;
                }
            }

            return sp;
        }

        // Select List
        public static IEnumerable<T> GetList<T>(string targetDB, string storedProcedure, DynamicParameters param = null) where T : class
        {
            using (SqlConnection connection = new SqlConnection(targetDB))
            {
                try
                {
                    //if (connection.State != ConnectionState.Open)
                        connection.Open();

                    var output = connection.Query<T>(GetStoredProcedure(storedProcedure), param, commandType: CommandType.StoredProcedure);
                    return output;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        // Multiple Select
        //public static Dictionary<Tmain, List<Tsub>> GetMultiPleList<Tmain, Tsub>(string targetDB, string storedProcedure, DynamicParameters param = null) where Tmain : class
        //{
        //    using (SqlConnection connection = new SqlConnection(targetDB))
        //    {
        //        connection.Open();

        //        Dictionary<Tmain, List<Tsub>> listTable = new Dictionary<Tmain, List<Tsub>>();

        //        using (var output = connection.QueryMultiple(GetStoredProcedure(storedProcedure), param, commandType: CommandType.StoredProcedure))
        //        {
        //            var main = output.Read<Tmain>().SingleOrDefault();
        //            var sub = output.Read<Tsub>().ToList();

        //            if (main != null && sub != null)
        //            {
        //                listTable.Add((Tmain)main, sub);
        //            }

        //            return listTable;
        //        }
        //    }
        //}

        public static IList<T> GetSingleInMultipleList<T>(string targetDB, string storedProcedure, DynamicParameters param = null) where T : class
        {
            using (SqlConnection connection = new SqlConnection(targetDB))
            {
                try
                {
                    connection.Open();

                    using (var output = connection.QueryMultiple(GetStoredProcedure(storedProcedure), param, commandType: CommandType.StoredProcedure))
                    {
                        IList<T> t1 = null;
                        while (!output.IsConsumed)
                        {
                            t1 = NextResult<T>(output);
                        }

                        return t1;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public static Tuple<IList<T1>, IList<T2>, IList<T3>> GetMultiPleList<T1, T2, T3>(string targetDB, string storedProcedure, DynamicParameters param = null) where T1 : class where T2 : class where T3 : class 
        {
            using (SqlConnection connection = new SqlConnection(targetDB))
            {
                try
                {
                    connection.Open();

                    using (var output = connection.QueryMultiple(GetStoredProcedure(storedProcedure), param, commandType: CommandType.StoredProcedure))
                    {
                        IList<T1> t1 = NextResult<T1>(output);
                        IList<T2> t2 = NextResult<T2>(output);
                        IList<T3> t3 = NextResult<T3>(output);
                        return new Tuple<IList<T1>, IList<T2>, IList<T3>>(t1, t2, t3);
                    }
                }

                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public static Tuple<IList<T1>, IList<T2>> GetMultiPleList<T1, T2>(string targetDB, string storedProcedure, DynamicParameters param = null) where T1 : class where T2 : class
        {
            using (SqlConnection connection = new SqlConnection(targetDB))
            {
                try
                {
                    connection.Open();

                    using (var output = connection.QueryMultiple(GetStoredProcedure(storedProcedure), param, commandType: CommandType.StoredProcedure))
                    {
                        IList<T1> t1 = NextResult<T1>(output);
                        IList<T2> t2 = NextResult<T2>(output);

                        return new Tuple<IList<T1>, IList<T2>>(t1, t2);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public static IList<T> NextResult<T>(GridReader resultReader) where T : class
        {
            List<T> result = null;
            if (!resultReader.IsConsumed)
            {
                result = resultReader.Read<T>(true).ToList();
            }
            return result;
        }


        // Top 1
        public static T GetTopOne<T>(string targetDB, string storedProcedure, DynamicParameters param = null) where T : class
        {
            using (SqlConnection connection = new SqlConnection(targetDB))
            {
                try
                {
                    connection.Open();
                    var output = connection.Query<T>(GetStoredProcedure(storedProcedure), param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return output;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        // Insert, Update, Delete
        public static void ExecuteProcess(string targetDB, string storedProcedure, DynamicParameters param = null)
        {
            using (SqlConnection connection = new SqlConnection(targetDB))
            {
                try
                {
                    connection.Open();
                    connection.Execute(GetStoredProcedure(storedProcedure), param, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        // 메인 서브 트랜잭션
        //public int DeleteProduct(Product product)
        //{
        //    using (IDbConnection connection = OpenConnection())
        //    {
        //        const string deleteImageQuery = "DELETE FROM Production.ProductProductPhoto " +
        //                                        "WHERE ProductID = @ProductID";
        //        const string deleteProductQuery = "DELETE FROM Production.Product " +
        //                                          "WHERE ProductID = @ProductID";

        //        IDbTransaction transaction = connection.BeginTransaction();

        //        int rowsAffected = connection.Execute(deleteImageQuery,new { ProductID = product.ProductID }, transaction);

        //        rowsAffected += connection.Execute(deleteProductQuery,new { ProductID = product.ProductID }, transaction);

        //        transaction.Commit();
        //        return rowsAffected;
        //    }
        //}
    }
}
