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
        [Route("Acpt/GetPaymentList/{hosCd?}/{unitNo?}/{chosGb?}/{proxyYn?}")]
        public HttpResponseDataTableResult<DataTable> GetPaymentList(string hosCd, string unitNo, string chosGb, string proxyYn)
        {
            try
            {
                AcptLib acptLib = new AcptLib();

                var dt = acptLib.GetPaymentList(hosCd ?? "", unitNo ?? "", chosGb ?? "", proxyYn ?? "");

                return new HttpResponseDataTableResult<DataTable> { result = dt, error = new ErrorInfo { flag = false } };

            }
            catch (Exception ex)
            {
                Common.Utility.Utils._WriteLog(ex.Message, $"Acpt/{nameof(GetPaymentList)}");
                return new HttpResponseDataTableResult<DataTable> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("Acpt/GetPaymentOPD/{hosCd?}/{unitNo?}/{chosGb?}/{chosNo?}/{deptCd?}/{clnYmd?}/{acptYmd?}/{proxyYn?}")]
        public HttpResponseDataTableResult<DataTable> GetPaymentOPD(string hosCd, string unitNo, string chosGb, string chosNo, string deptCd, string clnYmd, string acptYmd, string proxyYn)
        {
            try
            {
                //01 5935106
                AcptLib acptLib = new AcptLib();

                var dt = acptLib.GetPaymentOPD(hosCd ?? "", unitNo ?? "", chosGb ?? "", chosNo ?? "", deptCd ?? "", clnYmd ?? "", acptYmd ?? "", proxyYn ?? "");

                return new HttpResponseDataTableResult<DataTable> { result = dt, error = new ErrorInfo { flag = false } };

            }
            catch (Exception ex)
            {
                Common.Utility.Utils._WriteLog(ex.Message, $"Acpt/{nameof(GetPaymentOPD)}");
                return new HttpResponseDataTableResult<DataTable> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        //---입원---
        [Route("Acpt/GetAdmiPaymentList/{hosCd?}/{unitNo?}/{chosGb?}/{chosNo?}/{deptCd?}/{maintSubCd?}/{clnYmd?}/{acptYmd?}/{proxyYn?}")]
        public HttpResponseDataTableResult<DataTable> GetPaymentList(string hosCd, string unitNo, string chosGb, string chosNo, string deptCd, string maintSubCd, string clnYmd, string acptYmd, string proxyYn)
        {
            try
            {
                AcptAdmiLib acptAdmiLib = new AcptAdmiLib();

                var dt = acptAdmiLib.GetPaymentList(hosCd ?? "", unitNo ?? "", chosGb ?? "", chosNo ?? "", deptCd ?? "", maintSubCd ?? "", clnYmd ?? "", acptYmd ?? "", proxyYn ?? "");

                return new HttpResponseDataTableResult<DataTable> { result = dt, error = new ErrorInfo { flag = false } };

            }
            catch (Exception ex)
            {
                Common.Utility.Utils._WriteLog(ex.Message, $"Acpt/{nameof(GetPaymentList)}");
                return new HttpResponseDataTableResult<DataTable> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }

        }


        [Route("Acpt/SetPayment/{hosCd?}/{unitNo?}/{chosGb?}/{chosNo?}/{clnYmd?}/{crdIssLoc?}/{crdNo?}/{crdTypGb?}/{vldThru?}/{instMcnt?}/{permAmt?}/{permYmd?}/{permHms?}/{permNo?}/{slpNo?}/{mbstNo?}/{inpurcCoNm?}/{crdData?}/{vanSeqNo?}/{crdPermMeth?}/{iCPermMeth?}/{pOSNo?}/{vanGb?}/{inpurcCd?}")]

        public HttpResponseDataTableResult<DataTable> SetPayment(string hosCd, string unitNo, string chosGb, string chosNo
            , string clnYmd, string crdIssLoc, string crdNo, string crdTypGb
            , string vldThru, string instMcnt, string permAmt, string permYmd, string permHms, string permNo, string slpNo, string mbstNo, string inpurcCoNm
            , string crdData, string vanSeqNo, string crdPermMeth, string iCPermMeth, string pOSNo, string vanGb, string inpurcCd)
        {
            try
            {
                AcptAdmiLib acptAdmiLib = new AcptAdmiLib();

                DataTable dt;

                if (chosGb == "O")
                {
                    AcptLib acptLib = new AcptLib();

                    dt = acptLib.SaveAcptCalc(
                  hosCd ?? ""
                , unitNo ?? ""
                , chosGb ?? ""
                , chosNo ?? ""
                , clnYmd ?? ""
                , crdIssLoc ?? ""
                , crdNo ?? ""
                , crdTypGb ?? ""
                , vldThru ?? ""
                , instMcnt ?? ""
                , permAmt ?? ""
                , permYmd ?? ""
                , permHms ?? ""
                , permNo ?? ""
                , slpNo ?? ""
                , mbstNo ?? ""
                , inpurcCoNm ?? ""
                , crdData ?? ""
                , vanSeqNo ?? ""
                , crdPermMeth ?? ""
                , iCPermMeth ?? ""
                , pOSNo ?? ""
                , vanGb ?? ""
                , inpurcCd ?? ""
                );

                }
                else
                {
                    dt = acptAdmiLib.AcptExec(hosCd ?? "", unitNo ?? "", chosGb ?? "", chosNo ?? "", clnYmd ?? "", crdIssLoc ?? "", crdNo ?? "", crdTypGb ?? "", vldThru ?? "", instMcnt ?? ""
               , permAmt ?? "", permYmd ?? "", permHms ?? "", permNo ?? "", slpNo ?? "", mbstNo ?? "", inpurcCoNm ?? "", crdData ?? "", vanSeqNo ?? "", crdPermMeth ?? "", iCPermMeth ?? ""
               , pOSNo ?? "", vanGb ?? "", inpurcCd ?? "");
                }

                return new HttpResponseDataTableResult<DataTable> { result = dt, error = new ErrorInfo { flag = false } };

            }
            catch (Exception ex)
            {
                Common.Utility.Utils._WriteLog(ex.Message, $"Acpt/{nameof(SetPayment)}");
                return new HttpResponseDataTableResult<DataTable> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }
    }
}
