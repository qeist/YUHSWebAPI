using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using YUHS.WebAPI.Common.DataAccess;
using YUHS.WebAPI.Common.Security;
using YUHS.WebAPI.MCare.Patient.Models.Common;
using YUHS.WebAPI.MCare.Staff.Models.InSearch;

namespace YUHS.WebAPI.MCare.Staff.Controllers
{
    [RequireHttps]
    public class InSearchController : ApiController
    {
        [Route("InSearch/GetInPatListPerDoctor/{hosCd}/{drId}")]
        public HttpResponseResult<InPatList> GetInPatListPerDoctor(string hosCd, string drId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@DrId", value: drId, dbType: DbType.StringFixedLength, size: 7);

                IEnumerable<InPatList> info = SqlHelper.GetList<InPatList>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getInPatListPerDoctor", param: param);

                return new HttpResponseResult<InPatList> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<InPatList> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }

        }

        [Route("InSearch/GetInPatListPerDept/{hosCd}/{deptCd}")]
        public HttpResponseResult<InPatList> GetInPatListPerDept(string hosCd, string deptCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@DeptCd", value: deptCd, dbType: DbType.StringFixedLength, size: 2);

                IEnumerable<InPatList> info = SqlHelper.GetList<InPatList>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getInPatListPerDept", param: param);

                return new HttpResponseResult<InPatList> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<InPatList> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }

        }

        [Route("InSearch/GetInPatListPerWard/{hosCd}/{ward}")]
        public HttpResponseResult<InPatList> GetInPatListPerWard(string hosCd, string ward)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@Ward", value: ward, dbType: DbType.String, size: 5);

                IEnumerable<InPatList> info = SqlHelper.GetList<InPatList>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getInPatListPerWard", param: param);

                return new HttpResponseResult<InPatList> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<InPatList> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }

        }

        [Route("InSearch/GetInPatListPerDD/{hosCd}/{deptCd}/{drId}")]
        public HttpResponseResult<InPatList> GetInPatListPerDD(string hosCd, string deptCd, string drId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@DeptCd", value: deptCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@DrId", value: drId, dbType: DbType.StringFixedLength, size: 7);

                IEnumerable<InPatList> info = SqlHelper.GetList<InPatList>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getInPatListPerDD", param: param);

                return new HttpResponseResult<InPatList> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<InPatList> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }

        }

        [Route("InSearch/GetInPatListPerWD/{hosCd}/{ward}/{drId}")]
        public HttpResponseResult<InPatList> GetInPatListPerWD(string hosCd, string ward, string drId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@Ward", value: ward, dbType: DbType.String, size: 5);
                param.Add(name: "@DrId", value: drId, dbType: DbType.StringFixedLength, size: 7);

                IEnumerable<InPatList> info = SqlHelper.GetList<InPatList>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getInPatListPerWD", param: param);

                return new HttpResponseResult<InPatList> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<InPatList> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }

        }

        [Route("InSearch/GetInPatInfo/{hosCd}/{unitNo}/{treatTyp}")]
        public HttpResponseResult<InPatInfo> GetInPatInfo(string hosCd, string unitNo, string treatTyp)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.StringFixedLength, size: 10);
                param.Add(name: "@TreatTyp", value: treatTyp, dbType: DbType.StringFixedLength, size: 1);

                IEnumerable<InPatInfo> info = SqlHelper.GetList<InPatInfo>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getInPatInfo", param: param);

                return new HttpResponseResult<InPatInfo> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<InPatInfo> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }
    }
}
