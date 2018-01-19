using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using static YUHS.WebAPI.Common.Communication.HIS20ProxyFactory;

namespace YUHS.WebAPI.MCare.Patient.Library
{
    public class AcptAdmiLib
    {
        public ZZZFacade _ZZZFacade { get; set; }
        public FEEFacade _FEEFacade { get; set; }

        public DataTable ErrDt { get; set; }

        public AcptAdmiLib()
        {
            _ZZZFacade = new ZZZProxy();
            _FEEFacade = new FEEProxy();
        }

        private string strHosCd;
        private string strUnitNo;
        private string ErrorValue;
        private string strKioskID;
        private string strExceptionMessage;
        private string ErrorMsg;
        private string strChosNo;
        DataTable errDt;
        private string strBilNo;
        private DataTable rtnResult;
        private string TotalPayAmtBilYn = "N";          // 전액수납여부 체크 
        private DataTable OCS_CARD_LISTDT;
        private bool isExsitPermCrd = false;

        public DataTable GetPaymentList(string hosCd, string unitNo, string chosGb, string chosNo, string deptCd, string MaintSubCd, string clnYmd, string acptYmd, string proxyYn)
        {
            // 전역변수 설정 
			strHosCd = hosCd; 
			strUnitNo = unitNo; 
			//strChosNo = pChosNo;
            strKioskID = "MOBILE";
			
			DataSet admiAcptList = null; 
			DataTable admiAcptListDt = null; 

			ErrorMsg = string.Empty; 

			Hashtable ht = new Hashtable();

			ht.Add("HosCd", strHosCd );
			ht.Add("UnitNo", strUnitNo.Trim());
			//ht.Add("ChosNo", strChosNo.Trim());
			ht.Add("ChosNo", string.Empty);
			ht.Add("ChosGb" , "I");		// 입원 

             
            DataTable Ord = MakeAdmiChosList();
            DataRow rOrd = Ord.NewRow();

			try
			{	
				ErrorMsg = "ERR_FIND_PATINFO";

                #region ●1 - 환자의 입원 정보를 조회합니다 

                //HIS.Facade.HP.FEE.FEEFacade _oFacade = new HIS.Facade.HP.FEE.FEEFacade(); 
                //admiAcptList = _oFacade.SelectAdmiDep_AdmiAcpNtx(ht); 

                admiAcptList = _FEEFacade.SelectAdmiDep_AdmiAcpNtx(ht);

				// 리턴받은 테이블 구조 
				//		AdmiPat		:	환자신상-접수 정보
				//		AdmiAcuracc	:	수납금액정보 
				//					PatChagTotAmt	System.Int32
				//					PatChagCalcYmd
				//					AcptAmt		System.Int32
				//					RduAmt		System.Int32
				//					DscAcptYn	
				//					FractionAmt	System.Int32
				//
				//		OpCommYn	:	수술전송여부
				//					OpCommYn	

				#endregion 
                
				// 가지고 온 결과에 대한 Filtering
				if ( admiAcptList.Tables.Count == 0 ||
					! admiAcptList.Tables.Contains("AdmiPat") || 
					admiAcptList.Tables["AdmiPat"].Rows.Count == 0 
					)
				{
					// 입력받은 등록번호로 재원 중인 내원건이 없는 경우
					//return GetErrorDs( "", "NOT_FOUND_PATINFO", makeMsg("NOT_FOUND_PATINFO") , "B", "환자정보가 없습니다" , strKioskID );
                    rOrd["ErrorMsg"] = GetErrorDs("", "NOT_FOUND_PATINFO", makeMsg("NOT_FOUND_PATINFO"), "B", "환자정보가 없습니다", strKioskID).Rows[0]["ERROR_MSG"];
                    Ord.Rows.Add(rOrd);
                    Ord.AcceptChanges();
                    return Ord;
                    
				}

				// #1 . admiAcptList		First Table "AdmiPat"	
				admiAcptListDt = admiAcptList.Tables["AdmiPat"];	

				// 전역변수 ChosNo 를 선언한다. 
				strChosNo =  admiAcptList.Tables["AdmiPat"].Rows[0]["ChosNo"].ToString();

				DataTable AdmiAcuracc = admiAcptList.Tables["AdmiAcuracc"] ; 

				// [2007.01.04] 
				// 입원무인수납 불가 체크 
				DataTable psblDt = ChkAdmiAcptPsbl( ref admiAcptListDt, strChosNo );

                if (psblDt != null)
				{
                    rOrd["ErrorMsg"] = psblDt.Rows[0]["ERROR_MSG"];
                    Ord.Rows.Add(rOrd);
                    Ord.AcceptChanges();
                     
                    return Ord;
                    //return psblDt;
				}



				// [2006.06.07] 
				// 입원시작일자 부터 오늘까지를 컬럼으로 넣습니다. 



				#region ●2 - 환자 입원 정보 처리 

				// 테이블 추가 정보 기록 
				admiAcptListDt.Columns.Add("DscAcptYn", typeof(string)); 
				admiAcptListDt.Rows[0]["DscAcptYn"] = "N";		// 초기값 : 중간수납

				admiAcptListDt.Columns.Add("AmbCount", typeof(string));		// 외래 예약건 존재 시 그 예약건의 갯수 
				admiAcptListDt.Rows[0]["AmbCount"] = 0;			// 예약건 없다

				// 내셔야할 금액
				// AdmiAcuracc 테이블에 기록한다. 
				if (! AdmiAcuracc.Columns.Contains("TotalPayAmt") )
				{
					AdmiAcuracc.Columns.Add("TotalPayAmt", typeof(double));
					AdmiAcuracc.Rows[0]["TotalPayAmt"] = 0;	
				}

				// 퇴원수납 시 단수차액 : FractionAmt 
				// AdmiAcuracc 테이블에 기록한다. 
				if ( ! AdmiAcuracc.Columns.Contains("DscFractionAmt") )
				{
					AdmiAcuracc.Columns.Add("DscFractionAmt", typeof(double));
					AdmiAcuracc.Rows[0]["DscFractionAmt"] = 0;	
				}

				// 퇴원수납 시 외래 예약건에 대한 Total 내셔야 할 금액 
				// AdmiAcuracc 테이블에 기록한다. 
				if ( ! AdmiAcuracc.Columns.Contains("AmbRsvTotalAmt") )
				{
					AdmiAcuracc.Columns.Add("AmbRsvTotalAmt", typeof( double)); 
					AdmiAcuracc.Rows[0]["AmbRsvTotalAmt"] = 0; 
				}

				// 외래 + 입원에 대한 총수납액을 구합니다.
				// AdmiAcuracc 테이블에 기록한다. 
				if ( ! AdmiAcuracc.Columns.Contains("AmbAdmiSumAmt") )
				{
					AdmiAcuracc.Columns.Add("AmbAdmiSumAmt", typeof( double)); 
					AdmiAcuracc.Rows[0]["AmbAdmiSumAmt"] = 0; 
				}
				




				// 재원기간을 넣는다 
				// admiAcptListDt 테이블에 기록한다. 
				if ( ! admiAcptListDt.Columns.Contains("ToDay") )
				{
					admiAcptListDt.Columns.Add("ToDay", typeof( string )); 
					admiAcptListDt.Rows[0]["ToDay"] = DateTime.Now.ToString("yyyyMMdd");
				}

				double iPatChagTotAmt = 0;		// 환자부담총액
				double iRduAmt = 0;			// 감액/후납
				double ipAcptAmt =0;			// 기수납금액
				double iTotalPayAmt =0;		// 환자의 내셔야 할 금액

				

				iPatChagTotAmt = double.Parse(AdmiAcuracc.Rows[0]["PatChagTotAmt"].ToString());
				// iRduAmt = double.Parse(AdmiAcuracc.Rows[0]["AcptAmt"].ToString());
				// ipAcptAmt = double.Parse(AdmiAcuracc.Rows[0]["RduAmt"].ToString());

				ipAcptAmt = double.Parse(AdmiAcuracc.Rows[0]["AcptAmt"].ToString());
				iRduAmt = double.Parse(AdmiAcuracc.Rows[0]["RduAmt"].ToString());

				#region ●2.1 - 환자가 내셔야 할 금액을 계산합니다. 

				// 단수차액 계산을 하고 
				iTotalPayAmt = DoWonFloor(DoWonFloor(iPatChagTotAmt - iRduAmt) - ipAcptAmt );

				AdmiAcuracc.Rows[0]["TotalPayAmt"] = iTotalPayAmt; 
				
				#endregion 
		

				// 퇴원계산일자가 DateTime 이므로 데이터 전환을 위한 값을 넣습니다. 
				admiAcptList.Tables["AdmiPat"].Columns.Add("DscCalcYmdDT", typeof(string)); 
				admiAcptList.Tables["AdmiPat"].Rows[0]["DscCalcYmdDT"] = ""; 

				// 퇴원 수납여부를 기록합니다. DscAcptYn
				if(admiAcptList.Tables["AdmiPat"].Rows[0]["DscCalcYmd"].ToString().Length > 0 &&
					admiAcptList.Tables["AdmiPat"].Rows[0]["DscCalcYmd"].ToString().Substring(0,4) != "1900")
				{
					admiAcptListDt.Rows[0]["DscAcptYn"] = "Y";	// 퇴원수납!!
					admiAcptList.Tables["AdmiPat"].Rows[0]["DscCalcYmdDT"] = Convert.ToDateTime( admiAcptList.Tables["AdmiPat"].Rows[0]["DscCalcYmd"]).ToString("yyyyMMdd");

					admiAcptListDt.Rows[0]["ToDay"] = admiAcptList.Tables["AdmiPat"].Rows[0]["DscCalcYmdDT"] ; 


					// 외래 수납내역을 바인딩합니다. 
					ErrorMsg = "ERR_FIND_AMBACPT"; 

					#region ●2.2 - 외래 예약 내역을 조회 합니다. 
					/*
					SetAmbItem( ref admiAcptList , strUnitNo, strHosCd, pKioskID); 

					// 외래 예약건 조회 후... 예약건이 존재시에는 KIOSK 화면에 알릴 수 있도록 합니다. 
					if ( admiAcptList.Tables.Contains("Ord") 
						&& admiAcptList.Tables["Ord"].Rows.Count > 0
						)
					{
						admiAcptListDt.Rows[0]["AmbCount"] = admiAcptList.Tables["Ord"].Rows.Count; 
					}
                    */
					#endregion 
                    /*
					// 조회를 한 외래 예약건의 내셔야 할 금액의 Sum을 구합니다. 
					// 외래의 경우, 여러 개의 내원 건이 있으므로, Total Sum을 구합니다. 
					// AmbRsvTotalAmt 에 기록 					

					this.ErrorMsg = "ERR_Sum_AmbRsvAmt"; 
					int dAmbRsvAmt = 0; 

					if ( admiAcptList.Tables.Contains("Ord") 
						&& admiAcptList.Tables["Ord"].Rows.Count > 0 )	// 외래에서 처리 대상이 있을 경우 
					{
						DataTable ambAcptList = admiAcptList.Tables["Ord"]; 

						foreach( DataRow aRow in ambAcptList.Rows )
						{
							// 선택은 기본으로 풀어버린다. kiosk 화면에서 자동으로 선택되도록 한다. 
							// aRow["StusGb"] = 0; 

							if ( aRow["PayAmt"] == null || aRow["PayAmt"] == DBNull.Value ||
								aRow["PayAmt"].ToString().Trim() == string.Empty )
								aRow["PayAmt"] = 0; 


							dAmbRsvAmt += Convert.ToInt32( aRow["PayAmt"].ToString().Trim().Replace(",", "") ); 
						}
						AdmiAcuracc.Rows[0]["AmbRsvTotalAmt"] = dAmbRsvAmt; 
					}
                    */
				}
			
	
				#region ●3 퇴원수납일 경우.. 단수 차액을 발생시킨다. DscFractionAmt

				this.ErrorMsg = "ERR_DoFractionAmt"; 

				double iDscFractionAmt = 0;

                string DisCalcYn = "";
                if (admiAcptListDt.Rows[0]["DscAcptYn"].ToString() == "Y")
                {
                    DisCalcYn = "Y";
                }
                else
                {
                    DisCalcYn = "N";
                }

				if ( admiAcptListDt.Rows[0]["DscAcptYn"].ToString() == "Y" )
				{
					// 단수차액 
					//	본인부담총액 - 기수납액 - 감액 
					//		- ( 절삭 본인부담총액 - 기수납액 - 감액 ) 
					//		- 현재 수납 테이블에서 들어간 단수차액의 합계	 
					//					(	USP_HP_FEE_IP_AdmiAcptDA_selectFractionAmt 에서 계산한 단수차액  )

					iDscFractionAmt = 
						(iPatChagTotAmt-ipAcptAmt-iRduAmt 
						- DoWonFloor(iPatChagTotAmt - ipAcptAmt - iRduAmt )
						) -	double.Parse(AdmiAcuracc.Rows[0]["FractionAmt"].ToString());
				}
			
				AdmiAcuracc.Rows[0]["DscFractionAmt"] = iDscFractionAmt; 

				#endregion 
				
				// ★(???) 이미 퇴원수납을 한 대상이면, 메시지 처리를 해야 하는가? 
				if ( admiAcptList.Tables["AdmiAcuracc"].Rows[0]["DscAcptYn"].ToString() == "Y" )
				{
					//return GetErrorDs( strChosNo, "DSC_ACPTTED", "이미 퇴원수납 된 환자입니다." , "B", "퇴원 수납이 되어서 입금불가." , strKioskID ); 
                    rOrd["ErrorMsg"] = GetErrorDs("", "NOT_FOUND_PATINFO", makeMsg("NOT_FOUND_PATINFO"), "B", "환자정보가 없습니다", strKioskID).Rows[0]["ERROR_MSG"];
                    Ord.Rows.Add(rOrd);
                    Ord.AcceptChanges();
                    return Ord;
				}
                
				#endregion 

				#region Old Source
//				if ( iTotalPayAmt < 1000 )
//				{
//					return GetErrorDs( strChosNo, "CNNT_ADMITAMT", "내셔야 할 금액이 "+iTotalPayAmt+" 입니다. 1000원미만의 금액은 창구를 사용하셔요." , "B", "1000원 미만금액("+iTotalPayAmt+") 입금불가" , strKioskID ); 
//				}
				#endregion 

				#region New Source
				/* 2010.07.21 장근주 신용카드 1000원미만승인불가에서 1원미만승인불가로 변경*/
				if ( iTotalPayAmt < 1 )
				{
					//return GetErrorDs( strChosNo, "CNNT_ADMITAMT", "내셔야 할 금액이 "+iTotalPayAmt+" 입니다. 1원미만의 금액은 창구를 사용하셔요." , "B", "1000원 미만금액("+iTotalPayAmt+") 입금불가" , strKioskID ); 
                    rOrd["ErrorMsg"] = GetErrorDs("", "NOT_FOUND_PATINFO", makeMsg("NOT_FOUND_PATINFO"), "B", "환자정보가 없습니다", strKioskID).Rows[0]["ERROR_MSG"];
                    Ord.Rows.Add(rOrd);
                    Ord.AcceptChanges();
                    return Ord;
				}
				#endregion 

                if (admiAcptList.Tables.Contains("AdmiPat") && admiAcptList.Tables["AdmiPat"].Rows.Count>0)
                {
                    //입원은 무조건 0번로우만..
                    rOrd["HosCd"] = hosCd;
                    rOrd["UnitNo"] = unitNo;
                    rOrd["PatNm"] = admiAcptList.Tables["AdmiPat"].Rows[0]["PatNm"].ToString();
                    rOrd["DeptCd"] = admiAcptList.Tables["AdmiPat"].Rows[0]["DeptCd"].ToString();
                    rOrd["AdmiYmd"] = admiAcptList.Tables["AdmiPat"].Rows[0]["OrdStrYmd"].ToString();
                    rOrd["ChosNo"] = admiAcptList.Tables["AdmiPat"].Rows[0]["ChosNo"].ToString();
                    rOrd["CalcStrYmd"] = admiAcptList.Tables["AdmiPat"].Rows[0]["OrdStrYmd"].ToString();
                    if (admiAcptList.Tables["AdmiPat"].Rows[0]["DscCalcYmd"] != null &&admiAcptList.Tables["AdmiPat"].Rows[0]["DscCalcYmd"].ToString() !="" )
                    {
                        rOrd["CalcEndYmd"] = Convert.ToDateTime(admiAcptList.Tables["AdmiPat"].Rows[0]["DscCalcYmd"]).ToString("yyyyMMdd");
                    }
                    else
                    {
                        rOrd["CalcEndYmd"] = admiAcptList.Tables["AdmiPat"].Rows[0]["DscCalcYmd"].ToString();
                    }
                    rOrd["ChosGb"] = "I";
                    rOrd["InfoMsg"] = admiAcptList.Tables["AdmiPat"].Rows[0]["InfoMsg"].ToString();
                    rOrd["MCarePayAll"] = DisCalcYn;
                    rOrd["TotalPayAmt"] = admiAcptList.Tables["AdmiAcuracc"].Rows[0]["TotalPayAmt"].ToString();//
                    rOrd["vatAmt"] = "0";
                    rOrd["AcptAmt"] = admiAcptList.Tables["AdmiAcuracc"].Rows[0]["AcptAmt"].ToString();
                    rOrd["ErrorMsg"] = "";
                    rOrd["DisCalcYn"] = DisCalcYn;

                    rOrd["DeptNm"] = admiAcptList.Tables["AdmiPat"].Rows[0]["DeptNm"].ToString();
                    rOrd["ClnDrId"] = "";
                    rOrd["UserNm"] = admiAcptList.Tables["AdmiPat"].Rows[0]["UserNm"].ToString();

                    Ord.Rows.Add(rOrd);
                    Ord.AcceptChanges();
                }




				// 입원 + 외래예약금의 Sum을 구합니다. 
				// AmbAdmiSumAmt
				//	AdmiAcuracc.Rows[0]["AmbAdmiSumAmt"] 

				//	AdmiAcuracc.Rows[0]["AmbRsvTotalAmt"]
				//	AdmiAcuracc.Rows[0]["TotalPayAmt"]	[단수차액???]
				//AdmiAcuracc.Rows[0]["AmbAdmiSumAmt"] = Convert.ToInt32( AdmiAcuracc.Rows[0]["AmbRsvTotalAmt"] ) 
				//	+ Convert.ToInt32( AdmiAcuracc.Rows[0]["TotalPayAmt"] ) ; 
                

			}
			catch(Exception ex)
			{
				//return GetErrorDs( strChosNo, ErrorMsg, makeMsg(ErrorMsg) , "B", ex.Message , strKioskID ); 
                rOrd["ErrorMsg"] = GetErrorDs("", "NOT_FOUND_PATINFO", makeMsg("NOT_FOUND_PATINFO"), "B", "환자정보가 없습니다", strKioskID).Rows[0]["ERROR_MSG"];
                Ord.Rows.Add(rOrd);
                Ord.AcceptChanges();
                return Ord;
			}
            


            return Ord; 

        }

