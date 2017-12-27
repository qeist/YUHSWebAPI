using System;
using System.Collections;
using System.Data;
using static YUHS.WebAPI.Common.Communication.HIS20ProxyFactory;

namespace YUHS.WebAPI.MCare.Patient.Library
{
    public class AcptLib
    {
        public ZZZFacade _ZZZFacade { get; set; }
        public FEEFacade _FEEFacde { get; set; }

        public DataTable ErrDt { get; set; }

        public AcptLib()
        {
            _ZZZFacade = new ZZZProxy();
            _FEEFacde = new FEEProxy();
        }

        public string HosCd{ get; set; }
        public string UnitNo { get; set; }
        public string UserId { get; set; }

        private string MOBILE_GB { get; } = "M";
        private string MOBILE_USER { get; } = "MOBILE";
        private string MOBILE_WINDID { get; } = "MO";
        private string MOBILE_DOMAINNM { get; } = "MOBILE";

        public DataTable GetPaymentList(string hosCd, string unitNo)
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

            #region ☆ 2017.06.26 시행예정일 자동보류처리

               DataTable resultDt = null;

            //20170714 추가 WindId 정보를 가지고 오기 위해서(병원코드, PC명, 오늘날짜) 
            //DataTable dtWindId = GetWindId(HosCd, Dns.GetHostName().Trim(), DateTime.Now.ToString("yyyyMMdd"));

            //strWindId = dtWindId.Rows[0]["WindId"].ToString().Trim();

            //HIS.Facade.HP.ZZZ.ZZZFacade _zzzFacade = new HIS.Facade.HP.ZZZ.ZZZFacade();

            Hashtable hs = new Hashtable();

            hs.Add("HosCd", hosCd);
            hs.Add("DomainNm", MOBILE_DOMAINNM);
            hs.Add("UnitNo", unitNo);
            hs.Add("ChosNo", "");
            hs.Add("Rmk1", "");
            hs.Add("Rmk2", "");
            hs.Add("Rmk3", "");
            hs.Add("WindId", MOBILE_WINDID);
            hs.Add("UserId", MOBILE_USER);
            hs.Add("HoldGb", "A");

            // 프로시져 호출 : USP_HP_FEE_IP_INSERT_HP_KIOSKAutoHold
            DataSet dss = _ZZZFacade.InsertKIOSKAutoHold(hs);

            string PlnExecYmdUseYn = dss.Tables["Hold"].Rows[0]["PlnExecYmdUseYn"].ToString().Trim();

            #endregion

            //if (PlnExecYmdUseYn == "Y")
            //{
            //    DataSet ds = SelectRcptContByChosNo(pUnitNo, "", "", HosCd, "M", pUserid);
            //    _OrdDs = ds;
            //    //return ds;
            //}
            //else
            //{
            //    //[16.11.09]DataSet ds=SelectRcptContByChosNo( pUnitNo, "" , "", HosCd, "A",  pUserid);
            //모바일에선 K가 아니라 M으로 
            resultDt = getPaymentList_1(hosCd, unitNo); //2016.11.09 이용주 무인수납기에서 예약비 수납못하게 @strGb가 strGb 파라미터 A ->K 로 변경
            //    _OrdDs = ds;
            //    //return ds;
            //}

            #region ☆ 2017.06.26 시행예정일 자동보류처리

            if (resultDt == null || resultDt.Columns.Contains("ErrorMsg"))
            {

                Hashtable hss = new Hashtable();

                hss.Add("HosCd", hosCd);
                hss.Add("DomainNm", MOBILE_DOMAINNM);
                hss.Add("UnitNo", unitNo);

                hss.Add("ChosNo", "");
                hss.Add("Rmk1", "");
                hss.Add("Rmk2", "");
                hss.Add("Rmk3", "");
                hss.Add("WindId", MOBILE_WINDID);
                hss.Add("UserId", MOBILE_USER);
                hss.Add("HoldGb", "C");

                // 프로시져 호출 : USP_HP_FEE_IP_INSERT_HP_KIOSKAutoHold
                _ZZZFacade.InsertKIOSKAutoHold(hss);

            }

            #endregion

            return resultDt;
        }

