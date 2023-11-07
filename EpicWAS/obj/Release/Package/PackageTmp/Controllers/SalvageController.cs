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
    public class SalvageController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage LoadJobForSalvageById(string strUID, string strPass, string strCurCompany, string strCurPlant, string strEnvId, string strJobNum)
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsLoadSalvageOK = false;

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
                    JobBO oJobBO = new JobBO();
                    JobSalvage oJobSal = new JobSalvage();

                    IsLoadSalvageOK = oJobBO._LoadJobForSalvageById(ref oEpicorEnv, strCurCompany, strCurPlant, strJobNum, ref oJobSal, out strReturnMsg);


                    if (IsLoadSalvageOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, oJobSal);
                    }
                    else
                    {
                        HttpError err = new HttpError(strReturnMsg);
                        return Request.CreateResponse(HttpStatusCode.NotFound, err);
                    }

                }
                else
                {
                    HttpError err = new HttpError(strReturnMsg);
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, err);
                } // end if for Islogin

            }
            else
            {
                HttpError err = new HttpError(strReturnMsg);
                return Request.CreateResponse(HttpStatusCode.NotFound, err);
            } // end if for iscomplete

        }



        [HttpPost]
        public HttpResponseMessage PerformReceiptsToSalvage(string strUID, string strPass, string strEnvId, string strCurCompany
                                                            , string strJobNum, string strPartNum
                                                            , decimal dtranQTy, string strIUM, string strReference, string strLotNum
                                                            , string strToWarehouseCode, string strToBinNum, string strCurPlant, int iAssemblySeq = 0, int iMaterialSeq = 0, int iLabelCount = 1)
        {

            string strReturnMsg;
            bool IsTrxComplete = false;
            bool IsComplete = false;
            bool IsLoadMostExpensiveJobMtlOK = false;
            SpecialCharHandler oSpecialChar = new SpecialCharHandler();

            EpicEnv oEpicorEnv = new EpicEnv();
            EpicEnvBO oEpicorEnvBO = new EpicEnvBO();
            IsComplete = oEpicorEnvBO._LoadEpicEnvById(strEnvId, ref oEpicorEnv, out strReturnMsg);

            // new addon on 2019-04-23 for validation 
            IList<Error> oErrors = new List<Error>();

            ErrorBO oValidate = new ErrorBO();

            if (ConfigurationManager.AppSettings["EnableCheckJob"].Equals("true"))
            { oValidate._CheckJob(ref oEpicorEnv, strCurCompany, strJobNum, strCurPlant, "[JRTS02]", ref oErrors); }

            if (ConfigurationManager.AppSettings["EnableCheckJobNum"].Equals("true"))
            { oValidate._CheckJobNum(ref oEpicorEnv, strCurCompany, strJobNum, "[JRTS01]", ref oErrors); }

            //if (ConfigurationManager.AppSettings["EnableCheckWhse"].Equals("true"))
            //{ oValidate._CheckWhse(ref oEpicorEnv, strCurCompany, strCurPlant, strToWarehouseCode, "JRTS03]", ref oErrors);  }

            //if (ConfigurationManager.AppSettings["EnableCheckPartWhse"].Equals("true"))
            //{ oValidate._CheckPartWhse(ref oEpicorEnv, strCurCompany, strToWarehouseCode, strPartNum, "JRTS03]", ref oErrors); }

           // if (ConfigurationManager.AppSettings["EnableCheckWhseBin"].Equals("true"))
            //{ oValidate._CheckWhseBinNum(ref oEpicorEnv, strCurCompany, strToWarehouseCode, strToBinNum, "[JRTS04]", ref oErrors);  }


            if (oErrors.Count > 0)
            {
                string strErr = "";

                foreach (Error oErr in oErrors)
                {
                    strErr += oErr.ErrorCode + " " + oErr.ErrorDescription + ": ";
                }
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, strErr);
            }

            // end addon on 2019-04-23 for validation


            if (IsComplete)
            {
                EpicorBO oEpicor = new EpicorBO();
                JobBO oJobBO = new JobBO();
                JobMtl oExpensiveJobMtl = new JobMtl();
                JobSalvage2 oSalvage = new JobSalvage2();

                IsLoadMostExpensiveJobMtlOK = oJobBO._LoadMostExpensiveJobMtlById(ref oEpicorEnv, strCurCompany, strCurPlant, strJobNum, ref oExpensiveJobMtl, out strReturnMsg);

                oSalvage.Company = strCurCompany;
                oSalvage.Plant = strCurPlant;

                oSalvage.JobNum = strJobNum;
                oSalvage.CurrentPlant = strCurPlant;
                oSalvage.AssemblySeq = oExpensiveJobMtl.AssemblySeq;
                oSalvage.MtlSeq = oExpensiveJobMtl.MtlSeq;
                oSalvage.MtlPartNum = oExpensiveJobMtl.PartNum;

                oSalvage.PartNum = oSpecialChar.replaceSpecialCharCSharp( strPartNum);
                oSalvage.IUM = strIUM;
                
                
                oSalvage.ToWarehouseCode = strToWarehouseCode;
                oSalvage.ToBinNum = strToBinNum;


                if (ConfigurationManager.AppSettings["EnableAMM"].Equals("true"))
                {
                    oSalvage.ToWarehouseCode = strToWarehouseCode;
                    oSalvage.ToBinNum = strToBinNum;
                }
                else
                {
                    DefWhseBin oDefWhseBin = new DefWhseBin();
                    oJobBO._LoadDefaultCompanyWhseAndBin(ref oEpicorEnv, strCurCompany, strCurPlant, ref oDefWhseBin, out strReturnMsg);

                    oSalvage.ToWarehouseCode = oDefWhseBin.Whse;
                    oSalvage.ToBinNum = oDefWhseBin.BinNum;

                }


                oSalvage.LotNum = (strLotNum == null ? string.Empty : strLotNum);
                oSalvage.Reference = (strReference == null ? string.Empty : strReference);
                oSalvage.dTranQty = dtranQTy;

                oSalvage.LabelCount = iLabelCount;

                IsTrxComplete = oEpicor._EpicJobReceiptsToSalvage(ref oEpicorEnv, ref oSalvage, out strReturnMsg, strUID, strPass);

                if (IsTrxComplete)
                {
                    SQLServerBO oSQL = new SQLServerBO();
                    string _strSQLCon = oSQL._retSQLConnectionString();
                    _strSQLCon = string.Format(_strSQLCon, oEpicorEnv.Env_SQLServer, oEpicorEnv.Env_SQLDB, oEpicorEnv.Env_SQLUserId, oEpicorEnv.Env_SQLPassKey);

                    oSQL._exeSDSCreateLabelSP(_strSQLCon, strCurCompany, strUID, "SVG-STK", DateTime.Now,oSpecialChar.replaceSpecialCharCSharp( strPartNum), strIUM, (strLotNum == null ? string.Empty : strLotNum), dtranQTy, iLabelCount, "", "", oSalvage.TranNum);


                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    HttpError err = new HttpError(strReturnMsg);
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
                }

            }
            else
            {
                HttpError err = new HttpError(strReturnMsg);
                return Request.CreateResponse(HttpStatusCode.NotFound, err);
            } // end for iscomplete

        }
    }
}
