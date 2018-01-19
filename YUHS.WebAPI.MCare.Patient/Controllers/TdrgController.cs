using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using YUHS.WebAPI.Common.DataAccess;
using YUHS.WebAPI.Common.Security;
using YUHS.WebAPI.MCare.Patient.Models.Common;
using YUHS.WebAPI.MCare.Patient.Models.Tdrg;

namespace YUHS.WebAPI.MCare.Patient.Controllers
{
    [RequireHttps]
    public class TdrgController : ApiController
    {
        [Route("Tdrg/GetPatientCounseling/{hospitalCd}/{orderCd}/{languageCd}")]
        public HttpResponseResult<PatientCounseling> GetPatientCounseling(string hospitalCd, string orderCd, string languageCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@hospitalCd", value: hospitalCd, dbType: DbType.String, size: 3);
                param.Add(name: "@orderCd", value: orderCd, dbType: DbType.String, size: 100);
                param.Add(name: "@languageCd", value: languageCd, dbType: DbType.String, size: 3);

                IEnumerable <PatientCounseling> info = SqlHelper.GetList<PatientCounseling>(targetDB: SqlHelper.GetConnectionString("PConnectionString"), storedProcedure: "USP_FIRSTDIS_MOBILE_PatientCounseling", param: param);

                return new HttpResponseResult<PatientCounseling> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                //Common.Utility.Utils._WriteLog(ex.Message, $"Tdrg/{nameof(GetPatientCounseling)}");
                return new HttpResponseResult<PatientCounseling> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("Tdrg/GetPatientEducation/{hospitalCd}/{orderCd}/{languageCd}")]
        public HttpResponseResult<PatientEducation> GetPatientEducation(string hospitalCd, string orderCd, string languageCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@hospitalCd", value: hospitalCd, dbType: DbType.String, size: 3);
                param.Add(name: "@orderCd", value: orderCd, dbType: DbType.String, size: 100);
                param.Add(name: "@languageCd", value: languageCd, dbType: DbType.String, size: 3);

                IEnumerable<PatientEducation> info = SqlHelper.GetList<PatientEducation>(targetDB: SqlHelper.GetConnectionString("PConnectionString"), storedProcedure: "USP_FIRSTDIS_MOBILE_PatientEducation", param: param);

                return new HttpResponseResult<PatientEducation> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<PatientEducation> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }
    }
}
