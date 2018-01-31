using Dapper;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using YUHS.WebAPI.Common.DataAccess;
using YUHS.WebAPI.Common.Security;
using YUHS.WebAPI.MCare.Staff.Models.Common;
using YUHS.WebAPI.MCare.Staff.Models.InRound;

namespace YUHS.WebAPI.MCare.Staff.Controllers
{
    [RequireHttps]
    public class InRoundController : ApiController
    {
        //[Route("InRound/ExecuteRoundTreat/{hosCd}/{unitNo}/{clnYmd}/{clnHms}/{comment}/{sysYmd}/{sysHms}/{deptCd}/{userId}")]
        //public HttpResponseResult<RoundTreat> ExecuteRoundTreat(string hosCd, string unitNo, string clnYmd, string clnHms, string comment, string sysYmd, string sysHms, string deptCd, string userId)
        //{
        //    try
        //    {
        //        var param = new DynamicParameters();
        //        param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
        //        param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.StringFixedLength, size: 10);
        //        param.Add(name: "@ClnYmd", value: clnYmd, dbType: DbType.StringFixedLength, size: 8);
        //        param.Add(name: "@ClnHms", value: clnHms, dbType: DbType.StringFixedLength, size: 6);
        //        param.Add(name: "@Comment", value: comment, dbType: DbType.String, size: 500);
        //        param.Add(name: "@SysYmd", value: sysYmd, dbType: DbType.StringFixedLength, size: 8);
        //        param.Add(name: "@SysHms", value: sysHms, dbType: DbType.StringFixedLength, size: 6);
        //        param.Add(name: "@DeptCd", value: deptCd, dbType: DbType.String, size: 6);
        //        param.Add(name: "@UserId", value: userId, dbType: DbType.String, size: 8);

        //        IEnumerable<RoundTreat> info = SqlHelper.GetList<RoundTreat>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_executeRoundTreat", param: param);

        //        return new HttpResponseResult<RoundTreat> { result = info, error = new ErrorInfo { flag = false } };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new HttpResponseResult<RoundTreat> { error = new ErrorInfo { flag = true, message = ex.Message } };
        //    }

        //}

//        Newtonsoft.Json 은 NuGet 에서 Newton 만 쳐도 나옵니다.혹은 첨부파일 참조해주시고 Newtonsoft.Json.Linq 도 참조해주시고 ( using Newtonsoft.Json.Linq; )

//string json = @" { "car" : { "Name" : "Sonata" } , "test" : "abc" } " //json 문자열
//JObject jobj = JObject.Parse(json); //문자를 객체화

//    이렇게하면 파싱 끝입니다.사용방법은

//    MessageBox.Show(jobj["test"].ToString());

//"test" : "abc" 이므로 출력값은 "abc" 입니다.



//json 에서 "car" : { "Name" : "Sonata" } " 와 같이 car 의 값이 오브젝트라도 그냥

//jobj["car"]["Name"].ToString()

//하면 car 의 Name 인 "Sonata" 가 반환됩니다



//그럼 json 에서 배열일때는 어떻하냐..

//string json = @"[ "dog" , "cat" , "horse" ]"

//JArray jarr = JArray.Parse(json)

//JObject 를 JArray 로 바꿔주기만 하면 파싱됩니다.



//일반 배열이랑 비슷하게 사용하면 되요

//foreach(JObject jobj in jarr)

//{

//MessageBox.Show(jobj.ToString());

//}



//마지막으로 이것들을 응용해서 BukkitAPI 를 파싱해 보겠습니다



//using Newtonsoft.Json.Linq;

//using System.Net;



//참조해주시고



//using (WebClient wc = new WebClient())

//{

//     string json = wc.DownloadString("http://ksi123456ab.dothome.co.kr/API/Plugin.json"); //API 사이트에서 json 받아옴

//JArray jarr = JArray.Parse(json); //json 객체로

//     foreach(JObject jobj in jarr)

//     {

//     MessageBox.Show(jobj["name"].ToString() + " , 버전 : " + jobj["version"].ToString() + "￦n" + jobj["url"].ToString()); //플러그인명,버전,url 출력

//     }

//}
        [Route("InRound/ExecuteRoundTreat/{hosCd}")]
        public HttpResponseResult<IList<IEnumerable<RoundTreat>>> ExecuteRoundTreat(string hosCd, System.Net.Http.HttpRequestMessage round)
        {
            try
            {
                JObject jObject = JObject.Parse(round.Content.ReadAsStringAsync().Result);

                string roundObj = jObject["round"].ToString();

                JArray jArr = JArray.Parse(roundObj);

                IList<IEnumerable<RoundTreat>> list = new List<IEnumerable<RoundTreat>>();
                foreach (var jObj in jArr)
                {
                    var param = new DynamicParameters();

                    param.Add(name: "@HosCd", value: jObj["hospitalCd"].ToString(), dbType: DbType.StringFixedLength, size: 2);
                    param.Add(name: "@UnitNo", value: jObj["patientId"].ToString(), dbType: DbType.StringFixedLength, size: 10);
                    param.Add(name: "@ClnYmd", value: jObj["roundTreatDt"].ToString(), dbType: DbType.StringFixedLength, size: 8);
                    param.Add(name: "@ClnHms", value: jObj["roundTreatTm"].ToString(), dbType: DbType.StringFixedLength, size: 6);
                    param.Add(name: "@Comment", value: jObj["roundTreatMemo"].ToString(), dbType: DbType.String, size: 500);
                    param.Add(name: "@SysYmd", value: jObj["inputDt"].ToString(), dbType: DbType.StringFixedLength, size: 8);
                    param.Add(name: "@SysHms", value: jObj["inputTm"].ToString(), dbType: DbType.StringFixedLength, size: 6);
                    param.Add(name: "@DeptCd", value: jObj["departmentCd"].ToString(), dbType: DbType.String, size: 6);
                    param.Add(name: "@UserId", value: jObj["userId"].ToString(), dbType: DbType.String, size: 8);

                    list.Add(SqlHelper.GetList<RoundTreat>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_executeRoundTreat", param: param));
                }

                //IList<IEnumerable<ExamRst>> iList = new List<IEnumerable<ExamRst>>
                //{
                //    tuples.Item1,
                //    tuples.Item2,
                //    tuples.Item3,
                //    tuples.Item4
                //};
                //IEnumerable<RoundTreat> info = SqlHelper.GetList<RoundTreat>(targetDB: SqlHelper.GetConnectionString("ZConnectionString"), storedProcedure: "USP_ZZ_EXT_IF_Mobile_executeRoundTreat", param: param);

                return new HttpResponseResult<IList<IEnumerable<RoundTreat>>> { result = list, error = new ErrorInfo { flag = false } };
                
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<IList<IEnumerable<RoundTreat>>> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }

        }
    }
}