        private DataTable getPaymentList_1(string hosCd, string unitNo)
        {
            string pUserid = "9000001"; //? MOBILE로 바꾸면 아래 걸림.
            if (pUserid.Length >= 8)
                pUserid = pUserid.Substring(0, 8);

            DataSet ds = null;
            DataSet _result = null;

            HosCd = hosCd;		// 병원코드를 넣는다. 
            UnitNo = unitNo;	// 등록번호를 입력 
            UserId = pUserid;	// UserId

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
                pht.Add("TermId", pUserid);


                // 데이터 조회 : 강승숙 선생님의 SP 실행
                DataSet AcptPsbl = _FEEFacde.HpEtcDs(pht);

                ErrDt = KIOSKAcptPsbl(AcptPsbl, "KIOSKAcptPsbl", null);

                if (ErrDt != null)
                {
                    //DataSet returnDs = new DataSet();
                    //returnDs.Merge(errDt);
                    //return returnDs; 
                    DataRow rOrd = Ord.NewRow();
                    rOrd["ErrorMsg"] = ErrDt.Rows[0]["ERROR_MSG"].ToString();
                    Ord.Rows.Add(rOrd);
                    Ord.AcceptChanges();
                    return Ord;
                }

                // 환자 정보에 대한 처리 
                DataSet patInfoDs = _FEEFacde.SelectAcptContByChosNo(unitNo, hosCd);

                if (patInfoDs.Tables.Contains("PatInfo") && patInfoDs.Tables["PatInfo"].Rows.Count > 0 && patInfoDs.Tables["PatInfo"].Rows[0]["PatNm"] != null)
                {
                    strPatNm = patInfoDs.Tables["PatInfo"].Rows[0]["PatNm"].ToString();
                }
                ErrDt = KIOSKAcptPsbl(patInfoDs, "UnitNo", null);

                if (ErrDt != null)
                {
                    //DataSet returnDs = new DataSet();
                    //returnDs.Merge(errDt);
                    //return returnDs;
                    DataRow rOrd = Ord.NewRow();
                    rOrd["ErrorMsg"] = ErrDt.Rows[0]["ERROR_MSG"].ToString();
                    Ord.Rows.Add(rOrd);
                    Ord.AcceptChanges();
                    return Ord;
                }
                

                #endregion


                // 내원건을 뽑아오잒~
                //ds = facade.SelectRcptContByChosNo(pUnitNo, pFrClnYmd, pToClnYmd, pHosCd, pGb);
                ds = _FEEFacde.SelectRcptContByChosNo(unitNo, "", "", hosCd, MOBILE_GB);

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

                if (_result == null) return null;

                _result = _FEEFacde.ChkKIOSKExcept(_result);

                if (_result == null) return null;


                ErrDt = KIOSKAcptPsbl(_result, "ChosNo", pht);

                // KIOSK 불가 대상을 내원번호 별로 선별하자 
                if (ErrDt != null)
                {
                    // KIOSK 불가 대상일 경우 
                    //DataSet returnDs = new DataSet();
                    //returnDs.Merge(errDt);
                    //return returnDs;
                    DataRow rOrd = Ord.NewRow();
                    rOrd["ErrorMsg"] = ErrDt.Rows[0]["ERROR_MSG"].ToString();
                    Ord.Rows.Add(rOrd);
                    Ord.AcceptChanges();
                    return Ord;
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
                    rOrd["ClnYmd"] = _result.Tables["Ord"].Rows[i]["ClnYmd"];
                    rOrd["ClnHms"] = _result.Tables["Ord"].Rows[i]["ClnHms"];
                    rOrd["ChosNo"] = _result.Tables["Ord"].Rows[i]["ChosNo"];
                    rOrd["AcptYmd"] = DateTime.Now.ToString("yyyyMMdd");
                    rOrd["AcptGb"] = "";
                    rOrd["CalcStrYmd"] = "";//외래빈값
                    rOrd["CalcEndYmd"] = "";//외래빈값
                    rOrd["ChosGb"] = "O";
                    rOrd["MainSubInsuNo"] = "M";//외래 M고정값.
                    rOrd["ErrorMsg"] = "";
                    rOrd["PatNm"] = strPatNm;
                    rOrd["UnitNo"] = unitNo;
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
                rOrd["ErrorMsg"] = ErrDt.Rows[0]["ERROR_MSG"].ToString();
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

        private DataSet CalcFeeAmt(DataSet ordDs, string chosNo, int remitAmtv)
        {
            try
            {
                return _FEEFacde.GetAmbAcptPayAmt(ordDs, chosNo, remitAmtv);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DataSet AcptOrdByChosNo(DataSet ordDs, string chosNo, bool checkRcpt)
        {
            DataSet OrdDsChosNo = new DataSet();	// 리턴을 할, 실제 수납에 저장을 할 데이터 셋

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

        private DataTable GetErrorDt(string errchosNo, string errcode, string errvalue, string deeth)
        {
            ErrDt = MakeErrorDt();

            ErrDt.Rows[0]["ChosNo"] = errchosNo;
            ErrDt.Rows[0]["ERROR_CODE"] = errcode;
            ErrDt.Rows[0]["ERROR_MSG"] = errvalue;
            ErrDt.Rows[0]["DEEPTH"] = deeth;

            ErrDt.AcceptChanges();
            return ErrDt;
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
                OrdDs = _FEEFacde.SelectOrdByChosNo(chosNo, clnDeptCd, hosCd, unitNo, sugaAppYmd, viewGb);



                // 당일 건[예약건 아님. 접수 건]이면서 기수납일 경우 .. 
                // 기존 카드 수납 건[카드 등]에 대한 처리 
                ArrayList chosNoList = new ArrayList();
                chosNoList.Add(chosNo);
                CardDt = _FEEFacde.SelectAmbCrdAcptByChosNoList_AmbCrdAcptNTx(chosNoList);

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
                return _FEEFacde.KIOSKAcptPsblDt(ordDs, psblGb, pht, HosCd, UnitNo, UserId);
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
            Ord.Columns.Add("MainSubInsuNo");	//보험주부유형. M(주), S(부)
            Ord.Columns.Add("ErrorMsg");	//
            Ord.Columns.Add("PatNm");
            Ord.Columns.Add("UnitNo");

            return Ord;
        }
    }
}