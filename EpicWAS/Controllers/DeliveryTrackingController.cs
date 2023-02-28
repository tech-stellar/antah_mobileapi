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
    public class DeliveryTrackingController : ApiController
    {
        // GET: DeliveryTracking
        [HttpGet]
        public HttpResponseMessage LoadDOCustInfo(string strUID, string strPass, string strCurCompany, string strCurPlant, string strEnvId, string strLegalNumber)
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsLoadDOOK = false;
            bool IsLoadCustomerOK = false;

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
                    Customer oCustomer = new Customer();
                    CustShipHead oCustShipHead = new CustShipHead();

                    DeliveryTrackingVal oDeliveryTracking = new DeliveryTrackingVal();

                    IsLoadDOOK = oDeliveryTracking._GetPackSlip(ref oEpicorEnv, strCurCompany, strLegalNumber, ref oCustShipHead, out strReturnMsg);

                    if (!IsLoadDOOK)
                    {
                        HttpError err = new HttpError(strReturnMsg);
                        return Request.CreateResponse(HttpStatusCode.NotFound, err);
                    }
        
                    int iCustNum = oCustShipHead.CustNum;
                    oCustomer.Company = oCustShipHead.Company;

                    EpicorBO oEpicorBO = new EpicorBO();
                    IsLoadCustomerOK = oEpicorBO._EpicGetCustInfo(ref oEpicorEnv, ref oCustomer, iCustNum, out strReturnMsg, strUID, strPass);

                    if (IsLoadCustomerOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, oCustomer);
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
        public HttpResponseMessage PerformReceiveTimeStamp(string strUID, string strPass, string strCurCompany, string strCurPlant, string strEnvId, string strLegalNumber)
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsUD25OK = false;

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
                    EpicorBO oEpicorBO = new EpicorBO();
                    UD25 oUD25 = new UD25();

                    oUD25.Company = strCurCompany;

                    IsUD25OK = oEpicorBO._EpicUpdateDeliveryTime(ref oEpicorEnv, ref oUD25, strLegalNumber, out strReturnMsg, strUID, strPass);

                    if(IsUD25OK)
                    {
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

            } // end for iscomplete
        }
    }
}