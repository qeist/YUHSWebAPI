using System;
using System.Collections;
using System.Data;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml;

namespace YUHS.WebAPI.Common.Communication
{
    public class HIS20ProxyFactory
    {
        #region Service Interface
        [ServiceContract]
        public interface NUTFacade
        {
            [OperationContract]
            DataSet SelectMlOrderListByChosNo_DietReqNtx(string pUnitNo, string pOrdYmd);

            [OperationContract]
            void InsertMlAddSpmen_DietReqNtx(Hashtable pTempHT, DataSet pTempDS);

            [OperationContract]
            void DeleteMlAddSpmen(Hashtable pTempHT, DataSet pTempDS);
            
        }
        
        [ServiceContract]
        public interface ZZZFacade
        {
            [OperationContract]
            DataSet InsertKIOSKAutoHold(Hashtable ht);

            [OperationContract]
            DataTable setRcptRscMapping(DataSet OrdDs);

            [OperationContract]
            DataSet ChkAdmiKIOSKPsbl(Hashtable pht);

            [OperationContract]
            int KIOSK_MAX_RetrSeq(Hashtable pht);

            [OperationContract]
            DataTable GET_KIOSK_BILLDATA(string pChosGb, string pDscYn, string pChosNo1, string pBillNo1, string pChosNo2, string pBillNo2, string HosCd, string PcNm, string RgtId, string pTotalPayAmtBilYn);
        }

        [ServiceKnownType(typeof(DBNull))]
        [ServiceContract]
        public interface FEEFacade
        {
            [OperationContract]
            DataSet HpEtcDs(Hashtable pht);

            [OperationContract]
            DataSet SelectAcptContByChosNo(string pUnitNo, string pHosCd);

            [OperationContract]
            DataSet SelectRcptContByChosNo(string pUnitNo, string pFrClnYmd, string pToClnYmd, string pHosCd, string pGb);

            [OperationContract]
            DataSet ChkKIOSKExcept(DataSet OrdDs);

            [OperationContract]
            DataSet GetAmbAcptPayAmt(DataSet OrdDs, string strChosNo, int icRemitAmt);

            [OperationContract]
            DataSet SelectOrdByChosNo(string pChosNo, string pClnDeptCd, string pHosCd, string pUnitNo, string pSugaAppYmd, string pViewGb);

            [OperationContract]
            DataTable SelectAmbCrdAcptByChosNoList_AmbCrdAcptNTx(ArrayList chosNoList);

            [OperationContract]
            DataTable KIOSKAcptPsblDt(DataSet ordDs, string PsblGb, Hashtable _pht, string strHosCd, string strUnitNo, string strUserId);

            [OperationContract]
            DataSet SelectAdmiDep_AdmiAcpNtx(Hashtable ht);

            [OperationContract]
            void LogWrite(string pChosNo, string pUnitNo, string pLogGb, string pAppFrYmd, string pAppToYmd, string pLogNm, string pLog1, string pUserid, string pChosGb, string pBilNo, string pLog2, string pLog3);

            [OperationContract]
            string SelectPgmLog_AdmiAcptNTx(string pChosNo);

            [OperationContract]
            DataSet SelectFeeNoList(Hashtable pHt);

            [OperationContract]
            string AdmiAcptCmpIt_AdmiAcptTx(Hashtable pHt1, DataSet pDS);

            [OperationContract]
            int UPDATE_HP_KIOSKAdmiPat(Hashtable pht);

            [OperationContract]
            int INSERT_HP_KIOSKBillInfo(string DOMAIN, int RetrSeq, string billNo1, string ChosNo1, string billNo2, string ChosNo2, string KIOSKID, DataTable billDt, string pBilGb);

            [OperationContract]
            DataTable SaveAcptCalc(DataSet OrdDs, string pHosCd, string pUnitNo, string pWindId, string pUserId);

            [OperationContract]
            bool AcptVerific(DataSet ordDs, string pUnitNo, string pUserId);

            [OperationContract]
            void AmbAcptLogWrite(string pChosNo, string pUnitNo, string pLogGb, string pUserId, string pChosGb, string pBilNo, string pErroLocation, string pLogMessage);
        }
        #endregion

