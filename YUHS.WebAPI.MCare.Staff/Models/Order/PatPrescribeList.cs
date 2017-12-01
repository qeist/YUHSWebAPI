using System;

namespace YUHS.WebAPI.MCare.Staff.Models.Order
{
    public class PatPrescribeList
    {
        public string hospitalCd { get; set; }
        public string supportDeptNm { get; set; }
        public string prescriptionCd { get; set; }
        public string prescriptionNm { get; set; }
        public string prescriptionStat { get; set; }
        public string roomInfo { get; set; }
        public float doseQtyPerTim { get; set; }
        public Int16 doseQtyPerDay { get; set; }
        public Int16 doseDay { get; set; }
        public string commentYn { get; set; }
        public string appointYn { get; set; }
        public string collectDt { get; set; }
        public string procedureCd { get; set; }
        public string doctorId { get; set; }
        public string doctorNm { get; set; }
        public string hopeDt { get; set; }
        public string comment { get; set; }
        public string patientId { get; set; }

    }
}