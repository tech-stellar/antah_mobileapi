using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using EpicWAS.Models;

namespace EpicWAS.Controllers
{
    public class MoveInventoryController : ApiController
    {
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
        public HttpResponseMessage LoadMoveInventoryRequest(string strUID, string strPass, string strCurCompany, int iReqStatus, string strEnvId)
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsLoadRequestOK = false;

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
                    MoveInventoryBO oMove = new MoveInventoryBO();
                    IList<MoveInventory> oMoveInventoryList = new List<MoveInventory>();

                    IsLoadRequestOK = oMove._LoadMoveInvTrx(ref oEpicorEnv, strCurCompany, iReqStatus,ref oMoveInventoryList, out strReturnMsg);

                    if (IsLoadRequestOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, oMoveInventoryList);
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
        public HttpResponseMessage LoadMoveInventoryRequestById(string strUID, string strPass, string strCurCompany, string strReqNum, string strEnvId)
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsLoadRequestOK = false;

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
                    MoveInventoryBO oMove = new MoveInventoryBO();
                    MoveInventory oMoveInventory = new MoveInventory();

                    IsLoadRequestOK = oMove._LoadMoveInvTrxById(ref oEpicorEnv, strCurCompany, strReqNum, ref oMoveInventory, out strReturnMsg);

                    if (IsLoadRequestOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, oMoveInventory);
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
        public HttpResponseMessage PerformMoveInventory(string strEnvId, string strCurCompany, string strPartNum, string strIUM, decimal dTranQty, string strFromWarehouseCode,
                                                        string strFromBinNum, string strFromLotNum, string strToWarehouseCode, string strToBinNum, string strToLotNum, 
                                                        string strReference)
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
                MoveInventory oMove = new MoveInventory();

                oMove.Company = strCurCompany;
                oMove.PartNum = strPartNum;
                oMove.IUM = strIUM;
                oMove.TranQty = dTranQty;

                oMove.FromWarehouseCode = strFromWarehouseCode;
                oMove.FromLotNum = strFromLotNum;
                oMove.FromBinNum = strFromBinNum;

                oMove.ToWarehouseCode = strToWarehouseCode;
                oMove.ToLotNum = strToLotNum;
                oMove.ToBinNum = strToBinNum;

                oMove.Reference = strReference;

                IsTrxComplete = oEpicor._EpicInvTransfer( ref oEpicorEnv, ref oMove, out strReturnMsg);

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


        [HttpPost]
        public HttpResponseMessage PerformMoveInventoryRequest(string strEnvId, string strCurCompany, string strPartNum, string strIUM, decimal dTranQty, string strFromWarehouseCode,
                                                        string strFromBinNum, string strFromLotNum, string strToWarehouseCode, string strToBinNum, string strToLotNum,
                                                        string strReference)
        {
            string strReturnMsg;
            bool IsTrxComplete = false;
            bool IsComplete = false;

            EpicEnv oEpicorEnv = new EpicEnv();
            EpicEnvBO oEpicorEnvBO = new EpicEnvBO();
            IsComplete = oEpicorEnvBO._LoadEpicEnvById(strEnvId, ref oEpicorEnv, out strReturnMsg);

            if (IsComplete)
            {
                MoveInventoryBO oMoveInventory = new MoveInventoryBO();
                MoveInventory oMove = new MoveInventory();

                oMove.Company = strCurCompany;
                oMove.PartNum = strPartNum;
                oMove.IUM = strIUM;
                oMove.TranQty = dTranQty;

                oMove.FromWarehouseCode = strFromWarehouseCode;
                oMove.FromLotNum = strFromLotNum;
                oMove.FromBinNum = strFromBinNum;

                oMove.ToWarehouseCode = strToWarehouseCode;
                oMove.ToLotNum = strToLotNum;
                oMove.ToBinNum = strToBinNum;

                oMove.Reference = strReference;
                oMove.ReqStatus = 0;

                IsTrxComplete = oMoveInventory._NewMoveInvTrxReq(ref oEpicorEnv, ref oMove, out strReturnMsg);

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


        [HttpPost]
        public HttpResponseMessage PerformMoveInventoryRequestApproval(string strEnvId, string strCurCompany, string strReqNum, int iReqStatus)
        {
            string strReturnMsg;
            bool IsTrxComplete = false;
            bool IsEpicTrxComplete = false;
            bool IsComplete = false;
            bool IsLoadTrxOK = false;

            EpicEnv oEpicorEnv = new EpicEnv();
            EpicEnvBO oEpicorEnvBO = new EpicEnvBO();
            IsComplete = oEpicorEnvBO._LoadEpicEnvById(strEnvId, ref oEpicorEnv, out strReturnMsg);

            if (IsComplete)
            {
                MoveInventoryBO oMoveInventory = new MoveInventoryBO();
                MoveInventory oMove = new MoveInventory();

                IsLoadTrxOK = oMoveInventory._LoadMoveInvTrxById(ref oEpicorEnv, strCurCompany, strReqNum, ref oMove, out strReturnMsg);
                
                if (IsLoadTrxOK)
                {
                    oMove.ReqStatus = iReqStatus;

                    if (iReqStatus == 1)
                    {
                        EpicorBO oEpicor = new EpicorBO();

                        IsEpicTrxComplete = oEpicor._EpicInvTransfer(ref oEpicorEnv, ref oMove, out strReturnMsg);
                    }

                    if (IsEpicTrxComplete)
                    {
                        IsTrxComplete = oMoveInventory._UpdMoveInvTrxStatus(ref oEpicorEnv, ref oMove, out strReturnMsg);
                    }
                    
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
                } // end for end if isloadtrxok

            }
            else
            {
                HttpError err = new HttpError(strReturnMsg);
                return Request.CreateResponse(HttpStatusCode.NotFound, err);
            } // end for iscomplete


        }



    }
}
