using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using YUHS.WebAPI.Common.DataAccess;
using YUHS.WebAPI.Common.Security;
using YUHS.WebAPI.MCare.Patient.Models.Common;
using YUHS.WebAPI.MCare.Patient.Models.User;

namespace YUHS.WebAPI.MCare.Patient.Controllers
{
    [RequireHttps]
    public class UserController : ApiController
    {
        
        [Route("User/GetUserInfo/{hosCd}/{unitNo}")]
        public HttpResponseResult<PatBasInfo> GetUserInfo(string hosCd, string unitNo)
        {
            var sw = new System.Diagnostics.Stopwatch();
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);

                sw.Start();
                IEnumerable<PatBasInfo> info = SqlHelper.GetList<PatBasInfo>(targetDB: SqlHelper.GetConnectionString("HConnectionString"), storedProcedure: "USP_HP_EXT_IF_Mobile_GetUserInfoByUnitNo", param: param);
                sw.Stop();

                TimeSpan ts = sw.Elapsed;
                //string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                Common.Utility.Utils._WriteLog($"RunTime {sw.ElapsedMilliseconds.ToString()}", $"User/{nameof(GetUserInfo)} {hosCd} {unitNo}");

                return new HttpResponseResult<PatBasInfo> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                Common.Utility.Utils._WriteLog(ex.Message, $"User/{nameof(GetUserInfo)}");
                return new HttpResponseResult<PatBasInfo> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("User/GetFindPatient/{hosCd}/{patNm}/{HpNo}")]
        public HttpResponseResult<PatBasInfo> GetFindPatient(string hosCd, string patNm , string hpNo)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@PatNm", value: patNm, dbType: DbType.String, size: 100);
                param.Add(name: "@HpNo", value: hpNo, dbType: DbType.String, size: 15);

                IEnumerable<PatBasInfo> info = SqlHelper.GetList<PatBasInfo>(targetDB: SqlHelper.GetConnectionString("HConnectionString"), storedProcedure: "USP_HP_EXT_IF_Mobile_SelectPatBasInfoByPatInfo", param: param);

                return new HttpResponseResult<PatBasInfo> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<PatBasInfo> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("User/SetTelNo/{hosCd}/{unitNo}/{hpNo}")]
        public HttpResponseResult<ExecuteResult> SetTelNo(string hosCd, string unitNo, string hpNo)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@HpNo", value: hpNo, dbType: DbType.String, size: 15);

                IEnumerable<ExecuteResult> info = SqlHelper.GetList<ExecuteResult>(targetDB: SqlHelper.GetConnectionString("HConnectionString"), storedProcedure: "USP_HP_EXT_IF_Mobile_UpdatePatBasInfo", param: param);

                return new HttpResponseResult<ExecuteResult> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<ExecuteResult> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("User/SetAddress/{hosCd}/{unitNo}/{zip}/{fAdr}/{lAdr}")]
        public HttpResponseResult<ExecuteResult> SetAddress(string hosCd, string unitNo, string zip, string fAdr, string lAdr)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@Zip", value: zip, dbType: DbType.StringFixedLength, size: 5);
                param.Add(name: "@Fadr", value: fAdr, dbType: DbType.String, size: 100);
                param.Add(name: "@Ladr", value: lAdr, dbType: DbType.String, size: 100);

                IEnumerable<ExecuteResult> info = SqlHelper.GetList<ExecuteResult>(targetDB: SqlHelper.GetConnectionString("HConnectionString"), storedProcedure: "USP_HP_EXT_IF_Mobile_UpdatePatBasInfo_Addr", param: param);

                return new HttpResponseResult<ExecuteResult> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<ExecuteResult> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("User/GetVisitInfoNow/{hosCd}/{unitNo}")]
        public HttpResponseResult<VisitInfoNow> GetVisitInfoNow(string hosCd, string unitNo)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                
                IEnumerable<VisitInfoNow> info = SqlHelper.GetList<VisitInfoNow>(targetDB: SqlHelper.GetConnectionString("HConnectionString"), storedProcedure: "USP_HP_EXT_IF_Mobile_SelectLocs", param: param);

                return new HttpResponseResult<VisitInfoNow> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<VisitInfoNow> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }

        }

        [Route("User/GetExtMemberInfo/{userId}/{userPwd}")]
        public HttpResponseResult<ExtMember> GetExtMemberInfo(string userId, string userPwd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@userid", value: userId, dbType: DbType.String, size: 100);
                param.Add(name: "@userpwd", value: userPwd, dbType: DbType.String, size: 255);

                IEnumerable<ExtMember> info = SqlHelper.GetList<ExtMember>(targetDB: SqlHelper.GetConnectionString("PRConnectionString"), storedProcedure: "dbo.USP_2017_getExtMemberInfo", param: param);

                return new HttpResponseResult<ExtMember> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<ExtMember> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("User/GetFindPatientId/{hospitalCd}/{patientNm}/{personalNo}")]
        public HttpResponseResult<PatientId> GetFindPatientId(string hospitalCd, string patientNm, string personalNo)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hospitalCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@PatNm", value: patientNm, dbType: DbType.String, size: 100);
                param.Add(name: "@IdNo", value: personalNo, dbType: DbType.String, size: 14);

                IEnumerable<PatientId> info = SqlHelper.GetList<PatientId>(targetDB: SqlHelper.GetConnectionString("HConnectionString"), storedProcedure: "USP_HP_EXT_IF_Mobile_SelectUnitNoByPatId", param: param);

                return new HttpResponseResult<PatientId> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<PatientId> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }

        }

        
    }
}