using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using static YUHS.WebAPI.Common.Communication.HIS20ProxyFactory;

namespace YUHS.WebAPI.MCare.Patient.Library
{
    public class NutLib
    {
        public NUTFacade _NUTFacade { get; set; }

        public NutLib()
        {
            _NUTFacade = new NUTProxy();
        }

        public DataSet SelectMlOrderListByChosNo_DietReqNtx(string pUnitNo, string pOrdYmd)
        {
            return _NUTFacade.SelectMlOrderListByChosNo_DietReqNtx(pUnitNo, pOrdYmd);
        }

        public void InsertMlAddSpmen_DietReqNtx(Hashtable pTempHT, DataSet pTempDS)
        {
            _NUTFacade.InsertMlAddSpmen_DietReqNtx(pTempHT, pTempDS);
        }

        public void DeleteMlAddSpmen(Hashtable pTempHT, DataSet pTempDS)
        {
            _NUTFacade.DeleteMlAddSpmen(pTempHT, pTempDS);
        }
    }
}