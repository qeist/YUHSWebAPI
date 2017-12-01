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
        public HttpResponseResult<ExamListByEx> GetExamListByEx(string unitNo, string retrGb, string inOutGb, string retrFrYmd, string retrToYmd, string slipGrGb, string ordCd, string userId)
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

                IEnumerable<ExamListByEx> info = SqlHelper.GetList<ExamListByEx>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getExamListByEx");

                return new HttpResponseResult<ExamListByEx> { result = info, error = new ErrorInfo { flag = false } };
                
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<ExamListByEx> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }

        }

        [Route("TESExamRslt/GetExamListByFrmLab/{unitNo}/{retrGb}/{inOutGb}/{retrFrYmd}/{retrToYmd}/{slipGrGb}/{ordCd}/{userId}")]
        public HttpResponseResult<ExamListByFrmLab> GetExamListByFrmLab(string unitNo, string retrGb, string inOutGb, string retrFrYmd, string retrToYmd, string slipGrGb, string ordCd, string userId)
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

                IEnumerable<ExamListByFrmLab> info = SqlHelper.GetList<ExamListByFrmLab>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getExamListByFrmLab");

                return new HttpResponseResult<ExamListByFrmLab> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<ExamListByFrmLab> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/GetExamListByHC/{unitNo}/{retrGb}/{inOutGb}/{retrFrYmd}/{retrToYmd}/{slipGrGb}/{ordCd}/{userId}")]
        public HttpResponseResult<ExamListByHC> GetExamListByHC(string unitNo, string retrGb, string inOutGb, string retrFrYmd, string retrToYmd, string slipGrGb, string ordCd, string userId)
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

                IEnumerable<ExamListByHC> info = SqlHelper.GetList<ExamListByHC>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getExamListByHC");

                return new HttpResponseResult<ExamListByHC> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<ExamListByHC> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/GetExamListByLab/{unitNo}/{retrGb}/{inOutGb}/{retrFrYmd}/{retrToYmd}/{slipGrGb}/{ordCd}/{userId}")]
        public HttpResponseResult<ExamListByLab> GetExamListByLab(string unitNo, string retrGb, string inOutGb, string retrFrYmd, string retrToYmd, string slipGrGb, string ordCd, string userId)
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

                IEnumerable<ExamListByLab> info = SqlHelper.GetList<ExamListByLab>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getExamListByLab");

                return new HttpResponseResult<ExamListByLab> { result = info, error = new ErrorInfo { flag = false } };
                
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<ExamListByLab> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }


        [Route("TESExamRslt/SelectPACSOutCDByUnitNo/{unitNo}")]
        public HttpResponseResult<PACSOutCd> SelectPACSOutCDByUnitNo(string unitNo)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.StringFixedLength, size: 10);
             
                IEnumerable<PACSOutCd> info = SqlHelper.GetList<PACSOutCd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getExamRsltIntgViewDA_SelectPACSOutCDByUnitNo");

                return new HttpResponseResult<PACSOutCd> { result = info, error = new ErrorInfo { flag = false } };

            }
            catch (Exception ex)
            {
                return new HttpResponseResult<PACSOutCd> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectChosNoCurrClnInfoByOrd/{ymdGb}/{ordYmd}/{ordExecYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<CurrClnInfo> SelectChosNoCurrClnInfoByOrd(string ymdGb, string ordYmd, string ordExecYmd,  string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@OrdExecYmd", value: ordExecYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<CurrClnInfo> info = SqlHelper.GetList<CurrClnInfo>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectChosNoCurrClnInfoByOrd");

                return new HttpResponseResult<CurrClnInfo> { result = info, error = new ErrorInfo { flag = false } };

            }
            catch (Exception ex)
            {
                return new HttpResponseResult<CurrClnInfo> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRslt/{ymdGb}/{ordYmd}/{ordExecYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<SpcNoForGnlRslt> SelectOrdRsltForGnlRslt(string ymdGb, string ordYmd, string ordExecYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@OrdExecYmd", value: ordExecYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<SpcNoForGnlRslt> info = SqlHelper.GetList<SpcNoForGnlRslt>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRslt");

                return new HttpResponseResult<SpcNoForGnlRslt> { result = info, error = new ErrorInfo { flag = false } };

            }
            catch (Exception ex)
            {
                return new HttpResponseResult<SpcNoForGnlRslt> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByCnti/{ymdGb}/{ordYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<OrdRsltForGnlRslt> SelectOrdRsltForGnlRsltByCnti(string ymdGb, string ordYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRslt> info = SqlHelper.GetList<OrdRsltForGnlRslt>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByCnti");

                return new HttpResponseResult<OrdRsltForGnlRslt> { result = info, error = new ErrorInfo { flag = false } };

            }
            catch (Exception ex)
            {
                return new HttpResponseResult<OrdRsltForGnlRslt> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByCnti_YD/{ymdGb}/{ordYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<OrdRsltForGnlRslt> SelectOrdRsltForGnlRsltByCnti_YD(string ymdGb, string ordYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRslt> info = SqlHelper.GetList<OrdRsltForGnlRslt>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByCnti_YD");

                return new HttpResponseResult<OrdRsltForGnlRslt> { result = info, error = new ErrorInfo { flag = false } };

            }
            catch (Exception ex)
            {
                return new HttpResponseResult<OrdRsltForGnlRslt> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByGnl11/{ymdGb}/{ordYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> SelectOrdRsltForGnlRsltByGnl11(string ymdGb, string ordYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRsltByGnlOrd> info = SqlHelper.GetList<OrdRsltForGnlRsltByGnlOrd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByGnl11");

                return new HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByGnl11_YD/{ymdGb}/{ordYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> SelectOrdRsltForGnlRsltByGnl11_YD(string ymdGb, string ordYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRsltByGnlOrd> info = SqlHelper.GetList<OrdRsltForGnlRsltByGnlOrd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByGnl11_YD");

                return new HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByGnl12/{ymdGb}/{ordYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> SelectOrdRsltForGnlRsltByGnl12(string ymdGb, string ordYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRsltByGnlOrd> info = SqlHelper.GetList<OrdRsltForGnlRsltByGnlOrd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByGnl12");

                return new HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByGnl12_YD/{ymdGb}/{ordYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> SelectOrdRsltForGnlRsltByGnl12_YD(string ymdGb, string ordYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRsltByGnlOrd> info = SqlHelper.GetList<OrdRsltForGnlRsltByGnlOrd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByGnl12_YD");

                return new HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByGnl2l/{ymdGb}/{ordYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> SelectOrdRsltForGnlRsltByGnl2l(string ymdGb, string ordYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRsltByGnlOrd> info = SqlHelper.GetList<OrdRsltForGnlRsltByGnlOrd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByGnl21");

                return new HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByGnl21_YD/{ymdGb}/{ordYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> SelectOrdRsltForGnlRsltByGnl21_YD(string ymdGb, string ordYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRsltByGnlOrd> info = SqlHelper.GetList<OrdRsltForGnlRsltByGnlOrd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByGnl21_YD");

                return new HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByGnl22/{ymdGb}/{ordYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> SelectOrdRsltForGnlRsltByGnl22(string ymdGb, string ordYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRsltByGnlOrd> info = SqlHelper.GetList<OrdRsltForGnlRsltByGnlOrd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByGnl22");

                return new HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByGnl22_YD/{ymdGb}/{ordYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> SelectOrdRsltForGnlRsltByGnl22_YD(string ymdGb, string ordYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRsltByGnlOrd> info = SqlHelper.GetList<OrdRsltForGnlRsltByGnlOrd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByGnl22_YD");

                return new HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByGnl31/{ymdGb}/{ordYmd}/{ordExecYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> SelectOrdRsltForGnlRsltByGnl31(string ymdGb, string ordYmd, string ordExecYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@OrdExecYmd", value: ordExecYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRsltByGnlOrd> info = SqlHelper.GetList<OrdRsltForGnlRsltByGnlOrd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByGnl31");

                return new HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByGnl31_YD/{ymdGb}/{ordYmd}/{ordExecYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> SelectOrdRsltForGnlRsltByGnl31_YD(string ymdGb, string ordYmd, string ordExecYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@OrdExecYmd", value: ordExecYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRsltByGnlOrd> info = SqlHelper.GetList<OrdRsltForGnlRsltByGnlOrd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByGnl31_YD");

                return new HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByGnl32/{ymdGb}/{ordYmd}/{ordExecYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> SelectOrdRsltForGnlRsltByGnl32(string ymdGb, string ordYmd, string ordExecYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@OrdExecYmd", value: ordExecYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRsltByGnlOrd> info = SqlHelper.GetList<OrdRsltForGnlRsltByGnlOrd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByGnl32");

                return new HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByGnl32_YD/{ymdGb}/{ordYmd}/{ordExecYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> SelectOrdRsltForGnlRsltByGnl32_YD(string ymdGb, string ordYmd, string ordExecYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@OrdExecYmd", value: ordExecYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRsltByGnlOrd> info = SqlHelper.GetList<OrdRsltForGnlRsltByGnlOrd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByGnl32_YD");

                return new HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByOrd11/{ymdGb}/{ordYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> SelectOrdRsltForGnlRsltByOrd11(string ymdGb, string ordYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRsltByGnlOrd> info = SqlHelper.GetList<OrdRsltForGnlRsltByGnlOrd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByOrd11");

                return new HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByOrd12/{ymdGb}/{ordYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> SelectOrdRsltForGnlRsltByOrd12(string ymdGb, string ordYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRsltByGnlOrd> info = SqlHelper.GetList<OrdRsltForGnlRsltByGnlOrd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByOrd12");

                return new HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByOrd21/{ymdGb}/{ordYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> SelectOrdRsltForGnlRsltByOrd21(string ymdGb, string ordYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRsltByGnlOrd> info = SqlHelper.GetList<OrdRsltForGnlRsltByGnlOrd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByOrd21");

                return new HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByOrd21_YD/{ymdGb}/{ordYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> SelectOrdRsltForGnlRsltByOrd21_YD(string ymdGb, string ordYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRsltByGnlOrd> info = SqlHelper.GetList<OrdRsltForGnlRsltByGnlOrd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByOrd21_YD");

                return new HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByOrd22/{ymdGb}/{ordYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> SelectOrdRsltForGnlRsltByOrd22(string ymdGb, string ordYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRsltByGnlOrd> info = SqlHelper.GetList<OrdRsltForGnlRsltByGnlOrd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByOrd22");

                return new HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByOrd22_YD/{ymdGb}/{ordYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> SelectOrdRsltForGnlRsltByOrd22_YD(string ymdGb, string ordYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRsltByGnlOrd> info = SqlHelper.GetList<OrdRsltForGnlRsltByGnlOrd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByOrd22_YD");

                return new HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByOrd31/{ymdGb}/{ordYmd}/{ordExecYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> SelectOrdRsltForGnlRsltByOrd31(string ymdGb, string ordYmd, string ordExecYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@OrdExecYmd", value: ordExecYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRsltByGnlOrd> info = SqlHelper.GetList<OrdRsltForGnlRsltByGnlOrd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByOrd31");

                return new HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByOrd31_YD/{ymdGb}/{ordYmd}/{ordExecYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> SelectOrdRsltForGnlRsltByOrd31_YD(string ymdGb, string ordYmd, string ordExecYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@OrdExecYmd", value: ordExecYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRsltByGnlOrd> info = SqlHelper.GetList<OrdRsltForGnlRsltByGnlOrd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByOrd31_YD");

                return new HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByOrd32/{ymdGb}/{ordYmd}/{ordExecYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> SelectOrdRsltForGnlRsltByOrd32(string ymdGb, string ordYmd, string ordExecYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@OrdExecYmd", value: ordExecYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRsltByGnlOrd> info = SqlHelper.GetList<OrdRsltForGnlRsltByGnlOrd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByOrd32");

                return new HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<OrdRsltForGnlRsltByGnlOrd> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectExRsltByRsltKey/{rsltKey}")]
        public HttpResponseResult<ExRsltByRsltKey> SelectExRsltByRsltKey(string rsltKey)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@RsltKey", value: Convert.ToInt64(rsltKey), dbType: DbType.Int64);

                IEnumerable<ExRsltByRsltKey> info = SqlHelper.GetList<ExRsltByRsltKey>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_RsltKeyDA_getSelectExRsltByRsltKey");

                return new HttpResponseResult<ExRsltByRsltKey> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<ExRsltByRsltKey> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }
    }
}
