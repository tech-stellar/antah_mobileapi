using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using EpicWAS.Models;

namespace EpicWAS.Controllers
{
    public class JobController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage LoadJobHeadById(string strUID, string strPass, string strCurCompany, string strCurPlant, string strEnvId, string strJobNum)
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsLoadJobOK = false;

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
                    JobBO oJobBO = new JobBO();
                    JobHead JobHead = new JobHead();

                    IsLoadJobOK = oJobBO._LoadJobHeadById(ref oEpicorEnv, strCurCompany, strCurPlant, strJobNum, ref JobHead, out strReturnMsg);

                    if (IsLoadJobOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, JobHead);
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
                } // end if for Islogin

            }
            else
            {
                HttpError err = new HttpError(strReturnMsg);
                return Request.CreateResponse(HttpStatusCode.NotFound, err);
            } // end if for iscomplete

        }

        [HttpGet]
        public HttpResponseMessage LoadJobAssemblyByAssmbSeq(string strUID, string strPass, string strCurCompany, string strCurPlant, string strEnvId, string strJobNum, int iAssemblySeq)
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsLoadJobOK = false;

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
                    JobBO oJobBO = new JobBO();
                    JobAsmbl JobAssembly= new JobAsmbl();

                    IsLoadJobOK = oJobBO._LoadJobAsmblBySeqId(ref oEpicorEnv, strCurCompany, strCurPlant, strJobNum,iAssemblySeq, ref JobAssembly, out strReturnMsg);

                    if (IsLoadJobOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, JobAssembly);
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
                } // end if for Islogin

            }
            else
            {
                HttpError err = new HttpError(strReturnMsg);
                return Request.CreateResponse(HttpStatusCode.NotFound, err);
            } // end if for iscomplete

        }

        [HttpGet]
        public HttpResponseMessage LoadJobMaterialByMtlSeq(string strUID, string strPass, string strCurCompany, string strCurPlant, string strEnvId, string strJobNum, int iAssemblySeq, int iMtlSeq)
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsLoadJobOK = false;

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
                    JobBO oJobBO = new JobBO();
                    JobMtl JobMaterial = new JobMtl();

                    IsLoadJobOK = oJobBO._LoadJobMtlByMtlSeq(ref oEpicorEnv, strCurCompany, strCurPlant, strJobNum, iAssemblySeq,iMtlSeq, ref JobMaterial, out strReturnMsg);

                    if (IsLoadJobOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, JobMaterial);
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
                } // end if for Islogin

            }
            else
            {
                HttpError err = new HttpError(strReturnMsg);
                return Request.CreateResponse(HttpStatusCode.NotFound, err);
            } // end if for iscomplete


        }



    }
}
