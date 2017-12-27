using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using YUHS.WebAPI.Common.DataAccess;
using YUHS.WebAPI.Common.Security;
using YUHS.WebAPI.MCare.Patient.Models.CheckUp;
using YUHS.WebAPI.MCare.Patient.Models.Common;

namespace YUHS.WebAPI.MCare.Patient.Controllers
{
    [RequireHttps]
    public class CheckUpController : ApiController
    {
        [Route("CheckUp/GetCheckupDate/{hospitalCd}/{patientId}/{inputDt}")]
        public HttpResponseResult<CheckupDate> GetCheckupDate(string hospitalCd, string patientId, string inputDt)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@hospitalCd", value: hospitalCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@patientId", value: patientId, dbType: DbType.String, size: 10);
                param.Add(name: "@inputDt", value: inputDt, dbType: DbType.StringFixedLength, size: 8);


                IEnumerable<CheckupDate> info = SqlHelper.GetList<CheckupDate>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getCheckupDate", param: param);

                return new HttpResponseResult<CheckupDate> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<CheckupDate> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }

        }

        [Route("CheckUp/GetCheckupList/{hospitalCd}/{visitNo}")]
        public HttpResponseResult<CheckupList> GetCheckupList(string hospitalCd, string visitNo)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@hospitalCd", value: hospitalCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@visitNo", value: visitNo, dbType: DbType.String, size: 14);

                IEnumerable<CheckupList> info = SqlHelper.GetList<CheckupList>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getCheckupList", param: param);

                return new HttpResponseResult<CheckupList> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<CheckupList> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }

        }

        [Route("CheckUp/GetCheckupResult/{hospitalCd}/{visitNo}")]
        public HttpResponseResult<CheckupResult> GetCheckupResult(string hospitalCd, string visitNo)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@hospitalCd", value: hospitalCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@visitNo", value: visitNo, dbType: DbType.String, size: 14);

                IEnumerable<CheckupResult> info = SqlHelper.GetList<CheckupResult>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getCheckupResult", param: param);

                return new HttpResponseResult<CheckupResult> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<CheckupResult> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }

        }

        [Route("CheckUp/GetCheckupOpinion/{hospitalCd}/{visitNo}")]
        public HttpResponseResult<CheckupOpinion> GetCheckupOpinion(string hospitalCd, string visitNo)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@hospitalCd", value: hospitalCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@visitNo", value: visitNo, dbType: DbType.String, size: 14);

                IEnumerable<CheckupOpinion> info = SqlHelper.GetList<CheckupOpinion>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getCheckupOpinion", param: param);

                return new HttpResponseResult<CheckupOpinion> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<CheckupOpinion> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }

        }

        [Route("CheckUp/GetMetabolicInfo/{hospitalCd}/{patientId}/{checkupDt}")]
        public HttpResponseResult<MetabolicInfo> GetMetabolicInfo(string hospitalCd, string patientId, string checkupDt)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@hospitalCd", value: hospitalCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@patientId", value: patientId, dbType: DbType.String, size: 10);
                param.Add(name: "@checkupDt", value: checkupDt, dbType: DbType.StringFixedLength, size: 8);

                IEnumerable<MetabolicInfo> info = SqlHelper.GetList<MetabolicInfo>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getMetabolicInfo", param: param);

                return new HttpResponseResult<MetabolicInfo> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<MetabolicInfo> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("CheckUp/GetPatientInfoSurvey/{hospitalCd}/{patientId}")]
        public HttpResponseResult<PatientInfoSurvey> GetPatientInfoSurvey(string hospitalCd, string patientId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@hospitalCd", value: hospitalCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@patientId", value: patientId, dbType: DbType.String, size: 10);

                IEnumerable<PatientInfoSurvey> info = SqlHelper.GetList<PatientInfoSurvey>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getPatientInfoSurvey", param: param);

                return new HttpResponseResult<PatientInfoSurvey> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<PatientInfoSurvey> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("CheckUp/GetMasterCodeSurvey/{hospitalCd}")]
        public HttpResponseResult<MasterCodeSurvey> GetMasterCodeSurvey(string hospitalCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@hospitalCd", value: hospitalCd, dbType: DbType.StringFixedLength, size: 2);

                IEnumerable<MasterCodeSurvey> info = SqlHelper.GetList<MasterCodeSurvey>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getMasterCodeSurvey", param: param);

                return new HttpResponseResult<MasterCodeSurvey> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<MasterCodeSurvey> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("CheckUp/GetGroupCodeSurvey/{hospitalCd}")]
        public HttpResponseResult<GroupCodeSurvey> GetGroupCodeSurvey(string hospitalCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@hospitalCd", value: hospitalCd, dbType: DbType.StringFixedLength, size: 2);

                IEnumerable<GroupCodeSurvey> info = SqlHelper.GetList<GroupCodeSurvey>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getGroupCodeSurvey", param: param);

                return new HttpResponseResult<GroupCodeSurvey> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<GroupCodeSurvey> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("CheckUp/SaveCheckupSurvey/{hospitalCd}/{chosNo}/{grCd}/{papWeiList}/{userId}")]
        public HttpResponseResult<ExecuteResult> SaveCheckupSurvey(string hospitalCd, string chosNo, string grCd, string papWeiList, string userId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@hospitalCd", value: hospitalCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@ChosNo", value: chosNo, dbType: DbType.String, size: 14);
                param.Add(name: "@GrCd", value: grCd, dbType: DbType.String, size: 10);
                param.Add(name: "@PapWeiList", value: papWeiList, dbType: DbType.String);
                param.Add(name: "@UserId", value: userId, dbType: DbType.StringFixedLength, size: 8);

                SqlHelper.ExecuteProcess(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_saveCheckupSurvey", param: param);
                IEnumerable<ExecuteResult> info = new List<ExecuteResult>();
                return new HttpResponseResult<ExecuteResult> { result = info, error = new ErrorInfo { flag = false } };

            }
            catch (Exception ex)
            {
                return new HttpResponseResult<ExecuteResult> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }
    }
}
