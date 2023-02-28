using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using EpicWAS.Models;

namespace EpicWAS.Controllers
{
    public class SetupController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage LoadVendors(string strUID, string strPass, string strCurCompany, string strCurPlant, string strEnvId, string strVendorId, string strVendorName, string strGroupCode)
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsLoadVendorOK = false;

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
                    IList<Vendor> Vendors = new List<Vendor>();
                    MaintenanceBO oMaintenanceBO = new MaintenanceBO();

                    IsLoadVendorOK = oMaintenanceBO._LoadVendors(ref oEpicorEnv, strCurCompany, strVendorId, strVendorName, strGroupCode, ref Vendors, out strReturnMsg);


                    if (IsLoadVendorOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, Vendors);
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
        public HttpResponseMessage LoadVendorById(string strUID, string strPass, string strCurCompany, string strCurPlant, string strEnvId, string strVendorId)
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsLoadVendorOK = false;

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
                    Vendor oVendor = new Vendor();
                    MaintenanceBO oMaintenanceBO = new MaintenanceBO();

                    IsLoadVendorOK = oMaintenanceBO._LoadVendorById(ref oEpicorEnv, strCurCompany, strVendorId, ref oVendor, out strReturnMsg);


                    if (IsLoadVendorOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, oVendor);
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
