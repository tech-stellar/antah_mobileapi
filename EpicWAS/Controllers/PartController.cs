using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using EpicWAS.Models;

namespace EpicWAS.Controllers
{
    public class PartController : ApiController
    {

        [HttpGet]
        public HttpResponseMessage LoadPartWhseById(string strUID, string strPass, string strCurCompany, string strCurPlant, string strEnvId, string strPartNum, string strWhse)
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

                    IsLoadPartWhseOK = oPartBO._LoadPartWhseById(ref oEpicorEnv, strCurCompany, strPartNum, strWhse, ref oPartWhse, out strReturnMsg);


                    if (IsLoadPartWhseOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, oPartWhse);
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
        public HttpResponseMessage LoadPartWhseBinById(string strUID, string strPass, string strCurCompany, string strCurPlant, string strEnvId, string strPartNum, string strWhse, string strBinNum)
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsLoadPartWhseBinOK = false;

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
                    PartBin oPartBin = new PartBin();

                    IsLoadPartWhseBinOK = oPartBO._LoadPartWhseBinById(ref oEpicorEnv, strCurCompany, strPartNum, strWhse, strBinNum,ref oPartBin, out strReturnMsg);


                    if (IsLoadPartWhseBinOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, oPartBin);
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

                    IsLoadPartOK = oPartBO._LoadPartById(ref oEpicorEnv, strCurCompany, strPartNum, ref oPart, out strReturnMsg);

                    if (IsLoadPartOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, oPart);

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


        // added on 20190612
        [HttpGet]
        public HttpResponseMessage LoadUOMByPart(string strUID, string strPass, string strCurCompany, string strCurPlant, string strEnvId, string strPartNum)
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsLoadUOMOK = false;

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
                    IList<UOM> UOMList = new List<UOM>();
                    PartBO oPartBO = new PartBO();

                    IsLoadUOMOK = oPartBO._LoadUOMForPart(ref oEpicorEnv, strCurCompany, strPartNum, ref UOMList, out strReturnMsg);

