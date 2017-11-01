using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using YUHS.WebAPI.Common.DataAccess;
using YUHS.WebAPI.Common.Security;
using YUHS.WebAPI.Common.Utility;
using YUHS.WebAPI.MCare.Patient.Models.Common;
using YUHS.WebAPI.MCare.Patient.Models.Reservation;

namespace YUHS.WebAPI.MCare.Patient.Controllers
{
    [RequireHttps]
    public class ReservationController : ApiController
    {
        [Route("Reservation/GetReserveDept/{hosCd}/{unitNo}")]
        public HttpResponseResult<ReserveDept> GetReserveDept(string hosCd, string unitNo)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);

                IEnumerable<ReserveDept> info = SqlHelper.GetList<ReserveDept>(targetDB: SqlHelper.GetConnectionString("HConnectionString"), storedProcedure: "USP_HP_EXT_IF_Mobile_SelectDeptCdListByHP", param: param);

                return new HttpResponseResult<ReserveDept> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<ReserveDept> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("Reservation/GetReserveDr/{hosCd}/{unitNo}/{deptCd}")]
        public HttpResponseResult<ReserveDr> GetReserveDr(string hosCd, string unitNo, string deptCd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@DeptCd", value: deptCd, dbType: DbType.String, size: 6);

                IEnumerable<ReserveDr> info = SqlHelper.GetList<ReserveDr>(targetDB: SqlHelper.GetConnectionString("HConnectionString"), storedProcedure: "USP_HP_EXT_IF_Mobile_SelectUserCdListByHostCDDeptCd", param: param);

                return new HttpResponseResult<ReserveDr> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<ReserveDr> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("Reservation/GetReserveDate/{hosCd}/{unitNo}/{deptCd}/{drId}/{frYmd}/{toYmd}")]
        public HttpResponseResult<ReserveDate> GetReserveDate(string hosCd, string unitNo, string deptCd, string drId, string frYmd, string toYmd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@DeptCd", value: deptCd, dbType: DbType.String, size: 6);
                param.Add(name: "@DrId", value: drId, dbType: DbType.String, size: 8);
                param.Add(name: "@FrYmd", value: frYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@ToYmd", value: toYmd, dbType: DbType.StringFixedLength, size: 8);

                IEnumerable<ReserveDate> info = SqlHelper.GetList<ReserveDate>(targetDB: SqlHelper.GetConnectionString("HConnectionString"), storedProcedure: "USP_HP_EXT_IF_Mobile_SelectRsvPsblYmd", param: param);

                return new HttpResponseResult<ReserveDate> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<ReserveDate> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("Reservation/GetReserveTime/{hosCd}/{unitNo}/{patNm}/{deptCd}/{drId}/{ymd}")]
        public HttpResponseResult<ReserveTime> GetReserveTime(string hosCd, string unitNo, string patNm, string deptCd, string drId, string ymd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@PatNm", value: patNm, dbType: DbType.String, size: 100);
                param.Add(name: "@DeptCd", value: deptCd, dbType: DbType.String, size: 6);
                param.Add(name: "@DrId", value: drId, dbType: DbType.String, size: 8);
                param.Add(name: "@Ymd", value: ymd, dbType: DbType.StringFixedLength, size: 8);

                IEnumerable<ReserveTime> info = SqlHelper.GetList<ReserveTime>(targetDB: SqlHelper.GetConnectionString("HConnectionString"), storedProcedure: "USP_HP_EXT_IF_Mobile_MakeRsvTimeTable", param: param);

                return new HttpResponseResult<ReserveTime> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<ReserveTime> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }

        }

        [Route("Reservation/SetReservation/{hosCd=hosCd}/{unitNo=unitNo}/{patNm=patNm}/{deptCd=deptCd}/{drId=drId}/{ymd=ymd}/{hhmm=hhmm}/{memo?}")]
        public HttpResponseResult<ReservationResult3> SetReservation(string hosCd, string unitNo, string patNm, string deptCd, string drId, string ymd, string hhmm, string memo = null)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@PatNm", value: patNm, dbType: DbType.String, size: 100);
                param.Add(name: "@DeptCd", value: deptCd, dbType: DbType.String, size: 6);
                param.Add(name: "@DrId", value: drId, dbType: DbType.String, size: 8);
                param.Add(name: "@Ymd", value: ymd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@HHMM", value: hhmm, dbType: DbType.String, size: 5);
                param.Add(name: "@Memo", value: memo, dbType: DbType.String, size: 30);

                var info = SqlHelper.GetMultiPleList<ReservationResult1, ReservationResult2, ReservationResult3>(targetDB: SqlHelper.GetConnectionString("HConnectionString"), storedProcedure: "USP_HP_EXT_IF_Mobile_SaveWebRsv", param: param);
                var result1 = info.Item1;
                var result2 = info.Item2;
                var result3 = info.Item3;

                return new HttpResponseResult<ReservationResult3> { result = result3, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                Utils._WriteLog(ex.Message, "SetReservation");
                return new HttpResponseResult<ReservationResult3> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("Reservation/GetReservation/{hosCd}/{unitNo}/{frYmd}/{toYmd}")]
        public HttpResponseResult<Reservation> GetReservation(string hosCd, string unitNo, string frYmd, string toYmd)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@FrYmd", value: frYmd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@ToYmd", value: toYmd, dbType: DbType.StringFixedLength, size: 8);

                IEnumerable<Reservation> info = SqlHelper.GetList<Reservation>(targetDB: SqlHelper.GetConnectionString("HConnectionString"), storedProcedure: "USP_HP_EXT_IF_Mobile_SelectAmbRcptLst", param: param);

                return new HttpResponseResult<Reservation> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<Reservation> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }

        [Route("Reservation/SetReservationCancel/{hosCd}/{unitNo}/{deptCd}/{ymd}/{patNm}/{drId}/{chosNo}")]
        public HttpResponseResult<ExecuteResult> SetReservationCancel(string hosCd, string unitNo, string deptCd, string ymd, string patNm, string drId, string chosNo)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add(name: "@HosCd", value: hosCd, dbType: DbType.StringFixedLength, size: 2);
                param.Add(name: "@UnitNo", value: unitNo, dbType: DbType.String, size: 10);
                param.Add(name: "@DeptCd", value: deptCd, dbType: DbType.String, size: 6);
                param.Add(name: "@Ymd", value: ymd, dbType: DbType.StringFixedLength, size: 8);
                param.Add(name: "@PatNm", value: patNm, dbType: DbType.String, size: 50);
                param.Add(name: "@DrId", value: drId, dbType: DbType.String, size: 8);
                param.Add(name: "@ChosNo", value: chosNo, dbType: DbType.String, size: 14);

                IEnumerable<ExecuteResult> info = SqlHelper.GetList<ExecuteResult>(targetDB: SqlHelper.GetConnectionString("HConnectionString"), storedProcedure: "USP_HP_EXT_IF_Mobile_RsvChgCn", param: param);

                return new HttpResponseResult<ExecuteResult> { result = info, error = new ErrorInfo { flag = false } };
            }
            catch (Exception ex)
            {
                return new HttpResponseResult<ExecuteResult> { error = new ErrorInfo { flag = true, message = ex.Message } };
            }
        }
    }
}