using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YUHS.WebAPI.MCare.Patient.Models.LabResult
{
    public class LabResult
    {
        public string hospitalCd { get; set; }
        public string orderGrpCd { get; set; }
        public string orderGrpNm { get; set; }
        public string orderCd { get; set; }
        public string orderNm { get; set; }
        public string orderCd2 { get; set; }
        public string orderNm2 { get; set; }
        public string orderDt { get; set; }
        public string orderResultVal { get; set; }
        public string orderResultDt { get; set; }
        public string referenceVal { get; set; }
        public string unit { get; set; }
        public string referenceMinVal { get; set; }
        public string referenceMaxVal { get; set; }
        public string referencMinMaxVal { get; set; }
        public string beforeResultVal { get; set; }
    }
}