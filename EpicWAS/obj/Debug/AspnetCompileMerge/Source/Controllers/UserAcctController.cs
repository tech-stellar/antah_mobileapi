using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using EpicWAS.Models;

namespace EpicWAS.Controllers
{
   
    public class UserAcctController : ApiController
    {
        [HttpGet]
        [ActionName("VerifyUserLogin")]
        public HttpResponseMessage VerifyUserLogin(string strUID, string strPass, string strEnvId)
        {
            string strReturnMsg;
            bool IsComplete = false;
            bool IsLogin = false;

            EpicEnv oEpicorEnv = new EpicEnv();
            EpicUser oEpicUser = new EpicUser();

            oEpicUser.Epic_UserId = strUID;
            oEpicUser.Epic_PassKey = strPass;

            EpicEnvBO oEpicorEnvBO = new EpicEnvBO();
            IsComplete = oEpicorEnvBO._LoadEpicEnvById(strEnvId, ref oEpicorEnv, out strReturnMsg);
            //oEpicorEnvBO.Dispose(true);

            if (IsComplete)
            {
                EpicUserBO oEpicUserBO = new EpicUserBO();
                IsLogin = oEpicUserBO._VerifyEpicorLogin(ref oEpicorEnv, ref oEpicUser, out strReturnMsg);
                //oEpicUserBO.Dispose(true);

                if (IsLogin)
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
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
                return Request.CreateResponse(HttpStatusCode.Unauthorized, err);
            }

            


        }

        [HttpGet]
        [ActionName("LoadActiveEpicEnvironment")]
        public HttpResponseMessage LoadActiveEpicEnvironment()
        {
            string strReturnMsg;
            IList<EpicEnv> EpicEnvList = new List<EpicEnv>();

            EpicEnvBO oEpicEnvBO = new EpicEnvBO();
            oEpicEnvBO._LoadEpicEnv(true, ref EpicEnvList, out strReturnMsg);

            if (EpicEnvList.Count == 0)
            {
                HttpError err = new HttpError(strReturnMsg);
                return Request.CreateResponse(HttpStatusCode.NotFound, err);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, EpicEnvList);
            }

        }

