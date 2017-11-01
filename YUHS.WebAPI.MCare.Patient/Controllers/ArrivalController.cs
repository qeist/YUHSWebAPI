using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using YUHS.WebAPI.Common.DataAccess;
using YUHS.WebAPI.Common.Security;
using YUHS.WebAPI.MCare.Patient.Models.Arrive;
using YUHS.WebAPI.MCare.Patient.Models.Common;

namespace YUHS.WebAPI.MCare.Patient.Controllers
{
    [RequireHttps]
    public class ArrivalController : ApiController
    {
        [Route("Arrival/GetArriveConfirm/{hosCd}/{unitNo}")]
        public HttpResponseResult<ArriveConfirmTargetList> GetArriveConfirm(string hosCd, string unitNo)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 8);

                IEnumerable<ArriveConfirmTargetList> info = SqlHelper.GetList<ArriveConfirmTargetList>(targetDB: SqlHelper.GetConnectionString("MConnectionString"), storedProcedure: "USP_MD_ArriveConfirmTargetList", param: param);

                return new HttpResponseResult<ArriveConfirmTargetList> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<ArriveConfirmTargetList> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("Arrival/SetArriveConfirm/{hosCd}/{unitNo}/{clnDeptCd}/{visitDt}/{stateCd}")]
        public HttpResponseResult<ArrivedConfirm> SetArriveConfirm(string hosCd, string unitNo, string clnDeptCd, string visitDt, string stateCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 8);
                param.Add(name: "@ClnDeptCd", value: clnDeptCd, dbType: DbType.String, size: 6);
                param.Add(name: "@VisitDt", value: visitDt, dbType: DbType.String, size: 8);
                param.Add(name: "@StateCd", value: stateCd, dbType: DbType.StringFixedLength, size: 1);

                IEnumerable<ArrivedConfirm> info = SqlHelper.GetList<ArrivedConfirm>(targetDB: SqlHelper.GetConnectionString("MConnectionString"), storedProcedure: "USP_MD_ReqArrivedConfirm", param: param);

                return new HttpResponseResult<ArrivedConfirm> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<ArrivedConfirm> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }
    }
}
