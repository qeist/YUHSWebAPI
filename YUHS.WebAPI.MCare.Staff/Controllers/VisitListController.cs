﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using YUHS.WebAPI.Common.DataAccess;
using YUHS.WebAPI.Common.Security;
using YUHS.WebAPI.MCare.Staff.Models.Common;
using YUHS.WebAPI.MCare.Staff.Models.VisitList;

namespace YUHS.WebAPI.MCare.Staff.Controllers
{
    [RequireHttps]
    public class VisitListController : ApiController
    {
        [Route("VisitList/GetOutVisitList/{hosCd}/{unitNo}/{symd}/{eymd}")]
        public HttpResponseResult<IEnumerable<OutVisitList>> GetOutVisitList(string hosCd, string unitNo, string symd, string eymd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@Symd", value: symd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@Eymd", value: eymd, dbType: DbType.StringFixedLength, size: 8);

                IEnumerable<OutVisitList> info = SqlHelper.GetList<OutVisitList>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getOutVisitList", param: param);

                return new HttpResponseResult<IEnumerable<OutVisitList>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<OutVisitList>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }

        }

        [Route("VisitList/GetInVisitList/{hosCd}/{unitNo}/{symd}/{eymd}")]
        public HttpResponseResult<IEnumerable<InVisitList>> GetInVisitList(string hosCd, string unitNo, string symd, string eymd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@Symd", value: symd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@Eymd", value: eymd, dbType: DbType.StringFixedLength, size: 8);

                IEnumerable<InVisitList> info = SqlHelper.GetList<InVisitList>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getInVisitList", param: param);

                return new HttpResponseResult<IEnumerable<InVisitList>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<InVisitList>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("VisitList/GetOpVisitList/{hosCd}/{unitNo}/{symd}/{eymd}")]
        public HttpResponseResult<IEnumerable<OpVisitList>> GetOpVisitList(string hosCd, string unitNo, string symd, string eymd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@Symd", value: symd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@Eymd", value: eymd, dbType: DbType.StringFixedLength, size: 8);

                IEnumerable<OpVisitList> info = SqlHelper.GetList<OpVisitList>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getOpVisitList", param: param);

                return new HttpResponseResult<IEnumerable<OpVisitList>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<OpVisitList>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("VisitList/GetTreatInfo/{hosCd}/{deptCd}/{unitNo}/{treatTyp}/{treatDt}")]
        public HttpResponseResult<IEnumerable<TreatInfo>> GetTreatInfo(string hosCd, string deptCd, string unitNo, string treatTyp, string treatDt)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@DeptCd", value: deptCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@TreatTyp", value: treatTyp, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@TreatDt", value: treatDt, dbType: DbType.StringFixedLength, size: 8);

                IEnumerable<TreatInfo> info = SqlHelper.GetList<TreatInfo>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getTreatInfo", param: param);

                return new HttpResponseResult<IEnumerable<TreatInfo>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<TreatInfo>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("VisitList/GetPastTreatDate/{hosCd}/{unitNo}/{treatDt}/{deptCd}")]
        public HttpResponseResult<IEnumerable<PastTreatDate>> GetPastTreatDate(string hosCd, string unitNo, string treatDt, string deptCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@TreatDt", value: treatDt, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@DeptCd", value: deptCd, dbType: DbType.StringFixedLength, size: 2);

                IEnumerable<PastTreatDate> info = SqlHelper.GetList<PastTreatDate>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getPastTreatDate", param: param);

                return new HttpResponseResult<IEnumerable<PastTreatDate>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<PastTreatDate>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }
    }
}
