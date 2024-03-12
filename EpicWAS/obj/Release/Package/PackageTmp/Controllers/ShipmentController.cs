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
    public class ShipmentController : ApiController
    {
		[HttpGet]
		public HttpResponseMessage LoadSummary(string strUID, string strPass, string strCurCompany, string strOrderNum, string strPartNum, string strPickListNum, string strWarehouse, string strStatus, bool backOrder, 
            string strFromPickDate, string strToPickDate, string strCurPlant, string strEnvId, bool showVoid = false)
		{
			string strReturnMsg;
			bool IsLogin = false;
			bool IsComplete = false;
			bool IsLoadSummaryOk = false;

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
					PickPackBO oPickPackBO = new PickPackBO();
					IList<Summary> SummaryL = new List<Summary>();

                    strOrderNum = strOrderNum == null ? "" : strOrderNum;
                    strPartNum = strPartNum == null ? "" : strPartNum;
                    strPickListNum = strPickListNum == null ? "" : strPickListNum;
                    strWarehouse = strWarehouse == null ? "" : strWarehouse;
                    strStatus = strStatus == null ? "" : strStatus;

					IsLoadSummaryOk = oPickPackBO._LoadSummary(ref oEpicorEnv, strCurCompany, strUID, strOrderNum, strPartNum, strPickListNum, 
                        strWarehouse, strStatus, backOrder, strFromPickDate, strToPickDate, showVoid, ref SummaryL, out strReturnMsg);



					if (IsLoadSummaryOk)
					{
						return Request.CreateResponse(HttpStatusCode.OK, SummaryL);
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
        public HttpResponseMessage LoadPickPackSlip(string strUID, string strPass, string strCurCompany, string strCurPlant, string strEnvId,  string strCustID, string strCustName, string strPickNum, string strTagNum, string ud = "")
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsLoadPickPackOK = false;

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
                    PickPackBO oPickPackBO = new PickPackBO();
                    IList<PickPack2> PickPackL = new List<PickPack2>();
                    
                    if (ud == "UD103")
                    {
						IsLoadPickPackOK = oPickPackBO._LoadPickPacksUD103(ref oEpicorEnv, strCurCompany, strCurPlant, strCustID, strCustName, ref PickPackL, strPickNum, strTagNum, strUID, out strReturnMsg);
					}
                    else
                    {
						IsLoadPickPackOK = oPickPackBO._LoadPickPacksV2(ref oEpicorEnv, strCurCompany, strCustID, strCustName, ref PickPackL, strPickNum, strTagNum, strUID, out strReturnMsg);
					}
                    


                    if (IsLoadPickPackOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, PickPackL);
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
        public HttpResponseMessage RetrievePickPack(string strUID, string strPass, string strCurCompany, string strCurPlant, string strEnvId, string strPicker, string ud = "")
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsLoadPickPackOK = false;

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
                    PickPackBO oPickPackBO = new PickPackBO();
                    IList<PickPack> PickPackL = new List<PickPack>();

                    if (ud == "UD103")
                    {
						IsLoadPickPackOK = oPickPackBO._AssignPickPacksUD103(ref oEpicorEnv, strCurCompany, strCurPlant, strPicker, ref PickPackL, out strReturnMsg);
					}
                    else
                    {
						IsLoadPickPackOK = oPickPackBO._AssignPickPacks(ref oEpicorEnv, strCurCompany, strPicker, ref PickPackL, out strReturnMsg);
					}
                    

                    if (IsLoadPickPackOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, PickPackL);
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
		public HttpResponseMessage RetrieveBackPickPack(string strUID, string strPass, string strCurCompany, string strCurPlant, string strEnvId, string strPicker, string strPartNum)
		{
			string strReturnMsg;
			bool IsLogin = false;
			bool IsComplete = false;
			bool IsLoadPickPackOK = false;

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
					PickPackBO oPickPackBO = new PickPackBO();
					IList<PickPack> PickPackL = new List<PickPack>();

					IsLoadPickPackOK = oPickPackBO._AssignBackPickPacks(ref oEpicorEnv, strCurCompany, strCurPlant, strPicker, strPartNum, ref PickPackL, out strReturnMsg);

					if (IsLoadPickPackOK)
					{
						return Request.CreateResponse(HttpStatusCode.OK, PickPackL);
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
        public HttpResponseMessage LoadUserDefineReasonCodes(string strUID, string strPass, string strCurCompany, string strCodeTypeId, string strEnvId)
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
                    PickPackBO oPickPackBO = new PickPackBO();
                    IList<ReasonCodes> ReasonCodesList = new List<ReasonCodes>();

                    IsLoadReasonCodesOK = oPickPackBO._LoadUDReasonCodes(ref oEpicorEnv, strCurCompany, strCodeTypeId, ref ReasonCodesList, out strReturnMsg);

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
        public HttpResponseMessage LoadEscalateList(string strUID, string strPass, string strCurCompany, string strCurPlant, string strEnvId, string strPickNum, string strCust, string ud = "")
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsLoadPickPackOK = false;

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
                    PickPackBO oPickPackBO = new PickPackBO();
                    IList<PickPackEscalate> EscalateL = new List<PickPackEscalate>();

                    if (ud == "UD103")
                    {
						IsLoadPickPackOK = oPickPackBO._LoadEscalateListUD103(ref oEpicorEnv, strCurCompany, strCust, ref EscalateL, strPickNum, out strReturnMsg);
					}
                    else
                    {
						IsLoadPickPackOK = oPickPackBO._LoadEscalateList(ref oEpicorEnv, strCurCompany, strCust, ref EscalateL, strPickNum, out strReturnMsg);
					}


                    if (IsLoadPickPackOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, EscalateL);
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
        public HttpResponseMessage LoadInvoices(string strUID, string strPass, string strEnvId, string strCurCompany, string strCurPlant, string strInvoice)
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsLoadInvoiceOK = false;

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
                    PickPackBO oPickPackBO = new PickPackBO();
                    IList<Invoices> InvoicesL = new List<Invoices>();

                    IsLoadInvoiceOK = oPickPackBO._LoadInvoices(  ref oEpicorEnv, strCurCompany, strInvoice, ref InvoicesL, out strReturnMsg);

                    if (IsLoadInvoiceOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, InvoicesL);
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
        public HttpResponseMessage VerifyTag(string strUID, string strPass, string strEnvId, string strCurCompany, string strCurPlant, string strPackListNum, string strTag, string ud = "")
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsTagOK = false;

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
                    PickPackBO oPickPackBO = new PickPackBO();
                    
                    if (ud == "UD103")
                    {
						IsTagOK = oPickPackBO._Verify_TagUD103(ref oEpicorEnv, strCurCompany, strPackListNum, strTag, out strReturnMsg);
					}
                    else
                    {
						IsTagOK = oPickPackBO._Verify_Tag(ref oEpicorEnv, strCurCompany, strPackListNum, strTag, out strReturnMsg);
					}

                    if (IsTagOK)
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
                } // end if for Islogin

            }
            else
            {
                HttpError err = new HttpError(strReturnMsg);
                return Request.CreateResponse(HttpStatusCode.NotFound, err);
            } // end if for iscomplete

        }


        [HttpGet]
        public HttpResponseMessage LoadShipVia(string strUID, string strPass, string strCurCompany, string strShipViaCode, string strEnvId)
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsLoadShipViaOK = false;

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
                    PickPackBO oPickPackBO = new PickPackBO();
                    IList<ShipVia> ShipViaList = new List<ShipVia>();

                    IsLoadShipViaOK = oPickPackBO._LoadShipVia(ref oEpicorEnv, strCurCompany, strShipViaCode, ref ShipViaList, out strReturnMsg);

                    if (IsLoadShipViaOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ShipViaList);
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
        public HttpResponseMessage LoadNewShipVia(string strUID, string strPass, string strCurCompany, string strShipViaCode, string strEnvId)
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsLoadShipViaOK = false;

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
                    PickPackBO oPickPackBO = new PickPackBO();
                    IList<NewShipVia> ShipViaList = new List<NewShipVia>();

                    IsLoadShipViaOK = oPickPackBO._LoadNewShipVia(ref oEpicorEnv, strCurCompany, strShipViaCode, ref ShipViaList, out strReturnMsg);

                    if (IsLoadShipViaOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ShipViaList);
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
        public HttpResponseMessage LoadState(string strUID, string strPass, string strCurCompany, string strPicklistNum, string strEnvId)
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsLoadStateOK = false;
            bool IsControlWeight = false;

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
                    PickPackBO oPickPackBO = new PickPackBO();
                    IList<NewShipVia> ShipViaList = new List<NewShipVia>();

                    IsLoadStateOK = oPickPackBO._LoadState(ref oEpicorEnv, strCurCompany, strPicklistNum, out IsControlWeight);

                    if (IsLoadStateOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, IsControlWeight.ToString());
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
        public HttpResponseMessage LoadInvoiceForReprint(string strUID, string strPass, string strEnvId, string strCurCompany, string strCurPlant, string strInvoice, string strLegalNumber, string ud = "")
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsLoadInvoiceOK = false;

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
                    PickPackBO oPickPackBO = new PickPackBO();
                    IList<ReprintInvoice> InvoicesL = new List<ReprintInvoice>();

                    if (ud == "UD103")
                    {
						IsLoadInvoiceOK = oPickPackBO._LoadInvoicesForReprintUD103(ref oEpicorEnv, strCurCompany, strInvoice, strLegalNumber, ref InvoicesL, out strReturnMsg);
					}
                    else
                    {
						IsLoadInvoiceOK = oPickPackBO._LoadInvoicesForReprint(ref oEpicorEnv, strCurCompany, strInvoice, strLegalNumber, ref InvoicesL, out strReturnMsg);
					}

                    if (IsLoadInvoiceOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, InvoicesL);
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
        public HttpResponseMessage LoadPalletNumbers(string strUID, string strPass, string strCurCompany, string strOwnership, string strEnvId)
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
                    PickPackBO oPickPackBO = new PickPackBO();
                    IList<Pallets> PalletsList = new List<Pallets>();

                    IsLoadReasonCodesOK = oPickPackBO._LoadPalletNum(ref oEpicorEnv, strCurCompany, strOwnership, ref PalletsList, out strReturnMsg);

                    if (IsLoadReasonCodesOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, PalletsList);
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
        public HttpResponseMessage LoadOrderFromQueue(string strUID, string strPass, string strCurCompany, string strEnvId, string strCurPlant)
        {
            string strReturnMsg;
            bool IsLogin = false;
            bool IsComplete = false;
            bool IsLoadOrderOK = false;

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
                oEpicUserBO._LoadEpicUserByUserId(strUID, ref oEpicorEnv, ref oEpicUser, out strReturnMsg);

                if (IsLogin)
                {
                    PickPackBO oPickPackBO = new PickPackBO();
                    OrderHed oOrderHed = new OrderHed();

                    IsLoadOrderOK = oPickPackBO._LoadOrderFromQueue(ref oEpicorEnv, strCurCompany, strCurPlant, oEpicUser, oOrderHed, out strReturnMsg);

                    if (IsLoadOrderOK)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, oOrderHed);
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
        public HttpResponseMessage SalesOrderAllocation(string strUID, string strPass, string strEnvId, string strCurCompany, string strCurPlant, string strOrderNum)
        {
            string strReturnMsg;
            bool IsComplete = false;
            bool IsTrxComplete = false;

            EpicEnv oEpicorEnv = new EpicEnv();
            EpicEnvBO oEpicorEnvBO = new EpicEnvBO();

            IsComplete = oEpicorEnvBO._LoadEpicEnvById(strEnvId, ref oEpicorEnv, out strReturnMsg);

            if (IsComplete)
            {
                PickPackBO oPickPack = new PickPackBO();
                OrderHed oOrderHed = new OrderHed();

                oOrderHed.Company = strCurCompany;
                oOrderHed.OrderNum = int.Parse(strOrderNum);

                // start doing the allocation
                IsTrxComplete = oPickPack._SalesOrderAllocation(ref oEpicorEnv, strCurCompany, strCurPlant, strUID, strPass, ref oOrderHed, out strReturnMsg);

                if (IsTrxComplete)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, oOrderHed);
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

        [HttpGet]
        public HttpResponseMessage LoadCustomerShipment(string strUID, string strPass, string strEnvId, string strCurCompany, string strCurPlant, string strPickListNum, string strPackNum = "")
        {
            string strReturnMsg;
            bool IsComplete = false;
            bool IsTrxComplete = false;

            EpicEnv oEpicorEnv = new EpicEnv();
            EpicEnvBO oEpicorEnvBO = new EpicEnvBO();

            IsComplete = oEpicorEnvBO._LoadEpicEnvById(strEnvId, ref oEpicorEnv, out strReturnMsg);

            if (IsComplete)
            {
                PickPackBO oPickPack = new PickPackBO();
                IList<ShipDetail> ShipmentsList = new List<ShipDetail>();

                // start doing the allocation
                IsTrxComplete = oPickPack._LoadCustomerShipment(ref oEpicorEnv, strCurCompany, strCurPlant, strPickListNum,strPackNum, ref ShipmentsList, strUID, strPass,  out strReturnMsg);

                if (IsTrxComplete)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, ShipmentsList);
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

        [HttpGet]
        public HttpResponseMessage LoadActiveShipment(string strUID, string strPass, string strEnvId, string strCurCompany, string strCurPlant, string strPicker = "", string strPacker = "")
        {
            string strReturnMsg;
            bool IsComplete = false;
            bool IsTrxComplete = false;

            EpicEnv oEpicorEnv = new EpicEnv();
            EpicEnvBO oEpicorEnvBO = new EpicEnvBO();

            IsComplete = oEpicorEnvBO._LoadEpicEnvById(strEnvId, ref oEpicorEnv, out strReturnMsg);

            if (IsComplete)
            {
                PickPackBO oPickPack = new PickPackBO();
                ShipHeader oShiphead = new ShipHeader();

                // start doing the allocation
                IsTrxComplete = oPickPack._LoadActiveShipment(ref oEpicorEnv, strCurCompany, strCurPlant,strPicker,strPacker, ref oShiphead, strUID, strPass, out strReturnMsg);

                if (IsTrxComplete)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, oShiphead);
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

        [HttpGet]
        public HttpResponseMessage LoadPackPartQty(string strEnvId, string pickNum, string partNum, string lotNum)
        {
			string strReturnMsg;
			bool IsComplete = false;
			double packPartQty = 0;

			EpicEnv oEpicorEnv = new EpicEnv();
			EpicEnvBO oEpicorEnvBO = new EpicEnvBO();

			IsComplete = oEpicorEnvBO._LoadEpicEnvById(strEnvId, ref oEpicorEnv, out strReturnMsg);

			if (IsComplete)
			{
				PickPackBO oPickPack = new PickPackBO();
				packPartQty = oPickPack._checkPickPartQty(ref oEpicorEnv, pickNum, partNum, lotNum);

				return Request.CreateResponse(HttpStatusCode.OK, packPartQty);
			}
			else
			{
				HttpError err = new HttpError(strReturnMsg);
				return Request.CreateResponse(HttpStatusCode.NotFound, err);
			} // end for iscomplete
		}

        [HttpGet]
        public HttpResponseMessage RetrievePickPackStatus(string strUID, string strPass, string strEnvId, string strCurCompany, string strCurPlant, string strPackListNum)
        {
            string strReturnMsg;
            bool IsComplete = false;
            bool IsTrxComplete = false;

            EpicEnv oEpicorEnv = new EpicEnv();
            EpicEnvBO oEpicorEnvBO = new EpicEnvBO();

            IsComplete = oEpicorEnvBO._LoadEpicEnvById(strEnvId, ref oEpicorEnv, out strReturnMsg);

            if (IsComplete)
            {
                PickPack3 PickPack3 = new PickPack3();
                PickPackBO oPickPack = new PickPackBO();

                IsTrxComplete = oPickPack._RetrievePickPackStatus(ref oEpicorEnv, strCurCompany, strCurPlant, strPackListNum,ref PickPack3, out strReturnMsg);

                if (IsTrxComplete)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, PickPack3);
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
        public HttpResponseMessage LoadPickPackRevert(string strUID, string strPass, string strEnvId, string strCurCompany, string strCurPlant, string strMQSysRowID, string strU14SysRowID, string strRemark, string strPicker, string strReason, DateTime dtEscalateDt, string strCurrentStage = "", string pickListNo = "", string ud = "" )
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

                PickPackEscalate oPickPackES = new PickPackEscalate();
                oPickPackES.Company = strCurCompany;
                oPickPackES.MQ_SysRowID = strMQSysRowID;
                oPickPackES.U14_SysRowID = strU14SysRowID;
                oPickPackES.PickPackRemarks= strRemark;
                oPickPackES.CurrentPlant = strCurPlant;
                oPickPackES.Reason = strReason;
                oPickPackES.EscalateDateTime = dtEscalateDt.ToString();
                oPickPackES.Picker = strPicker;
                oPickPackES.CurrentStage = strCurrentStage;
                oPickPackES.PickListNo = pickListNo;
               
                EpicorBO oEpicor = new EpicorBO();

                if (ud != "UD103")
                {
					IsTrxComplete = oEpicor._InsertInToUD18(ref oEpicorEnv, ref oPickPackES, out strReturnMsg, strUID, strPass);
				}
                else
                {
					IsTrxComplete = oEpicor._InsertInToUD18New(ref oEpicorEnv, ref oPickPackES, out strReturnMsg, strUID, strPass);
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
            } // end for iscomplete

        }

        [HttpPost]
        public HttpResponseMessage SavePickPack(string strUID, string strPass, string strEnvId, string strCurCompany, string strCurPlant, string strU14SysRowID, string strLotNum, string strWarehouse, string strBinNum, decimal dQuantity, string strTag, string strPallet="", string ud = "", string pickNum = "", string pickLine = "")
        {
            string strReturnMsg;
            bool IsComplete = false;
            bool IsTrxComplete = false;

            EpicEnv oEpicorEnv = new EpicEnv();
            EpicEnvBO oEpicorEnvBO = new EpicEnvBO();
            IsComplete = oEpicorEnvBO._LoadEpicEnvById(strEnvId, ref oEpicorEnv, out strReturnMsg);


            if (IsComplete)
            {
                PickPackBO oPickPack = new PickPackBO();
                
                if (ud == "UD103")
                {
					IsTrxComplete = oPickPack._SavePickPackUD103(ref oEpicorEnv, strCurCompany, strCurPlant, strUID, strPass, strLotNum, strWarehouse, strBinNum, dQuantity, strTag, strPallet, pickNum, pickLine, out strReturnMsg);
				}
                else
                {
					IsTrxComplete = oPickPack._SavePickPack(ref oEpicorEnv, strCurCompany, strCurPlant, strUID, strPass, strLotNum, strWarehouse, strBinNum, strU14SysRowID, dQuantity, strTag, strPallet, out strReturnMsg);
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
            } // end for iscomplete

        }

        [HttpPost]
        public HttpResponseMessage SavePacking(string strUID, string strPass, string strEnvId, string strCurCompany, string strCurPlant, string strRemark, string strTransporter, string strConsignment, string strPallet, decimal dTotalWeight, decimal dTotalBox, string strPickListNum, string strPacker, string strStation = "", string strPackPallet ="", string ud = "")
        {
            string strReturnMsg;
            bool IsComplete = false;
            bool IsTrxComplete = false;

            EpicEnv oEpicorEnv = new EpicEnv();
            EpicEnvBO oEpicorEnvBO = new EpicEnvBO();
            IsComplete = oEpicorEnvBO._LoadEpicEnvById(strEnvId, ref oEpicorEnv, out strReturnMsg);


            if (IsComplete)
            {
                PickPackBO oPickPack = new PickPackBO();

                if (ud == "UD103")
                {
                    IsTrxComplete = oPickPack._SavePackListUD103(ref oEpicorEnv, strCurCompany, strRemark, strTransporter, strConsignment, strPallet, dTotalWeight, dTotalBox, strPickListNum, strPacker, strStation, strUID, strPass, strCurPlant, strPackPallet, out strReturnMsg);
                }
                else
                {
					IsTrxComplete = oPickPack._SavePackList(ref oEpicorEnv, strCurCompany, strRemark, strTransporter, strConsignment, strPallet, dTotalWeight, dTotalBox, strPickListNum, strPacker, strStation, strUID, strPass, strCurPlant, strPackPallet, out strReturnMsg);
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
            } // end for iscomplete

        }

        [HttpPost]
        public HttpResponseMessage ResolvedEscalate(string strUID, string strPass, string strEnvId, string strCurCompany, string strCurPlant, string strPickListNum, string strReason = "", string strRemark = "", string ud = "")
        {
            string strReturnMsg;
            bool IsComplete = false;
            bool IsTrxComplete = false;

            EpicEnv oEpicorEnv = new EpicEnv();
            EpicEnvBO oEpicorEnvBO = new EpicEnvBO();
            IsComplete = oEpicorEnvBO._LoadEpicEnvById(strEnvId, ref oEpicorEnv, out strReturnMsg);


            if (IsComplete)
            {
                PickPackBO oPickPack = new PickPackBO();

                if (ud == "UD103")
                {
                    IsTrxComplete = oPickPack._ResolvedEscalateUD103(ref oEpicorEnv, strCurCompany, strPickListNum, strReason, strRemark, out strReturnMsg);
				}
                else
                {
					IsTrxComplete = oPickPack._ResolvedEscalate(ref oEpicorEnv, strCurCompany, strPickListNum, strReason, strRemark, out strReturnMsg);
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
            } // end for iscomplete

        }

        [HttpPost]
        public HttpResponseMessage ScanSignedDO(string strUID, string strPass, string strEnvId, string strCurCompany, string strCurPlant, string strInvoice)
        {
            string strReturnMsg;
            bool IsComplete = false;
            bool IsTrxComplete = false;

            EpicEnv oEpicorEnv = new EpicEnv();
            EpicEnvBO oEpicorEnvBO = new EpicEnvBO();
            IsComplete = oEpicorEnvBO._LoadEpicEnvById(strEnvId, ref oEpicorEnv, out strReturnMsg);


            if (IsComplete)
            {
                PickPackBO oPickPack = new PickPackBO();

                IsTrxComplete = oPickPack._ScanSignedDO(ref oEpicorEnv, strCurCompany, strCurPlant, strUID, strInvoice, out strReturnMsg);

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
        public HttpResponseMessage ConfirmDelivery(string strUID, string strPass, string strEnvId, string strCurCompany, string strCurPlant, string strInvoice)
        {
            string strReturnMsg;
            bool IsComplete = false;
            bool IsTrxComplete = false;

            EpicEnv oEpicorEnv = new EpicEnv();
            EpicEnvBO oEpicorEnvBO = new EpicEnvBO();
            IsComplete = oEpicorEnvBO._LoadEpicEnvById(strEnvId, ref oEpicorEnv, out strReturnMsg);


            if (IsComplete)
            {
                PickPackBO oPickPack = new PickPackBO();

                IsTrxComplete = oPickPack._ConfirmDelivery(ref oEpicorEnv, strCurCompany, strCurPlant, strUID, strInvoice, out strReturnMsg);

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
        public HttpResponseMessage AssignEscalatePIC(string strUID, string strPass, string strEnvId, string strCurCompany, string strCurPlant, string strKey1)
        {
            string strReturnMsg;
            bool IsComplete = false;
            bool IsTrxComplete = false;

            EpicEnv oEpicorEnv = new EpicEnv();
            EpicEnvBO oEpicorEnvBO = new EpicEnvBO();
            IsComplete = oEpicorEnvBO._LoadEpicEnvById(strEnvId, ref oEpicorEnv, out strReturnMsg);


            if (IsComplete)
            {
                PickPackBO oPickPack = new PickPackBO();

                IsTrxComplete = oPickPack._AssignEscalatePIC(ref oEpicorEnv, strCurCompany, strCurPlant, strUID, strKey1, out strReturnMsg);

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
        public HttpResponseMessage SaveReAssignDO(string strUID, string strPass, string strEnvId, string strCurCompany, string strCurPlant, string strInvoice, decimal dWeight, decimal dCarton, string strTransporter, string strPackNum,string strRemark)
        {
            string strReturnMsg;
            bool IsComplete = false;
            bool IsTrxComplete = false;

            EpicEnv oEpicorEnv = new EpicEnv();
            EpicEnvBO oEpicorEnvBO = new EpicEnvBO();
            IsComplete = oEpicorEnvBO._LoadEpicEnvById(strEnvId, ref oEpicorEnv, out strReturnMsg);


            if (IsComplete)
            {
                PickPackBO oPickPack = new PickPackBO();

                IsTrxComplete = oPickPack._ReAssignDO(ref oEpicorEnv, strCurCompany, strCurPlant, strUID, strInvoice, dWeight, dCarton, strTransporter, strPackNum, strRemark, out strReturnMsg);

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
        public HttpResponseMessage ReprintInvoice(string strUID, string strPass, string strEnvId, string strCurCompany, string strCurPlant, string strInvoice, string strLegalNumber, string strPickListNum, string strReason, string strRemark="")
        {
            string strReturnMsg;
            bool IsComplete = false;
            bool IsTrxComplete = false;

            EpicEnv oEpicorEnv = new EpicEnv();
            EpicEnvBO oEpicorEnvBO = new EpicEnvBO();
            IsComplete = oEpicorEnvBO._LoadEpicEnvById(strEnvId, ref oEpicorEnv, out strReturnMsg);


            if (IsComplete)
            {
                EpicorREST oEpicor = new EpicorREST();

                UD17 oUD17 = new UD17();

                oUD17.Company = strCurCompany;
                oUD17.Key1 = DateTime.Now.ToString("yyyyMMdd-HHmmss");
                oUD17.Key2 = "";
                oUD17.Key3 = "";
                oUD17.Key4 = "";
                oUD17.Key5 = "1";
                oUD17.ShortChar01 = strPickListNum;
                oUD17.ShortChar02 = strLegalNumber;
                oUD17.ShortChar03 = strInvoice;
                oUD17.ShortChar04 = strUID;
                oUD17.ShortChar05 = "";
                oUD17.CheckBox01 = true;

                IsTrxComplete = oEpicor._ReprintInvoice(ref oEpicorEnv, ref oUD17, strCurCompany, strCurPlant, out strReturnMsg, strUID, strPass);

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
        public HttpResponseMessage ReprintLabelOnly(string strUID, string strPass, string strEnvId, string strCurCompany, string strCurPlant, string strPickList, int iQuantity)
        {
            string strReturnMsg;
            bool IsComplete = false;
            bool IsTrxComplete = false;

            EpicEnv oEpicorEnv = new EpicEnv();
            EpicEnvBO oEpicorEnvBO = new EpicEnvBO();
            IsComplete = oEpicorEnvBO._LoadEpicEnvById(strEnvId, ref oEpicorEnv, out strReturnMsg);


            if (IsComplete)
            {
                PickPackBO oPickPack = new PickPackBO();

                IsTrxComplete = oPickPack._ReprintLabelOnly(ref oEpicorEnv, strCurCompany, strCurPlant, strUID, strPickList, iQuantity, out strReturnMsg);

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
        public HttpResponseMessage UpdateShipmentDtl(string strUID, string strPass, string strEnvId, string strCurCompany,  string strCurPlant, string strPickListNum, string strPackNum="", string strPackLine="", string strPicker="", string strPacker="", string strTagNum="", string strPallet="")
        {
            string strReturnMsg;
            bool IsComplete = false;
            bool IsTrxComplete = false;

            EpicEnv oEpicorEnv = new EpicEnv();
            EpicEnvBO oEpicorEnvBO = new EpicEnvBO();
            IsComplete = oEpicorEnvBO._LoadEpicEnvById(strEnvId, ref oEpicorEnv, out strReturnMsg);

            if (IsComplete)
            {
                PickPackBO oPickPack = new PickPackBO();

                IsTrxComplete = oPickPack._UpdatePickPackShipment(ref oEpicorEnv, strCurCompany, strCurPlant, strPickListNum, strPackNum, strPackLine, strPicker, strPacker, strTagNum, strPallet, strUID, strPass, out strReturnMsg);

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

    }
}