        #region GetBinding
        private static Binding GetBinding()
        {
            var httpTransport = new HttpTransportBindingElement()
            {
                MaxBufferSize = 2147483647,
                MaxBufferPoolSize = 2147483647,
                MaxReceivedMessageSize = 2147483647
            };

            var binaryEconding = new BinaryMessageEncodingBindingElement()
            {
                ReaderQuotas = new XmlDictionaryReaderQuotas()
                {
                    MaxDepth = 32,
                    MaxStringContentLength = 5242880,
                    MaxArrayLength = 200000,
                    MaxBytesPerRead = 4096,
                    MaxNameTableCharCount = 16384
                }
            };

            var retBinding = new CustomBinding(binaryEconding, httpTransport)
            {
                Name = "http_Unsecured",
                SendTimeout = TimeSpan.MaxValue,
                ReceiveTimeout = TimeSpan.MaxValue
            };

            return retBinding;
        }
        #endregion

        #region FEEProxy
        public class FEEProxy:FEEFacade
        {
            private Binding Binding { get; set; }
            private EndpointAddress Address { get; set; }
            private ChannelFactory<FEEFacade> Factory { get; set; }

            private FEEFacade Channel { get; set; }


            public FEEProxy()
            {
                Binding = GetBinding();
                Address = new EndpointAddress($@"{System.Configuration.ConfigurationManager.AppSettings["HIS20Server"]}/HP-FEE.svc");
                Factory = new ChannelFactory<FEEFacade>(Binding, Address);
                Channel = Factory.CreateChannel();
            }

            public DataSet HpEtcDs(Hashtable ht)
            {
                return Channel.HpEtcDs(ht);
            }

            public DataSet SelectAcptContByChosNo(string unitNo, string hosCd)
            {
                return Channel.SelectAcptContByChosNo(unitNo, hosCd);
            }

            public DataSet SelectRcptContByChosNo(string unitNo, string frClnYmd, string toClnYmd, string hosCd, string gb)
            {
                return Channel.SelectRcptContByChosNo(unitNo, frClnYmd, toClnYmd, hosCd, gb);
            }

            public DataSet ChkKIOSKExcept(DataSet ordDs)
            {
                return Channel.ChkKIOSKExcept(ordDs);
            }

            public DataSet GetAmbAcptPayAmt(DataSet ordDs, string chosNo, int remitAmtv)
            {
                return Channel.GetAmbAcptPayAmt(ordDs, chosNo, remitAmtv);
            }

            public DataSet SelectOrdByChosNo(string chosNo, string clnDeptCd, string hosCd, string unitNo, string sugaAppYmd, string viewGb)
            {
                return Channel.SelectOrdByChosNo(chosNo, clnDeptCd, hosCd, unitNo, sugaAppYmd, viewGb);
            }

            public DataTable SelectAmbCrdAcptByChosNoList_AmbCrdAcptNTx(ArrayList chosNoList)
            {
                return Channel.SelectAmbCrdAcptByChosNoList_AmbCrdAcptNTx(chosNoList);
            }

            public DataTable KIOSKAcptPsblDt(DataSet ordDs, string psblGb, Hashtable pht, string hosCd, string unitNo, string userId)
            {
                return Channel.KIOSKAcptPsblDt(ordDs, psblGb, pht, hosCd, unitNo, userId);
            }

            public DataSet SelectAdmiDep_AdmiAcpNtx(Hashtable ht)
            {
                return Channel.SelectAdmiDep_AdmiAcpNtx(ht);
            }

            public void LogWrite(string pChosNo, string pUnitNo, string pLogGb, string pAppFrYmd, string pAppToYmd, string pLogNm, string pLog1, string pUserid, string pChosGb, string pBilNo, string pLog2, string pLog3)
            {
                Channel.LogWrite(pChosNo, pUnitNo, pLogGb, pAppFrYmd, pAppToYmd, pLogNm, pLog1, pUserid, pChosGb, pBilNo, pLog2, pLog3);
            }

            public string SelectPgmLog_AdmiAcptNTx(string pChosNo)
            {
                return Channel.SelectPgmLog_AdmiAcptNTx(pChosNo);
            }

            public DataSet SelectFeeNoList(Hashtable pHt)
            {
                return Channel.SelectFeeNoList(pHt);
            }

            public string AdmiAcptCmpIt_AdmiAcptTx(Hashtable pHt1, DataSet pDS)
            {
                return Channel.AdmiAcptCmpIt_AdmiAcptTx(pHt1, pDS);
            }

            public int UPDATE_HP_KIOSKAdmiPat(Hashtable pht)
            {
                return Channel.UPDATE_HP_KIOSKAdmiPat(pht);
            }

