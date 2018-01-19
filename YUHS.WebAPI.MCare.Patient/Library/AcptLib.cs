using System;
using System.Collections;
using System.Data;
using static YUHS.WebAPI.Common.Communication.HIS20ProxyFactory;

namespace YUHS.WebAPI.MCare.Patient.Library
{
    public class AcptLib
    {
        private string strHosCd;
        private string strUnitNo;
        private string strUserid;
        private string strChosNo;
        private string strClnYmd;
        private string strErrLogNm = "Mobile 결제 수납 테이블 처리 오류";
        private string strPermAmt;
        private string strChosGb;
        private string strLogDesc2;

        public ZZZFacade _ZZZFacade { get; set; }
        public FEEFacade _FEEFacade { get; set; }

        public DataTable errDt { get; set; }

        public AcptLib()
        {
            _ZZZFacade = new ZZZProxy();
            _FEEFacade = new FEEProxy();
        }
        private bool CheckAcptPsbl = true;		// 수납불가 대상 체크를 실행한다.  - 모바일은 무조건 체크
        public string HosCd { get; set; }
        public string UnitNo { get; set; }
        public string UserId { get; set; }

        private string MOBILE_GB { get; } = "M";
        private string MOBILE_USER { get; } = "MOBILE";
        private string MOBILE_WINDID { get; } = "MO";
        private string MOBILE_DOMAINNM { get; } = "MOBILE";

        public DataTable GetPaymentList(string hosCd, string unitNo, string chosGb, string proxyYn)
        {
            #region 기존 소스

            //[16.11.09]DataSet ds=SelectRcptContByChosNo( pUnitNo, "" , "", HosCd, "A",  pUserid);
            //DataSet ds = SelectRcptContByChosNo(pUnitNo, "", "", HosCd, "K", pUserid); //2016.11.09 이용주 무인수납기에서 예약비 수납못하게 @strGb가 strGb 파라미터 A ->K 로 변경

            #region ■ DEBUG

            //			StreamWriter _fs = new StreamWriter("C:\\OCSCardLog\\DEBUG.txt",true);	
            //
            //			if ( ds == null )
            //			{
            //				_fs.WriteLine(pUnitNo + " / " +HosCd+ " /  NULL 입니다." ); 
            //				_fs.Close(); 
            //			}
            //			else
            //			{
            //				_fs.WriteLine(pUnitNo + " / " +HosCd+ " / "+ "TABLE COUNT : " + ds.Tables.Count + "\n" + "내원건 Count : " + ds.Tables["Ord"].Rows.Count ); 
            //				_fs.Close(); 
            //			}
            #endregion

            //return ds;

            #endregion



            DataTable resultDt = null;

            #region ☆ 2017.06.26 시행예정일 자동보류처리- 20180105 메서드추출 - 메서드 추출

            ////20170714 추가 WindId 정보를 가지고 오기 위해서(병원코드, PC명, 오늘날짜) 
            ////DataTable dtWindId = GetWindId(HosCd, Dns.GetHostName().Trim(), DateTime.Now.ToString("yyyyMMdd"));

            ////strWindId = dtWindId.Rows[0]["WindId"].ToString().Trim();

            //HIS.Facade.HP.ZZZ.ZZZFacade _zzzFacade = new HIS.Facade.HP.ZZZ.ZZZFacade();

            //Hashtable hs = new Hashtable();

            //hs.Add("HosCd", hosCd);
            //hs.Add("DomainNm", MOBILE_USER);
            //hs.Add("UnitNo", unitNo);
            //hs.Add("ChosNo", "");
            //hs.Add("Rmk1", "");
            //hs.Add("Rmk2", "");
            //hs.Add("Rmk3", "");
            //hs.Add("WindId", MOBILE_WINDID);
            //hs.Add("UserId", MOBILE_USER);
            //hs.Add("HoldGb", "A");

            //// 프로시져 호출 : USP_HP_FEE_IP_INSERT_HP_KIOSKAutoHold
            //DataSet dss = _zzzFacade.InsertKIOSKAutoHold(hs);

            //string PlnExecYmdUseYn = dss.Tables["Hold"].Rows[0]["PlnExecYmdUseYn"].ToString().Trim();


            #endregion

            string PlnExecYmdUseYn = FnPlnExecYmdUseYn(hosCd, unitNo, "A", "");

            if (PlnExecYmdUseYn == "Y")
            {
                resultDt = getPaymentList_1(hosCd, unitNo, "M", chosGb);
                //_OrdDs = ds;
                //return ds;
            }
            else
            {
                //    //[16.11.09]DataSet ds=SelectRcptContByChosNo( pUnitNo, "" , "", HosCd, "A",  pUserid);
                //모바일에선 K가 아니라 M으로 
                resultDt = getPaymentList_1(hosCd, unitNo, "K", chosGb); //2016.11.09 이용주 무인수납기에서 예약비 수납못하게 @strGb가 strGb 파라미터 A ->K 로 변경
                //_OrdDs = ds;
                //return ds;
            }

            #region ☆ 2017.06.26 시행예정일 자동보류처리- 메서드 추출

            //if (resultDt == null || resultDt.Columns.Contains("ErrorMsg") )
            //if (resultDt == null || (resultDt.Rows.Count > 0
            //    && resultDt.Rows[0]["ErrorMsg"]  != null 
            //    && resultDt.Rows[0]["ErrorMsg"].ToString() != ""))
            //{

            //Hashtable hss = new Hashtable();

            //hss.Add("HosCd", hosCd);
            //hss.Add("DomainNm", MOBILE_USER);
            //hss.Add("UnitNo", unitNo);

            //hss.Add("ChosNo", "");
            //hss.Add("Rmk1", "");
            //hss.Add("Rmk2", "");
            //hss.Add("Rmk3", "");
            //hss.Add("WindId", MOBILE_WINDID);
            //hss.Add("UserId", MOBILE_USER);
            //hss.Add("HoldGb", "C");

            //// 프로시져 호출 : USP_HP_FEE_IP_INSERT_HP_KIOSKAutoHold
            //_zzzFacade.InsertKIOSKAutoHold(hss);

            //}

            #endregion

            //모바일 UnitNo조회시  무조건 원복
            FnPlnExecYmdUseYn(hosCd, unitNo, "C", "");

            return resultDt;
        }
        #region ☆ 2017.06.26 시행예정일 자동보류처리- 모바일 메서드 추출
        private string FnPlnExecYmdUseYn(string hosCd, string unitNo, string HoldGb, string rmk1)
        {
            Hashtable hs = new Hashtable();

            hs.Add("HosCd", hosCd);
            hs.Add("DomainNm", MOBILE_USER);
            hs.Add("UnitNo", unitNo);
            hs.Add("ChosNo", "");
            hs.Add("Rmk1", rmk1);
            hs.Add("Rmk2", "");
            hs.Add("Rmk3", "");
            hs.Add("WindId", MOBILE_WINDID);
            hs.Add("UserId", MOBILE_USER);
            hs.Add("HoldGb", HoldGb);

            // 프로시져 호출 : USP_HP_FEE_IP_INSERT_HP_KIOSKAutoHold
            DataSet dss = _ZZZFacade.InsertKIOSKAutoHold(hs);

            string PlnExecYmdUseYn = "";
            if (HoldGb == "A")
            {
                PlnExecYmdUseYn = dss.Tables["Hold"].Rows[0]["PlnExecYmdUseYn"].ToString().Trim();
            }

            return PlnExecYmdUseYn;
        }
        #endregion

        private DataTable getPaymentList_1(string hosCd, string unitNo, string gb, string chosGb)
        {
            //string pUserid = "MOBILE"; //? MOBILE로 바꾸면 아래 걸림.
            //if (pUserid.Length >= 8)
            //    pUserid = pUserid.Substring(0, 8);

            DataSet ds = null;
            DataSet _result = null;


            strHosCd = hosCd;		// 병원코드를 넣는다. 
            strUnitNo = unitNo;	// 등록번호를 입력 
            strUserid = MOBILE_USER;	// UserId


            DataTable Ord = getChosListTable();
            string strPatNm = "";
            try
            {
                //HIS.Facade.HP.FEE.FEEFacade facade = new HIS.Facade.HP.FEE.FEEFacade();

                // 등록번호별로 무인수납 불가대상을 선별한다. 


                #region ● KIOSK 에서 환자에 대한 수납 불가 여부를 조회한다.

                Hashtable pht = new Hashtable();
                pht.Add("TRNSGB", "N");
                pht.Add("SPGB", "KIOSK_GETPATINFO");
                pht.Add("UnitNo", unitNo);
                pht.Add("TermId", MOBILE_USER);

                if (CheckAcptPsbl)
                {
                    // 데이터 조회 : 강승숙 선생님의 SP 실행
                    DataSet AcptPsbl = _FEEFacade.HpEtcDs(pht);

                    errDt = KIOSKAcptPsbl(AcptPsbl, "KIOSKAcptPsbl", null);

                    if (errDt != null)
                    {
                        //DataSet returnDs = new DataSet();
                        //returnDs.Merge(errDt);
                        //return returnDs; 
                        DataRow rOrd = Ord.NewRow();
                        rOrd["ErrorMsg"] = errDt.Rows[0]["ERROR_MSG"].ToString();
                        Ord.Rows.Add(rOrd);
                        Ord.AcceptChanges();
                        return Ord;
                    }

                    // 환자 정보에 대한 처리 
                    DataSet patInfoDs = _FEEFacade.SelectAcptContByChosNo(unitNo, hosCd);

                    if (patInfoDs.Tables.Contains("PatInfo") && patInfoDs.Tables["PatInfo"].Rows.Count > 0 && patInfoDs.Tables["PatInfo"].Rows[0]["PatNm"] != null)
                    {
                        strPatNm = patInfoDs.Tables["PatInfo"].Rows[0]["PatNm"].ToString();
                    }
                    errDt = KIOSKAcptPsbl(patInfoDs, "UnitNo", null);

                    if (errDt != null)
                    {
                        //DataSet returnDs = new DataSet();
                        //returnDs.Merge(errDt);
                        //return returnDs;
                        DataRow rOrd = Ord.NewRow();
                        rOrd["ErrorMsg"] = errDt.Rows[0]["ERROR_MSG"].ToString();
                        Ord.Rows.Add(rOrd);
                        Ord.AcceptChanges();
                        return Ord;
                    }
                }

                #endregion


                // 내원건을 뽑아오잒~
                //ds = facade.SelectRcptContByChosNo(pUnitNo, pFrClnYmd, pToClnYmd, pHosCd, pGb);
                ds = _FEEFacade.SelectRcptContByChosNo(unitNo, "", "", hosCd, gb);
                if (ds == null || ds.Tables.Count == 0 || !ds.Tables.Contains("RcptCont") || ds.Tables["RcptCont"].Rows.Count == 0)
                {
                    DataRow rOrd = Ord.NewRow();
                    rOrd["ErrorMsg"] = "수납하실 내역이 없습니다";
                    Ord.Rows.Add(rOrd);
                    Ord.AcceptChanges();
                    return Ord;
                }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables.Contains("RcptCont") && ds.Tables["RcptCont"].Rows.Count > 0)
                {
                    // RH 과이면서, 진료일자가 오늘이 아닌 과는 모두 삭제한다. 
                    // 추후 내원사유( ChosResnCd ) 를 판단해서, 물리치료(E) 인 것만 골라내도록 한다. 
                    //	추후 반영 SP :	USP_HP_FEE_OP_AmbAcptDA_SelectRcptContByChosNoA
                    // 접수일자 가 오늘 이고 물리치료(E) 일경우 수납내역 표시
                    // 접수일자 오늘이 아니고 물리치료(E) 일경우 수납내역 표시 안함

                    //[11.10.13]신촌도 적용(SM-11-01425)if (IFUtil.IsYD())		/*IFUtil.GetLocationCd() == "10"*/	// 영동일 경우 
                    //[11.10.13]{
                    string toDayClnYmd = DateTime.Now.ToString("yyyyMMdd");		// ClnYmdDt

                    // [07.02.19]
                    // string selectedContent = "TRIM(ClnDeptCd) LIKE 'RH' "; 
                    string selectedContent = " TRIM(ChosResnCd) LIKE 'E' ";
                    selectedContent += "AND TRIM(ClnYmdDt) NOT LIKE '" + toDayClnYmd + "'";

                    DataRow[] deleteRow = ds.Tables["RcptCont"].Select(selectedContent);

                    for (int i = 0; i < deleteRow.Length; i++)
                    {
                        ds.Tables["RcptCont"].Rows.Remove(deleteRow[i]);
                    }
                    //[11.10.13]}
                }

                ds.Tables["RcptCont"].AcceptChanges();
                ds.AcceptChanges();

                // 내원번호 별로 진료비를 계산하자~
                _result = SetRcptFee(ref ds, unitNo, hosCd);

                pht.Clear();
                pht = null;

                pht = new Hashtable();
                pht.Add("UnitNo", unitNo);
                pht.Add("HosCd", hosCd);

                if (_result == null)
                {
                    //return null;
                    //20180105 계산결과없을경우
                    DataRow rOrd = Ord.NewRow();
                    rOrd["ErrorMsg"] = "계산 내역 조회 실패. 다시 진행 하세요.";
                    Ord.Rows.Add(rOrd);
                    Ord.AcceptChanges();
                    return Ord;
                }

                _result = _FEEFacade.ChkKIOSKExcept(_result);

                if (_result == null)
                {
                    //return null;
                    //20180105 계산결과없을경우
                    DataRow rOrd = Ord.NewRow();
                    rOrd["ErrorMsg"] = "계산 내역 조회 실패. 다시 진행 하세요.";
                    Ord.Rows.Add(rOrd);
                    Ord.AcceptChanges();
                    return Ord;
                }

                if (CheckAcptPsbl)
                {
                    errDt = KIOSKAcptPsbl(_result, "ChosNo", pht);

                    // KIOSK 불가 대상을 내원번호 별로 선별하자 
                    if (errDt != null)
                    {
                        // KIOSK 불가 대상일 경우 
                        //DataSet returnDs = new DataSet();
                        //returnDs.Merge(errDt);
                        //return returnDs;
                        DataRow rOrd = Ord.NewRow();
                        rOrd["ErrorMsg"] = errDt.Rows[0]["ERROR_MSG"].ToString();
                        Ord.Rows.Add(rOrd);
                        Ord.AcceptChanges();
                        return Ord;
                    }
                }

                for (int i = 0; i < _result.Tables["Ord"].Rows.Count; i++)
                {
                    //일단은 외래?
                    DataRow rOrd = Ord.NewRow();

                    rOrd["HosCd"] = hosCd;
                    rOrd["ClnDeptCd"] = _result.Tables["Ord"].Rows[i]["ClnDeptCd"];
                    rOrd["DeptNm"] = _result.Tables["Ord"].Rows[i]["DeptNm"];
                    rOrd["ClnDrId"] = _result.Tables["Ord"].Rows[i]["ClnDrId"];
                    rOrd["UserNm"] = _result.Tables["Ord"].Rows[i]["UserNm"];
                    rOrd["ClnYmd"] = Convert.ToDateTime(_result.Tables["Ord"].Rows[i]["ClnYmd"]).ToString("yyyyMMdd");
                    rOrd["ClnHms"] = _result.Tables["Ord"].Rows[i]["ClnHms"];
                    rOrd["ChosNo"] = _result.Tables["Ord"].Rows[i]["ChosNo"];
                    rOrd["AcptYmd"] = DateTime.Now.ToString("yyyyMMdd");
                    rOrd["AcptGb"] = "";
                    rOrd["CalcStrYmd"] = "";//외래빈값
                    rOrd["CalcEndYmd"] = "";//외래빈값
                    rOrd["ChosGb"] = chosGb;
                    //안내번호
                    rOrd["InfoMsg"] = "";
                    for (int j = 0; j < ds.Tables["RcptCont"].Rows.Count; j++)
                    {
                        if (_result.Tables["Ord"].Rows[i]["ChosNo"].ToString() == ds.Tables["RcptCont"].Rows[i]["ChosNo"].ToString())
                        {
                            rOrd["InfoMsg"] = ds.Tables["RcptCont"].Rows[i]["InfoMsg"].ToString();
                            break;
                        }
                    }
                    rOrd["ErrorMsg"] = "";
                    rOrd["PatNm"] = strPatNm;
                    rOrd["UnitNo"] = unitNo;
                    rOrd["DisCalcYn"] = "";

                    string iChosNo = _result.Tables["Ord"].Rows[i]["ChosNo"].ToString();

                    int PharmCnt = 0;
                    int OtherCnt = 0;

                    //약처방관련
                    if (_result.Tables.Contains("Ord" + iChosNo))
                    {
                        //약건수만 있으면 수납이 안되야됨
                        //그외 건수가 있으면 수납이 되야됨
                        //접수비(RcptAmt)가 널이면 수납이되야됨(있으면수납된것) 그런데 접수비는 otherCnt에 카운트가 안되서 카운트를 따로해준다
                        for (int j = 0; j < ds.Tables["RcptCont"].Rows.Count; j++)
                        {
                            if (iChosNo == ds.Tables["RcptCont"].Rows[j]["ChosNo"].ToString()
                                && Convert.ToString(ds.Tables["RcptCont"].Rows[j]["RcptAmt"]) == "")
                            {
                                OtherCnt++;
                                break;
                            }
                        }

                        for (int j = 0; j < _result.Tables["Ord" + iChosNo].Rows.Count; j++)
                        {
                            if (_result.Tables["Ord" + iChosNo].Rows[j]["AcptGb"].ToString() == "N")
                            {
                                if (_result.Tables["Ord" + iChosNo].Rows[j]["OrdTypCd"].ToString() == "P")
                                    PharmCnt++;//약 처방건수
                                else
                                    OtherCnt++;//그 외 처방건수
                            }
                        }
                    }

                    rOrd["PharmCnt"] = Convert.ToString(PharmCnt);
                    rOrd["OtherCnt"] = Convert.ToString(OtherCnt);
                    Ord.Rows.Add(rOrd);
                    Ord.AcceptChanges();

                }
            }
            catch (Exception Ex)
            {
                //DataSet returnDs = new DataSet();
                //returnDs.Merge(GetErrorDt("", "ERR_GETRCPTINFO", "접수내역확인", "B"));
                //return returnDs;
                DataRow rOrd = Ord.NewRow();
                rOrd["ErrorMsg"] = errDt.Rows[0]["ERROR_MSG"].ToString();
                Ord.Rows.Add(rOrd);
                return Ord;
            }

