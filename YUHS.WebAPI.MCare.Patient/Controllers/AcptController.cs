using System;
using System.Data;
using System.Web.Http;
using YUHS.WebAPI.Common.Security;
using YUHS.WebAPI.MCare.Patient.Library;
using YUHS.WebAPI.MCare.Patient.Models.Common;

namespace YUHS.WebAPI.MCare.Patient.Controllers
{
    [RequireHttps]
    public class AcptController : ApiController
    {
        /*
         * 
         * 결제 관련 API는 파라미터방식으로 개발요청
         * ex : [Route("Reservation/SetReservation/{hosCd=hosCd}/{unitNo=unitNo}/{patNm=patNm}/{deptCd=deptCd}/{drId=drId}/{ymd=ymd}/{hhmm=hhmm}/{memo?}")]
         * 
        */
        [Route("Acpt/GetPaymentList/{hosCd=hosCd}/{unitNo=unitNo}")]
        public HttpResponseDataTableResult<DataTable> GetPaymentList(string hosCd, string unitNo)
        {
            try
            {
                //01 5935106
                AcptLib acptLib = new AcptLib();

                var dt = acptLib.GetPaymentList(hosCd, unitNo);

                return new HttpResponseDataTableResult<DataTable> { result = dt, error = new ErrorInfo { flag = false } };

            }
            catch (Exception ex)
            {
                Common.Utility.Utils._WriteLog(ex.Message, $"Acpt/{nameof(GetPaymentList)}");
                return new HttpResponseDataTableResult<DataTable> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }
    }
}
