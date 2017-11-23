using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using YUHS.WebAPI.Common.DataAccess;
using YUHS.WebAPI.Common.Security;
using YUHS.WebAPI.MCare.Patient.Models.Common;
using YUHS.WebAPI.MCare.Patient.Models.Order;

namespace YUHS.WebAPI.MCare.Patient.Controllers
{
    [RequireHttps]
    public class OrderController : ApiController
    {
        [Route("Order/GetPrescription/{hosCd}/{unitNo}/{strYmd}/{endYmd}")]
        public HttpResponseResult<Prescription> GetPrescription(string hosCd, string unitNo, string strYmd, string endYmd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@StrYmd", value: strYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@EndYmd", value: endYmd, dbType: DbType.StringFixedLength, size: 8);

                IEnumerable<Prescription> info = SqlHelper.GetList<Prescription>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getPrescription", param: param);

                return new HttpResponseResult<Prescription> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<Prescription> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }
    }
}
