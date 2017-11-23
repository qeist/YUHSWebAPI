using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using YUHS.WebAPI.Common.DataAccess;
using YUHS.WebAPI.Common.Security;
using YUHS.WebAPI.Common.Utility;
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

        [Route("Nursing/GetActListByDuty/{chosNo}/{retrYmd}/{dutGb}/{hosCd}/{ward}/{paraGb}/{userId}")]
        public HttpResponseResult<ActListByDuty> GetActListByDuty(string chosNo, string retrYmd, string dutGb, string hosCd, string ward, string paraGb, string userId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@ChosNo", value: chosNo, dbType: DbType.StringFixedLength, size: 14);
                param.Add(name: "@RetrYmd", value: retrYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@DutGb", value: dutGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@Ward", value: ward, dbType: DbType.String, size: 6);
                param.Add(name: "@ParaGb", value: paraGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@UserId", value: userId, dbType: DbType.String, size: 8);

                IEnumerable<ActListByDuty> info = SqlHelper.GetList<ActListByDuty>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_SelectPatWrkShtList", param: param);

                return new HttpResponseResult<ActListByDuty> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<ActListByDuty> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }

        }

        [Route("Nursing/GetActList/{chosNo}/{paraGb1}/{ordSlipCd}/{strYmd}/{strHms}/{endYmd}/{endHms}/{userId}/{hosCd}/{ward}")]
        public HttpResponseResult<ActList> GetActList(string chosNo, string paraGb1, string ordSlipCd, string strYmd, string strHms, string endYmd, string endHms, string userId, string hosCd, string ward)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@ChosNo", value: chosNo, dbType: DbType.String, size: 14);
                param.Add(name: "@ParaGb1", value: paraGb1, dbType: DbType.String, size: 1);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);
                param.Add(name: "@StrYmd", value: strYmd, dbType: DbType.String, size: 8);
                param.Add(name: "@StrHms", value: strHms, dbType: DbType.String, size: 6);
                param.Add(name: "@EndYmd", value: endYmd, dbType: DbType.String, size: 8);
                param.Add(name: "@EndHms", value: endHms, dbType: DbType.String, size: 6);
                param.Add(name: "@UserId", value: userId, dbType: DbType.String, size: 8);
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.String, size: 2);
                param.Add(name: "@Ward", value: ward, dbType: DbType.String, size: 6);

                IEnumerable<ActList> info = SqlHelper.GetList<ActList>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_SelectWrkShtList", param: param);

                return new HttpResponseResult<ActList> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<ActList> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("Nursing/Execute/{execYn}/{ordSeq}/{drgInjExecSeq}/{userId}/{callLoc}")]
        public HttpResponseResult<VitalInfo> Execute(string execYn, Int64 ordSeq, Int16 drgInjExecSeq, string userId, string callLoc)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add(name: "@ExecYn", value: execYn, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdSeq", value: ordSeq, dbType: DbType.Int64);
                param.Add(name: "@DrgInjExecSeq", value: drgInjExecSeq, dbType: DbType.Int16);
                param.Add(name: "@UserId", value: userId, dbType: DbType.String, size: 8);
                param.Add(name: "@CallLoc", value: callLoc, dbType: DbType.StringFixedLength, size: 1);

                IEnumerable<VitalInfo> info = SqlHelper.GetList<VitalInfo>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_DrgInjExecDA_UpdateWrkShtExec", param: param);

                return new HttpResponseResult<VitalInfo> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<VitalInfo> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("Nursing/ExecuteAfterChange/{ordSeq}/{drgInjExecSeq}/{drgInjExecStrYmd}/{drgInjExecStrHm}/{hmChgResn}/{userId}/{callLoc}")]
        public HttpResponseResult<VitalInfo> ExecuteAfterChange(Int64 ordSeq, Int16 drgInjExecSeq, string drgInjExecStrYmd, string drgInjExecStrHm, string hmChgResn, string userId, string callLoc)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add(name: "@OrdSeq", value: ordSeq, dbType: DbType.Int64);
                param.Add(name: "@DrgInjExecSeq", value: drgInjExecSeq, dbType: DbType.Int16);
                param.Add(name: "@DrgInjExecStrYmd", value: drgInjExecStrYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@DrgInjExecStrHm", value: drgInjExecStrHm, dbType: DbType.StringFixedLength, size: 4);
                param.Add(name: "@HmChgResn", value: hmChgResn, dbType: DbType.String, size: 500);
                param.Add(name: "@UserId", value: userId, dbType: DbType.String, size: 8);
                param.Add(name: "@CallLoc", value: callLoc, dbType: DbType.StringFixedLength, size: 1);

                IEnumerable<VitalInfo> info = SqlHelper.GetList<VitalInfo>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_DrgInjExecDA_UpdateWrkShtHmChg", param: param);

                return new HttpResponseResult<VitalInfo> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<VitalInfo> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("Nursing/ExecuteIvStart/{ordSeq}/{drgInjExecSeq}/{drgInjExecStrYmd}/{drgInjExecStrHm}/{userId}/{callLoc}")]
        public HttpResponseResult<VitalInfo> ExecuteIvStart(Int64 ordSeq, Int16 drgInjExecSeq, string drgInjExecStrYmd, string drgInjExecStrHm, string userId, string callLoc)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add(name: "@OrdSeq", value: ordSeq, dbType: DbType.Int64);
                param.Add(name: "@DrgInjExecSeq", value: drgInjExecSeq, dbType: DbType.Int16);
                param.Add(name: "@DrgInjExecStrYmd", value: drgInjExecStrYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@DrgInjExecStrHm", value: drgInjExecStrHm, dbType: DbType.StringFixedLength, size: 4);
                param.Add(name: "@UserId", value: userId, dbType: DbType.String, size: 8);
                param.Add(name: "@CallLoc", value: callLoc, dbType: DbType.StringFixedLength, size: 1);

                IEnumerable<VitalInfo> info = SqlHelper.GetList<VitalInfo>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_DrgInjExecDA_UpdateWrkShtInjExecStr", param: param);

                return new HttpResponseResult<VitalInfo> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<VitalInfo> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("Nursing/ExecuteIvClnObsRecIVF/{grNo}/{chosNo}/{msmtEndGb}/{ActDt}/{msmtVal}/{rgtId}")]
        public void ExecuteIvClnObsRecIVF(string grNo, string chosNo, string msmtEndGb, string actDt, string msmtVal, string rgtId)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add(name: "@GrNo", value: grNo, dbType: DbType.String, size: 30);
                param.Add(name: "@ChosNo", value: chosNo, dbType: DbType.StringFixedLength, size: 14);
                param.Add(name: "@MsmtEndGb", value: msmtEndGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@ActDt", value: actDt, dbType: DbType.String, size: 16);
                param.Add(name: "@MsmtVal", value: msmtVal, dbType: DbType.String, size: 50);
                param.Add(name: "@RgtId", value: rgtId, dbType: DbType.String, size: 8);

                SqlHelper.ExecuteProcess(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_InsertUpdateClnObsRecIVF", param: param);
            }
            catch (Exception ex)
            {
                Utils._WriteLog(ex.Message, "GetInsertExRsltSrhHis");
            }
        }


        [Route("Nursing/ExecuteIvEnd/{ordSeq}/{drgInjExecSeq}/{injDuseQty}/{execEndYmd}/{execEndHm}/{userId}/{callLoc}")]
        public HttpResponseResult<VitalInfo> ExecuteIvEnd(Int64 ordSeq, Int16 drgInjExecSeq, float injDuseQty, string execEndYmd, string execEndHm, string userId, string callLoc)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add(name: "@OrdSeq", value: ordSeq, dbType: DbType.Int64);
                param.Add(name: "@DrgInjExecSeq", value: drgInjExecSeq, dbType: DbType.Int16);
                param.Add(name: "@InjDuseQty", value: injDuseQty, dbType: DbType.VarNumeric, precision: 10, scale:3);
                param.Add(name: "@ExecEndYmd", value: execEndYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@ExecEndHm", value: execEndHm, dbType: DbType.StringFixedLength, size: 4);
                param.Add(name: "@UserId", value: userId, dbType: DbType.String, size: 8);
                param.Add(name: "@CallLoc", value: callLoc, dbType: DbType.StringFixedLength, size: 1);

                IEnumerable<VitalInfo> info = SqlHelper.GetList<VitalInfo>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_DrgInjExecDA_UpdateWrkShtInjExecEnd", param: param);

                return new HttpResponseResult<VitalInfo> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<VitalInfo> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("Nursing/AmpleOrdCd/{ordSeq}")]
        public HttpResponseResult<AmpleOrdCd> AmpleOrdCd(string ordSeq)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add(name: "@OrdSeq", value: ordSeq, dbType: DbType.Int64);

                IEnumerable<AmpleOrdCd> info = SqlHelper.GetList<AmpleOrdCd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_TretMatDA_SelectAddMtlOrdExec", param: param);

                return new HttpResponseResult<AmpleOrdCd> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<AmpleOrdCd> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }

        }


    }
}
