using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using YUHS.WebAPI.Common.DataAccess;
using YUHS.WebAPI.Common.Security;
using YUHS.WebAPI.MCare.Patient.Models.Common;
using YUHS.WebAPI.MCare.Patient.Models.WaitTime;

namespace YUHS.WebAPI.MCare.Patient.Controllers
{
    [RequireHttps]
    public class WaitTimeController : ApiController
    {
        [Route("WaitTime/GetWaitingTime/{hosCd}/{unitNo}")]
        public HttpResponseResult<WaitingList> GetWaitingTime(string hosCd, string unitNo)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);

                IEnumerable<WaitingList> info = SqlHelper.GetList<WaitingList>(targetDB: SqlHelper.GetConnectionString("MConnectionString"), storedProcedure: "USP_MD_GetWaitingList", param: param);

                return new HttpResponseResult<WaitingList> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<WaitingList> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }
    }
}