            // 이상이 없을 경우
            // 혹시 ERROR_MSG 가 있으면 삭제한다. 
            //if (_result.Tables.Contains("ERROR_MSG"))
            //    _result.Tables.Remove("ERROR_MSG");

            return Ord;
        }

        private DataSet SetRcptFee(ref DataSet ds, string unitNo, string hosCd)
        {
            DataSet ordDs = new DataSet();	// 최종적으로 데이터를 가지고 있는 DataSet 

            DataTable _dt = ds.Tables["RcptCont"];	//접수 내역만 얻어온다. 

            // 수납완료 (ReCalcYn = 'Y') 인 건은 날려 버린다. 
            DataRow[] _dr = _dt.Select(" ReCalcYn = 'N'");
            for (int i = 0; i < _dr.Length; i++) _dt.Rows.Remove(_dr[i]);

            if (_dt.Rows.Count <= 0) return null;

            DataSet _FeeList = null;
            DataRow aRow = null;

            string strChosNo = string.Empty;	//내원번호
            string strClnDeptCd = string.Empty; //진료부서

            string currentDeptCd = string.Empty;
            string tempDeptCd = string.Empty;



            #region 주석 처리

            // ※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★
            // 1. 진료비_예약비 DT를 처리합니다. 
            //			DataTable _reservation = new DataTable("ResvBillDT"); 
            //			_reservation.Columns.Add("RcptGb", typeof(int)) ;		// 접수/예약 구분
            //			_reservation.Columns.Add("ChosNo", typeof(string )) ;		// 접수 건
            //			_reservation.Columns.Add("ResvChosNo", typeof(string )) ;	// 예약 건

            // 여기에서 부터 접수건과 예약건을 구분한다.
            //			int addindex = _dt.Rows.Count; 


            // ※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★
            // 2. 마지막 Row 의 위치를 만듭니다. 
            //	정렬 후 삭제처리를 할 계획 입니다. 
            //			DataRow demyRow = _dt.NewRow();		// Dumy Row 추가하고 ... 
            //			demyRow["ChosNo"] = "-"; 
            //			demyRow["ClnDeptCd"] = ""; 
            //            _dt.Rows.InsertAt( demyRow,addindex); 

            // ※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★
            // 3. 접수내역에 대해 부서별, 일자별로 정렬을 한다. 
            //			DataRow[] RcptResvRow = _dt.Select("", " ClnDeptCd Desc , ClnYmd " );	// 부서 코드는 같다 .. 그리고 더미로 들어가는 코드는 '' 로 Hash 비교에서 가장 작다. 



            //			DataRow _reservationRow = null;
            //			int RcptCount = 0; 


            // ※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★
            // 4. 접수 건을 돌리면서, 같은 진료부서에 대해 Sort 를 합니다.
            //			for ( int i=0; i< RcptResvRow.Length ; i++)
            //			{
            //				currentDeptCd = RcptResvRow[i]["ClnDeptCd"].ToString().Trim(); 
            //
            //				if ( currentDeptCd == "" ) break;	// 맨 마지막일 경우에는 탈출을 한다. 
            //
            //				// ※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★※☆★
            //				// 4.1 ☆ 내원건을 돌면서 만든 데이터를  
            //				//		_reservation 에 저장합니다. 
            //				_reservationRow = _reservation.NewRow(); 
            //
            //				if ( currentDeptCd != tempDeptCd )
            //				{
            //					// 진료 부서가 틀릴 경우의 처리 
            //					// _reservation에서 RcptGb 에 O를 체크한다. 
            //					_reservationRow["RcptGb"] = 0; 
            //					_reservationRow["ChosNo"] = RcptResvRow[i]["ChosNo"].ToString().Trim(); 
            //					_reservationRow["ResvChosNo"] =RcptResvRow[i]["ChosNo"].ToString().Trim(); 
            //
            //					tempDeptCd = currentDeptCd; 
            //					RcptCount = 0; 
            //				}
            //				else
            //				{
            //					RcptCount++; 
            //
            //					_reservationRow["RcptGb"] = RcptCount; 
            //					_reservationRow["ChosNo"] = RcptResvRow[i]["ChosNo"].ToString().Trim(); 
            //					_reservationRow["ResvChosNo"] = RcptResvRow[i]["ChosNo"].ToString().Trim(); 
            //				}
            //
            //				_reservation.Rows.Add(_reservationRow); 
            //			}
            //
            //			_reservation.AcceptChanges(); 
            //
            //			// 추가한 Row를 삭제한다. 
            //			_dt.Rows.Remove(_dt.Select("ChosNo ='-'")[0]); 

            #endregion


            bool CheckRcpt = false;

            // #1. _dt의 Row 수만큼 돌리면서 내원 번호를 뽑아낸다. 
            // #2. 내원번호 별로 진료비 계산을 돌리면서, 진료비를 산정한다. 
            // 내원번호 별로 계산된 데이터를 ordDs에 Merge 한다. 

            for (int i = 0; i < _dt.Rows.Count; i++)
            {
                aRow = _dt.Rows[i];

                strChosNo = aRow["ChosNo"].ToString().Trim(); //내원번호
                strClnDeptCd = aRow["ClnDeptCd"].ToString().Trim(); //부서코드

                // 진료비 계산을 통해 내원번호 별로 OrdDs를 뽑아온다.
                // [2005.12.08] 기존 카드 리스트를 얻어온다
                _FeeList = SelectOrdByChosNo(strChosNo, strClnDeptCd, hosCd, unitNo, DateTime.Now.ToString("yyyyMMdd"), "M");

                // IFUtil을 통해, 예약비가 0원 수납은 아니면서, 1000원 미만인 데이터를 죽여버린다? 
                // IFUtil.ExceptAmbAcpt( ref _FeeList ); 

                // 카드. 현금 영수증 테이블을 생성한다.
                CreateCrdCashDt(ref _FeeList, hosCd);

                CheckRcpt = false;

                #region

                //				if ( _reservation.Select(" ChosNo ='" + strChosNo+"'")[0]["RcptGb"].ToString().Trim() =="0")
                //					CheckRcpt = true; 
                #endregion

                // _FeeList 에는 예약건 까지 계산되는 경우가 있을 수 있다.
                // 예약건을 필터링해서, 예약건의 Dt를 담아서 리턴한다. 
                // 내원번호 하나에 대한 예약건은 별도의 Dt에 담는다 
                _FeeList = AcptOrdByChosNo(_FeeList, strChosNo, CheckRcpt);

                // 접수 건에 대해 예약건을 넣는다. 
                #region 주석처리
                //			_reservation.Select(" ChosNo ='" + strChosNo+"'")[0]["ResvChosNo"] = _FeeList.Tables["ResvBillDT"].Rows[0]["ResvChosNo"].ToString(); 
                //			_FeeList.Tables.Remove("ResvBillDT"); 
                #endregion

                // 내원번호 별로 하나로 뭉치자... 
                ordDs.Merge(_FeeList);
            }

            // 뽑아온 진료비에 대해 내셔야 할 금액을 계산한다. 
            ordDs = CalcFeeAmt(ordDs, strChosNo, 0);	// 무인수납기에서 송금액은 0 원이다. 


            // ordDs 의 "Ord" Table 안의 ChosNo 와 _reservation 의 ChosNo 를 돌려서 누락된 ChosNo 만큼을 
            // 새로운 접수 건으로 만들어서 _reservation 에 담는다. 
            // CheckWTHOUTChosNoInReservDt( ref ordDs , ref new DataTable() ); 


            //	접수건에 대한 예약건은 삭제를 한다. 
            //	위의 For문을 돌면서, _reservation 는 모든 케이스에 대해 [ 접수건 - 예약건 ] 의 모습을 가지고 있다. 
            //	이 중에서 접수 건과 예약건이 틀린 케이스를 뽑아서, 
            //	그 케이스의 예약건의 내원번호를 가지고 있는 접수건을 삭제한다. 

            #region 주석 처리

            // 삭제 여부를 마킹할 Column을 삽입한다. 
            //			_reservation.Columns.Add("DelYn" , typeof(string));
            //			DataRow[] CsResvRow = _reservation.Select(" ChosNo <> ResvChosNo ");			// --> 접수 건과 예약건이 틀린 케이스를 뽑아낸다. 
            //
            //			for ( int i_CsResvRow =0; i_CsResvRow < CsResvRow.Length ; i_CsResvRow++ )
            //			{
            //				for ( int i_reserv=0; i_reserv < _reservation.Rows.Count ; i_reserv++)			
            //				{
            //					// 접수 건과 예약건의 틀린 케이스의 예약건과 
            //					// 같은 접수 건을 가지고 있고 &&
            //					// 그 건은 접수건과 예약건이 같다면....
            //					if ( CsResvRow[i_CsResvRow]["ResvChosNo"].ToString().Trim() ==
            //							_reservation.Rows[i_reserv]["ChosNo"].ToString() 
            //						&&
            //							_reservation.Rows[i_reserv]["ChosNo"] == _reservation.Rows[i_reserv]["ResvChosNo"]	
            //					  )
            //					{
            //						// Marking을 한다. 
            //						_reservation.Rows[i_reserv]["DelYn" ] = "Y"; 
            //					}
            //				}
            //			}
            //            
            //			DataRow[] DeleteResvRow = _reservation.Select(" DelYn ='Y' "); 
            //
            //			for ( int deleteindex =0; deleteindex < DeleteResvRow.Length ; deleteindex++)
            //			{
            //				_reservation.Rows.Remove(DeleteResvRow[deleteindex]); 
            //			}
            //			
            //			_reservation.AcceptChanges();
            //
            //			_reservation.AcceptChanges(); 
            //			ordDs.Merge(_reservation); 

            #endregion

            //HP_KIOSKOrd의 DscRsvYn 에 들어가는 값
            // 입원 수납 시 퇴원예약일 경우에는 'Y'
            // 외래 수납 시 진료비 수납일 경우에는 'N'
            // 외래 수납 시 예약비 수납일 경우에는 'R'
            if (!ordDs.Tables["Ord"].Columns.Contains("RsvYn"))
            {
                ordDs.Tables["Ord"].Columns.Add("RsvYn", typeof(string));
            }

            //HIS.Facade.HP.ZZZ.ZZZFacade _ZZZFacade = new HIS.Facade.HP.ZZZ.ZZZFacade();
            DataTable mappingDt = _ZZZFacade.setRcptRscMapping(ordDs);

            foreach (DataRow RRow in ordDs.Tables["Ord"].Rows)
            {
                foreach (DataRow mappingRow in mappingDt.Rows)
                {
                    if (RRow["ChosNo"].ToString().Trim() == mappingRow["ChosNo1"].ToString().Trim())
                    {
                        if (mappingRow["MAP"].ToString().Trim() == "1")
                        {
                            RRow["RsvYn"] = "R";
                        }

                        if (mappingRow["MAP"].ToString().Trim() == "0")
                        {
                            RRow["RsvYn"] = "N";
                        }

                        break;
                    }
                }
            }

            ordDs.Merge(mappingDt);
            return ordDs;
        }

