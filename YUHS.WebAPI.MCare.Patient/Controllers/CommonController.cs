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
    public class CommonController : ApiController
    {
        [Route("Common/SetPutSms/{hosCd?}/{unitNo?}/{patNm?}/{rcvHpNo=rcvHpNo}/{sndHpNo=sndHpNo}/{subject?}/{sndMsg=sndMsg}")]
        
        public HttpResponseResult<ExecuteResult> SetPutSms(string rcvHpNo, string sndHpNo, string sndMsg, string hosCd = null, string unitNo = null, string patNm = null, string subject = null)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@PatNm", value: patNm, dbType: DbType.String, size: 100);
                param.Add(name: "@RcvHpNo", value: rcvHpNo, dbType: DbType.String, size: 20);
                param.Add(name: "@SndHpNo", value: sndHpNo, dbType: DbType.String, size: 400);
                param.Add(name: "@Subject", value: subject, dbType: DbType.String, size: 50);
                param.Add(name: "@SndMsg", value: sndMsg, dbType: DbType.String, size: 4000);

                IEnumerable<ExecuteResult> info = SqlHelper.GetList<ExecuteResult>(targetDB: SqlHelper.GetConnectionString("HConnectionString"), storedProcedure: "USP_HP_EXT_IF_Mobile_SendSMSMMS", param: param);

                return new HttpResponseResult<ExecuteResult> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<ExecuteResult> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }
    }
}