            public int INSERT_HP_KIOSKBillInfo(string DOMAIN, int RetrSeq, string billNo1, string ChosNo1, string billNo2, string ChosNo2, string KIOSKID, DataTable billDt, string pBilGb)
            {
                return Channel.INSERT_HP_KIOSKBillInfo(DOMAIN, RetrSeq, billNo1, ChosNo1, billNo2, ChosNo2, KIOSKID, billDt, pBilGb);
            }

            public DataTable SaveAcptCalc(DataSet OrdDs, string pHosCd, string pUnitNo, string pWindId, string pUserId)
            {
                return Channel.SaveAcptCalc(OrdDs, pHosCd, pUnitNo, pWindId, pUserId);
            }

            public bool AcptVerific(DataSet ordDs, string pUnitNo, string pUserId)
            {
                return Channel.AcptVerific(ordDs, pUnitNo, pUserId);
            }

            public void AmbAcptLogWrite(string pChosNo, string pUnitNo, string pLogGb, string pUserId, string pChosGb, string pBilNo, string pErroLocation, string pLogMessage)
            {
                Channel.AmbAcptLogWrite(pChosNo, pUnitNo, pLogGb, pUserId, pChosGb, pBilNo, pErroLocation, pLogMessage);
            }
        }
        #endregion

        #region NUTProxy
        public class NUTProxy : NUTFacade
        {
            private Binding Binding { get; set; }
            private EndpointAddress Address { get; set; }
            private ChannelFactory<NUTFacade> Factory { get; set; }

            private NUTFacade Channel { get; set; }


            public NUTProxy()
            {
                Binding = GetBinding();
                Address = new EndpointAddress($@"{System.Configuration.ConfigurationManager.AppSettings["HIS20Server"]}/SP-NUT.svc");
                Factory = new ChannelFactory<NUTFacade>(Binding, Address);
                Channel = Factory.CreateChannel();
            }

            public DataSet SelectMlOrderListByChosNo_DietReqNtx(string pUnitNo, string pOrdYmd)
            {
                return Channel.SelectMlOrderListByChosNo_DietReqNtx(pUnitNo, pOrdYmd);
            }

            public void InsertMlAddSpmen_DietReqNtx(Hashtable pTempHT, DataSet pTempDS)
            {
                Channel.InsertMlAddSpmen_DietReqNtx(pTempHT, pTempDS);
            }

            public void DeleteMlAddSpmen(Hashtable pTempHT, DataSet pTempDS)
            {
                Channel.DeleteMlAddSpmen(pTempHT, pTempDS);
            }
        }
        #endregion

        #region ZZZProxy
        public class ZZZProxy : ZZZFacade
        {
            private Binding Binding { get; set; }
            private EndpointAddress Address { get; set; }
            private ChannelFactory<ZZZFacade> Factory { get; set; }

            

            private ZZZFacade Channel { get; set; }
            public ZZZProxy()
            {
                Binding = GetBinding();
                Address = new EndpointAddress($@"{System.Configuration.ConfigurationManager.AppSettings["HIS20Server"]}/HP-ZZZ.svc");
                Factory = new ChannelFactory<ZZZFacade>(Binding, Address);
                Channel = Factory.CreateChannel();
            }
            public DataSet InsertKIOSKAutoHold(Hashtable ht)
            {
                return Channel.InsertKIOSKAutoHold(ht);
            }
            
            public DataTable setRcptRscMapping(DataSet ordDs)
            {
                return Channel.setRcptRscMapping(ordDs);
            }

            public DataSet ChkAdmiKIOSKPsbl(Hashtable pht)
            {
                return Channel.ChkAdmiKIOSKPsbl(pht);
            }

            public int KIOSK_MAX_RetrSeq(Hashtable pht)
            {
                return Channel.KIOSK_MAX_RetrSeq(pht);
            }

            public DataTable GET_KIOSK_BILLDATA(string pChosGb, string pDscYn, string pChosNo1, string pBillNo1, string pChosNo2, string pBillNo2, string HosCd, string PcNm, string RgtId, string pTotalPayAmtBilYn)
            {
                return Channel.GET_KIOSK_BILLDATA(pChosGb, pDscYn, pChosNo1, pBillNo1, pChosNo2, pBillNo2, HosCd, PcNm, RgtId, pTotalPayAmtBilYn);
            }
        }
        #endregion

    }
}