        public DataTable GetPaymentOPD(string hosCd, string unitNo, string chosGb, string chosNo, string deptCd, string ClnYmd, string acptYmd, string proxyYn)
        {
            DataTable resultDt;

            string PlnExecYmdUseYn = FnPlnExecYmdUseYn(hosCd, unitNo, "A", "HOLD");

            if (PlnExecYmdUseYn == "Y")
            {
                resultDt = getPaymentOPD_1(hosCd, unitNo, chosGb, chosNo, deptCd, ClnYmd, acptYmd, proxyYn, "M");
                //_OrdDs = ds;
                //return ds;
            }
            else
            {
                //    //[16.11.09]DataSet ds=SelectRcptContByChosNo( pUnitNo, "" , "", HosCd, "A",  pUserid);
                //모바일에선 K가 아니라 M으로 
                resultDt = getPaymentOPD_1(hosCd, unitNo, chosGb, chosNo, deptCd, ClnYmd, acptYmd, proxyYn, "K"); //2016.11.09 이용주 무인수납기에서 예약비 수납못하게 @strGb가 strGb 파라미터 A ->K 로 변경
                //_OrdDs = ds;
                //return ds;
            }


            //모바일 UnitNo조회시  무조건 원복
            FnPlnExecYmdUseYn(hosCd, unitNo, "C", "");

            return resultDt;
        }
        public DataTable getPaymentOPD_1(string hosCd, string unitNo, string chosGb, string chosNo, string deptCd, string ClnYmd, string acptYmd, string proxyYn, string gb)
        {

            //string pUserid = "MOBILE";
            //if (pUserid.Length >= 8)
            //    pUserid = pUserid.Substring(0, 8);

            DataSet ds = null;
            DataSet _result = null;

            strHosCd = hosCd;		// 병원코드를 넣는다. 
            strUnitNo = unitNo;	// 등록번호를 입력 
            strUserid = MOBILE_USER;	// UserId

            DataTable Ord = getPaymentOPDTable();

            string strPatNm = "";
            int PharmCnt = 0;
            int OtherCnt = 0;


            try
            {
                //HIS.Facade.HP.FEE.FEEFacade facade = new HIS.Facade.HP.FEE.FEEFacade();

                // 등록번호별로 무인수납 불가대상을 선별한다. 


                #region ● KIOSK 에서 환자에 대한 수납 불가 여부를 조회한다.

                Hashtable pht = new Hashtable();
                pht.Add("TRNSGB", "N");
                pht.Add("SPGB", "KIOSK_GETPATINFO");
                pht.Add("UnitNo", unitNo);
                pht.Add("TermId", MOBILE_USER);

                if (CheckAcptPsbl)
                {
                    // 데이터 조회 : 강승숙 선생님의 SP 실행
                    DataSet AcptPsbl = _FEEFacade.HpEtcDs(pht);

                    errDt = KIOSKAcptPsbl(AcptPsbl, "KIOSKAcptPsbl", null);

                    if (errDt != null)
                    {
                        //DataSet returnDs = new DataSet();
                        //returnDs.Merge(errDt);
                        //return returnDs;

                        DataRow rOrd = Ord.NewRow();
                        rOrd["ErrorMsg"] = errDt.Rows[0]["ERROR_MSG"].ToString();
                        Ord.Rows.Add(rOrd);
                        Ord.AcceptChanges();
                        return Ord;
                    }

                    // 환자 정보에 대한 처리 
                    DataSet patInfoDs = _FEEFacade.SelectAcptContByChosNo(unitNo, hosCd);
                    if (patInfoDs.Tables.Contains("PatInfo") && patInfoDs.Tables["PatInfo"].Rows.Count > 0 && patInfoDs.Tables["PatInfo"].Rows[0]["PatNm"] != null)
                    {
                        strPatNm = patInfoDs.Tables["PatInfo"].Rows[0]["PatNm"].ToString();
                    }
                    errDt = KIOSKAcptPsbl(patInfoDs, "UnitNo", null);

                    if (errDt != null)
                    {
                        //DataSet returnDs = new DataSet();
                        //returnDs.Merge(errDt);
                        //return returnDs;

                        DataRow rOrd = Ord.NewRow();
                        rOrd["ErrorMsg"] = errDt.Rows[0]["ERROR_MSG"].ToString();
                        Ord.Rows.Add(rOrd);
                        Ord.AcceptChanges();
                        return Ord;
                    }
                }
                #endregion

                // 내원건을 뽑아오잒~
                ds = _FEEFacade.SelectRcptContByChosNo(unitNo, "", "", hosCd, gb);//MOBILE은 K->M 

                if (ds != null && ds.Tables.Count > 0 && ds.Tables.Contains("RcptCont") && ds.Tables["RcptCont"].Rows.Count > 0)
                {
                    // RH 과이면서, 진료일자가 오늘이 아닌 과는 모두 삭제한다. 
                    // 추후 내원사유( ChosResnCd ) 를 판단해서, 물리치료(E) 인 것만 골라내도록 한다. 
                    //	추후 반영 SP :	USP_HP_FEE_OP_AmbAcptDA_SelectRcptContByChosNoA
                    // 접수일자 가 오늘 이고 물리치료(E) 일경우 수납내역 표시
                    // 접수일자 오늘이 아니고 물리치료(E) 일경우 수납내역 표시 안함

                    //[11.10.13]신촌도 적용(SM-11-01425)if (IFUtil.IsYD())		/*IFUtil.GetLocationCd() == "10"*/	// 영동일 경우 
                    //[11.10.13]{
                    string toDayClnYmd = DateTime.Now.ToString("yyyyMMdd");		// ClnYmdDt

                    // [07.02.19]
                    // string selectedContent = "TRIM(ClnDeptCd) LIKE 'RH' "; 
                    string selectedContent = " TRIM(ChosResnCd) LIKE 'E' ";
                    selectedContent += "AND TRIM(ClnYmdDt) NOT LIKE '" + toDayClnYmd + "'";

                    DataRow[] deleteRow = ds.Tables["RcptCont"].Select(selectedContent);
                    if (deleteRow.Length > 0)
                    {
                        for (int i = 0; i < deleteRow.Length; i++)
                        {
                            ds.Tables["RcptCont"].Rows.Remove(deleteRow[i]);
                        }
                    }
                    //[11.10.13]}
                }

                ds.Tables["RcptCont"].AcceptChanges();
                ds.AcceptChanges();


                DataTable _dt = ds.Tables["RcptCont"];	//접수 내역만 얻어온다. 
                //내원번호다른것 날림
                DataRow[] _dr = ds.Tables["RcptCont"].Select(" ChosNo <>'" + chosNo + "'");
                if (_dr.Length > 0)
                {
                    for (int i = 0; i < ds.Tables["RcptCont"].Rows.Count; i++) ds.Tables["RcptCont"].Rows.Remove(_dr[i]);
                }

                // 내원번호 별로 진료비를 계산하자~
                _result = SetRcptFee(ref ds, unitNo, hosCd);

                pht.Clear();
                pht = null;

                pht = new Hashtable();
                pht.Add("UnitNo", unitNo);
                pht.Add("HosCd", hosCd);

                if (_result == null)
                {
                    //return null;
                    //20180105 계산결과없을경우
                    DataRow rOrd = Ord.NewRow();
                    rOrd["ErrorMsg"] = "계산 내역 조회 실패. 다시 진행 하세요.";
                    Ord.Rows.Add(rOrd);
                    Ord.AcceptChanges();
                    return Ord;
                }

                _result = _FEEFacade.ChkKIOSKExcept(_result);

                if (_result == null)
                {
                    //return null;
                    //20180105 계산결과없을경우
                    DataRow rOrd = Ord.NewRow();
                    rOrd["ErrorMsg"] = "계산 내역 조회 실패. 다시 진행 하세요.";
                    Ord.Rows.Add(rOrd);
                    Ord.AcceptChanges();
                    return Ord;
                }

                if (CheckAcptPsbl)
                {
                    errDt = KIOSKAcptPsbl(_result, "ChosNo", pht);

                    // KIOSK 불가 대상을 내원번호 별로 선별하자 
                    if (errDt != null)
                    {
                        // KIOSK 불가 대상일 경우 
                        //DataSet returnDs = new DataSet();
                        //returnDs.Merge(errDt);
                        //return returnDs;

                        DataRow rOrd = Ord.NewRow();
                        rOrd["ErrorMsg"] = errDt.Rows[0]["ERROR_MSG"].ToString();
                        Ord.Rows.Add(rOrd);
                        Ord.AcceptChanges();
                        return Ord;
                    }
                }
                //약처방관련
                if (_result.Tables.Contains("Ord" + chosNo))
                {
                    if (Convert.ToString(ds.Tables["RcptCont"].Rows[0]["RcptAmt"]) == "")
                    {
                        OtherCnt++;
                    }

                    for (int i = 0; i < _result.Tables["Ord" + chosNo].Rows.Count; i++)
                    {
                        if (_result.Tables["Ord" + chosNo].Rows[i]["AcptGb"].ToString() == "N"
                            || _result.Tables["Ord" + chosNo].Rows[i]["AcptGb"].ToString() == "H")
                        {
                            if (_result.Tables["Ord" + chosNo].Rows[i]["OrdTypCd"].ToString() == "P")
                                PharmCnt++;//약 처방건수
                        }
                        if (_result.Tables["Ord" + chosNo].Rows[i]["AcptGb"].ToString() == "N")
                        {
                            if (_result.Tables["Ord" + chosNo].Rows[i]["OrdTypCd"].ToString() != "P")
                                OtherCnt++;//그 외 처방건수
                        }
                    }
                }

                for (int i = 0; i < _result.Tables["Ord"].Rows.Count; i++)
                {
                    //일단은 외래?
                    DataRow rOrd = Ord.NewRow();

                    rOrd["HosCd"] = _result.Tables["Ord"].Rows[i]["HosCd"];
                    rOrd["UnitNo"] = unitNo;
                    rOrd["PatNm"] = strPatNm;
                    rOrd["ChosNo"] = _result.Tables["Ord"].Rows[i]["ChosNo"];
                    rOrd["ClnYmd"] = "";
                    rOrd["ClnHms"] = "";
                    rOrd["MCarePayAll"] = "Y";
                    rOrd["ChosGb"] = chosGb;
                    rOrd["PayAmt"] = _result.Tables["PayAmt"].Rows[0]["lPayAmt"];
                    rOrd["AddTaxAmt"] = _result.Tables["PayAmt"].Rows[0]["lblAddTaxAmt"];
                    rOrd["ErrorMsg"] = "";
                    rOrd["DisCalcYn"] = "";
                    rOrd["PharmCnt"] = Convert.ToString(PharmCnt);
                    rOrd["OtherCnt"] = Convert.ToString(OtherCnt);

                    Ord.Rows.Add(rOrd);
                    Ord.AcceptChanges();

                }

            }
            catch (Exception Ex)
            {
                DataRow rOrd = Ord.NewRow();
                rOrd["ErrorMsg"] = errDt.Rows[0]["ERROR_MSG"].ToString();
                Ord.Rows.Add(rOrd);
                Ord.AcceptChanges();
                return Ord;
            }

            return Ord;
        }

