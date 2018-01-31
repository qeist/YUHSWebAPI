using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using YUHS.WebAPI.Common.DataAccess;
using YUHS.WebAPI.Common.Security;
using YUHS.WebAPI.MCare.Staff.Models.Common;
using YUHS.WebAPI.MCare.Staff.Models.Sergery;

namespace YUHS.WebAPI.MCare.Staff.Controllers
{
    [RequireHttps]
    public class SergeryController : ApiController
    {
        [Route("Sergery/GetFindOpPatLst/{hospitalCd}/{viewFmDt}/{viewToDt}/{operatingCntr}/{departmentCd}/{operatingRoom}/{operatingDrNm}/{operatingTyp1}/{operatingTyp2}/{operatingTyp3}")]
        public HttpResponseResult<IEnumerable<OpPat>> GetFindOpPatLst(string hospitalCd, string viewFmDt, string viewToDt, string operatingCntr, string departmentCd, string operatingRoom, string operatingDrNm, string operatingTyp1, string operatingTyp2, string operatingTyp3)
        {

            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@hospitalCd", value: hospitalCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@viewFmDt", value: viewFmDt, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@viewToDt", value: viewToDt, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@operatingCntr", value: operatingCntr, dbType: DbType.String, size: 6);
                param.Add(name: "@departmentCd", value: departmentCd, dbType: DbType.String, size: 6);
                param.Add(name: "@operatingRoom", value: operatingRoom, dbType: DbType.String, size: 6);
                param.Add(name: "@operatingDrNm", value: operatingDrNm, dbType: DbType.String, size: 6);
                param.Add(name: "@operatingTyp1", value: operatingTyp1, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@operatingTyp2", value: operatingTyp2, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@operatingTyp3", value: operatingTyp3, dbType: DbType.StringFixedLength, size: 1);

                IEnumerable<OpPat> info = SqlHelper.GetList<OpPat>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_SelectPatgetFindOpPatLst", param: param);

                return new HttpResponseResult<IEnumerable<OpPat>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<OpPat>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("Sergery/GetFindAnsPatLst/{hospitalCd}/{viewFmDt}/{viewToDt}/{operatingCntr}/{departmentCd}/{operatingRoom}/{operatingDrNm}/{operatingTyp1}/{operatingTyp2}/{operatingTyp3}")]
        public HttpResponseResult<IEnumerable<AnsPat>> GetFindAnsPatLst(string hospitalCd, string viewFmDt, string viewToDt, string operatingCntr, string departmentCd, string operatingRoom, string operatingDrNm, string operatingTyp1, string operatingTyp2, string operatingTyp3)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@hospitalCd", value: hospitalCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@viewFmDt", value: viewFmDt, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@viewToDt", value: viewToDt, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@operatingCntr", value: operatingCntr, dbType: DbType.String, size: 6);
                param.Add(name: "@departmentCd", value: departmentCd, dbType: DbType.String, size: 6);
                param.Add(name: "@operatingRoom", value: operatingRoom, dbType: DbType.String, size: 6);
                param.Add(name: "@operatingDrNm", value: operatingDrNm, dbType: DbType.String, size: 6);
                param.Add(name: "@operatingTyp1", value: operatingTyp1, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@operatingTyp2", value: operatingTyp2, dbType: DbType.StringFixedLength, size: 1);
                param.Add(name: "@operatingTyp3", value: operatingTyp3, dbType: DbType.StringFixedLength, size: 1);

                IEnumerable<AnsPat> info = SqlHelper.GetList<AnsPat>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_SelectgetFindAnsPatLst", param: param);

                return new HttpResponseResult<IEnumerable<AnsPat>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<AnsPat>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("Sergery/GetFindOpRmNo/{upDeptCd}/{rosset}")]
        public HttpResponseResult<IEnumerable<OpRm>> GetFindOpRmNo(string upDeptCd, string rosset)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@UpDeptCd", value: upDeptCd, dbType: DbType.String, size: 6);
                param.Add(name: "@Rosset", value: rosset, dbType: DbType.String, size: 6);

                IEnumerable<OpRm> info = SqlHelper.GetList<OpRm>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_SelectOpRmNoByRosset", param: param);

                return new HttpResponseResult<IEnumerable<OpRm>> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IEnumerable<OpRm>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }
    }
}
