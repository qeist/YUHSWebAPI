namespace YUHS.WebAPI.MCare.Patient.Models.Tdrg
{
    public class PatientCounseling
    {
        public string hospitalCd { get; set; }
        public string orderCd { get; set; }
        public string drugNm { get; set; }
        public string drugMakerNm { get; set; }
        public string drugIngredient { get; set; }
        public string drugEffect { get; set; }
        public string drugStorage { get; set; }
        public string drugInteraction { get; set; }
        public string drugPhoto { get; set; }
    }
}