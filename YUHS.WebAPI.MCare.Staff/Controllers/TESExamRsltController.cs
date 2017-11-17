using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using YUHS.WebAPI.Common.DataAccess;
using YUHS.WebAPI.Common.Security;
using YUHS.WebAPI.Common.Utility;
using YUHS.WebAPI.MCare.Patient.Models.Common;
using YUHS.WebAPI.MCare.Staff.Models.TESExamRslt;

namespace YUHS.WebAPI.MCare.Staff.Controllers
{
    [RequireHttps]
    public class TESExamRsltController : ApiController
    {
        [Route("TESExamRslt/GetSelectUserInfo/{unitNo}")]
        public HttpResponseResult<UserInfo> GetSelectUserInfo(string unitNo)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.StringFixedLength, size: 10);

                IEnumerable<UserInfo> info = SqlHelper.GetList<UserInfo>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getSelectUserInfo", param: param);

                return new HttpResponseResult<UserInfo> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<UserInfo> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }

        }

        [Route("TESExamRslt/GetSelectAppYmdByEGFR")]
        public HttpResponseResult<AppYmdByEGFR> GetSelectAppYmdByEGFR()
        {
            try
            {
                IEnumerable<AppYmdByEGFR> info = SqlHelper.GetList<AppYmdByEGFR>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_ExamMDA_getSelectAppYmdByEGFR");

                return new HttpResponseResult<AppYmdByEGFR> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<AppYmdByEGFR> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/GetInsertExRsltSrhHis/{unitNo}/{logInfo}/{userId}")]
        public void GetInsertExRsltSrhHis(string unitNo, string logInfo, string userId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@LogInfo", value: logInfo, dbType: DbType.String, size: 500);
                param.Add(name: "@UserId", value: userId, dbType: DbType.String, size: 8);

                SqlHelper.ExecuteProcess(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_ExamRsltIntgViewDA_getInsertExRsltSrhHis", param: param);
            }
            catch (Exception ex)
            {
                Utils._WriteLog(ex.Message, "GetInsertExRsltSrhHis");
            }

        }

        [Route("TESExamRslt/GetExamListByEx/{unitNo}/{retrGb}/{inOutGb}/{retrFrYmd}/{retrToYmd}/{slipGrGb}/{ordCd}/{userId}")]
        public void GetExamListByEx(string unitNo, string retrGb, string inOutGb, string retrFrYmd, string retrToYmd, string slipGrGb, string ordCd, string userId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.StringFixedLength, size: 10);
                param.Add(name: "@RetrGb", value: retrGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@InOutGb", value: inOutGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@RetrFrYmd", value: retrFrYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@RetrToYmd", value: retrToYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@SlipGrGb", value: slipGrGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdCd", value: ordCd, dbType: DbType.String, size: 1000);
                param.Add(name: "@UserId", value: userId, dbType: DbType.String, size: 8);
                
                SqlHelper.ExecuteProcess(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getExamListByEx", param: param);
            }
            catch (Exception ex)
            {
                Utils._WriteLog(ex.Message, "GetExamListByEx");
            }

        }

        [Route("TESExamRslt/GetExamListByFrmLab/{unitNo}/{retrGb}/{inOutGb}/{retrFrYmd}/{retrToYmd}/{slipGrGb}/{ordCd}/{userId}")]
        public void GetExamListByFrmLab(string unitNo, string retrGb, string inOutGb, string retrFrYmd, string retrToYmd, string slipGrGb, string ordCd, string userId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.StringFixedLength, size: 10);
                param.Add(name: "@RetrGb", value: retrGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@InOutGb", value: inOutGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@RetrFrYmd", value: retrFrYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@RetrToYmd", value: retrToYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@SlipGrGb", value: slipGrGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdCd", value: ordCd, dbType: DbType.String, size: 1000);
                param.Add(name: "@UserId", value: userId, dbType: DbType.String, size: 8);

                SqlHelper.ExecuteProcess(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getExamListByFrmLab", param: param);
            }
            catch (Exception ex)
            {
                Utils._WriteLog(ex.Message, "GetExamListByFrmLab");
            }
        }

        [Route("TESExamRslt/GetExamListByHC/{unitNo}/{retrGb}/{inOutGb}/{retrFrYmd}/{retrToYmd}/{slipGrGb}/{ordCd}/{userId}")]
        public void GetExamListByHC(string unitNo, string retrGb, string inOutGb, string retrFrYmd, string retrToYmd, string slipGrGb, string ordCd, string userId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.StringFixedLength, size: 10);
                param.Add(name: "@RetrGb", value: retrGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@InOutGb", value: inOutGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@RetrFrYmd", value: retrFrYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@RetrToYmd", value: retrToYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@SlipGrGb", value: slipGrGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdCd", value: ordCd, dbType: DbType.String, size: 1000);
                param.Add(name: "@UserId", value: userId, dbType: DbType.String, size: 8);

                SqlHelper.ExecuteProcess(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getExamListByHC", param: param);
            }
            catch (Exception ex)
            {
                Utils._WriteLog(ex.Message, "GetExamListByHC");
            }
        }

        [Route("TESExamRslt/GetExamListByLab/{unitNo}/{retrGb}/{inOutGb}/{retrFrYmd}/{retrToYmd}/{slipGrGb}/{ordCd}/{userId}")]
        public void GetExamListByLab(string unitNo, string retrGb, string inOutGb, string retrFrYmd, string retrToYmd, string slipGrGb, string ordCd, string userId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.StringFixedLength, size: 10);
                param.Add(name: "@RetrGb", value: retrGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@InOutGb", value: inOutGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@RetrFrYmd", value: retrFrYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@RetrToYmd", value: retrToYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@SlipGrGb", value: slipGrGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdCd", value: ordCd, dbType: DbType.String, size: 1000);
                param.Add(name: "@UserId", value: userId, dbType: DbType.String, size: 8);

                SqlHelper.ExecuteProcess(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getExamListByLab", param: param);
            }
            catch (Exception ex)
            {
                Utils._WriteLog(ex.Message, "GetExamListByLab");
            }
        }

        
    }
}
