using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using EpicWAS.Models;

namespace EpicWAS.Controllers
{
    public class ReceiptController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage LoadReceiptsDetailsByPONum(string strUID, string strPass, string strEnvId,
                                                        string strCurCompany, string strCurPlant, int iPONum, string strLegalNumber="")
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsLoadReceiptListOK = false;

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
                    ReceiptBO oRecvBo = new ReceiptBO();

                    IList<ReceiptDetail> RecvDtlList = new List<ReceiptDetail>();

                    /* 2021-04-30 yew mun add in legal number parameter  */
                    IsLoadReceiptListOK = oRecvBo._LoadPORelDetailById(ref oEpicorEnv, strCurCompany, iPONum, ref RecvDtlList, strLegalNumber, out strReturnMsg);

                    if (IsLoadReceiptListOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, RecvDtlList);
                    }
                    else
                    {
                        HttpError err = new HttpError(strReturnMsg);
                        return Request.CreateResponse(HttpStatusCode.NotFound, err);
                    } // end if for IsLoadReceiptListOK



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
        public HttpResponseMessage LoadReceiptByPackSlip(string strUID, string strPass, string strEnvId,
                                                        string strCurCompany, string strCurPlant,int iPONum, string strPackSlip, string strLegalNumber="" )
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsLoadPackSlipOK = false;

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
                    ReceiptBO oRecvBo = new ReceiptBO();
                    ReceiptHead oRecvHed = new ReceiptHead();

                    IsLoadPackSlipOK = oRecvBo._LoadReceiptsById2(ref oEpicorEnv, strCurCompany, iPONum, strPackSlip, strLegalNumber, ref oRecvHed, out strReturnMsg);

                    if (IsLoadPackSlipOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, oRecvHed);
                    }
                    else
                    {
                        HttpError err = new HttpError(strReturnMsg);
                        return Request.CreateResponse(HttpStatusCode.NotFound, err);
                    } // end if for IsLoadReceiptListOK

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
        public HttpResponseMessage LoadPurchaseOrders(string strUID, string strPass, string strEnvId,
                                                        string strCurCompany, string strCurPlant, int iPONum, 
                                                        string dBeginDate, string dEndDate, string strVendorId, 
                                                        string strVendorName, string strBuyer, string strLegalNumber="" )
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsLoadPackSlipOK = false;

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
                    ReceiptBO oRecvBo = new ReceiptBO();
                    IList<PurchaseOrder> oPOList = new List<PurchaseOrder>();

                    /* 2021-04-30 yew mun add in legal number parameter  */
                    DateTime tmpDate;

                    if (!DateTime.TryParse(dBeginDate, out tmpDate))
                    {
                        // handle parse failure
                        dBeginDate = null;
                    }

                    if (!DateTime.TryParse(dEndDate, out tmpDate))
                    {
                        // handle parse failure
                        dEndDate = null;
                    }


                    IsLoadPackSlipOK = oRecvBo._LoadPurchaseOrders(ref oEpicorEnv, strCurCompany, iPONum, dBeginDate, dEndDate, strVendorId, strVendorName, strBuyer,  ref oPOList, strLegalNumber, out strReturnMsg);

                    if (IsLoadPackSlipOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, oPOList);
                    }
                    else
                    {
                        HttpError err = new HttpError(strReturnMsg);
                        return Request.CreateResponse(HttpStatusCode.NotFound, err);
                    } // end if for IsLoadReceiptListOK

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
        public HttpResponseMessage LoadPurchaseOrdersByLegalId(string strUID, string strPass, string strEnvId,
                                                        string strCurCompany, string strCurPlant, int iPONum,
                                                        string strLegalNumber = "")
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsLoadPackSlipOK = false;

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
                    ReceiptBO oRecvBo = new ReceiptBO();
                    IList<PurchaseOrder> oPOList = new List<PurchaseOrder>();


                    IsLoadPackSlipOK = oRecvBo._LoadPurchaseOrdersByLegalId(ref oEpicorEnv, strCurCompany, iPONum, ref oPOList, strLegalNumber, out strReturnMsg);

                    if (IsLoadPackSlipOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, oPOList);
                    }
                    else
                    {
                        HttpError err = new HttpError(strReturnMsg);
                        return Request.CreateResponse(HttpStatusCode.NotFound, err);
                    } // end if for IsLoadReceiptListOK

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
        public HttpResponseMessage LoadReceiptsDetails(string strUID, string strPass, string strEnvId,
                                                        string strCurCompany, string strCurPlant, int iPONum, string strLegalNumber = "", string strVendId = "", string strPartNum = "")
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsLoadReceiptListOK = false;

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
                    ReceiptBO oRecvBo = new ReceiptBO();

                    IList<ReceiptDetail> RecvDtlList = new List<ReceiptDetail>();

                    /* 2021-04-30 yew mun add in legal number parameter  */
                    IsLoadReceiptListOK = oRecvBo._LoadPORelDetail(ref oEpicorEnv, strCurCompany, iPONum, ref RecvDtlList, strLegalNumber, strVendId, strPartNum, out strReturnMsg);

                    if (IsLoadReceiptListOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, RecvDtlList);
                    }
                    else
                    {
                        HttpError err = new HttpError(strReturnMsg);
                        return Request.CreateResponse(HttpStatusCode.NotFound, err);
                    } // end if for IsLoadReceiptListOK



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
        public HttpResponseMessage PerformNewReceiptHead(string strUID, string strPass, string strEnvId, 
                                                        string strCurCompany, string strCurPlant, int iPONum, 
                                                        string strVendId, string strPackSlip)
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

                ReceiptDetail oRecHed = new  ReceiptDetail();
                oRecHed.Company = strCurCompany;
                oRecHed.EntryPerson = strUID;
                oRecHed.PONum = iPONum;
                oRecHed.VendorID = strVendId;
                oRecHed.PackSlip = strPackSlip;

                IsTrxComplete = oEpicor._EpicReceiptsHeader(ref oEpicorEnv, ref oRecHed, out strReturnMsg, strCurPlant, strUID, strPass);

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
        public HttpResponseMessage PerformNewReceiptDetail(string strUID, string strPass, string strEnvId,
                                                        string strCurCompany, string strCurPlant, int iVendNum, string strPackSlip, int iPONum,
                                                        int iPOLine, int iPORel,string strPartNum, string strWarehouseCode, string strBinNum, string strLotNum,
                                                        decimal dTranQty, string strUOM, int iLabelCount = 1)
        {
            string strReturnMsg;
            bool IsComplete = false;
            bool IsHeaderOK = false;
            bool IsDetailOK = false;
            bool ChkHeader = false;
            SpecialCharHandler oSpecialChar = new SpecialCharHandler();

            EpicEnv oEpicorEnv = new EpicEnv();
            EpicEnvBO oEpicorEnvBO = new EpicEnvBO();
            IsComplete = oEpicorEnvBO._LoadEpicEnvById(strEnvId, ref oEpicorEnv, out strReturnMsg);

            if (IsComplete)
            {
                EpicorBO oEpicor = new EpicorBO();

                ReceiptDetail oRecvDet = new ReceiptDetail();
                oRecvDet.Company = strCurCompany;
                oRecvDet.BinNum = strBinNum;
                oRecvDet.EntryPerson = strUID;
                oRecvDet.LotNum = (strLotNum == null ? "" : strLotNum); 
                oRecvDet.PackSlip = strPackSlip;
                oRecvDet.PartNum = oSpecialChar.replaceSpecialCharCSharp( strPartNum);

                PartBO oPartBO = new PartBO();
                Part oPart = new Part();

                oPartBO._LoadPartById(ref oEpicorEnv, strCurCompany, strPartNum, ref oPart, out strReturnMsg);


                oRecvDet.PartDesc = oPart.PartDescription;
                oRecvDet.POLine = iPOLine;
                oRecvDet.POLineRel = iPORel;
                oRecvDet.PONum = iPONum;
                oRecvDet.RecvUOM = strUOM;
                oRecvDet.VendorNum = iVendNum;
                oRecvDet.WarehouseCode = strWarehouseCode;
                oRecvDet.TranQty = dTranQty;
                oRecvDet.LabelCount = iLabelCount;


                // added on 2019-05-27
                ReceiptBO oReceiptBO = new ReceiptBO();
                ReceiptHead oRecvHead = new ReceiptHead();

                ChkHeader = oReceiptBO._LoadReceiptsById(ref oEpicorEnv, strCurCompany, iVendNum, strPackSlip, ref oRecvHead, out strReturnMsg);

                if (ChkHeader == false)
                {
                    IsHeaderOK = oEpicor._EpicReceiptsHeader(ref oEpicorEnv, ref oRecvDet, out strReturnMsg, strCurPlant, strUID, strPass);
                }
                else
                {
                    IsHeaderOK = true;
                }

                IsDetailOK = oEpicor._EpicReceiptsDetail(ref oEpicorEnv, ref oRecvDet, out strReturnMsg, strCurPlant, strUID, strPass);

                if (IsHeaderOK && IsDetailOK)
                {
                    SQLServerBO oSQL = new SQLServerBO();
                    string _strSQLCon = oSQL._retSQLConnectionString();
                    _strSQLCon = string.Format(_strSQLCon, oEpicorEnv.Env_SQLServer, oEpicorEnv.Env_SQLDB, oEpicorEnv.Env_SQLUserId, oEpicorEnv.Env_SQLPassKey);

                    string strParm01;

                    strParm01 = oRecvDet.VendorNum + "~" + oRecvDet.PurPoint + "~" + oRecvDet.PackSlip + "~" + oRecvDet.PackLine + "~" + oRecvDet.PONum + "~" + oRecvDet.POLine + "~" + oRecvDet.POLineRel;

                    oSQL._exeSDSCreateLabelSP(_strSQLCon, strCurCompany, strUID, "PUR-STK", DateTime.Now, oSpecialChar.replaceSpecialCharCSharp( strPartNum), strUOM, (strLotNum == null ? string.Empty : strLotNum), dTranQty, iLabelCount, strParm01, strWarehouseCode, strBinNum);

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
