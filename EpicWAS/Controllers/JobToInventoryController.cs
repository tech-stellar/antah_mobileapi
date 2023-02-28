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
    public class JobToInventoryController : ApiController
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
                        oJobBO._LoadJobAsmbl(ref oEpicorEnv, strCurCompany, strCurPlant, strJobNum, ref JobAsmblList, out strReturnMsg);

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
        public HttpResponseMessage LoadPartLot(string strUID, string strPass, string strCurCompany, string strCurPlant, string strEnvId, string strPartNum, string strLotNum)
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






        [HttpPost]
        public HttpResponseMessage PerformReceiptsToInventory(string strUID, string strPass, string strEnvId, string strCurCompany, string strJobNum, int iAssemblySeq, string strPartNum,
                                                                string strLotNum, decimal dtranQTy, string strIUM, string strFromWarehouseCode, string strFromBinNum,
                                                                string strToWarehouseCode, string strToBinNum, string strCurPlant, int iLabelCount = 1, string strReference = "")
        {
            string strReturnMsg;
            bool IsTrxComplete = false;
            bool IsComplete = false;
            SpecialCharHandler oSpecialChar = new SpecialCharHandler();

            EpicEnv oEpicorEnv = new EpicEnv();
            EpicEnvBO oEpicorEnvBO = new EpicEnvBO();
            IsComplete = oEpicorEnvBO._LoadEpicEnvById(strEnvId, ref oEpicorEnv, out strReturnMsg);

            // new addon on 2019-04-23 for validation 
            IList<Error> oErrors = new List<Error>();
            ErrorBO oValidate = new ErrorBO();

            if (ConfigurationManager.AppSettings["EnableCheckJobAssembly"].Equals("true"))
            { oValidate._CheckJobAssembly(ref oEpicorEnv, strCurCompany, strJobNum, iAssemblySeq, "[JRTI01]", ref oErrors); }


            if (ConfigurationManager.AppSettings["EnableCheckJob"].Equals("true"))
            { oValidate._CheckJob(ref oEpicorEnv, strCurCompany, strJobNum, strCurPlant, "[JRTI02]", ref oErrors); }

            if (ConfigurationManager.AppSettings["EnableCheckPartWhse"].Equals("true"))
            { oValidate._CheckPartWhse(ref oEpicorEnv, strCurCompany, strFromWarehouseCode, strPartNum, "[JRTI05]", ref oErrors); }

            if (ConfigurationManager.AppSettings["EnableCheckWhse"].Equals("true"))
            {
                //oValidate._CheckWhse(ref oEpicorEnv, strCurCompany, strCurPlant, strToWarehouseCode, "[JRTI05]", ref oErrors);
                oValidate._CheckWhse(ref oEpicorEnv, strCurCompany, strCurPlant, strFromWarehouseCode, "Issue Material", ref oErrors);
            }

            if (ConfigurationManager.AppSettings["EnableCheckWhseBin"].Equals("true"))
            {
                oValidate._CheckWhseBinNum(ref oEpicorEnv, strCurCompany, strFromWarehouseCode, strFromBinNum, "Receipts To Inventory", ref oErrors);
                //oValidate._CheckWhseBinNum(ref oEpicorEnv, strCurCompany, strToWarehouseCode, strToBinNum, "[JRTI06]", ref oErrors);
            }

            if (iAssemblySeq != 0)
            { oValidate._CheckJobAssemblyPart(ref oEpicorEnv, strCurCompany, strJobNum, iAssemblySeq, strPartNum, "[JRTI03]", ref oErrors);  }
            else
            { oValidate._CheckJobPart(ref oEpicorEnv, strCurCompany, strJobNum, strPartNum, "[JRTI04]", ref oErrors); }

            PartBO oPartBO = new PartBO();
            Part oPart = new Part();

            oPartBO._LoadPartById(ref oEpicorEnv, strCurCompany, strPartNum, ref oPart, out strReturnMsg);

            if (oPart.TrackLots)
            {
                if (ConfigurationManager.AppSettings["EnableCheckPartLot"].Equals("true"))
                { oValidate._CheckPartLot(ref oEpicorEnv, strCurCompany, strPartNum, strLotNum, "[JRTI07]", ref oErrors); }
            }


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
                ReceiptsFromMfg oReceiptFromMfg = new ReceiptsFromMfg();

                JobBO oJob = new JobBO();

                oReceiptFromMfg.AssemblySeq = iAssemblySeq;
                oReceiptFromMfg.Company = strCurCompany;
                //oReceiptFromMfg.FromBinNum = strFromBinNum;
                //oReceiptFromMfg.FromWarehouseCode = strFromWarehouseCode;
                oReceiptFromMfg.IUM = strIUM;
                oReceiptFromMfg.JobNum = strJobNum;
                oReceiptFromMfg.LotNum = (string.IsNullOrEmpty(strLotNum) == true? string.Empty : strLotNum);
                oReceiptFromMfg.PartNum = oSpecialChar.replaceSpecialCharCSharp( strPartNum);
                oReceiptFromMfg.CurrentPlant = strCurPlant;

                //if (strToBinNum == null || strToWarehouseCode == "")
                //{
                //    DefWhseBin oDefWhseBin = new DefWhseBin();
                //    oJob._LoadDefaultWhseAndBinForPerformMaterialToInventory(ref oEpicorEnv, strCurCompany, strPartNum, ref oDefWhseBin, out strReturnMsg);

                //    oReceiptFromMfg.ToBinNum = oDefWhseBin.BinNum;
                //    oReceiptFromMfg.ToWarehouseCode = oDefWhseBin.Whse;
                //}
                //else
                //{
                //    oReceiptFromMfg.ToBinNum = strToBinNum;
                //    oReceiptFromMfg.ToWarehouseCode = strToWarehouseCode;
                // }

                if (oReceiptFromMfg.FromWarehouseCode == null)
                {
                    JobOprDtl oJobOprDtl = new JobOprDtl();
                    oJob._LoadDefaultJobMtlWhseAndBin2(ref oEpicorEnv, strCurCompany, strCurPlant, strJobNum, iAssemblySeq, ref oJobOprDtl, out strReturnMsg);

                    oReceiptFromMfg.FromWarehouseCode = oJobOprDtl.InputWhse;
                    oReceiptFromMfg.FromBinNum = oJobOprDtl.InputBinNum;
                }


                oReceiptFromMfg.ToBinNum = strFromBinNum;
                oReceiptFromMfg.ToWarehouseCode = strFromWarehouseCode;


                oReceiptFromMfg.TranQty = dtranQTy;
                oReceiptFromMfg.LabelCount = iLabelCount;
                oReceiptFromMfg.Reference = (strReference == null ? string.Empty : strReference);

                IsTrxComplete = oEpicor._EpicReceiptsFromMfg(ref oEpicorEnv, ref oReceiptFromMfg, out strReturnMsg, strUID, strPass);

                if (IsTrxComplete)
                {
                    SQLServerBO oSQL = new SQLServerBO();
                    string _strSQLCon = oSQL._retSQLConnectionString();
                    _strSQLCon = string.Format(_strSQLCon, oEpicorEnv.Env_SQLServer, oEpicorEnv.Env_SQLDB, oEpicorEnv.Env_SQLUserId, oEpicorEnv.Env_SQLPassKey);

                    oSQL._exeSDSCreateLabelSP(_strSQLCon, strCurCompany, strUID, "MFG-STK", DateTime.Now, oSpecialChar.replaceSpecialCharCSharp( strPartNum), strIUM, (strLotNum == null ? string.Empty : strLotNum), dtranQTy, iLabelCount, strJobNum, "", oReceiptFromMfg.TranNum);


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
        public HttpResponseMessage PerformMaterialToInventory(string strUID, string strPass, string strEnvId, string strCurCompany, string strJobNum, int iAssemblySeq, int iMaterialSeq, string strPartNum,
                                                                string strLotNum, decimal dtranQTy, string strIUM, string strFromWarehouseCode, string strFromBinNum,
                                                                string strToWarehouseCode, string strToBinNum,string strCurPlant, int iLabelCount = 1)
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
            { IsPartNumOK = oValidate._CheckPartNum(ref oEpicorEnv, strCurCompany, strPartNum, "[RM03]", ref oErrors); }

            if (IsPartNumOK)
            { oPartBO._LoadPartById(ref oEpicorEnv, strCurCompany, strPartNum, ref oPart, out strReturnMsg); }

            if (ConfigurationManager.AppSettings["EnableCheckJobMatSeq"].Equals("true"))
            { oValidate._CheckJobMaterial(ref oEpicorEnv, strCurCompany, strJobNum, iAssemblySeq, iMaterialSeq, "[RM01]", ref oErrors); }

            if (ConfigurationManager.AppSettings["EnableCheckJob"].Equals("true"))
            { oValidate._CheckJob(ref oEpicorEnv, strCurCompany, strJobNum, strCurPlant, "[RM02]", ref oErrors); }

            if (ConfigurationManager.AppSettings["EnableCheckJobRelease"].Equals("true"))
            { oValidate._CheckJobRelease(ref oEpicorEnv, strCurCompany, strJobNum, "[RM09]", ref oErrors); }

            if (ConfigurationManager.AppSettings["EnableCheckJobComplete"].Equals("true"))
            { oValidate._CheckJobComplete(ref oEpicorEnv, strCurCompany, strJobNum, "[RM10]", ref oErrors); }

            if (IsPartNumOK && oPart.TrackLots)
            {
                if (ConfigurationManager.AppSettings["EnableCheckPartLot"].Equals("true"))
                { oValidate._CheckPartLot(ref oEpicorEnv, strCurCompany, strPartNum, strLotNum, "[RM04]", ref oErrors); }
            }

            if (ConfigurationManager.AppSettings["EnableCheckWhse"].Equals("true"))
            {
                //if (ConfigurationManager.AppSettings["EnableAMM"].Equals("true"))
                //{
                 //   oValidate._CheckWhse(ref oEpicorEnv, strCurCompany, strCurPlant, strToWarehouseCode, "[RM05]", ref oErrors);
                //}
                oValidate._CheckWhse(ref oEpicorEnv, strCurCompany, strCurPlant, strFromWarehouseCode, "[RM07]", ref oErrors);
            }

            if (ConfigurationManager.AppSettings["EnableCheckWhseBin"].Equals("true"))
            {
                oValidate._CheckWhseBinNum(ref oEpicorEnv, strCurCompany, strFromWarehouseCode, strFromBinNum, "[RM08]", ref oErrors);
                //if (ConfigurationManager.AppSettings["EnableAMM"].Equals("true"))
                //{
                //    oValidate._CheckWhseBinNum(ref oEpicorEnv, strCurCompany, strFromWarehouseCode, strFromBinNum, "[RM06]", ref oErrors);
                //}
            }

            //if (ConfigurationManager.AppSettings["EnableCheckPartWhse"].Equals("true"))
            //{ oValidate._CheckPartWhse(ref oEpicorEnv, strCurCompany, strFromWarehouseCode, strPartNum, "Return Material", ref oErrors); }

            //if (ConfigurationManager.AppSettings["EnableCheckAvailableQty"].Equals("true"))
            //{ oValidate._CheckAvailableQty(ref oEpicorEnv, strCurCompany, strPartNum, strFromWarehouseCode, strFromBinNum, strLotNum, strIUM, dtranQTy, "Return Material", ref oErrors); }



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
                JobBO oJob = new JobBO();

                JobHead oJobHead = new JobHead();
                JobMtl oJobMtl = new JobMtl();

                ReturnMtlToStock oReturnMaterial = new ReturnMtlToStock();

                oJob._LoadJobHeadById(ref oEpicorEnv, strCurCompany, strCurPlant, strJobNum, ref oJobHead, out strReturnMsg);
                oJob._LoadJobMtlByMtlSeq(ref oEpicorEnv, strCurCompany, strCurPlant, strJobNum, iAssemblySeq, iMaterialSeq, ref oJobMtl, out strReturnMsg);

                oReturnMaterial.Company = strCurCompany;
                oReturnMaterial.FromJobNum = strJobNum;
                oReturnMaterial.FromAssemblySeq = iAssemblySeq;
                oReturnMaterial.FromMaterialSeq = iMaterialSeq;
                //oReturnMaterial.FromWarehouseCode = strFromWarehouseCode;
                //oReturnMaterial.FromBinNum = strFromBinNum;
                oReturnMaterial.FromJobPartNum = oJobHead.PartNum;
                oReturnMaterial.Plant = strCurPlant;

                oReturnMaterial.ToMaterialPartNum = oSpecialChar.replaceSpecialCharCSharp( strPartNum);
                oReturnMaterial.ToMaterialPartIUM = strIUM;

                // assign default warehouse and bin into from section
                if (oReturnMaterial.FromWarehouseCode == null || oReturnMaterial.FromWarehouseCode == "")
                {
                    JobOprDtl oJobOprDtl = new JobOprDtl();
                    oJob._LoadDefaultJobMtlWhseAndBin(ref oEpicorEnv, strCurCompany, strCurPlant, strJobNum, iAssemblySeq, oJobMtl.RelatedOperation, ref oJobOprDtl, out strReturnMsg);

                    oReturnMaterial.FromWarehouseCode = oJobOprDtl.InputWhse; //ChangeToInput koo 0602021
                    oReturnMaterial.FromBinNum = oJobOprDtl.InputBinNum; //ChangeToInput koo 06012021
                }
                // end assign default warehouse and bin


                oReturnMaterial.ToWarehouseCode = strFromWarehouseCode;
                oReturnMaterial.ToBinNum = strFromBinNum;

                oReturnMaterial.LabelCount = iLabelCount;

                string strTempLot = (strLotNum == null ? string.Empty : strLotNum);
                oReturnMaterial.ToLotNum = strTempLot;

                oReturnMaterial.TranQty = dtranQTy;

                IsTrxComplete = oEpicor._EpicReturnMaterial(ref oEpicorEnv, ref oReturnMaterial, out strReturnMsg, strUID, strPass);

                if (IsTrxComplete)
                {
                    SQLServerBO oSQL = new SQLServerBO();
                    string _strSQLCon = oSQL._retSQLConnectionString();
                    _strSQLCon = string.Format(_strSQLCon, oEpicorEnv.Env_SQLServer, oEpicorEnv.Env_SQLDB, oEpicorEnv.Env_SQLUserId, oEpicorEnv.Env_SQLPassKey);

                    oSQL._exeSDSCreateLabelSP(_strSQLCon, strCurCompany, strUID, "MTL-STK", DateTime.Now, oSpecialChar.replaceSpecialCharCSharp( strPartNum), strIUM, (strLotNum == null ? string.Empty : strLotNum), dtranQTy, iLabelCount, strJobNum, iAssemblySeq.ToString(), iMaterialSeq.ToString());

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
        public HttpResponseMessage PerformAssemblyToInventory(string strUID, string strPass, string strEnvId, string strCurCompany, string strJobNum, int iAssemblySeq, string strPartNum,
                                                                string strLotNum, decimal dtranQTy, string strIUM, string strFromWarehouseCode, string strFromBinNum,
                                                                string strToWarehouseCode, string strToBinNum, string strCurPlant, int iLabelCount = 1, string strReference = "")
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
            { IsPartNumOK = oValidate._CheckPartNum(ref oEpicorEnv, strCurCompany, strPartNum, "[RM03]", ref oErrors); }

            if (IsPartNumOK)
            { oPartBO._LoadPartById(ref oEpicorEnv, strCurCompany, strPartNum, ref oPart, out strReturnMsg); }

            if (ConfigurationManager.AppSettings["EnableCheckJob"].Equals("true"))
            { oValidate._CheckJob(ref oEpicorEnv, strCurCompany, strJobNum, strCurPlant, "[RM02]", ref oErrors); }

            if (ConfigurationManager.AppSettings["EnableCheckJobRelease"].Equals("true"))
            { oValidate._CheckJobRelease(ref oEpicorEnv, strCurCompany, strJobNum, "[RM09]", ref oErrors); }

            if (ConfigurationManager.AppSettings["EnableCheckJobComplete"].Equals("true"))
            { oValidate._CheckJobComplete(ref oEpicorEnv, strCurCompany, strJobNum, "[RM10]", ref oErrors); }

            if (IsPartNumOK && oPart.TrackLots)
            {
                if (ConfigurationManager.AppSettings["EnableCheckPartLot"].Equals("true"))
                { oValidate._CheckPartLot(ref oEpicorEnv, strCurCompany, strPartNum, strLotNum, "[RM04]", ref oErrors); }
            }

            if (ConfigurationManager.AppSettings["EnableCheckWhse"].Equals("true"))
            {
                //oValidate._CheckWhse(ref oEpicorEnv, strCurCompany, strCurPlant, strToWarehouseCode, "[RM05]", ref oErrors);
                oValidate._CheckWhse(ref oEpicorEnv, strCurCompany, strCurPlant, strFromWarehouseCode, "[RM07]", ref oErrors);
            }

            if (ConfigurationManager.AppSettings["EnableCheckWhseBin"].Equals("true"))
            {
                oValidate._CheckWhseBinNum(ref oEpicorEnv, strCurCompany, strFromWarehouseCode, strFromBinNum, "[RM08]", ref oErrors);
                //oValidate._CheckWhseBinNum(ref oEpicorEnv, strCurCompany, strToWarehouseCode, strToBinNum, "[RM06]", ref oErrors);
            }

            //if (ConfigurationManager.AppSettings["EnableCheckPartWhse"].Equals("true"))
            //{ oValidate._CheckPartWhse(ref oEpicorEnv, strCurCompany, strFromWarehouseCode, strPartNum, "Return Material", ref oErrors); }

            //if (ConfigurationManager.AppSettings["EnableCheckAvailableQty"].Equals("true"))
            //{ oValidate._CheckAvailableQty(ref oEpicorEnv, strCurCompany, strPartNum, strFromWarehouseCode, strFromBinNum, strLotNum, strIUM, dtranQTy, "Return Material", ref oErrors); }



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
                JobBO oJob = new JobBO();

                JobHead oJobHead = new JobHead();
                ReturnAssemblyToStock oReturnAssembly = new ReturnAssemblyToStock();

                oJob._LoadJobHeadById(ref oEpicorEnv, strCurCompany, strCurPlant, strJobNum, ref oJobHead, out strReturnMsg);

                oReturnAssembly.Company = strCurCompany;
                oReturnAssembly.FromJobNum = strJobNum;
                oReturnAssembly.FromAssemblySeq = iAssemblySeq;

                //oReturnAssembly.FromWarehouseCode = strFromWarehouseCode;
                //oReturnAssembly.FromBinNum = strFromBinNum;

                oReturnAssembly.Plant = strCurPlant;

                oReturnAssembly.ToPartNum = oSpecialChar.replaceSpecialCharCSharp( strPartNum);
                oReturnAssembly.ToPartIUM = strIUM;
                oReturnAssembly.CurrentPlant = strCurPlant;


                if (oReturnAssembly.FromWarehouseCode == null)
                {
                    JobOprDtl oJobOprDtl = new JobOprDtl();
                    oJob._LoadDefaultJobMtlWhseAndBin2(ref oEpicorEnv, strCurCompany, strCurPlant, strJobNum, iAssemblySeq, ref oJobOprDtl, out strReturnMsg);

                    oReturnAssembly.FromWarehouseCode = oJobOprDtl.InputWhse;
                    oReturnAssembly.FromBinNum = oJobOprDtl.InputBinNum;
                }

                oReturnAssembly.ToWarehouseCode = strFromWarehouseCode;
                oReturnAssembly.ToBinNum = strFromBinNum;

                //if (ConfigurationManager.AppSettings["EnableAMM"].Equals("true"))
                //{
                 //   oReturnAssembly.ToWarehouseCode = strToWarehouseCode;
                  //  oReturnAssembly.ToBinNum = strToBinNum;
                //}
                //else
                //{
                //    DefWhseBin oDefWhseBin = new DefWhseBin();
                //    oJob._LoadDefaultWhseAndBinForPerformMaterialToInventory(ref oEpicorEnv, strCurCompany, strPartNum, ref oDefWhseBin, out strReturnMsg);

                 //   oReturnAssembly.ToWarehouseCode = oDefWhseBin.Whse;
                  //  oReturnAssembly.ToBinNum = oDefWhseBin.BinNum;

                //}

                oReturnAssembly.LabelCount = iLabelCount;

                //modified on 2019-07-05
                oReturnAssembly.Reference = (strReference == null ? string.Empty : strReference);

                string strTempLot = (strLotNum == null ? string.Empty : strLotNum);
                oReturnAssembly.ToLotNum = strTempLot;

                oReturnAssembly.TranQty = dtranQTy;

                IsTrxComplete = oEpicor._EpicReturnAssembly(ref oEpicorEnv, ref oReturnAssembly, out strReturnMsg, strUID, strPass);

                if (IsTrxComplete)
                {
                    SQLServerBO oSQL = new SQLServerBO();
                    string _strSQLCon = oSQL._retSQLConnectionString();
                    _strSQLCon = string.Format(_strSQLCon, oEpicorEnv.Env_SQLServer, oEpicorEnv.Env_SQLDB, oEpicorEnv.Env_SQLUserId, oEpicorEnv.Env_SQLPassKey);

                    oSQL._exeSDSCreateLabelSP(_strSQLCon, strCurCompany, strUID, "ASM-STK", DateTime.Now, oSpecialChar.replaceSpecialCharCSharp( strPartNum), strIUM, (strLotNum == null ? string.Empty : strLotNum), dtranQTy, iLabelCount, strJobNum, iAssemblySeq.ToString(), oReturnAssembly.TranNum);


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
