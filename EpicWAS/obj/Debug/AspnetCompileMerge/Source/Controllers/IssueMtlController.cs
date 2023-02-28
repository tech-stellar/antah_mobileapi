using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using EpicWAS.Models;

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








        [HttpPost]
        public HttpResponseMessage PerformIssueMaterial(string strEnvId, string strCurCompany, string strJobNum, int iAssemblySeq, 
                                                        string strPartNum, string strIUM, decimal dTranQty, string strFromWarehouseCode, 
                                                        string strFromBinNum, string strToWarehouseCode, string strToBinNum,
                                                        int iToJobSeq, string strLotNum)
        {
            string strReturnMsg;
            bool IsTrxComplete = false;
            bool IsComplete = false;

            EpicEnv oEpicorEnv = new EpicEnv();
            EpicEnvBO oEpicorEnvBO = new EpicEnvBO();
            IsComplete = oEpicorEnvBO._LoadEpicEnvById(strEnvId, ref oEpicorEnv, out strReturnMsg);

            if (IsComplete)
            {
                EpicorBO oEpicor = new EpicorBO();

                IssueMtlToJob oIssueMtlToJob = new IssueMtlToJob();
                oIssueMtlToJob.Company = strCurCompany;
                oIssueMtlToJob.JobNum = strJobNum;
                oIssueMtlToJob.AssemblySeq = iAssemblySeq;

                oIssueMtlToJob.PartNum = strPartNum;
                oIssueMtlToJob.IUM = strIUM;
                oIssueMtlToJob.TranQty = dTranQty;
                oIssueMtlToJob.LotNum = strLotNum;

                oIssueMtlToJob.FromWarehouseCode = strFromWarehouseCode;
                oIssueMtlToJob.FromBinNum = strFromBinNum;

                oIssueMtlToJob.ToWarehouseCode = strToWarehouseCode;
                oIssueMtlToJob.ToBinNum = strToBinNum;
               
                oIssueMtlToJob.ToJobSeq = iToJobSeq;

                IsTrxComplete = oEpicor._EpicIssueMaterialToJob(ref oEpicorEnv, ref oIssueMtlToJob, out strReturnMsg);

                if (IsTrxComplete)
                {
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
