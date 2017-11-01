using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using YUHS.WebAPI.Common.DataAccess;
using YUHS.WebAPI.Common.Security;
using YUHS.WebAPI.MCare.Patient.Models.Common;
using YUHS.WebAPI.MCare.Staff.Models.Master;

namespace YUHS.WebAPI.MCare.Staff.Controllers
{
    [RequireHttps]
    public class MasterController : ApiController
    {
        [Route("Master/GetDeptList/{hosCd}/{gubn}")]
        public HttpResponseResult<DeptList> GetDeptList(string hosCd, string gubn)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@GuBn", value: gubn, dbType: DbType.StringFixedLength, size: 1);

                IEnumerable<DeptList> info = SqlHelper.GetList<DeptList>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getDeptList", param: param);

                return new HttpResponseResult<DeptList> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<DeptList> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("Master/GetDoctorList/{hosCd}/{deptCd}")]
        public HttpResponseResult<DoctorList> GetDoctorList(string hosCd, string deptCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@DeptCd", value: deptCd, dbType: DbType.StringFixedLength, size: 2);

                IEnumerable<DoctorList> info = SqlHelper.GetList<DoctorList>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getDoctorList", param: param);

                return new HttpResponseResult<DoctorList> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<DoctorList> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }
    }
}
