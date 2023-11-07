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
    public class ProductionsController : ApiController
    {

        [HttpGet]
        public HttpResponseMessage LoadWorkQueue(string strUID, string strPass, string strCurCompany, string strCurPlant, string strEnvId, string strEmpId = "", string strResourceGrp = "", string strJobNum = "", int iAssemblySeq = 0, int iOperation = 0, string strResourceId = "")
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsLoadWorkQueueOK = false;

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
                    LaborBO oLaborBO = new LaborBO();
                    IList<WorkQueue> WorkQueueList = new List<WorkQueue>();

                    IsLoadWorkQueueOK = oLaborBO._LoadWorkQueue(ref oEpicorEnv, strCurCompany,  ref WorkQueueList, out strReturnMsg, strEmpId, strResourceGrp, strJobNum, iAssemblySeq, iOperation,strResourceId);


                    if (IsLoadWorkQueueOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, WorkQueueList);
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
        public HttpResponseMessage LoadActiveEmployee(string strUID, string strPass, string strCurCompany, string strCurPlant, string strEnvId)
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsLoadActiveEmployeeOK = false;

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
                    LaborBO oLaborBO = new LaborBO();
                    IList<ActiveEmployee> ActiveEmployeeList = new List<ActiveEmployee>();
                    
                    IsLoadActiveEmployeeOK = oLaborBO._LoadCurrentActiveEmployee(ref oEpicorEnv, strCurCompany,  ref ActiveEmployeeList, out strReturnMsg);

                    if (IsLoadActiveEmployeeOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ActiveEmployeeList);
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
        public HttpResponseMessage LoadActiveEmployeeById(string strUID, string strPass, string strCurCompany, string strCurPlant, string strEnvId, string strEmpId = "")
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsLoadActiveEmployeeOK = false;

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
                    LaborBO oLaborBO = new LaborBO();
                    ActiveEmployee oActiveEmployee = new ActiveEmployee();

                    IsLoadActiveEmployeeOK = oLaborBO._LoadCurrentActiveEmployeeByEmpId(ref oEpicorEnv, strCurCompany, strEmpId, ref oActiveEmployee, out strReturnMsg);

                    if (IsLoadActiveEmployeeOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, oActiveEmployee);
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
        public HttpResponseMessage LoadReasonCodes(string strUID, string strPass, string strCurCompany, string strReasonType, string strEnvId)
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsLoadReasonCodesOK = false;

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
                    LaborBO oLaborBO = new LaborBO();
                    IList<ReasonCodes> ReasonCodesList = new List<ReasonCodes>();

                    IsLoadReasonCodesOK = oLaborBO._LoadReasonCodes(ref oEpicorEnv, strCurCompany, strReasonType, ref ReasonCodesList, out strReturnMsg);

                    if (IsLoadReasonCodesOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ReasonCodesList);
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
        public HttpResponseMessage LoadJobOperationResource(string strUID, string strPass, string strCurCompany, string strJobNum, int iAssemblySeq, int iOperation, string strEnvId)
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsLoadResourcesOK = false;

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
                    LaborBO oLaborBO = new LaborBO();
                    IList<JobResources> ResourcesList = new List<JobResources>();

                    IsLoadResourcesOK = oLaborBO._LoadResourceByJobOperation(ref oEpicorEnv, strCurCompany,strJobNum,iAssemblySeq,iOperation, ref ResourcesList, out strReturnMsg);

                    if (IsLoadResourcesOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ResourcesList);
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
        public HttpResponseMessage LoadJobOperationResourceById(string strUID, string strPass, string strCurCompany, string strJobNum, int iAssemblySeq, int iOperation, string strResourceId, string strEnvId)
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsLoadResourcesOK = false;

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
                    LaborBO oLaborBO = new LaborBO();
                    JobResources oJobResource = new JobResources();

                    IsLoadResourcesOK = oLaborBO._LoadResourceByJobOperationById(ref oEpicorEnv, strCurCompany, strJobNum, iAssemblySeq, iOperation, strResourceId, ref oJobResource, out strReturnMsg);

                    if (IsLoadResourcesOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, oJobResource);
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
        public HttpResponseMessage LoadEmployees(string strUID, string strPass, string strCurCompany,  string strEnvId)
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsLoadEmployeesOK = false;

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
                    LaborBO oLaborBO = new LaborBO();
                    IList<Employee> EmployeeList = new List<Employee>();

                    IsLoadEmployeesOK = oLaborBO._LoadEmployee(ref oEpicorEnv, strCurCompany, ref EmployeeList, out strReturnMsg);

                    if (IsLoadEmployeesOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, EmployeeList);
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
        public HttpResponseMessage PerformEmployeeClockIn(string strUID, string strPass, string strEnvId, string strCurCompany, string strEmpId, int iShift)
        {
            string strReturnMsg;
            bool IsComplete = false;
            bool IsTrxComplete = false;

            EpicEnv oEpicorEnv = new EpicEnv();
            EpicEnvBO oEpicorEnvBO = new EpicEnvBO();
            IsComplete = oEpicorEnvBO._LoadEpicEnvById(strEnvId, ref oEpicorEnv, out strReturnMsg);

            ErrorBO oValidate = new ErrorBO();
            IList<Error> oErrors = new List<Error>();

            if (ConfigurationManager.AppSettings["EnableCheckEmployeeId"].Equals("true"))
            { oValidate._CheckEmployeeId(ref oEpicorEnv, strCurCompany, strEmpId, "[CLK01]", ref oErrors); }

            if (ConfigurationManager.AppSettings["EnableCheckShift"].Equals("true"))
            { oValidate._CheckShift(ref oEpicorEnv, strCurCompany, iShift, "[CLK02]", ref oErrors); }

            if (ConfigurationManager.AppSettings["EnableCheckClockIn"].Equals("true"))
            { oValidate._CheckClockIn(ref oEpicorEnv, strCurCompany, strEmpId, "[CLK03]", ref oErrors); }



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
                        
                EmpClock oEmpClock = new EmpClock();
                oEmpClock.Company = strCurCompany;
                oEmpClock.Employee = strEmpId;
                oEmpClock.Shift = iShift;
                

                EpicorBO oEpicor = new EpicorBO();

                IsTrxComplete = oEpicor._EpicEmployeeClockIn(ref oEpicorEnv, ref oEmpClock, out strReturnMsg, strUID, strPass);

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
        public HttpResponseMessage PerformEmployeeClockOut(string strUID, string strPass, string strEnvId, string strCurCompany, string strEmpId)
        {
            string strReturnMsg;
            bool IsComplete = false;
            bool IsTrxComplete = false;

            EpicEnv oEpicorEnv = new EpicEnv();
            EpicEnvBO oEpicorEnvBO = new EpicEnvBO();
            IsComplete = oEpicorEnvBO._LoadEpicEnvById(strEnvId, ref oEpicorEnv, out strReturnMsg);

            ErrorBO oValidate = new ErrorBO();
            IList<Error> oErrors = new List<Error>();

            if (ConfigurationManager.AppSettings["EnableCheckEmployeeId"].Equals("true"))
            { oValidate._CheckEmployeeId(ref oEpicorEnv, strCurCompany, strEmpId, "[EMP01]", ref oErrors); }

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

                EmpClock oEmpClock = new EmpClock();
                oEmpClock.Company = strCurCompany;
                oEmpClock.Employee = strEmpId;

                EpicorBO oEpicor = new EpicorBO();

                IsTrxComplete = oEpicor._EpicEmployeeClockOut(ref oEpicorEnv, ref oEmpClock, out strReturnMsg, strUID, strPass);

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
        public HttpResponseMessage PerformEmployeeStartActivity(string strUID, string strPass, string strEnvId, string strCurCompany, string strEmpId, string strJobNum, int iAssemblySeq, int iOperation, string strResourceId, int iLaborHedSeq, string strCurPlant)
        {
            string strReturnMsg;
            bool IsComplete = false;
            bool IsTrxComplete = false;

            EpicEnv oEpicorEnv = new EpicEnv();
            EpicEnvBO oEpicorEnvBO = new EpicEnvBO();
            IsComplete = oEpicorEnvBO._LoadEpicEnvById(strEnvId, ref oEpicorEnv, out strReturnMsg);

            ErrorBO oValidate = new ErrorBO();
            IList<Error> oErrors = new List<Error>();

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
                WorkQueue oWorkQueue = new WorkQueue();

                oWorkQueue.Company = strCurCompany;
                oWorkQueue.AssemblySeq = iAssemblySeq;
                oWorkQueue.EmployeeNum = strEmpId;
                oWorkQueue.JobNum = strJobNum;
                oWorkQueue.LaborHedSeq = iLaborHedSeq;
                oWorkQueue.OprSeq = iOperation;
                oWorkQueue.ResourceID = strResourceId;
                oWorkQueue.CurPlant = strCurPlant;

                EpicorBO oEpicor = new EpicorBO();
                IsTrxComplete = oEpicor._EpicEmployeeStartActivity(ref oEpicorEnv, ref oWorkQueue, out strReturnMsg, strUID, strPass);

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
        public HttpResponseMessage PerformEmployeeEndActivity(string strUID, string strPass, string strEnvId, string strCurCompany, int iLaborHedSeq, int iLaborDtlSeq, string strCurPlant, decimal dTranQty)
        {
            string strReturnMsg;
            bool IsComplete = false;
            bool IsTrxComplete = false;

            EpicEnv oEpicorEnv = new EpicEnv();
            EpicEnvBO oEpicorEnvBO = new EpicEnvBO();
            IsComplete = oEpicorEnvBO._LoadEpicEnvById(strEnvId, ref oEpicorEnv, out strReturnMsg);

            ErrorBO oValidate = new ErrorBO();
            IList<Error> oErrors = new List<Error>();

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
                WorkQueue oWorkQueue = new WorkQueue();

                oWorkQueue.Company = strCurCompany;
                oWorkQueue.LaborHedSeq = iLaborHedSeq;
                oWorkQueue.LaborDtlSeq = iLaborDtlSeq;
                oWorkQueue.CurPlant = strCurPlant;
                oWorkQueue.TranQty = dTranQty;

                EpicorBO oEpicor = new EpicorBO();
                IsTrxComplete = oEpicor._EpicEmployeeEndActivity(ref oEpicorEnv, ref oWorkQueue, out strReturnMsg, strUID, strPass);


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
        public HttpResponseMessage PerformEndActivitiesByJobOperation(string strUID, string strPass, string strEnvId, string strCurCompany, string strEmpId, string strJobNum, int iAssemblySeq, int iOperation, string strResourceId, string strCurPlant, decimal dTranQty)
        {
            string strReturnMsg;
            bool IsComplete = false;
            //bool IsTrxComplete = false;
            bool IsLoadWorkQueueOK = false;
            bool IsLoadSupervisorWorkQueueOK = false;

            EpicEnv oEpicorEnv = new EpicEnv();
            EpicEnvBO oEpicorEnvBO = new EpicEnvBO();
            IsComplete = oEpicorEnvBO._LoadEpicEnvById(strEnvId, ref oEpicorEnv, out strReturnMsg);

            ErrorBO oValidate = new ErrorBO();
            IList<Error> oErrors = new List<Error>();

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
                LaborBO oLaborBO = new LaborBO();
                EpicorBO oEpicor = new EpicorBO();

                IList<WorkQueue> SupervisorWorkQueueList = new List<WorkQueue>();
                IList<WorkQueue> WorkQueueList = new List<WorkQueue>();

                IsLoadSupervisorWorkQueueOK = oLaborBO._LoadWorkQueue(ref oEpicorEnv, strCurCompany, ref SupervisorWorkQueueList, out strReturnMsg, strEmpId, "", strJobNum, iAssemblySeq, iOperation, ""); //koo - remove resource criteria during load work queue
                IsLoadWorkQueueOK = oLaborBO._LoadWorkQueueExceptSup(ref oEpicorEnv, strCurCompany, ref WorkQueueList, out strReturnMsg, strEmpId, "", strJobNum, iAssemblySeq, iOperation, ""); //koo - remove resource criteria during load work queue - use work queue without supervisor

                if (IsLoadWorkQueueOK || IsLoadSupervisorWorkQueueOK)
                {
                    // report quantity
                    /*foreach (WorkQueue oSupervisorWorkQueue in SupervisorWorkQueueList)
                    {
                        WorkQueue superq = oSupervisorWorkQueue;

                        superq.TranQty = dTranQty;

                        oEpicor._EpicReportQty(ref oEpicorEnv, ref superq, out strReturnMsg, strUID, strPass);
                    }*/
                    if (IsLoadSupervisorWorkQueueOK)
                    { 
                        foreach (WorkQueue oSupervisorWorkQueue in SupervisorWorkQueueList)
                        {
                            WorkQueue superq = oSupervisorWorkQueue;

                            superq.TranQty = dTranQty;

                            oEpicor._EpicEmployeeEndActivity(ref oEpicorEnv, ref superq, out strReturnMsg, strUID, strPass);
                        }
                    }
                    if (IsLoadWorkQueueOK)
                        {
                        // end activity for all other employee  for a job operation
                        foreach ( WorkQueue oWorkQueue in WorkQueueList)
                        {
                            WorkQueue wq = oWorkQueue;

                            wq.TranQty = dTranQty;

                            oEpicor._EpicEmployeeEndActivity(ref oEpicorEnv, ref wq, out strReturnMsg, strUID, strPass);

                        }
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
                return Request.CreateResponse(HttpStatusCode.NotFound, err);
            } // end for iscomplete


        }




    }


}