        public DataTable SaveAcptCalc(
              string hosCd
            , string unitNo
            , string chosGb
            , string chosNo
            , string clnYmd
            , string crdIssLoc
            , string crdNo
            , string crdTypGb
            , string vldThru
            , string instMcnt
            , string permAmt
            , string permYmd
            , string permHms
            , string permNo
            , string slpNo
            , string mbstNo
            , string inpurcCoNm
            , string crdData
            , string vanSeqNo
            , string crdPermMeth
            , string iCPermMeth
            , string pOSNo
            , string vanGb
            , string inpurcCd
            )
        {
            //    _cracker = null;
            //    _cracker = _p_cracker; 

            //에러시 로그기록을 위한 변수셋팅
            strHosCd = hosCd;
            strChosNo = chosNo;
            strUnitNo = unitNo;
            strClnYmd = clnYmd;
            strPermAmt = permAmt;
            strChosGb = chosGb;
            strLogDesc2 = hosCd + "|" + unitNo + "|" + chosGb + "|" + chosNo + "|" + clnYmd + "|" + crdIssLoc + "|" + crdNo
                + "|" + crdTypGb + "|" + vldThru + "|" + instMcnt + "|" + permAmt + "|" + permYmd + "|" + permHms
                + "|" + permNo + "|" + slpNo + "|" + mbstNo + "|" + inpurcCoNm + "|" + crdData + "|" + vanSeqNo
                + "|" + crdPermMeth + "|" + iCPermMeth + "|" + pOSNo + "|" + vanGb + "|" + inpurcCd;


            DataTable AcptResult = makeAcptResultDt();
            AcptResult.Rows[0]["HosCd"] = hosCd;
            DataSet OrdDs;

            //string crdUserId = userId;

            //if (userId.Length > 8)
            //    userId = userId.Substring(0, 8);
            //확인하기 12/11
            //내원번호와 유닛넘버로 다시 해당 내원건을 조회한다
            //OrdDs = SelectRcptContByChosNo(pUnitNo,pHosCd,pUserId,pChosNo);


            string PlnExecYmdUseYn = FnPlnExecYmdUseYn(hosCd, unitNo, "A", "HOLD");

            if (PlnExecYmdUseYn == "Y")
            {
                OrdDs = SelectRcptContByChosNo_MOB3(unitNo, hosCd, chosNo, "M");
                //_OrdDs = ds;
                //return ds;
            }
            else
            {
                //    //[16.11.09]DataSet ds=SelectRcptContByChosNo( pUnitNo, "" , "", HosCd, "A",  pUserid);
                //모바일에선 K가 아니라 M으로 
                OrdDs = SelectRcptContByChosNo_MOB3(unitNo, hosCd, chosNo, "K");
                //_OrdDs = ds;
                //return ds;
            }






            //OrdDs = SelectRcptContByChosNo_MOB3(unitNo, hosCd, chosNo);
            if (OrdDs.Tables.Contains("ERROR_MSG"))
            {
                //모바일 UnitNo조회시  무조건 원복
                FnPlnExecYmdUseYn(hosCd, unitNo, "C", "");

                AcptResult.Rows[0]["ErrorMsg"] = "수납조회시 오류발생";
                AcptResult.Rows[0]["ResultCd"] = "N";
                return AcptResult;
            }


            if (!Check_Ord_LstUpDt(ref OrdDs))
            {
                //모바일 UnitNo조회시  무조건 원복
                FnPlnExecYmdUseYn(hosCd, unitNo, "C", "");

                //return GetErrorDt("", "ERR_ACPTSAV", "수납도중처방변경", "C");
                AcptResult.Rows[0]["ErrorMsg"] = "수납도중처방변경";
                AcptResult.Rows[0]["ResultCd"] = "N";
                return AcptResult;
            }

            DataTable dt = null;
            Hashtable FeeNoHt = new Hashtable();

            //DataTable beforeCrdFiltering = null;
            //DataTable beforeCashFiltering = null;

            //Hashtable CrdHt = null;		// 카드 승인 요청 후 리턴
            //Hashtable CashHt = null;	// 현금 영수증 승인 요청 후 리턴

            // 현금 영수증 승인 요청 후 리턴 받는 카드 

            //DataTable CrdDt = null;		// 카드 Dt
            //DataTable CashDt = null;	// 현금영수증 Dt 

            //bool isExistCrd = false;
            //bool isExistCash = false;

            //grbCrdDt = null;	// 취소 내역을 보내기 위한 카드 Dt 
            //grdCashDt = null;	// 취소 내역을 보내기 위한 현금 영수증 Dt

            //boolCashAdmit = false;	// 현금 영수증 승인 여부 ( 승인 시 TRUE ) 
            //boolCrdAdmit = false;   // 카드 승인 여부 ( 승인 시 TRUE ) 



            try
            {
                AcptGbSet(ref OrdDs); //AcptGb 는 2
                DataRow crdRow = OrdDs.Tables["HF_CrdIF"].NewRow();


                /*
                crdRow["HosCd"] = hosCd;
                crdRow["AccCd"] = GetAccCd(inpurcCd);
                crdRow["InpurcCd"] = "008";
                crdRow["CrdIssLoc"] = crdIssLoc;
                crdRow["MemNm"] = "DDC";
                crdRow["CrdNo"] = crdNo;
                crdRow["VldThru"] = vldThru;
                crdRow["InstMcnt"] = instMcnt;
                crdRow["PermAmt"] = Convert.ToInt32(permAmt.Trim());
                crdRow["PermYmd"] = DateTime.Now.ToString("yyyyMMdd");
                crdRow["PermHms"] = DateTime.Now.ToString("hhmmss");
                crdRow["PermNo"] = permNo;//승인번호
                crdRow["VanGb"] = vanGb;
                crdRow["PermStusGb"] = "P"; // 신규 승인
                crdRow["SlpNo"] = slpNo;  // 거래일련번호
                crdRow["MbstNo"] = mbstNo;  // 가맹점범호 
                crdRow["InpurcCoNm"] = inpurcCoNm;  // 매입사명
                crdRow["InsMeth"] = "2";	// 카드리더기   ??????????????????
                crdRow["CrdData"] = crdData;//CrdData;
                crdRow["CnYn"] = "N";  // 카드 취소 여부 
                crdRow["AcptCnYn"] = "N"; // 수납 취소 여부 
                crdRow["RgtId"] = "MOBILE"; // 등록자 id
                crdRow["LstUpdId"] = "MOBILE"; // 등록자 id
                crdRow["RgtDt"] = System.DateTime.Now;
                crdRow["LstUpdDt"] = System.DateTime.Now;
                crdRow["CheckedCancel"] = false;	// 취소 아님 모두 승인 
                crdRow["SubYn"] = "N";  // 계정 처리 
                crdRow["ChosNo"] = chosNo; // 내원번호
                crdRow["PermGb"] = "N"; // 신규 승인
                crdRow["DeptCd"] = ""; // 부서
                crdRow["ChosGb"] = "O";
                crdRow["VanSeqNo"] = vanSeqNo; //가라
                crdRow["CrdPermMeth"] = "MO";
                crdRow["ICPermMeth"] = "MO";
                crdRow["POSNo"] = "";//빈값고정
                crdRow["CrdTypGb"] = CrdTypGb; //카드타입
                */
                //가라
                crdRow["HosCd"] = hosCd;
                crdRow["AccCd"] = GetAccCd(inpurcCd);
                crdRow["InpurcCd"] = inpurcCd;
                crdRow["CrdIssLoc"] = crdIssLoc;
                crdRow["MemNm"] = "DDC";
                crdRow["CrdNo"] = crdNo;
                crdRow["VldThru"] = vldThru;
                crdRow["InstMcnt"] = instMcnt;
                crdRow["PermAmt"] = Convert.ToInt32(permAmt.Trim());
                crdRow["PermYmd"] = DateTime.Now.ToString("yyyyMMdd");
                crdRow["PermHms"] = DateTime.Now.ToString("hhmmss");
                crdRow["PermNo"] = permNo;
                //crdRow["CnBfCrdIFNo"] = //승인번호
                crdRow["VanGb"] = vanGb;
                crdRow["PermStusGb"] = "P"; // 신규 승인
                crdRow["SlpNo"] = slpNo; // 거래일련번호
                crdRow["MbstNo"] = mbstNo;  // 가맹점번호
                crdRow["InpurcCoNm"] = inpurcCoNm;  // 매입사명

                /* TODO : YUMC_Change_Start(2009.10.30, By 장근주) GM-09-00325 */
                //무인수납기에서 후수납(오픈카드대상일경우)카드입력값을 키보드로 인식
                //crdRow["InsMeth"] = "2";	// 카드리더기 
                //if (CrdData.Length <= 25)
                //{
                //    crdRow["InsMeth"] = "1";	// 키보드(오픈카드,후수납) 
                //}
                //else
                //{
                crdRow["InsMeth"] = "2";	// 카드리더기   
                //}
                /* TODO : YUMC_Change_End(2009.10.30, By 장근주) GM-09-00325 */
                crdRow["CrdData"] = crdData;//CrdData;
                crdRow["CnYn"] = "N";  // 카드 취소 여부 
                //crdRow["CnYmd"] =  // 카드 취소 여부 
                //crdRow["CnHms"] =  // 카드 취소 여부 
                crdRow["AcptCnYn"] = "N"; // 수납 취소 여부 
                //crdRow["UcoltOccSeq"] = 
                crdRow["RgtId"] = MOBILE_USER; // 등록자 id
                crdRow["LstUpdId"] = MOBILE_USER; // 등록자 id
                crdRow["RgtDt"] = System.DateTime.Now;
                crdRow["LstUpdDt"] = System.DateTime.Now;
                crdRow["CheckedCancel"] = false;	// 취소 아님 모두 승인 
                //crdRow["DataFromCardReader"] =
                crdRow["SubYn"] = "N";  // 계정 처리 
                crdRow["ChosNo"] = chosNo; // 내원번호
                crdRow["PermGb"] = "N"; // 신규 승인
                crdRow["DeptCd"] = ""; // 부서
                crdRow["ChosGb"] = "O";
                //crdRow["SubCheck"] = 
                crdRow["VanSeqNo"] = vanSeqNo; //가라
                crdRow["CrdPermMeth"] = crdPermMeth;
                crdRow["ICPermMeth"] = iCPermMeth;
                crdRow["POSNo"] = "";//빈값고정
                crdRow["CrdTypGb"] = crdTypGb;//카드타입

                //OrdDs.Tables["HF_CrdIF"].Rows.Add(crdRow);

                // Ord에 셋팅한다. 
                //				DataRow OrdDr = AcptOrdDs.Tables["Ord"].Rows[0]; 
                //
                //				int iPayAmt = int.Parse(OrdDr["PayAmt"].ToString());
                //
                //				if ( iPayAmt == PermAmt)
                //				{
                OrdDs.Tables["HF_CrdIF"].Rows.Add(crdRow);
                OrdDs.AcceptChanges();

                //HIS.Facade.HP.FEE.FEEFacade facade = new HIS.Facade.HP.FEE.FEEFacade();
                //Payment payment = new Payment();

                if (OrdDs.Tables.Contains("HF_CrdIF") && OrdDs.Tables["HF_CrdIF"].Rows.Count > 0)
                {
                    //카드 수납 시 기존 금액에 대해, 신규 요청액을 합산한다. 
                    SetCardDtAndOrdByAcpt(ref OrdDs);

                }

                // [2006.05.24] 수납 시 필터링 
                CheckAmbAcptBefore(ref OrdDs);

                #region ◐ 금액 검증

                bool ambverific = AcptVerific(OrdDs, unitNo, MOBILE_USER);

                if (!ambverific)
                {
                    //return GetErrorDt("", "ERR_AMTVARIABLE", "금액검증오류", "B");
                    AcptResult.Rows[0]["ErrorMsg"] = "금액검증오류";
                    AcptResult.Rows[0]["ResultCd"] = "N";
                    return AcptResult;
                }

                #endregion

                #region 주석 처방을 한번 더 조회를 해서 지금 수납하려는 금액과의 차이를 확인한다.

                /*string strChosNo = OrdDs.Tables["Ord"].Rows[0]["ChosNo"].ToString().Trim(); 
				int iPayAmt = Convert.ToInt32( OrdDs.Tables["Ord"].Rows[0]["PayAmt"].ToString().Trim()  ); 

				if ( CheckReCalCompare )
				{
					#region [2006.11.13 박성용] ○ 수납 전 체크 사항에서 제외 ( 진료비를 다시 계산하지 않는다 )

//					if (! RcptReRetr( iPayAmt, pUnitNo, pHosCd, strChosNo, strUserid ) )
//					{
//						// 오류 처리를 한다
//						return  GetErrorDt( "", "ERR_GETORDRESELECT","처방재조회시오류", "B"); 
//					}
					#endregion 
				}
                */
                #endregion

                #region ◐ 카드 승인 요청



                // 카드 승인 요청 
                if (OrdDs.Tables.Contains("HF_CrdIF") && OrdDs.Tables["HF_CrdIF"].Rows.Count > 0)
                {
                    //beforeCrdFiltering = OrdDs.Tables["HF_CrdIF"].Copy();		//Exception 발생 시 원상 복구를 위해 
                    //isExistCrd = true;

                    // 내원번호는 클래스 안에서 
                    //_oCrdCach.prtAcptProc = "AmbAcpt";	// 외래수납
                    //_oCrdCach.prtApprovalCancelGb = "Approval"; // 승인
                    //_oCrdCach.prtChosNo = OrdDs.Tables["Ord"].Rows[0]["ChosNo"].ToString(); 
                    //_oCrdCach.prtUnitNo  = pUnitNo;
                    //_oCrdCach.prtCrdCashGb = "Card"; 	
                    //_oCrdCach.prtBilNo = ""; 
                    //_oCrdCach.strHosCd = pHosCd; 

                    //CrdHt = _oCrdCach.AmbCardApproval( OrdDs.Tables["HF_CrdIF"], 
                    //    pHosCd, pUnitNo, pDeptCd, pWindId, crdUserId, "AmbAcpt"
                    //    , ref _cracker 
                    //    ); 

                    //CrdDt = CrdHt["HF_CrdIF"] as DataTable;



                    // 카드 테이블을 설정한다. 
                    CrdAcptDt(ref OrdDs);
                    int iCount = 0;
                    // 카드 승인 시 채번의 조건 
                    // #1. 기존 승인 된 건에 대한 취소 건 [ 정산 Status ='Z' ]을 구하기 위해 : [PreData] = 'C' 
                    // #2. 새로운 신규 승인 요청 : [PreData] = 'N' 
                    for (int i = 0; i < OrdDs.Tables["HF_CrdIF"].Rows.Count; i++)
                    {
                        if (OrdDs.Tables["HF_CrdIF"].Rows[i]["PreData"].ToString().Trim() == "C" || OrdDs.Tables["HF_CrdIF"].Rows[i]["PreData"].ToString().Trim() == "N")
                        {
                            iCount++;
                        }
                    }
                    if (iCount > 0)
                    {
                        //카드IF번호 채번(신규카드 + 취소 카드만 채번한다)
                        // DataSet ifNoDS = GetIFNo(0, 1);
                        //OrdDs.Tables["HF_CrdIF"].Rows[0]["CrdIFNo"] = ifNoDS.Tables[0].Rows[0][0].ToString();

                        // IF번호 Setting
                        //MatchFeeNo(OrdDs.Tables["HF_CrdIF"], ifNoDS.Tables["CrdIF"], false);
                        //OrdDs.AcceptChanges();
                        //


                        //카드IF번호 채번(신규카드 + 취소 카드만 채번한다)
                        DataSet ifNoDS = GetIFNo(0, iCount);

                        // IF번호 Setting
                        MatchFeeNo(OrdDs.Tables["HF_CrdIF"], ifNoDS.Tables["CrdIF"], false);
                        //CrdDt.AcceptChanges();

                        // 카드 승인 요청을 한다. 
                        // 기존 카드의 취소 요청도 한다. 
                        //if (!ReqCrdAdmitCanel(pUnitNo, pDeptCd, ref CrdDt))
                        //{
                        //    CrdDt.RejectChanges();
                        //    CrdDt = beforeCrdFiltering; // 원래의 모습으로 

                        //    // 리턴 값 셋팅 "N" --> 오류 발생 수납을 중지하라
                        //    _reHt["Approval"] = "N";
                        //    _reHt["HF_CrdIF"] = null;	// 오류 발생 시에는 Null 을 보낸다. 
                        //    return _reHt;
                        //}



                    }
                    //OrdDs.Tables.Remove("HF_CrdIF");

                    //    // 승인을 얻었는지를 질의한다. 
                    //    if ( CrdHt["Approval"].ToString().Equals("Y") && CrdDt != null  && CrdDt.Rows.Count > 0  )	
                    //    {	
                    //        CrdDt.TableName = "HF_CrdIF"; 
                    //        OrdDs.Merge(CrdDt); 

                    //        boolCrdAdmit = true;	// 카드 승인이 났음을 알린다. 

                    //        if ( grbCrdDt != null )
                    //        {
                    //            grbCrdDt.Clear();
                    //        }

                    //        grbCrdDt = CrdDt.Clone();		// 카드 승인난 내역을 들고 있는다. 
                    //        DataRow[] admitCrdDr = CrdDt.Select(" PreData ='N' ");	// 승인 정보만 얻어온다. 

                    //        for ( int i=0; i < admitCrdDr.Length ; i++)
                    //        {
                    //            // 카드의 정보만 담는다.  
                    //            grbCrdDt.Rows.Add((object[]) admitCrdDr[i].ItemArray.Clone() ); 
                    //        } 
                    //    }
                    //    else if( CrdHt["Approval"].ToString().Equals("H") ){}
                    //    else
                    //    {
                    //        // 수납을 중지한다 NULL 반환
                    //        if (isExistCrd)
                    //        {
                    //            if( OrdDs.Tables.Contains("HF_CrdIF")){ OrdDs.Tables.Remove("HF_CrdIF");}
                    //            OrdDs.Merge(beforeCrdFiltering); 
                    //        }

                    //        if ( boolCrdAdmit & grbCrdDt != null )
                    //        {
                    //            _oCrdCach.prtAcptProc = "AmbAcpt";	// 외래수납
                    //            _oCrdCach.prtApprovalCancelGb = "Cancel"; // 승인
                    //            _oCrdCach.prtChosNo = OrdDs.Tables["Ord"].Rows[0]["ChosNo"].ToString(); 
                    //            _oCrdCach.prtUnitNo  = pUnitNo;
                    //            _oCrdCach.prtCrdCashGb = "Card"; 	
                    //            _oCrdCach.prtBilNo = ""; 
                    //            _oCrdCach.strHosCd = pHosCd; 

                    //            _oCrdCach.CancelAdmiCashDT( pUnitNo, pDeptCd, grbCrdDt ); 

                    //            _oCrdCach.LogWrite(); 
                    //            boolCrdAdmit = false; 
                    //        }

                    //        facade.AmbAcptLogWrite(OrdDs.Tables["Ord"].Rows[0]["ChosNo"].ToString(), pUnitNo, "AmbAcpt", pUserId, "O", "",  
                    //            "WinUI", "카드 승인 실패 LogGb ='AmbCrd', LogGb ='AmbCrdCn'으로 조회확인 " ); 

                    //        return  GetErrorDt( "", "ERR_PERMCRD","카드인증시오류", "B"); 
                    //    }
                    //}
                    //else if ( OrdDs.Tables.Contains("HF_CrdIF") && OrdDs.Tables["HF_CrdIF"].Rows.Count <= 0 )
                    //{
                    //    OrdDs.Tables.Remove("HF_CrdIF"); 
                    //}

                    #endregion
                }

                // 리턴 = 영수증 테이블 
                dt = _FEEFacade.SaveAcptCalc(OrdDs, hosCd, unitNo, MOBILE_WINDID, MOBILE_USER);

            }
            catch (Exception Ex)
            {
                #region Excpetion 처리

                /* 모바일에서는 필요없을듯 - 익셉션나면 새로 호출해야되니
                if (isExistCrd)
                {
                    if (OrdDs.Tables.Contains("HF_CrdIF")) { OrdDs.Tables.Remove("HF_CrdIF"); }
                    OrdDs.Merge(beforeCrdFiltering);
                }

                if (isExistCash)
                {
                    if (OrdDs.Tables.Contains("HF_CashIF")) { OrdDs.Tables.Remove("HF_CashIF"); }
                    OrdDs.Merge(beforeCashFiltering);
                }*/

                // 카드 승인 후, 수납 오류가 발생하면 ... 방금 승인을 한 내역에 대해 취소 처리를 한다. 
                //if (boolCrdAdmit & grbCrdDt != null )
                //{
                //    _oCrdCach.prtAcptProc = "AmbAcpt";	// 외래수납
                //    _oCrdCach.prtApprovalCancelGb = "Cancel"; // 승인
                //    _oCrdCach.prtChosNo = OrdDs.Tables["Ord"].Rows[0]["ChosNo"].ToString(); 
                //    _oCrdCach.prtUnitNo  = pUnitNo;
                //    _oCrdCach.prtCrdCashGb = "Card"; 	
                //    _oCrdCach.prtBilNo = ""; 

                //    _oCrdCach.CancelAdmiCrdDT( pUnitNo,pDeptCd, grbCrdDt); 
                //    // 수납 오류 시 Log를 기록한다. 
                //    _oCrdCach.LogWrite(); 
                //    boolCrdAdmit = false; 

                //    if ( grbCrdDt != null ) grbCrdDt.Clear(); 
                //    grbCrdDt = null; 
                //}



                //HIS.Facade.HP.FEE.FEEFacade _logFacade = new HIS.Facade.HP.FEE.FEEFacade();

                string[] err = Ex.Message.Trim().Split('§');

                // Biz 에서 발생 시 : 내원번호 + "§" + 로그 위치 + "§" + 에러 메시지 

                string logChosNo = string.Empty;
                string logLogDesn = string.Empty;
                string logLogMsg = string.Empty;
                string logLogMsg2 = string.Empty;
                string logLogMsg3 = string.Empty;

                bool isVerific = false;

                if (err.Length >= 3)	// 서버에서 오류 발생 
                {
                    logChosNo = err[0].Trim();
                    logLogDesn = err[1].Trim();
                    logLogMsg = err[2].Trim();

                    if (err.Length == 4)
                    {
                        logLogMsg2 = err[3].Trim();	// 에러 메시지 2 
                    }

                    if (err.Length == 5)
                    {
                        logLogMsg3 = err[4].Trim();	// 에러 메시지 3 
                    }

                    // 서버 오류 시 외래 금액 검증 용인지를 확인한다. 
                    if (logLogMsg.IndexOf("HP_Verific") > 1)
                    {
                        //모바일에서 안해도될듯
                        AcptVerificFiltering(logChosNo, logLogDesn, logLogMsg, unitNo, MOBILE_USER);
                        isVerific = true;
                    }
                }
                else	// 클라이언트에서 발생  
                {
                    logChosNo = OrdDs.Tables["Ord"].Rows.Count > 0 ? OrdDs.Tables["Ord"].Rows[0]["ChosNo"].ToString() : "";
                    logLogDesn = "MOBILE";//20180104
                    logLogMsg = Ex.Message;
                }


                if (!isVerific)
                {
                    _FEEFacade.AmbAcptLogWrite(logChosNo, unitNo, "AmbAcpt", MOBILE_USER, "O", "", logLogDesn, logLogMsg);

                    _FEEFacade.LogWrite(logChosNo, unitNo, "AmbAcpt", DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.ToString("yyyyMMdd"),
                        logLogDesn, logLogMsg, MOBILE_USER, "O", "", logLogMsg2, logLogMsg3);

                }

                #endregion

                FnPlnExecYmdUseYn(hosCd, unitNo, "C", "");

                //return GetErrorDt("", "ERR_ACPTSAV", "수납시오류발생", "B");
                AcptResult.Rows[0]["ErrorMsg"] = "수납시오류발생";
                AcptResult.Rows[0]["ResultCd"] = "N";
                return AcptResult;

            }// CatCh 

            //if (dt == null || dt.Rows.Count <= 0)
            //{
            //    //return GetErrorDt("", "ERR_ACPTSAV", "수납시오류발생", "B");
            //    AcptResult.Rows[0]["ErrorMsg"] = "수납시오류발생";
            //    AcptResult.Rows[0]["ResultCd"] = "N";
            //    return AcptResult;
            //}

            if (dt != null && dt.Rows.Count > 0)
            {
                AcptResult.Rows[0]["BillNo"] = dt.Rows[0]["BillNo"].ToString();
                AcptResult.Rows[0]["ResultCd"] = "Y";
                AcptResult.Rows[0]["HosoOrdNo"] = dt.Rows[0]["HosoOrdNo"].ToString();
                AcptResult.Rows[0]["ErrorMsg"] = "";
            }
            else
            {
                AcptResult.Rows[0]["ErrorMsg"] = "수납 처리 실패 하였습니다.";
                AcptResult.Rows[0]["ResultCd"] = "N";
            }

            //이것도 모바일에서 필요없음.
            // 성공을 했을 경우이므로, 카드 및 현금 영수증을 백업 받은 데이터를 초기화 한다. 
            //boolCashAdmit = false;
            //boolCrdAdmit = false;
            //if (grbCrdDt != null) grbCrdDt.Clear();
            //if (grdCashDt != null) grdCashDt.Clear();

            //모바일 UnitNo조회시  무조건 원복
            FnPlnExecYmdUseYn(hosCd, unitNo, "C", "");

            return AcptResult;
        }

