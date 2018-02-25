using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using YUHS.WebAPI.Common.DataAccess;
using YUHS.WebAPI.Common.Security;
using YUHS.WebAPI.Common.Utility;
using YUHS.WebAPI.MCare.Staff.Models.Common;
using YUHS.WebAPI.MCare.Staff.Models.TESExamRslt;

namespace YUHS.WebAPI.MCare.Staff.Controllers
{
    [RequireHttps]
    public class TESExamRsltController : ApiController
    {
        [Route("TESExamRslt/GetSelectUserInfo/{unitNo}")]
        public HttpResponseResult<IEnumerable<UserInfo>> GetSelectUserInfo(string unitNo)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.StringFixedLength, size: 10);

                IEnumerable<UserInfo> info = SqlHelper.GetList<UserInfo>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getSelectUserInfo", param: param);

                return new HttpResponseResult<IEnumerable<UserInfo>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<UserInfo>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }

        }

        [Route("TESExamRslt/GetSelectAppYmdByEGFR")]
        public HttpResponseResult<IEnumerable<AppYmdByEGFR>> GetSelectAppYmdByEGFR()
        {
            try
            {
                IEnumerable<AppYmdByEGFR> info = SqlHelper.GetList<AppYmdByEGFR>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_ExamMDA_getSelectAppYmdByEGFR");

                return new HttpResponseResult<IEnumerable<AppYmdByEGFR>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<AppYmdByEGFR>> { error = new ErrorInfo { flag = true, message = ex.Message } };
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
        public HttpResponseResult<IEnumerable<ExamListByEx>> GetExamListByEx(string unitNo, string retrGb, string inOutGb, string retrFrYmd, string retrToYmd, string slipGrGb, string ordCd, string userId)
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

                IEnumerable<ExamListByEx> info = SqlHelper.GetList<ExamListByEx>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getExamListByEx", param: param);

                return new HttpResponseResult<IEnumerable<ExamListByEx>> { result = info, error = new ErrorInfo { flag = false } };
                
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<ExamListByEx>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }

        }

        [Route("TESExamRslt/GetExamListByFrmLab/{unitNo}/{retrGb}/{inOutGb}/{retrFrYmd}/{retrToYmd}/{slipGrGb}/{ordCd}/{userId}")]
        public HttpResponseResult<IEnumerable<ExamListByFrmLab>> GetExamListByFrmLab(string unitNo, string retrGb, string inOutGb, string retrFrYmd, string retrToYmd, string slipGrGb, string ordCd, string userId)
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

                IEnumerable<ExamListByFrmLab> info = SqlHelper.GetList<ExamListByFrmLab>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getExamListByFrmLab", param: param);

                return new HttpResponseResult<IEnumerable<ExamListByFrmLab>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<ExamListByFrmLab>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/GetExamListByHC/{unitNo}/{retrGb}/{inOutGb}/{retrFrYmd}/{retrToYmd}/{slipGrGb}/{ordCd}/{userId}")]
        public HttpResponseResult<IEnumerable<ExamListByHC>> GetExamListByHC(string unitNo, string retrGb, string inOutGb, string retrFrYmd, string retrToYmd, string slipGrGb, string ordCd, string userId)
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

                IEnumerable<ExamListByHC> info = SqlHelper.GetList<ExamListByHC>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getExamListByHC", param: param);

                return new HttpResponseResult<IEnumerable<ExamListByHC>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<ExamListByHC>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/GetExamListByLab/{unitNo}/{retrGb}/{inOutGb}/{retrFrYmd}/{retrToYmd}/{slipGrGb}/{ordCd}/{userId}")]
        public HttpResponseResult<IEnumerable<ExamListByLab>> GetExamListByLab(string unitNo, string retrGb, string inOutGb, string retrFrYmd, string retrToYmd, string slipGrGb, string ordCd, string userId)
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

                IEnumerable<ExamListByLab> info = SqlHelper.GetList<ExamListByLab>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getExamListByLab", param: param);

                return new HttpResponseResult<IEnumerable<ExamListByLab>> { result = info, error = new ErrorInfo { flag = false } };
                
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<ExamListByLab>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }


        [Route("TESExamRslt/SelectPACSOutCDByUnitNo/{unitNo}")]
        public HttpResponseResult<IEnumerable<PACSOutCd>> SelectPACSOutCDByUnitNo(string unitNo)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.StringFixedLength, size: 10);
             
                IEnumerable<PACSOutCd> info = SqlHelper.GetList<PACSOutCd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getExamRsltIntgViewDA_SelectPACSOutCDByUnitNo", param: param);

                return new HttpResponseResult<IEnumerable<PACSOutCd>> { result = info, error = new ErrorInfo { flag = false } };

            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<PACSOutCd>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectChosNoCurrClnInfoByOrd/{ymdGb}/{ordYmd}/{ordExecYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<IEnumerable<CurrClnInfo>> SelectChosNoCurrClnInfoByOrd(string ymdGb, string ordYmd, string ordExecYmd,  string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@OrdExecYmd", value: ordExecYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<CurrClnInfo> info = SqlHelper.GetList<CurrClnInfo>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectChosNoCurrClnInfoByOrd", param: param);

                return new HttpResponseResult<IEnumerable<CurrClnInfo>> { result = info, error = new ErrorInfo { flag = false } };

            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<CurrClnInfo>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRslt/{ymdGb}/{ordYmd}/{ordExecYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<IEnumerable<SpcNoForGnlRslt>> SelectOrdRsltForGnlRslt(string ymdGb, string ordYmd, string ordExecYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@OrdExecYmd", value: ordExecYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<SpcNoForGnlRslt> info = SqlHelper.GetList<SpcNoForGnlRslt>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRslt", param: param);

                return new HttpResponseResult<IEnumerable<SpcNoForGnlRslt>> { result = info, error = new ErrorInfo { flag = false } };

            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<SpcNoForGnlRslt>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByCnti/{ymdGb}/{ordYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<IEnumerable<OrdRsltForGnlRslt>> SelectOrdRsltForGnlRsltByCnti(string ymdGb, string ordYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRslt> info = SqlHelper.GetList<OrdRsltForGnlRslt>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByCnti", param: param);

                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRslt>> { result = info, error = new ErrorInfo { flag = false } };

            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRslt>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByCnti_YD/{ymdGb}/{ordYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<IEnumerable<OrdRsltForGnlRslt>> SelectOrdRsltForGnlRsltByCnti_YD(string ymdGb, string ordYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRslt> info = SqlHelper.GetList<OrdRsltForGnlRslt>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByCnti_YD", param: param);

                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRslt>> { result = info, error = new ErrorInfo { flag = false } };

            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRslt>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByGnl11/{ymdGb}/{ordYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> SelectOrdRsltForGnlRsltByGnl11(string ymdGb, string ordYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRsltByGnlOrd> info = SqlHelper.GetList<OrdRsltForGnlRsltByGnlOrd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByGnl11", param: param);

                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByGnl11_YD/{ymdGb}/{ordYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> SelectOrdRsltForGnlRsltByGnl11_YD(string ymdGb, string ordYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRsltByGnlOrd> info = SqlHelper.GetList<OrdRsltForGnlRsltByGnlOrd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByGnl11_YD", param: param);

                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByGnl12/{ymdGb}/{ordYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> SelectOrdRsltForGnlRsltByGnl12(string ymdGb, string ordYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRsltByGnlOrd> info = SqlHelper.GetList<OrdRsltForGnlRsltByGnlOrd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByGnl12", param: param);

                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByGnl12_YD/{ymdGb}/{ordYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> SelectOrdRsltForGnlRsltByGnl12_YD(string ymdGb, string ordYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRsltByGnlOrd> info = SqlHelper.GetList<OrdRsltForGnlRsltByGnlOrd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByGnl12_YD", param: param);

                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByGnl2l/{ymdGb}/{ordYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> SelectOrdRsltForGnlRsltByGnl2l(string ymdGb, string ordYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRsltByGnlOrd> info = SqlHelper.GetList<OrdRsltForGnlRsltByGnlOrd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByGnl21", param: param);

                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByGnl21_YD/{ymdGb}/{ordYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> SelectOrdRsltForGnlRsltByGnl21_YD(string ymdGb, string ordYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRsltByGnlOrd> info = SqlHelper.GetList<OrdRsltForGnlRsltByGnlOrd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByGnl21_YD", param: param);

                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByGnl22/{ymdGb}/{ordYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> SelectOrdRsltForGnlRsltByGnl22(string ymdGb, string ordYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRsltByGnlOrd> info = SqlHelper.GetList<OrdRsltForGnlRsltByGnlOrd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByGnl22", param: param);

                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByGnl22_YD/{ymdGb}/{ordYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> SelectOrdRsltForGnlRsltByGnl22_YD(string ymdGb, string ordYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRsltByGnlOrd> info = SqlHelper.GetList<OrdRsltForGnlRsltByGnlOrd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByGnl22_YD", param: param);

                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByGnl31/{ymdGb}/{ordYmd}/{ordExecYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> SelectOrdRsltForGnlRsltByGnl31(string ymdGb, string ordYmd, string ordExecYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@OrdExecYmd", value: ordExecYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRsltByGnlOrd> info = SqlHelper.GetList<OrdRsltForGnlRsltByGnlOrd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByGnl31", param: param);

                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByGnl31_YD/{ymdGb}/{ordYmd}/{ordExecYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> SelectOrdRsltForGnlRsltByGnl31_YD(string ymdGb, string ordYmd, string ordExecYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@OrdExecYmd", value: ordExecYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRsltByGnlOrd> info = SqlHelper.GetList<OrdRsltForGnlRsltByGnlOrd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByGnl31_YD", param: param);

                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByGnl32/{ymdGb}/{ordYmd}/{ordExecYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> SelectOrdRsltForGnlRsltByGnl32(string ymdGb, string ordYmd, string ordExecYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@OrdExecYmd", value: ordExecYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRsltByGnlOrd> info = SqlHelper.GetList<OrdRsltForGnlRsltByGnlOrd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByGnl32", param: param);

                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByGnl32_YD/{ymdGb}/{ordYmd}/{ordExecYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> SelectOrdRsltForGnlRsltByGnl32_YD(string ymdGb, string ordYmd, string ordExecYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@OrdExecYmd", value: ordExecYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRsltByGnlOrd> info = SqlHelper.GetList<OrdRsltForGnlRsltByGnlOrd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByGnl32_YD", param: param);

                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByOrd11/{ymdGb}/{ordYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> SelectOrdRsltForGnlRsltByOrd11(string ymdGb, string ordYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRsltByGnlOrd> info = SqlHelper.GetList<OrdRsltForGnlRsltByGnlOrd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByOrd11", param: param);

                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByOrd12/{ymdGb}/{ordYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> SelectOrdRsltForGnlRsltByOrd12(string ymdGb, string ordYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRsltByGnlOrd> info = SqlHelper.GetList<OrdRsltForGnlRsltByGnlOrd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByOrd12", param: param);

                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByOrd21/{ymdGb}/{ordYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> SelectOrdRsltForGnlRsltByOrd21(string ymdGb, string ordYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRsltByGnlOrd> info = SqlHelper.GetList<OrdRsltForGnlRsltByGnlOrd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByOrd21", param: param);

                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByOrd21_YD/{ymdGb}/{ordYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> SelectOrdRsltForGnlRsltByOrd21_YD(string ymdGb, string ordYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRsltByGnlOrd> info = SqlHelper.GetList<OrdRsltForGnlRsltByGnlOrd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByOrd21_YD", param: param);

                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByOrd22/{ymdGb}/{ordYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> SelectOrdRsltForGnlRsltByOrd22(string ymdGb, string ordYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRsltByGnlOrd> info = SqlHelper.GetList<OrdRsltForGnlRsltByGnlOrd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByOrd22", param: param);

                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByOrd22_YD/{ymdGb}/{ordYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> SelectOrdRsltForGnlRsltByOrd22_YD(string ymdGb, string ordYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRsltByGnlOrd> info = SqlHelper.GetList<OrdRsltForGnlRsltByGnlOrd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByOrd22_YD", param: param);

                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByOrd31/{ymdGb}/{ordYmd}/{ordExecYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> SelectOrdRsltForGnlRsltByOrd31(string ymdGb, string ordYmd, string ordExecYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@OrdExecYmd", value: ordExecYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRsltByGnlOrd> info = SqlHelper.GetList<OrdRsltForGnlRsltByGnlOrd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByOrd31", param: param);

                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByOrd31_YD/{ymdGb}/{ordYmd}/{ordExecYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> SelectOrdRsltForGnlRsltByOrd31_YD(string ymdGb, string ordYmd, string ordExecYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@OrdExecYmd", value: ordExecYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRsltByGnlOrd> info = SqlHelper.GetList<OrdRsltForGnlRsltByGnlOrd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByOrd31_YD", param: param);

                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectOrdRsltForGnlRsltByOrd32/{ymdGb}/{ordYmd}/{ordExecYmd}/{unitNo}/{ordSlipCd}")]
        public HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> SelectOrdRsltForGnlRsltByOrd32(string ymdGb, string ordYmd, string ordExecYmd, string unitNo, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@OrdExecYmd", value: ordExecYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.String, size: 5);

                IEnumerable<OrdRsltForGnlRsltByGnlOrd> info = SqlHelper.GetList<OrdRsltForGnlRsltByGnlOrd>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_GnlRsltDA_getSelectOrdRsltForGnlRsltByOrd32", param: param);

                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<OrdRsltForGnlRsltByGnlOrd>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectExRsltByRsltKey/{rsltKey}")]
        public HttpResponseResult<IEnumerable<ExRsltByRsltKey>> SelectExRsltByRsltKey(string rsltKey)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@RsltKey", value: Convert.ToInt64(rsltKey), dbType: DbType.Int64);

                IEnumerable<ExRsltByRsltKey> info = SqlHelper.GetList<ExRsltByRsltKey>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_RsltKeyDA_getSelectExRsltByRsltKey", param: param);

                return new HttpResponseResult<IEnumerable<ExRsltByRsltKey>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<ExRsltByRsltKey>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectTreeExamLstByUnitNoByEx/{unitNo}/{retrGb}/{inOutGb}/{retrFrYmd}/{retrToYmd}/{slipGrGb}/{ordCd}/{userId}")]
        public HttpResponseResult<IEnumerable<ExamListByEx>> SelectTreeExamLstByUnitNoByEx(string unitNo, string retrGb, string inOutGb, string retrFrYmd, string retrToYmd, string slipGrGb, string ordCd, string userId)
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
                param.Add(name: "@UserId", value: userId, dbType: DbType.StringFixedLength, size: 8);


                IEnumerable<ExamListByEx> info = SqlHelper.GetList<ExamListByEx>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_ExamRsltIntgViewDA_getSelectTreeExamLstByUnitNoByEx", param: param);

                return new HttpResponseResult<IEnumerable<ExamListByEx>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<ExamListByEx>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/SelectTreeExamLstByUnitNoByEx2/{unitNo}/{retrGb}/{inOutGb}/{retrFrYmd}/{retrToYmd}/{slipGrGb}/{ordCd}/{userId}")]
        public HttpResponseResult<IEnumerable<TreeExamLstByUnitNoByEx2>> SelectTreeExamLstByUnitNoByEx2(string unitNo, string retrGb, string inOutGb, string retrFrYmd, string retrToYmd, string slipGrGb, string ordCd, string userId)
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
                param.Add(name: "@UserId", value: userId, dbType: DbType.StringFixedLength, size: 8);


                IEnumerable<TreeExamLstByUnitNoByEx2> info = SqlHelper.GetList<TreeExamLstByUnitNoByEx2>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_ExamRsltIntgViewDA_getSelectTreeExamLstByUnitNoByEx2", param: param);

                return new HttpResponseResult<IEnumerable<TreeExamLstByUnitNoByEx2>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<TreeExamLstByUnitNoByEx2>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/GetExamRst/{hospitalCd}/{patientId}")]
        public HttpResponseResult<IList<IEnumerable<ExamRst>>> GetExamRst(string hospitalCd, string patientId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@hospitalCd", value: hospitalCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@patientId", value: patientId, dbType: DbType.StringFixedLength, size: 10);
               

                var tuples = SqlHelper.GetMultiPleList<ExamRst, ExamRst, ExamRst, ExamRst>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getExamRst", param: param);
                

                IList<IEnumerable<ExamRst>> iList = new List<IEnumerable<ExamRst>>
                {
                    tuples.Item1,
                    tuples.Item2,
                    tuples.Item3,
                    tuples.Item4
                };

                return new HttpResponseResult<IList<IEnumerable<ExamRst>>> { result = iList, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IList<IEnumerable<ExamRst>>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/GetExamInfo/{ymdGb}/{unitNo}/{ordYmd}/{ordExecYmd}/{ordSlipCd}")]
        public HttpResponseResult<IEnumerable<ExamInfo>> GetExamInfo(string ymdGb, string unitNo, string ordYmd, string ordExecYmd, string ordSlipCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@YmdGb", value: ymdGb, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.StringFixedLength, size: 10);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@OrdExecYmd", value: ordExecYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@OrdSlipCd", value: ordSlipCd, dbType: DbType.StringFixedLength, size: 5);
                

                IEnumerable<ExamInfo> info = SqlHelper.GetList<ExamInfo>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getExamInfo", param: param);

                return new HttpResponseResult<IEnumerable<ExamInfo>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<ExamInfo>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("TESExamRslt/GetExamForm/{rsltkey}")]
        public HttpResponseResult<IEnumerable<ExamForm>> GetExamForm(string rsltkey)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@Rsltkey", value: rsltkey, dbType: DbType.Int64);

                IEnumerable<ExamForm> info = SqlHelper.GetList<ExamForm>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getExamForm", param: param);

                return new HttpResponseResult<IEnumerable<ExamForm>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<ExamForm>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }
        
        [Route("TESExamRslt/GetExamRsltByEx/{accNo}")]
        public HttpResponseResult<IEnumerable<TrSchedule>> GetExamRsltByEx(string accNo)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@AccNo", value: accNo, dbType: DbType.String, size: 10); //DbType.String, size: 10

                IEnumerable<TrSchedule> info = SqlHelper.GetList<TrSchedule>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getExamRsltByEx", param: param);

                return new HttpResponseResult<IEnumerable<TrSchedule>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<TrSchedule>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }
    }
}
