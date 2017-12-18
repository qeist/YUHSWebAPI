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
            DataSet SelectMlOrderListByChosNo_DietReqNtx(string unitNo, string ordYmd);

            [OperationContract]
            DataTable SelectAdmiChkByUnitNo(string sParam);

            [OperationContract]
            DataTable SelectMlOrdMList_DietReqNtx();

            [OperationContract]
            void InsertMlAddSpmen_DietReqNtx(Hashtable pTempHT, DataSet pTempDS);

            [OperationContract]
            void DeleteMlAddSpmen(Hashtable pTempHT, DataSet pTempDS);

            [OperationContract]
            void InsertDailyMlOrdCncl(string[] sParam);

            [OperationContract]
            DataTable SelectAdMlOrd(string[] sParam);

            [OperationContract]
            DataSet InitInquiry();
        }
        
        [ServiceContract]
        public interface ZZZFacade
        {
            [OperationContract]
            DataSet InsertKIOSKAutoHold(Hashtable ht);

            [OperationContract]
            DataTable setRcptRscMapping(DataSet OrdDs);
        }

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

            public void DeleteMlAddSpmen(Hashtable pTempHT, DataSet pTempDS)
            {
                throw new NotImplementedException();
            }

            public DataSet InitInquiry()
            {
                return Channel.InitInquiry();
            }

            public void InsertDailyMlOrdCncl(string[] sParam)
            {
                throw new NotImplementedException();
            }

            public void InsertMlAddSpmen_DietReqNtx(Hashtable pTempHT, DataSet pTempDS)
            {
                throw new NotImplementedException();
            }

            public DataTable SelectAdmiChkByUnitNo(string sParam)
            {
                throw new NotImplementedException();
            }

            public DataTable SelectAdMlOrd(string[] sParam)
            {
                throw new NotImplementedException();
            }

            public DataSet SelectMlOrderListByChosNo_DietReqNtx(string unitNo, string ordYmd)
            {
                return Channel.SelectMlOrderListByChosNo_DietReqNtx(unitNo, ordYmd);
            }

            public DataTable SelectMlOrdMList_DietReqNtx()
            {
                throw new NotImplementedException();
            }
        }
        #endregion

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
        }

    }
}

