using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using YUHS.WebAPI.Common.DataAccess;
using YUHS.WebAPI.Common.Security;
using YUHS.WebAPI.MCare.Patient.Models.Common;
using YUHS.WebAPI.MCare.Staff.Models.InRound;

namespace YUHS.WebAPI.MCare.Staff.Controllers
{
    [RequireHttps]
    public class InRoundController : ApiController
    {
        [Route("InRound/ExecuteRoundTreat/{hosCd}/{unitNo}/{clnYmd}/{clnHms}/{comment}/{sysYmd}/{sysHms}/{deptCd}/{userId}")]
        public HttpResponseResult<RoundTreat> ExecuteRoundTreat(string hosCd, string unitNo, string clnYmd, string clnHms, string comment, string sysYmd, string sysHms, string deptCd, string userId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.StringFixedLength, size: 10);
                param.Add(name: "@ClnYmd", value: clnYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@ClnHms", value: clnHms, dbType: DbType.StringFixedLength, size: 6);
                param.Add(name: "@Comment", value: comment, dbType: DbType.String, size: 500);
                param.Add(name: "@SysYmd", value: sysYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@SysHms", value: sysHms, dbType: DbType.StringFixedLength, size: 6);
                param.Add(name: "@DeptCd", value: deptCd, dbType: DbType.String, size: 6);
                param.Add(name: "@UserId", value: userId, dbType: DbType.String, size: 8);

                IEnumerable<RoundTreat> info = SqlHelper.GetList<RoundTreat>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_executeRoundTreat", param: param);

                return new HttpResponseResult<RoundTreat> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<RoundTreat> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }

        }
    }
}