        public DataSet SelectRcptContByChosNo_MOB3(string pUnitNo, string pHosCd, string pChosNo, string gb)
        {
            //string pUserid = "MOBILE";
            //if (pUserid.Length >= 8)
            //    pUserid = pUserid.Substring(0, 8);

            DataSet ds = null;
            DataSet _result = null;

            //strHosCd = pHosCd;		// 병원코드를 넣는다. 
            //strUnitNo = pUnitNo;	// 등록번호를 입력 
            //strUserid = MOBILE_USER;	// UserId

            try
            {
                //HIS.Facade.HP.FEE.FEEFacade facade = new HIS.Facade.HP.FEE.FEEFacade();

                // 등록번호별로 무인수납 불가대상을 선별한다. 


                #region ● KIOSK 에서 환자에 대한 수납 불가 여부를 조회한다.

                Hashtable pht = new Hashtable();
                pht.Add("TRNSGB", "N");
                pht.Add("SPGB", "KIOSK_GETPATINFO");
                pht.Add("UnitNo", pUnitNo);
                pht.Add("TermId", MOBILE_USER);

                if (CheckAcptPsbl)
                {
                    // 데이터 조회 : 강승숙 선생님의 SP 실행
                    DataSet AcptPsbl = _FEEFacade.HpEtcDs(pht);

                    errDt = KIOSKAcptPsbl(AcptPsbl, "KIOSKAcptPsbl", null);

                    if (errDt != null)
                    {
                        DataSet returnDs = new DataSet();
                        returnDs.Merge(errDt);
                        return returnDs;
                    }

                    // 환자 정보에 대한 처리 
                    DataSet patInfoDs = _FEEFacade.SelectAcptContByChosNo(pUnitNo, pHosCd);

                    errDt = KIOSKAcptPsbl(patInfoDs, "UnitNo", null);

                    if (errDt != null)
                    {
                        DataSet returnDs = new DataSet();
                        returnDs.Merge(errDt);
                        return returnDs;
                    }
                }

                #endregion


                // 내원건을 뽑아오잒~
                ds = _FEEFacade.SelectRcptContByChosNo(pUnitNo, "", "", pHosCd, gb);//시행예정일 파라미터 변수로 전달

                if (ds != null && ds.Tables.Count > 0 && ds.Tables.Contains("RcptCont") && ds.Tables["RcptCont"].Rows.Count > 0)
                {
                    // RH 과이면서, 진료일자가 오늘이 아닌 과는 모두 삭제한다. 
                    // 추후 내원사유( ChosResnCd ) 를 판단해서, 물리치료(E) 인 것만 골라내도록 한다. 
                    //	추후 반영 SP :	USP_HP_FEE_OP_AmbAcptDA_SelectRcptContByChosNoA
                    // 접수일자 가 오늘 이고 물리치료(E) 일경우 수납내역 표시
                    // 접수일자 오늘이 아니고 물리치료(E) 일경우 수납내역 표시 안함

                    //[11.10.13]신촌도 적용(SM-11-01425)if (IFUtil.IsYD())		/*IFUtil.GetLocationCd() == "10"*/	// 영동일 경우 
                    //[11.10.13]{
                    string toDayClnYmd = DateTime.Now.ToString("yyyyMMdd");		// ClnYmdDt

                    // [07.02.19]
                    // string selectedContent = "TRIM(ClnDeptCd) LIKE 'RH' "; 
                    string selectedContent = " TRIM(ChosResnCd) LIKE 'E' ";
                    selectedContent += "AND TRIM(ClnYmdDt) NOT LIKE '" + toDayClnYmd + "'";

                    DataRow[] deleteRow = ds.Tables["RcptCont"].Select(selectedContent);
                    if (deleteRow.Length > 0)
                    {
                        for (int i = 0; i < deleteRow.Length; i++)
                        {
                            ds.Tables["RcptCont"].Rows.Remove(deleteRow[i]);
                        }
                    }
                    //[11.10.13]}
                }

                ds.Tables["RcptCont"].AcceptChanges();
                ds.AcceptChanges();
                DataTable _dt = ds.Tables["RcptCont"];	//접수 내역만 얻어온다.

                //내원번호다른것 날림
                DataRow[] _dr = ds.Tables["RcptCont"].Select(" ChosNo <>'" + pChosNo + "'");
                if (_dr.Length > 0)
                {
                    for (int i = 0; i < ds.Tables["RcptCont"].Rows.Count; i++) ds.Tables["RcptCont"].Rows.Remove(_dr[i]);
                }
                // 내원번호 별로 진료비를 계산하자~
                _result = SetRcptFee(ref ds, pUnitNo, pHosCd);

                pht.Clear();
                pht = null;

                pht = new Hashtable();
                pht.Add("UnitNo", pUnitNo);
                pht.Add("HosCd", pHosCd);

                if (_result == null) return null;

                _result = _FEEFacade.ChkKIOSKExcept(_result);

                if (_result == null) return null;

                if (CheckAcptPsbl)
                {
                    errDt = KIOSKAcptPsbl(_result, "ChosNo", pht);

                    // KIOSK 불가 대상을 내원번호 별로 선별하자 
                    if (errDt != null)
                    {
                        // KIOSK 불가 대상일 경우 
                        DataSet returnDs = new DataSet();
                        returnDs.Merge(errDt);
                        return returnDs;
                    }
                }
            }
            catch (Exception Ex)
            {
                DataSet returnDs = new DataSet();
                returnDs.Merge(GetErrorDt("", "ERR_GETRCPTINFO", "접수내역확인", "B"));
                return returnDs;
            }

            // 이상이 없을 경우
            // 혹시 ERROR_MSG 가 있으면 삭제한다. 
            if (_result.Tables.Contains("ERROR_MSG"))
                _result.Tables.Remove("ERROR_MSG");

            return _result;
        }

