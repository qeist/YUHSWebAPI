using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using YUHS.WebAPI.Common.DataAccess;
using YUHS.WebAPI.Common.Security;
using YUHS.WebAPI.MCare.Staff.Models.Common;
using YUHS.WebAPI.MCare.Staff.Models.InSearch;

namespace YUHS.WebAPI.MCare.Staff.Controllers
{
    [RequireHttps]
    public class InSearchController : ApiController
    {
        [Route("InSearch/GetInPatListPerDoctor/{hosCd}/{drId}")]
        public HttpResponseResult<IEnumerable<InPatList>> GetInPatListPerDoctor(string hosCd, string drId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@DrId", value: drId, dbType: DbType.StringFixedLength, size: 7);

                IEnumerable<InPatList> info = SqlHelper.GetList<InPatList>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getInPatListPerDoctor", param: param);

                return new HttpResponseResult<IEnumerable<InPatList>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<InPatList>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }

        }

        [Route("InSearch/GetInPatListPerDept/{hosCd}/{deptCd}")]
        public HttpResponseResult<IEnumerable<InPatList>> GetInPatListPerDept(string hosCd, string deptCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@DeptCd", value: deptCd, dbType: DbType.StringFixedLength, size: 2);

                IEnumerable<InPatList> info = SqlHelper.GetList<InPatList>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getInPatListPerDept", param: param);

                return new HttpResponseResult<IEnumerable<InPatList>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<InPatList>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }

        }

        [Route("InSearch/GetInPatListPerWard/{hosCd}/{ward}")]
        public HttpResponseResult<IEnumerable<InPatList>> GetInPatListPerWard(string hosCd, string ward)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@Ward", value: ward, dbType: DbType.String, size: 5);

                IEnumerable<InPatList> info = SqlHelper.GetList<InPatList>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getInPatListPerWard", param: param);

                return new HttpResponseResult<IEnumerable<InPatList>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<InPatList>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }

        }

        [Route("InSearch/GetInPatListPerDD/{hosCd}/{deptCd}/{drId}")]
        public HttpResponseResult<IEnumerable<InPatList>> GetInPatListPerDD(string hosCd, string deptCd, string drId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@DeptCd", value: deptCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@DrId", value: drId, dbType: DbType.StringFixedLength, size: 7);

                IEnumerable<InPatList> info = SqlHelper.GetList<InPatList>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getInPatListPerDD", param: param);

                return new HttpResponseResult<IEnumerable<InPatList>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<InPatList>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }

        }

        [Route("InSearch/GetInPatListPerWD/{hosCd}/{ward}/{drId}")]
        public HttpResponseResult<IEnumerable<InPatList>> GetInPatListPerWD(string hosCd, string ward, string drId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@Ward", value: ward, dbType: DbType.String, size: 5);
                param.Add(name: "@DrId", value: drId, dbType: DbType.StringFixedLength, size: 7);

                IEnumerable<InPatList> info = SqlHelper.GetList<InPatList>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getInPatListPerWD", param: param);

                return new HttpResponseResult<IEnumerable<InPatList>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<InPatList>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }

        }

        [Route("InSearch/GetInPatInfo/{hosCd}/{unitNo}/{treatTyp}")]
        public HttpResponseResult<IEnumerable<InPatInfo>> GetInPatInfo(string hosCd, string unitNo, string treatTyp)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.StringFixedLength, size: 10);
                param.Add(name: "@TreatTyp", value: treatTyp, dbType: DbType.StringFixedLength, size: 1);

                IEnumerable<InPatInfo> info = SqlHelper.GetList<InPatInfo>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getInPatInfo", param: param);

                return new HttpResponseResult<IEnumerable<InPatInfo>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<InPatInfo>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("InSearch/GetPatientInfo/{hosCd}/{unitNo}")]
        public HttpResponseResult<IEnumerable<PatientInfo>> GetPatientInfo(string hosCd, string unitNo)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.StringFixedLength, size: 10);

                IEnumerable<PatientInfo> info = SqlHelper.GetList<PatientInfo>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getPatientInfo", param: param);

                return new HttpResponseResult<IEnumerable<PatientInfo>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<PatientInfo>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }

        }

       
    }
}
