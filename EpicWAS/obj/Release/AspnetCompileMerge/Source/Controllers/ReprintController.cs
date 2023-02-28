using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using EpicWAS.Models;
using System.Configuration;

namespace EpicWAS.Controllers
{
    public class ReprintController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage LoadTranInfo(string strUID, string strPass, string strCurCompany, string strCurPlant, string strEnvId, int TranNo)
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsPartTranOK = false;

            EpicEnv oEpicorEnv = new EpicEnv();
            EpicUser oEpicUser = new EpicUser();

            oEpicUser.Epic_UserId = strUID;
            oEpicUser.Epic_PassKey = strPass;

            EpicEnvBO oEpicorEnvBO = new EpicEnvBO();
            IsComplete = oEpicorEnvBO._LoadEpicEnvById(strEnvId, ref oEpicorEnv, out strReturnMsg);

            if (IsComplete)
            {
                EpicUserBO oEpicUserBO = new EpicUserBO();

                IsLogin = oEpicUserBO._VerifyEpicorLogin(ref oEpicorEnv, ref oEpicUser, out strReturnMsg);

                if (IsLogin)
                {
                    PartTran oPartTran = new PartTran();

                    oPartTran.TranNo = TranNo;

                    PartTranBO oPartTranBO = new PartTranBO();

                    IsPartTranOK = oPartTranBO._GetTranInfo(ref oEpicorEnv, strCurCompany, ref oPartTran, out strReturnMsg);

                    if (!IsPartTranOK)
                    {
                        HttpError err = new HttpError(strReturnMsg);
                        return Request.CreateResponse(HttpStatusCode.NotFound, err);
                    }
                    else
                    {

                        return Request.CreateResponse(HttpStatusCode.OK, oPartTran);
                    }
                }
                else
                {
                    HttpError err = new HttpError(strReturnMsg);
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, err);
                } // end if for islogin

            }
            else
            {
                HttpError err = new HttpError(strReturnMsg);
                return Request.CreateResponse(HttpStatusCode.NotFound, err);

            }
        }

        [HttpPost]
        public HttpResponseMessage PerformReprint(string strUID, string strPass, string strCurCompany, string strCurPlant, string strEnvId, string SysDate, 
            string TranNum, string strPartNum, string TranType, decimal dtranQty, string strIUM, string strLotNum, string strJobNum, int iAssemblySeq, int iLabelCount = 0, string strWhse ="")
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            SpecialCharHandler oSpecialChar = new SpecialCharHandler();

            EpicEnv oEpicorEnv = new EpicEnv();
            EpicUser oEpicUser = new EpicUser();

            oEpicUser.Epic_UserId = strUID;
            oEpicUser.Epic_PassKey = strPass;

            EpicEnvBO oEpicorEnvBO = new EpicEnvBO();
            IsComplete = oEpicorEnvBO._LoadEpicEnvById(strEnvId, ref oEpicorEnv, out strReturnMsg);

            if (IsComplete)
            {
                EpicUserBO oEpicUserBO = new EpicUserBO();

                IsLogin = oEpicUserBO._VerifyEpicorLogin(ref oEpicorEnv, ref oEpicUser, out strReturnMsg);

                if (IsLogin)
                {
                    SQLServerBO oSQL = new SQLServerBO();
                    string _strSQLCon = oSQL._retSQLConnectionString();
                    _strSQLCon = string.Format(_strSQLCon, oEpicorEnv.Env_SQLServer, oEpicorEnv.Env_SQLDB, oEpicorEnv.Env_SQLUserId, oEpicorEnv.Env_SQLPassKey);

                    //TranType = (TranType == "" || TranType == null ? "REPRINT-01" : TranType);

                    oSQL._exeSDSCreateLabelSP_Reprint(_strSQLCon, strCurCompany, strUID, TranType, Convert.ToDateTime(SysDate).Date, oSpecialChar.replaceSpecialCharCSharp( strPartNum), strIUM, (strLotNum == null ? string.Empty : strLotNum), dtranQty, iLabelCount, strJobNum, iAssemblySeq.ToString(), TranNum.ToString(), strWhse);

                    return Request.CreateResponse(HttpStatusCode.OK);
   
                }
                else
                {
                    HttpError err = new HttpError(strReturnMsg);
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, err);
                } // end if for islogin

            }
            else
            {
                HttpError err = new HttpError(strReturnMsg);
                return Request.CreateResponse(HttpStatusCode.NotFound, err);

            } // end for iscomplete
        }
    }
}