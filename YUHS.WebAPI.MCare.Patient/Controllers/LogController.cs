using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using YUHS.WebAPI.Common.DataAccess;
using YUHS.WebAPI.Common.Security;
using YUHS.WebAPI.MCare.Patient.Models.Common;

namespace YUHS.WebAPI.MCare.Patient.Controllers
{
    [RequireHttps]
    public class LogController : ApiController
    {
        [Route("Log/SetPaymentLog/{chosNo?}/{unitNo?}/{logGb?}/{appFrYmd?}/{appToYmd?}/{logNm?}/{logDesc?}/{userId?}/{chosGb?}/{bilNo?}/{logDesc2?}/{logDesc3?}/{inLogYmd?}/{inLogHms?}")]
        public HttpResponseResult<ExecuteResult> SetPaymentLog(string chosNo, string unitNo, string logGb, string appFrYmd, string appToYmd, string logNm, string logDesc, string userId, string chosGb, string bilNo, string logDesc2, string logDesc3, string inLogYmd, string inLogHms)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add(name: "@ChosNo", value: chosNo, dbType: DbType.String, size: 14);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.StringFixedLength, size: 10);
                param.Add(name: "@LogGb", value: logGb, dbType: DbType.String, size: 10);
                param.Add(name: "@AppFrYmd", value: appFrYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@AppToYmd", value: appToYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@LogNm", value: logNm, dbType: DbType.String, size: 100);
                param.Add(name: "@LogDesc", value: logDesc, dbType: DbType.String, size: 500);
                param.Add(name: "@UserId", value: userId, dbType: DbType.String, size: 8);
                param.Add(name: "@ChosGb", value: chosGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@BilNo", value: bilNo, dbType: DbType.StringFixedLength, size: 13);
                param.Add(name: "@LogDesc2", value: logDesc2, dbType: DbType.String, size: 500);
                param.Add(name: "@LogDesc3", value: logDesc3, dbType: DbType.String, size: 500);
                param.Add(name: "@InLogYmd", value: inLogYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@InLogHms", value: inLogHms, dbType: DbType.StringFixedLength, size: 6);


                SqlHelper.ExecuteProcess(targetDB: SqlHelper.GetConnectionString("HConnectionString"), storedProcedure: "USP_HP_ZZZ_GE_PgmLogDA_InsertPgmLog", param: param);
                IEnumerable<ExecuteResult> info = new List<ExecuteResult>();
                return new HttpResponseResult<ExecuteResult> { result = info, error = new ErrorInfo { flag = false } };

            }
            catch (Exception ex)
            {
                return new HttpResponseResult<ExecuteResult> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }

        }
    }
}