        public DataSet getPaymentList_1(string pHosCd, string pUnitNo)
        {
            // 전역변수 설정 
            strHosCd = pHosCd;
            strUnitNo = pUnitNo;
            //strChosNo = pChosNo;
            strKioskID = "MOBILE";

            DataSet admiAcptList = null;
            DataTable admiAcptListDt = null;

            ErrorMsg = string.Empty;

            Hashtable ht = new Hashtable();

            ht.Add("HosCd", strHosCd);
            ht.Add("UnitNo", strUnitNo.Trim());
            //ht.Add("ChosNo", strChosNo.Trim());
            ht.Add("ChosNo", string.Empty);
            ht.Add("ChosGb", "I");		// 입원 

            DataTable Ord = MakeAdmiChosList();
            DataRow rOrd = Ord.NewRow();

            try
            {
                ErrorMsg = "ERR_FIND_PATINFO";

                #region ●1 - 환자의 입원 정보를 조회합니다

                //HIS.Facade.HP.FEE.FEEFacade _oFacade = new HIS.Facade.HP.FEE.FEEFacade();
                admiAcptList = _FEEFacade.SelectAdmiDep_AdmiAcpNtx(ht);

                // 리턴받은 테이블 구조 
                //		AdmiPat		:	환자신상-접수 정보
                //		AdmiAcuracc	:	수납금액정보 
                //					PatChagTotAmt	System.Int32
                //					PatChagCalcYmd
                //					AcptAmt		System.Int32
                //					RduAmt		System.Int32
                //					DscAcptYn	
                //					FractionAmt	System.Int32
                //
                //		OpCommYn	:	수술전송여부
                //					OpCommYn	

                #endregion

                // 가지고 온 결과에 대한 Filtering
                if (admiAcptList.Tables.Count == 0 ||
                    !admiAcptList.Tables.Contains("AdmiPat") ||
                    admiAcptList.Tables["AdmiPat"].Rows.Count == 0
                    )
                {
                    // 입력받은 등록번호로 재원 중인 내원건이 없는 경우
                    //return GetErrorDs( "", "NOT_FOUND_PATINFO", makeMsg("NOT_FOUND_PATINFO") , "B", "환자정보가 없습니다" , strKioskID );
                    rOrd["ErrorMsg"] = GetErrorDs("", "NOT_FOUND_PATINFO", makeMsg("NOT_FOUND_PATINFO"), "B", "환자정보가 없습니다", strKioskID).Rows[0]["ERROR_MSG"];
                    Ord.Rows.Add(rOrd);
                    Ord.AcceptChanges();

                    admiAcptList.Merge(Ord);
                    return admiAcptList;

                }

                // #1 . admiAcptList		First Table "AdmiPat"	
                admiAcptListDt = admiAcptList.Tables["AdmiPat"];

                // 전역변수 ChosNo 를 선언한다. 
                strChosNo = admiAcptList.Tables["AdmiPat"].Rows[0]["ChosNo"].ToString();

                DataTable AdmiAcuracc = admiAcptList.Tables["AdmiAcuracc"];

                // [2007.01.04] 
                // 입원무인수납 불가 체크 
                DataTable psblDt = ChkAdmiAcptPsbl(ref admiAcptListDt, strChosNo);

                if (psblDt != null)
                {
                    rOrd["ErrorMsg"] = psblDt.Rows[0]["ERROR_MSG"];
                    Ord.Rows.Add(rOrd);
                    Ord.AcceptChanges();

                    admiAcptList.Merge(Ord);
                    return admiAcptList;
                    //return psblDt;
                }



                // [2006.06.07] 
                // 입원시작일자 부터 오늘까지를 컬럼으로 넣습니다. 



                #region ●2 - 환자 입원 정보 처리

                // 테이블 추가 정보 기록 
                admiAcptListDt.Columns.Add("DscAcptYn", typeof(string));
                admiAcptListDt.Rows[0]["DscAcptYn"] = "N";		// 초기값 : 중간수납

                admiAcptListDt.Columns.Add("AmbCount", typeof(string));		// 외래 예약건 존재 시 그 예약건의 갯수 
                admiAcptListDt.Rows[0]["AmbCount"] = 0;			// 예약건 없다

                // 내셔야할 금액
                // AdmiAcuracc 테이블에 기록한다. 
                if (!AdmiAcuracc.Columns.Contains("TotalPayAmt"))
                {
                    AdmiAcuracc.Columns.Add("TotalPayAmt", typeof(double));
                    AdmiAcuracc.Rows[0]["TotalPayAmt"] = 0;
                }

                // 퇴원수납 시 단수차액 : FractionAmt 
                // AdmiAcuracc 테이블에 기록한다. 
                if (!AdmiAcuracc.Columns.Contains("DscFractionAmt"))
                {
                    AdmiAcuracc.Columns.Add("DscFractionAmt", typeof(double));
                    AdmiAcuracc.Rows[0]["DscFractionAmt"] = 0;
                }

                // 퇴원수납 시 외래 예약건에 대한 Total 내셔야 할 금액 
                // AdmiAcuracc 테이블에 기록한다. 
                if (!AdmiAcuracc.Columns.Contains("AmbRsvTotalAmt"))
                {
                    AdmiAcuracc.Columns.Add("AmbRsvTotalAmt", typeof(double));
                    AdmiAcuracc.Rows[0]["AmbRsvTotalAmt"] = 0;
                }

                // 외래 + 입원에 대한 총수납액을 구합니다.
                // AdmiAcuracc 테이블에 기록한다. 
                if (!AdmiAcuracc.Columns.Contains("AmbAdmiSumAmt"))
                {
                    AdmiAcuracc.Columns.Add("AmbAdmiSumAmt", typeof(double));
                    AdmiAcuracc.Rows[0]["AmbAdmiSumAmt"] = 0;
                }





                // 재원기간을 넣는다 
                // admiAcptListDt 테이블에 기록한다. 
                if (!admiAcptListDt.Columns.Contains("ToDay"))
                {
                    admiAcptListDt.Columns.Add("ToDay", typeof(string));
                    admiAcptListDt.Rows[0]["ToDay"] = DateTime.Now.ToString("yyyyMMdd");
                }

                double iPatChagTotAmt = 0;		// 환자부담총액
                double iRduAmt = 0;			// 감액/후납
                double ipAcptAmt = 0;			// 기수납금액
                double iTotalPayAmt = 0;		// 환자의 내셔야 할 금액



                iPatChagTotAmt = double.Parse(AdmiAcuracc.Rows[0]["PatChagTotAmt"].ToString());
                // iRduAmt = double.Parse(AdmiAcuracc.Rows[0]["AcptAmt"].ToString());
                // ipAcptAmt = double.Parse(AdmiAcuracc.Rows[0]["RduAmt"].ToString());

                ipAcptAmt = double.Parse(AdmiAcuracc.Rows[0]["AcptAmt"].ToString());
                iRduAmt = double.Parse(AdmiAcuracc.Rows[0]["RduAmt"].ToString());

                #region ●2.1 - 환자가 내셔야 할 금액을 계산합니다.

                // 단수차액 계산을 하고 
                iTotalPayAmt = DoWonFloor(DoWonFloor(iPatChagTotAmt - iRduAmt) - ipAcptAmt);

                AdmiAcuracc.Rows[0]["TotalPayAmt"] = iTotalPayAmt;

                #endregion


                // 퇴원계산일자가 DateTime 이므로 데이터 전환을 위한 값을 넣습니다. 
                admiAcptList.Tables["AdmiPat"].Columns.Add("DscCalcYmdDT", typeof(string));
                admiAcptList.Tables["AdmiPat"].Rows[0]["DscCalcYmdDT"] = "";

                // 퇴원 수납여부를 기록합니다. DscAcptYn
                if (admiAcptList.Tables["AdmiPat"].Rows[0]["DscCalcYmd"].ToString().Length > 0 &&
                    admiAcptList.Tables["AdmiPat"].Rows[0]["DscCalcYmd"].ToString().Substring(0, 4) != "1900")
                {
                    admiAcptListDt.Rows[0]["DscAcptYn"] = "Y";	// 퇴원수납!!
                    admiAcptList.Tables["AdmiPat"].Rows[0]["DscCalcYmdDT"] = Convert.ToDateTime(admiAcptList.Tables["AdmiPat"].Rows[0]["DscCalcYmd"]).ToString("yyyyMMdd");

                    admiAcptListDt.Rows[0]["ToDay"] = admiAcptList.Tables["AdmiPat"].Rows[0]["DscCalcYmdDT"];


                    // 외래 수납내역을 바인딩합니다. 
                    ErrorMsg = "ERR_FIND_AMBACPT";

                    #region ●2.2 - 외래 예약 내역을 조회 합니다.
                    /*
					SetAmbItem( ref admiAcptList , strUnitNo, strHosCd, pKioskID); 

					// 외래 예약건 조회 후... 예약건이 존재시에는 KIOSK 화면에 알릴 수 있도록 합니다. 
					if ( admiAcptList.Tables.Contains("Ord") 
						&& admiAcptList.Tables["Ord"].Rows.Count > 0
						)
					{
						admiAcptListDt.Rows[0]["AmbCount"] = admiAcptList.Tables["Ord"].Rows.Count; 
					}
                    */
                    #endregion
                    /*
					// 조회를 한 외래 예약건의 내셔야 할 금액의 Sum을 구합니다. 
					// 외래의 경우, 여러 개의 내원 건이 있으므로, Total Sum을 구합니다. 
					// AmbRsvTotalAmt 에 기록 					

					this.ErrorMsg = "ERR_Sum_AmbRsvAmt"; 
					int dAmbRsvAmt = 0; 

					if ( admiAcptList.Tables.Contains("Ord") 
						&& admiAcptList.Tables["Ord"].Rows.Count > 0 )	// 외래에서 처리 대상이 있을 경우 
					{
						DataTable ambAcptList = admiAcptList.Tables["Ord"]; 

						foreach( DataRow aRow in ambAcptList.Rows )
						{
							// 선택은 기본으로 풀어버린다. kiosk 화면에서 자동으로 선택되도록 한다. 
							// aRow["StusGb"] = 0; 

							if ( aRow["PayAmt"] == null || aRow["PayAmt"] == DBNull.Value ||
								aRow["PayAmt"].ToString().Trim() == string.Empty )
								aRow["PayAmt"] = 0; 


							dAmbRsvAmt += Convert.ToInt32( aRow["PayAmt"].ToString().Trim().Replace(",", "") ); 
						}
						AdmiAcuracc.Rows[0]["AmbRsvTotalAmt"] = dAmbRsvAmt; 
					}
                    */
                }


                #region ●3 퇴원수납일 경우.. 단수 차액을 발생시킨다. DscFractionAmt

                this.ErrorMsg = "ERR_DoFractionAmt";

                double iDscFractionAmt = 0;
                string DisCalcYn = "";
                if (admiAcptListDt.Rows[0]["DscAcptYn"].ToString() == "Y")
                {
                    DisCalcYn = "Y";
                }
                else
                {
                    DisCalcYn = "N";
                }

                if (admiAcptListDt.Rows[0]["DscAcptYn"].ToString() == "Y")
                {
                    // 단수차액 
                    //	본인부담총액 - 기수납액 - 감액 
                    //		- ( 절삭 본인부담총액 - 기수납액 - 감액 ) 
                    //		- 현재 수납 테이블에서 들어간 단수차액의 합계	 
                    //					(	USP_HP_FEE_IP_AdmiAcptDA_selectFractionAmt 에서 계산한 단수차액  )

                    iDscFractionAmt =
                        (iPatChagTotAmt - ipAcptAmt - iRduAmt
                        - DoWonFloor(iPatChagTotAmt - ipAcptAmt - iRduAmt)
                        ) - double.Parse(AdmiAcuracc.Rows[0]["FractionAmt"].ToString());
                }

                AdmiAcuracc.Rows[0]["DscFractionAmt"] = iDscFractionAmt;

                #endregion

                // ★(???) 이미 퇴원수납을 한 대상이면, 메시지 처리를 해야 하는가? 
                if (admiAcptList.Tables["AdmiAcuracc"].Rows[0]["DscAcptYn"].ToString() == "Y")
                {
                    //return GetErrorDs( strChosNo, "DSC_ACPTTED", "이미 퇴원수납 된 환자입니다." , "B", "퇴원 수납이 되어서 입금불가." , strKioskID ); 
                    rOrd["ErrorMsg"] = GetErrorDs("", "NOT_FOUND_PATINFO", makeMsg("NOT_FOUND_PATINFO"), "B", "환자정보가 없습니다", strKioskID).Rows[0]["ERROR_MSG"];
                    Ord.Rows.Add(rOrd);
                    Ord.AcceptChanges();

                    admiAcptList.Merge(Ord);
                    return admiAcptList;
                    //return Ord;
                }

                #endregion

                #region Old Source
                //				if ( iTotalPayAmt < 1000 )
                //				{
                //					return GetErrorDs( strChosNo, "CNNT_ADMITAMT", "내셔야 할 금액이 "+iTotalPayAmt+" 입니다. 1000원미만의 금액은 창구를 사용하셔요." , "B", "1000원 미만금액("+iTotalPayAmt+") 입금불가" , strKioskID ); 
                //				}
                #endregion

                #region New Source
                /* 2010.07.21 장근주 신용카드 1000원미만승인불가에서 1원미만승인불가로 변경*/
                if (iTotalPayAmt < 1)
                {
                    //return GetErrorDs( strChosNo, "CNNT_ADMITAMT", "내셔야 할 금액이 "+iTotalPayAmt+" 입니다. 1원미만의 금액은 창구를 사용하셔요." , "B", "1000원 미만금액("+iTotalPayAmt+") 입금불가" , strKioskID ); 
                    rOrd["ErrorMsg"] = GetErrorDs("", "NOT_FOUND_PATINFO", makeMsg("NOT_FOUND_PATINFO"), "B", "환자정보가 없습니다", strKioskID).Rows[0]["ERROR_MSG"];
                    Ord.Rows.Add(rOrd);
                    Ord.AcceptChanges();

                    admiAcptList.Merge(Ord);
                    return admiAcptList;
                    //return Ord;
                }
                #endregion

                if (admiAcptList.Tables.Contains("AdmiPat") && admiAcptList.Tables["AdmiPat"].Rows.Count > 0)
                {
                    //입원은 무조건 0번로우만..
                    rOrd["HosCd"] = pHosCd;
                    rOrd["UnitNo"] = pUnitNo;
                    rOrd["PatNm"] = admiAcptList.Tables["AdmiPat"].Rows[0]["PatNm"].ToString();
                    rOrd["DeptCd"] = admiAcptList.Tables["AdmiPat"].Rows[0]["DeptCd"].ToString();
                    rOrd["AdmiYmd"] = admiAcptList.Tables["AdmiPat"].Rows[0]["OrdStrYmd"].ToString();
                    rOrd["ChosNo"] = admiAcptList.Tables["AdmiPat"].Rows[0]["ChosNo"].ToString();
                    rOrd["CalcStrYmd"] = admiAcptList.Tables["AdmiPat"].Rows[0]["OrdStrYmd"].ToString();
                    if (admiAcptList.Tables["AdmiPat"].Rows[0]["DscCalcYmd"] != null && admiAcptList.Tables["AdmiPat"].Rows[0]["DscCalcYmd"].ToString() != "")
                    {
                        rOrd["CalcEndYmd"] = Convert.ToDateTime(admiAcptList.Tables["AdmiPat"].Rows[0]["DscCalcYmd"]).ToString("yyyyMMdd");
                    }
                    else
                    {
                        rOrd["CalcEndYmd"] = admiAcptList.Tables["AdmiPat"].Rows[0]["DscCalcYmd"].ToString();
                    }
                    rOrd["ChosGb"] = "I";
                    rOrd["InfoMsg"] = admiAcptList.Tables["AdmiPat"].Rows[0]["InfoMsg"].ToString();
                    rOrd["MCarePayAll"] = DisCalcYn;
                    rOrd["TotalPayAmt"] = admiAcptList.Tables["AdmiAcuracc"].Rows[0]["TotalPayAmt"].ToString();//
                    rOrd["vatAmt"] = "0";
                    rOrd["AcptAmt"] = admiAcptList.Tables["AdmiAcuracc"].Rows[0]["AcptAmt"].ToString();
                    rOrd["ErrorMsg"] = "";
                    rOrd["DisCalcYn"] = DisCalcYn;

                    rOrd["DeptNm"] = admiAcptList.Tables["AdmiPat"].Rows[0]["DeptNm"].ToString();
                    rOrd["ClnDrId"] = "";
                    rOrd["UserNm"] = admiAcptList.Tables["AdmiPat"].Rows[0]["UserNm"].ToString();

                    Ord.Rows.Add(rOrd);
                    Ord.AcceptChanges();
                    admiAcptList.Merge(Ord);
                }




                // 입원 + 외래예약금의 Sum을 구합니다. 
                // AmbAdmiSumAmt
                //	AdmiAcuracc.Rows[0]["AmbAdmiSumAmt"] 

                //	AdmiAcuracc.Rows[0]["AmbRsvTotalAmt"]
                //	AdmiAcuracc.Rows[0]["TotalPayAmt"]	[단수차액???]
                //AdmiAcuracc.Rows[0]["AmbAdmiSumAmt"] = Convert.ToInt32( AdmiAcuracc.Rows[0]["AmbRsvTotalAmt"] ) 
                //	+ Convert.ToInt32( AdmiAcuracc.Rows[0]["TotalPayAmt"] ) ; 


            }
            catch (Exception ex)
            {
                //return GetErrorDs( strChosNo, ErrorMsg, makeMsg(ErrorMsg) , "B", ex.Message , strKioskID ); 
                rOrd["ErrorMsg"] = GetErrorDs("", "NOT_FOUND_PATINFO", makeMsg("NOT_FOUND_PATINFO"), "B", "환자정보가 없습니다", strKioskID).Rows[0]["ERROR_MSG"];
                Ord.Rows.Add(rOrd);
                Ord.AcceptChanges();

                admiAcptList.Merge(Ord);
                return admiAcptList;
                //return Ord;
            }



            return admiAcptList;
        }

