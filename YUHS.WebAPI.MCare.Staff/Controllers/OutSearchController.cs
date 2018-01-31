using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using YUHS.WebAPI.Common.DataAccess;
using YUHS.WebAPI.Common.Security;
using YUHS.WebAPI.MCare.Staff.Models.Common;
using YUHS.WebAPI.MCare.Staff.Models.OutSearch;

namespace YUHS.WebAPI.MCare.Staff.Controllers
{
    [RequireHttps]
    public class OutSearchController : ApiController
    {
        [Route("OutSearch/GetOutPatListPerDoctor/{hosCd}/{clnYmd}/{drId}/{deptCd}")]
        public HttpResponseResult<IEnumerable<OutPatList>> GetOutPatListPerDoctor(string hosCd, string clnYmd, string drId, string deptCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@ClnYmd", value: clnYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@DrId", value: drId, dbType: DbType.StringFixedLength, size: 7);
                param.Add(name: "@DeptCd", value: deptCd, dbType: DbType.StringFixedLength, size: 2);

                IEnumerable<OutPatList> info = SqlHelper.GetList<OutPatList>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getOutPatListPerDoctor", param: param);

                return new HttpResponseResult<IEnumerable<OutPatList>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<OutPatList>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("OutSearch/GetOutPatListPerDept/{hosCd}/{clnYmd}/{deptCd}")]
        public HttpResponseResult<IEnumerable<OutPatList>> GetOutPatListPerDept(string hosCd, string clnYmd, string deptCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@ClnYmd", value: clnYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@DeptCd", value: deptCd, dbType: DbType.StringFixedLength, size: 2);

                IEnumerable<OutPatList> info = SqlHelper.GetList<OutPatList>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getOutPatListPerDept", param: param);

                return new HttpResponseResult<IEnumerable<OutPatList>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<OutPatList>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("OutSearch/GetErPatList/{hosCd}/{deptCd}")]
        public HttpResponseResult<IEnumerable<OutPatList>> GetErPatList(string hosCd, string deptCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@DeptCd", value: deptCd, dbType: DbType.StringFixedLength, size: 2);

                IEnumerable<OutPatList> info = SqlHelper.GetList<OutPatList>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getErPatList", param: param);

                return new HttpResponseResult<IEnumerable<OutPatList>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<OutPatList>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("OutSearch/GetOutPatInfo/{hosCd}/{unitNo}/{clnYmd}/{deptCd}/{treatTyp}")]
        public HttpResponseResult<IEnumerable<OutPatInfo>> GetOutPatInfo(string hosCd, string unitNo, string clnYmd, string deptCd, string treatTyp)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.StringFixedLength, size: 10);
                param.Add(name: "@ClnYmd", value: clnYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@DeptCd", value: deptCd, dbType: DbType.StringFixedLength, size: 6);
                param.Add(name: "@TreatTyp", value: treatTyp, dbType: DbType.StringFixedLength, size: 1);

                IEnumerable<OutPatInfo> info = SqlHelper.GetList<OutPatInfo>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getOutPatInfo", param: param);

                return new HttpResponseResult<IEnumerable<OutPatInfo>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<OutPatInfo>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }
    }
}
