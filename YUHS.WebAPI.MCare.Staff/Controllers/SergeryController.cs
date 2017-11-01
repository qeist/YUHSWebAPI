using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using YUHS.WebAPI.Common.DataAccess;
using YUHS.WebAPI.Common.Security;
using YUHS.WebAPI.MCare.Patient.Models.Common;
using YUHS.WebAPI.MCare.Staff.Models.Sergery;

namespace YUHS.WebAPI.MCare.Staff.Controllers
{
    [RequireHttps]
    public class SergeryController : ApiController
    {
        [Route("Sergery/GetFindOpPatLst/{opFrDt}/{opToDt}/{opCntr}/{clnDept}/{opRm}/{staff}/{pln}/{prog}/{end}")]
        public HttpResponseResult<OpPat> GetFindOpPatLst(string opFrDt, string opToDt, string opCntr, string clnDept, string opRm, string staff, string pln, string prog, string end)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@OpFrDt", value: opFrDt, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@OpToDt", value: opToDt, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@OpCntr", value: opCntr, dbType: DbType.String, size: 6);
                param.Add(name: "@ClnDept", value: clnDept, dbType: DbType.String, size: 6);
                param.Add(name: "@OpRm", value: opRm, dbType: DbType.String, size: 6);
                param.Add(name: "@Staff", value: staff, dbType: DbType.String, size: 8);
                param.Add(name: "@Pln", value: pln, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@Prog", value: prog, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@End", value: end, dbType: DbType.StringFixedLength, size: 1);

                IEnumerable<OpPat> info = SqlHelper.GetList<OpPat>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_SelectPatgetFindOpPatLst", param: param);

                return new HttpResponseResult<OpPat> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<OpPat> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("Sergery/GetFindAnsPatLst/{opFrDt}/{opToDt}/{opCntr}/{clnDept}/{rosset}/{staff}/{pln}/{prog}/{end}")]
        public HttpResponseResult<AnsPat> GetFindAnsPatLst(string opFrDt, string opToDt, string opCntr, string clnDept, string rosset, string staff, string pln, string prog, string end)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@OpFrDt", value: opFrDt, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@OpToDt", value: opToDt, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@OpCntr", value: opCntr, dbType: DbType.String, size: 6);
                param.Add(name: "@ClnDept", value: clnDept, dbType: DbType.String, size: 6);
                param.Add(name: "@Rosset", value: rosset, dbType: DbType.String, size: 4);
                param.Add(name: "@Staff", value: staff, dbType: DbType.String, size: 8);
                param.Add(name: "@Pln", value: pln, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@Prog", value: prog, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@End", value: end, dbType: DbType.StringFixedLength, size: 1);

                IEnumerable<AnsPat> info = SqlHelper.GetList<AnsPat>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_SelectgetFindAnsPatLst", param: param);

                return new HttpResponseResult<AnsPat> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<AnsPat> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("Sergery/GetFindOpRmNo/{upDeptCd}/{rosset}")]
        public HttpResponseResult<OpRm> GetFindOpRmNo(string upDeptCd, string rosset)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@UpDeptCd", value: upDeptCd, dbType: DbType.String, size: 6);
                param.Add(name: "@Rosset", value: rosset, dbType: DbType.String, size: 6);

                IEnumerable<OpRm> info = SqlHelper.GetList<OpRm>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_SelectOpRmNoByRosset", param: param);

                return new HttpResponseResult<OpRm> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<OpRm> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }
    }
}