                    if (IsLoadUOMOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, UOMList);
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
                }

            }
            else
            {
                HttpError err = new HttpError(strReturnMsg);
                return Request.CreateResponse(HttpStatusCode.NotFound, err);
            }


        }


        [HttpGet]
        public HttpResponseMessage LoadWarehouses(string strUID, string strPass, string strCurCompany, string strCurPlant, string strEnvId, string strWhse, string strWhseDesc)
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
                    IList<Warehse> Wareshouse = new List<Warehse>();
                    PartBO oPartBO = new PartBO();

                    IsLoadPartWhseOK = oPartBO._LoadWarehouse(ref oEpicorEnv, strCurCompany, strWhse, strWhseDesc, ref Wareshouse, out strReturnMsg, strCurPlant);

                    if (IsLoadPartWhseOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, Wareshouse);
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
        public HttpResponseMessage LoadWarehouseBins(string strUID, string strPass, string strCurCompany, string strCurPlant, string strEnvId, string strWhse, string strBin, string strBinDesc)
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
                    IList<WhseBin> WareshouseBins = new List<WhseBin>();
                    PartBO oPartBO = new PartBO();

                    IsLoadPartWhseOK = oPartBO._LoadWarehouseBin(ref oEpicorEnv, strCurCompany, strWhse, strBin, strBinDesc, ref WareshouseBins, out strReturnMsg);


                    if (IsLoadPartWhseOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, WareshouseBins);
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
        public HttpResponseMessage LoadPartLotBins(string strUID, string strPass, string strCurCompany, string strCurPlant, string strEnvId, string strPartNum, string strLotNum, string strWarehouse, string strBin, string strProdGroup="")
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
                    IList<PartLot> PartLots = new List<PartLot>();

                    IsLoadPartLotOK = oPartBO._LoadPartLotBin(ref oEpicorEnv, strCurCompany, strPartNum, strLotNum, strWarehouse, strBin, strProdGroup, ref PartLots, out strReturnMsg);

                    if (IsLoadPartLotOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, PartLots);
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
        public HttpResponseMessage LoadPartLotBins_v2(string strUID, string strPass, string strCurCompany, string strCurPlant, string strEnvId, string strPartNum, string strLotNum, string strWarehouse, string strBin, string strProdGroup = "", string strPartDesc = "", string strTypeCode = "")
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
                    IList<PartLot> PartLots = new List<PartLot>();

                    IsLoadPartLotOK = oPartBO._LoadPartLotBin_v2(ref oEpicorEnv, strCurCompany, strPartNum, strPartDesc, strLotNum, strWarehouse, strBin, strProdGroup, strTypeCode, ref PartLots, out strReturnMsg);

                    if (IsLoadPartLotOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, PartLots);
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
        public HttpResponseMessage LoadParts(string strUID, string strPass, string strCurCompany, string strCurPlant, string strEnvId, string strPartNum, string strPartDesc, string strProdGroup, string strTypeCode)
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

                    IList<Part> Parts = new List<Part>();

                    IsLoadPartOK = oPartBO._LoadParts(ref oEpicorEnv, strCurCompany, strPartNum, strPartDesc, strProdGroup, strTypeCode, ref Parts, out strReturnMsg);

                    if (IsLoadPartOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, Parts);

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
        public HttpResponseMessage LoadStockReplenishments(string strUID, string strPass, string strCurCompany, string strCurPlant, string strEnvId, string strPartNum, string strWhseCode, string strBinNum, string strLotNum)
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsLoadStkReplOK = false;

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

                    IList<StkRepl> StkRepls = new List<StkRepl>();

                    IsLoadStkReplOK = oPartBO._StockReplenishment(ref oEpicorEnv, strCurCompany, strPartNum, strWhseCode, strBinNum, strLotNum, ref StkRepls, out strReturnMsg);

                    if (IsLoadStkReplOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, StkRepls);

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
        public HttpResponseMessage LoadStockReplenishments2(string strUID, string strPass, string strCurCompany, string strCurPlant, string strEnvId, string strPartNum, string strWhseCode, string strAgency)
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsLoadStkReplOK = false;

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

                    IList<StkRepl2> StkRepls = new List<StkRepl2>();

                    IsLoadStkReplOK = oPartBO._StockReplenishment2(ref oEpicorEnv, strCurCompany, strUID, strPartNum, strWhseCode, strAgency, ref StkRepls, out strReturnMsg);

                    if (IsLoadStkReplOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, StkRepls);
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
        public HttpResponseMessage LoadStockReplenishments2Sub(string strUID, string strPass, string strCurCompany, string strCurPlant, string strEnvId, string strPartNum, string strWarehouse)
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsLoadStkReplOK = false;

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

                    IList<StkRepl2Sub> StkReplSubs = new List<StkRepl2Sub>();

                    IsLoadStkReplOK = oPartBO._StockReplenishment2Sub(ref oEpicorEnv, strCurCompany, strPartNum, strWarehouse, ref StkReplSubs, out strReturnMsg);

                    if (IsLoadStkReplOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, StkReplSubs);
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



        [HttpPost]
        public HttpResponseMessage PerformPartLotUpdate(string strUID, string strPass, string strEnvId, string strCurCompany, string strPartNum = "", string strLotNum = "",
                                                        string strLotDesc = "", string strBatch = "", string strMfgBatch = "", string strMfgLot = "",
                                                        string strHeatNum = "", string strFirmWare = "", string dtBestBeforeDt = "",
                                                        string dtMfgDt = "", string dtCureDt = "", string dtExpireDt = "", int iLabelCount = 1)
        {
            string strReturnMsg;
            bool IsComplete = false;
            bool IsTrxComplete = false;
            bool IsLoadPartOK = false;
            SpecialCharHandler oSpecialChar = new SpecialCharHandler();

            IList<Error> oErrors = new List<Error>();


            EpicEnv oEpicorEnv = new EpicEnv();
            EpicEnvBO oEpicorEnvBO = new EpicEnvBO();
            IsComplete = oEpicorEnvBO._LoadEpicEnvById(strEnvId, ref oEpicorEnv, out strReturnMsg);

            // added in 2019-05-27
            Part oPart = new Part();
            PartBO oPartBO = new PartBO();

            IsLoadPartOK = oPartBO._LoadPartById(ref oEpicorEnv, strCurCompany, strPartNum, ref oPart, out strReturnMsg);

            if (IsLoadPartOK)
            {

                if (oPart.AttBatch == "M" && (strBatch == "" || strBatch == null))
                {
                    Error oErr = new Error();
                    oErr.ErrorCode = "[LT01]";
                    oErr.ErrorDescription = String.Format("Batch is required");

                    oErrors.Add(oErr);
                }

                if (oPart.AttMfgBatch == "M" && (strMfgBatch == "" || strMfgBatch == null))
                {
                    Error oErr = new Error();
                    oErr.ErrorCode = "[LT02]";
                    oErr.ErrorDescription = String.Format("Mfg Batch is required");

                    oErrors.Add(oErr);
                }

                if (oPart.AttMfgLot == "M" && (strMfgLot == "" || strMfgLot == null))
                {
                    Error oErr = new Error();
                    oErr.ErrorCode = "[LT03]";
                    oErr.ErrorDescription = String.Format("Mfg Lot is required");

                    oErrors.Add(oErr);
                }

                if (oPart.AttHeat == "M" && (strHeatNum == "" || strHeatNum == null))
                {
                    Error oErr = new Error();
                    oErr.ErrorCode = "[LT04]";
                    oErr.ErrorDescription = String.Format("Heat Number is required");

                    oErrors.Add(oErr);
                }

                if (oPart.AttFirmware == "M" && (strFirmWare == "" || strFirmWare == null))
                {
                    Error oErr = new Error();
                    oErr.ErrorCode = "[LT05]";
                    oErr.ErrorDescription = String.Format("Firmware is required");

                    oErrors.Add(oErr);
                }

                if (oPart.AttBeforeDt == "M" && (dtBestBeforeDt == "" || dtBestBeforeDt == null))
                {
                    Error oErr = new Error();
                    oErr.ErrorCode = "[LT06]";
                    oErr.ErrorDescription = String.Format("Best Before Date is required");

                    oErrors.Add(oErr);
                }

                if (oPart.AttMfgDt == "M" && (dtMfgDt == "" || dtMfgDt == null))
                {
                    Error oErr = new Error();
                    oErr.ErrorCode = "[LT07]";
                    oErr.ErrorDescription = String.Format("Original Mfg Date is required");

                    oErrors.Add(oErr);
                }

                if (oPart.AttCureDt == "M" && (dtCureDt == "" || dtCureDt == null))
                {
                    Error oErr = new Error();
                    oErr.ErrorCode = "[LT08]";
                    oErr.ErrorDescription = String.Format("Cure Date is required");

                    oErrors.Add(oErr);
                }

                if (oPart.AttExpDt == "M" && (dtExpireDt == "" || dtExpireDt == null))
                {
                    Error oErr = new Error();
                    oErr.ErrorCode = "[LT09]";
                    oErr.ErrorDescription = String.Format("Expire Date is required");

                    oErrors.Add(oErr);
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

            }
            else
            {
                HttpError err = new HttpError(strReturnMsg);
                return Request.CreateResponse(HttpStatusCode.NotFound, err);
            }



            if (IsComplete)
            {
                EpicorBO oEpicor = new EpicorBO();
                PartLot oPartLot = new PartLot();

                oPartLot.Batch = (strBatch == null ? "" : strBatch) ;
                oPartLot.BestBeforeDt = (dtBestBeforeDt == null ? "" : dtBestBeforeDt);
                oPartLot.Company = strCurCompany;
                oPartLot.CureDt = (dtCureDt == null ? "" : dtCureDt);
                oPartLot.ExpireDt = (dtExpireDt == null ? "" : dtExpireDt);
                oPartLot.FirmWare = (strFirmWare == null ? "" : strFirmWare);
                oPartLot.HeatNum = (strHeatNum == null ? "" : strHeatNum);
                oPartLot.LotNum = (strLotNum == null ? "" : strLotNum);
                oPartLot.MfgBatch = (strMfgBatch == null ? "" : strMfgBatch);
                oPartLot.MfgDt = (dtMfgDt == null ? "" : dtMfgDt);
                oPartLot.MfgLot = (strMfgLot == null ? "" : strMfgLot);
                oPartLot.PartLotDescription = (strLotDesc == null ? "" : strLotDesc);
                oPartLot.PartNum = oSpecialChar.replaceSpecialCharCSharp( strPartNum);
                oPartLot.LabelCount = iLabelCount;

                IsTrxComplete = oEpicor._EpicLotSelectUpdate(ref oEpicorEnv, ref oPartLot, out strReturnMsg, strUID, strPass);

                if (IsTrxComplete)
                {
                    SQLServerBO oSQL = new SQLServerBO();
                    string _strSQLCon = oSQL._retSQLConnectionString();
                    _strSQLCon = string.Format(_strSQLCon, oEpicorEnv.Env_SQLServer, oEpicorEnv.Env_SQLDB, oEpicorEnv.Env_SQLUserId, oEpicorEnv.Env_SQLPassKey);

                    oSQL._exeSDSCreateLabelSP(_strSQLCon, strCurCompany, strUID, "PartLot", DateTime.Now, oSpecialChar.replaceSpecialCharCSharp( strPartNum), "", (strLotNum == null ? string.Empty : strLotNum), 0, iLabelCount, "", "", "");


                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    HttpError err = new HttpError(strReturnMsg);
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
                } // end if for istrxcomplete

            }
            else
            {
                HttpError err = new HttpError(strReturnMsg);
                return Request.CreateResponse(HttpStatusCode.NotFound, err);
            } // end if for iscomplete

        }

    }


}
