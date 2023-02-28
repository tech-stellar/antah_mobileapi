using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using EpicWAS.Models;

namespace EpicWAS.Controllers
{
    public class CRMController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage PerformNewCustomer(string strUID, string strPass, string strEnvId, string strCurCompany, string strCustId, string strCustName
                                                    , string strAddress1, string strAddress2, string strAddress3, string strCity, string strState, string strZip
                                                    , int iCountryNum, string strPhoneNum, string strFaxNum, string strEMailAddress, string strCustURL
                                                    , string strTerritoryID, string strSalesRepCode, string strTermsCode, string strGroupCode, string strBusinessCatList
                                                    , bool FS_APCLicense_c, bool FS_MMAMembership_c, bool FS_GVOTForm24_c, bool FS_GVOTForm49_c, bool FS_BussReg_c
                                                    , bool FS_TradingLicense_c, bool FS_IC_c)
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
                Customer oCustomer = new Customer();
                oCustomer.Company = strCurCompany;
                oCustomer.CustID = strCustId;
                oCustomer.CustName = strCustName;

                oCustomer.Address1 = (strAddress1 == "" || strAddress1 == null ? "" : strAddress1);
                oCustomer.Address2 = (strAddress2 == "" || strAddress2 == null ? "" : strAddress2);
                oCustomer.Address3 = (strAddress3 == "" || strAddress3 == null ? "" : strAddress3);
                oCustomer.City = (strCity == "" || strCity == null ? "" : strCity);
                oCustomer.State = (strState == "" || strState == null ? "" : strState);
                oCustomer.Zip = (strZip == "" || strZip == null ? "" : strZip);
                oCustomer.CountryNum = (iCountryNum == 0 ? 0 : iCountryNum);
                oCustomer.PhoneNum = (strPhoneNum == "" || strPhoneNum == null ? "" : strPhoneNum);
                oCustomer.FaxNum = (strFaxNum == "" || strFaxNum == null ? "" : strFaxNum);
                oCustomer.EMailAddress = (strEMailAddress == "" || strEMailAddress == null ? "" : strEMailAddress);
                oCustomer.CustURL = (strCustURL == "" || strCustURL == null ? "" : strCustURL);

                oCustomer.TerritoryID = (strTerritoryID == "" || strTerritoryID == null ? "" : strTerritoryID);
                oCustomer.SalesRepCode = (strSalesRepCode == "" || strSalesRepCode == null ? "" : strSalesRepCode);
                oCustomer.TermsCode = (strTermsCode == "" || strTermsCode == null ? "" : strTermsCode);
                oCustomer.GroupCode = (strGroupCode == "" || strGroupCode == null ? "" : strGroupCode);
                oCustomer.BusinessCatList = (strBusinessCatList == "" || strBusinessCatList == null ? "" : strBusinessCatList);

                oCustomer.FS_APCLicense_c = FS_APCLicense_c;
                oCustomer.FS_MMAMembership_c = FS_MMAMembership_c;
                oCustomer.FS_GVOTForm24_c = FS_GVOTForm24_c;
                oCustomer.FS_GVOTForm49_c = FS_GVOTForm49_c;
                oCustomer.FS_BussReg_c = FS_BussReg_c;
                oCustomer.FS_TradingLicense_c = FS_TradingLicense_c;
                oCustomer.FS_IC_c = FS_IC_c;


                EpicorBO oEpicor = new EpicorBO();

                IsTrxComplete = oEpicor._EpicCreateCustomer(ref oEpicorEnv, ref oCustomer, out strReturnMsg, strUID, strPass);

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
        public HttpResponseMessage PerformNewQuoteHed(string strUID, string strPass, string strEnvId, string strCurCompany, string strQuoteNum, int iCustNum, string strSalesPerson, string strDate, string strPONum, string strShipVia)
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
                Quote oQuote = new Quote();
                oQuote.Company = strCurCompany;
                //oQuote.ExtQuoteNum = strQuoteNum;
                oQuote.CustNum = iCustNum;
                oQuote.ShipToCustNum = iCustNum;
                //oQuote.CustID = "DKOG0944";
                oQuote.CustomerCustID = "DKOG0944";
                oQuote.ShipToCustID = "DKOG0944";
                //oQuote.EntryDate = (strDate == null || strDate == "" ? Convert.ToDateTime("1900-01-01") : Convert.ToDateTime(strDate));
                //oQuote.DueDate = (strDate == null || strDate == "" ? Convert.ToDateTime("1900-01-01") : Convert.ToDateTime(strDate)); ;
                //oQuote.SalesPerson = strSalesPerson;
                oQuote.PONum = strPONum;
                oQuote.ShipViaCode = string.IsNullOrEmpty(strShipVia) ? "":strShipVia;
                oQuote.QuoteComment = "Testing 123";
                oQuote.ShipToNum = "DEL 01";
                oQuote.TermsCode = "-";


                QuoteDtls quoteDtls = new QuoteDtls();
                quoteDtls.Company = strCurCompany;
                quoteDtls.LineDesc = "AURORIX 150mg TAB 100s";
                quoteDtls.OrderQty = 2;
                quoteDtls.PartNum = "MAAUR150100AC";
                quoteDtls.ProdCode = "MA";
                quoteDtls.QuoteComment = "sample rest api";
                quoteDtls.QuoteLine = 0;
                quoteDtls.QuoteNum = 0;
                quoteDtls.SellingExpectedQty = 2;
                quoteDtls.SellingExpectedUM = "BX";

                QuoteDtls quoteDtls1 = new QuoteDtls();
                quoteDtls1.Company = strCurCompany;
                quoteDtls1.LineDesc = "AURORIX 150mg TAB 100s";
                quoteDtls1.OrderQty = 2;
                quoteDtls1.PartNum = "MAAUR150100AC";
                quoteDtls1.ProdCode = "MA";
                quoteDtls1.QuoteComment = "sample rest api";
                quoteDtls1.QuoteLine = 0;
                quoteDtls1.QuoteNum = 0;
                quoteDtls1.SellingExpectedQty = 2;
                quoteDtls1.SellingExpectedUM = "BX";


                QuoteDtls[] items = { quoteDtls, quoteDtls1 };
                //QuoteDtls[] items = { quoteDtls1 };
                oQuote.QuoteDtls = items;

                EpicorREST oREST = new EpicorREST();
                IsTrxComplete = oREST._CreateQuote(ref oEpicorEnv, ref oQuote, out strReturnMsg, strUID, strPass);


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