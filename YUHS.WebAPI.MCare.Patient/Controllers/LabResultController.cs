using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using YUHS.WebAPI.Common.DataAccess;
using YUHS.WebAPI.Common.Security;
using YUHS.WebAPI.MCare.Patient.Models.Common;
using YUHS.WebAPI.MCare.Patient.Models.LabResult;

namespace YUHS.WebAPI.MCare.Patient.Controllers
{
    [RequireHttps]
    public class LabResultController : ApiController
    {
        [Route("LabResult/GetLabResult/{hospitalCd}/{patientId}/{startDt}/{endDt}")]
        public HttpResponseResult<LabResult> GetLabResult(string hospitalCd, string patientId, string startDt, string endDt)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@hospitalCd", value: hospitalCd, dbType: DbType.String, size: 6);
                param.Add(name: "@patientId", value: patientId, dbType: DbType.StringFixedLength, size: 10);
                param.Add(name: "@startDt", value: startDt, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@endDt", value: endDt, dbType: DbType.StringFixedLength, size: 8);

                IEnumerable<LabResult> info = SqlHelper.GetList<LabResult>(targetDB: SqlHelper.GetConnectionString("SConnectionString"), storedProcedure: "USP_SP_IF_Mobile_SelectLabItemList", param: param);

                return new HttpResponseResult<LabResult> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<LabResult> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }

        }
    }
}
