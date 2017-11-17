using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YUHS.WebAPI.MCare.Staff.Models.Consult
{
    public class ConsultReqList
    {
        public string hospitalCd { get; set; }
        public string requestDt { get; set; }
        public string consultTyp { get; set; }
        public string patientNm { get; set; }
        public string age { get; set; }
        public string sex { get; set; }
        public string patientId { get; set; }
        public string requestDeptCd { get; set; }
        public string requestDeptNm { get; set; }
        public string responseDtTm { get; set; }
        public string ward { get; set; }
        public string room { get; set; }
        public string responseDrId { get; set; }
        public string responseDrNm { get; set; }
        public string requestDrId { get; set; }
        public string requestDrNm { get; set; }
        public string hopeDrId { get; set; }
        public string hopeDrNm { get; set; }
        public string executeDeptCd { get; set; }
        public string executeDeptNm { get; set; }
        public string visitDt { get; set; }
    }
}