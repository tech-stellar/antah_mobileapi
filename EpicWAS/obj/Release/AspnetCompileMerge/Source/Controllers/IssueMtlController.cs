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
    public class IssueMtlController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage LoadJobHeadAsmblMtl(string strUID, string strPass, string strCurCompany, string strCurPlant, string strEnvId, string strJobNum)
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsLoadJobHeadOK = false;

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
                    JobAsmblMtl oJobDoc = new JobAsmblMtl();
                    JobHead oJobHead = new JobHead();

                    JobBO oJobBO = new JobBO();
                    IsLoadJobHeadOK = oJobBO._LoadJobHeadById(ref oEpicorEnv, strCurCompany, strCurPlant, strJobNum, ref oJobHead, out strReturnMsg);

                    if (IsLoadJobHeadOK)
                    {
                        oJobDoc.Jobs = oJobHead;

                        IList<JobAsmbl> JobAsmblList = new List<JobAsmbl>();
                        oJobBO._LoadJobAsmbl(ref oEpicorEnv, strCurCompany, strCurPlant, strJobNum,ref JobAsmblList, out strReturnMsg);

                        oJobDoc.JobAsmbls = JobAsmblList;

                        IList<JobMtl> JobMtlList = new List<JobMtl>();
                        oJobBO._LoadJobMtl(ref oEpicorEnv, strCurCompany, strCurPlant, strJobNum, ref JobMtlList, out strReturnMsg);

                        oJobDoc.JobMtls = JobMtlList;

                        return Request.CreateResponse(HttpStatusCode.OK, oJobDoc);
                    }
                    else
                    {
                        HttpError err = new HttpError(strReturnMsg);
                        return Request.CreateResponse(HttpStatusCode.NotFound, err);
                    } // end if for IsLoadJobHeadOK

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


        [HttpGet]
        public HttpResponseMessage LoadPart(string strUID, string strPass, string strCurCompany, string strCurPlant, string strEnvId, string strPartNum)
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsLoadPartOK = false;

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
                    PartBO oPartBO = new PartBO();

                    Part oPart = new Part();
                    PartWithLots oPartWithLots = new PartWithLots();

                    IsLoadPartOK = oPartBO._LoadPartById(ref oEpicorEnv, strCurCompany, strPartNum, ref oPart, out strReturnMsg);

                    if (IsLoadPartOK)
                    {
                        oPartWithLots.Parts = oPart;
                        IList<PartLot> PartLotList = new List<PartLot>();

                        oPartBO._LoadPartLotByPartId(ref oEpicorEnv, strCurCompany, strPartNum, ref PartLotList, out strReturnMsg);
                        oPartWithLots.PartLots = PartLotList;

                        return Request.CreateResponse(HttpStatusCode.OK, oPartWithLots);

                    }
                    else
                    {
                        HttpError err = new HttpError(strReturnMsg);
                        return Request.CreateResponse(HttpStatusCode.NotFound, err);
                    } // end if for isloadpartok


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


        [HttpGet]
        public HttpResponseMessage LoadPartWhseWithBin(string strUID, string strPass, string strCurCompany, string strCurPlant, string strEnvId, string strPartNum, string strWhse)
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsLoadPartWhseOK = false;

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
                    PartBO oPartBO = new PartBO();
                    PartWhse oPartWhse = new PartWhse();

                    WhseWithBIn oWhseWithBin = new WhseWithBIn();

                    IsLoadPartWhseOK = oPartBO._LoadPartWhseById(ref oEpicorEnv, strCurCompany, strPartNum, strWhse, ref oPartWhse, out strReturnMsg);
                    oWhseWithBin.PartWhse = oPartWhse;

                    IList<WhseBin> WhseBins = new List<WhseBin>();
                    oPartBO._LoadWhseBin(ref oEpicorEnv, strCurCompany, strWhse, ref WhseBins, out strReturnMsg);

                    oWhseWithBin.WhseBinList = WhseBins;

                    if (IsLoadPartWhseOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, oWhseWithBin);
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
                } // end if for islogin

            }
            else
            {
                HttpError err = new HttpError(strReturnMsg);
                return Request.CreateResponse(HttpStatusCode.NotFound, err);

            } // end for iscomplete

        }


        [HttpGet]
        public HttpResponseMessage LoadPartLot(string strUID, string strPass, string strCurCompany, string strCurPlant, string strEnvId, string strPartNum, string strLotNum )
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsLoadPartLotOK = false;

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
                    PartBO oPartBO = new PartBO();
                    PartLot oPartLot = new PartLot();

                    IsLoadPartLotOK = oPartBO._LoadPartLotById(ref oEpicorEnv, strCurCompany, strPartNum, strLotNum, ref oPartLot, out strReturnMsg);

                    if (IsLoadPartLotOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, oPartLot);
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
                } // end if for islogin
            }
            else
            {
                HttpError err = new HttpError(strReturnMsg);
                return Request.CreateResponse(HttpStatusCode.NotFound, err);
            } // end for iscomplete

        }







        // issue material to job
        [HttpPost]
        public HttpResponseMessage PerformIssueMaterial(string strUID, string strPass, string strEnvId, string strCurCompany, string strJobNum, int iAssemblySeq, 
                                                        string strPartNum, string strIUM, decimal dTranQty, string strFromWarehouseCode, 
                                                        string strFromBinNum, string strToWarehouseCode, string strToBinNum,
                                                        int iMaterialSeq, string strLotNum, string strCurPlant, int iLabelCount = 1)
        {
            string strReturnMsg;
            bool IsTrxComplete = false;
            bool IsComplete = false;
            SpecialCharHandler oSpecialChar = new SpecialCharHandler();
            string strMessage;

            EpicEnv oEpicorEnv = new EpicEnv();
            EpicEnvBO oEpicorEnvBO = new EpicEnvBO();
            IsComplete = oEpicorEnvBO._LoadEpicEnvById(strEnvId, ref oEpicorEnv, out strReturnMsg);

            // new addon on 2019-04-23 for validation 
            bool IsPartNumOK = false;
            IList<Error> oErrors = new List<Error>();

            ErrorBO oValidate = new ErrorBO();
            PartBO oPartBO = new PartBO();

            Part oPart = new Part();

            if (ConfigurationManager.AppSettings["EnableCheckPartNum"].Equals("true"))
            { IsPartNumOK = oValidate._CheckPartNum(ref oEpicorEnv, strCurCompany, strPartNum, "[IM03]", ref oErrors); }

            if (IsPartNumOK)
            { oPartBO._LoadPartById(ref oEpicorEnv, strCurCompany, strPartNum, ref oPart, out strReturnMsg); }

            if (ConfigurationManager.AppSettings["EnableCheckJobMatSeq"].Equals("true"))
            { oValidate._CheckJobMaterial(ref oEpicorEnv, strCurCompany, strJobNum, iAssemblySeq, iMaterialSeq, "[IM01]", ref oErrors); }

            if (ConfigurationManager.AppSettings["EnableCheckJob"].Equals("true"))
            { oValidate._CheckJob(ref oEpicorEnv, strCurCompany, strJobNum, strCurPlant, "[IM02]", ref oErrors); }

            if (ConfigurationManager.AppSettings["EnableCheckJobRelease"].Equals("true"))
            { oValidate._CheckJobRelease(ref oEpicorEnv, strCurCompany, strJobNum, "[IM10]", ref oErrors); }

            if (ConfigurationManager.AppSettings["EnableCheckJobComplete"].Equals("true"))
            { oValidate._CheckJobComplete(ref oEpicorEnv, strCurCompany, strJobNum, "[IM11]", ref oErrors); }

            if (IsPartNumOK && oPart.TrackLots)
            {
                if (ConfigurationManager.AppSettings["EnableCheckPartLot"].Equals("true"))
                { oValidate._CheckPartLot(ref oEpicorEnv, strCurCompany, strPartNum, strLotNum, "[IM04]", ref oErrors); }
            }

            if (ConfigurationManager.AppSettings["EnableCheckWhse"].Equals("true"))
            {
                //oValidate._CheckWhse(ref oEpicorEnv, strCurCompany, strCurPlant, strToWarehouseCode, "[IM07]", ref oErrors);
                oValidate._CheckWhse(ref oEpicorEnv, strCurCompany, strCurPlant, strFromWarehouseCode, "[IM05]", ref oErrors);
            }

            if (ConfigurationManager.AppSettings["EnableCheckWhseBin"].Equals("true"))
            {
                oValidate._CheckWhseBinNum(ref oEpicorEnv, strCurCompany, strFromWarehouseCode, strFromBinNum, "[IM06]", ref oErrors);
                //oValidate._CheckWhseBinNum(ref oEpicorEnv, strCurCompany, strToWarehouseCode, strToBinNum, "[IM08]", ref oErrors);
            }

            if (ConfigurationManager.AppSettings["EnableCheckPartWhse"].Equals("true"))
            { oValidate._CheckPartWhse(ref oEpicorEnv, strCurCompany, strFromWarehouseCode, strPartNum, "[IM05]", ref oErrors); }

            if (ConfigurationManager.AppSettings["EnableCheckAvailableQty"].Equals("true"))
            { oValidate._CheckAvailableQty(ref oEpicorEnv, strCurCompany, strPartNum, strFromWarehouseCode, strFromBinNum, strLotNum, strIUM, dTranQty, "[IM09]", ref oErrors); }



            if (oErrors.Count > 0)
            {
                string strErr = "";

                foreach (Error oErr in oErrors)
                {
                    strErr += oErr.ErrorCode + " " + oErr.ErrorDescription + ": " ;
                }
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, strErr);
            }

            // end addon on 2019-04-23 for validation



            if (IsComplete)
            {
                EpicorBO oEpicor = new EpicorBO();
                JobBO oJob = new JobBO();

                JobHead oJobHead = new JobHead();
                JobMtl oJobMtl = new JobMtl();

                oJob._LoadJobHeadById(ref oEpicorEnv, strCurCompany, strCurPlant, strJobNum, ref oJobHead, out strReturnMsg);
                oJob._LoadJobMtlByMtlSeq(ref oEpicorEnv, strCurCompany, strCurPlant, strJobNum, iAssemblySeq, iMaterialSeq, ref oJobMtl, out strReturnMsg);

                IssueMtlToJob oIssueMtlToJob = new IssueMtlToJob();
                oIssueMtlToJob.Company = strCurCompany;

                oIssueMtlToJob.FromPartNum = oSpecialChar.replaceSpecialCharCSharp(strPartNum);
                oIssueMtlToJob.FromPartIUM = strIUM;
                oIssueMtlToJob.FromQuantity = dTranQty;

                string strTempLot = (strLotNum == null ? string.Empty : strLotNum);
                oIssueMtlToJob.FromLotNum = strTempLot;

                oIssueMtlToJob.FromWhseCode = strFromWarehouseCode;
                oIssueMtlToJob.FromBinNum = strFromBinNum;

                oIssueMtlToJob.ToJobNum = strJobNum;
                oIssueMtlToJob.ToJobPartNum = oJobHead.PartNum;
                oIssueMtlToJob.ToJobAssemblySeq = iAssemblySeq;
                oIssueMtlToJob.ToJobMaterialSeq = iMaterialSeq;
                oIssueMtlToJob.ToJobMaterialSeqPartNum = oJobMtl.PartNum;
                oIssueMtlToJob.CurrentPlant = strCurPlant;

                oIssueMtlToJob.LabelCount = iLabelCount;

                if (oIssueMtlToJob.ToWhseCode == null || oIssueMtlToJob.ToWhseCode == "")
                {
                    JobOprDtl oJobOprDtl = new JobOprDtl();
                    if (oJob._LoadDefaultJobMtlWhseAndBin(ref oEpicorEnv, strCurCompany, strCurPlant, strJobNum, iAssemblySeq, oJobMtl.RelatedOperation, ref oJobOprDtl, out strReturnMsg))
                    {
                        oIssueMtlToJob.ToWhseCode = oJobOprDtl.InputWhse;
                        oIssueMtlToJob.ToBinNum = oJobOprDtl.InputBinNum;
                    }
                    else
                    {
                        DefWhseBin oDefWhseBin = new DefWhseBin();
                        oJob._LoadDefaultWhseAndBinFromSiteConfig(ref oEpicorEnv, strCurCompany, strCurPlant, ref oDefWhseBin, out strMessage);
                        oIssueMtlToJob.ToWhseCode = oDefWhseBin.Whse;
                        oIssueMtlToJob.ToBinNum = oDefWhseBin.BinNum;
                    }
                }
               
                IsTrxComplete = oEpicor._EpicIssueMaterialToJob(ref oEpicorEnv, ref oIssueMtlToJob, out strReturnMsg, strUID, strPass);

                if (IsTrxComplete)
                {
                    SQLServerBO oSQL = new SQLServerBO();
                    string _strSQLCon = oSQL._retSQLConnectionString();
                    _strSQLCon = string.Format(_strSQLCon, oEpicorEnv.Env_SQLServer, oEpicorEnv.Env_SQLDB, oEpicorEnv.Env_SQLUserId, oEpicorEnv.Env_SQLPassKey);

                    oSQL._exeSDSCreateLabelSP(_strSQLCon, strCurCompany, strUID, "STK-MTL", DateTime.Now, oSpecialChar.replaceSpecialCharCSharp(strPartNum), strIUM, (strLotNum == null ? string.Empty : strLotNum), dTranQty, iLabelCount, strJobNum, iAssemblySeq.ToString(), iMaterialSeq.ToString());

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


        [HttpPost]
        public HttpResponseMessage PerformIssueMiscMaterial(string strUID, string strPass, string strEnvId, string strCurCompany,
                                                        string strPartNum, string strIUM, decimal dTranQty, string strWarehouseCode,
                                                        string strBinNum, string strLotNum, string strReason, string strReference, 
                                                        string strCurPlant, int iLabelCount = 1)
        {
            string strReturnMsg;
            bool IsTrxComplete = false;
            bool IsComplete = false;
            SpecialCharHandler oSpecialChar = new SpecialCharHandler();

            EpicEnv oEpicorEnv = new EpicEnv();
            EpicEnvBO oEpicorEnvBO = new EpicEnvBO();
            IsComplete = oEpicorEnvBO._LoadEpicEnvById(strEnvId, ref oEpicorEnv, out strReturnMsg);

            bool IsPartNumOK = false;
            IList<Error> oErrors = new List<Error>();

            ErrorBO oValidate = new ErrorBO();
            PartBO oPartBO = new PartBO();

            Part oPart = new Part();

            if (ConfigurationManager.AppSettings["EnableCheckPartNum"].Equals("true"))
            { IsPartNumOK = oValidate._CheckPartNum(ref oEpicorEnv, strCurCompany, strPartNum, "[IM03]", ref oErrors); }

            if (IsPartNumOK)
            { oPartBO._LoadPartById(ref oEpicorEnv, strCurCompany, strPartNum, ref oPart, out strReturnMsg); }

            if (IsPartNumOK && oPart.TrackLots)
            {
                if (ConfigurationManager.AppSettings["EnableCheckPartLot"].Equals("true"))
                { oValidate._CheckPartLot(ref oEpicorEnv, strCurCompany, strPartNum, strLotNum, "[IM04]", ref oErrors); }
            }

            if (ConfigurationManager.AppSettings["EnableCheckWhse"].Equals("true"))
            { oValidate._CheckWhse(ref oEpicorEnv, strCurCompany, strCurPlant, strWarehouseCode, "[IM07]", ref oErrors); }

            if (ConfigurationManager.AppSettings["EnableCheckWhseBin"].Equals("true"))
            { oValidate._CheckWhseBinNum(ref oEpicorEnv, strCurCompany, strWarehouseCode, strBinNum, "[IM06]", ref oErrors); }

            if (ConfigurationManager.AppSettings["EnableCheckPartWhse"].Equals("true"))
            { oValidate._CheckPartWhse(ref oEpicorEnv, strCurCompany, strWarehouseCode, strPartNum, "[IM05]", ref oErrors); }

            if (ConfigurationManager.AppSettings["EnableCheckAvailableQty"].Equals("true"))
            { oValidate._CheckAvailableQty(ref oEpicorEnv, strCurCompany, strPartNum, strWarehouseCode, strBinNum, strLotNum, strIUM, dTranQty, "[IM09]", ref oErrors); }

            if (oErrors.Count > 0)
            {
                string strErr = "";

                foreach (Error oErr in oErrors)
                {
                    strErr += oErr.ErrorCode + " " + oErr.ErrorDescription + ": ";
                }
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, strErr);
            }

            if (IsComplete)
                {
                    EpicorBO oEpicor = new EpicorBO();

                    MiscMaterial oMiscMaterial = new MiscMaterial();
                    oMiscMaterial.Company = strCurCompany;
                    oMiscMaterial.PartNum = oSpecialChar.replaceSpecialCharCSharp( strPartNum);
                    oMiscMaterial.IUM = strIUM;
                    oMiscMaterial.WarehouseCode = strWarehouseCode;
                    oMiscMaterial.BinNum = strBinNum;
                    oMiscMaterial.LotNum = strLotNum;
                    oMiscMaterial.TranQty = dTranQty;
                    oMiscMaterial.ReasonCode = strReason;
                    oMiscMaterial.Reference = strReference;
                    oMiscMaterial.CurrentPlant = strCurPlant;

                    IsTrxComplete = oEpicor._EpicIssueMiscMaterial( ref oEpicorEnv, ref oMiscMaterial, out strReturnMsg, strUID, strPass);

                    if (IsTrxComplete)
                    {
                        SQLServerBO oSQL = new SQLServerBO();
                        string _strSQLCon = oSQL._retSQLConnectionString();
                        _strSQLCon = string.Format(_strSQLCon, oEpicorEnv.Env_SQLServer, oEpicorEnv.Env_SQLDB, oEpicorEnv.Env_SQLUserId, oEpicorEnv.Env_SQLPassKey);

                        oSQL._exeSDSCreateLabelSP(_strSQLCon, strCurCompany, strUID, "STK-UKN", DateTime.Now, oSpecialChar.replaceSpecialCharCSharp( strPartNum), strIUM, (strLotNum == null ? string.Empty : strLotNum), dTranQty, iLabelCount, "", "", oMiscMaterial.TranNum);

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


        [HttpPost]
        public HttpResponseMessage PerformReturnMiscMaterial(string strUID, string strPass, string strEnvId, string strCurCompany,
                                                        string strPartNum, string strIUM, decimal dTranQty, string strWarehouseCode,
                                                        string strBinNum, string strLotNum, string strReason, string strReference,
                                                        string strCurPlant, int iLabelCount = 1)
        {
            string strReturnMsg;
            bool IsTrxComplete = false;
            bool IsComplete = false;
            SpecialCharHandler oSpecialChar = new SpecialCharHandler();

            EpicEnv oEpicorEnv = new EpicEnv();
            EpicEnvBO oEpicorEnvBO = new EpicEnvBO();
            IsComplete = oEpicorEnvBO._LoadEpicEnvById(strEnvId, ref oEpicorEnv, out strReturnMsg);

            bool IsPartNumOK = false;
            IList<Error> oErrors = new List<Error>();

            ErrorBO oValidate = new ErrorBO();
            PartBO oPartBO = new PartBO();

            Part oPart = new Part();

            if (ConfigurationManager.AppSettings["EnableCheckPartNum"].Equals("true"))
            { IsPartNumOK = oValidate._CheckPartNum(ref oEpicorEnv, strCurCompany, strPartNum, "[IM03]", ref oErrors); }

            if (IsPartNumOK)
            { oPartBO._LoadPartById(ref oEpicorEnv, strCurCompany, strPartNum, ref oPart, out strReturnMsg); }

            if (IsPartNumOK && oPart.TrackLots)
            {
                if (ConfigurationManager.AppSettings["EnableCheckPartLot"].Equals("true"))
                { oValidate._CheckPartLot(ref oEpicorEnv, strCurCompany, strPartNum, strLotNum, "[IM04]", ref oErrors); }
            }

            if (ConfigurationManager.AppSettings["EnableCheckWhse"].Equals("true"))
            { oValidate._CheckWhse(ref oEpicorEnv, strCurCompany, strCurPlant, strWarehouseCode, "[IM07]", ref oErrors); }

            if (ConfigurationManager.AppSettings["EnableCheckWhseBin"].Equals("true"))
            { oValidate._CheckWhseBinNum(ref oEpicorEnv, strCurCompany, strWarehouseCode, strBinNum, "[IM06]", ref oErrors); }

            if (ConfigurationManager.AppSettings["EnableCheckPartWhse"].Equals("true"))
            { oValidate._CheckPartWhse(ref oEpicorEnv, strCurCompany, strWarehouseCode, strPartNum, "[IM05]", ref oErrors); }

            if (ConfigurationManager.AppSettings["EnableCheckAvailableQty"].Equals("true"))
            { oValidate._CheckAvailableQty(ref oEpicorEnv, strCurCompany, strPartNum, strWarehouseCode, strBinNum, strLotNum, strIUM, dTranQty, "[IM09]", ref oErrors); }

            if (oErrors.Count > 0)
            {
                string strErr = "";

                foreach (Error oErr in oErrors)
                {
                    strErr += oErr.ErrorCode + " " + oErr.ErrorDescription + ": ";
                }
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, strErr);
            }

            if (IsComplete)
            {
                EpicorBO oEpicor = new EpicorBO();

                MiscMaterial oMiscMaterial = new MiscMaterial();
                oMiscMaterial.Company = strCurCompany;
                oMiscMaterial.PartNum = oSpecialChar.replaceSpecialCharCSharp( strPartNum);
                oMiscMaterial.IUM = strIUM;
                oMiscMaterial.WarehouseCode = strWarehouseCode;
                oMiscMaterial.BinNum = strBinNum;
                oMiscMaterial.LotNum = strLotNum;
                oMiscMaterial.TranQty = dTranQty;
                oMiscMaterial.ReasonCode = strReason;
                oMiscMaterial.Reference = strReference;
                oMiscMaterial.CurrentPlant = strCurPlant;

                IsTrxComplete = oEpicor._EpicReturnMiscMaterial(ref oEpicorEnv, ref oMiscMaterial, out strReturnMsg, strUID, strPass);

                if (IsTrxComplete)
                {
                    SQLServerBO oSQL = new SQLServerBO();
                    string _strSQLCon = oSQL._retSQLConnectionString();
                    _strSQLCon = string.Format(_strSQLCon, oEpicorEnv.Env_SQLServer, oEpicorEnv.Env_SQLDB, oEpicorEnv.Env_SQLUserId, oEpicorEnv.Env_SQLPassKey);

                    oSQL._exeSDSCreateLabelSP(_strSQLCon, strCurCompany, strUID, "UKN-STK", DateTime.Now, oSpecialChar.replaceSpecialCharCSharp( strPartNum), strIUM, (strLotNum == null ? string.Empty : strLotNum), dTranQty, iLabelCount, "", "", oMiscMaterial.TranNum);

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


        [HttpPost]
        public HttpResponseMessage PerformQtyAdjustment(string strUID, string strPass, string strEnvId, string strCurCompany,
                                                        string strPartNum, string strIUM, decimal dTranQty, string strWarehouseCode,
                                                        string strBinNum, string strLotNum, string strReason, string strReference,
                                                        string strCurPlant, int iLabelCount = 1)
        {
            string strReturnMsg;
            bool IsTrxComplete = false;
            bool IsComplete = false;
            SpecialCharHandler oSpecialChar = new SpecialCharHandler();

            EpicEnv oEpicorEnv = new EpicEnv();
            EpicEnvBO oEpicorEnvBO = new EpicEnvBO();
            IsComplete = oEpicorEnvBO._LoadEpicEnvById(strEnvId, ref oEpicorEnv, out strReturnMsg);

            bool IsPartNumOK = false;
            IList<Error> oErrors = new List<Error>();

            ErrorBO oValidate = new ErrorBO();
            PartBO oPartBO = new PartBO();

            Part oPart = new Part();

            if (ConfigurationManager.AppSettings["EnableCheckPartNum"].Equals("true"))
            { IsPartNumOK = oValidate._CheckPartNum(ref oEpicorEnv, strCurCompany, strPartNum, "[IM03]", ref oErrors); }

            if (IsPartNumOK)
            { oPartBO._LoadPartById(ref oEpicorEnv, strCurCompany, strPartNum, ref oPart, out strReturnMsg); }

            if (IsPartNumOK && oPart.TrackLots)
            {
                if (ConfigurationManager.AppSettings["EnableCheckPartLot"].Equals("true"))
                { oValidate._CheckPartLot(ref oEpicorEnv, strCurCompany, strPartNum, strLotNum, "[IM04]", ref oErrors); }
            }

            if (ConfigurationManager.AppSettings["EnableCheckWhse"].Equals("true"))
            { oValidate._CheckWhse(ref oEpicorEnv, strCurCompany, strCurPlant, strWarehouseCode, "[IM07]", ref oErrors); }

            if (ConfigurationManager.AppSettings["EnableCheckWhseBin"].Equals("true"))
            { oValidate._CheckWhseBinNum(ref oEpicorEnv, strCurCompany, strWarehouseCode, strBinNum, "[IM06]", ref oErrors); }

            if (ConfigurationManager.AppSettings["EnableCheckPartWhse"].Equals("true"))
            { oValidate._CheckPartWhse(ref oEpicorEnv, strCurCompany, strWarehouseCode, strPartNum, "[IM05]", ref oErrors); }

            //if (ConfigurationManager.AppSettings["EnableCheckAvailableQty"].Equals("true"))
            //{ oValidate._CheckAvailableQty(ref oEpicorEnv, strCurCompany, strPartNum, strWarehouseCode, strBinNum, strLotNum, strIUM, dTranQty, "[IM09]", ref oErrors); }

            if (oErrors.Count > 0)
            {
                string strErr = "";

                foreach (Error oErr in oErrors)
                {
                    strErr += oErr.ErrorCode + " " + oErr.ErrorDescription + ": ";
                }
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, strErr);
            }

            if (IsComplete)
            {
                EpicorBO oEpicor = new EpicorBO();

                QtyAdjustment oQtyAdjustment = new QtyAdjustment();
                oQtyAdjustment.Company = strCurCompany;
                oQtyAdjustment.PartNum = oSpecialChar.replaceSpecialCharCSharp( strPartNum);
                oQtyAdjustment.IUM = strIUM;
                oQtyAdjustment.WarehouseCode = strWarehouseCode;
                oQtyAdjustment.BinNum = strBinNum;
                oQtyAdjustment.LotNum = strLotNum;
                oQtyAdjustment.TranQty = dTranQty;
                oQtyAdjustment.ReasonCode = strReason;
                oQtyAdjustment.Reference = strReference;
                oQtyAdjustment.CurrentPlant = strCurPlant;

                IsTrxComplete = oEpicor._EpicQtyAdjustment(ref oEpicorEnv, ref oQtyAdjustment, out strReturnMsg, strUID, strPass);

                if (IsTrxComplete)
                {
                    SQLServerBO oSQL = new SQLServerBO();
                    string _strSQLCon = oSQL._retSQLConnectionString();
                    _strSQLCon = string.Format(_strSQLCon, oEpicorEnv.Env_SQLServer, oEpicorEnv.Env_SQLDB, oEpicorEnv.Env_SQLUserId, oEpicorEnv.Env_SQLPassKey);

                    oSQL._exeSDSCreateLabelSP(_strSQLCon, strCurCompany, strUID, "ADJ-QTY", DateTime.Now, oSpecialChar.replaceSpecialCharCSharp( strPartNum), strIUM, (strLotNum == null ? string.Empty : strLotNum), dTranQty, iLabelCount, "", "", oQtyAdjustment.TranNum);

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


        [HttpPost]
        public HttpResponseMessage PerformIssueAssembly(string strUID, string strPass, string strEnvId, string strCurCompany, string strJobNum, int iAssemblySeq,
                                                        string strPartNum, string strIUM, decimal dTranQty, string strFromWarehouseCode,
                                                        string strFromBinNum, string strToWarehouseCode, string strToBinNum,
                                                        string strLotNum, string strCurPlant, int iLabelCount = 1, string strReference = "")
        {
            string strReturnMsg;
            bool IsTrxComplete = false;
            bool IsComplete = false;
            SpecialCharHandler oSpecialChar = new SpecialCharHandler();

            EpicEnv oEpicorEnv = new EpicEnv();
            EpicEnvBO oEpicorEnvBO = new EpicEnvBO();

            IsComplete = oEpicorEnvBO._LoadEpicEnvById(strEnvId, ref oEpicorEnv, out strReturnMsg);

            // new addon on 2019-04-23 for validation 
            bool IsPartNumOK = false;
            IList<Error> oErrors = new List<Error>();

            ErrorBO oValidate = new ErrorBO();
            PartBO oPartBO = new PartBO();

            Part oPart = new Part();

            if (ConfigurationManager.AppSettings["EnableCheckPartNum"].Equals("true"))
            { IsPartNumOK = oValidate._CheckPartNum(ref oEpicorEnv, strCurCompany, strPartNum, "Issue Assembly", ref oErrors); }

            if (IsPartNumOK)
            { oPartBO._LoadPartById(ref oEpicorEnv, strCurCompany, strPartNum, ref oPart, out strReturnMsg); }

            if (ConfigurationManager.AppSettings["EnableCheckJob"].Equals("true"))
            { oValidate._CheckJob(ref oEpicorEnv, strCurCompany, strJobNum, strCurPlant, "Issue Assembly", ref oErrors); }

            if (ConfigurationManager.AppSettings["EnableCheckJobRelease"].Equals("true"))
            { oValidate._CheckJobRelease(ref oEpicorEnv, strCurCompany, strJobNum, "Issue Assembly", ref oErrors); }

            if (ConfigurationManager.AppSettings["EnableCheckJobComplete"].Equals("true"))
            { oValidate._CheckJobComplete(ref oEpicorEnv, strCurCompany, strJobNum, "Issue Assembly", ref oErrors); }

            if (IsPartNumOK && oPart.TrackLots)
            {
                if (ConfigurationManager.AppSettings["EnableCheckPartLot"].Equals("true"))
                { oValidate._CheckPartLot(ref oEpicorEnv, strCurCompany, strPartNum, strLotNum, "Issue Assembly", ref oErrors); }
            }

            if (ConfigurationManager.AppSettings["EnableCheckWhse"].Equals("true"))
            {
                //oValidate._CheckWhse(ref oEpicorEnv, strCurCompany, strCurPlant, strToWarehouseCode, "Issue Assembly", ref oErrors);
                oValidate._CheckWhse(ref oEpicorEnv, strCurCompany, strCurPlant, strFromWarehouseCode, "Issue Assembly", ref oErrors);
            }

            if (ConfigurationManager.AppSettings["EnableCheckWhseBin"].Equals("true"))
            {
                oValidate._CheckWhseBinNum(ref oEpicorEnv, strCurCompany, strFromWarehouseCode, strFromBinNum, "Issue Assembly", ref oErrors);
                //oValidate._CheckWhseBinNum(ref oEpicorEnv, strCurCompany, strToWarehouseCode, strToBinNum, "Issue Assembly", ref oErrors);
            }

            if (ConfigurationManager.AppSettings["EnableCheckPartWhse"].Equals("true"))
            { oValidate._CheckPartWhse(ref oEpicorEnv, strCurCompany, strFromWarehouseCode, strPartNum, "Issue Assembly", ref oErrors); }

            if (ConfigurationManager.AppSettings["EnableCheckAvailableQty"].Equals("true"))
            { oValidate._CheckAvailableQty(ref oEpicorEnv, strCurCompany, strPartNum, strFromWarehouseCode, strFromBinNum, strLotNum, strIUM, dTranQty, "Issue Assembly", ref oErrors); }



            if (oErrors.Count > 0)
            { return Request.CreateResponse(HttpStatusCode.ExpectationFailed, oErrors); }

            // end addon on 2019-04-23 for validation

            if (IsComplete)
            {
                EpicorBO oEpicor = new EpicorBO();
                JobBO oJob = new JobBO();

                //JobHead oJobHead = new JobHead();
                //JobAsmbl oJobAssm = new JobAsmbl();

                //oJob._LoadJobHeadById(ref oEpicorEnv, strCurCompany, strCurPlant, strJobNum, ref oJobHead, out strReturnMsg);
                //oJob._LoadJobAsmblBySeqId(ref oEpicorEnv, strCurCompany, strCurPlant, strJobNum, iAssemblySeq, ref oJobAssm, out strReturnMsg);

                IssueAssemblyToJob oIssueAssembly = new IssueAssemblyToJob();
                oIssueAssembly.Company = strCurCompany;

                oIssueAssembly.FromPartNum = oSpecialChar.replaceSpecialCharCSharp( strPartNum);
                oIssueAssembly.FromPartIUM = strIUM;
                oIssueAssembly.FromQuantity = dTranQty;

                string strTempLot = (strLotNum == null ? string.Empty : strLotNum);
                oIssueAssembly.FromLotNum = strTempLot;

                oIssueAssembly.FromWhseCode = strFromWarehouseCode;
                oIssueAssembly.FromBinNum = strFromBinNum;

                oIssueAssembly.ToJobNum = strJobNum;
                //oIssueMtlToJob.ToJobPartNum = oJobHead.PartNum;
                oIssueAssembly.ToJobAssemblySeq = iAssemblySeq;

                oIssueAssembly.ToWhseCode = strToWarehouseCode;
                oIssueAssembly.ToBinNum = strToBinNum;
                oIssueAssembly.Reference = (strReference == null ? string.Empty : strReference);
                oIssueAssembly.CurPlant = strCurPlant;

                if (oIssueAssembly.ToWhseCode == null)
                {
                    JobOprDtl oJobOprDtl = new JobOprDtl();
                    oJob._LoadDefaultJobMtlWhseAndBin2(ref oEpicorEnv, strCurCompany, strCurPlant, strJobNum, iAssemblySeq,  ref oJobOprDtl, out strReturnMsg);

                    oIssueAssembly.ToWhseCode = oJobOprDtl.InputWhse;
                    oIssueAssembly.ToBinNum = oJobOprDtl.InputBinNum;
                 }

                IsTrxComplete = oEpicor._EpicIssueAssemblyToJob(ref oEpicorEnv, ref oIssueAssembly, out strReturnMsg, strUID, strPass);

                if (IsTrxComplete)
                {
                    SQLServerBO oSQL = new SQLServerBO();
                    string _strSQLCon = oSQL._retSQLConnectionString();
                    _strSQLCon = string.Format(_strSQLCon, oEpicorEnv.Env_SQLServer, oEpicorEnv.Env_SQLDB, oEpicorEnv.Env_SQLUserId, oEpicorEnv.Env_SQLPassKey);

                    oSQL._exeSDSCreateLabelSP(_strSQLCon, strCurCompany, strUID, "STK-ASM", DateTime.Now, oSpecialChar.replaceSpecialCharCSharp( strPartNum), strIUM, (strLotNum == null ? string.Empty : strLotNum), dTranQty, iLabelCount, strJobNum, iAssemblySeq.ToString(), oIssueAssembly.TranNum);


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
