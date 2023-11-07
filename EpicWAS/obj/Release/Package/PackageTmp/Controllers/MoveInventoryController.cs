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
        public HttpResponseMessage LoadPart_Antah(string strUID, string strPass, string strCurCompany, string strCurPlant, string strEnvId, string strPartNum)
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

                    IsLoadPartOK = oPartBO._LoadPartById_Antah(ref oEpicorEnv, strCurCompany, strPartNum, ref oPart, out strReturnMsg);

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
        public HttpResponseMessage LoadMoveInventoryRequest(string strUID, string strPass, string strCurCompany, int iReqStatus, string strEnvId, string strToWarehouse = "", string strToBin = "")
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

                    IsLoadRequestOK = oMove._LoadMoveInvTrx(ref oEpicorEnv, strCurCompany, iReqStatus, ref oMoveInventoryList, out strReturnMsg, strToWarehouse, strToBin);

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
        public HttpResponseMessage PerformMoveInventory(string strUID, string strPass, string strEnvId, string strCurCompany, string strPartNum, string strIUM, decimal dTranQty, string strFromWarehouseCode,
                                                        string strFromBinNum, string strFromLotNum, string strToWarehouseCode, string strToBinNum, string strToLotNum, 
                                                        string strReference, string strCurPlant, int iLabelCount = 1)
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
            { IsPartNumOK = oValidate._CheckPartNum(ref oEpicorEnv, strCurCompany, strPartNum, "[MI01]", ref oErrors); }

            if (IsPartNumOK)
            { oPartBO._LoadPartById(ref oEpicorEnv, strCurCompany, strPartNum, ref oPart, out strReturnMsg); }

            if (IsPartNumOK && oPart.TrackLots)
            {
                if (ConfigurationManager.AppSettings["EnableCheckPartLot"].Equals("true"))
                {
                    oValidate._CheckPartLot(ref oEpicorEnv, strCurCompany, strPartNum, strFromLotNum, "[MI02]", ref oErrors);
                    oValidate._CheckPartLot(ref oEpicorEnv, strCurCompany, strPartNum, strToLotNum, "[MI02]", ref oErrors); // 22072019 ADD TO LOT VALIDATION
                }
            }

            if (ConfigurationManager.AppSettings["EnableCheckWhse"].Equals("true"))
            {
                oValidate._CheckWhse(ref oEpicorEnv, strCurCompany, strCurPlant, strToWarehouseCode, "[MI05]", ref oErrors);
                oValidate._CheckWhse(ref oEpicorEnv, strCurCompany, strCurPlant, strFromWarehouseCode, "[MI03]", ref oErrors);
            }

            if (ConfigurationManager.AppSettings["EnableCheckPartWhse"].Equals("true"))
            {
                oValidate._CheckPartWhse(ref oEpicorEnv, strCurCompany, strToWarehouseCode, strPartNum, "[MI05]", ref oErrors);
                oValidate._CheckPartWhse(ref oEpicorEnv, strCurCompany, strFromWarehouseCode, strPartNum, "[MI03]", ref oErrors);
            }

            if (ConfigurationManager.AppSettings["EnableCheckWhseBin"].Equals("true"))
            {
                oValidate._CheckWhseBinNum(ref oEpicorEnv, strCurCompany, strFromWarehouseCode, strFromBinNum, "[MI04]", ref oErrors);
                oValidate._CheckWhseBinNum(ref oEpicorEnv, strCurCompany, strToWarehouseCode, strToBinNum, "[MI06]", ref oErrors);
            }

            if (ConfigurationManager.AppSettings["EnableCheckAvailableQty"].Equals("true"))
            { oValidate._CheckAvailableQty(ref oEpicorEnv, strCurCompany, strPartNum, strFromWarehouseCode, strFromBinNum, strFromLotNum, strIUM, dTranQty, "[MI07]", ref oErrors); }




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
                MoveInventory oMove = new MoveInventory();

                oMove.Company = strCurCompany;
                oMove.PartNum = oSpecialChar.replaceSpecialCharCSharp( strPartNum);
                oMove.IUM = strIUM;
                oMove.TranQty = dTranQty;
                oMove.CurrentPlant = strCurPlant;

                oMove.FromWarehouseCode = strFromWarehouseCode;
                string strTempFromLot = (strFromLotNum == null ? string.Empty : strFromLotNum);
                oMove.FromLotNum = strTempFromLot;
                oMove.FromBinNum = strFromBinNum;

                oMove.ToWarehouseCode = strToWarehouseCode;
                string strTempToLot = (strToLotNum == null ? string.Empty : strToLotNum);
                oMove.ToLotNum = strTempToLot;

                oMove.ToBinNum = strToBinNum;
                string strTempRef = (strReference == null ? string.Empty : strReference);
                oMove.Reference = strTempRef;

                oMove.LabelCount = iLabelCount;

                IsTrxComplete = oEpicor._EpicInvTransfer( ref oEpicorEnv, ref oMove, out strReturnMsg, strUID, strPass);

                if (IsTrxComplete)
                {
                    SQLServerBO oSQL = new SQLServerBO();
                    string _strSQLCon = oSQL._retSQLConnectionString();
                    _strSQLCon = string.Format(_strSQLCon, oEpicorEnv.Env_SQLServer, oEpicorEnv.Env_SQLDB, oEpicorEnv.Env_SQLUserId, oEpicorEnv.Env_SQLPassKey);

                    oSQL._exeSDSCreateLabelSP(_strSQLCon, strCurCompany, strUID, "STK-STK", DateTime.Now,oSpecialChar.replaceSpecialCharCSharp( strPartNum), strIUM, (strToLotNum == null ? string.Empty : strToLotNum), dTranQty, iLabelCount, "", "", oMove.TranNum);


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
        public HttpResponseMessage PerformMoveInventoryRequest(string strUID, string strPass, string strEnvId, string strCurCompany, string strPartNum, string strIUM, decimal dTranQty, string strFromWarehouseCode,
                                                        string strFromBinNum, string strFromLotNum, string strToWarehouseCode, string strToBinNum, string strToLotNum,
                                                        string strReference, string strCurPlant = "", int iLabelCount = 1)
        {
            string strReturnMsg;
            bool IsTrxComplete = false;
            bool IsComplete = false;
            SpecialCharHandler oSpecialChar = new SpecialCharHandler();

            EpicEnv oEpicorEnv = new EpicEnv();
            EpicEnvBO oEpicorEnvBO = new EpicEnvBO();
            IsComplete = oEpicorEnvBO._LoadEpicEnvById(strEnvId, ref oEpicorEnv, out strReturnMsg);

            //added on 20190612
            bool IsPartNumOK = false;
            IList<Error> oErrors = new List<Error>();

            ErrorBO oValidate = new ErrorBO();
            PartBO oPartBO = new PartBO();

            Part oPart = new Part();

            if (ConfigurationManager.AppSettings["EnableCheckPartNum"].Equals("true"))
            { IsPartNumOK = oValidate._CheckPartNum(ref oEpicorEnv, strCurCompany, strPartNum, "[MI01]", ref oErrors); }

            if (IsPartNumOK)
            { oPartBO._LoadPartById(ref oEpicorEnv, strCurCompany, strPartNum, ref oPart, out strReturnMsg); }

            if (IsPartNumOK && oPart.TrackLots)
            {
                if (ConfigurationManager.AppSettings["EnableCheckPartLot"].Equals("true"))
                {
                    oValidate._CheckPartLot(ref oEpicorEnv, strCurCompany, strPartNum, strFromLotNum, "[MI02]", ref oErrors);
                    oValidate._CheckPartLot(ref oEpicorEnv, strCurCompany, strPartNum, strToLotNum, "[MI02]", ref oErrors); // 22072019 ADD TO LOT VALIDATION
                }
            }

            if (ConfigurationManager.AppSettings["EnableCheckWhse"].Equals("true"))
            {
                oValidate._CheckWhse(ref oEpicorEnv, strCurCompany, strCurPlant, strToWarehouseCode, "[MI05]", ref oErrors);
                oValidate._CheckWhse(ref oEpicorEnv, strCurCompany, strCurPlant, strFromWarehouseCode, "[MI03]", ref oErrors);
            }

            if (ConfigurationManager.AppSettings["EnableCheckPartWhse"].Equals("true"))
            {
                oValidate._CheckPartWhse(ref oEpicorEnv, strCurCompany, strToWarehouseCode, strPartNum, "[MI05]", ref oErrors);
                oValidate._CheckPartWhse(ref oEpicorEnv, strCurCompany, strFromWarehouseCode, strPartNum, "[MI03]", ref oErrors);
            }

            if (ConfigurationManager.AppSettings["EnableCheckWhseBin"].Equals("true"))
            {
                oValidate._CheckWhseBinNum(ref oEpicorEnv, strCurCompany, strFromWarehouseCode, strFromBinNum, "[MI04]", ref oErrors);
                oValidate._CheckWhseBinNum(ref oEpicorEnv, strCurCompany, strToWarehouseCode, strToBinNum, "[MI06]", ref oErrors);
            }

            if (ConfigurationManager.AppSettings["EnableCheckAvailableQty"].Equals("true"))
            { oValidate._CheckAvailableQty(ref oEpicorEnv, strCurCompany, strPartNum, strFromWarehouseCode, strFromBinNum, strFromLotNum, strIUM, dTranQty, "[MI07]", ref oErrors); }

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
                MoveInventoryBO oMoveInventory = new MoveInventoryBO();
                MoveInventory oMove = new MoveInventory();

                oMove.Company = strCurCompany;
                oMove.PartNum = oSpecialChar.replaceSpecialCharCSharp( strPartNum);
                oMove.IUM = strIUM;
                oMove.TranQty = dTranQty;
                oMove.ReqNum = DateTime.Now.ToString("yyyyMMddHHmmss");

                oMove.FromWarehouseCode = strFromWarehouseCode;
                oMove.FromLotNum = strFromLotNum;
                oMove.FromBinNum = strFromBinNum;

                oMove.ToWarehouseCode = strToWarehouseCode;
                oMove.ToLotNum = strToLotNum;
                oMove.ToBinNum = strToBinNum;

                oMove.Reference = strReference;
                oMove.ReqStatus = 0;
                oMove.ReqBy = strUID;
                oMove.UpdBy = strUID;
                oMove.UpdDate = DateTime.Now;
                oMove.ReqDate = DateTime.Now;
                oMove.LabelCount = iLabelCount;
                oMove.CurrentPlant = strCurPlant;

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
        public HttpResponseMessage PerformMoveInventoryRequestApproval(string strUID, string strPass, string strEnvId, string strCurCompany, string strReqNum, int iReqStatus, decimal dTranQty = 0, int iLabelCount = 0)
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
                    if (dTranQty > 0)
                    {
                        oMove.TranQty = dTranQty;
                    }

                    if (iLabelCount > 0)
                    {
                        oMove.LabelCount = iLabelCount;
                    }

                    oMove.ReqStatus = iReqStatus;

                    if (iReqStatus == 1)
                    {

                        ErrorBO oValidate = new ErrorBO();
                        IList<Error> oErrors = new List<Error>();

                        if (ConfigurationManager.AppSettings["EnableCheckAvailableQty"].Equals("true"))
                        { oValidate._CheckAvailableQty(ref oEpicorEnv, strCurCompany, oMove.PartNum, oMove.FromWarehouseCode, oMove.FromBinNum, oMove.FromLotNum, oMove.IUM, oMove.TranQty, "[MI07]", ref oErrors); }


                        if (oErrors.Count > 0)
                        {
                            string strErr = "";

                            foreach (Error oErr in oErrors)
                            {
                                strErr += oErr.ErrorCode + " " + oErr.ErrorDescription + ": ";
                            }
                            return Request.CreateResponse(HttpStatusCode.ExpectationFailed, strErr);
                        }


                        EpicorBO oEpicor = new EpicorBO();
                        IsEpicTrxComplete = oEpicor._EpicInvTransfer(ref oEpicorEnv, ref oMove, out strReturnMsg, strUID, strPass);

                        if (IsEpicTrxComplete)
                        {
                            IsTrxComplete = oMoveInventory._UpdMoveInvTrxStatus(ref oEpicorEnv, ref oMove, out strReturnMsg);
                        }
                    }
                    else
                    {
                        EpicorBO oEpicor = new EpicorBO();
                        IsTrxComplete = oMoveInventory._UpdMoveInvTrxStatus(ref oEpicorEnv, ref oMove, out strReturnMsg);

                    }



                    if (IsTrxComplete)
                    {
                        SQLServerBO oSQL = new SQLServerBO();
                        string _strSQLCon = oSQL._retSQLConnectionString();
                        _strSQLCon = string.Format(_strSQLCon, oEpicorEnv.Env_SQLServer, oEpicorEnv.Env_SQLDB, oEpicorEnv.Env_SQLUserId, oEpicorEnv.Env_SQLPassKey);

                        oSQL._exeSDSCreateLabelSP(_strSQLCon, strCurCompany, strUID, "STK-STK", DateTime.Now, oMove.PartNum, oMove.IUM, (oMove.ToLotNum == null ? string.Empty : oMove.ToLotNum), oMove.TranQty, oMove.LabelCount, "", "", "");


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
