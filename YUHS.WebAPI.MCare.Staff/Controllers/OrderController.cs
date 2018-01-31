using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using YUHS.WebAPI.Common.DataAccess;
using YUHS.WebAPI.Common.Security;
using YUHS.WebAPI.MCare.Staff.Models.Common;
using YUHS.WebAPI.MCare.Staff.Models.Order;

namespace YUHS.WebAPI.MCare.Staff.Controllers
{
    [RequireHttps]
    public class OrderController : ApiController
    {
        [Route("Order/GetInPatPrescribeList/{hosCd}/{unitNo}/{admiYmd}/{ordYmd}")]
        public HttpResponseResult<IEnumerable<PatPrescribeList>> GetInPatPrescribeList(string hosCd, string unitNo, string admiYmd, string ordYmd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.StringFixedLength, size: 10);
                param.Add(name: "@AdmiYmd", value: admiYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);

                IEnumerable<PatPrescribeList> info = SqlHelper.GetList<PatPrescribeList>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getInPatPrescribeList", param: param);

                return new HttpResponseResult<IEnumerable<PatPrescribeList>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<PatPrescribeList>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }

        }

        [Route("Order/GetOutPatPrescribeList/{hosCd}/{unitNo}/{deptCd}/{ordYmd}/{outPatTyp}")]
        public HttpResponseResult<IEnumerable<PatPrescribeList>> GetOutPatPrescribeList(string hosCd, string unitNo, string deptCd, string ordYmd, string outPatTyp)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.StringFixedLength, size: 10);
                param.Add(name: "@DeptCd", value: deptCd, dbType: DbType.String, size: 6);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@OutPatTyp", value: outPatTyp, dbType: DbType.StringFixedLength, size: 1);

                IEnumerable<PatPrescribeList> info = SqlHelper.GetList<PatPrescribeList>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getOutPatPrescribeList", param: param);

                return new HttpResponseResult<IEnumerable<PatPrescribeList>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<PatPrescribeList>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }

        }
    }
}
