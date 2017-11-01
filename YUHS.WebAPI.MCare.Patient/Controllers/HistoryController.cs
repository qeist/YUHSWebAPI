using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using YUHS.WebAPI.Common.DataAccess;
using YUHS.WebAPI.Common.Security;
using YUHS.WebAPI.MCare.Models.History;
using YUHS.WebAPI.MCare.Patient.Models.Common;

namespace YUHS.WebAPI.MCare.Patient.Controllers
{
    [RequireHttps]
    public class HistoryController : ApiController
    {
        [Route("History/GetVisitList/{hosCd}/{unitNo}/{frYmd}/{toYmd}/{chosGb}")]
        public HttpResponseResult<VisitList> GetVisitList(string hosCd, string unitNo, string frYmd, string toYmd, string chosGb)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@FrYmd", value: frYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@ToYmd", value: toYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@ChosGb", value: chosGb, dbType: DbType.StringFixedLength, size: 1);

                IEnumerable<VisitList> info = SqlHelper.GetList<VisitList>(targetDB: SqlHelper.GetConnectionString("HConnectionString"), storedProcedure: "USP_HP_EXT_IF_Mobile_SelectChosHis", param: param);

                return new HttpResponseResult<VisitList> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<VisitList> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }
    }
}