        [HttpGet]
        [ActionName("LoadEpicEnvironment")]
        public HttpResponseMessage LoadEpicEnvironment()
        {
            string strReturnMsg;
            IList<EpicEnv> EpicEnvList = new List<EpicEnv>();

            EpicEnvBO oEpicEnvBO = new EpicEnvBO();
            oEpicEnvBO._LoadEpicEnv(false, ref EpicEnvList, out strReturnMsg);

            if (EpicEnvList.Count == 0)
            {
                HttpError err = new HttpError(strReturnMsg);
                return Request.CreateResponse(HttpStatusCode.NotFound, err);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, EpicEnvList);
            }

        }

        [HttpGet]
        [ActionName("LoadEpicEnvironmentById")]
        public HttpResponseMessage LoadEpicEnvironmentById(string strEnvId)
        {
            string strReturnMsg;
            bool IsComplete = false;

            EpicEnv oEpicEnv = new EpicEnv();
            EpicEnvBO oEpicEnvBO = new EpicEnvBO();

            IsComplete = oEpicEnvBO._LoadEpicEnvById(strEnvId, ref oEpicEnv, out strReturnMsg);

            if (IsComplete == false)
            {
                HttpError err = new HttpError(strReturnMsg);
                return Request.CreateResponse(HttpStatusCode.NotFound, err);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, oEpicEnv);
            }

        }


        [HttpGet]
        [ActionName("LoadEpicUserCompany")]
        public HttpResponseMessage LoadEpicUserCompany(string strUserId, string strPass, string strEnvId)
        {
            string strReturnMsg;
            bool IsLoadEnvOK = false;
            bool IsLoadUserOK = false;
            bool IsLoadUserComp = false;
            bool IsLogin = false;

            IList<EpicCompany> lstEpicCompany = new List<EpicCompany>();

            EpicEnv oEpicEnv = new EpicEnv();
            EpicUser oEpicUser = new EpicUser();

            oEpicUser.Epic_UserId = strUserId;
            oEpicUser.Epic_PassKey = strPass;

            EpicEnvBO oEpicEnvBO = new EpicEnvBO();
            EpicUserBO oEpicUserBO = new EpicUserBO();

            IsLoadEnvOK = oEpicEnvBO._LoadEpicEnvById(strEnvId, ref oEpicEnv, out strReturnMsg);

            if (IsLoadEnvOK == true)
            {
                IsLogin = oEpicUserBO._VerifyEpicorLogin(ref oEpicEnv, ref oEpicUser, out strReturnMsg);

                if (IsLogin)
                {
                    IsLoadUserOK = oEpicUserBO._LoadEpicUserByUserId(strUserId, ref oEpicEnv, ref oEpicUser, out strReturnMsg);

                    if (IsLoadUserOK == true)
                    {
                        IsLoadUserComp = oEpicUserBO._LoadEpicCompanyByUserId(ref oEpicEnv, ref oEpicUser, ref lstEpicCompany, out strReturnMsg);

                        if (IsLoadUserComp == true)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, lstEpicCompany);
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
                        return Request.CreateResponse(HttpStatusCode.NotFound, err);
                    } // end if for isloaduserok

                } 
                else
                {
                    HttpError err = new HttpError(strReturnMsg);
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
                } // end if for islogin
            }
            else
            {
                HttpError err = new HttpError(strReturnMsg);
                return Request.CreateResponse(HttpStatusCode.NotFound, err);
            }



        }


        [HttpGet]
        [ActionName("LoadEpicCompanyAndPlantByUserId")]
        public HttpResponseMessage LoadEpicCompanyAndPlantByUserId(string strUserId, string strEnvId)
        {
            string strReturnMsg;
            bool IsLoadEnvOK = false;
            bool IsLoadUserOK = false;
            bool IsLoadUserComp = false;
            IList<EpicCompPlant> lstEpicCompPlant = new List<EpicCompPlant>();

            EpicEnv oEpicEnv = new EpicEnv();
            EpicUser oEpicUser = new EpicUser();


            EpicEnvBO oEpicEnvBO = new EpicEnvBO();
            EpicUserBO oEpicUserBO = new EpicUserBO();

            IsLoadEnvOK = oEpicEnvBO._LoadEpicEnvById(strEnvId, ref oEpicEnv, out strReturnMsg);

            if (IsLoadEnvOK == true)
            {
                IsLoadUserOK = oEpicUserBO._LoadEpicUserByUserId(strUserId, ref oEpicEnv, ref oEpicUser, out strReturnMsg);

                if (IsLoadUserOK == true)
                {
                    IsLoadUserComp = oEpicUserBO._LoadEpicCompanyAndPlantByUserId(ref oEpicEnv, ref oEpicUser, ref lstEpicCompPlant, out strReturnMsg);

                    if (IsLoadUserComp == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, lstEpicCompPlant);
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
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
                }
            }
            else
            {
                HttpError err = new HttpError(strReturnMsg);
                return Request.CreateResponse(HttpStatusCode.NotFound, err);
            }



        }



        [HttpPost]
        [ActionName("RemoveEpicEnvironment")]
        public HttpResponseMessage RemoveEpicEnvironment(string strEnvId)
        {
            string strReturnMsg;
            bool IsLoaded = false;
            bool IsComplete = false;

            EpicEnv oEpicEnv = new EpicEnv();
            EpicEnvBO oEpicEnvBO = new EpicEnvBO();

            IsLoaded = oEpicEnvBO._LoadEpicEnvById(strEnvId, ref oEpicEnv, out strReturnMsg);

            if (IsLoaded == true)
            {
                IsComplete = oEpicEnvBO._RemoveEpicEnv(ref oEpicEnv, out strReturnMsg);

                if (IsComplete == true)
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    HttpError err = new HttpError(strReturnMsg);
                    return Request.CreateResponse(HttpStatusCode.ServiceUnavailable, err);
                }

            }
            else
            {
                HttpError err = new HttpError(strReturnMsg);
                return Request.CreateResponse(HttpStatusCode.NotFound, err);
            }



        }


        [HttpPost]
        [ActionName("UpdateEpicEnvironment")]
        public HttpResponseMessage UpdateEpicEnvironment(string strEnvId, string strEnvName, bool blIsActive, 
                                                             string strEpicApp, string strAppEnv, string strAppUser, string strAppUPass,
                                                             string strSQLServer, string strSQLDB, string strSQLUser, string strSQLUPass)
        {
            string strReturnMsg;
            bool IsComplete = false;

            EpicEnv oEpicEnv = new EpicEnv();
            EpicEnvBO oEpicEnvBO = new EpicEnvBO();

            oEpicEnv.Env_ID = strEnvId;
            oEpicEnv.Env_Description = strEnvName;
            oEpicEnv.Env_IsActive = blIsActive;

            oEpicEnv.Env_AppServer = strEpicApp;
            oEpicEnv.Env_AppEpicor = strAppEnv;
            oEpicEnv.Env_AppUserId = strAppUser;
            oEpicEnv.Env_AppPassKey = strAppUPass;

            oEpicEnv.Env_SQLServer = strSQLServer;
            oEpicEnv.Env_SQLDB = strSQLDB;
            oEpicEnv.Env_SQLUserId = strSQLUser;
            oEpicEnv.Env_SQLPassKey = strSQLUPass;

            IsComplete = oEpicEnvBO._EditEpicEnv(ref oEpicEnv, out strReturnMsg);

            if (IsComplete == true)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                HttpError err = new HttpError(strReturnMsg);
                return Request.CreateResponse(HttpStatusCode.ServiceUnavailable, err);
            }


        }


        [HttpPost]
        [ActionName("CreateEpicEnvironment")]
        public HttpResponseMessage CreateEpicEnvironment(string strEnvId, string strEnvName, bool blIsActive,
                                                             string strEpicApp, string strAppEnv, string strAppUser, string strAppUPass,
                                                             string strSQLServer, string strSQLDB, string strSQLUser, string strSQLUPass)
        {
            string strReturnMsg;
            bool IsComplete = false;

            EpicEnv oEpicEnv = new EpicEnv();
            EpicEnvBO oEpicEnvBO = new EpicEnvBO();

            oEpicEnv.Env_ID = strEnvId;
            oEpicEnv.Env_Description = strEnvName;
            oEpicEnv.Env_IsActive = blIsActive;

            oEpicEnv.Env_AppServer = strEpicApp;
            oEpicEnv.Env_AppEpicor = strAppEnv;
            oEpicEnv.Env_AppUserId = strAppUser;
            oEpicEnv.Env_AppPassKey = strAppUPass;

            oEpicEnv.Env_SQLServer = strSQLServer;
            oEpicEnv.Env_SQLDB = strSQLDB;
            oEpicEnv.Env_SQLUserId = strSQLUser;
            oEpicEnv.Env_SQLPassKey = strSQLUPass;

            IsComplete = oEpicEnvBO._NewEpicEnv(ref oEpicEnv, out strReturnMsg);

            if (IsComplete == true)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                HttpError err = new HttpError(strReturnMsg);
                return Request.CreateResponse(HttpStatusCode.ServiceUnavailable, err);
            }


        }




    }
}
