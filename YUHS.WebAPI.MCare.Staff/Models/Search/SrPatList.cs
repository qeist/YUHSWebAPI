namespace YUHS.WebAPI.MCare.Staff.Models.Search
{
    public class SrPatList
    {
        public string hospitalCd { get; set; }
        public string patientId { get; set; }
        public string patientNm { get; set; }
        public string age { get; set; }
        public string sex { get; set; }
        public string ward { get; set; }
        public string room { get; set; }
        public string departmentCd { get; set; }
        public string departmentNm { get; set; }
        public string doctorNm { get; set; }
        public string doctorId { get; set; }
        public string inDt { get; set; }
        public string inDay { get; set; }
    }
}