        private double DoWonFloor(double dAmt)
        {
            if (dAmt < 0)
                return Math.Floor(Math.Abs(dAmt) / 10) * 10 * -1;
            else
                return Math.Floor(dAmt / 10) * 10;
        }

        /// <summary>
		/// 입원무인수납 불가 대상 체크
		///		06.12.28	휴복 후 처리 --> sp로 변환 가능하도록 
		/// </summary>
		/// <param name="admiDs"></param>
		public DataTable ChkAdmiAcptPsbl(ref DataTable admiDt, string pChosNo)
        {
            #region 주석 처리 
            /*
			// 가정간호 수납 대상자는 별도로 체크를 한다. 
			this.ErrorMsg = "Chk_IsExist_HomNurAmt"; 

			if ( admiDt.Rows[0]["DscAcptYn"].ToString() == "Y")
			{
				if ( admiDt != null 
					&& admiDt.Rows.Count > 0 
					&& admiDt.Columns.Contains("ChosNo") 
					&& admiDt.Rows[0]["ChosNo"] != null
					&& admiDt.Rows[0]["ChosNo"] != DBNull.Value 
					&& admiDt.Rows[0]["ChosNo"].ToString().Trim().Length > 0 
				   )
				{

					Hashtable ht = new Hashtable();
					ht.Add( "ChosNo",  admiDt.Rows[0]["ChosNo"].ToString().Trim() );

					HIS.Facade.HP.FEE.FEEFacade _oFacade = new HIS.Facade.HP.FEE.FEEFacade(); 
					DataTable homNurDt = _oFacade.SelectHomNurAmt(ht);

					if ( homNurDt != null && homNurDt.Rows.Count > 0 )
					{
						if( 
							homNurDt.Rows.Count > 0 
							&& homNurDt.Rows[0]["CardYn"] != null
							&& homNurDt.Rows[0]["CardYn"] != DBNull.Value 
							&& homNurDt.Rows[0]["CardYn"].ToString() != "Y"
							)
						{
							if ( Convert.ToInt32(homNurDt.Rows[0]["RcptAmt"])  > 0 )
							{
								throw new Exception("가정간호 수납 대상금액이 있습니다."); 
							}							
						}
					}
				}
			}
			*/
            #endregion

            ErrorMsg = "ChkAdmiKIOSKPsbl";

            try
            {
                Hashtable pht = new Hashtable();
                pht.Add("ChosNo", pChosNo);
                pht.Add("UserId", "MOBILE");//strKioskID ); 
                pht.Add("PcNm", "MOBILE");//Dns.GetHostName() ); 

                //HIS.Facade.HP.ZZZ.ZZZFacade _oZZZFacade = new HIS.Facade.HP.ZZZ.ZZZFacade();
                DataSet ds = _ZZZFacade.ChkAdmiKIOSKPsbl(pht);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables.Contains("ChkAdmiKIOSK")
                    && ds.Tables["ChkAdmiKIOSK"].Rows.Count > 0)
                {
                    if (ds.Tables["ChkAdmiKIOSK"].Rows[0]["Result"] != null
                        && ds.Tables["ChkAdmiKIOSK"].Rows[0]["Result"].ToString().Trim() == "N")
                    {
                        return GetErrorDs(pChosNo
                            , ds.Tables["ChkAdmiKIOSK"].Rows[0]["Msg"].ToString().Trim()
                            , makeMsg(ds.Tables["ChkAdmiKIOSK"].Rows[0]["Msg"].ToString().Trim())
                            , "B"
                            , ds.Tables["ChkAdmiKIOSK"].Rows[0]["Msg"].ToString().Trim()
                            , strKioskID);
                    }
                }
            }
            catch (Exception ex)
            {
                return GetErrorDs(pChosNo, ErrorMsg, makeMsg(ErrorMsg), "B", ex.Message, strKioskID);
            }

            return null;
        }

        private string makeMsg(string trigerMsg)
        {
            var _oOutPut = new MessageOutPut();
            return _oOutPut.ReturnMsg(trigerMsg);
        }

        private DataTable GetErrorDs(string errchosNo, string errcode, string errvalue, string deeth, string ExceptionMessage, string pstrKioskID)
        {
            errDt = null;   // 기존 Dt를 날려버리고 
            errDt = MakeErrorDt();


            //if ( this.acptIFCracker != null && this.acptIFCracker.GetErrorMsg().Trim().Length > 0 )
            //{
            //    errvalue =  errvalue+ " / " + this.acptIFCracker.GetErrorMsg().Trim(); 
            //}

            if (ExceptionMessage.IndexOf("###") >= 0)
            {
                errvalue = "###" + errvalue;
            }

            errDt.Rows[0]["ChosNo"] = errchosNo;
            errDt.Rows[0]["ERROR_CODE"] = errcode;
            errDt.Rows[0]["ERROR_MSG"] = errvalue;
            errDt.Rows[0]["DEEPTH"] = deeth;

            ErrorValue = errvalue;
            strKioskID = pstrKioskID;
            strExceptionMessage = ExceptionMessage;

            errDt.AcceptChanges();

            ErrorLogWrite();

            //DataSet errDs = new DataSet(); 
            //errDs.Merge(errDt); 
            return errDt;
        }

        private DataTable MakeErrorDt()
        {
            DataTable _dt = new DataTable("ERROR_MSG");

            _dt.Columns.Add("ChosNo", typeof(string));      // 내원번호
            _dt.Columns.Add("ERROR_CODE", typeof(string));  // 에러 코드 
            _dt.Columns.Add("ERROR_MSG", typeof(string));   // 에러 메시지 
            _dt.Columns.Add("DEEPTH", typeof(string));      // 정도 

            DataRow errRow = _dt.NewRow();
            _dt.Rows.Add(errRow);

            return _dt;
        }

        private void ErrorLogWrite()
        {
            string Log3 = string.Empty;

            if (strExceptionMessage.Length > 350)
            {
                Log3 = strExceptionMessage.Substring(300);
                strExceptionMessage = strExceptionMessage.Substring(0, 290);
            }

            try
            {
                //HIS.Facade.HP.FEE.FEEFacade _facade = new HIS.Facade.HP.FEE.FEEFacade();
                _FEEFacade.LogWrite(strChosNo, strUnitNo, "ADMIKIOSK", DateTime.Now.ToString("yyyyMMdd")
                    , DateTime.Now.ToString("yyyyMMdd")
                    , ErrorMsg      // LogNm 은 KEY 
                    , ErrorValue    // Log1 은 KEY-VALUE 
                    , strKioskID    // UserID 
                    , "I"           // 입원 KIOSK는 모두 입원 처리합니다. 
                    , strBilNo
                    , strExceptionMessage // Log2
                    , Log3
                    );
            }
            catch (Exception ex)
            {
            }
        }



        private DataTable MakeAdmiChosList()
        {
            DataTable Ord = new DataTable("Ord");

            Ord.Columns.Add("HosCd");	//병원코드
            Ord.Columns.Add("UnitNo");
            Ord.Columns.Add("PatNm");
            Ord.Columns.Add("DeptCd");	//진료과코드
            Ord.Columns.Add("AdmiYmd");	//입원일자
            Ord.Columns.Add("ChosNo");	//접수번호(cretno)
            Ord.Columns.Add("CalcStrYmd");	//중간금 계산 시작 일자
            Ord.Columns.Add("CalcEndYmd");	//중간금 계산 종료 일자
            Ord.Columns.Add("ChosGb");	//진료유형(O:외래,I:입원,E:응급)
            Ord.Columns.Add("InfoMsg");	//고객 센터 연결 정보
            Ord.Columns.Add("MCarePayAll");//전액수납여부
            Ord.Columns.Add("TotalPayAmt");     //총액
            Ord.Columns.Add("vatAmt");     //부가세
            Ord.Columns.Add("AcptAmt");     //납입금
            //Ord.Columns.Add("UserNm");	//진료의명
            //Ord.Columns.Add("ClnYmd");		//진료날짜(방문일자)
            //Ord.Columns.Add("ClnHms");	//진료시간(방문시간)
            Ord.Columns.Add("ErrorMsg");    //
            Ord.Columns.Add("DisCalcYn");	//퇴원계산여부 Y:퇴원 수납 N:중간금

            Ord.Columns.Add("DeptNm");    //
            Ord.Columns.Add("ClnDrId");    //
            Ord.Columns.Add("UserNm");    //

            return Ord;
        }

