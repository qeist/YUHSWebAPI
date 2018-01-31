using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using YUHS.WebAPI.Common.DataAccess;
using YUHS.WebAPI.Common.Security;
using YUHS.WebAPI.MCare.Staff.Models.Common;
using YUHS.WebAPI.MCare.Staff.Models.Search;

namespace YUHS.WebAPI.MCare.Staff.Controllers
{
    [RequireHttps]
    public class SearchController : ApiController
    {
        [Route("Search/GetSrPatListByName/{hosCd}/{patNm}")]
        public HttpResponseResult<IEnumerable<SrPatList>> GetSrPatListByName(string hosCd, string patNm)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@PatNm", value: patNm, dbType: DbType.String, size: 50);

                IEnumerable<SrPatList> info = SqlHelper.GetList<SrPatList>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getSrPatListByName", param: param);

                return new HttpResponseResult<IEnumerable<SrPatList>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<SrPatList>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }

        }

        [Route("Search/GetSrPatListById/{hosCd}/{unitNo}")]
        public HttpResponseResult<IEnumerable<SrPatList>> GetSrPatListById(string hosCd, string unitNo)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);

                IEnumerable<SrPatList> info = SqlHelper.GetList<SrPatList>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getSrPatListById", param: param);

                return new HttpResponseResult<IEnumerable<SrPatList>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<SrPatList>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }

        }

        [Route("Search/GetSrPatListByDW/{hosCd}/{deptCd}/{ward}")]
        public HttpResponseResult<IEnumerable<SrPatList>> GetSrPatListByDW(string hosCd, string deptCd, string ward)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@DeptCd", value: deptCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@Ward", value: ward, dbType: DbType.String, size: 5);

                IEnumerable<SrPatList> info = SqlHelper.GetList<SrPatList>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getSrPatListByDW", param: param);

                return new HttpResponseResult<IEnumerable<SrPatList>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<SrPatList>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }

        }
    }
}
