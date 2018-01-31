using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using YUHS.WebAPI.Common.DataAccess;
using YUHS.WebAPI.Common.Security;
using YUHS.WebAPI.MCare.Staff.Models.Common;
using YUHS.WebAPI.MCare.Staff.Models.User;

namespace YUHS.WebAPI.MCare.Staff.Controllers
{
    [RequireHttps]
    public class UserController : ApiController
    {
        [Route("User/GetLoginInfo/{hosCd}/{userId}/{pwd}")]
        public HttpResponseResult<IEnumerable<LoginInfo>> GetLoginInfo(string hosCd, string userId, string pwd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@UserId", value: userId, dbType: DbType.String, size: 10);
                param.Add(name: "@Pwd", value: pwd, dbType: DbType.String, size: 100);

                IEnumerable<LoginInfo> info = SqlHelper.GetList<LoginInfo>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getLoginInfo", param: param);

                return new HttpResponseResult<IEnumerable<LoginInfo>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<LoginInfo>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }

        }
    }
}
