using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using YUHS.WebAPI.Common.DataAccess;
using YUHS.WebAPI.Common.Security;
using YUHS.WebAPI.MCare.Patient.Models.Common;
using YUHS.WebAPI.MCare.Patient.Models.Park;

namespace YUHS.WebAPI.MCare.Patient.Controllers
{
    [RequireHttps]
    public class ParkController : ApiController
    {
        [Route("Park/GetVehicleNo/{unitNo}/{rgtId}")]
        public HttpResponseResult<PlateNoPat> GetVehicleNo(string unitNo, string rgtId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@RgtId", value: rgtId, dbType: DbType.StringFixedLength, size: 8);

                IEnumerable<PlateNoPat> info = SqlHelper.GetList<PlateNoPat>(targetDB: SqlHelper.GetConnectionString("HConnectionString"), storedProcedure: "USP_HP_ZZZ_GE_ParkingManagerDA_SelectPlateNoPat", param: param);

                return new HttpResponseResult<PlateNoPat> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<PlateNoPat> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }

        }

        [Route("Park/SetVehicleNo/{plateNo}/{unitNo}/{rgtId}")]
        public HttpResponseResult<ExecuteResult> SetVehicleNo(string plateNo, string unitNo, string rgtId)
        {
            try
            {
                var param = new DynamicParameters(); ;
                param.Add(name: "@PlateNo", value: plateNo, dbType: DbType.String, size: 20);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@RgtId", value: rgtId, dbType: DbType.StringFixedLength, size: 8);

                SqlHelper.ExecuteProcess(targetDB: SqlHelper.GetConnectionString("HConnectionString"), storedProcedure: "USP_HP_ZZZ_GE_ParkingManagerDA_SavePlateNoPat", param: param);
                IEnumerable<ExecuteResult> info = new List<ExecuteResult>();
                return new HttpResponseResult<ExecuteResult> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<ExecuteResult> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }
    }
}