        #region▣신촌) 입퇴원 수납 [중간수납/퇴원수납]
        /// <summary>
        /// 입퇴원 수납 [중간수납/퇴원수납]
        /// </summary>
        /// <param name="acptList">입퇴원 수납내역</param>
        /// <param name="pht">신용카드 정보</param>
        /// <param name="hosCd">병원코드</param>
        /// <param name="pWindId">창구번호</param>
        /// <param name="strKioskID">KIOSK ID</param>
        /// <param name="permAmt">입원수납금액입니다.</param>
        public DataTable AcptExec
            (
              //DataSet acptList,
              //string strKioskID,

              //string hosCd,
              //int permAmt,
              //string pWindId,
              //string strCrdInfo, //?
              //string strCrdNO,   //?
              //string strVldTh,
              //string strInstMcnt,
              //string SelectedAmbChosNo,
              //string unitNo 

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

            //모바일-기존파라미터 
            string pWindId = "MO";
            string strCrdInfo = "";
            string strCrdNO = crdNo;
            string strVldTh = vldThru;
            string strInstMcnt = instMcnt;
            string SelectedAmbChosNo = null; //외래 안함.

            //카드승인정보pht로 바꿔 넘긴다.
            Hashtable crdHt = new Hashtable();
            crdHt["HosCd"] = hosCd;
            crdHt["UnitNo "] = unitNo;
            crdHt["ChosGb"] = chosGb;
            crdHt["ChosNo"] = chosNo;
            crdHt["ClnYmd"] = clnYmd;
            crdHt["CrdIssLoc"] = crdIssLoc;
            crdHt["CrdNo"] = crdNo;
            crdHt["CrdTypGb"] = crdTypGb;
            crdHt["VldThru"] = vldThru;
            crdHt["InstMcnt"] = instMcnt;
            crdHt["PermAmt"] = permAmt;
            crdHt["PermYmd"] = permYmd;
            crdHt["PermHms"] = permHms;
            crdHt["PermNo"] = permNo;
            crdHt["SlpNo"] = slpNo;
            crdHt["MbstNo"] = mbstNo;
            crdHt["InpurcCoNm"] = inpurcCoNm;
            crdHt["CrdData"] = crdData;
            crdHt["VanSeqNo"] = vanSeqNo;
            crdHt["CrdPermMeth"] = crdPermMeth;
            crdHt["ICPermMeth"] = iCPermMeth;
            crdHt["POSNo"] = pOSNo;
            crdHt["VanGb"] = vanGb;
            crdHt["InpurcCd"] = inpurcCd;


            //모바일-결제금액을 string->int로 변환
            int iPermAmt = 0;
            if (permAmt != null && permAmt != "")
                iPermAmt = Convert.ToInt32(permAmt);

            string ErrMsg = string.Empty;
            DataSet acptList = null;

            //모바일-결과 테이블 생성
            rtnResult = MakeAcptResult();
            rtnResult.Rows[0]["HosCd"] = hosCd;

            //모바일-OrdDs 받아야되서 다시 조회한다.

            acptList = getPaymentList_1(hosCd, unitNo);
            if (acptList.Tables["Ord"].Rows[0]["ErrorMsg"].ToString() != "")
            {
                rtnResult.Rows[0]["ResultCd"] = "N";
                rtnResult.Rows[0]["ErrorMsg"] = "수납 조회시 오류";
                return rtnResult;
            }

            // 금액이 10원으로 나누어 떨어지는지 확인합니다. 

            string strViewMsg = string.Empty;		// 화면으로 던지는 메시지 
            string strLogKey = string.Empty;		// Log 기록 Key 값 
            string strLogMsg = string.Empty;		// 기록하는 메시지 

            // KIOSKTotPayAmt 는 입원 수납 금액입니다. 
            // 외래는 acptList 내의 "Ord" Table 내에서 StusGb 가 선택된 내에서 
            // PayAmt 컬럼의 금액을 모두 지불하는 것으로 합니다. 

            this.ErrorMsg = "ACPT_START";

            // 전역변수 처리 

            // 전역변수 설정 
            strHosCd = hosCd;
            strUnitNo = acptList.Tables["AdmiPat"].Rows[0]["UnitNo"].ToString().Trim();
            strChosNo = acptList.Tables["AdmiPat"].Rows[0]["ChosNo"].ToString().Trim();
            string strKioskID = "MOBILE";

            DataSet BillDs = new DataSet();		// 영수증 테이블 

            // 카드 수납이 아닐 경우에는 각각의 값에 빈값으로 넣어주셔요 
            Hashtable pht = new Hashtable();
            pht.Add("AcptGb", "CARD");	// 수납방법
            pht.Add("TRACK_DATA", strCrdInfo);	// SWIPE 데이터 
            pht.Add("CARD_NO", strCrdNO);	// 카드번호 
            pht.Add("VldTh", strVldTh);	// 유효기간
            pht.Add("INSTMCNT", strInstMcnt);	// 할부기간

            try
            {
                // 카드인지 현금인지를 확인합니다. 
                string acptgb = pht["AcptGb"].ToString().Trim();		// 수납구분
                string tack_data = pht["TRACK_DATA"].ToString().Trim();	// 신용카드 Swipe 데이터 
                string card_no = pht["CARD_NO"].ToString().Trim();		// 카드 번호 
                string vldth = pht["VldTh"].ToString().Trim();			// 유효기간 
                string instmcnt = pht["INSTMCNT"].ToString();			// 할부기간 

                string pchosno = acptList.Tables["AdmiPat"].Rows[0]["ChosNo"].ToString().Trim();

                // 입원 수납 전 체크를 합니다. 
                //if (!ChkAcpt(ref acptList, ref strLogKey, ref strViewMsg, ref strLogMsg, permAmt))
                if (!ChkAcpt(ref acptList, ref strLogKey, ref strViewMsg, ref strLogMsg, iPermAmt))
                {
                    //return GetErrorDs(pchosno, strLogKey, strViewMsg, "B", strLogMsg, strKioskID);
                    rtnResult.Rows[0]["ErrorMsg"] = GetErrorDs("", "NOT_FOUND_PATINFO", makeMsg("NOT_FOUND_PATINFO"), "B", "환자정보가 없습니다", strKioskID).Rows[0]["ERROR_MSG"].ToString();
                    rtnResult.Rows[0]["ResultCd"] = "N";
                    return rtnResult;
                }


                // 수납시작 
                this.ErrorMsg = "ADMI_ACPT_START";

                // 전액수납인지를 확인합니다. 
                // 내셔야할 금액보다 수납금액이 클 경우 

                if (acptList.Tables["AdmiAcuracc"].Rows[0]["TotalPayAmt"] == null ||
                    acptList.Tables["AdmiAcuracc"].Rows[0]["TotalPayAmt"] == DBNull.Value)
                    acptList.Tables["AdmiAcuracc"].Rows[0]["TotalPayAmt"] = 0;

                int iAdmiTotalPayAmt = Convert.ToInt32(acptList.Tables["AdmiAcuracc"].Rows[0]["TotalPayAmt"].ToString());

                //if (permAmt >= iAdmiTotalPayAmt)
                if (iPermAmt >= iAdmiTotalPayAmt)
                {
                    TotalPayAmtBilYn = "Y";		// 전액수납 
                }


                //BillDs = DoAcpt(ref acptList, pht, pWindId, permAmt, hosCd, strKioskID, SelectedAmbChosNo, TotalPayAmtBilYn);
                BillDs = DoAcpt(ref acptList, pht, pWindId, iPermAmt, hosCd, strKioskID, SelectedAmbChosNo, TotalPayAmtBilYn, crdHt);

            }
            catch (Exception ex)
            {
                rtnResult.Rows[0]["ErrorMsg"] = GetErrorDs("", ErrorMsg, makeMsg(ErrorMsg), "", ex.Message, strKioskID).Rows[0]["ERROR_MSG"].ToString();
                rtnResult.Rows[0]["ResultCd"] = "N";
                return rtnResult;
            }

            return rtnResult;
            //return BillDs;
        }
        #endregion

        // ◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇
        // ◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆
        /// <summary>
        /// 입원 수납 데이터를 만듭니다. 
        ///		--> 신용카드 수납을 위한 데이터 처리와 
        ///			현금 수납을 위한 데이터 처리를 합니다. 
        /// </summary>
        /// <param name="admiAcptList">입퇴원 수납 내역</param>
        /// <param name="pht">신용카드정보 && 수납방법정보</param>
        /// <param name="pWindId">창구ID</param>
        /// <param name="ipayamt">입원수납금액</param> 
        private DataSet DoAcpt
            (
            ref DataSet admiAcptList,
            Hashtable pht,
            string pWindId,
            int ipayamt,
            string pHosCd,
            string pKioskId,
            string SelectedAmbChosNo
            , string pTotalPayAmtBilYn
            , Hashtable crdHt //모바일
            )
        {

            DataSet billDs = new DataSet("BILL_DS");

            // ●1. ※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※
            //	입원수납 테이블 [HF_AdmiAcpt] 에 기록될 기본 정보를 만듭니다. 
            this.ErrorMsg = "ADMI_MAKEDT_1";

            #region	◎	기본 정보

            Hashtable ht = new Hashtable();
            ht.Add("WindId", pWindId);
            ht.Add("HosCd", pHosCd);							// 병원코드
            ht.Add("RgtId", pKioskId);							// 등록자 ID
            ht.Add("ChosNo", admiAcptList.Tables["AdmiPat"].Rows[0]["ChosNo"]);			// 내원번호
            ht.Add("DscCalcYmd", admiAcptList.Tables["AdmiPat"].Rows[0]["DscCalcYmd"]);	// 퇴원계산일
            ht.Add("ChosGb", "I");														// [??] I	(입원)으로 하드코딩??
            ht.Add("UnitNo", admiAcptList.Tables["AdmiPat"].Rows[0]["UnitNo"]);			// 등록번호

            string pDscAcptYn = string.Empty;		// 퇴원수납여부 

            pDscAcptYn = admiAcptList.Tables["AdmiPat"].Rows[0]["DscAcptYn"].ToString();

            if (pDscAcptYn == "Y")
            {
                // 퇴원수납일 경우 
                ht.Add("AcptGb", "2");										// 퇴원

                // 외래수입여부 : 응급에서 들어왔고, 6시만 미만인 경우는 Y로 세팅해야한다.
                if (admiAcptList.Tables["AdmiPat"].Rows[0]["EmerCalcTyp"].ToString() == "1")		// 외래로 세팅
                    ht.Add("AmbPrftYn", "Y");
                else
                    ht.Add("AmbPrftYn", "N");
            }
            else
            {
                ht.Add("AcptGb", "0");										// 중간
                ht.Add("AmbPrftYn", "N");                                   // 중간계산은 무조건 'N'
            }
            #endregion

            // ●2. ※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※
            //	수납 기본 데이터를 만듭니다. 

            this.ErrorMsg = "ADMI_MAKEDT_2";

            #region ◎ 수납 기본 정보 데이터 테이블 _acptItemDT 을 선언

            // 수납 기본 정보블 셋팅합니다. [모든 데이터가 초기화 처리된 데이터를 만듭니다]
            DataTable _acptItemDT = new DataTable();
            _acptItemDT.Columns.Add("SubYn");					// 계정처리
            _acptItemDT.Columns.Add("AcptItem");				// 항목명
            _acptItemDT.Columns.Add("TotalAmt");				// 받을금액
            _acptItemDT.Columns.Add("RemainAmt");				// 남은 금액
            _acptItemDT.Columns.Add("CrdNo");					// 카드번호
            _acptItemDT.Columns.Add("VldThru");					// 유효기간
            _acptItemDT.Columns.Add("InstMcnt");				// 할부개월
            _acptItemDT.Columns.Add("PermAmt");					// 결제금액	[신용카드 금액으로 정의합니다] 
            _acptItemDT.Columns.Add("CardAccCd");				// 카드계정코드
            _acptItemDT.Columns.Add("ItemTag");					// 항목 구분	admi / amb / ''
            _acptItemDT.Columns.Add("InsMeth");					// 결제방법(1: keyboard, 2:리더기)
            _acptItemDT.Columns.Add("CrdData");					// 카드데이터(카드리더기로 읽은 Data)
            _acptItemDT.Columns.Add("ChosNo");					// 내원번호

            // ★	=======================================================
            // [OCS의 입원 수납과의 차이입니다]
            _acptItemDT.Columns.Add("CrdAppYn");				// 신용카드결재여부 [Y / N]	:	N 일 경우에는 현금결재 
            _acptItemDT.Columns.Add("CashAmt");					// 현금결재금액

            _acptItemDT.Columns.Add("CrdTypGb");					// 2013.01.23 

            #endregion

            // ●3. ※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※
            //	kiosk에서 받은 수납 정보를 기초로 금액 데이터 (현금/카드) 데이터에 대한 기초 정보를 만듭니다. 
            //	입원데이터로 입니다. 

            this.ErrorMsg = "ADMI_MAKEDT_3";

            #region ◎	입원 수납 정보 생성 [_acptItemDT]

            DataRow dr = _acptItemDT.NewRow();		// 새로운 Row 생성 [이미 Clear()] 했으므로 	

            int ichargeAmt = 0;				// 환자가 총 납부를 할 금액	[ ● 총수납금액: 퇴원수납일 경우에는 '총수납금액'='현재수납금액' ]
            ichargeAmt = Convert.ToInt32(admiAcptList.Tables["AdmiAcuracc"].Rows[0]["TotalPayAmt"]);

            // kiosk 에서 입력을 받은 금액(ipayamt)이 총 내셔야 할 금액(ichargeAmt)보다 클 경우에는 자동으로 
            // Kiosk로 부터 입력을 받은 금액으로 바뀌어 진다. 
            //	ipayamt 에 대한 설정 

            //모바일-이미 승인된금액을 받기때문에 이걸 바꿔주면 안되서 주석처리.
            //if (ichargeAmt < ipayamt) ipayamt = ichargeAmt;

            // 카드 정보를 얻어온다.
            //string AcptGb = pht["AcptGb"].ToString().Trim();			//	수납방법 CASH / CARD 
            string AcptGb = "CARD"; //--무조건 카드수납이니까 카드로

            string TRACK_DATA = string.Empty;
            string CARD_NO = string.Empty;
            string VldTh = string.Empty;
            string INSTMCNT = string.Empty;

            int iCardPermAmt = 0;		// 신용카드 결재 금액 
            int iCashAmt = 0;			// 현금결재 금액 
            int iRmdAmt = 0;			// 잔액 

            // 신용카드일 경우 
            if (AcptGb == "CARD")
            {
                TRACK_DATA = pht["TRACK_DATA"].ToString().Trim();	//	SWIPE 데이터 
                CARD_NO = pht["CARD_NO"].ToString().Trim();			//	카드번호 
                VldTh = pht["VldTh"].ToString().Trim();				//	유효기간 
                INSTMCNT = pht["INSTMCNT"].ToString().Trim();		//	할부기간 

                // 입원 신용카드 결재 금액은 kiosk로 부터 입력받은 금액이다. 
                iCardPermAmt = ipayamt;
                // 현금 금액은 없다. 
                iCashAmt = 0;
                // 잔액 = 총 내셔야 할 금액 - 입력받은 금액 
                iRmdAmt = ichargeAmt - ipayamt;		// 잔액은 총 내셔야 할 금액 - 입력받은 현금액 

                dr["CrdAppYn"] = "Y";	// 신용카드 
            }
            else
            {
                // 현금결재 금액 
                INSTMCNT = "00";

                iCardPermAmt = 0;	// 카드금액은 없다 	
                iCashAmt = ipayamt;	// 현금금액은 KIOSK로 부터 입력받은 금액이다. 
                iRmdAmt = ichargeAmt - ipayamt;		// 잔액은 총 내셔야 할 금액 - 입력받은 현금액 

                dr["CrdAppYn"] = "N";  // 현금 
            }


            dr["SubYn"] = string.Empty;		// KIOSK에서 계정처리를 없다
            dr["AcptItem"] = "입원수납";
            dr["TotalAmt"] = ichargeAmt;			// 통째로 내야 할 금액	
            dr["RemainAmt"] = iRmdAmt;			// 잔액 [일단 올인한다] : 추후 KIOSK에서 입력받은 금액을 처리한다. 			
            dr["CrdNo"] = CARD_NO;			// 카드번호
            dr["VldThru"] = VldTh;				// 유효기간 .	일단 빈값
            dr["InstMcnt"] = INSTMCNT;			// 할부기간	.	일단 일시불 
            dr["PermAmt"] = iCardPermAmt;		// 결재금액	.	입원은 입력받은 값으로 셋팅한다. 
            dr["CashAmt"] = iCashAmt;			// 결재금액	.	현금 수납금액 	
            dr["CardAccCd"] = string.Empty;		// 카드 결재 코드 [신용카드일 경우는 계정처리를 없다 모두 빈값이다]
            dr["ItemTag"] = "admi";             // 입원 
                                                /* TODO : YUMC_Change_Start(2009.10.30, By 장근주) GM-09-00325 */
                                                //무인수납기에서 후수납(오픈카드대상일경우)카드입력값을 키보드로 인식
                                                //dr["InsMeth"]	=	"2";				// KIOSK는 할부 처리한다. 
                                                //if (TRACK_DATA.Length <= 25)
                                                //{
                                                //    dr["InsMeth"] = "1";	// 키보드(오픈카드,후수납) 
                                                //}
                                                //else
                                                //{
                                                //--
            dr["InsMeth"] = "2";	// 카드리더기 
            //}
            /* TODO : YUMC_Change_End(2009.10.30, By 장근주) GM-09-00325 */
            dr["CrdData"] = TRACK_DATA;					// KI0SK에서 신용카드를 Swipe 한 데이터 
            dr["ChosNo"] = admiAcptList.Tables["AdmiPat"].Rows[0]["ChosNo"].ToString();	// 내원번호 [ChosNo]

            _acptItemDT.Rows.Add(dr);
            _acptItemDT.AcceptChanges();
            dr = null;

            #endregion

            //--무인 수납기도 쓸수 있으니 일단 주석안하고 놔둔다 외래 수납이 없으면 알아서 빠질수 있게
            // ●4. ※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※
            //	kiosk에서 받은 수납 정보를 기초로 금액 데이터 (현금/카드) 데이터에 대한 기초 정보를 만듭니다. 
            //	외래데이터 입니다. 

            this.ErrorMsg = "ADMI_MAKEDT_4";

            #region ◎	외래수납 건에 대한 수납 정보 생성 	[_acptItemDT]
            //모바일-모바일에서 외래 일단 주석
            //if (pDscAcptYn == "Y" && admiAcptList.Tables.Contains("Ord"))
            //{
            //    string ambchosno = string.Empty;

            //    int iambCrdAmt = 0;		//	카드승인금액
            //    int iambCashAmt = 0;		//	현금승인금액

            //    string p_CARD_NO = string.Empty;
            //    string p_VldTh = string.Empty;
            //    string p_INSTMCNT = string.Empty;
            //    string p_TRACK_DATA = string.Empty;

            //    // 퇴원수납일 경우 
            //    foreach (DataRow aRow in admiAcptList.Tables["Ord"].Rows)
            //    {
            //        #region 외래건을 돌리면서 데이터 생성
            //        // 선택된 것만 얻어온다. 
            //        //	StusGb 
            //        if (Convert.ToInt32(aRow["StusGb"]) == 1)
            //        {
            //            // 새로운 외래건이므로 
            //            dr = _acptItemDT.NewRow();

            //            #region Old Source
            //            // 카드이면서 내셔야 할 금액이 1000 원 이상일 경우 
            //            //						if ( AcptGb == "CARD" 
            //            //							&& Convert.ToInt32( aRow["PayAmt"] ) >= 1000
            //            //							)
            //            //						{
            //            //							// 카드 
            //            //							iambCashAmt = 0; 
            //            //							iambCrdAmt = Convert.ToInt32( aRow["PayAmt"] );
            //            //
            //            //							dr["CrdAppYn"] ="Y";	// 신용카드 승인요청 여부 
            //            //
            //            //							p_CARD_NO		= CARD_NO; 
            //            //							p_VldTh			= VldTh; 
            //            //							p_INSTMCNT		= INSTMCNT; 
            //            //							p_TRACK_DATA	= TRACK_DATA; 
            //            //
            //            //						}
            //            #endregion

            //            #region New Source
            //            /* 2010.07.21 장근주 신용카드 1000원미만승인불가에서 1원미만승인불가로 변경*/
            //            if (AcptGb == "CARD"
            //                && Convert.ToInt32(aRow["PayAmt"]) >= 1
            //                )
            //            {
            //                // 카드 
            //                iambCashAmt = 0;
            //                iambCrdAmt = Convert.ToInt32(aRow["PayAmt"]);

            //                dr["CrdAppYn"] = "Y";	// 신용카드 승인요청 여부 

            //                p_CARD_NO = CARD_NO;
            //                p_VldTh = VldTh;
            //                p_INSTMCNT = INSTMCNT;
            //                p_TRACK_DATA = TRACK_DATA;

            //            }
            //            #endregion
            //            else
            //            {
            //                // 현금 
            //                iambCrdAmt = 0;
            //                iambCashAmt = Convert.ToInt32(aRow["PayAmt"]);

            //                dr["CrdAppYn"] = "N";	// 현금결재를 한다. 

            //                p_CARD_NO = string.Empty;
            //                p_VldTh = string.Empty;
            //                p_INSTMCNT = string.Empty;
            //                p_TRACK_DATA = string.Empty;
            //            }

            //            ambchosno = aRow["ChosNo"].ToString().Trim();	// 외래내원번호 

            //            dr["SubYn"] = string.Empty;		// KIOSK에서 계정처리를 없다
            //            dr["AcptItem"] = "외래예약비";
            //            dr["TotalAmt"] = Convert.ToInt32(aRow["PayAmt"]);	// 외래 내셔야 할 금액 [PayAmt]
            //            dr["RemainAmt"] = 0;		// 외래 내셔야 할 금액 [PayAmt]이 ALL IN 하므로 잔액은 0 원 

            //            dr["CrdNo"] = p_CARD_NO;		// 카드번호
            //            dr["VldThru"] = p_VldTh;		// 유효기간 .	일단 빈값
            //            dr["InstMcnt"] = p_INSTMCNT;				// 할부기간	.	일단 일시불 

            //            dr["PermAmt"] = iambCrdAmt;		// 결재금액	.	일단 내셔야할 금액으로 
            //            dr["CashAmt"] = iambCashAmt;		// 현금결재금액
            //            dr["CardAccCd"] = string.Empty;		// 카드 결재 코드
            //            dr["ItemTag"] = "amb";
            //            /* TODO : YUMC_Change_Start(2009.10.30, By 장근주) GM-09-00325 */
            //            //무인수납기에서 후수납(오픈카드대상일경우)카드입력값을 키보드로 인식
            //            //dr["InsMeth"]	=	"2";				// SWIPE 
            //            if (TRACK_DATA.Length <= 25)
            //            {
            //                dr["InsMeth"] = "1";	// 키보드(오픈카드,후수납) 
            //            }
            //            else
            //            {
            //                dr["InsMeth"] = "2";	// 카드리더기 
            //            }
            //            /* TODO : YUMC_Change_End(2009.10.30, By 장근주) GM-09-00325 */
            //            dr["CrdData"] = p_TRACK_DATA;					// KI0SK에서 신용카드를 Swipe 한 데이터 

            //            dr["ChosNo"] = ambchosno;		// 내원번호 [ChosNo]
            //            _acptItemDT.Rows.Add(dr);			// DataRow 

            //            dr = null;
            //        }
            //        // foreach 
            //        #endregion
            //    }
            //}

            #endregion

            // ●5. ※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※
            //	추가 사항입니다. 

            this.ErrorMsg = "ADMI_MAKEDT_5";

            #region ◎ 추가 상황

            ht.Add("HidIpEmGb", "I");		// 무인수납기는 일단 다 입원[I] 으로 
            ht.Add("HidTabGb", "D");		// 모두 입금 
            ht.Add("VldYn", "Y");		// VldYn ='Y' 
            ht.Add("OnlnDepGb", "");			// 온라인 입금이 아니면 빈칸

            ht.Add("CashAcptAmt", iCashAmt);		// 현금결제 금액
            ht.Add("CrdAcptAmt", iCardPermAmt);		// 카드결제 금액

            ht.Add("RemitYmd", System.DBNull.Value);						// 송금일자 [ KIOSK는 이런 기능 없다.. 생기기 전에 탈출한다 ! ] 
            ht.Add("Remitter", System.DBNull.Value);						// 송금자   [ 행여나/// ]
            ht.Add("CashBilIFNo", System.DBNull.Value);						// 현금영수증	[ 글쎄... 설마... ]

            ht.Add("DistCd", admiAcptList.Tables["AdmiPat"].Rows[0]["DistCd"]);	//	구역코드
            ht.Add("DeptCd", admiAcptList.Tables["AdmiPat"].Rows[0]["DeptCd"]);	//	진료부서코드

            // 중간수납일 경우에는 단수 차액은 0 원 
            // 퇴원수납일 경우에는 단수 차액은 계산을 한 값 
            //		AdmiAcuracc.Columns.Contains("DscFractionAmt")	
            if (pDscAcptYn == "Y")
                ht.Add("FractionAmt", Convert.ToInt32(admiAcptList.Tables["AdmiAcuracc"].Rows[0]["DscFractionAmt"]));
            else
                ht.Add("FractionAmt", 0);

            #endregion

            // 영수증에 대한 RetrSeq를 조회합니다. 
            //HIS.Facade.HP.ZZZ.ZZZFacade _oZZZFacade = new HIS.Facade.HP.ZZZ.ZZZFacade();
            Hashtable kioskpht = new Hashtable();

            //kioskpht.Add("DomainNm", IFUtil.GetIPAddr());
            //kioskpht.Add("Table", "HP_KIOSKBillInfo");


            //--모바일은 이렇게
            kioskpht.Add("DomainNm", "MOBILE");
            kioskpht.Add("Table", "HP_KIOSKOrd");

            int iBillInfoRetrMaxSeqNo = _ZZZFacade.KIOSK_MAX_RetrSeq(kioskpht);


            // 입원수납을 시작합니다. 
            //DataTable admiBillData = DoadmiAcpt(ht, _acptItemDT, pHosCd, pKioskId, pDscAcptYn, pTotalPayAmtBilYn);
            DataTable admiBillData = DoadmiAcpt(ht, _acptItemDT, pHosCd, pKioskId, pDscAcptYn, pTotalPayAmtBilYn, crdHt);//모바일

            //★★★★★모바일 - 위에입원수납시 오류일경우 리턴★★★★★
            if (rtnResult.Rows[0]["ResultCd"].ToString() == "N")
            {
                return billDs;
            }

            // 리턴을 받은 admiBillData 는 이미 가공된 Dt 입니다. 

            // 입원영수증 데이터를 호출합니다. 
            #region ※ 입원 영수증 [HP_KIOSKBillInfo]

            string stradmichosno = string.Empty;
            string strbillno = string.Empty;
            string strDscYn = string.Empty;

            string billChosNo1 = string.Empty;
            string billBilNo1 = string.Empty;

            this.ErrorMsg = "AMB_MAKE_ADMI_BILLDT";

            try
            {
                // admiBillData : 입원영수증 데이블
                if (admiBillData != null
                    && admiBillData.Rows.Count > 0
                    && admiBillData.Rows.Count > 0)
                {
                    #region ■ 주석 처리

                    //					stradmichosno = admiBillData.Rows[0]["ChosNo"].ToString().Trim();	// 내원 번호 
                    //					strbillno	= admiBillData.Rows[0]["BillNo"].ToString().Trim();	// 영수증 번호 
                    //					strDscYn	= admiBillData.Rows[0]["DscGb"].ToString().Trim();	// 퇴원수납 여부
                    //	
                    //					HIS.Facade.HP.ZZZ.ZZZFacade _oFacade = new HIS.Facade.HP.ZZZ.ZZZFacade(); 
                    //
                    //					DataTable admiBillDt 
                    //						= _oFacade.GET_KIOSK_BILLDATA( "I", strDscYn, stradmichosno, strbillno, "", "", pHosCd, IFUtil.GetIPAddr(), pKioskId, iBillInfoRetrMaxSeqNo); 
                    //
                    //					if ( admiBillDt != null && admiBillDt.Rows.Count > 0 )
                    //					{
                    //						if ( billDs.Tables.Contains("AmbBillData") )
                    //						{
                    //							admiBillDt.Rows[0]["AmbBillYn"] = "Y"; 
                    //						}
                    //
                    //						billDs.Merge(admiBillDt); 
                    //					}
                    #endregion


                    billChosNo1 = admiBillData.Rows[0]["ChosNo1"].ToString().Trim();
                    billBilNo1 = admiBillData.Rows[0]["BilNo1"].ToString().Trim();

                    // 입원영수증 테이블을 저장합니다. 									
                    //	HP_KIOSKBillInfo					

                    INSERT_HP_KIOSKBillInfo(
                        "MOBILE" //IFUtil.GetIPAddr()
                        , iBillInfoRetrMaxSeqNo
                        , billBilNo1
                        , billChosNo1
                        , ""
                        , ""
                        , pKioskId
                        , admiBillData
                        , "I"
                        );

                    //모바일
                    rtnResult.Rows[0]["BillNo"] = billBilNo1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            #endregion

            DataTable dtAdmiBillData = new DataTable("AdmiBillData");
            dtAdmiBillData.Columns.Add("BillNo", typeof(string));
            dtAdmiBillData.Columns.Add("ChosNo", typeof(string));
            dtAdmiBillData.Columns.Add("DscYn", typeof(string));

            DataRow admiBilRow = dtAdmiBillData.NewRow();
            admiBilRow["BillNo"] = billBilNo1;
            admiBilRow["ChosNo"] = billChosNo1;
            admiBilRow["DscYn"] = pDscAcptYn;
            dtAdmiBillData.Rows.Add(admiBilRow);

            DataSet pBilDs = new DataSet();
            pBilDs.Merge(dtAdmiBillData);

            /*
            if (admiAcptList.Tables.Contains("Ord"))
            {

                this.ErrorMsg = "AMB_RSV_MAKE_INFO";

                string strChosno = string.Empty;

                HIS.WinUI.HP.AFE.AFEController _oafecontroller = null;

                string pdeptcd = string.Empty;

                DataSet tmpErrDt = new DataSet();

                string strCheck = string.Empty;
                int ICheck = 0;

                #region ◆ DeBug	○
                // ◎ DeBug ★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★				

                //				foreach( DataRow ordrow in admiAcptList.Tables["Ord"].Rows )
                //				{
                //					StreamWriter _fs = new StreamWriter("C:\\OCSCardLog\\DEBUG.txt",true);	
                //					_fs.WriteLine( ordrow["StusGb"].ToString() ); 
                //					_fs.Close(); 
                //				}
                #endregion

                // ★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆

                // 외래 내원 건의 선택여부를 업데이트 합니다. 
                foreach (DataRow ordrow in admiAcptList.Tables["Ord"].Rows)
                {
                    ordrow["StusGb"] = 0;
                }

                if (SelectedAmbChosNo == null) SelectedAmbChosNo = string.Empty;

                string[] ambSelectdChosNo = SelectedAmbChosNo.Split(',');
                DataRow[] selectedAmbRow = null;

                if (ambSelectdChosNo != null)
                {
                    for (int i = 0; i < ambSelectdChosNo.Length; i++)
                    {
                        if (ambSelectdChosNo[i] == null ||
                            ambSelectdChosNo[i].Trim().Equals("")) continue;

                        selectedAmbRow = admiAcptList.Tables["Ord"].Select("ChosNo ='" + ambSelectdChosNo[i].Trim() + "'");

                        if (selectedAmbRow.Length > 0)
                        {
                            selectedAmbRow[0]["StusGb"] = 1;
                        }
                    }
                }

                // 수납 전체에 대한 COUNT 
                int AmbObjectCnt = admiAcptList.Tables["Ord"].Rows.Count;

                // 수납완료 건 수 
                int AmbObjAcptCmplt = 0;

                foreach (DataRow ordrow in admiAcptList.Tables["Ord"].Rows)
                {
                    #region 주석처리

                    //					strCheck = (ordrow["StusGb"] == null && ordrow["StusGb"] == DBNull.Value ) ? "0" : ordrow["StusGb"].ToString().Trim(); 
                    //
                    //					ordrow["StusGb"] = 0; 
                    //
                    //					switch( strCheck )
                    //					{
                    //						case "0" :
                    //						case "48" :
                    //							ordrow["StusGb"] = 0; 
                    //							break; 
                    //
                    //						case "1" :
                    //						case "49" : 
                    //							ordrow["StusGb"] = 1; 
                    //							break; 
                    //					}
                    #endregion


                    // 선택이 되었을 경우에만
                    if (Convert.ToInt32(ordrow["StusGb"]) == 1)
                    {
                        #region	■	DEBUG
                        //						StreamWriter _fs = new StreamWriter("C:\\OCSCardLog\\DEBUG.txt",true);	
                        //						_fs.WriteLine( ordrow["StusGb"].ToString() ); 
                        //						_fs.Close(); 
                        #endregion

                        strChosno = ordrow["ChosNo"].ToString().Trim();
                        pdeptcd = ordrow["ClnDeptCd"].ToString().Trim();

                        // 외래수납을 시작합니다. 
                        DataSet ambAcptList = SetAmbDs(admiAcptList, _acptItemDT, pHosCd, pKioskId, strChosno);

                        // 입원수납을 종료하고, 외래수납을 실행합니다. 
                        _oafecontroller = new AFEController();

                        // 무인수납을 체크할까??
                        _oafecontroller.SetCheckAcptPsbl(false);		//	무인수납 불가 대상 체크
                        _oafecontroller.SetCheckReCalCompare(false);	//	진료비 재계산 후 비교 체크 

                        #region ■ DEBUG
                        //						_fs = new StreamWriter("C:\\OCSCardLog\\DEBUG.txt",true);	
                        //						_fs.WriteLine( "수납진행합니다. " + strChosno ); 
                        //						_fs.Close(); 
                        #endregion

                        this.ErrorMsg = "AMB_RSV_START_RECEIPT";

                        acptIFCracker = null;
                        acptIFCracker = new IFCracker(admiAcptList.Tables["AdmiPat"].Rows[0]["UnitNo"].ToString().Trim()
                                        , strChosno, "0");

                        // 외래수납을 진행합니다. 	
                        DataTable ambDt = _oafecontroller.SaveAcptCalc(ambAcptList, pHosCd,
                            admiAcptList.Tables["AdmiPat"].Rows[0]["UnitNo"].ToString().Trim(),
                            pdeptcd, getWindId(pHosCd), IFUtil.GetIPAddr(), pKioskId
                            , ref acptIFCracker
                            );

                        #region ■ DEBUG

                        //						StreamWriter _fs = new StreamWriter("C:\\OCSCardLog\\DEBUG.txt",true);	
                        //						_fs.WriteLine( "수납종료를 합니다. " + strChosno 
                        //							+", " + ambAcptList.Tables.Count + " , " + pHosCd +" , " +
                        //							admiAcptList.Tables["AdmiPat"].Rows[0]["UnitNo"].ToString().Trim() + " , "
                        //							+ pdeptcd +" , "+ getWindId( pHosCd ) +" , "+ IFUtil.GetIPAddr() + ", "+ pKioskId + " , " 
                        //							+ Convert.ToInt32( ordrow["StusGb"] ) + " , " + Convert.ToBoolean(ordrow["StusGb"])
                        //							+ ", " + ambDt.Rows.Count +"개"  
                        //							); 
                        //						_fs.Close(); 
                        #endregion


                        // ◎ ●⊙●⊙●⊙●⊙●⊙●⊙●⊙●⊙●⊙
                        if (ambDt.TableName == "ERROR_MSG")
                        {	// 에러는 에러 테이블에 
                            tmpErrDt.Merge(ambDt);

                            throw new Exception(
                                "###총 " + AmbObjectCnt + 1 + "건 중 " + AmbObjAcptCmplt + "건이 수납완료되었습니다. 영수증을 확인하셔요."
                                + "■외래수납 중 오류 발생[" + ambAcptList.Tables["Ord"].Rows[0]["ClnYmdDT"] + "/"
                                + ambAcptList.Tables["Ord"].Rows[0]["ClnDeptCd"]
                                + "]");
                        }
                        else
                        {
                            //2015.09.09 전길준 통검 자동접수 관련사항 별도 트랜잭션으로 분리
                            HIS.Facade.HP.FEE.FEEFacade _oFacade = new HIS.Facade.HP.FEE.FEEFacade();
                            try
                            {
                                _oFacade.SaveOPDAutoExRcpt_NewTx(ambDt, strUnitNo);
                            }
                            catch (Exception ex)
                            {
                                try
                                {
                                    //통검 자동접수 오류시 PgmLog남기기
                                    string tLogDesc = ex.Message;
                                    _oFacade.LogWrite(strChosNo, strUnitNo, "AutoExRcpt", DateTime.Now.ToString("yyyyMMdd"), "", "통검자동접수 오류", tLogDesc, strKioskID, "", "", "ADMIKIOSK", "");
                                }
                                catch (Exception Ex)
                                {
                                }
                            }

                            //2016.01.27 전길준 외래수납 완료 후 ERP I/F 물품테이블 처리를 신규 진료 Sp 호출로 변경 및 트랜잭션 분리
                            //2016.03.23 전길준 외래수납 완료 후 ERP I/F 물품테이블 여러건 처리
                            try
                            {
                                _oFacade.UpdateMMReqOwarhAcptGb(ambDt);
                            }
                            catch (Exception ex)
                            {
                                try
                                {
                                    //ERP물품처리 오류시 PgmLog남기기
                                    string tLogDesc = ex.Message;
                                    _oFacade.LogWrite(strChosNo, strUnitNo, "MMReqOwarh", Convert.ToString(ambAcptList.Tables["Ord"].Rows[0]["ClnYmdDT"]), "", "ERP물품처리 오류", tLogDesc, strKioskID, "", "", "ADMIKIOSK", "");
                                }
                                catch (Exception Ex)
                                {
                                }
                            }

                            AmbObjAcptCmplt++;		// 수납완료 건수를 증가 시킵니다. 

                            ambDt.TableName = "AmbBillData";
                            pBilDs.Merge(ambDt);
                            // 수납후 뒷처리 작업을 합니다. 
                            // HP_KIOSKOrd 테이블에 
                            //					BilNo을 업데이트 합니다. 


                            // 신촌 StusGb : -999, 강남 StusGb : 1
                            int iStusGb = -999;
                            // 2012.06.20 광주추가
                            if (pHosCd == "10" || pHosCd == "20" || pHosCd == "30") //용인추가
                            {
                                iStusGb = 1;
                            }

                            if (ambDt.Rows.Count > 0)
                            {
                                UPDATE_HP_KIOSKOrd(IFUtil.GetIPAddr(), iStusGb, "X", ambDt.Rows[0]["ChosNo"].ToString().Trim()
                                    , "X", "X", "X", -999, ambDt.Rows[0]["BillNo"].ToString().Trim(), "X", pKioskId);

                                this.ErrorMsg = "AMB_MAKE_AMB_BILLDT";		// 외래 테이블 관련 

                                #region  무인수납기 외래 영수증 데이터 처리

                                // [2006.08.11] ○○○○○○○○○○○○○○○○○○○○○○○○○○○○○○○○○○○○○○
                                // 외래 영수증 데이터 작업을 합니다. 
                                //	변수: pBilDs	AmbBillData
                                //	RetrSeq 는 동일합니다 : iBillInfoRetrMaxSeqNo 


                                // 영수증 번호로 무인수납 영수증 조회를 합니다. 
                                // 무인수납기가 인식할 수 있도록 변환을 합니다.

                                DataTable MacptBillData = _oZZZFacade.GET_KIOSK_BILLDATA(
                                        "O", "N"
                                        , ambDt.Rows[0]["ChosNo"].ToString().Trim()
                                        , ambDt.Rows[0]["BillNo"].ToString().Trim()
                                        , "", ""
                                        , pHosCd
                                        , IFUtil.GetIPAddr()
                                        , strKioskID
                                        , ""
                                        );


                                // 영수증 테이블에 내원번호와 영수증 번호를 넣습니다. 
                                // 입원영수증 테이블 입니다. 
                                if (MacptBillData.Rows.Count > 0)
                                {
                                    MacptBillData.Rows[0]["ChosNo1"] = ambDt.Rows[0]["ChosNo"].ToString().Trim();
                                    MacptBillData.Rows[0]["BilNo1"] = ambDt.Rows[0]["BillNo"].ToString().Trim();
                                    MacptBillData.Rows[0]["ChosNo2"] = "";
                                    MacptBillData.Rows[0]["BilNo2"] = "";
                                }

                                // AmbBillData

                                INSERT_HP_KIOSKBillInfo(IFUtil.GetIPAddr()
                                    , iBillInfoRetrMaxSeqNo
                                    , MacptBillData.Rows[0]["BilNo1"].ToString().Trim()
                                    , MacptBillData.Rows[0]["ChosNo1"].ToString().Trim()
                                    , ""
                                    , ""
                                    , strKioskID
                                    , MacptBillData
                                    , "O"	// 외래 
                                    );

                                #endregion
                            }

                        }
                        // ◎ ●⊙●⊙●⊙●⊙●⊙●⊙●⊙●⊙●⊙
                    }
                }

            }*/

            // ◎ 수납 후 영수증 처리를 진행합니다. ※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※
            // pBilDs : 외래 수납 영수증 
            if (pBilDs.Tables.Count > 0) billDs.Merge(pBilDs);	// 외래 수납 영수증 테이블 



            // 외래의 경우 : AmbBillData
            // 입원의 경우 : AdmiBillData
            // AmbBillYn */
            return billDs;
        }

        #region ● 입원 수납 시작 메소드 DoadmiAcpt()

        /// <summary>
        /// 입원수납을 시작합니다. 
        ///		
        ///		1.	신용카드 결재 처리를 위한 처리 세트 
        ///		
        ///		2.	입원 수납 신용카드 결재 처리 
        ///				-- 신용카드 승인, 취소 처리 
        ///				
        ///		3.	입원 수납 OCS 처리 
        /// </summary>
        private DataTable DoadmiAcpt(
            Hashtable pht,
            DataTable _acptItemDT,
            string pHosCd,
            string strKioskID,
            string pDscAcptYn,
            string pTotalPayAmtBilYn //	전액수납 여부 체크 
            , Hashtable crdHt//모바일
            )
        {
            // [	pht --> KEY 값	]

            //	WindId		창구 ID 
            //	HosCd		병원코드 
            //	RgtId		KIOSK ID
            //	ChosNo		내원번호 
            //	DscCalcYmd	DscCalcYmd 은 퇴원계산일자 
            //	ChosGb		I
            //	UnitNo		등록번호 
            //	AcptGb		0 [중간수납] / 2 [퇴원수납]
            //	AmbPrftYn	외래수입여부 [ Y / N ]
            //	HidIpEmGb	I 	[입원]
            //	HidTabGb		D	[입금]
            //	VldYn		Y
            //	OnlnDepGb	""	[온라인구분 -> 모두 빈값]
            //	CashAcptAmt	현금수납액
            //	CrdAcptAmt	카드결재요청액
            //	DistCd		구역코드 
            //	DeptCd		부서코드 
            //	FractionAmt	단수차액

            int admiCrdAcptAmt = 0;	// 입원수납총액
            int admiCashAcptAmt = 0;	// 현금수납총액 

            string AcptCmpYn = "N";

            // DataTable pAdmiBillDt = new DataTable("AdmiBillDT"); 

            DataSet IFNoDS = new DataSet();

            // 입원카운트를 얻어온다. 			
            // 신용카드 결재 건수를 얻어옵니다. 
            int icntCrdIFNo = _acptItemDT.Select(" ItemTag='admi' AND CrdAppYn='Y' ").Length;


            this.OCS_CARD_LISTDT = null;
            // getAdmiCrdDt() 호출 
            this.OCS_CARD_LISTDT = getAdmiCrdDt();


            ///신용카드 승인여부 
            this.isExsitPermCrd = false;

            DataTable MacptBillData = new DataTable();
            //HIS.Facade.HP.ZZZ.ZZZFacade zFacade = new HIS.Facade.HP.ZZZ.ZZZFacade();
            //HIS.Facade.HP.FEE.FEEFacade oFacade = new HIS.Facade.HP.FEE.FEEFacade();

            string sBilNo = string.Empty;

            //	◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇
            //	● 입원수납 시 신용카드 처리를 할 내역이 있을 경우

            try
            {

                #region 신용카드 처리를 할 내역이 있을 경우의 처리 InsertAdmiCrdDT 메소드 호출

                if (icntCrdIFNo > 0)
                {
                    // 현금,카드 IF번호를 채번한다.
                    IFNoDS = GetIFNo(0, icntCrdIFNo);

                    for (int i = 0; i < _acptItemDT.Rows.Count; i++)
                    {
                        if (_acptItemDT.Rows[i]["ItemTag"].ToString() == "admi"
                            && _acptItemDT.Rows[i]["CrdAppYn"].ToString() == "Y"
                            )
                        {
                            //	● 입원수납 총액을 구합니다. 
                            //	KIOKS는 카드를 1개만 사용을 하므로, 총액을 구할 필요는 사실상 없지만,
                            //	차후 확장(카드 2개이상을 사용하는 경)을 위해 OCS와 소스를 맞춥니다. 
                            admiCrdAcptAmt += int.Parse(_acptItemDT.Rows[i]["PermAmt"].ToString());

                            //	●	승인을 얻어서 DTO 데이터 셋에 추가를 합니다. 
                            bool rnt = InsertAdmiCrdDT("admi",
                                _acptItemDT.Rows[i],
                                int.Parse(IFNoDS.Tables["CrdIF"].Rows[i][0].ToString()),
                                pHosCd,
                                pht,
                                strKioskID,
                                ref OCS_CARD_LISTDT
                                , crdHt
                                );

                            //모바일
                            if (rnt)
                            {
                                rtnResult.Rows[0]["ResultCd"] = "Y";
                                rtnResult.Rows[0]["ErrorMsg"] = "";
                            }
                            else
                            {
                                rtnResult.Rows[0]["ResultCd"] = "N";
                                rtnResult.Rows[0]["ErrorMsg"] = "수납 처리 실패 하였습니다.";
                            }
                        }
                    }

                }
                #endregion

                //	◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇

                this.ErrorMsg = "ADMI_MAKE_CASHDATA";

                //	◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇
                //	● 입원수납 시 현금수납 내역 데이터를 만듭니다. 

                int iCashCnt = _acptItemDT.Select(" ItemTag='admi' AND CrdAppYn <> 'Y' ").Length;

                if (iCashCnt > 0)	// 현금액 건수를 구합니다. 
                {
                    for (int i = 0; i < _acptItemDT.Rows.Count; i++)
                    {
                        if (_acptItemDT.Rows[i]["ItemTag"].ToString() == "admi"
                            && _acptItemDT.Rows[i]["ItemTag"].ToString() != "Y"
                            )
                        {
                            //	● 입원수납 총액을 구합니다. 
                            //	KIOKS는 카드를 1개만 사용을 하므로, 총액을 구할 필요는 사실상 없지만,
                            //	차후 확장(카드 2개이상을 사용하는 경)을 위해 OCS와 소스를 맞춥니다. 
                            admiCashAcptAmt += int.Parse(_acptItemDT.Rows[i]["CashAmt"].ToString());
                        }
                    }
                }

                // 주석처리 
                // pht["CrdAcptAmt"] = admiCrdAcptAmt; 
                // pht["CashAcptAmt"] = admiCashAcptAmt; 

                //	◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇◆◇


                DataSet AcptDTO = new DataSet();
                AcptDTO.Merge(OCS_CARD_LISTDT);	// 신용카드 정보 Dt :	AdmiCrdDT 
                AcptDTO.Merge(getCashDt());		// 현금영수증 Dt : AdmiCashBilIFDT 

                this.ErrorMsg = "ADMI_ACPT_OCSSAVE";

                // OCS에 저장을 보냅니다.

                sBilNo = _FEEFacade.AdmiAcptCmpIt_AdmiAcptTx(pht, AcptDTO);



            }
            catch (Exception ex)
            {
                /*
                //	● 1. WORK
                //	신용카드 승인을 얻은 후 에러 발생 시 
                //	기존의 승인내역에 대해 취소 요청을 보낸다. 

                if (this.isExsitPermCrd)
                {
                    CancelAdmiCrdDT(
                        pht["ChosNo"].ToString().Trim(),
                        pht["UnitNo"].ToString().Trim(),
                        pHosCd,
                        "I",	// [??] 응급으로 분리되어야 할 필요가 있을까?? 
                        pht,
                        strKioskID
                        );

                    this.isExsitPermCrd = false;

                    if (OCS_CARD_LISTDT != null)
                    {
                        OCS_CARD_LISTDT.Clear();
                    }
                }
                //	● 2. WORK 
                //	에러 메시지를 넘겨 받아서, KIOSK에 조회될 수 있도록 처리한다. 
                throw ex;
                 */
                //모바일 에러처리
                rtnResult.Rows[0]["ResultCd"] = "N";
                rtnResult.Rows[0]["ErrorMsg"] = "모바일 수납 처리 실패 하였습니다";
                return rtnResult;

            }


            // 수납 완료 SIGN을 준다.
            // AcptCmpYn ="Y" 처리되면, 이 이후에 발생한 에러에 대해서는 신용카드 Rollback 처리를 하지 않습니다. 
            AcptCmpYn = "Y";

            this.ErrorMsg = "AMB_MAKE_ADMI_BILLDT";


            // 영수증 번호로 무인수납 영수증 조회를 합니다. 
            // 무인수납기가 인식할 수 있도록 변환을 합니다. 


            MacptBillData = _ZZZFacade.GET_KIOSK_BILLDATA(
                "I", pDscAcptYn, pht["ChosNo"].ToString().Trim(), sBilNo
                , "", "", pHosCd, "MOBILE"//IFUtil.GetIPAddr()
                , strKioskID
                , pTotalPayAmtBilYn
                );

            // 영수증 테이블에 내원번호와 영수증 번호를 넣습니다. 
            // 입원영수증 테이블 입니다. 
            if (MacptBillData.Rows.Count > 0)
            {
                MacptBillData.Rows[0]["ChosNo1"] = pht["ChosNo"].ToString().Trim();
                MacptBillData.Rows[0]["BilNo1"] = sBilNo;
                MacptBillData.Rows[0]["ChosNo2"] = "";
                MacptBillData.Rows[0]["BilNo2"] = "";
            }

            // 수납 후 수납 완료에 대한 처리를 진행합니다. 

            Hashtable afterAcpt = new Hashtable();

            afterAcpt["DomainNm"] = "MOBILE";//IFUtil.GetIPAddr();
            afterAcpt["UnitNo"] = "X";
            afterAcpt["ChosNo"] = "X";
            afterAcpt["ChosGb"] = "X";
            afterAcpt["KIOSKID"] = strKioskID;
            afterAcpt["IdNo"] = "X";
            afterAcpt["DeptCd"] = "X";
            afterAcpt["PatNm"] = "X";
            afterAcpt["UserNm"] = "X";
            afterAcpt["Ward"] = "X";
            afterAcpt["DscCalcYmd"] = "X";
            afterAcpt["Sex"] = "X";
            afterAcpt["DeptNm"] = "X";
            afterAcpt["Room"] = "X";
            afterAcpt["TmpDscYmd"] = "X";
            afterAcpt["EmerCalcTyp"] = "X";
            afterAcpt["DscCalcYn"] = "X";
            afterAcpt["DscAcptYn"] = "X";
            afterAcpt["AmbCount"] = -999;
            afterAcpt["PatChagTotAmt"] = -999;
            afterAcpt["OrdStrYmd"] = "X";
            afterAcpt["ToDay"] = "X";
            afterAcpt["AcptAmt"] = -999;
            afterAcpt["RduAmt"] = -999;
            afterAcpt["TotalPayAmt"] = -999;
            afterAcpt["AmbRsvTotalAmt"] = -999;
            afterAcpt["AmbAdmiSumAmt"] = -999;
            afterAcpt["ReqAcptAmt"] = -999;
            afterAcpt["HowToAcpt"] = "X";
            afterAcpt["CardNo"] = "XXXXXXXXXXXXXX";
            afterAcpt["VldTh"] = "X";
            afterAcpt["INSTMCNT"] = "X";
            afterAcpt["CreditNo"] = "XXXXXXXXXXXXXX";
            afterAcpt["AcptCmplYn"] = "Y";	// 수납여부 
            afterAcpt["BilNo"] = sBilNo;		// 영수증 


            _FEEFacade.UPDATE_HP_KIOSKAdmiPat(afterAcpt);



            MacptBillData.TableName = "AdmiBillDT";
            return MacptBillData;
        }

        #endregion 

        #region ■ IFNo 채번 : GetIFNo
        /// <summary>
        /// IFNo 채번 
        /// </summary>
        /// <param name="pCashIFCount">현금IFNO</param>
        /// <param name="pCrdIFCount">카드IFNO</param>
        /// <returns></returns>
        private DataSet GetIFNo(int pCashIFCount, int pCrdIFCount)
        {
            DataSet ds = null;

            Hashtable ht = new Hashtable();

            ht.Add("CashIFCount", pCashIFCount);
            ht.Add("CrdIFCount", pCrdIFCount);

            //HIS.Facade.HP.FEE.FEEFacade oFacade = new HIS.Facade.HP.FEE.FEEFacade();
            ds = _FEEFacade.SelectFeeNoList(ht);

            return ds;
        }
        #endregion

        #region ◎ 신용카드 승인 : InsertAdmiCrdDT

        /// <summary>
        /// 신용카드 승인 요청 
        /// </summary>
        /// <param name="itemTag">수납구분( admi / amb )</param>
        /// <param name="aRow">수납데이터</param>
        /// <param name="pCrdIFNo">CrdIFNo</param>
        /// <param name="pHosCd">병원코드</param>
        /// <param name="pht">수납기본정보 HashTable</param>
        /// <param name="strKioskId">KIOSK 이름</param>
        /// <param name="pDt">신용카드 승인 내역</param>
        /// <returns></returns>
        private bool InsertAdmiCrdDT(
            string itemTag,
            DataRow aRow,
            int pCrdIFNo,
            string pHosCd,
            Hashtable pht,
            string strKioskId,
            ref DataTable pDt
            , Hashtable crdHt
            )
        {
            bool rtnVal = true;			// 승인여부 
            bool LogWriteYn = true;		// 오류 발생 시 Log 기록 여부 

            /*
            HIS.WinUI.HP.AFE.CrdCachApprovalController _oCrdCach = new CrdCachApprovalController();
            HIS.WinUI.HP.AFE.Payment obj = new HIS.WinUI.HP.AFE.Payment();

            // ● 1.	신용카드 결재 처리를 위한 처리 ※※※※※※※※※※※※※※※※※※※※※※
            // 내원번호는 클래스 안에서 
            _oCrdCach.prtAcptProc = "AdmiAcpt";	// 외래수납
            _oCrdCach.prtApprovalCancelGb = "Approval"; // 승인
            _oCrdCach.prtChosNo = pht["ChosNo"].ToString().Trim();
            _oCrdCach.prtUnitNo = pht["UnitNo"].ToString().Trim();
            _oCrdCach.prtCrdCashGb = "Card";
            _oCrdCach.prtBilNo = "";
            _oCrdCach.strHosCd = pHosCd;
            _oCrdCach.prtLogNm = "KIOSK,입원,카드승인요청";


            //	● 2.	Payment 클래스 내에서의 처리를 합니다.  ※※※※※※※※※※※※※※※※※※※※※※
            //	Payment 내에서의 처리 
            SetPayment(ref obj, pHosCd, "I");
            */

            // ● 3.	신용카드 결재 처리를 위한 처리 ※※※※※※※※※※※※※※※※※※※※※※
            //		●	3.1.	무인수납기는 계정처리가 없으므로, 계정처리 여부는 파악하지 않습니다. 
            bool rnt = false;	// 일단 FALSE!

            #region Old Source
            //	★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆
            //		●	3.2.	 결재금액이 1000 원 미만인지를 확인합니다. 
            //			this.ErrorMsg = "ADMI_CARD_PERM_UNDER_1000" ; 
            //
            //			if (Convert.ToInt32(aRow["PermAmt"].ToString().Trim()) < 1000  )
            //			{
            //				throw new Exception("승인요청금액 1000원미만 오류"); 
            //				return rnt; 
            //			}
            #endregion

            #region New Source
            /* 2010.07.21 장근주 신용카드 1000원미만승인불가에서 1원미만승인불가로 변경*/
            //	★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆
            //		●	3.2.	 결재금액이 1 원 미만인지를 확인합니다. 
            this.ErrorMsg = "ADMI_CARD_PERM_UNDER_1";

            if (Convert.ToInt32(aRow["PermAmt"].ToString().Trim()) < 1)
            {
                throw new Exception("승인요청금액 1원미만 오류");
                return rnt;
            }
            #endregion

            //	★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆
            //		●	3.3.	 할부적용을 할 때, 5000 원 미만인지를 확인합니다. 
            this.ErrorMsg = "ADMI_CARD_VldThru_UNDER_5000";

            if (Convert.ToInt32(aRow["PermAmt"].ToString().Trim()) < 50000
                && aRow["InstMcnt"].ToString().Trim() != "00"
                )
            {
                throw new Exception("할부적용 50000원 미만오류");
                return rnt;
            }

            this.ErrorMsg = "ADMI_CARD_PERMMIT";
            /* 모바일에서 할 필요가 없어보인다.
            if (!File.Exists("AccPerm"))	// 계정처리한다. // 테스트가 아닐 경우에만 처리한다. 
            {

                //if (obj.GetSectionNo(pHosCd, "I") != "999")
                //{
                //    StreamWriter
                //    _fs = new StreamWriter("C:\\OCSCardLog\\DEBUG.txt", true);
                //    _fs.WriteLine("섹션 번호가 999가 아니다.");
                //    _fs.Close();
                //    return false;
                //}

                // ● 4. 승인요청을 때리고 
                obj.CardAdmit(pht["UnitNo"].ToString().Trim()
                    , "I"
                    , pht["DeptCd"].ToString().Trim()
                    , strKioskId
                    , aRow["CrdNo"].ToString().Trim()
                    , aRow["VldThru"].ToString().Trim()
                    , aRow["InstMcnt"].ToString().Trim()
                    , int.Parse(aRow["PermAmt"].ToString().Trim())
                    , pCrdIFNo
                    , aRow["InsMeth"].ToString().Trim()
                    , aRow["CrdData"].ToString().Trim()
                    , obj.GetSectionNo(pHosCd, "I")
                    );

                // 승인요청에 대해 성공/취소 여부를 판단한다. 
                _oCrdCach.ApprovalYn(ref LogWriteYn, ref rtnVal, ref obj);

                // OCS의 경우에는 
                // 입원 수납 하나에 신용카드 한개이다. 복수개의 신용카드를 사용하지 않는다.
                // 때문에 신용카드 하나를 결재 처리 후, 승인/취소의 여부에 따라 결정된다. 

                if (!rtnVal)
                {
                    this.ErrorMsg = obj.ErrNotice;	// KIOSK에 에러메시지를 공지한다. 
                    throw new Exception(obj.ErrNotice);		// Exception을 Log 에 기록한다. 
                    return rnt;
                }
            }
            */
            // pDt에 OCS에 담을 데이터를 기록합니다. 
            //OCS_CARD_INFO_MAKEDT("admi", aRow, ref obj, ref pDt, pCrdIFNo);
            OCS_CARD_INFO_MAKEDT("admi", aRow, ref pDt, pCrdIFNo, crdHt);

            rnt = true;		// 마지막에 승인처리 
            return rnt;
        }

        #endregion 

        #region ■ OCS에 저장할 신용카드 승인 내역을 만듭니다.

        /// <summary>
        /// OCS 데이터 저장 
        /// </summary>
        //private void OCS_CARD_INFO_MAKEDT(string ItemTag, DataRow aRow, ref HIS.WinUI.HP.AFE.Payment obj, ref DataTable pDt, int pCrdIFNo)
        private void OCS_CARD_INFO_MAKEDT(string ItemTag, DataRow aRow, ref DataTable pDt, int pCrdIFNo, Hashtable crdHt)
        {
            // OCS에 저장할 내역을 기록합니다. 
            #region pDt의 Layout :

            //			DataTable dt = new DataTable("AdmiCrdDT");
            //			dt.Columns.Add("ItemTag");
            //			dt.Columns.Add("CrdIFNo");
            //			dt.Columns.Add("SubYn");
            //			dt.Columns.Add("AccCd");
            //			dt.Columns.Add("InpurcCd");
            //			dt.Columns.Add("CrdIssLoc");
            //			dt.Columns.Add("CrdNo");
            //			dt.Columns.Add("VldThru");
            //			dt.Columns.Add("InstMcnt");
            //			dt.Columns.Add("PermAmt");
            //			dt.Columns.Add("PermYmd");
            //			dt.Columns.Add("PermHms");
            //			dt.Columns.Add("PermNo");
            //			dt.Columns.Add("VanGb");
            //			dt.Columns.Add("SlpNo");
            //			dt.Columns.Add("MbstNo");
            //			dt.Columns.Add("InpurcCoNm");
            //			dt.Columns.Add("InsMeth");
            //			dt.Columns.Add("CrdData");
            //			dt.Columns.Add("CnBfCrdIFNo");
            //			dt.Columns.Add("PermStusGb");
            //			dt.Columns.Add("CnYn");
            //			dt.Columns.Add("CnYmd");
            //			dt.Columns.Add("CnHms");
            //			dt.Columns.Add("AcptCnYn");
            //			dt.Columns.Add("UcoltOccSeq");

            #endregion

            // 2013.01.23 CrdTypGb 추가

            if (pDt.Columns.Contains("CrdTypGb") == false)
            {
                pDt.Columns.Add("CrdTypGb");
            }

            DataRow crdRow = pDt.NewRow();
            crdRow["ItemTag"] = ItemTag;		// "admi"	입원수납
            crdRow["CrdIFNo"] = pCrdIFNo;		//	CRDIFNO	
            crdRow["CrdNo"] = crdHt["CrdNo"].ToString().Trim();			// 카드번호 
            crdRow["VldThru"] = crdHt["VldThru"].ToString().Trim();		// 유효기간 
            crdRow["InstMcnt"] = crdHt["InstMcnt"].ToString().Trim();		// 할부기간 
            crdRow["PermAmt"] = int.Parse(crdHt["PermAmt"].ToString().Trim());	// 승인금액 
            crdRow["InsMeth"] = "2"; //
            crdRow["CrdData"] = crdHt["CrdData"] == null ? "": crdHt["CrdData"].ToString().Trim();
            crdRow["CnBfCrdIFNo"] = System.DBNull.Value;
            crdRow["PermStusGb"] = "P";
            crdRow["CnYn"] = "N";
            crdRow["CnYmd"] = "";
            crdRow["CnHms"] = "";
            crdRow["AcptCnYn"] = "N";
            crdRow["UcoltOccSeq"] = System.DBNull.Value;

            //모바일추가
            crdRow["VanSeqNo"] = crdHt["VanSeqNo"].ToString().Trim();
            crdRow["CrdPermMeth"] = crdHt["CrdPermMeth"].ToString().Trim();
            crdRow["ICPermMeth"] = crdHt["ICPermMeth"].ToString().Trim();
            crdRow["POSNo"] = crdHt["POSNo"] == null ? "" :  crdHt["POSNo"].ToString().Trim();

            if (!File.Exists("AccPerm"))
            {
                crdRow["SubYn"] = "N";				//	KIOSK는 계정처리가 없다. 
                crdRow["AccCd"] = GetAccCd(crdHt["InpurcCd"].ToString().Trim());	//	카드 계정 [승인결과 문장 파싱 결과]
                crdRow["InpurcCd"] = crdHt["InpurcCd"].ToString().Trim();
                crdRow["CrdIssLoc"] = crdHt["CrdIssLoc"].ToString().Trim();
                crdRow["PermYmd"] = crdHt["PermYmd"].ToString().Trim(); 		// 승인날자 
                crdRow["PermHms"] = crdHt["PermHms"].ToString().Trim(); 		// 승인시간
                crdRow["PermNo"] = crdHt["PermNo"].ToString().Trim(); 	// 승인번호 
                crdRow["VanGb"] = crdHt["VanGb"].ToString().Trim(); 		// Van Gb 구분 
                crdRow["SlpNo"] = crdHt["SlpNo"].ToString().Trim(); 		// 매출번호 
                crdRow["MbstNo"] = crdHt["MbstNo"].ToString().Trim();
                crdRow["InpurcCoNm"] = crdHt["InpurcCoNm"] ==null ? "" : crdHt["InpurcCoNm"].ToString().Trim();

                crdRow["CrdTypGb"] = crdHt["CrdTypGb"].ToString().Trim();
            }
            else
            {
                // 테스트 상황에서 계정 처리 
                crdRow["SubYn"] = "Y";
                crdRow["AccCd"] = "S101";			// bc카드 처리 
                crdRow["InpurcCd"] = "BC테스트";
                crdRow["CrdIssLoc"] = "BC카드";
                crdRow["PermYmd"] = string.Empty;		// 승인날자 
                crdRow["PermHms"] = string.Empty;		// 승인시간
                crdRow["PermNo"] = string.Empty;		// 승인번호 
                crdRow["VanGb"] = string.Empty;		// Van Gb 구분 
                crdRow["SlpNo"] = string.Empty;		// 매출번호 
                crdRow["MbstNo"] = string.Empty;
                crdRow["InpurcCoNm"] = string.Empty;

                crdRow["CrdTypGb"] = "";
            }

            pDt.Rows.Add(crdRow);

            pDt.AcceptChanges();

            this.isExsitPermCrd = true;

        }

        #endregion

        #region 영수증 저장 메소드

        /// <summary>
        /// 영수증 저장 메소드 
        /// </summary>
        /// <param name="DOMAIN"></param>
        /// <param name="RetrSeq"></param>
        /// <param name="billNo1"></param>
        /// <param name="ChosNo1"></param>
        /// <param name="billNo2"></param>
        /// <param name="ChosNo2"></param>
        /// <param name="KIOSKID"></param>
        /// <param name="billDt"></param>
        /// <returns></returns>
        public int INSERT_HP_KIOSKBillInfo(
                       string DOMAIN
                       , int RetrSeq
                       , string billNo1
                       , string ChosNo1
                       , string billNo2
                       , string ChosNo2
                       , string KIOSKID
                       , DataTable billDt
                       , string pBillGb
                       )
        {
            int iResult = -1;

            try
            {
                // 입원영수증 테이블을 저장합니다. 									
                //	HP_KIOSKBillInfo
                //HIS.Facade.HP.FEE.FEEFacade _oFacade = new HIS.Facade.HP.FEE.FEEFacade();
                iResult =_FEEFacade.INSERT_HP_KIOSKBillInfo(
                    "MOBILE"
                    , RetrSeq
                    , billNo1
                    , ChosNo1
                    , billNo2
                    , ChosNo2
                    , KIOSKID
                    , billDt
                    , pBillGb
                    );
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return iResult;
        }


        #endregion 

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

        #region ■ 신용카드 승인정보를 담을 데이터 셋 입니다.

        /// <summary>
        /// 신용카드 승인정보를 담을 데이터 셋 입니다. 
        /// </summary>
        /// <returns></returns>
        private DataTable getAdmiCrdDt()
        {
            // 카드
            DataTable dt = new DataTable("AdmiCrdDT");
            dt.Columns.Add("ItemTag");
            dt.Columns.Add("CrdIFNo");
            dt.Columns.Add("SubYn");
            dt.Columns.Add("AccCd");
            dt.Columns.Add("InpurcCd");
            dt.Columns.Add("CrdIssLoc");
            dt.Columns.Add("CrdNo");
            dt.Columns.Add("VldThru");
            dt.Columns.Add("InstMcnt");
            dt.Columns.Add("PermAmt");
            dt.Columns.Add("PermYmd");
            dt.Columns.Add("PermHms");
            dt.Columns.Add("PermNo");
            dt.Columns.Add("VanGb");
            dt.Columns.Add("SlpNo");
            dt.Columns.Add("MbstNo");
            dt.Columns.Add("InpurcCoNm");
            dt.Columns.Add("InsMeth");
            dt.Columns.Add("CrdData");
            dt.Columns.Add("CnBfCrdIFNo");
            dt.Columns.Add("PermStusGb");
            dt.Columns.Add("CnYn");
            dt.Columns.Add("CnYmd");
            dt.Columns.Add("CnHms");
            dt.Columns.Add("AcptCnYn");
            dt.Columns.Add("UcoltOccSeq");

            //[IC카드-2018-1차필수] 
            dt.Columns.Add("VanSeqNo");
            dt.Columns.Add("CrdPermMeth");
            dt.Columns.Add("ICPermMeth");
            dt.Columns.Add("POSNo");
            dt.Columns.Add("CrdTypGb");
            return dt;
        }
        #endregion

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
            return _dt;
        }

        public DataTable MakeAcptResult()
        {
            DataTable Ord = new DataTable("AcptResult");

            Ord.Columns.Add("HosCd");
            Ord.Columns.Add("ResultCd");
            Ord.Columns.Add("ErrorMsg");
            Ord.Columns.Add("BillNo");
            Ord.Columns.Add("HosoOrdNo");
            DataRow eRow = Ord.NewRow();
            Ord.Rows.Add(eRow);

            return Ord;
        }

        private DataTable getCashDt()
        {
            DataTable dt = null;
            dt = new DataTable("AdmiCashBilIFDT");

            dt.Columns.Add("ItemTag");
            dt.Columns.Add("CashBilIFNo");	// 현금영수증번호
            dt.Columns.Add("InpurcCd");
            dt.Columns.Add("IssLoc");
            dt.Columns.Add("SlfYn");
            dt.Columns.Add("InsNoGb");
            dt.Columns.Add("InsNo");
            dt.Columns.Add("PermAmt");
            dt.Columns.Add("PermYmd");
            dt.Columns.Add("PermHMS");
            dt.Columns.Add("PermNo");
            dt.Columns.Add("VanGb");
            dt.Columns.Add("SlpNo");
            dt.Columns.Add("InsMeth");
            dt.Columns.Add("InsData");
            dt.Columns.Add("CnYn");
            dt.Columns.Add("CnPermNo");
            dt.Columns.Add("CnYmd");
            dt.Columns.Add("CnHms");
            return dt;
        }
        /// <summary>
        /// 수납 체크 여부
        /// </summary>
        /// <param name="acptList">입/퇴원 수납 내역</param>
        /// <param name="KIOSKTotPayAmt">KIOSK에서 던진 내역</param>
        /// <param name="strErrMsg">에러메시지</param>
        /// <param name="strLogErrMsg">Log에 기록을 할 에러 메시지</param>
        /// <param name="strLogKey">Log에 기록을 할 Error Key</param>
        /// <returns></returns>
        private bool ChkAcpt(ref DataSet acptList, ref string strLogKey, ref string strErrMsg, ref string strLogErrMsg, int KIOSKTotPayAmt)
        {
            string returnmsg = strErrMsg;
            string pchosno = string.Empty;		// 내원번호 

            pchosno = acptList.Tables["AdmiPat"].Rows[0]["ChosNo"].ToString().Trim();

            int iTotadmiAcptAmt = Convert.ToInt32(acptList.Tables["AdmiAcuracc"].Rows[0]["TotalPayAmt"].ToString());

            #region Old Source
            //			if ( KIOSKTotPayAmt < 1000 ) 
            //			{
            //				strLogKey = "DSC_CNNT_CRDADMIT_1000"; 
            //				strErrMsg = "1000원 미만의 금액은 입금이 불가합니다.";	// 에러 메시지 
            //				strLogErrMsg = "1000원 미만 수납"; 
            //				return false; 
            //			}
            #endregion

            #region New Source
            if (KIOSKTotPayAmt < 1)
            {
                strLogKey = "DSC_CNNT_CRDADMIT_1";
                strErrMsg = "1원 미만의 금액은 입금이 불가합니다.";	// 에러 메시지 
                strLogErrMsg = "1원 미만 수납";
                return false;
            }
            #endregion

            // 환자가 kiosk 화면에서 입력한 수납금액 
            // 수납금액은 10으로 나누어 떨어져야 합니다. 
            if (KIOSKTotPayAmt % 10 != 0)
            {
                strLogKey = "DSC_CNNT_MOD10";
                strErrMsg = "수납금액은 1원 단위일 수 없습니다.";	// 에러 메시지 
                strLogErrMsg = "수납금액 10원 미만 절삭 필요";
                return false;
            }



            if (acptList.Tables["AdmiPat"].Rows[0]["DscAcptYn"].ToString() == "Y")
            {
                // ◎	체크사항	퇴원수납일 경우	◎
                // case 1 : 퇴원 수납 시 퇴원 수납시에 HZ_PgmLog Table에 LogGb='FATAL'에 데이터가 존재 하면 퇴원 수납처리 불가 
                if (DscCalcErrorCheck(pchosno) != "P")
                {
                    strLogKey = "DSC_FATAL";
                    strErrMsg = "퇴원계산내역 오류.";	// 에러 메시지 
                    strLogErrMsg = "퇴원 수납 시 퇴원 수납시에 HZ_PgmLog Table에 LogGb='FATAL'에 데이터가 존재";
                    return false;
                }

                // case 2 : 퇴원 수납을 한 환자일 경우에는 다시 퇴원 수납을 할 수 없도록 한다. 
                if (acptList.Tables["AdmiAcuracc"].Rows[0]["DscAcptYn"].ToString() == "Y")
                {
                    strLogKey = "DSC_ACPTTED";
                    strErrMsg = "이미 퇴원수납 된 환자입니다.";	// 에러 메시지 
                    strLogErrMsg = "퇴원 수납이 되어서 입금불가.";
                    return false;
                }

                // case 3 : 퇴원 수납 시, 입원 수납 금액에 잔여금액이 있을 경우 
                //	입원 금액을 처리합니다. 
                if (iTotadmiAcptAmt > KIOSKTotPayAmt)
                {
                    strLogKey = "DSC_RMD_AMT";
                    strErrMsg = "퇴원수납 시 잔여금이 있으면 안됩니다..";	// 에러 메시지 
                    strLogErrMsg = "퇴원 수납이 잔여금 존재.";
                    return false;
                }
            }
            else
            {
                // 중간수납[??] 

                // 수납금액은 10으로 나누어 떨어져야 합니다. 
                if (KIOSKTotPayAmt % 10 != 0)
                {
                    strLogKey = "DSC_CNNT_MOD10";
                    strErrMsg = "수납금액은 1원 단위일 수 없습니다.";	// 에러 메시지 
                    strLogErrMsg = "수납금액 10원 미만 절삭 필요";
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 퇴원 수납시에 HZ_PgmLog Table에 LogGb='FATAL'에 데이터가 존재 여부 Check
        /// </summary>
        /// <returns></returns>
        private string DscCalcErrorCheck(string pChosNo)
        {
            string ErrorCheck = string.Empty;

            try
            {
                //HIS.Facade.HP.FEE.FEEFacade oFacade = new HIS.Facade.HP.FEE.FEEFacade();
                ErrorCheck = _FEEFacade.SelectPgmLog_AdmiAcptNTx(pChosNo);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ErrorCheck;
        }
    }

    public class MessageOutPut
    {
        public MessageOutPut()
        {
            //
            // TODO: 여기에 생성자 논리를 추가합니다.
            //
            // 메시지 목록 Class 입니다. 
        }

        #region 메시지 목록을 설정합니다. 

        /// <summary>
        /// 등록번호 및 내원번호 조회 시 환자 정보가 없을 경우 
        /// </summary>
        private string NOT_FOUND_PATINFO = "환자정보가 없습니다";

        /// <summary>
        /// 재원 환자의 정보 조회 시 에러 발생 
        /// </summary>
        private string ERR_FIND_PATINFO = "재원정보조회 중 에러발생";

        /// <summary>
        /// 외래 예약건 조회 중 에러 발생 
        /// </summary>
        private string ERR_FIND_AMBACPT = "외래예약건 조회 중 에러";

        /// <summary>
        /// 외래 예약 계산 호출 시 에러
        /// </summary>
        private string ERR_FIND_CALL_AMB = "외래예약계산호출 에러";

        /// <summary>
        /// 외래 예약건에 대한 Sum을 구할 때 오류 발생 
        /// </summary>
        private string ERR_Sum_AmbRsvAmt = "예약건 합계 시 오류";

        /// <summary>
        /// 퇴원수납일 경우.. 단수 차액을 계산 시 오류 발생 
        /// </summary>
        private string ERR_DoFractionAmt = "단수차액 오류";


        /// <summary>
        /// 예약비 조회 시, 외래 예약 대상 조회 
        /// </summary>
        private string ERR_FIND_AMBRSV_LIST = "예약 리스트 조회 오류";

        /// <summary>
        /// 예약비에 대한 진료비 계산을 호출 시 오류 발생
        /// </summary>
        private string ERR_FIND_AMBRSV_CALC = "예약비 계산 시 오류 발생";

        /// <summary>
        /// 예약비에 대한 카드 기수납 조회 에러
        /// </summary>
        private string ERR_AMBRSV_CALC_CARD = "예약비에 대한 카드 기수납 조회 에러";

        /// <summary>
        /// 수납 스타트 시 오류 발생 
        /// </summary>
        private string ACPT_START = "수납시작오류";

        /// <summary>
        /// 입원수납체크사항
        /// </summary>
        private string ACPT_CHECK = "입원수납체크사항";

        /// <summary>
        /// 입원수납 데이터 생성 오류
        /// </summary>
        private string ADMI_MAKE_DT = "입원수납데이터생성 오류";

        /// <summary>
        /// 입원수납 시 1000원 미만에 대한 승인요청을 보낼 경우 
        /// </summary>
        private string ADMI_CARD_PERM_UNDER_1000 = "1000원미만 승인요청";

        /// <summary>
        /// 입원수납 시 할부기간이 적용될 경우, 
        /// 5000원 미만에 대해 
        /// </summary>
        private string ADMI_CARD_VldThru_UNDER_5000 = "5000원 미만 할부미적용";

        /// <summary>
        /// 신용카드 승인 요청 오류 
        /// </summary>
        private string ADMI_CARD_PERMMIT = "신용카드 승인요청오류";

        /// <summary>
        /// 현금수납 데이터 오류
        /// </summary>
        private string ADMI_MAKE_CASHDATA = "현금수납 데이터생성 오류";

        /// <summary>
        /// 수납내역을 OCS에 저장 중 에러 발생 
        /// </summary>
        private string ADMI_ACPT_OCSSAVE = "수납내역 저장 오류";

        /// <summary>
        /// 외래수납데이터생성오류 
        /// </summary>
        private string AMB_RSV_MAKE_INFO = "외래수납데이터생성오류";

        /// <summary>
        /// 입원영수증데이터 생성 오류 
        /// </summary>
        private string AMB_MAKE_ADMI_BILLDT = "입원영수증데이터생성오류";

        /// <summary>
        /// 외래영수증데이터 생성 오류 
        /// </summary>
        private string AMB_MAKE_AMB_BILLDT = "외래영수증데이터생성오류";

        /// <summary>
        /// 수납금액이 1원 단위일 경우의 오류 처리 
        /// </summary>
        private string DSC_CNNT_MOD10 = "수납금액은 1원 단위일 수 없습니다";

        /// <summary>
        /// 외래 수납 내역 대상 생성 중 오류 발생 
        /// </summary>
        private string AMB_RSV_MAKE_AcptObject = "외래수납내역 대상생성 오류(SetAmbDs)";

        /// <summary>
        /// 외래 수납 시 오류 발생 
        /// </summary>
        private string AMB_RSV_START_RECEIPT = "외래예약건 수납 시 오류";

        #endregion


        public string ReturnMsg(string Msg)
        {
            string strvalue = string.Empty;

            switch (Msg)
            {
                case "NOT_FOUND_PATINFO":
                    strvalue = this.NOT_FOUND_PATINFO;
                    break;

                case "ERR_FIND_PATINFO":
                    strvalue = this.ERR_FIND_PATINFO;
                    break;

                case "ERR_FIND_AMBACPT":
                    strvalue = this.ERR_FIND_AMBACPT;
                    break;

                case "ERR_FIND_CALL_AMB":
                    strvalue = this.ERR_FIND_CALL_AMB;
                    break;

                case "ERR_Sum_AmbRsvAmt":
                    strvalue = this.ERR_Sum_AmbRsvAmt;
                    break;

                case "ERR_DoFractionAmt":
                    strvalue = this.ERR_DoFractionAmt;
                    break;

                case "ERR_FIND_AMBRSV_LIST":
                    strvalue = this.ERR_FIND_AMBRSV_LIST;
                    break;

                case "ERR_FIND_AMBRSV_CALC":
                case "ERR_AMBRSV_CALC_CALL":
                    strvalue = this.ERR_FIND_AMBRSV_CALC;
                    break;

                case "ERR_AMBRSV_CALC_CARD":
                    strvalue = this.ERR_AMBRSV_CALC_CARD;
                    break;

                case "ACPT_START":
                    strvalue = this.ACPT_START;
                    break;

                case "ACPT_CHECK":
                case "ADMI_ACPT_START":
                    strvalue = this.ACPT_CHECK;
                    break;

                case "ADMI_MAKEDT_1":
                case "ADMI_MAKEDT_2":
                case "ADMI_MAKEDT_3":
                case "ADMI_MAKEDT_4":
                case "ADMI_MAKEDT_5":
                    strvalue = this.ADMI_MAKE_DT;
                    break;


                case "ADMI_CARD_PERM_UNDER_1000":
                    strvalue = this.ADMI_CARD_PERM_UNDER_1000;
                    break;

                case "ADMI_CARD_VldThru_UNDER_5000":
                    strvalue = this.ADMI_CARD_VldThru_UNDER_5000;
                    break;

                case "ADMI_CARD_PERMMIT":
                    strvalue = this.ADMI_CARD_PERMMIT;
                    break;

                case "ADMI_MAKE_CASHDATA":
                    strvalue = this.ADMI_MAKE_CASHDATA;
                    break;

                case "ADMI_ACPT_OCSSAVE":
                    strvalue = this.ADMI_ACPT_OCSSAVE;
                    break;

                case "AMB_RSV_MAKE_INFO":
                    strvalue = this.AMB_RSV_MAKE_INFO;
                    break;

                case "AMB_MAKE_ADMI_BILLDT":
                    strvalue = this.AMB_MAKE_ADMI_BILLDT;
                    break;

                case "AMB_MAKE_AMB_BILLDT":
                    strvalue = this.AMB_MAKE_AMB_BILLDT;
                    break;

                case "DSC_CNNT_MOD10":
                    strvalue = this.DSC_CNNT_MOD10;
                    break;

                case "AMB_RSV_MAKE_AcptObject":
                    strvalue = this.AMB_RSV_MAKE_AcptObject;
                    break;

                case "AMB_RSV_START_RECEIPT":
                    strvalue = this.AMB_RSV_START_RECEIPT;
                    break;
            }

            if (strvalue == string.Empty) strvalue = Msg;

            return strvalue;
        }
    }
}