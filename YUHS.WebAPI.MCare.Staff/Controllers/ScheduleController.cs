using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using YUHS.WebAPI.Common.DataAccess;
using YUHS.WebAPI.Common.Security;
using YUHS.WebAPI.MCare.Staff.Models.Common;
using YUHS.WebAPI.MCare.Staff.Models.Schedule;

namespace YUHS.WebAPI.MCare.Staff.Controllers
{
    [RequireHttps]
    public class ScheduleController : ApiController
    {
        [Route("Schedule/GetOrderSchedule/{clnDeptCd}")]
        public HttpResponseResult<IEnumerable<OrderSchedule>> GetOrderSchedule(string clnDeptCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@ClnDeptCd", value: clnDeptCd, dbType: DbType.String, size: 6);
                
                IEnumerable<OrderSchedule> info = SqlHelper.GetList<OrderSchedule>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getSettingSchedule", param: param);

                return new HttpResponseResult<IEnumerable<OrderSchedule>> { result = info, error = new ErrorInfo { flag = false } };

            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<OrderSchedule>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }

        }
    }
}
