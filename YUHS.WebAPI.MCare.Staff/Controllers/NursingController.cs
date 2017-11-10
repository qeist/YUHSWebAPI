using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using YUHS.WebAPI.Common.DataAccess;
using YUHS.WebAPI.Common.Security;
using YUHS.WebAPI.MCare.Patient.Models.Common;
using YUHS.WebAPI.MCare.Staff.Models.Nusing;

namespace YUHS.WebAPI.MCare.Staff.Controllers
{
    [RequireHttps]
    public class NursingController : ApiController
    {
        [Route("Nursing/AddVitalInfo/{chosNo}/{actDt}/{chosGb}/{itmCd}/{msmtVal}/{exLocCd}/{rgtId}/{lstUpdId}")]
        public HttpResponseResult<VitalInfo> AddVitalInfo(string chosNo, string actDt, string chosGb, string itmCd, string msmtVal, string exLocCd, string rgtId, string lstUpdId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@ChosNo", value: chosNo, dbType: DbType.StringFixedLength, size: 14);
                param.Add(name: "@ActDt", value: actDt, dbType: DbType.String, size: 16);
                param.Add(name: "@ChosGb", value: chosGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@ItmCd", value: itmCd, dbType: DbType.String, size: 30);
                param.Add(name: "@MsmtVal", value: msmtVal, dbType: DbType.String, size: 50);
                param.Add(name: "@ExLocCd", value: exLocCd, dbType: DbType.StringFixedLength, size: 6);
                param.Add(name: "@RgtId", value: rgtId, dbType: DbType.String, size: 8);
                param.Add(name: "@LstUpdId", value: lstUpdId, dbType: DbType.String, size: 8);

                IEnumerable<VitalInfo> info = SqlHelper.GetList<VitalInfo>(targetDB: SqlHelper.GetConnectionString("EConnectionString"), storedProcedure: "USP_MR_INF_BZ_InterfaceDA_SavePatPrClnObsRecEqupByMobile", param: param);

                return new HttpResponseResult<VitalInfo> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<VitalInfo> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("Nursing/GetVitalList/{chosNo}/{RetrYmd}")]
        public HttpResponseResult<VitalList> GetVitalList(string chosNo, string retrYmd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@ChosNo", value: chosNo, dbType: DbType.StringFixedLength, size: 14);
                param.Add(name: "@RetrYmd", value: retrYmd, dbType: DbType.StringFixedLength, size: 10);

                IEnumerable<VitalList> info = SqlHelper.GetList<VitalList>(targetDB: SqlHelper.GetConnectionString("EConnectionString"), storedProcedure: "USP_MR_FOR_NA_EmrClnObsRecDA_SelectClnObsRecByMobile", param: param);

                return new HttpResponseResult<VitalList> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<VitalList> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("Nursing/GetMedicationHistory/{chosNo}/{chosGb}/{retrTypGb}/{dsgRetrGb}/{divRetrGb}/{ordGbRetrGb}/{ordKindRetrGb}/{medRecRetrGb}/{ordSeq}/{strYmd}/{endYmd}/{clnDeptCd}/{regimenOrd}")]
        public HttpResponseResult<MedicationHistory> GetMedicationHistory(string chosNo, string chosGb, string retrTypGb, string dsgRetrGb, string divRetrGb, string ordGbRetrGb, string ordKindRetrGb, string medRecRetrGb, string ordSeq, string strYmd, string endYmd, string clnDeptCd, string regimenOrd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@ChosNo", value: chosNo, dbType: DbType.StringFixedLength, size: 14);
                param.Add(name: "@ChosGb", value: chosGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@RetrTypGb", value: retrTypGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@DsgRetrGb", value: dsgRetrGb, dbType: DbType.String, size: 50);
                param.Add(name: "@DivRetrGb", value: divRetrGb, dbType: DbType.String, size: 50);
                param.Add(name: "@OrdGbRetrGb", value: ordGbRetrGb, dbType: DbType.String, size: 50);
                param.Add(name: "@OrdKindRetrGb", value: ordKindRetrGb, dbType: DbType.String, size: 50);
                param.Add(name: "@MedRecRetrGb", value: medRecRetrGb, dbType: DbType.String, size: 2000);
                param.Add(name: "@OrdSeq", value: ordSeq, dbType: DbType.Int64);
                param.Add(name: "@StrYmd", value: strYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@EndYmd", value: endYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@ClnDeptCd", value: clnDeptCd, dbType: DbType.String, size: 6);
                param.Add(name: "@RegimenOrd", value: regimenOrd, dbType: DbType.StringFixedLength, size: 1);

                IEnumerable<MedicationHistory> info = SqlHelper.GetList<MedicationHistory>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_get_SelectMedRecList", param: param);

                return new HttpResponseResult<MedicationHistory> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<MedicationHistory> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }
    }
}
