using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using YUHS.WebAPI.Common.DataAccess;
using YUHS.WebAPI.Common.Security;
using YUHS.WebAPI.MCare.Patient.Library;
using YUHS.WebAPI.MCare.Patient.Models.Common;
using YUHS.WebAPI.MCare.Patient.Models.Nut;

namespace YUHS.WebAPI.MCare.Patient.Controllers
{
    [RequireHttps]
    public class NutController : ApiController
    {
        [Route("Nut/SelectAdmiChk/{unitNo}")]
        public HttpResponseResult<AdmiChk> SelectAdmiChkByUnitNo(string unitNo)
        {
            try
            {
                var param = new DynamicParameters();
                
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.StringFixedLength, size: 10);

                IEnumerable<AdmiChk> info = SqlHelper.GetList<AdmiChk>(targetDB: SqlHelper.GetConnectionString("SConnectionString"), storedProcedure: "USP_SP_NUT_BI_MlOrdDA_SelectAdmiChkByUnitNo", param: param);

                return new HttpResponseResult<AdmiChk> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<AdmiChk> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("Nut/SelectMlOrdMList")]
        public HttpResponseResult<MlOrdMList> SelectMlOrdMList()
        {
            try
            {
                IEnumerable<MlOrdMList> info = SqlHelper.GetList<MlOrdMList>(targetDB: SqlHelper.GetConnectionString("SConnectionString"), storedProcedure: "USP_SP_NUT_BI_DietSpmenDA_SelectMlOrdMList");

                return new HttpResponseResult<MlOrdMList> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<MlOrdMList> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }
        
        [Route("Nut/SelectAdMlOrd/{chosNo}/{ordYmd}")]
        public HttpResponseResult<AdMlOrd> SelectAdMlOrd(string chosNo, string ordYmd)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add(name: "@ChosNo", value: chosNo, dbType: DbType.StringFixedLength, size: 14);
                param.Add(name: "@OrdYmd", value: ordYmd, dbType: DbType.StringFixedLength, size: 8);

                IEnumerable<AdMlOrd> info = SqlHelper.GetList<AdMlOrd>(targetDB: SqlHelper.GetConnectionString("SConnectionString"), storedProcedure: "USP_SP_NUT_ME_MlKndOrdDA_SelectAdMlOrdByChosNo");

                return new HttpResponseResult<AdMlOrd> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<AdMlOrd> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("Nut/InitInquiry")]
        public HttpResponseResult<BasInfo> InitInquiry()
        {
            try
            {
                IEnumerable<BasInfo> info = SqlHelper.GetList<BasInfo>(targetDB: SqlHelper.GetConnectionString("SConnectionString"), storedProcedure: "USP_SP_NUT_ME_EatCloseDA_SelectBasInfo");

                return new HttpResponseResult<BasInfo> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<BasInfo> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }

        }

        [Route("Nut/SelectMlOrderListByChosNo/{unitNo}/{ordYmd}")]
        public HttpResponseDataSetResult<DataSet> SelectMlOrderListByChosNo(string unitNo, string ordYmd)
        {
            try
            {
                NutLib nutLib = new NutLib();
                var ds = nutLib.SelectMlOrderListByChosNo_DietReqNtx(unitNo, ordYmd);
                return new HttpResponseDataSetResult<DataSet> { result = ds, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseDataSetResult<DataSet> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }

        }

        [Route("Nut/InsertMlAddSpmen/{userId}/{chosNo}/{ordUserPos}/{deptCd}/{ordDrId}/{locCd}/{mEMlTypGbAAddYn}")]
        public HttpResponseResult<ExecuteResult> InsertMlAddSpmen(string userId, string chosNo, string ordUserPos, string deptCd, string ordDrId, string locCd, string mEMlTypGbAAddYn, System.Net.Http.HttpRequestMessage ds)
        {
            try
            {
                System.Collections.Hashtable pTempHT = new System.Collections.Hashtable();

                pTempHT["UserId"] = userId;
                pTempHT["ChosNo"] = chosNo;
                pTempHT["OrdUserPos"] = ordUserPos;
                pTempHT["DeptCd"] = deptCd;
                pTempHT["OrdDrId"] = ordDrId;
                pTempHT["LocCd"] = locCd;
                pTempHT["MEMlTypGbAAddYn"] = mEMlTypGbAAddYn;

                DataSet pTempDS = JsonConvert.DeserializeObject<DataSet>(ds.Content.ReadAsStringAsync().Result);

                NutLib nutLib = new NutLib();
                nutLib.InsertMlAddSpmen_DietReqNtx(pTempHT, pTempDS);

                IEnumerable<ExecuteResult> info = null;

                return new HttpResponseResult<ExecuteResult> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<ExecuteResult> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }

        }

        [Route("Nut/DeleteMlAddSpmen/{userId}/{chosNo}/{ordUserPos}/{deptCd}/{ordDrId}/{locCd}/{mEMlTypGbAAddYn}")]
        public HttpResponseResult<ExecuteResult> DeleteMlAddSpmen(string userId, string chosNo, string ordUserPos, string deptCd, string ordDrId, string locCd, string mEMlTypGbAAddYn, System.Net.Http.HttpRequestMessage ds)
        {
            try
            {
                System.Collections.Hashtable pTempHT = new System.Collections.Hashtable();

                pTempHT["UserId"] = userId;
                pTempHT["ChosNo"] = chosNo;
                pTempHT["OrdUserPos"] = ordUserPos;
                pTempHT["DeptCd"] = deptCd;
                pTempHT["OrdDrId"] = ordDrId;
                pTempHT["LocCd"] = locCd;
                pTempHT["MEMlTypGbAAddYn"] = mEMlTypGbAAddYn;

                DataSet pTempDS = JsonConvert.DeserializeObject<DataSet>(ds.Content.ReadAsStringAsync().Result);

                NutLib nutLib = new NutLib();
                nutLib.DeleteMlAddSpmen(pTempHT, pTempDS);

                IEnumerable<ExecuteResult> info = null;

                return new HttpResponseResult<ExecuteResult> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<ExecuteResult> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }

        }


    }
}
