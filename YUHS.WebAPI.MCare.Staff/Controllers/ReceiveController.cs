using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using YUHS.WebAPI.Common.DataAccess;
using YUHS.WebAPI.Common.Security;
using YUHS.WebAPI.MCare.Staff.Models.Common;

namespace YUHS.WebAPI.MCare.Staff.Controllers
{
    [RequireHttps]
    public class ReceiveController : ApiController
    {
        [HttpGet]
        [Route("Receive/Gnkakaoreceive1/{hpNo}")]
        public HttpResponseResult<IEnumerable<ExecuteResult>> Gnkakaoreceive1(string hpNo)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HpNo", value: hpNo, dbType: DbType.String, size: 20);

                SqlHelper.ExecuteProcess(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_gnkakaoreceive1", param: param);
                IEnumerable<ExecuteResult> info = new List<ExecuteResult>();
                return new HttpResponseResult<IEnumerable<ExecuteResult>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<ExecuteResult>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [HttpGet]
        [Route("Receive/Gnkakaoreceive2/{hpNo}")]
        public HttpResponseResult<IEnumerable<ExecuteResult>> Gnkakaoreceive2(string hpNo)
        {
            try
            {
                var param = new DynamicParameters(); ;
                param.Add(name: "@HpNo", value: hpNo, dbType: DbType.String, size: 20);

                SqlHelper.ExecuteProcess(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_gnkakaoreceive2", param: param);
                IEnumerable<ExecuteResult> info = new List<ExecuteResult>();
                return new HttpResponseResult<IEnumerable<ExecuteResult>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<ExecuteResult>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }
    }
}
