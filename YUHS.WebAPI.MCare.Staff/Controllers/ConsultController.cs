using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using YUHS.WebAPI.Common.DataAccess;
using YUHS.WebAPI.Common.Security;
using YUHS.WebAPI.MCare.Patient.Models.Common;
using YUHS.WebAPI.MCare.Staff.Models.Consult;

namespace YUHS.WebAPI.MCare.Staff.Controllers
{
    [RequireHttps]
    public class ConsultController : ApiController
    {
        [Route("Consult/GetConsultReqListPerDoctor/{hosCd}/{sYmd}/{eYmd}/{deptCd}/{drId}/{statusCd}")]
        public HttpResponseResult<ConsultReqList> GetConsultReqListPerDoctor(string hosCd, string sYmd, string eYmd, string deptCd, string drId, string statusCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@SYmd", value: sYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@EYmd", value: eYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@DeptCd", value: deptCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@DrId", value: drId, dbType: DbType.StringFixedLength, size: 7);
                param.Add(name: "@StatusCd", value: statusCd, dbType: DbType.StringFixedLength, size: 1);

                IEnumerable<ConsultReqList> info = SqlHelper.GetList<ConsultReqList>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getConsultReqListPerDoctor", param: param);

                return new HttpResponseResult<ConsultReqList> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<ConsultReqList> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("Consult/GetConsultResListPerDoctor/{hosCd}/{sYmd}/{eYmd}/{drId}/{deptCd}/{viewTyp}/{treatTyp}/{statusTyp}")]
        public HttpResponseResult<ConsultReqList> GetConsultResListPerDoctor(string hosCd, string sYmd, string eYmd, string drId, string deptCd, string viewTyp, string treatTyp, string statusTyp)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@SYmd", value: sYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@EYmd", value: eYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@DrId", value: drId, dbType: DbType.StringFixedLength, size: 7);
                param.Add(name: "@DeptCd", value: deptCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@ViewTyp", value: viewTyp, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@TreatTyp", value: treatTyp, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@StatusTyp", value: statusTyp, dbType: DbType.StringFixedLength, size: 1);

                IEnumerable<ConsultReqList> info = SqlHelper.GetList<ConsultReqList>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getConsultResListPerDoctor", param: param);

                return new HttpResponseResult<ConsultReqList> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<ConsultReqList> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("Consult/GetConsultResListPerDept/{hosCd}/{sYmd}/{eYmd}/{deptCd}/{treatTyp}/{statusTyp}")]
        public HttpResponseResult<ConsultReqList> GetConsultResListPerDept(string hosCd, string sYmd, string eYmd, string deptCd, string treatTyp, string statusTyp)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@SYmd", value: sYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@EYmd", value: eYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@DeptCd", value: deptCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@TreatTyp", value: treatTyp, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@StatusTyp", value: statusTyp, dbType: DbType.StringFixedLength, size: 1);

                IEnumerable<ConsultReqList> info = SqlHelper.GetList<ConsultReqList>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_getConsultResListPerDept", param: param);

                return new HttpResponseResult<ConsultReqList> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<ConsultReqList> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }
    }
}
