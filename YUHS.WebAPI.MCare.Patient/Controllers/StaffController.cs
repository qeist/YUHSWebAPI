using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using YUHS.WebAPI.Common.DataAccess;
using YUHS.WebAPI.Common.Security;
using YUHS.WebAPI.MCare.Patient.Models.Common;
using YUHS.WebAPI.MCare.Patient.Models.Staff;

namespace YUHS.WebAPI.MCare.Patient.Controllers
{
    [RequireHttps]
    public class StaffController : ApiController
    {
        [Route("Staff/GetDepartmentInfo/{hosCd}")]
        public HttpResponseResult<DepartmentInfo> GetDepartmentInfo(string hosCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);

                IEnumerable<DepartmentInfo> info = SqlHelper.GetList<DepartmentInfo>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getDepartmentInfo", param: param);

                return new HttpResponseResult<DepartmentInfo> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<DepartmentInfo> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("Staff/GetClinicInfo/{hosCd}")]
        public HttpResponseResult<ClinicInfo> GetClinicInfo(string hosCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);

                IEnumerable<ClinicInfo> info = SqlHelper.GetList<ClinicInfo>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getClinicInfo", param: param);

                return new HttpResponseResult<ClinicInfo> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<ClinicInfo> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("Staff/GetCenterInfo/{hosCd}")]
        public HttpResponseResult<ClinicInfo> GetCenterInfo(string hosCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);

                IEnumerable<ClinicInfo> info = SqlHelper.GetList<ClinicInfo>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getCenterInfo", param: param);

                return new HttpResponseResult<ClinicInfo> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<ClinicInfo> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("Staff/GetDoctorByDepartment/{hosCd}/{deptCd}")]
        public HttpResponseResult<StaffInfo> GetDoctorByDepartment(string hosCd, string deptCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@DeptCd", value: deptCd, dbType: DbType.String, size: 6);

                IEnumerable<StaffInfo> info = SqlHelper.GetList<StaffInfo>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getDoctorByDepartment", param: param);

                return new HttpResponseResult<StaffInfo> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<StaffInfo> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("Staff/GetDoctorById/{hosCd}/{deptCd}/{drId}")]
        public HttpResponseResult<StaffInfo> GetDoctorById(string hosCd, string deptCd, string drId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@DeptCd", value: deptCd, dbType: DbType.String, size: 6);
                param.Add(name: "@DrId", value: drId, dbType: DbType.String, size: 8);

                IEnumerable<StaffInfo> info = SqlHelper.GetList<StaffInfo>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getDoctorById", param: param);

                return new HttpResponseResult<StaffInfo> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<StaffInfo> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("Staff/GetDoctorByName/{hosCd}/{drNm}")]
        public HttpResponseResult<StaffInfo> GetDoctorByName(string hosCd, string drNm)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@DrNm", value: drNm, dbType: DbType.String, size: 50);
                
                IEnumerable<StaffInfo> info = SqlHelper.GetList<StaffInfo>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getDoctorByName", param: param);

                return new HttpResponseResult<StaffInfo> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<StaffInfo> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("Staff/GetDoctorBySpeciality/{hosCd}/{memo}")]
        public HttpResponseResult<StaffInfo> GetDoctorBySpeciality(string hosCd, string memo)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@Memo", value: memo, dbType: DbType.String, size: 100);

                IEnumerable<StaffInfo> info = SqlHelper.GetList<StaffInfo>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getDoctorBySpeciality", param: param);

                return new HttpResponseResult<StaffInfo> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<StaffInfo> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("Staff/GetDoctorByAll/{hosCd}/{memo}")]
        public HttpResponseResult<StaffInfo> GetDoctorByAll(string hosCd, string memo)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@Memo", value: memo, dbType: DbType.String, size: 100);

                IEnumerable<StaffInfo> info = SqlHelper.GetList<StaffInfo>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getDoctorByAll", param: param);

                return new HttpResponseResult<StaffInfo> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<StaffInfo> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }
    }
}

    