        private DataSet CalcFeeAmt(DataSet ordDs, string chosNo, int remitAmtv)
        {
            try
            {
                return _FEEFacade.GetAmbAcptPayAmt(ordDs, chosNo, remitAmtv);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DataSet AcptOrdByChosNo(DataSet ordDs, string chosNo, bool checkRcpt)
        {
            DataSet OrdDsChosNo = new DataSet();    // 리턴을 할, 실제 수납에 저장을 할 데이터 셋

            // 예약건 Dt 
            //			DataTable _reservation = new DataTable("ResvBillDT"); 
            //			_reservation.Columns.Add("ChosNo", typeof(string )) ;		// 접수 건
            //			_reservation.Columns.Add("ResvChosNo", typeof(string )) ;	// 예약 건



            try
            {
                DataRow[] aRow;

                // #.1. Ord 			
                DataTable ord = ordDs.Tables["Ord"].Clone();
                aRow = ordDs.Tables["Ord"].Select(" ChosNo ='" + chosNo + "'");
                ord.Rows.Add((object[])aRow[0].ItemArray.Clone());
                ord.TableName = "Ord";

                // #.2. 처방 테이블을 뽑아낸다. Ord + 내원번호
                DataTable ordChosNo = ordDs.Tables["Ord" + chosNo].Copy();
                ordChosNo.TableName = "Ord" + chosNo;

                // #.3. 미수 테이블을 뽑아낸다 Ucolt + 내원번호 
                DataTable ucoltDt = ordDs.Tables["UcoltDT" + chosNo].Copy();
                ucoltDt.TableName = "UcoltDT" + chosNo;

                // #.4. 감액 테이블을 뽑아낸다 RduDT + 내원번호 
                DataTable rduDt = ordDs.Tables["RduDT" + chosNo].Copy();
                rduDt.TableName = "RduDT" + chosNo;

                // #.5. 카드 테이블을 뽑아낸다(내원번호 별로 기존의 수납 내역을 가지고 있다.)
                DataTable crdIF = ordDs.Tables["HF_CrdIF"].Copy();

                // #.6. 현금 영수증테이블을 뽑아낸다 (기본값은 빈값이다.)
                // 차후 현금 영수증 적용 시 CreateCrdCashDt 에서 셋팅을 하고 여기에서는 Copy()를 해야 한다. 
                DataTable cashIF = ordDs.Tables["HF_CashIF"].Clone();

                // #7. 접수건과 예약건을 담는다. 
                // pChosNo 와 틀린 것 중 가장 최근의 값을 뽑아낸다. 
                #region 주석 처리

                //				DataRow[] resvRow = pOrdDs.Tables["Ord"].Select("", "ClnYmd"); 
                //
                //				DataRow resvRow02 = _reservation.NewRow();	
                //
                //				if ( CheckRcpt )
                //				{
                //					_reservation.Rows.Add(resvRow02);	// 일단 저장하고...
                //
                //					for ( int i=0; i < resvRow.Length ; i++)
                //					{	
                //						#region <●※●> 접수 건 / 내원 건 판별
                // 
                //						// 접수 건이면서 
                //						// 내원번호가 틀릴 경우 [ 예약건이 존재시 ]
                //						if ( resvRow[i]["ChosNo"].ToString().Trim() != pChosNo )
                //						{
                //							resvRow02["ChosNo"] = pChosNo;		// 고정 값
                //							resvRow02["ResvChosNo"] = resvRow[i]["ChosNo"].ToString().Trim();	// Looping을 돌면서, 차근차근... 							 
                //
                //							iOrdCnt = pOrdDs.Tables["Ord"+resvRow02["ResvChosNo"].ToString().Trim()].Select( " AutoAdGb = '' ").Length; 
                //
                //							// 예약 건에 처방 수가 0개 이상일 경우 
                //							if ( iOrdCnt > 0 )
                //							{
                //								resvRow02["ResvChosNo"] = resvRow02["ChosNo"];		// 예약건이 접수 건이다. 
                //							}
                //
                //							if ( resvRow02["ResvChosNo"] != resvRow02["ChosNo"] ) break; // --> 접수 건과 예약건이 틀릴 경우, 1건만 만들고 빠져 나온다. 
                //						}
                //		
                //						
                //
                //						iOrdCnt = 0; 
                //						#endregion 
                //					}
                //				}
                //				else
                //				{
                //					// 접수건이 아닐 경우에는 접수건의 내원번호와 예약건의 내원번호가 동일하다.
                //					resvRow02["ChosNo"] = pChosNo ; 
                //					resvRow02["ResvChosNo"] = pChosNo;
                //					_reservation.Rows.Add(resvRow02); 
                //				}
                //
                //
                //				// 만일 예약건이 없을 경우 
                //				if ( _reservation.Rows.Count < 1 )
                //				{
                //					resvRow02["ChosNo"] = pChosNo ; 
                //					resvRow02["ResvChosNo"] = pChosNo;
                //					_reservation.Rows.Add(resvRow02); 
                //				}

                #endregion

                OrdDsChosNo.Merge(ord);
                OrdDsChosNo.Merge(ordChosNo);
                OrdDsChosNo.Merge(ucoltDt);
                OrdDsChosNo.Merge(rduDt);
                OrdDsChosNo.Merge(crdIF);
                OrdDsChosNo.Merge(cashIF);

                // OrdDsChosNo.Merge(_reservation);
            }
            catch (Exception ex)
            {
                DataSet ds = new DataSet();
                ds.Merge(GetErrorDt("", "ERR_ACPTSAV", "수납시오류", "B"));
                return ds;	// 초기화
            }

            return OrdDsChosNo;
        }

        #region ▣	외래 수납  #3.2	 ♣ AcptGb 변경
        /// <summary>
        /// 수납 시 필요한 AcptGb를 셋팅한다. 
        /// </summary>
        /// <param name="OrdDs"></param>
        public void AcptGbSet(ref DataSet OrdDs)
        {
            // 이 부분은 추후 수정 계획
            OrdDs.Tables["Ord"].Rows[0]["AcptGb"] = "2";

            OrdDs.AcceptChanges();
        }

        #endregion 

        private DataTable GetErrorDt(string errchosNo, string errcode, string errvalue, string deeth)
        {
            errDt = MakeErrorDt();

            errDt.Rows[0]["ChosNo"] = errchosNo;
            errDt.Rows[0]["ERROR_CODE"] = errcode;
            errDt.Rows[0]["ERROR_MSG"] = errvalue;
            errDt.Rows[0]["DEEPTH"] = deeth;

            errDt.AcceptChanges();
            return errDt;
        }

        private DataTable MakeErrorDt()
        {
            DataTable _dt = new DataTable("ERROR_MSG");

            _dt.Columns.Add("ChosNo", typeof(string));		// 내원번호
            _dt.Columns.Add("ERROR_CODE", typeof(string));	// 에러 코드 
            _dt.Columns.Add("ERROR_MSG", typeof(string));	// 에러 메시지 
            _dt.Columns.Add("DEEPTH", typeof(string));		// 정도 

            DataRow errRow = _dt.NewRow();
            _dt.Rows.Add(errRow);

            return _dt;
        }

        private void CreateCrdCashDt(ref DataSet ordDs, string hosCd)
        {
            DataTable cardif = HF_CrdIF();
            // 카드 Dt를 만든다 
            if (!ordDs.Tables.Contains("HF_CrdIF"))
            {
                // 임시 카드 Dt [ TempCrdDt ]의 존재 여부를 확인해서 없으면 그냥 빈 테이블인 cardif를 Merge 하고 
                // 있으면 cardif 에 넣고(복사)  OrdDs 내의 [TempCrdDt] 는 삭제한다. 
                if (ordDs.Tables.Contains("TempCrdDt"))
                {
                    DataTable tempCrdIF = ordDs.Tables["TempCrdDt"].Copy();
                    ordDs.Tables.Remove("TempCrdDt");

                    DataRow cardifRow = null;

                    foreach (DataRow crdRow in tempCrdIF.Rows)
                    {
                        // tempCrdIF 의 데이터를 cardif 에 넣는다.
                        cardifRow = cardif.NewRow();

                        for (int i = 0; i < cardif.Columns.Count; i++)
                        {
                            if (tempCrdIF.Columns.Contains(cardif.Columns[i].ColumnName))
                            {
                                // tempCrdIF 의 새 Row  <-- cardif 의 Row // crdRow
                                cardifRow[cardif.Columns[i].ColumnName] = crdRow[cardif.Columns[i].ColumnName];
                            }
                        }

                        // 잔여 컬럼을 셋팅한다. 
                        // tempCrdIF [crdRow] 의 데이터를 cardif [cardifRow] 에 넣는다.

                        cardifRow["DeptCd"] = crdRow["ClnDeptCd"];
                        cardifRow["ChosGb"] = "O"; // 나중에 입원은?
                        cardifRow["CnYn"] = crdRow["HF_CrdIF_CnYn"];
                        cardifRow["SubYn"] = crdRow["TretYn"];
                        cardifRow["DataFromCardReader"] = crdRow["CrdData"];
                        cardifRow["CheckedCancel"] = 0;	// 무인수납기에서 취소는 없다. 
                        cardifRow["HosCd"] = hosCd;

                        cardif.Rows.Add(cardifRow);
                    }
                }

                cardif.TableName = "HF_CrdIF";
                ordDs.Merge(cardif);
            }


            // 현금영수증 Dt를 만든다. 
            // [ 2005.12.08 : 현금 영수증은 임시 보류 한다 ]
            if (!ordDs.Tables.Contains("HF_CashIF"))
            {
                ordDs.Merge(HF_CashIF());
            }
        }

        private DataTable HF_CashIF()
        {
            DataTable _dt = new DataTable("HF_CashIF");

            _dt.Columns.Add("ChosNo", typeof(System.String));
            _dt.Columns.Add("ItemTag", typeof(System.String));
            _dt.Columns.Add("HosCd", typeof(System.String));
            _dt.Columns.Add("CashBilIFNo", typeof(System.String));
            _dt.Columns.Add("InpurcCd", typeof(System.String));
            _dt.Columns.Add("IssLoc", typeof(System.String));
            _dt.Columns.Add("SlfYn", typeof(System.String));
            _dt.Columns.Add("InsNoGb", typeof(System.String));
            _dt.Columns.Add("InsNo", typeof(System.String));
            _dt.Columns.Add("PermAmt", typeof(System.String));
            _dt.Columns.Add("PermYmd", typeof(System.String));
            _dt.Columns.Add("PermHMS", typeof(System.String));
            _dt.Columns.Add("PermNo", typeof(System.String));
            _dt.Columns.Add("SlpNo", typeof(System.String));
            _dt.Columns.Add("InsMeth", typeof(System.String));
            _dt.Columns.Add("InsData", typeof(System.String));
            _dt.Columns.Add("CnYn", typeof(System.String));
            _dt.Columns.Add("CnPermNo", typeof(System.String));
            _dt.Columns.Add("CnYmd", typeof(System.String));
            _dt.Columns.Add("CnHms", typeof(System.String));
            _dt.Columns.Add("RgtId", typeof(System.String));
            _dt.Columns.Add("VanGb", typeof(System.String));

            return _dt;
        }

        private DataTable HF_CrdIF()
        {
            DataTable _dt = new DataTable("HF_CrdIF");

            _dt.Columns.Add("CrdIFNo", typeof(System.Int32));
            _dt.Columns.Add("HosCd", typeof(System.String));
            _dt.Columns.Add("AccCd", typeof(System.String));
            _dt.Columns.Add("InpurcCd", typeof(System.String));
            _dt.Columns.Add("CrdIssLoc", typeof(System.String));
            _dt.Columns.Add("CrdNo", typeof(System.String));
            _dt.Columns.Add("MemNm", typeof(System.String));
            _dt.Columns.Add("VldThru", typeof(System.String));
            _dt.Columns.Add("InstMcnt", typeof(System.String));
            _dt.Columns.Add("PermAmt", typeof(System.Int32));
            _dt.Columns.Add("PermYmd", typeof(System.String));
            _dt.Columns.Add("PermHms", typeof(System.String));
            _dt.Columns.Add("PermNo", typeof(System.String));
            _dt.Columns.Add("CnBfCrdIFNo", typeof(System.Int32));
            _dt.Columns.Add("PermStusGb", typeof(System.String));
            _dt.Columns.Add("SlpNo", typeof(System.String));
            _dt.Columns.Add("MbstNo", typeof(System.String));
            _dt.Columns.Add("InpurcCoNm", typeof(System.String));
            _dt.Columns.Add("InsMeth", typeof(System.String));
            _dt.Columns.Add("CrdData", typeof(System.String));
            _dt.Columns.Add("CnYn", typeof(System.String));
            _dt.Columns.Add("CnYmd", typeof(System.String));
            _dt.Columns.Add("CnHms", typeof(System.String));
            _dt.Columns.Add("AcptCnYn", typeof(System.String));
            _dt.Columns.Add("UcoltOccSeq", typeof(System.Int64));
            _dt.Columns.Add("RgtId", typeof(System.String));
            _dt.Columns.Add("RgtDt", typeof(System.DateTime));
            _dt.Columns.Add("LstUpdId", typeof(System.String));
            _dt.Columns.Add("LstUpdDt", typeof(System.DateTime));
            _dt.Columns.Add("CheckedCancel", typeof(System.Boolean));
            _dt.Columns.Add("DataFromCardReader", typeof(System.String));
            _dt.Columns.Add("SubYn", typeof(System.String));
            _dt.Columns.Add("VanGb", typeof(System.String));
            _dt.Columns.Add("ChosNo", typeof(System.String));
            _dt.Columns.Add("PermGb", typeof(System.String));
            _dt.Columns.Add("DeptCd", typeof(System.String));
            _dt.Columns.Add("PreData", typeof(System.String));
            _dt.Columns.Add("ChosGb", typeof(System.String));
            _dt.Columns.Add("SubCheck", typeof(System.String));
            _dt.Columns.Add("CrdTypeGb", typeof(System.String));
            //[IC카드-2018-1차필수]
            _dt.Columns.Add("VanSeqNo", typeof(System.String));//거래고유번호※카드 취소를 위해 필요한  거래고유번호 실물 카드가 아닌 내부 카드 취소를 위한 번호로 승인 시 받은 거래 고유번호 (VAN_SEQNO)=ParseString(106,12)
            _dt.Columns.Add("CrdPermMeth", typeof(System.String));//카드승인방법  MR 또는 빈값:과거 dll 승인 방식  IC: 이지카드 승인 방식
            _dt.Columns.Add("ICPermMeth", typeof(System.String));//IC 승인방법  EZ:이지카드 HI: 하이패스 승인 KI:무인수납기 MB:모바일 
            _dt.Columns.Add("POSNo", typeof(System.String));//이지카드 POS번호 날짜+창구번호2자리+5자리 = 13자리로 생성 예정
            _dt.Columns.Add("CrdTypGb", typeof(System.String));
            return _dt;
        }

        private DataSet SelectOrdByChosNo(string chosNo, string clnDeptCd, string hosCd, string unitNo, string sugaAppYmd, string viewGb)
        {
            DataSet OrdDs = null;
            DataTable CardDt = null;

            try
            {
                OrdDs = _FEEFacade.SelectOrdByChosNo(chosNo, clnDeptCd, hosCd, unitNo, sugaAppYmd, viewGb);



                // 당일 건[예약건 아님. 접수 건]이면서 기수납일 경우 .. 
                // 기존 카드 수납 건[카드 등]에 대한 처리 
                ArrayList chosNoList = new ArrayList();
                chosNoList.Add(chosNo);
                CardDt = _FEEFacade.SelectAmbCrdAcptByChosNoList_AmbCrdAcptNTx(chosNoList);

                // DataRow 가 1개라도 있을 경우에만 Merge를 한다. 
                if (CardDt.Rows.Count > 0)
                {
                    CardDt.TableName = "TempCrdDt";
                    OrdDs.Merge(CardDt);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return OrdDs;
        }

        private DataTable KIOSKAcptPsbl(DataSet ordDs, string psblGb, Hashtable pht)
        {
            try
            {
                return _FEEFacade.KIOSKAcptPsblDt(ordDs, psblGb, pht, strHosCd, strUnitNo, strUserid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DataTable getChosListTable()
        {
            DataTable Ord = new DataTable("Ord");

            Ord.Columns.Add("HosCd");	//병원코드
            Ord.Columns.Add("ClnDeptCd");	//진료과코드
            Ord.Columns.Add("DeptNm");	//진료과명
            Ord.Columns.Add("ClnDrId");//의사ID
            Ord.Columns.Add("UserNm");	//진료의명
            Ord.Columns.Add("ClnYmd");		//진료날짜(방문일자)
            Ord.Columns.Add("ClnHms");	//진료시간(방문시간)
            Ord.Columns.Add("ChosNo");	//접수번호(cretno)
            Ord.Columns.Add("AcptYmd");	//계산일자
            Ord.Columns.Add("AcptGb");	//수납유형(예 : 진료후 수납(1B)/??) 
            Ord.Columns.Add("CalcStrYmd");	//중간금 계산 시작 일자
            Ord.Columns.Add("CalcEndYmd");	//중간금 계산 종료 일자
            Ord.Columns.Add("ChosGb");	//진료유형(O:외래,I:입원,E:응급)
            Ord.Columns.Add("InfoMsg");//고객 센터 연결 정보
            Ord.Columns.Add("ErrorMsg");	//
            Ord.Columns.Add("PatNm");
            Ord.Columns.Add("UnitNo");
            Ord.Columns.Add("DisCalcYn");
            Ord.Columns.Add("PharmCnt");
            Ord.Columns.Add("OtherCnt");
            return Ord;
        }
        public DataTable getPaymentOPDTable()
        {
            DataTable Ord = new DataTable("Ord");

            Ord.Columns.Add("HosCd");	    //병원코드
            Ord.Columns.Add("UnitNo");	//환자번호
            Ord.Columns.Add("PatNm");	//환자명
            Ord.Columns.Add("MCareOrdId"); //엠케어에서 생성
            Ord.Columns.Add("MCareSiteId");	//엠케어환경설정값
            Ord.Columns.Add("ChosNo");		//접수번호(cretno)
            Ord.Columns.Add("ClnYmd");	//외래방문일
            Ord.Columns.Add("ClnHms");	//외래방문시간
            Ord.Columns.Add("MCarePayAll");	//부분결제 여부
            Ord.Columns.Add("ChosGb");	//진료유형(O:외래,I:입원,E:응급)
            Ord.Columns.Add("PayAmt");	//총액
            Ord.Columns.Add("AddTaxAmt");	//부가세
            Ord.Columns.Add("McareServiceAmt");	//서비스료
            Ord.Columns.Add("McareApproval");	//엠케어환경설정값
            Ord.Columns.Add("ErrorMsg");	//
            Ord.Columns.Add("DisCalcYn");
            Ord.Columns.Add("PharmCnt");
            Ord.Columns.Add("OtherCnt");

            return Ord;
        }
        public DataTable makeAcptResultDt()
        {
            DataTable AcptResult = new DataTable("AcptResult");

            AcptResult.Columns.Add("HosCd");	    //
            AcptResult.Columns.Add("ResultCd");	    //
            AcptResult.Columns.Add("ErrorMsg");	//
            AcptResult.Columns.Add("BillNo");	    //
            AcptResult.Columns.Add("HosoOrdNo");	//
            DataRow addRow = AcptResult.NewRow();
            AcptResult.Rows.Add(addRow);


            return AcptResult;
        }

        private bool Check_Ord_LstUpDt(ref DataSet ordDs)
        {
            DataTable ordDt = ordDs.Tables["Ord"].Copy();
            DataSet returnDs = null;

            string pChosNo = string.Empty;

            bool boolvalue = true;

            string RcptMsg = string.Empty;


            foreach (DataRow ordrw in ordDt.Rows)
            {
                Hashtable _pht = new Hashtable();

                _pht.Add("TRNSGB", "N");
                _pht.Add("SPGB", "ComPareLstOrdDt");

                pChosNo = ordrw["ChosNo"].ToString();
                _pht.Add("ChosNo", pChosNo);

                if (ordrw["OrdLstUpdDt"] == DBNull.Value)
                {
                    _pht.Add("OrdLstUpdDt", null);
                }
                else
                {
                    _pht.Add("OrdLstUpdDt", (System.DateTime)ordrw["OrdLstUpdDt"]);
                }

                /* TODO : YUMC_Change_Start(20061113, By 박성용) */
                /*	수납 전 유효한 영수증에 대한 체크 */

                if (ordDt.Columns.Contains("VldBilNo"))
                {
                    if (ordrw["VldBilNo"] != null && ordrw["VldBilNo"] != DBNull.Value)
                    {
                        _pht.Add("VldBilNo", ordrw["VldBilNo"].ToString().Trim());
                    }
                    else
                    {
                        _pht.Add("VldBilNo", "");
                    }
                }
                else
                {
                    _pht.Add("VldBilNo", "NOT_CHECK");	// 수납영수증을 체크하지 말아라~
                }
                /* YUMC_Change_End(20061113, By 박성용)*/

                //HIS.Facade.HP.FEE.FEEFacade _controller = new HIS.Facade.HP.FEE.FEEFacade();
                returnDs = _FEEFacade.HpEtcDs(_pht);

                if (returnDs == null || returnDs.Tables.Count <= 0)
                {
                    if (boolvalue) boolvalue = true;
                }
                else
                {
                    boolvalue = false;
                }
            }

            return boolvalue;
        }

        #region 매입처코드별 카드계정코드를 조회한다. : GetAccCd
        //매입처코드별 카드계정코드를 조회한다.
        /// <summary>
        /// 007   신한카드
        /// 008   외환카드
        /// 016   국민카드
        /// 026   비씨카드
        /// 027   현대카드
        /// 029   엘지카드
        /// 031   삼성카드
        /// 047   롯데카드
        /// </summary>
        /// <param name="pInpurcCd"></param>
        /// <returns></returns>
        private string GetAccCd(string pInpurcCd)
        {
            /*
                CrdAcc	카드-계정매칭	007	S104	신한카드
                CrdAcc	카드-계정매칭	008	S103	외환카드
                CrdAcc	카드-계정매칭	016	S102	국민카드
                CrdAcc	카드-계정매칭	026	S101	비씨카드
                CrdAcc	카드-계정매칭	027	S204	현대카드
                CrdAcc	카드-계정매칭	029	S201	엘지카드
                CrdAcc	카드-계정매칭	031	S202	삼성카드
                CrdAcc	카드-계정매칭	047	S203	롯데카드
              
                CrdAcc	카드-계정매칭	041	S211	농협(굳센)카드
                CrdAcc	카드-계정매칭	018	S212	농협(NH)카드
             */

            if (pInpurcCd == "007") return "S104";
            if (pInpurcCd == "008") return "S103";
            if (pInpurcCd == "016") return "S102";
            if (pInpurcCd == "026") return "S101";
            if (pInpurcCd == "027") return "S204";
            if (pInpurcCd == "029") return "S201";

            //전일환 TEST위해 수정
            if (pInpurcCd == "001") return "S201";

            if (pInpurcCd == "031") return "S202";
            if (pInpurcCd == "047") return "S203";

            //[2012.09.05]광주관련 추가
            if (pInpurcCd == "041") return "S211";
            if (pInpurcCd == "018") return "S212";

            return "";
        }
        #endregion

        /// <summary>
        /// 카드 수납 시 기존 금액에 대해, 신규 요청액을 합산한다. 
        /// 
        /// 1. OrdDs 의 Ord 에서 내원번호 별로 뽑아내어서, cardDt에서 내원 번호별로 DataRow[] 를 얻는다. 
        /// 2. 내원 번호별 카드 에서, 신규 요청액 ( PreData ='' )의 금액을 
        ///		 OrdDs 의 Ord 내의 CrdAcptAmt와 합산한다. 
        ///			/// 
        /// </summary>
        /// <param name="OrdDs"></param>
        /// <param name="cardDt"></param>
        private void SetCardDtAndOrdByAcpt(ref DataSet OrdDs)
        {
            string pchosNo = string.Empty;
            DataRow[] aRow = null;
            int iNewPermAmt = 0;

            DataTable cardDt = OrdDs.Tables["HF_CrdIF"];

            for (int i = 0; i < OrdDs.Tables["Ord"].Rows.Count; i++)
            {
                pchosNo = OrdDs.Tables["Ord"].Rows[i]["ChosNo"].ToString();

                aRow = cardDt.Select("ChosNo ='" + pchosNo + "'");
                iNewPermAmt = 0;

                for (int j = 0; j < aRow.Length; j++)
                {
                    if (aRow[j]["PreData"].ToString().Trim() == "")	// 신규 일때 
                        iNewPermAmt += Convert.ToInt32(aRow[j]["PermAmt"].ToString().Replace(",", ""));
                }

                OrdDs.Tables["Ord"].Rows[i]["pAcptCrdAmt"] = Convert.ToInt32(OrdDs.Tables["Ord"].Rows[i]["CrdAcptAmt"].ToString().Replace(",", ""));
                OrdDs.Tables["Ord"].Rows[i]["CnCrdAcptAmt"] = 0;
                OrdDs.Tables["Ord"].Rows[i]["AcptNewCrdAmt"] = iNewPermAmt;

                // 실제 db에 저장될 금엑 
                OrdDs.Tables["Ord"].Rows[i]["CrdAcptAmt"] = Convert.ToInt32(OrdDs.Tables["Ord"].Rows[i]["CrdAcptAmt"].ToString().Replace(",", "")) + iNewPermAmt;

                OrdDs.Tables["Ord"].Rows[i].EndEdit();
            }
        }

        /* TODO : YUMC_Change_Start(20060524, By 박성용)
		사유	신촌요구사항 */

        /// <summary>
        /// 수납 전 체크를 할 사항에 대해 
        ///		[2006.05.24]	신용카드 결재 요청으로 응답을 받았을 경우.
        ///						금액이 0 원인 데이터에 대해서는, 현금 결재 처리를 위해 
        ///						해당 내원건의 HF_CrdIF 를 삭제한다. 
        /// 
        /// </summary>
        public void CheckAmbAcptBefore(ref DataSet ds)
        {
            DataTable ord = ds.Tables["Ord"];       // Ord 테이블 
            DataTable crd = ds.Tables["HF_CrdIF"];  // HF_CrdIF 테이블 

            if (!crd.Columns.Contains("DelYnCols"))
            {
                crd.Columns.Add("DelYnCols", typeof(string));
            }

            string pchosno = string.Empty;
            DataRow[] rRow = null;

            int ipayamt = 0;

            foreach (DataRow arow in ord.Rows)
            {
                pchosno = arow["ChosNo"].ToString().Trim();
                rRow = crd.Select("ChosNo ='" + pchosno + "'");

                for (int i = 0; i < rRow.Length; i++)
                {
                    rRow[i]["DelYnCols"] = "N";

                    if (rRow[i]["PermAmt"] == null || rRow[i]["PermAmt"] == DBNull.Value ||
                        rRow[i]["PermAmt"].ToString().Trim().Replace(",", "") == string.Empty)
                        rRow[i]["PermAmt"] = 0;

                    ipayamt = Convert.ToInt32(rRow[i]["PermAmt"]);

                    if (ipayamt == 0)   // 0 원일 경우 
                    {
                        rRow[i]["DelYnCols"] = "Y"; // 삭제처리한다. 
                    }
                }
            }

            rRow = crd.Select(" DelYnCols ='Y'");

            for (int i = 0; i < rRow.Length; i++)
                crd.Rows.Remove(rRow[i]);


            crd.AcceptChanges();
        }

        #region  ▣ 외래 수납   #4.1.1	 수납하는 금액을 검증합니다.

        /// <summary>
        /// 금액 검증
        /// </summary>
        /// <param name="ordDs"></param>
        /// <param name="pUnitNo"></param>
        /// <param name="pUserId"></param>
        /// <returns></returns>
        public bool AcptVerific(DataSet ordDs, string pUnitNo, string pUserId)
        {
            bool returnValue = true;

            try
            {
                //HIS.Facade.HP.FEE.FEEFacade FEEFacade = new HIS.Facade.HP.FEE.FEEFacade();
                returnValue = _FEEFacade.AcptVerific(ordDs, pUnitNo, pUserId);

            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            return returnValue;
        }

        #endregion

        #region 카드 Dt의 구분을 설정합니다.

        //		PreData 설정 : 
        // 
        //		P	:	기존 승인 
        //		C	:	기존 승인 취소 
        //		-	:	신규 승인 취소 --> 삭제됨
        //		N	:	신규 승인

        public void CrdAcptDt(ref DataSet ordDs) //datatable->dataset으로변경
        {
            DataRow cRow = null;

            for (int i = 0; i < ordDs.Tables["HF_CrdIF"].Rows.Count; i++)
            {
                cRow = ordDs.Tables["HF_CrdIF"].Rows[i];

                // 기존 승인에 대한 취소건을 판단합니다. 
                // 신규 승인에 대한 취소건은 화면에서 없어집니다. 

                if (Convert.ToBoolean(cRow["CheckedCancel"]) & cRow["PreData"].ToString().Trim() == "P")
                {
                    cRow["PreData"] = "C";		// C : 기존 승인에 대한 취소 건 [P --> C ]
                    continue;
                }


                // 취소 되지 않은 기존 승인 --> 그대로 P를 박아버린다. 
                if (!Convert.ToBoolean(cRow["CheckedCancel"]) & cRow["PreData"].ToString().Trim() == "P")
                {
                    cRow["PreData"] = "P";
                    continue;
                }


                if (!Convert.ToBoolean(cRow["CheckedCancel"]) & cRow["PreData"].ToString().Trim() == "")	// 이건 신규 승인 요청건 
                {
                    cRow["PreData"] = "N";		// N : 기존 승인에 대한 취소 건
                    continue;
                }

                // 만일 이런 황당한 건이 발생 시에는 원천봉쇄를 한다. 
                if (cRow["PreData"].ToString().Trim() == "")
                {
                    throw new Exception("카드의 셋팅 정보가 불확실 합니다. 카드 정보를 다시 입력해 주십시요");
                }

                //				// 혹시나 딸려온 신규 승인 취소 요청 건 --> 강 삭제한다. 
                //				if ( Convert.ToBoolean( cRow["CheckedCancel"]) & cRow["PreData"].ToString().Trim() =="" )	// 이건 신규 승인 취소 요청 건
                //				{
                //					cRow["PreData"] = "-";		// - : 기존 승인에 대한 취소 건
                //					continue; 
                //				}

            }

            // 신규 승인 요청건을 삭제한다. 
            DataRow[] deleteCrd = ordDs.Tables["HF_CrdIF"].Select(" PreData ='-' ");

            for (int d = 0; d < deleteCrd.Length; d++)
            {
                ordDs.Tables["HF_CrdIF"].Rows.Remove(deleteCrd[d]);
            }
        }

        #endregion

        #region 현금영수증, 카드IF 채번 : GetIFNo

        /// <summary>
        /// 현금영수증, 카드IF 채번
        /// </summary>
        /// <param name="pCashIFCount">현금영수증IF번호 채번수</param>
        /// <param name="pCrdIFCount">카드IF번호 채번수</param>
        /// <returns></returns>
        public DataSet GetIFNo(int pCashIFCount, int pCrdIFCount)
        {
            Hashtable ht = new Hashtable();
            ht.Add("CashIFCount", pCashIFCount);
            ht.Add("CrdIFCount", pCrdIFCount);

            //HIS.Facade.HP.FEE.FEEFacade facade = new HIS.Facade.HP.FEE.FEEFacade();
            DataSet ds = _FEEFacade.SelectFeeNoList(ht);

            return ds;
        }


        #endregion

        // 공통메소드 
        #region ★ 공통 메소드(MatchFeeNo) IF 번호 매치

        /// <summary>
        /// IF번호를 매치시킨다.
        /// </summary>
        /// <param name="OrdDt">현금 및 카드 Dt</param>
        /// <param name="FeeDt">IF 번호 DT</param>
        /// <param name="isCash">True : 현금영수증승인, False 카드승인</param>
        public void MatchFeeNo(DataTable OrdDt, DataTable FeeDt, bool isCash)
        {
            //현금 영수증인가?
            if (isCash)
            {
                for (int i = 0; i < OrdDt.Rows.Count; i++)
                {
                    // 현금영수증 IFNo 에 새로 얻은 카드 IFNO를 매치시킨다. 
                    OrdDt.Rows[i]["CashBilIFNo"] = FeeDt.Rows[i]["IFNo"];
                }
            }
            //카드인가?
            else
            {
                int Idx = 0;

                // 기존 승인 건에 대한 취소 : PreData = 'C'
                // 신규 승인 요청 건에 대한 요청 : PreData = 'N'
                for (int i = 0; i < OrdDt.Rows.Count; i++)
                {
                    if (OrdDt.Rows[i]["PreData"].ToString().Trim() == "N"
                        || OrdDt.Rows[i]["PreData"].ToString().Trim() == "C")
                    {
                        //	카드 테이블의 IFNo를 매치
                        OrdDt.Rows[i]["CrdIFNo"] = FeeDt.Rows[Idx]["IFNo"];
                        Idx++;
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// 수납 검증 메시지 처리 및 DB에 저장
        /// </summary>
        private void AcptVerificFiltering(string logChosNo, string logLogDesn, string logLogMsg, string pUnitNo, string pUserid)
        {
            string[] err = logLogMsg.Trim().Split('$');
            string[] errsub = err[1].Split('%');

            // 로그를 기록한다. 
            //HIS.Facade.HP.FEE.FEEFacade _logFacade = new HIS.Facade.HP.FEE.FEEFacade();

            _FEEFacade.LogWrite(logChosNo, pUnitNo, "AmbVerific", DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.ToString("yyyyMMdd"), errsub[1],
                errsub[2], pUserid, "O", "", errsub[3], errsub[4]);

        }
    }
}