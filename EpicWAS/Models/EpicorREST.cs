using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using Epicor.ServiceModel.Headers;
using Epicor.Hosting;
using Newtonsoft.Json;
using System.Text;

namespace EpicWAS.Models
{
    public class EpicorREST
    {

        public bool _CreateQuote(ref EpicEnv oEpicEnv, ref Quote oQuote, out string strMessage, string strUID = "", string strPass = "")
        {
            strMessage = string.Empty;
            string ServiceUrl = oEpicEnv.Env_RESTAPIURL;  //"https://AHCG-APPSVR.ahcg.com.my/e102test/"; //oEpicEnv.Env_RESTAPIURL;
            string ServiceKey = oEpicEnv.Env_RESTAPIKEY;  //"788YM8epyHCo5rogl0ddoid9xtzeLSjUSsRJnp7RFdjIa"; //oEpicEnv.Env_RESTAPIKEY;
            bool IsEpicTrxSuccess = false;

            try
            {
                var nQoute = oQuote;

                HttpClient client = CreateClient("manager", "P@ssw0rd", "AP", "MfgSys");

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ServiceUrl + "api/v2/odata/AP/Erp.BO.QuoteSvc/Quotes?api-key=" + ServiceKey);

                request.Content = new StringContent(JsonConvert.SerializeObject(nQoute), Encoding.Default, "application/json");
                /*
                request.Content = new StringContent(JsonConvert.SerializeObject(new
                {
                    Company = oQuote.Company,
                    CustNum = oQuote.CustNum,
                    QuoteComment = oQuote.QuoteComment,
                    PONum = oQuote.PONum,
                    CustomerCustID = oQuote.CustId,
                    ShipToCustNum = oQuote.CustNum,
                    ShipToCustID = oQuote.CustId,
                    ShipViaCode = oQuote.ShipVia,
                    ShipToNum = oQuote.ShipToNum,
                    //EntryDate = oQuote.EntryDate,
                    //DueDate = oQuote.DueDate,
                    TermsCode = oQuote.TermsCode

                }), Encoding.Default, "application/json");
                */

                HttpResponseMessage response = client.SendAsync(request).Result;

                if (response.IsSuccessStatusCode)
                {
                    IsEpicTrxSuccess = true;
                    strMessage = "";
                }
                else
                {
                    IsEpicTrxSuccess = false;
                    strMessage = response.ReasonPhrase;
                }
            }
            catch (Exception e)
            {
                IsEpicTrxSuccess = false;
            }

            return IsEpicTrxSuccess;
        }


        public bool _ReprintInvoice(ref EpicEnv oEpicEnv, ref UD17 oUD17, string strCurCompany, string strCurPlant, out string strMessage, string strUID = "", string strPass = "")
        {
            strMessage = string.Empty;
            string ServiceUrl = oEpicEnv.Env_RESTAPIURL;  
            string ServiceKey = oEpicEnv.Env_RESTAPIKEY;  
            bool IsEpicTrxSuccess = false;

            try
            {
                var nUD17 = oUD17;

                //HttpClient client = CreateClient("manager", "P@ssw0rd", "AP", "MfgSys");
                HttpClient client = CreateClient(strUID, strPass, strCurCompany, strCurPlant);

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ServiceUrl + "api/v2/odata/AP/Ice.BO.UD17Svc/UD17s?api-key=" + ServiceKey);

                request.Content = new StringContent(JsonConvert.SerializeObject(nUD17), Encoding.Default, "application/json");
                /*
                request.Content = new StringContent(JsonConvert.SerializeObject(new
                {
                    Company = oQuote.Company,
                    CustNum = oQuote.CustNum,
                    QuoteComment = oQuote.QuoteComment,
                    PONum = oQuote.PONum,
                    CustomerCustID = oQuote.CustId,
                    ShipToCustNum = oQuote.CustNum,
                    ShipToCustID = oQuote.CustId,
                    ShipViaCode = oQuote.ShipVia,
                    ShipToNum = oQuote.ShipToNum,
                    //EntryDate = oQuote.EntryDate,
                    //DueDate = oQuote.DueDate,
                    TermsCode = oQuote.TermsCode

                }), Encoding.Default, "application/json");
                */

                HttpResponseMessage response = client.SendAsync(request).Result;

                if (response.IsSuccessStatusCode)
                {
                    IsEpicTrxSuccess = true;
                    strMessage = "";
                }
                else
                {
                    IsEpicTrxSuccess = false;
                    strMessage = response.ReasonPhrase;
                }
            }
            catch (Exception e)
            {
                IsEpicTrxSuccess = false;
            }

            return IsEpicTrxSuccess;


        }

        private HttpClient CreateClient(string strUserId, string strUserPass, string strCompany, string strPlant)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", strUserId, strUserPass))));
            //header to set current company
            ICallHeader hdr = new CallSettings(strCompany, strPlant, "", "");
            client.DefaultRequestHeaders.Add(hdr.Name, JsonConvert.SerializeObject(hdr));
            return client;
        }


    }
}