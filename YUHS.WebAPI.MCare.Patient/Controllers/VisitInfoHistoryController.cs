using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using YUHS.WebAPI.Common.DataAccess;
using YUHS.WebAPI.Common.Security;
using YUHS.WebAPI.MCare.Patient.Models.VisitInfoHistory;
using YUHS.WebAPI.MCare.Patient.Models.Common;

namespace YUHS.WebAPI.MCare.Patient.Controllers
{
    [RequireHttps]
    public class VisitInfoHistoryController : ApiController
    {
        [Route("VisitInfoHistory/GetBillSum/{hosCd}/{chosNo}/{unitNo}/{ymd}/{chosGb}")]
        public HttpResponseResult<BillSum> GetBillSum(string hosCd, string chosNo, string unitNo, string ymd, string chosGb)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@ChosNo", value: chosNo, dbType: DbType.String, size: 14);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@Ymd", value: ymd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@ChosGb", value: chosGb, dbType: DbType.StringFixedLength, size: 1);

                IEnumerable<BillSum> info = SqlHelper.GetList<BillSum>(targetDB: SqlHelper.GetConnectionString("HConnectionString"), storedProcedure: "USP_HP_EXT_IF_Mobile_SelectBillSum", param: param);

                return new HttpResponseResult<BillSum> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<BillSum> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("VisitInfoHistory/GetBillDetail/{hosCd}/{chosNo}/{unitNo}/{ymd}/{chosGb}/{billNo}/{acptNo}/{acptYmd}")]
        public HttpResponseResult<BillSum> GetBillDetail(string hosCd, string chosNo, string unitNo, string ymd, string chosGb, string billNo, string acptNo, string acptYmd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@ChosNo", value: chosNo, dbType: DbType.String, size: 14);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@Ymd", value: ymd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@ChosGb", value: chosGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@BillNo", value: billNo, dbType: DbType.String, size: 13);
                param.Add(name: "@AcptNo", value: acptNo, dbType: DbType.String, size: 13);
                param.Add(name: "@AcptYmd", value: acptYmd, dbType: DbType.StringFixedLength, size: 8);

                IEnumerable<BillSum> info = SqlHelper.GetList<BillSum>(targetDB: SqlHelper.GetConnectionString("HConnectionString"), storedProcedure: "USP_HP_EXT_IF_Mobile_SelectBillDetail", param: param);

                return new HttpResponseResult<BillSum> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<BillSum> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }
    }
}
