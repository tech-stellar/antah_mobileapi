using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;

using EpicWAS.Models;
using System.Configuration;

namespace EpicWAS.Controllers
{
    public class SplitMergeUOMController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetSplitMergeList(string strUID, string strPass, string strCurCompany, string strCurPlant, string strEnvId, string strProc, string strPartNum, string strWarehouseCode, string strBinNum, string strLotNum, decimal dQty, string strUOM)
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsGETOK = false;

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
                    SplitMergeParam oSplitMergeParam = new SplitMergeParam();
                    oSplitMergeParam.Company = strCurCompany;
                    oSplitMergeParam.ProcessType = strProc;
                    oSplitMergeParam.PartNum = strPartNum;
                    oSplitMergeParam.WarehouseCode = strWarehouseCode;
                    oSplitMergeParam.BinNum = strBinNum;
                    oSplitMergeParam.LotNum = strLotNum;
                    oSplitMergeParam.Qty = dQty;
                    oSplitMergeParam.UOM = strUOM;
                    oSplitMergeParam.CurrentPlant = strCurPlant;

                    SplitMergeHead oSplitMergeHead = new SplitMergeHead();
                    IList<SplitMergeDetail> oSplitMergeDetailLst = new List<SplitMergeDetail>();

                    EpicorBO oEpicorBO = new EpicorBO();

                    IsGETOK = oEpicorBO._GetSplitMergeUOM(ref oEpicorEnv, ref oSplitMergeParam, ref oSplitMergeHead, ref oSplitMergeDetailLst, out strReturnMsg, strUID, strPass);

                    if (IsGETOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, oSplitMergeDetailLst);
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

            } // end for i
        }

        [HttpPost]
        public HttpResponseMessage PostSplitMergeList(string strUID, string strPass, string strCurCompany, string strCurPlant, string strEnvId, string strProc, string strPartNum, string strWarehouseCode, string strBinNum, string strLotNum, decimal dQty, string strUOM, string strSplitArray, int iLabelCount = 1)
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsPOSTOK = false;
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
                    SplitMergeParam oSplitMergeParam = new SplitMergeParam();
                    oSplitMergeParam.Company = strCurCompany;
                    oSplitMergeParam.ProcessType = strProc;
                    oSplitMergeParam.PartNum = oSpecialChar.replaceSpecialCharCSharp( strPartNum);
                    oSplitMergeParam.WarehouseCode = strWarehouseCode;
                    oSplitMergeParam.BinNum = strBinNum;
                    oSplitMergeParam.LotNum = strLotNum;
                    oSplitMergeParam.Qty = dQty;
                    oSplitMergeParam.UOM = strUOM;
                    oSplitMergeParam.CurrentPlant = strCurPlant;

                    SplitMergeHead oSplitMergeHead = new SplitMergeHead();
                    IList<SplitMergeDetail> oSplitMergeDetailLst = JsonConvert.DeserializeObject<IList<SplitMergeDetail>>(strSplitArray);
                    //IList<SplitMergeDetail> oSplitMergeDetailLst = new List<SplitMergeDetail>();

                    EpicorBO oEpicorBO = new EpicorBO();

                    IsPOSTOK = oEpicorBO._PostSplitMergeUOM(ref oEpicorEnv, ref oSplitMergeParam, ref oSplitMergeHead, ref oSplitMergeDetailLst, out strReturnMsg, strUID, strPass);

                    if (IsPOSTOK)
                    {
                        SQLServerBO oSQL = new SQLServerBO();
                        string _strSQLCon = oSQL._retSQLConnectionString();
                        _strSQLCon = string.Format(_strSQLCon, oEpicorEnv.Env_SQLServer, oEpicorEnv.Env_SQLDB, oEpicorEnv.Env_SQLUserId, oEpicorEnv.Env_SQLPassKey);

                        if (strProc == "S")
                        {
                            foreach (SplitMergeDetail smd in oSplitMergeDetailLst)
                            {
                                if (smd.Qty != 0)
                                {
                                    oSQL._exeSDSCreateLabelSP(_strSQLCon, strCurCompany, strUID, "SPLIT", DateTime.Now, oSpecialChar.replaceSpecialCharCSharp( strPartNum), smd.UOM, (strLotNum == null ? string.Empty : strLotNum), smd.Qty, iLabelCount, "", "", "");
                                }
                            }
                        }
                        else if (strProc == "M")
                        {
                            oSQL._exeSDSCreateLabelSP(_strSQLCon, strCurCompany, strUID, "MERGE", DateTime.Now, oSpecialChar.replaceSpecialCharCSharp( strPartNum), strUOM, (strLotNum == null ? string.Empty : strLotNum), dQty, iLabelCount, "", "", "");
                        }
                            
                        return Request.CreateResponse(HttpStatusCode.OK);
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

            } // end for i
        }
    }
}