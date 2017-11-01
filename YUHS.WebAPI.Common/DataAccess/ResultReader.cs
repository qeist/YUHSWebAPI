using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace YUHS.WebAPI.Common.DataAccess
{
    public static class ResultReader
    {
        private static Type dictionaryType = typeof(IDictionary<string, object>);

        public static SqlMapper.GridReader gridReader;
        public static IDbConnection connection;

        /// <summary>
        /// Flag indication if there are more datasets.
        /// </summary>
        public static bool HasMoreResults
        {
            get { return !gridReader.IsConsumed; }
        }

        /// <summary>
        /// Get next data set
        /// </summary>
        /// <typeparam name="T">Type of return data</typeparam>
        /// <returns>Array of return data</returns>
        public static IList<T> NextResult<T>()
        {
            if (typeof(T) == dictionaryType)
            {
                var dataDynamic = gridReader.Read(true);
                return dataDynamic.Select(d => (T)d).ToList();
            }
            return gridReader.Read<T>(true).ToList();
        }
    }
}
