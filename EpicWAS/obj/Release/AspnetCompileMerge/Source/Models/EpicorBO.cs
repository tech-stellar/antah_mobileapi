using System;
using System.Collections.Generic;
using System.Linq;

using System.ServiceModel;
using System.ServiceModel.Channels;
using EpicWAS.Epicor.SessionModSvc;
using EpicWAS.Epicor.IssueReturnSvc;
using EpicWAS.Epicor.InvTransferSvc;
using EpicWAS.Epicor.ReceiptsFromMfgSvc;
using EpicWAS.Epicor.LotSelectUpdateSvc;
using EpicWAS.Epicor.ReceiptSvc;
using EpicWAS.Epicor.CustomerSvc;
using EpicWAS.Epicor.UD25Svc;
using EpicWAS.Epicor.CustShipSvc;
using EpicWAS.Epicor.SplitMergeUOMSvc;
using EpicWAS.Epicor.InventoryQtyAdjSvc;
using EpicWAS.Epicor.EmpBasicSvc;
using EpicWAS.Epicor.LaborSvc;
using EpicWAS.Epicor.ReportQtySvc;
using EpicWAS.Epicor.UD18Svc;
using EpicWAS.Epicor.PickedOrdersSvc;
using EpicWAS.Epicor.MaterialQueueSvc;
//using EpicWAS.Epicor.UD14Svc;
//using EpicWAS.Epicor.QuoteSvc;
//using EpicWAS.Epicor.SalesOrderSvc;
//
//using EpicWAS.Epicor.BAQReportSvc;
//using EpicWAS.Epicor.DynamicReportSvc;
//using EpicWAS.Epicor.ReportMonitorSvc;

using System.Data;
using System.Configuration;
using System.Text.RegularExpressions;

namespace EpicWAS.Models
{
    public class EpicorBO
    {
        private enum EndpointBindingType
        {
            SOAPHttp,
            BasicHttp
        }

        private static WSHttpBinding GetWsHttpBinding()
        {
            var binding = new WSHttpBinding();
            const int maxBindingSize = Int32.MaxValue;
            binding.MaxReceivedMessageSize = maxBindingSize;
            binding.ReaderQuotas.MaxDepth = maxBindingSize;
            binding.ReaderQuotas.MaxStringContentLength = maxBindingSize;
            binding.ReaderQuotas.MaxArrayLength = maxBindingSize;
            binding.ReaderQuotas.MaxBytesPerRead = maxBindingSize;
            binding.ReaderQuotas.MaxNameTableCharCount = maxBindingSize;
            binding.Security.Mode = SecurityMode.Message;
            binding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;
            return binding;
        }

        public static BasicHttpBinding GetBasicHttpBinding()
        {
            var binding = new BasicHttpBinding();
            const int maxBindingSize = Int32.MaxValue;
            binding.MaxReceivedMessageSize = maxBindingSize;
            binding.ReaderQuotas.MaxDepth = maxBindingSize;
            binding.ReaderQuotas.MaxStringContentLength = maxBindingSize;
            binding.ReaderQuotas.MaxArrayLength = maxBindingSize;
            binding.ReaderQuotas.MaxBytesPerRead = maxBindingSize;
            binding.ReaderQuotas.MaxNameTableCharCount = maxBindingSize;
            binding.Security.Mode = BasicHttpSecurityMode.TransportWithMessageCredential;
            binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
            return binding;
        }

        private static TClient GetClient<TClient, TInterface>(
            string url, string username, string password,   EndpointBindingType bindingType)
            where TClient : ClientBase<TInterface>
            where TInterface : class
            {
                Binding binding = null;
                TClient client;
                var endpointAddress = new EndpointAddress(url);
                switch (bindingType)
                {
                    case EndpointBindingType.BasicHttp:
                        binding = GetBasicHttpBinding();
                        break;
                    case EndpointBindingType.SOAPHttp:
                        binding = GetWsHttpBinding();
                        break;
                }

                TimeSpan operationTimeout = new TimeSpan(0, 12, 0);
                binding.CloseTimeout = operationTimeout;
                binding.ReceiveTimeout = operationTimeout;
                binding.SendTimeout = operationTimeout;
                binding.OpenTimeout = operationTimeout;

                client = (TClient)Activator.CreateInstance(typeof(TClient), binding, endpointAddress);
                if (!string.IsNullOrEmpty(username) && (client.ClientCredentials != null))
                {
                    client.ClientCredentials.UserName.UserName = username;
                    client.ClientCredentials.UserName.Password = password;
                }
                return client;
            }


        public static string strSessionModSvcPath = "Ice/Lib/SessionMod.svc";
        public static string strIssueReturnSvcPath = "ERP/BO/IssueReturn.svc";
        public static string strInvTransferSvcPath = "ERP/BO/InvTransfer.svc";
        public static string strReceiptsFromMfgSvcPath = "ERP/BO/ReceiptsFromMfg.svc";
        public static string strLotSelectUpdateSvcPath = "ERP/BO/LotSelectUpdate.svc";
        public static string strReceiptSvcPath = "ERP/BO/Receipt.svc";
        public static string strUD25SvcPath = "ICE/BO/UD25.svc";
        public static string strCustomerSvcPath = "ERP/BO/Customer.svc";
        public static string strSplitMergeUOMSvcPath = "ERP/BO/SplitMergeUOM.svc";
        public static string strInventoryQtyAdjSvcPath = "ERP/BO/InventoryQtyAdj.svc";
        public static string strEmpBasicSvcPath = "ERP/BO/EmpBasic.svc";
        public static string strLaborSvcPath = "ERP/BO/Labor.svc";
        public static string strReportQtySvcPath = "ERP/BO/ReportQty.svc";
        public static string strUD18SvcPath = "ICE/BO/UD18.svc";
        public static string strCustShipSvcPath = "ERP/BO/CustShip.svc";
        public static string strPickedOrdersSvcPath = "ERP/BO/PickedOrders.svc";
        public static string strMaterialQueueSvcPath = "ERP/BO/MaterialQueue.svc";
        //public static string strUD14SvcPath = "ICE/BO/UD14.svc";
        //public static string strBAQReportSvcPath = "Ice/Rpt/BAQReport.Svc";
        //public static string strDynamicReportSvcPath = "Ice/BO/DynamicReport.svc";
        //public static string strReportMonitorSvcPath = "Ice/BO/ReportMonitor.svc";


        public bool _LoginIntoEpicor(ref EpicUser oEpicUser, ref EpicEnv oEpicEnv, out string strMessage)
        {

            bool IsEpicLoginOK = false;
            string scheme = "http";
            string strEpicSessionModSvc = oEpicEnv.Env_AppEpicor + "/" + strSessionModSvcPath;

            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => { return true; };
            EndpointBindingType bindingType = EndpointBindingType.BasicHttp;

            if (bindingType == EndpointBindingType.BasicHttp)
            {
                scheme = "https";
            }

            UriBuilder builder = new UriBuilder(scheme, oEpicEnv.Env_AppServer);
            builder.Path = strEpicSessionModSvc;

            SessionModSvcContractClient sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), oEpicUser.Epic_UserId, oEpicUser.Epic_PassKey, bindingType);

            Guid sessionId = Guid.Empty;

            try
            {
                sessionId = sessionModClient.Login();

                builder.Path = strEpicSessionModSvc;
                sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), oEpicUser.Epic_UserId, oEpicUser.Epic_PassKey, bindingType);
                sessionModClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, oEpicUser.Epic_UserId));

            }
            catch (Exception ex)
            {
                IsEpicLoginOK = false;
                strMessage = ex.Message.ToString();
            }

            if (sessionId != Guid.Empty)
            {
                IsEpicLoginOK = true;
                strMessage = "";
                sessionModClient.Logout();
            }
            else
            {
                IsEpicLoginOK = false;
                strMessage = "Please check the username and password";
            }

            return IsEpicLoginOK;


        }

        public bool _EpicIssueMaterialToJob( ref EpicEnv oEpicEnv, ref IssueMtlToJob oIssueMtlToJob, out string strMessage, string strUID ="", string strPass ="")
        {

            bool IsEpicLoginOK = false;
            bool IsEpicTrxSuccess = false;

            string scheme = "http";
            string strEpicSessionModSvc = oEpicEnv.Env_AppEpicor + "/" + strSessionModSvcPath;
            string strEpicIssueMaterialSvc = oEpicEnv.Env_AppEpicor + "/" + strIssueReturnSvcPath;
            string partTran = "";

            string strEpicUser = (oEpicEnv.Env_UsedEpicLogin? strUID: oEpicEnv.Env_AppUserId);
            string strEpicPwd = (oEpicEnv.Env_UsedEpicLogin? strPass: oEpicEnv.Env_AppPassKey);


            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => { return true; };
            EndpointBindingType bindingType = EndpointBindingType.BasicHttp;

            if (bindingType == EndpointBindingType.BasicHttp)
            {
                scheme = "https";
            }

            UriBuilder builder = new UriBuilder(scheme, oEpicEnv.Env_AppServer);

            builder.Path = strEpicSessionModSvc;
            SessionModSvcContractClient sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            builder.Path = strEpicIssueMaterialSvc;
            IssueReturnSvcContractClient issueReturnClient = GetClient<IssueReturnSvcContractClient, IssueReturnSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            Guid sessionId = Guid.Empty;

            try
            {
                sessionId = sessionModClient.Login();

                string companyName;
                string plantID;
                string plantName;
                string workstationID;
                string workstationDescription;
                string employeeID;
                string countryGroupCode;
                string countryCode;

                
                builder.Path = strEpicSessionModSvc;
                sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

                sessionModClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                sessionModClient.SetCompany(oIssueMtlToJob.Company, out companyName, out plantID, out plantName, out workstationID, out workstationDescription,
                                            out employeeID, out countryGroupCode, out countryCode);

                if (oIssueMtlToJob.CurrentPlant != "")
                {
                    sessionModClient.SetPlant(oIssueMtlToJob.CurrentPlant);
                }

                IsEpicLoginOK = true;
                strMessage = "";

            }
            catch (Exception ex)
            {
                IsEpicLoginOK = false;
                strMessage = ex.Message.ToString();
            }


            if (IsEpicLoginOK == true)
            {
                issueReturnClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                var ts = new  IssueReturnTableset();
                issueReturnClient.GetNewIssueReturnToJob(oIssueMtlToJob.ToJobNum, oIssueMtlToJob.ToJobAssemblySeq, "STK-MTL", Guid.NewGuid(), ref ts);

                var newRow = ts.IssueReturn[0]; //.Where(n => n.RowMod.Equals("A", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                
                if (newRow != null)
                {                  

                    newRow.Company = oIssueMtlToJob.Company.ToUpper();
                    newRow.PartNum = oIssueMtlToJob.FromPartNum.ToUpper();
                    newRow.PartIUM = oIssueMtlToJob.FromPartIUM.ToUpper();
                    newRow.DimCode = oIssueMtlToJob.FromPartIUM.ToUpper();
                    newRow.DUM = oIssueMtlToJob.FromPartIUM.ToUpper();
                    newRow.UM = oIssueMtlToJob.FromPartIUM.ToUpper();
                    newRow.LotNum = oIssueMtlToJob.FromLotNum.ToUpper();
                    newRow.TranQty = oIssueMtlToJob.FromQuantity;

                    newRow.FromWarehouseCode = oIssueMtlToJob.FromWhseCode.ToUpper();
                    newRow.FromBinNum = oIssueMtlToJob.FromBinNum.ToUpper();                    

                    newRow.ToWarehouseCode = oIssueMtlToJob.ToWhseCode.ToUpper();
                    newRow.ToBinNum = oIssueMtlToJob.ToBinNum.ToUpper();

                    newRow.ToJobNum = oIssueMtlToJob.ToJobNum.ToUpper();
                    newRow.ToAssemblySeq = oIssueMtlToJob.ToJobAssemblySeq;
                    newRow.ToJobSeq = oIssueMtlToJob.ToJobMaterialSeq;

                    newRow.ToJobPartNum = oIssueMtlToJob.ToJobPartNum.ToUpper();                                
                    newRow.ToJobSeqPartNum = oIssueMtlToJob.ToJobMaterialSeqPartNum.ToUpper();
                    
                    newRow.RowMod = "U";                

                    try
                    {
                        issueReturnClient.PrePerformMaterialMovement(ref ts);
                        issueReturnClient.PerformMaterialMovement(true, ref ts, out partTran);

                        oIssueMtlToJob.TranNum = partTran;
                    }
                    catch (Exception ex)
                    {
                        IsEpicTrxSuccess = false;
                        strMessage = ex.Message.ToString();
                    }

                    IsEpicTrxSuccess = (strMessage.Length > 0 ? false : true);

                    //strMessage = partTran;
                }
                else
                {
                    strMessage = "Unable to perform issue material to job. ";
                    IsEpicTrxSuccess = false;
                }

            }


            if (sessionId != Guid.Empty)
            {
                IsEpicLoginOK = true;
                sessionModClient.Logout();
            }
            else
            {
                IsEpicLoginOK = false;
                strMessage = "Unable to connect into Epicor";
            }

            return (IsEpicLoginOK && IsEpicTrxSuccess);
        }

        public bool _EpicReceiptsFromMfg(ref EpicEnv oEpicEnv, ref ReceiptsFromMfg oMfgRecv, out string strMessage, string strUID = "", string strPass = "")
        {
            bool IsEpicLoginOK = false;
            bool IsEpicTrxSuccess = false;

            string scheme = "http";
            string strEpicSessionModSvc = oEpicEnv.Env_AppEpicor + "/" + strSessionModSvcPath;
            string strEpicReceiptsFromMfgSvc = oEpicEnv.Env_AppEpicor + "/" + strReceiptsFromMfgSvcPath;
            string partTran = "";
            string strEpicUser = (oEpicEnv.Env_UsedEpicLogin ? strUID : oEpicEnv.Env_AppUserId);
            string strEpicPwd = (oEpicEnv.Env_UsedEpicLogin ? strPass : oEpicEnv.Env_AppPassKey);

            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => { return true; };
            EndpointBindingType bindingType = EndpointBindingType.BasicHttp;

            if (bindingType == EndpointBindingType.BasicHttp)
            {
                scheme = "https";
            }

            UriBuilder builder = new UriBuilder(scheme, oEpicEnv.Env_AppServer);

            builder.Path = strEpicSessionModSvc;
            SessionModSvcContractClient sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            builder.Path = strEpicReceiptsFromMfgSvc;
            ReceiptsFromMfgSvcContractClient receiptsFromMfgClient = GetClient<ReceiptsFromMfgSvcContractClient, ReceiptsFromMfgSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            Guid sessionId = Guid.Empty;

            try
            {
                sessionId = sessionModClient.Login();

                string companyName;
                string plantID;
                string plantName;
                string workstationID;
                string workstationDescription;
                string employeeID;
                string countryGroupCode;
                string countryCode;



                builder.Path = strEpicSessionModSvc;
                sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

                sessionModClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                sessionModClient.SetCompany(oMfgRecv.Company, out companyName, out plantID, out plantName, out workstationID, out workstationDescription,
                                            out employeeID, out countryGroupCode, out countryCode);

                if (oMfgRecv.CurrentPlant != "")
                {
                    sessionModClient.SetPlant(oMfgRecv.CurrentPlant);
                }

                IsEpicLoginOK = true;
                strMessage = "";

            }
            catch (Exception ex)
            {
                IsEpicLoginOK = false;
                strMessage = ex.Message.ToString();
            }


            if (IsEpicLoginOK == true)
            {
                receiptsFromMfgClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                var ts = new ReceiptsFromMfgTableset();

                receiptsFromMfgClient.GetNewReceiptsFromMfgJobAsm(oMfgRecv.JobNum, oMfgRecv.AssemblySeq, "MFG-STK", "RcptToInvEntry", ref ts);

                var newRow = ts.PartTran[0]; //.Where(n => n.RowMod.Equals("A", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

                if (newRow != null)
                {
                    newRow.Company = oMfgRecv.Company.ToUpper();
                    newRow.ActTranQty = oMfgRecv.TranQty;
                    newRow.TranQty = oMfgRecv.TranQty;
                    //newRow.ThisTranQty = oMfgRecv.TranQty;
                    newRow.InventoryTrans = true;
                    // - KOO - ADD FROM WHS AND BIN (START)
                    newRow.WareHouse2 = oMfgRecv.FromWarehouseCode.ToUpper();
                    newRow.BinNum2 = oMfgRecv.FromBinNum.ToUpper();
                    // - KOO - ADD FROM WHS AND BIN (END)
                    newRow.BinNum = oMfgRecv.ToBinNum.ToUpper();
                    newRow.WareHouseCode = oMfgRecv.ToWarehouseCode.ToUpper();
                    newRow.LotNum = (oMfgRecv.LotNum == null ? string.Empty : oMfgRecv.LotNum.ToUpper());
                    //newRow.UM = oMfgRecv.IUM;
                    newRow.TranReference = oMfgRecv.Reference; //added

                    newRow.RowMod = "U";
                    receiptsFromMfgClient.OnChangeActTranQty(ref ts);

                    //newRow.ActTransUOM = oMfgRecv.IUM;
                    newRow.RowMod = "U";
                    receiptsFromMfgClient.OnChangeActTransUOM(oMfgRecv.IUM, ref ts);

                    receiptsFromMfgClient.PreUpdate(ref ts);
                    receiptsFromMfgClient.ReceiveMfgPartToInventory(ref ts, 0, true, "RcptToInvEntry", out partTran);

                    string[] strArray = partTran.Split('~');

                    oMfgRecv.TranNum = strArray[2].ToString();

                    IsEpicTrxSuccess = (true);

                    strMessage = "";
                }
                else
                {
                    strMessage = "Unable to perform job receipt to inventory.";
                    IsEpicTrxSuccess = false;
                }

            }


            if (sessionId != Guid.Empty)
            {
                IsEpicLoginOK = true;
                sessionModClient.Logout();
            }
            else
            {
                IsEpicLoginOK = false;
                strMessage = "Unable to connect into Epicor";
            }

            return (IsEpicLoginOK && IsEpicTrxSuccess);


        }

        public bool _EpicInvTransfer(ref EpicEnv oEpicEnv, ref MoveInventory oMoveInventory, out string strMessage, string strUID = "", string strPass = "")
        {
            bool IsEpicLoginOK = false;
            bool IsEpicTrxSuccess = false;

            string scheme = "http";
            string strEpicSessionModSvc = oEpicEnv.Env_AppEpicor + "/" + strSessionModSvcPath;
            string strEpicInvTransferSvc = oEpicEnv.Env_AppEpicor + "/" + strInvTransferSvcPath;
            string partTran = "";
            string strEpicUser = (oEpicEnv.Env_UsedEpicLogin ? strUID : oEpicEnv.Env_AppUserId);
            string strEpicPwd = (oEpicEnv.Env_UsedEpicLogin ? strPass : oEpicEnv.Env_AppPassKey);

            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => { return true; };
            EndpointBindingType bindingType = EndpointBindingType.BasicHttp;

            if (bindingType == EndpointBindingType.BasicHttp)
            {
                scheme = "https";
            }

            UriBuilder builder = new UriBuilder(scheme, oEpicEnv.Env_AppServer);

            builder.Path = strEpicSessionModSvc;
            SessionModSvcContractClient sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            builder.Path = strEpicInvTransferSvc;
            InvTransferSvcContractClient invTransferClient = GetClient<InvTransferSvcContractClient, InvTransferSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            Guid sessionId = Guid.Empty;

            try
            {
                sessionId = sessionModClient.Login();

                string companyName;
                string plantID;
                string plantName;
                string workstationID;
                string workstationDescription;
                string employeeID;
                string countryGroupCode;
                string countryCode;



                builder.Path = strEpicSessionModSvc;
                sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

                sessionModClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                sessionModClient.SetCompany(oMoveInventory.Company, out companyName, out plantID, out plantName, out workstationID, out workstationDescription,
                                            out employeeID, out countryGroupCode, out countryCode);

                if (oMoveInventory.CurrentPlant != "" || oMoveInventory.CurrentPlant != null)
                {
                    sessionModClient.SetPlant(oMoveInventory.CurrentPlant);
                }

                IsEpicLoginOK = true;
                strMessage = "";

            }
            catch (Exception ex)
            {
                IsEpicLoginOK = false;
                strMessage = ex.Message.ToString();
                //strMessage = ex.Message.ToString();
            }


            if (IsEpicLoginOK == true)
            {
                invTransferClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                var ts = new InvTransferTableset();
                Guid rowId = Guid.Empty;

                invTransferClient.GetInvTransferRecords(oMoveInventory.PartNum, ts);
                invTransferClient.GetTransferRecord( oMoveInventory.PartNum, rowId,"", oMoveInventory.IUM, ref ts);
                //var ts = invTransferClient.GetTransferRecord(oMoveInventory.PartNum, oMoveInventory.IUM);

                var newRow = ts.InvTrans[0]; // .Where(n => n.RowMod.Equals("A", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

                if (newRow != null)
                {
                    try
                    {
                        ts.InvTrans[0].Company = oMoveInventory.Company.ToUpper();
                        ts.InvTrans[0].PartNum = oMoveInventory.PartNum.ToUpper();

                        PartBO oPartBO = new PartBO();
                        Part oPart = new Part();
                        //string strMessage="";

                        oPartBO._LoadPartById(ref oEpicEnv, oMoveInventory.Company, oMoveInventory.PartNum, ref oPart, out strMessage);

                        ts.InvTrans[0].PartDescription = oPart.PartDescription;


                        ts.InvTrans[0].TransferQty = oMoveInventory.TranQty;
                        //ts.InvTrans[0].TrackingQty = oMoveInventory.TranQty;

                        ts.InvTrans[0].FromWarehouseCode = oMoveInventory.FromWarehouseCode.ToUpper();
                        //ts.InvTrans[0].RowMod = "U";
                        //invTransferClient.ChangeFromWhse(ref ts);
                                              
                        ts.InvTrans[0].FromBinNum = oMoveInventory.FromBinNum.ToUpper();
                        //ts.InvTrans[0].RowMod = "U";
                        //invTransferClient.ChangeFromBin(ref ts);

                        ts.InvTrans[0].FromLotNumber = (oMoveInventory.FromLotNum == null ? "" : oMoveInventory.FromLotNum.ToUpper());
                    
                    //newRow.FromPlant = "";


                        ts.InvTrans[0].ToWarehouseCode = oMoveInventory.ToWarehouseCode.ToUpper();
                        //ts.InvTrans[0].RowMod = "U";
                        //invTransferClient.ChangeToWhse(ref ts);
                       
                        ts.InvTrans[0].ToBinNum = oMoveInventory.ToBinNum.ToUpper();
                        //newRow.ToPlant = "";
                        ts.InvTrans[0].TranReference = (oMoveInventory.Reference == null ? "" : oMoveInventory.Reference);
                        //ts.InvTrans[0].RowMod = "U";

                        //invTransferClient.ChangeToBin(ref ts);
                        //invTransferClient.ChangeUOM(ref ts);

                        ts.InvTrans[0].ToLotNumber = (oMoveInventory.ToLotNum == null ? "" : oMoveInventory.ToLotNum.ToUpper());
                        //ts.InvTrans[0].RowMod = "A";
                        //invTransferClient.ChangeLot(ref ts);
                        
                        invTransferClient.PreCommitTransfer(ref ts);
                        ts.InvTrans[0].RowMod = "A";
                        invTransferClient.CommitTransfer(ref ts, out partTran);

                        string[] retValues = partTran.Split('|');
                        string[] retValues1 = retValues[0].Split('~');

                        //oIssueAssembly.TranNum = retValues[2].ToString();
                        oMoveInventory.TranNum = retValues1[2].ToString();
                    }
                    catch (Exception ex)
                    {
                        
                        IsEpicTrxSuccess = false;
                        strMessage = ex.Message.ToString();
                    }

                    IsEpicTrxSuccess = (strMessage.Length > 0 ? false : true);

                    //strMessage = partTran;
                }
                else
                {
                    strMessage = "Unable to perform inventory transfer.";
                    IsEpicTrxSuccess = false;
                }

            }


            if (sessionId != Guid.Empty)
            {
                IsEpicLoginOK = true;
                sessionModClient.Logout();
            }
            else
            {
                IsEpicLoginOK = false;
                strMessage = "Unable to connect into Epicor";
            }

            return (IsEpicLoginOK && IsEpicTrxSuccess);


        }

        public bool _EpicJobReceiptsToSalvage(ref EpicEnv oEpicEnv,  ref JobSalvage2 oSalvage, out string strMessage, string strUID = "", string strPass = "")
        {

            bool IsEpicLoginOK = false;
            bool IsEpicTrxSuccess = false;

            string scheme = "http";
            string strEpicSessionModSvc = oEpicEnv.Env_AppEpicor + "/" + strSessionModSvcPath;
            string strEpicReceiptsFromMfgSvc = oEpicEnv.Env_AppEpicor + "/" + strReceiptsFromMfgSvcPath;
            string partTran = "";
            string os = "";
            string strEpicUser = (oEpicEnv.Env_UsedEpicLogin ? strUID : oEpicEnv.Env_AppUserId);
            string strEpicPwd = (oEpicEnv.Env_UsedEpicLogin ? strPass : oEpicEnv.Env_AppPassKey);


            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => { return true; };
            EndpointBindingType bindingType = EndpointBindingType.BasicHttp;

            if (bindingType == EndpointBindingType.BasicHttp)
            {
                scheme = "https";
            }

            UriBuilder builder = new UriBuilder(scheme, oEpicEnv.Env_AppServer);

            builder.Path = strEpicSessionModSvc;
            SessionModSvcContractClient sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            builder.Path = strEpicReceiptsFromMfgSvc;
            ReceiptsFromMfgSvcContractClient receiptsFromMfgClient = GetClient<ReceiptsFromMfgSvcContractClient, ReceiptsFromMfgSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            Guid sessionId = Guid.Empty;

            try
            {
                sessionId = sessionModClient.Login();

                string companyName;
                string plantID;
                string plantName;
                string workstationID;
                string workstationDescription;
                string employeeID;
                string countryGroupCode;
                string countryCode;


                builder.Path = strEpicSessionModSvc;
                sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

                sessionModClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                sessionModClient.SetCompany(oSalvage.Company, out companyName, out plantID, out plantName, out workstationID, out workstationDescription,
                                            out employeeID, out countryGroupCode, out countryCode);

                if (oSalvage.CurrentPlant != "")
                {
                    sessionModClient.SetPlant(oSalvage.CurrentPlant);
                }

                IsEpicLoginOK = true;
                strMessage = "";

            }
            catch (Exception ex)
            {
                IsEpicLoginOK = false;
                strMessage = ex.Message.ToString();
            }


            if (IsEpicLoginOK == true)
            {
                receiptsFromMfgClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                var ts = new ReceiptsFromMfgTableset();

                receiptsFromMfgClient.GetNewReceiptsFromMfg("SVG-STK", ref ts);

                if (ts.PartTran.Count != 0)
                {
                    try
                    {
                        ts.PartTran[0].Company = oSalvage.Company.ToUpper();

                        ts.PartTran[0].JobNum = oSalvage.JobNum.ToUpper();
                        ts.PartTran[0].AssemblySeq = oSalvage.AssemblySeq;
                        ts.PartTran[0].JobSeq = oSalvage.MtlSeq;
                        ts.PartTran[0].RowMod = "U";

                        receiptsFromMfgClient.OnChangeSalvageJobSeq(ref ts);

                        ts.PartTran[0].PartNum = oSalvage.PartNum.ToUpper();
                        ts.PartTran[0].PartNumMS = oSalvage.MtlPartNum.ToUpper();
                        ts.PartTran[0].RowMod = "U";
                        receiptsFromMfgClient.OnChangeSalvagePartNum(ref ts);

                        ts.PartTran[0].ActTranQty = oSalvage.dTranQty;
                        ts.PartTran[0].WareHouseCode = oSalvage.ToWarehouseCode.ToUpper();
                        ts.PartTran[0].ActTransUOM = oSalvage.IUM.ToUpper();
                        ts.PartTran[0].RowMod = "U";
                        receiptsFromMfgClient.OnChangeActTranQty(ref ts);

                        ts.PartTran[0].BinNum = oSalvage.ToBinNum.ToUpper();
                        ts.PartTran[0].TranReference = oSalvage.Reference;
                        ts.PartTran[0].RowMod = "U";
                        receiptsFromMfgClient.OnChangeBinNum(ref ts);

                        ts.PartTran[0].LotNum = oSalvage.LotNum.ToUpper();
                        ts.PartTran[0].RowMod = "U";
                        receiptsFromMfgClient.OnChangeLotNum(ref ts, false, oSalvage.LotNum.ToUpper(), out os);

                        receiptsFromMfgClient.PreUpdate(ref ts);
                        receiptsFromMfgClient.ReceiveSalvagedPartToInventory(ref ts, "RcptToSalEntry", out partTran);

                        oSalvage.TranNum = partTran;
                    }
                    catch (Exception ex)
                    {
                        IsEpicTrxSuccess = false;
                        strMessage = ex.Message.ToString();
                    }

                    IsEpicTrxSuccess = (strMessage.Length > 0 ? false : true);
                }
                else
                {
                    strMessage = "Unable to perform job receipt to salvage.";
                    IsEpicTrxSuccess = false;
                }

            }


            if (sessionId != Guid.Empty)
            {
                IsEpicLoginOK = true;
                sessionModClient.Logout();
            }
            else
            {
                IsEpicLoginOK = false;
                strMessage = "Unable to connect into Epicor";
            }

            return (IsEpicLoginOK && IsEpicTrxSuccess);

        }

        public bool _EpicLotSelectUpdate(ref EpicEnv oEpicEnv, ref PartLot oPartLot, out string strMessage, string strUID = "", string strPass = "")
        {
            bool IsEpicLoginOK = false;
            bool IsEpicTrxSuccess = false;

            string scheme = "http";
            string strEpicSessionModSvc = oEpicEnv.Env_AppEpicor + "/" + strSessionModSvcPath;
            string strEpicLotSelectUpdateSvc = oEpicEnv.Env_AppEpicor + "/" + strLotSelectUpdateSvcPath;
            string strEpicUser = (oEpicEnv.Env_UsedEpicLogin ? strUID : oEpicEnv.Env_AppUserId);
            string strEpicPwd = (oEpicEnv.Env_UsedEpicLogin ? strPass : oEpicEnv.Env_AppPassKey);

            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => { return true; };
            EndpointBindingType bindingType = EndpointBindingType.BasicHttp;

            if (bindingType == EndpointBindingType.BasicHttp)
            {
                scheme = "https";
            }

            UriBuilder builder = new UriBuilder(scheme, oEpicEnv.Env_AppServer);

            builder.Path = strEpicSessionModSvc;
            SessionModSvcContractClient sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            builder.Path = strEpicLotSelectUpdateSvc;
            LotSelectUpdateSvcContractClient lotSelectClient = GetClient<LotSelectUpdateSvcContractClient,LotSelectUpdateSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            Guid sessionId = Guid.Empty;

            try
            {
                sessionId = sessionModClient.Login();

                string companyName;
                string plantID;
                string plantName;
                string workstationID;
                string workstationDescription;
                string employeeID;
                string countryGroupCode;
                string countryCode;


                builder.Path = strEpicSessionModSvc;
                sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

                sessionModClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                sessionModClient.SetCompany(oPartLot.Company, out companyName, out plantID, out plantName, out workstationID, out workstationDescription,
                            out employeeID, out countryGroupCode, out countryCode);

                if (oPartLot.CurrentPlant != "" && oPartLot.CurrentPlant != null)
                {
                    sessionModClient.SetPlant(oPartLot.CurrentPlant);
                }

                IsEpicLoginOK = true;
                strMessage = "";

            }
            catch (Exception ex)
            {
                IsEpicLoginOK = false;
                strMessage = ex.Message.ToString();
            }


            if (IsEpicLoginOK == true)
            {
                lotSelectClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                var ts = new LotSelectUpdateTableset();
                lotSelectClient.GetNewPartLot(ref ts, oPartLot.PartNum);

                var newRow = ts.PartLot.Where(n => n.RowMod.Equals("A", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

                if (newRow != null)
                {
                    newRow.Company = oPartLot.Company.ToUpper();
                    newRow.PartNum = oPartLot.PartNum.ToUpper();
                    newRow.LotNum = oPartLot.LotNum.ToUpper();
                    newRow.PartLotDescription = oPartLot.PartLotDescription;
                    newRow.Batch = oPartLot.Batch.ToUpper();
                    newRow.MfgBatch = oPartLot.MfgBatch.ToUpper();
                    newRow.MfgLot = oPartLot.MfgLot.ToUpper();
                    newRow.HeatNum = oPartLot.HeatNum.ToUpper();
                    newRow.FirmWare = oPartLot.FirmWare.ToUpper();

                    if (oPartLot.BestBeforeDt != "")
                    {
                        newRow.BestBeforeDt = (Convert.ToDateTime(oPartLot.BestBeforeDt));
                    }

                    if (oPartLot.MfgDt != "")
                    {
                        newRow.MfgDt = (Convert.ToDateTime(oPartLot.MfgDt));
                    }

                    if (oPartLot.CureDt != "")
                    {
                        newRow.CureDt = (Convert.ToDateTime(oPartLot.CureDt));
                    }

                    if (oPartLot.ExpireDt != "")
                    {
                        newRow.ExpireDt = (Convert.ToDateTime(oPartLot.ExpireDt));
                    }

                    if (oPartLot.ExpireDt != "")
                    {
                        newRow.ExpirationDate = (Convert.ToDateTime(oPartLot.ExpireDt));
                    }

                    newRow.RowMod = "A";


                    //newRow.BestBeforeDt = oPartLot.BestBeforeDt;
                    //newRow.MfgDt = oPartLot.MfgDt;
                    //newRow.CureDt = oPartLot.CureDt;
                    //newRow.ExpireDt = oPartLot.ExpireDt;
                    //newRow.ExpirationDate = oPartLot.ExpireDt;

                    lotSelectClient.Update(ref ts);
                    
                    strMessage = "";
                    IsEpicTrxSuccess = true;
                }
                else
                {
                    strMessage = "Unable to create part lot.";
                    IsEpicTrxSuccess = false;
                }

            }
            else
            {
                strMessage = "Unable to create part lot.";
                IsEpicTrxSuccess = false;
            }

            if (sessionId != Guid.Empty)
            {
                IsEpicLoginOK = true;
                sessionModClient.Logout();
            }
            else
            {
                IsEpicLoginOK = false;
                strMessage = "Unable to connect into Epicor";
            }

            return (IsEpicLoginOK && IsEpicTrxSuccess);


        }

        public bool _EpicReturnMaterial(ref EpicEnv oEpicEnv, ref ReturnMtlToStock oReturnMtlToStock, out string strMessage, string strUID = "", string strPass = "")
        {

            bool IsEpicLoginOK = false;
            bool IsEpicTrxSuccess = false;

            string scheme = "http";
            string strEpicSessionModSvc = oEpicEnv.Env_AppEpicor + "/" + strSessionModSvcPath;
            string strEpicReturnMaterialSvc = oEpicEnv.Env_AppEpicor + "/" + strIssueReturnSvcPath;
            string partTran = "";
            string strEpicUser = (oEpicEnv.Env_UsedEpicLogin ? strUID : oEpicEnv.Env_AppUserId);
            string strEpicPwd = (oEpicEnv.Env_UsedEpicLogin ? strPass : oEpicEnv.Env_AppPassKey);

            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => { return true; };
            EndpointBindingType bindingType = EndpointBindingType.BasicHttp;

            if (bindingType == EndpointBindingType.BasicHttp)
            {
                scheme = "https";
            }

            UriBuilder builder = new UriBuilder(scheme, oEpicEnv.Env_AppServer);

            builder.Path = strEpicSessionModSvc;
            SessionModSvcContractClient sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            builder.Path = strEpicReturnMaterialSvc;
            IssueReturnSvcContractClient issueReturnClient = GetClient<IssueReturnSvcContractClient, IssueReturnSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            Guid sessionId = Guid.Empty;

            try
            {
                sessionId = sessionModClient.Login();

                string companyName;
                string plantID;
                string plantName;
                string workstationID;
                string workstationDescription;
                string employeeID;
                string countryGroupCode;
                string countryCode;              

                builder.Path = strEpicSessionModSvc;
                sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

                sessionModClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                sessionModClient.SetCompany(oReturnMtlToStock.Company, out companyName, out plantID, out plantName, out workstationID, out workstationDescription,
                                            out employeeID, out countryGroupCode, out countryCode);

                if (oReturnMtlToStock.Plant != "")
                {
                    sessionModClient.SetPlant(oReturnMtlToStock.Plant);
                }

                IsEpicLoginOK = true;
                strMessage = "";

            }
            catch (Exception ex)
            {
                IsEpicLoginOK = false;
                strMessage = ex.Message.ToString();
            }


            if (IsEpicLoginOK == true)
            {
                issueReturnClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                var ts = new IssueReturnTableset();
                issueReturnClient.GetNewIssueReturnFromJob(oReturnMtlToStock.FromJobNum, oReturnMtlToStock.FromAssemblySeq, "MTL-STK", Guid.NewGuid(), ref ts);

                var newRow = ts.IssueReturn[0]; // .Where(n => n.RowMod.Equals("A", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

                if (newRow != null)
                {
                    newRow.Company = oReturnMtlToStock.Company.ToUpper();                                   
                    newRow.FromJobNum = oReturnMtlToStock.FromJobNum.ToUpper();
                    newRow.FromAssemblySeq = oReturnMtlToStock.FromAssemblySeq;
                    newRow.FromJobSeq = oReturnMtlToStock.FromMaterialSeq;                  
                    newRow.FromWarehouseCode = oReturnMtlToStock.FromWarehouseCode.ToUpper();
                    newRow.FromBinNum = oReturnMtlToStock.FromBinNum.ToUpper();
                    newRow.FromJobPartNum = oReturnMtlToStock.FromJobPartNum.ToUpper();
                    newRow.FromJobSeqPartNum = oReturnMtlToStock.ToMaterialPartNum.ToUpper();
                    newRow.Plant = oReturnMtlToStock.Plant;

                    newRow.ToWarehouseCode = oReturnMtlToStock.ToWarehouseCode.ToUpper();
                    newRow.ToBinNum = oReturnMtlToStock.ToBinNum.ToUpper();
                    newRow.PartNum = oReturnMtlToStock.ToMaterialPartNum.ToUpper();
                    newRow.PartIUM = oReturnMtlToStock.ToMaterialPartIUM.ToUpper();
                    newRow.DimCode = oReturnMtlToStock.ToMaterialPartIUM.ToUpper();
                    newRow.DUM = oReturnMtlToStock.ToMaterialPartIUM.ToUpper();
                    newRow.UM = oReturnMtlToStock.ToMaterialPartIUM.ToUpper();
                    newRow.TranQty = oReturnMtlToStock.TranQty;
                    newRow.LotNum = oReturnMtlToStock.ToLotNum.ToUpper();
                    newRow.RowMod = "U";

                    try
                    {
                        issueReturnClient.PrePerformMaterialMovement(ref ts);
                        issueReturnClient.PerformMaterialMovement(true, ref ts, out partTran);

                        oReturnMtlToStock.TranNum = partTran;
                    }
                    catch (Exception ex)
                    {
                        IsEpicTrxSuccess = false;
                        strMessage = ex.Message.ToString();
                    }

                    IsEpicTrxSuccess = (strMessage.Length > 0 ? false : true);

                }
                else
                {
                    strMessage = "Unable to perform return material. ";
                    IsEpicTrxSuccess = false;
                }

            }


            if (sessionId != Guid.Empty)
            {
                IsEpicLoginOK = true;
                sessionModClient.Logout();
            }
            else
            {
                IsEpicLoginOK = false;
                strMessage = "Unable to connect into Epicor";
            }

            return (IsEpicLoginOK && IsEpicTrxSuccess);
        }

        public bool _EpicReceiptsHeader(ref EpicEnv oEpicEnv, ref ReceiptDetail oRecDtl, out string strMessage, string strCurPlant, string strUID = "", string strPass = "")
        {
            bool IsEpicLoginOK = false;
            bool IsEpicTrxSuccess = false;

            string scheme = "http";
            string strEpicSessionModSvc = oEpicEnv.Env_AppEpicor + "/" + strSessionModSvcPath;
            string strEpicReceiptSvc = oEpicEnv.Env_AppEpicor + "/" + strReceiptSvcPath;
            string strPurPoint = "";
            string strEpicUser = (oEpicEnv.Env_UsedEpicLogin ? strUID : oEpicEnv.Env_AppUserId);
            string strEpicPwd = (oEpicEnv.Env_UsedEpicLogin ? strPass : oEpicEnv.Env_AppPassKey);
            string warnmsg = "";
            string postatuswarnmsg = "";


            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => { return true; };
            EndpointBindingType bindingType = EndpointBindingType.BasicHttp;

            if (bindingType == EndpointBindingType.BasicHttp)
            {
                scheme = "https";
            }

            UriBuilder builder = new UriBuilder(scheme, oEpicEnv.Env_AppServer);

            builder.Path = strEpicSessionModSvc;
            SessionModSvcContractClient sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            builder.Path = strEpicReceiptSvc;
            ReceiptSvcContractClient receiptClient = GetClient<ReceiptSvcContractClient, ReceiptSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            Guid sessionId = Guid.Empty;

            try
            {
                sessionId = sessionModClient.Login();

                string companyName;
                string plantID;
                string plantName;
                string workstationID;
                string workstationDescription;
                string employeeID;
                string countryGroupCode;
                string countryCode;

                builder.Path = strEpicSessionModSvc;
                sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

                sessionModClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                sessionModClient.SetCompany(oRecDtl.Company, out companyName, out plantID, out plantName, out workstationID, out workstationDescription,
                                            out employeeID, out countryGroupCode, out countryCode);

                sessionModClient.SetPlant(strCurPlant);

                IsEpicLoginOK = true;
                strMessage = "";

            }
            catch (Exception ex)
            {
                IsEpicLoginOK = false;
                strMessage = ex.Message.ToString();
            }

            if (IsEpicLoginOK == true)
            {
                receiptClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));
                var ts = new ReceiptTableset();

                try
                {

                    Vendor oVendor = new Vendor();
                    MaintenanceBO oSetup = new MaintenanceBO();
                    string strReturnMsg;

                    oSetup._LoadVendorById(ref oEpicEnv, oRecDtl.Company, oRecDtl.VendorID, ref oVendor, out strReturnMsg);


                    receiptClient.GetNewRcvHeadWithPONum(ref ts, oVendor.VendorNum, "", oRecDtl.PONum);
                    /* 2021-04-20 = bug fixes on the po number checking */
                    if (oRecDtl.PONum != 0)
                    {
                        receiptClient.GetPOInfo(ref ts, oRecDtl.PONum, true, out strPurPoint, out warnmsg, out postatuswarnmsg);
                    }
                    else
                    {
                        ts.RcvHead[0].VendorNum = oVendor.VendorNum;

                    }
                    ts.RcvHead[0].Company = oRecDtl.Company.ToUpper();
                    ts.RcvHead[0].EntryPerson = oRecDtl.EntryPerson;
                    ts.RcvHead[0].ReceivePerson = oRecDtl.EntryPerson;
                    ts.RcvHead[0].PackSlip = oRecDtl.PackSlip.ToUpper();
                    ts.RcvHead[0].PONum = oRecDtl.PONum;
                    ts.RcvHead[0].EntryDate = DateTime.Today;
                    ts.RcvHead[0].ArrivedDate = DateTime.Today;
                    ts.RcvHead[0].ReceiptDate = DateTime.Today;

                    ts.RcvHead[0].RowMod = "A";

                    //receiptClient.Update(ref ts);

                    bool RunChkLCAmtBeforeUpdate = true;
                    bool RunChkHdrBeforeUpdate = true;
                    bool lRunChkDtl = false;
                    bool lRunChkDtlCompliance = false;
                    bool lRunCheckCompliance = false;
                    bool lRunPreUpdate = true;
                    bool lRunCreatePartLot = false;
                    bool lOkToUpdate = true;
                    bool lCompliant = false;
                    bool lRequiresUserInput = false;
                    bool lUpdateWasRun = false;

                    string opUpliftWarnMsg;
                    string opReceiptWarnMsg;
                    string opArriveWarnMsg;
                    string qMessageStr;
                    string sMessageStr;
                    string lcMessageStr;
                    string pcMessageStr;
                    string qDtlComplianceMsgStr;
                    string wrnLines;

                    receiptClient.UpdateMaster(RunChkLCAmtBeforeUpdate, RunChkHdrBeforeUpdate,
                                                oRecDtl.VendorNum, "", oRecDtl.PackSlip, 0,
                                                lRunChkDtl, lRunChkDtlCompliance, lRunCheckCompliance,
                                                lRunPreUpdate, lRunCreatePartLot, oRecDtl.PartNum,
                                                oRecDtl.LotNum, lOkToUpdate, ref ts, out opUpliftWarnMsg,
                                                out opReceiptWarnMsg, out opArriveWarnMsg, out qMessageStr,
                                                out sMessageStr, out lcMessageStr, out pcMessageStr,
                                                out qDtlComplianceMsgStr, out lCompliant, out lRequiresUserInput,
                                                out lUpdateWasRun, out wrnLines);







                    IsEpicTrxSuccess = true;

                }
                catch (Exception ex)
                {
                    IsEpicTrxSuccess = false;
                    strMessage = ex.Message.ToString();
                }

                IsEpicTrxSuccess = (strMessage.Length > 0 ? false : true);

            }
            else
            {
                strMessage = "Unable to create receipt haeder. ";
                IsEpicTrxSuccess = false;
            }


            if (sessionId != Guid.Empty)
            {
                IsEpicLoginOK = true;
                sessionModClient.Logout();
            }
            else
            {
                IsEpicLoginOK = false;
                strMessage = "Unable to connect into Epicor";
            }

            return (IsEpicLoginOK && IsEpicTrxSuccess);

        }

        public bool _EpicReceiptsDetail(ref EpicEnv oEpicEnv, ref ReceiptDetail oRecDtl, out string strMessage, string strCurPlant, string strUID = "", string strPass = "")
        {
            bool IsEpicLoginOK = false;
            bool IsEpicTrxSuccess = false;

            string scheme = "http";
            string strEpicSessionModSvc = oEpicEnv.Env_AppEpicor + "/" + strSessionModSvcPath;
            string strEpicReceiptSvc = oEpicEnv.Env_AppEpicor + "/" + strReceiptSvcPath;
            //string strPurPoint = "";
            string strEpicUser = (oEpicEnv.Env_UsedEpicLogin ? strUID : oEpicEnv.Env_AppUserId);
            string strEpicPwd = (oEpicEnv.Env_UsedEpicLogin ? strPass : oEpicEnv.Env_AppPassKey);

            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => { return true; };
            EndpointBindingType bindingType = EndpointBindingType.BasicHttp;

            if (bindingType == EndpointBindingType.BasicHttp)
            {
                scheme = "https";
            }

            UriBuilder builder = new UriBuilder(scheme, oEpicEnv.Env_AppServer);

            builder.Path = strEpicSessionModSvc;
            SessionModSvcContractClient sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            builder.Path = strEpicReceiptSvc;
            ReceiptSvcContractClient receiptClient = GetClient<ReceiptSvcContractClient, ReceiptSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            Guid sessionId = Guid.Empty;

            try
            {
                sessionId = sessionModClient.Login();

                string companyName;
                string plantID;
                string plantName;
                string workstationID;
                string workstationDescription;
                string employeeID;
                string countryGroupCode;
                string countryCode;
                

                builder.Path = strEpicSessionModSvc;
                sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

                sessionModClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                sessionModClient.SetCompany(oRecDtl.Company, out companyName, out plantID, out plantName, out workstationID, out workstationDescription,
                                            out employeeID, out countryGroupCode, out countryCode);

                sessionModClient.SetPlant(strCurPlant);

                IsEpicLoginOK = true;
                strMessage = "";

            }
            catch (Exception ex)
            {
                IsEpicLoginOK = false;
                strMessage = ex.Message.ToString();
            }

            if (IsEpicLoginOK == true)
            {
                receiptClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));
                //var ts = new ReceiptTableset();

                bool requireUserInput =false;
                string vWMessage, questionMsg, warnMsg;
                string strPartNum = "";
                int i;

                var ts = receiptClient.GetByID(oRecDtl.VendorNum, "", oRecDtl.PackSlip);

                receiptClient.GetNewRcvDtl(ref ts, oRecDtl.VendorNum, "", oRecDtl.PackSlip);

                i = ts.RcvDtl.Count() - 1;
                //var newRow = ts.RcvDtl[0]; //.Where(n => n.RowMod.Equals("A", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

                strPartNum = oRecDtl.PartNum;

                if (oRecDtl.PONum != 0)
                {
                    receiptClient.GetDtlPOInfo(ref ts, oRecDtl.VendorNum, "", oRecDtl.PackSlip, 0, oRecDtl.PONum, requireUserInput); // KOO - ADD CHANGE PO NUM 
                    receiptClient.GetDtlPOLineInfo(ref ts, oRecDtl.VendorNum, "", oRecDtl.PackSlip, 0, oRecDtl.POLine);
                    receiptClient.CheckDtlJobStatus(oRecDtl.PONum, oRecDtl.POLine, oRecDtl.POLineRel, "", out questionMsg, out warnMsg);
                    receiptClient.GetDtlPORelInfo(ref ts, oRecDtl.VendorNum, "", oRecDtl.PackSlip, 0, oRecDtl.POLineRel);
                }
                else
                {
                    ts.RcvDtl[i].ReceiptType = "M";
                    receiptClient.OnChangeDtlReceiptType(oRecDtl.VendorNum, "", oRecDtl.PackSlip, 0, "M", ref ts);

                    ts.RcvDtl[i].TranType = "PUR-STK";
                    ts.RcvDtl[i].JobSeqType = " ";
                    ts.RcvDtl[i].ReceivedTo = "PUR-STK";
                    ts.RcvDtl[i].PartNum = oRecDtl.PartNum.ToUpper();

                    ts.RcvDtl[i].PartDescription = oRecDtl.PartDesc.ToUpper();

                    receiptClient.CheckDtlSSN(oRecDtl.VendorNum, "", oRecDtl.PackSlip, 0, "PUR-STK", oRecDtl.PartNum, out vWMessage);
                    //receiptClient.GetDtlPartInfo(ref ts, oRecDtl.VendorNum, "", oRecDtl.PackSlip, 0,ref strPartNum, Guid.NewGuid(), "");

                    //ts.RcvDtl[i].PartNumIUM = oRecDtl.RecvUOM.ToUpper();
                    //ts.RcvDtl[i].PartNumPartDescription = "ZDBC";
                    //ts.RcvDtl[i].PartDescription = "ZDBC";
                    //ts.RcvDtl[i].PartNumSalesUM = oRecDtl.RecvUOM.ToUpper();

                    //ts.RcvDtl[i].TranReference = "";
                    //ts.RcvDtl[i].ArrivedDate = DateTime.Now;
                    //ts.RcvDtl[i].ChangeDate = DateTime.Now;
                    //ts.RcvDtl[i].ChangedBy = "manager";
                    

                }
                //ts.RcvDtl[0].Company = oRecDtl.Company;
                //ts.RcvDtl[0].PONum = oRecDtl.PONum;
                //newRow.VendorNum = oRecDtl.VendorNum;
                ts.RcvDtl[i].PackSlip = oRecDtl.PackSlip.ToUpper();
                ts.RcvDtl[i].PartNum = oRecDtl.PartNum.ToUpper();
                ts.RcvDtl[i].WareHouseCode = oRecDtl.WarehouseCode;
                ts.RcvDtl[i].BinNum = oRecDtl.BinNum;
                ts.RcvDtl[i].DimConvFactor = 1;
                ts.RcvDtl[i].IUM = oRecDtl.RecvUOM.ToUpper();
                ts.RcvDtl[i].PUM = oRecDtl.RecvUOM.ToUpper();
                //ts.RcvDtl[i].DUM = oRecDtl.RecvUOM.ToUpper();
                ts.RcvDtl[i].ThisTranUOM = oRecDtl.RecvUOM.ToUpper();



                //newRow.WareHouseCode = oRecDtl.WarehouseCode;
                //newRow.BinNum = oRecDtl.BinNum;
                if (oRecDtl.LotNum != "")
                {
                    ts.RcvDtl[i].LotNum = oRecDtl.LotNum.ToUpper();
                }

                ts.RcvDtl[i].OurQty = oRecDtl.TranQty;
                ts.RcvDtl[i].InputOurQty = oRecDtl.TranQty;
                //ts.RcvDtl[i].OurUnitCost = 1.2M;
                //ts.RcvDtl[i].OurUnInvcReceiptQty = oRecDtl.TranQty;
                //ts.RcvDtl[i].ReceivedComplete = false;


                string errorMsg;

                receiptClient.GetDtlQtyInfo(ref ts, oRecDtl.VendorNum, "", oRecDtl.PackSlip, 0, oRecDtl.TranQty, "", "QTY"); // change QTY BO - KOO 22072019
                receiptClient.GetDtlQtyInfo(ref ts, oRecDtl.VendorNum, "", oRecDtl.PackSlip, 0, 0, oRecDtl.RecvUOM, "UOM"); // change UOM BO - KOO 22072019



                if (oRecDtl.LotNum != "")
                {
                    receiptClient.CheckDtlLotInfo(ref ts, oRecDtl.VendorNum, "", oRecDtl.PackSlip, 0, oRecDtl.LotNum, out errorMsg); // change QTY BO - KOO 22072019
                    receiptClient.GetDtlLotInfo(ref ts, oRecDtl.VendorNum, "", oRecDtl.PackSlip, 0, oRecDtl.LotNum, out errorMsg);
                }

                ts.RcvDtl[i].Received = true;
                //newRow.Received = true;
                //newRow.RowMod = "A";
                ts.RcvDtl[i].RowMod = "A";


                try
                {
                    //receiptClient.Update(ref ts);

                    bool RunChkLCAmtBeforeUpdate = true;
                    bool RunChkHdrBeforeUpdate = true;
                    bool lRunChkDtl = true;
                    bool lRunChkDtlCompliance = false; //ROHS = true non ROHS = false (licence issue) -KOO
                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["EnableRoHS"]) == true) // web config check for enable ROHS -- KOO
                    { lRunChkDtlCompliance = true; }
                    bool lRunCheckCompliance = false;
                    bool lRunPreUpdate = true;
                    bool lRunCreatePartLot = false;
                    bool lOkToUpdate = true;
                    bool lCompliant = false;
                    bool lRequiresUserInput = false;
                    bool lUpdateWasRun = false;

                    string opUpliftWarnMsg;
                    string opReceiptWarnMsg;
                    string opArriveWarnMsg;
                    string qMessageStr;
                    string sMessageStr;
                    string lcMessageStr;
                    string pcMessageStr;
                    string qDtlComplianceMsgStr;
                    string wrnLines;


                    receiptClient.UpdateMaster(RunChkLCAmtBeforeUpdate, RunChkHdrBeforeUpdate,
                                                oRecDtl.VendorNum, "", oRecDtl.PackSlip, 0,
                                                lRunChkDtl, lRunChkDtlCompliance, lRunCheckCompliance,
                                                lRunPreUpdate, lRunCreatePartLot, oRecDtl.PartNum,
                                                oRecDtl.LotNum, lOkToUpdate, ref ts, out opUpliftWarnMsg,
                                                out opReceiptWarnMsg, out opArriveWarnMsg, out qMessageStr,
                                                out sMessageStr, out lcMessageStr, out pcMessageStr,
                                                out qDtlComplianceMsgStr, out lCompliant, out lRequiresUserInput,
                                                out lUpdateWasRun, out wrnLines);



                    oRecDtl.PackLine = ts.RcvDtl[i].PackLine;
                    oRecDtl.PurPoint = ts.RcvDtl[i].PurPoint;

                    receiptClient.ReceiveAllLines(true, ref ts);

                    IsEpicTrxSuccess = true;
                }

                catch (Exception ex)
                {
                    strMessage = ex.Message.ToString();
                    //ex.InnerException.InnerException.Message.ToString();
                    IsEpicTrxSuccess = false;
                }
                

            }
            else
            {
                strMessage = "Unable to create receipt detail. ";
                IsEpicTrxSuccess = false;
            }


            if (sessionId != Guid.Empty)
            {
                IsEpicLoginOK = true;
                sessionModClient.Logout();
            }
            else
            {
                IsEpicLoginOK = false;
                strMessage = "Unable to connect into Epicor";
            }

            return (IsEpicLoginOK && IsEpicTrxSuccess);

        }

        public bool _EpicGetCustInfo(ref EpicEnv oEpicEnv, ref Customer oCustomer, int iCustNum, out string strMessage, string strUID = "", string strPass = "")
        {

            bool IsEpicLoginOK = false;
            bool IsEpicTrxSuccess = false;

            string scheme = "http";
            string strEpicSessionModSvc = oEpicEnv.Env_AppEpicor + "/" + strSessionModSvcPath;
            string strCustomerSvc = oEpicEnv.Env_AppEpicor + "/" + strCustomerSvcPath;
            //string partTran = "";

            string strEpicUser = (oEpicEnv.Env_UsedEpicLogin ? strUID : oEpicEnv.Env_AppUserId);
            string strEpicPwd = (oEpicEnv.Env_UsedEpicLogin ? strPass : oEpicEnv.Env_AppPassKey);


            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => { return true; };
            EndpointBindingType bindingType = EndpointBindingType.BasicHttp;

            if (bindingType == EndpointBindingType.BasicHttp)
            {
                scheme = "https";
            }

            UriBuilder builder = new UriBuilder(scheme, oEpicEnv.Env_AppServer);

            builder.Path = strEpicSessionModSvc;
            SessionModSvcContractClient sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            builder.Path = strCustomerSvc;
            CustomerSvcContractClient customerClient = GetClient<CustomerSvcContractClient, CustomerSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            Guid sessionId = Guid.Empty;

            try
            {
                sessionId = sessionModClient.Login();

                string companyName;
                string plantID;
                string plantName;
                string workstationID;
                string workstationDescription;
                string employeeID;
                string countryGroupCode;
                string countryCode;


                builder.Path = strEpicSessionModSvc;
                sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

                sessionModClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                sessionModClient.SetCompany(oCustomer.Company, out companyName, out plantID, out plantName, out workstationID, out workstationDescription,
                                            out employeeID, out countryGroupCode, out countryCode);


                IsEpicLoginOK = true;
                strMessage = "";

            }
            catch (Exception ex)
            {
                IsEpicLoginOK = false;
                strMessage = ex.Message.ToString();
            }


            if (IsEpicLoginOK == true)
            {
                customerClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                try
                {

                    var dsc = customerClient.GetByID(iCustNum);

                    oCustomer.CustID = dsc.Customer[0].CustID;
                    oCustomer.CustNum = dsc.Customer[0].CustNum;
                    oCustomer.CustName = dsc.Customer[0].Name;
                    oCustomer.CustAddress = dsc.Customer[0].Address1 + (String.IsNullOrEmpty(dsc.Customer[0].Address2) ? "" : "~" + dsc.Customer[0].Address2) +
                        (String.IsNullOrEmpty(dsc.Customer[0].Address3) ? "" : "~" + dsc.Customer[0].Address3) + (String.IsNullOrEmpty(dsc.Customer[0].City) ? "" : "~" + dsc.Customer[0].City) +
                        (String.IsNullOrEmpty(dsc.Customer[0].State) ? "" : "~" + dsc.Customer[0].State) + (String.IsNullOrEmpty(dsc.Customer[0].Zip) ? "" : "~" + dsc.Customer[0].Zip) + 
                        (String.IsNullOrEmpty(dsc.Customer[0].Country) ? "" : "~" + dsc.Customer[0].Country);
                    IsEpicTrxSuccess = true;
                }
                catch (Exception ex)
                {
                    IsEpicTrxSuccess = false;
                    strMessage = ex.Message.ToString();
                }
                

            }


            if (sessionId != Guid.Empty)
            {
                IsEpicLoginOK = true;
                sessionModClient.Logout();
            }
            else
            {
                IsEpicLoginOK = false;
                strMessage = "Unable to connect into Epicor";
            }

            return (IsEpicLoginOK && IsEpicTrxSuccess);
        }

        public bool _EpicUpdateDeliveryTime(ref EpicEnv oEpicEnv, ref UD25 oUD25, string strLegalNumber, out string strMessage, string strUID = "", string strPass = "")
        {

            bool IsEpicLoginOK = false;
            bool IsEpicTrxSuccess = false;

            string scheme = "http";
            string strEpicSessionModSvc = oEpicEnv.Env_AppEpicor + "/" + strSessionModSvcPath;
            string strUD25Svc = oEpicEnv.Env_AppEpicor + "/" + strUD25SvcPath;
            //string partTran = "";

            string strEpicUser = (oEpicEnv.Env_UsedEpicLogin ? strUID : oEpicEnv.Env_AppUserId);
            string strEpicPwd = (oEpicEnv.Env_UsedEpicLogin ? strPass : oEpicEnv.Env_AppPassKey);


            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => { return true; };
            EndpointBindingType bindingType = EndpointBindingType.BasicHttp;

            if (bindingType == EndpointBindingType.BasicHttp)
            {
                scheme = "https";
            }

            UriBuilder builder = new UriBuilder(scheme, oEpicEnv.Env_AppServer);

            builder.Path = strEpicSessionModSvc;
            SessionModSvcContractClient sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            builder.Path = strUD25Svc;
            UD25SvcContractClient ud25Client = GetClient<UD25SvcContractClient, UD25SvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            Guid sessionId = Guid.Empty;

            try
            {
                sessionId = sessionModClient.Login();

                string companyName;
                string plantID;
                string plantName;
                string workstationID;
                string workstationDescription;
                string employeeID;
                string countryGroupCode;
                string countryCode;


                builder.Path = strEpicSessionModSvc;
                sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

                sessionModClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                sessionModClient.SetCompany(oUD25.Company, out companyName, out plantID, out plantName, out workstationID, out workstationDescription,
                                            out employeeID, out countryGroupCode, out countryCode);


                IsEpicLoginOK = true;
                strMessage = "";

            }
            catch (Exception ex)
            {
                IsEpicLoginOK = false;
                strMessage = ex.Message.ToString();
            }


            if (IsEpicLoginOK == true)
            {
                ud25Client.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                try
                {
                    var ts = new UD25Tableset();

                    ts = ud25Client.GetByID(strLegalNumber, strLegalNumber, "", "", "");

                    if (ts.UD25[0].Date01 != null)
                    {
                        IsEpicTrxSuccess = false;
                        strMessage = "DO already acknowledged in the system as delivered";
                        return (IsEpicLoginOK && IsEpicTrxSuccess);
                    }

                    ts.UD25[0].Date01 = DateTime.Now;
                    ts.UD25[0].UserDefinedColumns["ReceiveDate_c"] = DateTime.Now;
                    ts.UD25[0].RowMod = "U";

                    ud25Client.Update(ref ts);

                    IsEpicTrxSuccess = true;
                }
                catch (Exception ex)
                {
                    IsEpicTrxSuccess = false;
                    strMessage = ex.Message.ToString();
                }


            }


            if (sessionId != Guid.Empty)
            {
                IsEpicLoginOK = true;
                sessionModClient.Logout();
            }
            else
            {
                IsEpicLoginOK = false;
                strMessage = "Unable to connect into Epicor";
            }

            return (IsEpicLoginOK && IsEpicTrxSuccess);
        }

        public bool _GetSplitMergeUOM(ref EpicEnv oEpicEnv, ref SplitMergeParam oSplitMergeParam, ref SplitMergeHead oSplitMergeHead, ref IList<SplitMergeDetail> oSplitMergeDetailLst, out string strMessage, string strUID = "", string strPass = "")
        {

            bool IsEpicLoginOK = false;
            bool IsEpicTrxSuccess = false;

            string scheme = "http";
            string strEpicSessionModSvc = oEpicEnv.Env_AppEpicor + "/" + strSessionModSvcPath;
            string strEpicSplitMergeUOMSvc = oEpicEnv.Env_AppEpicor + "/" + strSplitMergeUOMSvcPath;
            string strEpicUser = (oEpicEnv.Env_UsedEpicLogin ? strUID : oEpicEnv.Env_AppUserId);
            string strEpicPwd = (oEpicEnv.Env_UsedEpicLogin ? strPass : oEpicEnv.Env_AppPassKey);

            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => { return true; };
            EndpointBindingType bindingType = EndpointBindingType.BasicHttp;

            if (bindingType == EndpointBindingType.BasicHttp)
            {
                scheme = "https";
            }

            UriBuilder builder = new UriBuilder(scheme, oEpicEnv.Env_AppServer);

            builder.Path = strEpicSessionModSvc;
            SessionModSvcContractClient sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            builder.Path = strEpicSplitMergeUOMSvc;
            SplitMergeUOMSvcContractClient splitMergeUOMClient = GetClient<SplitMergeUOMSvcContractClient, SplitMergeUOMSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            Guid sessionId = Guid.Empty;

            try
            {
                sessionId = sessionModClient.Login();

                string companyName;
                string plantID;
                string plantName;
                string workstationID;
                string workstationDescription;
                string employeeID;
                string countryGroupCode;
                string countryCode;

                builder.Path = strEpicSessionModSvc;
                sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

                sessionModClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                sessionModClient.SetCompany(oSplitMergeParam.Company, out companyName, out plantID, out plantName, out workstationID, out workstationDescription,
                                            out employeeID, out countryGroupCode, out countryCode);

                if (oSplitMergeParam.CurrentPlant != "")
                {
                    sessionModClient.SetPlant(oSplitMergeParam.CurrentPlant);
                }
                IsEpicLoginOK = true;
                strMessage = "";

            }
            catch (Exception ex)
            {
                IsEpicLoginOK = false;
                strMessage = ex.Message.ToString();
            }


            if (IsEpicLoginOK == true)
            {
                splitMergeUOMClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                var ts = new SplitMergeUOMTableset();
                try
                {
                    ts = splitMergeUOMClient.GetSplitMergeHeadData(oSplitMergeParam.PartNum);
                    splitMergeUOMClient.OnChangeWarehouse(oSplitMergeParam.PartNum, oSplitMergeParam.WarehouseCode, ref ts);
                    splitMergeUOMClient.OnChangeBinNum(oSplitMergeParam.PartNum, oSplitMergeParam.BinNum, ref ts);
                    splitMergeUOMClient.OnChangeLotNum(oSplitMergeParam.PartNum, oSplitMergeParam.LotNum, ref ts);
                    splitMergeUOMClient.OnChangeUOM(oSplitMergeParam.PartNum, oSplitMergeParam.UOM, ref ts);
                    splitMergeUOMClient.OnChangeQuantity(oSplitMergeParam.PartNum, oSplitMergeParam.Qty, ref ts);
                    splitMergeUOMClient.OnChangeProcType(oSplitMergeParam.PartNum, oSplitMergeParam.ProcessType, ref ts);
                    //splitMergeUOMClient.OnChangeUOM(oSplitMergeParam.PartNum, oSplitMergeParam.UOM, ref ts);

                    int rowno = 0;

                    foreach (SMDtlRow row in ts.SMDtl)
                    {
                        SplitMergeDetail oSplitMergeDetail = new SplitMergeDetail();
                        oSplitMergeDetail.RowNo = rowno;
                        oSplitMergeDetail.Company = row.Company.ToUpper();
                        oSplitMergeDetail.PartNum = row.PartNum.ToUpper();
                        oSplitMergeDetail.WarehouseCode = row.WarehouseCode.ToUpper();
                        oSplitMergeDetail.BinNum = row.BinNum.ToUpper();
                        oSplitMergeDetail.LotNum = row.LotNum.ToUpper();
                        oSplitMergeDetail.OnHandQty = row.OnHandQty;
                        oSplitMergeDetail.Qty = row.Quantity;
                        oSplitMergeDetail.UOM = row.UOM.ToUpper();
                        oSplitMergeDetail.ConvFact = row.ConvFactor;
                        oSplitMergeDetail.ConvFactUOM = row.ConvFactorUOM;
                        oSplitMergeDetail.AllocatedQty = row.AllocatedQty;
                        rowno ++;

                        oSplitMergeDetailLst.Add(oSplitMergeDetail);
                    }
                    IsEpicTrxSuccess = true;
                }
                catch (Exception ex)
                {
                    IsEpicTrxSuccess = false;
                    strMessage = ex.Message.ToString();
                }

                if (sessionId != Guid.Empty)
                {
                    IsEpicLoginOK = true;
                    sessionModClient.Logout();
                }
                else
                {
                    IsEpicLoginOK = false;
                    strMessage = "Unable to connect into Epicor";
                }
                
            }
            return (IsEpicLoginOK && IsEpicTrxSuccess);
        }

        public bool _PostSplitMergeUOM(ref EpicEnv oEpicEnv, ref SplitMergeParam oSplitMergeParam, ref SplitMergeHead oSplitMergeHead, ref IList<SplitMergeDetail> oSplitMergeDetailLst, out string strMessage, string strUID = "", string strPass = "")
        {

            bool IsEpicLoginOK = false;
            bool IsEpicTrxSuccess = false;

            string scheme = "http";
            string strEpicSessionModSvc = oEpicEnv.Env_AppEpicor + "/" + strSessionModSvcPath;
            string strEpicSplitMergeUOMSvc = oEpicEnv.Env_AppEpicor + "/" + strSplitMergeUOMSvcPath;
            string strEpicUser = (oEpicEnv.Env_UsedEpicLogin ? strUID : oEpicEnv.Env_AppUserId);
            string strEpicPwd = (oEpicEnv.Env_UsedEpicLogin ? strPass : oEpicEnv.Env_AppPassKey);

            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => { return true; };
            EndpointBindingType bindingType = EndpointBindingType.BasicHttp;

            if (bindingType == EndpointBindingType.BasicHttp)
            {
                scheme = "https";
            }

            UriBuilder builder = new UriBuilder(scheme, oEpicEnv.Env_AppServer);

            builder.Path = strEpicSessionModSvc;
            SessionModSvcContractClient sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            builder.Path = strEpicSplitMergeUOMSvc;
            SplitMergeUOMSvcContractClient splitMergeUOMClient = GetClient<SplitMergeUOMSvcContractClient, SplitMergeUOMSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            Guid sessionId = Guid.Empty;

            try
            {
                sessionId = sessionModClient.Login();

                string companyName;
                string plantID;
                string plantName;
                string workstationID;
                string workstationDescription;
                string employeeID;
                string countryGroupCode;
                string countryCode;

                builder.Path = strEpicSessionModSvc;
                sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

                sessionModClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                sessionModClient.SetCompany(oSplitMergeParam.Company, out companyName, out plantID, out plantName, out workstationID, out workstationDescription,
                                            out employeeID, out countryGroupCode, out countryCode);

                if (oSplitMergeParam.CurrentPlant != "")
                {
                    sessionModClient.SetPlant(oSplitMergeParam.CurrentPlant);
                }
                IsEpicLoginOK = true;
                strMessage = "";

            }
            catch (Exception ex)
            {
                IsEpicLoginOK = false;
                strMessage = ex.Message.ToString();
            }


            if (IsEpicLoginOK == true)
            {
                splitMergeUOMClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                var ts = new SplitMergeUOMTableset();
                try
                {
                    /*ts = splitMergeUOMClient.GetSplitMergeHeadData(oSplitMergeParam.PartNum);
                    splitMergeUOMClient.OnChangeUOM(oSplitMergeParam.PartNum, oSplitMergeParam.UOM, ref ts);
                    splitMergeUOMClient.OnChangeQuantity(oSplitMergeParam.PartNum, oSplitMergeParam.Qty, ref ts);
                    splitMergeUOMClient.OnChangeProcType(oSplitMergeParam.PartNum, oSplitMergeParam.ProcessType, ref ts);
                    //splitMergeUOMClient.OnChangeUOM(oSplitMergeParam.PartNum, oSplitMergeParam.UOM, ref ts);*/

                    ts = splitMergeUOMClient.GetSplitMergeHeadData(oSplitMergeParam.PartNum);
                    splitMergeUOMClient.OnChangeWarehouse(oSplitMergeParam.PartNum, oSplitMergeParam.WarehouseCode, ref ts);
                    splitMergeUOMClient.OnChangeBinNum(oSplitMergeParam.PartNum, oSplitMergeParam.BinNum, ref ts);
                    splitMergeUOMClient.OnChangeLotNum(oSplitMergeParam.PartNum, oSplitMergeParam.LotNum, ref ts);
                    splitMergeUOMClient.OnChangeUOM(oSplitMergeParam.PartNum, oSplitMergeParam.UOM, ref ts);
                    splitMergeUOMClient.OnChangeQuantity(oSplitMergeParam.PartNum, oSplitMergeParam.Qty, ref ts);
                    splitMergeUOMClient.OnChangeProcType(oSplitMergeParam.PartNum, oSplitMergeParam.ProcessType, ref ts);

                    SMHdrRow hd = ts.SMHdr[0];

                    hd.Quantity = oSplitMergeParam.Qty;

                    foreach (SMDtlRow row in ts.SMDtl)
                    {
                        foreach (SplitMergeDetail x in oSplitMergeDetailLst)
                        {
                            if (x.ConvFact == row.ConvFactor && x.ConvFactUOM == row.ConvFactorUOM)
                            {
                                row.Quantity = x.Qty;
                            }
                        }
                    }

                    splitMergeUOMClient.ProcessType(oSplitMergeParam.PartNum, ref ts);

                    IsEpicTrxSuccess = true;
                }
                catch (Exception ex)
                {
                    IsEpicTrxSuccess = false;
                    strMessage = ex.Message.ToString();
                }

                if (sessionId != Guid.Empty)
                {
                    IsEpicLoginOK = true;
                    sessionModClient.Logout();
                }
                else
                {
                    IsEpicLoginOK = false;
                    strMessage = "Unable to connect into Epicor";
                }

            }
            return (IsEpicLoginOK && IsEpicTrxSuccess);
        }

        public bool _EpicIssueMiscMaterial(ref EpicEnv oEpicEnv, ref MiscMaterial oMiscMaterial, out string strMessage, string strUID = "", string strPass = "")
        {
            bool IsEpicLoginOK = false;
            bool IsEpicTrxSuccess = false;

            string partTran = "";

            string scheme = "http";
            string strEpicSessionModSvc = oEpicEnv.Env_AppEpicor + "/" + strSessionModSvcPath;
            string strEpicReturnMaterialSvc = oEpicEnv.Env_AppEpicor + "/" + strIssueReturnSvcPath;
            string strEpicUser = (oEpicEnv.Env_UsedEpicLogin ? strUID : oEpicEnv.Env_AppUserId);
            string strEpicPwd = (oEpicEnv.Env_UsedEpicLogin ? strPass : oEpicEnv.Env_AppPassKey);

            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => { return true; };
            EndpointBindingType bindingType = EndpointBindingType.BasicHttp;

            if (bindingType == EndpointBindingType.BasicHttp)
            {
                scheme = "https";
            }

            UriBuilder builder = new UriBuilder(scheme, oEpicEnv.Env_AppServer);

            builder.Path = strEpicSessionModSvc;
            SessionModSvcContractClient sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            builder.Path = strEpicReturnMaterialSvc;
            IssueReturnSvcContractClient issueReturnClient = GetClient<IssueReturnSvcContractClient, IssueReturnSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            Guid sessionId = Guid.Empty;

            try
            {
                sessionId = sessionModClient.Login();

                string companyName;
                string plantID;
                string plantName;
                string workstationID;
                string workstationDescription;
                string employeeID;
                string countryGroupCode;
                string countryCode;

                builder.Path = strEpicSessionModSvc;
                sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

                sessionModClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                sessionModClient.SetCompany(oMiscMaterial.Company, out companyName, out plantID, out plantName, out workstationID, out workstationDescription,
                            out employeeID, out countryGroupCode, out countryCode);

                if (oMiscMaterial.CurrentPlant != "")
                {
                    sessionModClient.SetPlant(oMiscMaterial.CurrentPlant);
                }
                IsEpicLoginOK = true;
                strMessage = "";

            }
            catch (Exception ex)
            {
                IsEpicLoginOK = false;
                strMessage = ex.Message.ToString();
            }


            if (IsEpicLoginOK == true)
            {
                issueReturnClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                var ts = new IssueReturnTableset();

                Guid g = new Guid();

                issueReturnClient.GetNewIssueReturn("STK-UKN", g,string.Empty, ref ts );

                var newRow = ts.IssueReturn[0]; // .Where(n => n.RowMod.Equals("A", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

                if (newRow != null)
                    {
                        newRow.PartNum = oMiscMaterial.PartNum.ToUpper();
                        newRow.DimCode = oMiscMaterial.IUM.ToUpper();
                        newRow.UM = oMiscMaterial.IUM.ToUpper();

                        newRow.LotNum = (oMiscMaterial.LotNum == null ? string.Empty : oMiscMaterial.LotNum.ToUpper());
                        newRow.ReasonCode = (oMiscMaterial.ReasonCode == null ? string.Empty : oMiscMaterial.ReasonCode)  ;
                        newRow.FromWarehouseCode = oMiscMaterial.WarehouseCode.ToUpper();
                        newRow.FromBinNum = oMiscMaterial.BinNum.ToUpper();
                        newRow.TranReference = (oMiscMaterial.Reference == null ? string.Empty : oMiscMaterial.Reference) ;

                        newRow.DimConvFactor = 1;
                        newRow.MtlQueueRowId = g;

                    try
                        {
                            issueReturnClient.OnChangeTranQty(oMiscMaterial.TranQty, ref ts);
                            issueReturnClient.PrePerformMaterialMovement(ref ts);
                            issueReturnClient.NegativeInventoryTest(oMiscMaterial.PartNum, oMiscMaterial.WarehouseCode, oMiscMaterial.BinNum, oMiscMaterial.LotNum, string.Empty, oMiscMaterial.IUM, 1, oMiscMaterial.TranQty, out partTran);

                            issueReturnClient.PerformMaterialMovement(true, ref ts, out partTran );

                            oMiscMaterial.TranNum = partTran;
                        }
                        catch (Exception ex)
                        {
                            IsEpicTrxSuccess = false;
                            strMessage = ex.Message.ToString();
                        }

                        IsEpicTrxSuccess = (strMessage.Length > 0 ? false : true);

                }
                else
                    {
                        strMessage = "Unable to perform Issue Misc. Material. ";
                        IsEpicTrxSuccess = false;
                    }

            }
            else
            {
                strMessage = "Unable to connect into Epicor";
                IsEpicTrxSuccess = false;
            }

            if (sessionId != Guid.Empty)
            {
                IsEpicLoginOK = true;
                sessionModClient.Logout();
            }
            else
            {
                IsEpicLoginOK = false;
                strMessage = "Unable to connect into Epicor";
            }

            return (IsEpicLoginOK && IsEpicTrxSuccess);

        }

        public bool _EpicReturnMiscMaterial(ref EpicEnv oEpicEnv, ref MiscMaterial oMiscMaterial, out string strMessage, string strUID = "", string strPass = "")
        {
            bool IsEpicLoginOK = false;
            bool IsEpicTrxSuccess = false;

            string partTran = "";

            string scheme = "http";
            string strEpicSessionModSvc = oEpicEnv.Env_AppEpicor + "/" + strSessionModSvcPath;
            string strEpicReturnMaterialSvc = oEpicEnv.Env_AppEpicor + "/" + strIssueReturnSvcPath;
            string strEpicUser = (oEpicEnv.Env_UsedEpicLogin ? strUID : oEpicEnv.Env_AppUserId);
            string strEpicPwd = (oEpicEnv.Env_UsedEpicLogin ? strPass : oEpicEnv.Env_AppPassKey);

            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => { return true; };
            EndpointBindingType bindingType = EndpointBindingType.BasicHttp;

            if (bindingType == EndpointBindingType.BasicHttp)
            {
                scheme = "https";
            }

            UriBuilder builder = new UriBuilder(scheme, oEpicEnv.Env_AppServer);

            builder.Path = strEpicSessionModSvc;
            SessionModSvcContractClient sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            builder.Path = strEpicReturnMaterialSvc;
            IssueReturnSvcContractClient issueReturnClient = GetClient<IssueReturnSvcContractClient, IssueReturnSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            Guid sessionId = Guid.Empty;

            try
            {
                sessionId = sessionModClient.Login();

                string companyName;
                string plantID;
                string plantName;
                string workstationID;
                string workstationDescription;
                string employeeID;
                string countryGroupCode;
                string countryCode;

                builder.Path = strEpicSessionModSvc;
                sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

                sessionModClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                sessionModClient.SetCompany(oMiscMaterial.Company, out companyName, out plantID, out plantName, out workstationID, out workstationDescription,
                            out employeeID, out countryGroupCode, out countryCode);


                if (oMiscMaterial.CurrentPlant != "")
                {
                    sessionModClient.SetPlant(oMiscMaterial.CurrentPlant);
                }

                IsEpicLoginOK = true;
                strMessage = "";

            }
            catch (Exception ex)
            {
                IsEpicLoginOK = false;
                strMessage = ex.Message.ToString();
            }


            if (IsEpicLoginOK == true)
            {
                issueReturnClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                var ts = new IssueReturnTableset();

                Guid g = new Guid();

                issueReturnClient.GetNewIssueReturn("UKN-STK", g, string.Empty, ref ts);

                var newRow = ts.IssueReturn[0]; // .Where(n => n.RowMod.Equals("A", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

                if (newRow != null)
                {
                    newRow.PartNum = oMiscMaterial.PartNum.ToUpper();
                    newRow.DimCode = oMiscMaterial.IUM.ToUpper();
                    newRow.UM = oMiscMaterial.IUM.ToUpper();

                    newRow.LotNum =  (oMiscMaterial.LotNum == null ? string.Empty : oMiscMaterial.LotNum.ToUpper());
                    newRow.ReasonCode = (oMiscMaterial.ReasonCode == null ? string.Empty : oMiscMaterial.ReasonCode) ;
                    newRow.ToWarehouseCode = oMiscMaterial.WarehouseCode.ToUpper();
                    newRow.ToBinNum = oMiscMaterial.BinNum.ToUpper();

                    newRow.TranReference = (oMiscMaterial.Reference == null ? string.Empty : oMiscMaterial.Reference) ;
                    newRow.DimConvFactor = 1;
                    newRow.MtlQueueRowId = g;


                    try
                    {
                        issueReturnClient.OnChangeTranQty(oMiscMaterial.TranQty, ref ts);
                        issueReturnClient.PrePerformMaterialMovement(ref ts);
                        //issueReturnClient.NegativeInventoryTest(oMiscMaterial.PartNum, oMiscMaterial.WarehouseCode, oMiscMaterial.BinNum, oMiscMaterial.LotNum, string.Empty, oMiscMaterial.IUM, 1, oMiscMaterial.TranQty, out partTran);

                        issueReturnClient.PerformMaterialMovement(true, ref ts, out partTran);

                        oMiscMaterial.TranNum = partTran;
                    }
                    catch (Exception ex)
                    {
                        IsEpicTrxSuccess = false;
                        strMessage = ex.Message.ToString();
                    }

                    IsEpicTrxSuccess = (strMessage.Length > 0 ? false : true);

                }
                else
                {
                    strMessage = "Unable to perform Return Misc. Material. ";
                    IsEpicTrxSuccess = false;
                }

            }
            else
            {
                strMessage = "Unable to connect into Epicor";
                IsEpicTrxSuccess = false;
            }

            if (sessionId != Guid.Empty)
            {
                IsEpicLoginOK = true;
                sessionModClient.Logout();
            }
            else
            {
                IsEpicLoginOK = false;
                strMessage = "Unable to connect into Epicor";
            }

            return (IsEpicLoginOK && IsEpicTrxSuccess);

        }

        public bool _EpicQtyAdjustment(ref EpicEnv oEpicEnv, ref QtyAdjustment oQtyAdjustment, out string strMessage, string strUID = "", string strPass = "")
        {
            bool IsEpicLoginOK = false;
            bool IsEpicTrxSuccess = false;

            string scheme = "http";
            string strEpicSessionModSvc = oEpicEnv.Env_AppEpicor + "/" + strSessionModSvcPath;
            string strEpicInventoryQtyAdjSvc = oEpicEnv.Env_AppEpicor + "/" + strInventoryQtyAdjSvcPath;
            string strEpicUser = (oEpicEnv.Env_UsedEpicLogin ? strUID : oEpicEnv.Env_AppUserId);
            string strEpicPwd = (oEpicEnv.Env_UsedEpicLogin ? strPass : oEpicEnv.Env_AppPassKey);

            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => { return true; };
            EndpointBindingType bindingType = EndpointBindingType.BasicHttp;

            if (bindingType == EndpointBindingType.BasicHttp)
            {
                scheme = "https";
            }

            UriBuilder builder = new UriBuilder(scheme, oEpicEnv.Env_AppServer);

            builder.Path = strEpicSessionModSvc;
            SessionModSvcContractClient sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            builder.Path = strEpicInventoryQtyAdjSvc;
            InventoryQtyAdjSvcContractClient InventoryAdjClient = GetClient<InventoryQtyAdjSvcContractClient, InventoryQtyAdjSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            Guid sessionId = Guid.Empty;

            try
            {
                sessionId = sessionModClient.Login();

                string companyName;
                string plantID;
                string plantName;
                string workstationID;
                string workstationDescription;
                string employeeID;
                string countryGroupCode;
                string countryCode;

                builder.Path = strEpicSessionModSvc;
                sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

                sessionModClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                sessionModClient.SetCompany(oQtyAdjustment.Company, out companyName, out plantID, out plantName, out workstationID, out workstationDescription,
                            out employeeID, out countryGroupCode, out countryCode);

                if (oQtyAdjustment.CurrentPlant != "")
                {
                    sessionModClient.SetPlant(oQtyAdjustment.CurrentPlant);
                }

                IsEpicLoginOK = true;
                strMessage = "";

            }
            catch (Exception ex)
            {
                IsEpicLoginOK = false;
                strMessage = ex.Message.ToString();
            }


            if (IsEpicLoginOK == true)
            {
                string strOutMessage;
                string strBin;
                string strPart;
                string strUnitOfMeasure;
                string strTranNum;

                string question;
                bool multipleMatches = false;

                InventoryAdjClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));
                //var ts = new InventoryQtyAdjTableset();

                strPart = oQtyAdjustment.PartNum;
                strUnitOfMeasure = oQtyAdjustment.IUM;

                var ts = InventoryAdjClient.GetInventoryQtyAdj(oQtyAdjustment.PartNum,string.Empty);


                var newRow = ts.InventoryQtyAdj[0]; // .Where(n => n.RowMod.Equals("A", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

                if (newRow != null)
                {
                    newRow.Company = oQtyAdjustment.Company.ToUpper();
                    newRow.TransDate = DateTime.Now;
                    newRow.PartNum = oQtyAdjustment.PartNum.ToUpper();
                    newRow.AdjustQuantity = oQtyAdjustment.TranQty;
                    newRow.UnitOfMeasure = oQtyAdjustment.IUM.ToUpper();

                    newRow.LotNum = (oQtyAdjustment.LotNum == null ? string.Empty : oQtyAdjustment.LotNum.ToUpper());
                    /* 2021-04-20 = bug fixes on the qty adjustment if null then set to empty string */
                    newRow.ReasonCode = (oQtyAdjustment.ReasonCode == null ? string.Empty : oQtyAdjustment.ReasonCode);
                    newRow.WareHseCode = oQtyAdjustment.WarehouseCode.ToUpper();
                    newRow.BinNum = oQtyAdjustment.BinNum.ToUpper();
                    /* 2021-04-20 = bug fixes on the qty adjustment if null then set to empty string */
                    newRow.Reference = (oQtyAdjustment.Reference == null ? string.Empty : oQtyAdjustment.Reference);

                    newRow.RowMod = "U";

                    try
                    {
                       
                        InventoryAdjClient.GetPartXRefInfo(ref strPart, null, null, ref strUnitOfMeasure, out question, out multipleMatches);
                        InventoryAdjClient.GetInventoryQtyAdjBrw(oQtyAdjustment.PartNum,0, oQtyAdjustment.WarehouseCode, out strBin);
                        InventoryAdjClient.NegativeInventoryTest(oQtyAdjustment.PartNum, oQtyAdjustment.WarehouseCode, oQtyAdjustment.BinNum, 
                                                                 oQtyAdjustment.LotNum, 0,"", oQtyAdjustment.IUM, 1, oQtyAdjustment.TranQty, out strOutMessage);



                        InventoryAdjClient.PreSetInventoryQtyAdj(ref ts);


                        strTranNum =InventoryAdjClient.SetInventoryQtyAdj(ref ts);

                        oQtyAdjustment.TranNum = strTranNum;

                        IsEpicTrxSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        IsEpicTrxSuccess = false;
                        strMessage = ex.Message.ToString();
                    }

                    //IsEpicTrxSuccess = (strMessage.Length > 0 ? false : true);

                }
                else
                {
                    strMessage = "Unable to perform Quantity Adjustment ";
                    IsEpicTrxSuccess = false;
                }

            }
            else
            {
                strMessage = "Unable to connect into Epicor";
                IsEpicTrxSuccess = false;
            }

            if (sessionId != Guid.Empty)
            {
                IsEpicLoginOK = true;
                sessionModClient.Logout();
            }
            else
            {
                IsEpicLoginOK = false;
                strMessage = "Unable to connect into Epicor";
            }

            return (IsEpicLoginOK && IsEpicTrxSuccess);

        }

        public bool _EpicEmployeeClockIn(ref EpicEnv oEpicEnv, ref EmpClock oEmpClock, out string strMessage, string strUID = "", string strPass = "")
        {
            bool IsEpicLoginOK = false;
            bool IsEpicTrxSuccess = false;

            string scheme = "http";
            string strEpicSessionModSvc = oEpicEnv.Env_AppEpicor + "/" + strSessionModSvcPath;
            string strEpicEmpBasicSvc = oEpicEnv.Env_AppEpicor + "/" + strEmpBasicSvcPath;
            string strEpicUser = (oEpicEnv.Env_UsedEpicLogin ? strUID : oEpicEnv.Env_AppUserId);
            string strEpicPwd = (oEpicEnv.Env_UsedEpicLogin ? strPass : oEpicEnv.Env_AppPassKey);

            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => { return true; };
            EndpointBindingType bindingType = EndpointBindingType.BasicHttp;

            if (bindingType == EndpointBindingType.BasicHttp)
            {
                scheme = "https";
            }

            UriBuilder builder = new UriBuilder(scheme, oEpicEnv.Env_AppServer);

            builder.Path = strEpicSessionModSvc;
            SessionModSvcContractClient sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            builder.Path = strEpicEmpBasicSvc;
            EmpBasicSvcContractClient EmpBasicClient = GetClient<EmpBasicSvcContractClient, EmpBasicSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            Guid sessionId = Guid.Empty;

            try
            {
                sessionId = sessionModClient.Login();

                string companyName;
                string plantID;
                string plantName;
                string workstationID;
                string workstationDescription;
                string employeeID;
                string countryGroupCode;
                string countryCode;

                builder.Path = strEpicSessionModSvc;
                sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

                sessionModClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                sessionModClient.SetCompany(oEmpClock.Company, out companyName, out plantID, out plantName, out workstationID, out workstationDescription,
                            out employeeID, out countryGroupCode, out countryCode);

                IsEpicLoginOK = true;
                strMessage = "";

            }
            catch (Exception ex)
            {
                IsEpicLoginOK = false;
                strMessage = ex.Message.ToString();
            }

            if (IsEpicLoginOK == true)
            {
                string strErr;

                EmpBasicClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));
                //var ts = new EmpBasicTableset();

                strErr = EmpBasicClient.CheckShift(oEmpClock.Employee, oEmpClock.Shift);

                if (strErr.Length == 0)
                {
                    strErr = EmpBasicClient.ClockIn(oEmpClock.Employee, oEmpClock.Shift);

                    if (strErr.Length == 0)
                    {
                        IsEpicTrxSuccess = true;
                    }
                    else
                    {
                        DateTime tempDate;

                        if (DateTime.TryParse(strErr, out tempDate) == true)
                        {
                            IsEpicTrxSuccess = true;
                        }
                        else
                        {
                            strMessage = strErr;
                            IsEpicTrxSuccess = false;
                        }
                    }

                }
                else
                {
                    strMessage = strErr;
                    IsEpicTrxSuccess = false;
                }

            }
            else
            {
                strMessage = "Unable to connect into Epicor";
                IsEpicTrxSuccess = false;
            }


            if (sessionId != Guid.Empty)
            {
                IsEpicLoginOK = true;
                sessionModClient.Logout();
            }
            else
            {
                IsEpicLoginOK = false;
                strMessage = "Unable to connect into Epicor";
            }

            return (IsEpicLoginOK && IsEpicTrxSuccess);

        }

        public bool _EpicEmployeeClockOut(ref EpicEnv oEpicEnv, ref EmpClock oEmpClock, out string strMessage, string strUID = "", string strPass = "")
        {
            bool IsEpicLoginOK = false;
            bool IsEpicTrxSuccess = false;

            string scheme = "http";
            string strEpicSessionModSvc = oEpicEnv.Env_AppEpicor + "/" + strSessionModSvcPath;
            string strEpicEmpBasicSvc = oEpicEnv.Env_AppEpicor + "/" + strEmpBasicSvcPath;
            string strEpicUser = (oEpicEnv.Env_UsedEpicLogin ? strUID : oEpicEnv.Env_AppUserId);
            string strEpicPwd = (oEpicEnv.Env_UsedEpicLogin ? strPass : oEpicEnv.Env_AppPassKey);

            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => { return true; };
            EndpointBindingType bindingType = EndpointBindingType.BasicHttp;

            if (bindingType == EndpointBindingType.BasicHttp)
            {
                scheme = "https";
            }

            UriBuilder builder = new UriBuilder(scheme, oEpicEnv.Env_AppServer);

            builder.Path = strEpicSessionModSvc;
            SessionModSvcContractClient sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            builder.Path = strEpicEmpBasicSvc;
            EmpBasicSvcContractClient EmpBasicClient = GetClient<EmpBasicSvcContractClient, EmpBasicSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            Guid sessionId = Guid.Empty;

            try
            {
                sessionId = sessionModClient.Login();

                string companyName;
                string plantID;
                string plantName;
                string workstationID;
                string workstationDescription;
                string employeeID;
                string countryGroupCode;
                string countryCode;

                builder.Path = strEpicSessionModSvc;
                sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

                sessionModClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                sessionModClient.SetCompany(oEmpClock.Company, out companyName, out plantID, out plantName, out workstationID, out workstationDescription,
                            out employeeID, out countryGroupCode, out countryCode);

                IsEpicLoginOK = true;
                strMessage = "";

            }
            catch (Exception ex)
            {
                IsEpicLoginOK = false;
                strMessage = ex.Message.ToString();
            }

            if (IsEpicLoginOK == true)
            {
                EmpBasicClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));
                //var ts = new EmpBasicTableset();
                string e = "";

                e = oEmpClock.Employee;

                try
                {
                    EmpBasicClient.ClockOut(ref e);
                    IsEpicTrxSuccess = true;
                }
                catch (Exception ex)
                {
                    IsEpicTrxSuccess = false;
                    strMessage = ex.Message.ToString();
                }
            }
            else
            {
                strMessage = "Unable to connect into Epicor";
                IsEpicTrxSuccess = false;
            }


            if (sessionId != Guid.Empty)
            {
                IsEpicLoginOK = true;
                sessionModClient.Logout();
            }
            else
            {
                IsEpicLoginOK = false;
                strMessage = "Unable to connect into Epicor";
            }

            return (IsEpicLoginOK && IsEpicTrxSuccess);

        }

        public bool _EpicEmployeeStartActivity(ref EpicEnv oEpicEnv, ref WorkQueue oWorkQueue, out string strMessage, string strUID = "", string strPass = "")
        {
            bool IsEpicLoginOK = false;
            bool IsEpicTrxSuccess = false;

            string scheme = "http";
            string strEpicSessionModSvc = oEpicEnv.Env_AppEpicor + "/" + strSessionModSvcPath;
            string strEpicLaborSvc = oEpicEnv.Env_AppEpicor + "/" + strLaborSvcPath;
            string strEpicUser = (oEpicEnv.Env_UsedEpicLogin ? strUID : oEpicEnv.Env_AppUserId);
            string strEpicPwd = (oEpicEnv.Env_UsedEpicLogin ? strPass : oEpicEnv.Env_AppPassKey);

            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => { return true; };
            EndpointBindingType bindingType = EndpointBindingType.BasicHttp;

            if (bindingType == EndpointBindingType.BasicHttp)
            {
                scheme = "https";
            }

            UriBuilder builder = new UriBuilder(scheme, oEpicEnv.Env_AppServer);

            builder.Path = strEpicSessionModSvc;
            SessionModSvcContractClient sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            builder.Path = strEpicLaborSvc;
            LaborSvcContractClient LaborClient = GetClient<LaborSvcContractClient, LaborSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            Guid sessionId = Guid.Empty;

            try
            {
                sessionId = sessionModClient.Login();

                string companyName;
                string plantID;
                string plantName;
                string workstationID;
                string workstationDescription;
                string employeeID;
                string countryGroupCode;
                string countryCode;

                builder.Path = strEpicSessionModSvc;
                sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

                sessionModClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                sessionModClient.SetCompany(oWorkQueue.Company, out companyName, out plantID, out plantName, out workstationID, out workstationDescription,
                            out employeeID, out countryGroupCode, out countryCode);

                if (oWorkQueue.CurPlant != "")
                {
                    sessionModClient.SetPlant(oWorkQueue.CurPlant);
                }

                IsEpicLoginOK = true;
                strMessage = "";

            }
            catch (Exception ex)
            {
                IsEpicLoginOK = false;
                strMessage = ex.Message.ToString();
            }

            if (IsEpicLoginOK == true)
            {
                LaborClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                var ts = LaborClient.GetByID(oWorkQueue.LaborHedSeq);

                if (ts != null)
                {

                    try {
                       
                        LaborClient.StartActivity(oWorkQueue.LaborHedSeq, "P", ref ts);
                        LaborClient.DefaultJobNum(ref ts, oWorkQueue.JobNum);
                        //LaborClient.LaborRateCalc(ref ts);
                        LaborClient.DefaultAssemblySeq(ref ts, oWorkQueue.AssemblySeq);
                        LaborClient.DefaultOprSeq(ref ts, oWorkQueue.OprSeq);

                        LaborClient.LaborRateCalc(ref ts);
                        LaborClient.CheckWarnings(ref ts);
                        LaborClient.CheckEmployeeActivity(oWorkQueue.EmployeeNum
                                                        , oWorkQueue.LaborHedSeq
                                                        , oWorkQueue.JobNum
                                                        , oWorkQueue.AssemblySeq
                                                        , oWorkQueue.OprSeq
                                                        , oWorkQueue.ResourceID);

                        LaborClient.CheckFirstArticleWarning(ref ts);
                        LaborClient.Update(ref ts);

                        IsEpicTrxSuccess = true;

                    }
                    catch (Exception ex)
                    {
                        strMessage = ex.Message.ToString();
                        IsEpicTrxSuccess = false;
                        
                    }

                }

            }
            else
            {
                strMessage = "Unable to perform start activity";
                IsEpicTrxSuccess = false;
            }

            if (sessionId != Guid.Empty)
            {
                IsEpicLoginOK = true;
                sessionModClient.Logout();
            }
            else
            {
                IsEpicLoginOK = false;
                strMessage = "Unable to connect into Epicor";
            }

            return (IsEpicLoginOK && IsEpicTrxSuccess);

        }

        public bool _EpicEmployeeEndActivity(ref EpicEnv oEpicEnv, ref WorkQueue oWorkQueue, out string strMessage, string strUID = "", string strPass = "")
        {
            bool IsEpicLoginOK = false;
            bool IsEpicTrxSuccess = false;

            string scheme = "http";
            string strEpicSessionModSvc = oEpicEnv.Env_AppEpicor + "/" + strSessionModSvcPath;
            string strEpicLaborSvc = oEpicEnv.Env_AppEpicor + "/" + strLaborSvcPath;
            string strEpicUser = (oEpicEnv.Env_UsedEpicLogin ? strUID : oEpicEnv.Env_AppUserId);
            string strEpicPwd = (oEpicEnv.Env_UsedEpicLogin ? strPass : oEpicEnv.Env_AppPassKey);

            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => { return true; };
            EndpointBindingType bindingType = EndpointBindingType.BasicHttp;

            if (bindingType == EndpointBindingType.BasicHttp)
            {
                scheme = "https";
            }

            UriBuilder builder = new UriBuilder(scheme, oEpicEnv.Env_AppServer);

            builder.Path = strEpicSessionModSvc;
            SessionModSvcContractClient sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            builder.Path = strEpicLaborSvc;
            LaborSvcContractClient LaborClient = GetClient<LaborSvcContractClient, LaborSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            Guid sessionId = Guid.Empty;

            try
            {
                sessionId = sessionModClient.Login();

                string companyName;
                string plantID;
                string plantName;
                string workstationID;
                string workstationDescription;
                string employeeID;
                string countryGroupCode;
                string countryCode;

                builder.Path = strEpicSessionModSvc;
                sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

                sessionModClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                sessionModClient.SetCompany(oWorkQueue.Company, out companyName, out plantID, out plantName, out workstationID, out workstationDescription,
                            out employeeID, out countryGroupCode, out countryCode);

                IsEpicLoginOK = true;
                strMessage = "";

            }
            catch (Exception ex)
            {
                IsEpicLoginOK = false;
                strMessage = ex.Message.ToString();
            }

            if (IsEpicLoginOK == true)
            {
                LaborClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                var ts = LaborClient.GetDetail(oWorkQueue.LaborHedSeq, oWorkQueue.LaborDtlSeq);

                if (ts != null)
                {
                    try {

                        var newRow = ts.LaborDtl[0];

                        if (newRow != null)
                        {
                            newRow.EndActivity = true;
                            newRow.RowMod = "U";

                            LaborClient.EndActivity(ref ts);
                            LaborClient.DefaultLaborQty(ref ts, oWorkQueue.TranQty);
                            LaborClient.CheckWarnings(ref ts);
                            LaborClient.Update(ref ts);

                            IsEpicTrxSuccess = true;
                        }
                        else
                        {
                            strMessage = "Unable to perform End Activity ";
                            IsEpicTrxSuccess = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        IsEpicTrxSuccess = false;
                        strMessage = ex.Message.ToString();
                    }
                }

            }
            else
            {
                strMessage = "Unable to connect into Epicor";
                IsEpicTrxSuccess = false;
            }


            if (sessionId != Guid.Empty)
            {
                IsEpicLoginOK = true;
                sessionModClient.Logout();
            }
            else
            {
                IsEpicLoginOK = false;
                strMessage = "Unable to connect into Epicor";
            }

            return (IsEpicLoginOK && IsEpicTrxSuccess);

        }

        public bool _EpicReportQty(ref EpicEnv oEpicEnv, ref WorkQueue oWorkQueue, out string strMessage, string strUID = "", string strPass = "")
        {
            bool IsEpicLoginOK = false;
            bool IsEpicTrxSuccess = false;

            string scheme = "http";
            string strEpicSessionModSvc = oEpicEnv.Env_AppEpicor + "/" + strSessionModSvcPath;
            string strEpicReportQtySvc = oEpicEnv.Env_AppEpicor + "/" + strReportQtySvcPath;
            string strEpicUser = (oEpicEnv.Env_UsedEpicLogin ? strUID : oEpicEnv.Env_AppUserId);
            string strEpicPwd = (oEpicEnv.Env_UsedEpicLogin ? strPass : oEpicEnv.Env_AppPassKey);

            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => { return true; };
            EndpointBindingType bindingType = EndpointBindingType.BasicHttp;

            if (bindingType == EndpointBindingType.BasicHttp)
            {
                scheme = "https";
            }

            UriBuilder builder = new UriBuilder(scheme, oEpicEnv.Env_AppServer);

            builder.Path = strEpicSessionModSvc;
            SessionModSvcContractClient sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            builder.Path = strEpicReportQtySvc;
            ReportQtySvcContractClient ReportQtyClient = GetClient<ReportQtySvcContractClient, ReportQtySvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            Guid sessionId = Guid.Empty;

            try
            {
                sessionId = sessionModClient.Login();

                string companyName;
                string plantID;
                string plantName;
                string workstationID;
                string workstationDescription;
                string employeeID;
                string countryGroupCode;
                string countryCode;

                builder.Path = strEpicSessionModSvc;
                sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

                sessionModClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                sessionModClient.SetCompany(oWorkQueue.Company, out companyName, out plantID, out plantName, out workstationID, out workstationDescription,
                            out employeeID, out countryGroupCode, out countryCode);

                IsEpicLoginOK = true;
                strMessage = "";

            }
            catch (Exception ex)
            {
                IsEpicLoginOK = false;
                strMessage = ex.Message.ToString();
            }

            if (IsEpicLoginOK == true)
            {
                ReportQtyClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                var ts = ReportQtyClient.GetNewReportQty(oWorkQueue.EmployeeNum,oWorkQueue.LaborHedSeq, oWorkQueue.LaborDtlSeq);

                if (ts != null)
                {
                    try
                    {

                        var newRow = ts.ReportQtyPart[0];

                        if (newRow != null)
                        {
                            //newRow.TotalQty = oWorkQueue.TranQty;
                            newRow.CurrentQty = oWorkQueue.TranQty;

                            ReportQtyClient.ReportQuantity(oWorkQueue.LaborHedSeq, oWorkQueue.LaborDtlSeq,ref ts);

                            IsEpicTrxSuccess = true;
                        }
                        else
                        {
                            strMessage = "Unable to perform End Activity ";
                            IsEpicTrxSuccess = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        IsEpicTrxSuccess = false;
                        strMessage = ex.Message.ToString();
                    }



                }
                else
                {
                    strMessage = "Unable to perform Report Quantity ";
                    IsEpicTrxSuccess = false;
                }



                IsEpicTrxSuccess = true;
            }
            else
            {
                strMessage = "Unable to connect into Epicor";
                IsEpicTrxSuccess = false;
            }

            if (sessionId != Guid.Empty)
            {
                IsEpicLoginOK = true;
                sessionModClient.Logout();
            }
            else
            {
                IsEpicLoginOK = false;
                strMessage = "Unable to connect into Epicor";
            }

            return (IsEpicLoginOK && IsEpicTrxSuccess);

        }

        public bool _EpicReturnAssembly(ref EpicEnv oEpicEnv, ref ReturnAssemblyToStock oReturnAssmToStock, out string strMessage, string strUID = "", string strPass = "")
        {

            bool IsEpicLoginOK = false;
            bool IsEpicTrxSuccess = false;

            string scheme = "http";
            string strEpicSessionModSvc = oEpicEnv.Env_AppEpicor + "/" + strSessionModSvcPath;
            string strEpicReturnMaterialSvc = oEpicEnv.Env_AppEpicor + "/" + strIssueReturnSvcPath;
            string partTran = "";
            string strEpicUser = (oEpicEnv.Env_UsedEpicLogin ? strUID : oEpicEnv.Env_AppUserId);
            string strEpicPwd = (oEpicEnv.Env_UsedEpicLogin ? strPass : oEpicEnv.Env_AppPassKey);

            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => { return true; };
            EndpointBindingType bindingType = EndpointBindingType.BasicHttp;

            if (bindingType == EndpointBindingType.BasicHttp)
            {
                scheme = "https";
            }

            UriBuilder builder = new UriBuilder(scheme, oEpicEnv.Env_AppServer);

            builder.Path = strEpicSessionModSvc;
            SessionModSvcContractClient sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            builder.Path = strEpicReturnMaterialSvc;
            IssueReturnSvcContractClient issueReturnClient = GetClient<IssueReturnSvcContractClient, IssueReturnSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            Guid sessionId = Guid.Empty;

            try
            {
                sessionId = sessionModClient.Login();

                string companyName;
                string plantID;
                string plantName;
                string workstationID;
                string workstationDescription;
                string employeeID;
                string countryGroupCode;
                string countryCode;

                builder.Path = strEpicSessionModSvc;
                sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

                sessionModClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                sessionModClient.SetCompany(oReturnAssmToStock.Company, out companyName, out plantID, out plantName, out workstationID, out workstationDescription,
                                            out employeeID, out countryGroupCode, out countryCode);

                if (oReturnAssmToStock.CurrentPlant != "")
                {
                    sessionModClient.SetPlant(oReturnAssmToStock.CurrentPlant);
                }

                IsEpicLoginOK = true;
                strMessage = "";

            }
            catch (Exception ex)
            {
                IsEpicLoginOK = false;
                strMessage = ex.Message.ToString();
            }


            if (IsEpicLoginOK == true)
            {
                issueReturnClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                var ts = new IssueReturnTableset();
                issueReturnClient.GetNewIssueReturnFromJob(oReturnAssmToStock.FromJobNum, oReturnAssmToStock.FromAssemblySeq, "ASM-STK", Guid.NewGuid(), ref ts);

                var newRow = ts.IssueReturn[0]; // .Where(n => n.RowMod.Equals("A", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

                if (newRow != null)
                {
                    newRow.Company = oReturnAssmToStock.Company;

                    newRow.FromJobNum = oReturnAssmToStock.FromJobNum.ToUpper();
                    newRow.FromAssemblySeq = oReturnAssmToStock.FromAssemblySeq;

                    newRow.FromWarehouseCode = oReturnAssmToStock.FromWarehouseCode.ToUpper();
                    newRow.FromBinNum = oReturnAssmToStock.FromBinNum.ToUpper();

                    newRow.Plant = oReturnAssmToStock.Plant;

                    newRow.ToWarehouseCode = oReturnAssmToStock.ToWarehouseCode.ToUpper();
                    newRow.ToBinNum = oReturnAssmToStock.ToBinNum.ToUpper();
                    newRow.PartNum = oReturnAssmToStock.ToPartNum.ToUpper();
                    newRow.PartIUM = oReturnAssmToStock.ToPartIUM.ToUpper();
                    newRow.DimCode = oReturnAssmToStock.ToPartIUM.ToUpper();
                    newRow.DUM = oReturnAssmToStock.ToPartIUM.ToUpper();
                    newRow.UM = oReturnAssmToStock.ToPartIUM.ToUpper();
                    newRow.TranQty = oReturnAssmToStock.TranQty;
                    newRow.LotNum = oReturnAssmToStock.ToLotNum.ToUpper();
                    newRow.RequirementQty = oReturnAssmToStock.TranQty;

                    newRow.TranReference = oReturnAssmToStock.Reference;

                    newRow.RowMod = "U";


                    try
                    {
                        issueReturnClient.OnChangeTranQty(oReturnAssmToStock.TranQty, ref ts);
                        issueReturnClient.PrePerformMaterialMovement(ref ts);
                        issueReturnClient.PerformMaterialMovement(true, ref ts, out partTran);

                        string[] retValues = partTran.Split('~');

                        oReturnAssmToStock.TranNum = retValues[2].ToString();
                    }
                    catch (Exception ex)
                    {
                        IsEpicTrxSuccess = false;
                        strMessage = ex.Message.ToString();
                    }

                    IsEpicTrxSuccess = (strMessage.Length > 0 ? false : true);

                    //strMessage = partTran;
                }
                else
                {
                    strMessage = "Unable to perform return assembly. ";
                    IsEpicTrxSuccess = false;
                }

            }


            if (sessionId != Guid.Empty)
            {
                IsEpicLoginOK = true;
                sessionModClient.Logout();
            }
            else
            {
                IsEpicLoginOK = false;
                strMessage = "Unable to connect into epicor";
            }

            return (IsEpicLoginOK && IsEpicTrxSuccess);
        }

        public bool _EpicIssueAssemblyToJob(ref EpicEnv oEpicEnv, ref IssueAssemblyToJob oIssueAssembly, out string strMessage, string strUID = "", string strPass = "")
        {

            bool IsEpicLoginOK = false;
            bool IsEpicTrxSuccess = false;

            string scheme = "http";
            string strEpicSessionModSvc = oEpicEnv.Env_AppEpicor + "/" + strSessionModSvcPath;
            string strEpicIssueMaterialSvc = oEpicEnv.Env_AppEpicor + "/" + strIssueReturnSvcPath;
            string partTran = "";
            string strEpicUser = (oEpicEnv.Env_UsedEpicLogin ? strUID : oEpicEnv.Env_AppUserId);
            string strEpicPwd = (oEpicEnv.Env_UsedEpicLogin ? strPass : oEpicEnv.Env_AppPassKey);

            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => { return true; };
            EndpointBindingType bindingType = EndpointBindingType.BasicHttp;

            if (bindingType == EndpointBindingType.BasicHttp)
            {
                scheme = "https";
            }

            UriBuilder builder = new UriBuilder(scheme, oEpicEnv.Env_AppServer);

            builder.Path = strEpicSessionModSvc;
            SessionModSvcContractClient sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            builder.Path = strEpicIssueMaterialSvc;
            IssueReturnSvcContractClient issueReturnClient = GetClient<IssueReturnSvcContractClient, IssueReturnSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            Guid sessionId = Guid.Empty;

            try
            {
                sessionId = sessionModClient.Login();

                string companyName;
                string plantID;
                string plantName;
                string workstationID;
                string workstationDescription;
                string employeeID;
                string countryGroupCode;
                string countryCode;


                builder.Path = strEpicSessionModSvc;
                sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

                sessionModClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                sessionModClient.SetCompany(oIssueAssembly.Company, out companyName, out plantID, out plantName, out workstationID, out workstationDescription,
                                            out employeeID, out countryGroupCode, out countryCode);

                if (oIssueAssembly.CurPlant != "")
                {
                    sessionModClient.SetPlant(oIssueAssembly.CurPlant);
                }

                IsEpicLoginOK = true;
                strMessage = "";

            }
            catch (Exception ex)
            {
                IsEpicLoginOK = false;
                strMessage = ex.Message.ToString();
            }


            if (IsEpicLoginOK == true)
            {
                issueReturnClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                var ts = new IssueReturnTableset();
                issueReturnClient.GetNewIssueReturnToJob(oIssueAssembly.ToJobNum, oIssueAssembly.ToJobAssemblySeq, "STK-ASM", Guid.NewGuid(), ref ts);

                var newRow = ts.IssueReturn[0]; //.Where(n => n.RowMod.Equals("A", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

                if (newRow != null)
                {

                    try
                    {

                        newRow.Company = oIssueAssembly.Company.ToUpper();
                        newRow.PartNum = oIssueAssembly.FromPartNum.ToUpper();
                        newRow.PartIUM = oIssueAssembly.FromPartIUM.ToUpper();
                        newRow.DimCode = oIssueAssembly.FromPartIUM.ToUpper();
                        newRow.DUM = oIssueAssembly.FromPartIUM.ToUpper();
                        newRow.UM = oIssueAssembly.FromPartIUM.ToUpper();
                        newRow.TranReference = oIssueAssembly.Reference;

                        //newRow.RowMod = "U";
                        //issueReturnClient.OnChangeUM(oIssueAssembly.FromPartIUM, ref ts);

                        newRow.LotNum = oIssueAssembly.FromLotNum.ToUpper();
                        newRow.TranQty = oIssueAssembly.FromQuantity;

                        //newRow.RowMod = "U";
                        //issueReturnClient.OnChangeTranQty(oIssueAssembly.FromQuantity, ref ts);

                        newRow.FromWarehouseCode = oIssueAssembly.FromWhseCode.ToUpper();
                        newRow.FromBinNum = oIssueAssembly.FromBinNum.ToUpper();

                        newRow.ToWarehouseCode = oIssueAssembly.ToWhseCode.ToUpper();
                        newRow.ToBinNum = oIssueAssembly.ToBinNum.ToUpper();

                        newRow.ToJobNum = oIssueAssembly.ToJobNum;
                        newRow.ToAssemblySeq = oIssueAssembly.ToJobAssemblySeq;
                        newRow.ToAssemblyPartNum = oIssueAssembly.FromPartNum.ToUpper();

                        //newRow.ToJobPartNum = oIssueAssembly.ToJobPartNum;
                        //issueReturnClient.OnChangeToWarehouse(ref ts, "IssueAssembly");
                        newRow.RequirementQty = oIssueAssembly.FromQuantity;
                        newRow.RowMod = "U";


                        //issueReturnClient.OnChangeUM(oIssueAssembly.FromPartIUM, ref ts);
                        issueReturnClient.OnChangeTranQty(oIssueAssembly.FromQuantity, ref ts);
                        //issueReturnClient.OnChangeUM(oIssueAssembly.FromPartIUM, ref ts);
                        //issueReturnClient.OnChangeFromWarehouse(ref ts, "IssueAssembly");
                        //issueReturnClient.OnChangeFromBinNum(false, ref ts);

                        issueReturnClient.PrePerformMaterialMovement(ref ts);
                        issueReturnClient.PerformMaterialMovement(true, ref ts, out partTran);

                        string[] retValues = partTran.Split('~');

                        oIssueAssembly.TranNum = retValues[2].ToString();
                    }
                    catch (Exception ex)
                    {
                        IsEpicTrxSuccess = false;
                        strMessage = ex.Message.ToString();
                    }

                    IsEpicTrxSuccess = (strMessage.Length > 0 ? false : true);

                    //strMessage = partTran;
                }
                else
                {
                    strMessage = "Unable to perform issue assembly to job. ";
                    IsEpicTrxSuccess = false;
                }

            }


            if (sessionId != Guid.Empty)
            {
                IsEpicLoginOK = true;
                sessionModClient.Logout();
            }
            else
            {
                IsEpicLoginOK = false;
                strMessage = "Unable to connect into Epicor";
            }

            return (IsEpicLoginOK && IsEpicTrxSuccess);
        }

        public bool _InsertInToUD18(ref EpicEnv oEpicEnv, ref PickPackEscalate oPickPackEscalate, out string strMessage, string strUID = "", string strPass = "")
        {
            bool IsEpicLoginOK = false;
            bool IsEpicTrxSuccess = false;

            string scheme = "http";
            string strEpicSessionModSvc = oEpicEnv.Env_AppEpicor + "/" + strSessionModSvcPath;
            string strEpicUD18Svc = oEpicEnv.Env_AppEpicor + "/" + strUD18SvcPath;

            string strEpicUser = (oEpicEnv.Env_UsedEpicLogin ? strUID : oEpicEnv.Env_AppUserId);
            string strEpicPwd = (oEpicEnv.Env_UsedEpicLogin ? strPass : oEpicEnv.Env_AppPassKey);

            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => { return true; };
            EndpointBindingType bindingType = EndpointBindingType.BasicHttp;

            if (bindingType == EndpointBindingType.BasicHttp)
            {
                scheme = "https";
            }

            UriBuilder builder = new UriBuilder(scheme, oEpicEnv.Env_AppServer);

            builder.Path = strEpicSessionModSvc;
            SessionModSvcContractClient sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            builder.Path = strEpicUD18Svc;
            UD18SvcContractClient ud18Client = GetClient<UD18SvcContractClient, UD18SvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            Guid sessionId = Guid.Empty;

            try
            {
                sessionId = sessionModClient.Login();

                string companyName;
                string plantID;
                string plantName;
                string workstationID;
                string workstationDescription;
                string employeeID;
                string countryGroupCode;
                string countryCode;


                builder.Path = strEpicSessionModSvc;
                sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

                sessionModClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                sessionModClient.SetCompany(oPickPackEscalate.Company, out companyName, out plantID, out plantName, out workstationID, out workstationDescription,
                                            out employeeID, out countryGroupCode, out countryCode);

                if (oPickPackEscalate.CurrentPlant != "")
                {
                    sessionModClient.SetPlant(oPickPackEscalate.CurrentPlant);
                }

                IsEpicLoginOK = true;
                strMessage = "";

            }
            catch (Exception ex)
            {
                IsEpicLoginOK = false;
                strMessage = ex.Message.ToString();
            }


            if (IsEpicLoginOK == true)
            {
                try {

                    ud18Client.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                    var ts = new UD18Tableset();
                    ud18Client.GetaNewUD18(ref ts);

                    var newRow = ts.UD18[0];// .Where(n => n.RowMod.Equals("A", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

                    if (newRow != null)
                    {
                        newRow.Company = oPickPackEscalate.Company.ToUpper();
                        Guid gkey = Guid.NewGuid();
                        newRow.Key1 = gkey.ToString();
                        
                        newRow.ShortChar01 = strUID;
                        newRow.ShortChar02 = oPickPackEscalate.Picker;
                        newRow.ShortChar03 = oPickPackEscalate.Reason;
                        newRow.ShortChar04 = oPickPackEscalate.CurrentStage;
                        newRow.ShortChar18 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt");

                        newRow.Date01 = DateTime.Now;
                        newRow.Date02 = Convert.ToDateTime(oPickPackEscalate.EscalateDateTime);

                        newRow.Character01 = oPickPackEscalate.PickPackRemarks;
                        
                        newRow.UserDefinedColumns["SD_MQGUID_c"] = Guid.Parse(oPickPackEscalate.MQ_SysRowID);
                        newRow.UserDefinedColumns["SD_UD14GUID_c"] = Guid.Parse(oPickPackEscalate.U14_SysRowID);
                        //newRow["SD_MQGUID_c"] = oPickPackEscalate.MQ_SysRowID;
                        //newRow["SD_UD14GUID_c"] = oPickPackEscalate.U14_SysRowID;
                        ud18Client.Update(ref ts);
                    }


                    SQLServerBO oSQL = new SQLServerBO();
                    string _strSQLCon = oSQL._retSQLConnectionString();
                    _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                    string strSQL;
                    DataSet _dts;
                    string strKey5 = "";

                    strSQL = "select distinct key5 from ice.ud14 ";
                    strSQL += "where key5 in ( select distinct key5 from ice.ud14 where sysrowid = '" + oPickPackEscalate.U14_SysRowID + "' and company = '" + oPickPackEscalate.Company + "') ";

                    _dts = oSQL._MSSQLDataSetResult(strSQL, _strSQLCon);

                    if (_dts.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in _dts.Tables[0].Rows)
                        {
                            strKey5 = row["key5"].ToString();
                        }
                    }


                    strSQL = "update ice.ud14 set CheckBox19 = 0, CheckBox17 = 1 ";
                    strSQL += "where key5 = '" + strKey5 + "' and company = '" + oPickPackEscalate.Company + "' ";

                    oSQL._exeSQLCommand(strSQL, _strSQLCon);


                    strSQL = "update ice.ud14 set SHORTCHAR17 = isnull((select top 1 x.shortchar17 from ice.ud14 x where x.key5 = '" + strKey5 + "' and x.Company = '" + oPickPackEscalate.Company  + "' and shortchar17 <> ''),'') ";
                    strSQL += "where key5 = '" + strKey5 + "' and company = '" + oPickPackEscalate.Company + "' ";
                    strSQL += "and SHORTCHAR17 = '' ";

                    oSQL._exeSQLCommand(strSQL, _strSQLCon);


                }
                catch (Exception ex)
                {
                    IsEpicTrxSuccess = false;
                    strMessage = ex.Message.ToString();
                }

                IsEpicTrxSuccess = (strMessage.Length > 0 ? false : true);

            }
            else
            {
                strMessage = "Login into Epicor failed. ";
                IsEpicTrxSuccess = false;
            }


            if (sessionId != Guid.Empty)
            {
                IsEpicLoginOK = true;
                sessionModClient.Logout();
            }
            else
            {
                IsEpicLoginOK = false;
                strMessage = "Unable to connect into Epicor";
            }

            return (IsEpicLoginOK && IsEpicTrxSuccess);



        }

        public bool _AMMIssueReturn(ref EpicEnv oEpicEnv, out string strMessage, string strUID = "", string strPass = "", string strCompany ="", string strPlant = "", string strMtlQueueRowID = "")
        {
            bool IsEpicLoginOK = false;
            bool IsEpicTrxSuccess = false;

            string partTran = "";

            string scheme = "http";
            string strEpicSessionModSvc = oEpicEnv.Env_AppEpicor + "/" + strSessionModSvcPath;
            string strEpicReturnMaterialSvc = oEpicEnv.Env_AppEpicor + "/" + strIssueReturnSvcPath;
            string strEpicUser = (oEpicEnv.Env_UsedEpicLogin ? strUID : oEpicEnv.Env_AppUserId);
            string strEpicPwd = (oEpicEnv.Env_UsedEpicLogin ? strPass : oEpicEnv.Env_AppPassKey);

            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => { return true; };
            EndpointBindingType bindingType = EndpointBindingType.BasicHttp;

            if (bindingType == EndpointBindingType.BasicHttp)
            {
                scheme = "https";
            }

            UriBuilder builder = new UriBuilder(scheme, oEpicEnv.Env_AppServer);

            builder.Path = strEpicSessionModSvc;
            SessionModSvcContractClient sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            builder.Path = strEpicReturnMaterialSvc;
            IssueReturnSvcContractClient issueReturnClient = GetClient<IssueReturnSvcContractClient, IssueReturnSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            Guid sessionId = Guid.Empty;

            try
            {
                sessionId = sessionModClient.Login();

                string companyName;
                string plantID;
                string plantName;
                string workstationID;
                string workstationDescription;
                string employeeID;
                string countryGroupCode;
                string countryCode;

                builder.Path = strEpicSessionModSvc;
                sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

                sessionModClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                sessionModClient.SetCompany(strCompany, out companyName, out plantID, out plantName, out workstationID, out workstationDescription,
                            out employeeID, out countryGroupCode, out countryCode);

                if (strPlant != "")
                {
                    sessionModClient.SetPlant(strPlant);
                }
                IsEpicLoginOK = true;
                strMessage = "";

            }
            catch (Exception ex)
            {
                IsEpicLoginOK = false;
                strMessage = ex.Message.ToString();
            }

            if (IsEpicLoginOK == true)
            {
                issueReturnClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                string strPCMessage;
                string strNeqQtyMessage, strBinAction, strBinMessage, strOutBinAction, strOutBinMessage;
                decimal dQtyAvail;
                bool NegQtyAction = false;
                var ts = new IssueReturnTableset();

                issueReturnClient.PreGetNewIssueReturn(Guid.Parse(strMtlQueueRowID), out strPCMessage, out dQtyAvail);
                issueReturnClient.GetNewIssueReturn("", Guid.Parse(strMtlQueueRowID), "MaterialQueue", ref ts);

                try 
                {
                    issueReturnClient.PrePerformMaterialMovement(ref ts);
                    issueReturnClient.MasterInventoryBinTests(ref ts, out strNeqQtyMessage, out strBinAction, out strBinMessage, out strOutBinAction, out strOutBinMessage);
                    issueReturnClient.PerformMaterialMovement(NegQtyAction, ref ts, out partTran);
                }  
                catch (Exception ex)
                {
                    IsEpicTrxSuccess = false;
                    strMessage = ex.Message.ToString();
                }

                IsEpicTrxSuccess = (strMessage.Length > 0 ? false : true);


            }
            else
            {
                strMessage = "Unable to connect into Epicor";
                IsEpicTrxSuccess = false;
            }





            if (sessionId != Guid.Empty)
            {
                IsEpicLoginOK = true;
                sessionModClient.Logout();
            }
            else
            {
                IsEpicLoginOK = false;
                strMessage = "Unable to connect into Epicor";
            }

            return (IsEpicLoginOK && IsEpicTrxSuccess);


        }

        public bool _PickedOrder(ref EpicEnv oEpicEnv, out string strMessage, ref IList<Order> oOrderList, string strUID = "", string strPass = "", string strCompany = "", string strPlant = "", string strPickList = "", string strConsignment = "", decimal dTotalWeight = 0M, string strShipVia = "", decimal dTotalBox = 0M, string strStationId = "")
        {
            bool IsEpicLoginOK = false;
            bool IsEpicTrxSuccess = false;

            string scheme = "http";
            string strEpicSessionModSvc = oEpicEnv.Env_AppEpicor + "/" + strSessionModSvcPath;
            string strEpicCustShipSvc = oEpicEnv.Env_AppEpicor + "/" + strCustShipSvcPath;
            string strEpicPickedOrdersSvc = oEpicEnv.Env_AppEpicor + "/" + strPickedOrdersSvcPath;
            string strEpicMaterialQueueSvc = oEpicEnv.Env_AppEpicor + "/" + strMaterialQueueSvcPath;
            string strEpicUser = (oEpicEnv.Env_UsedEpicLogin ? strUID : oEpicEnv.Env_AppUserId);
            string strEpicPwd = (oEpicEnv.Env_UsedEpicLogin ? strPass : oEpicEnv.Env_AppPassKey);

            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => { return true; };
            EndpointBindingType bindingType = EndpointBindingType.BasicHttp;

            if (bindingType == EndpointBindingType.BasicHttp)
            {
                scheme = "https";
            }

            UriBuilder builder = new UriBuilder(scheme, oEpicEnv.Env_AppServer);

            builder.Path = strEpicSessionModSvc;
            SessionModSvcContractClient sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            builder.Path = strEpicCustShipSvc;
            CustShipSvcContractClient custShipClient = GetClient<CustShipSvcContractClient, CustShipSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);
            CustShipSvcContractClient custShipClient1 = GetClient<CustShipSvcContractClient, CustShipSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            builder.Path = strEpicPickedOrdersSvc;
            PickedOrdersSvcContractClient pickedOrdersClient = GetClient<PickedOrdersSvcContractClient, PickedOrdersSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            builder.Path = strEpicMaterialQueueSvc;
            MaterialQueueSvcContractClient materialQueueClient = GetClient<MaterialQueueSvcContractClient, MaterialQueueSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            Guid sessionId = Guid.Empty;

            try
            {
                sessionId = sessionModClient.Login();

                string companyName;
                string plantID;
                string plantName;
                string workstationID;
                string workstationDescription;
                string employeeID;
                string countryGroupCode;
                string countryCode;

                builder.Path = strEpicSessionModSvc;
                sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

                sessionModClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));
                sessionModClient.SetTaskClientID(strEpicUser);
                sessionModClient.SetCompany(strCompany, out companyName, out plantID, out plantName, out workstationID, out workstationDescription,
                            out employeeID, out countryGroupCode, out countryCode);

                if (strPlant != "")
                {
                    sessionModClient.SetPlant(strPlant);
                }
                IsEpicLoginOK = true;
                strMessage = "";

            }
            catch (Exception ex)
            {
                IsEpicLoginOK = false;
                strMessage = ex.Message.ToString();
            }


            if (IsEpicLoginOK == true)
            {
                try
                {
                    int iPackNum;
                    //bool bErrorOccur;

                    custShipClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                    CustShipTableset ds = new CustShipTableset();


                    // create custshipment header
                    var iOrderNum = oOrderList.Select(o => o.OrderNum).Distinct().ToList();
                    custShipClient.GetNewShipHead(ref ds);
                    custShipClient.GetHeadOrderInfo(int.Parse( iOrderNum[0].ToString()), ref ds);

                    ds.ShipHead[0].Plant = strPlant;
                    ds.ShipHead[0].ReadyToInvoice = false;
                    ds.ShipHead[0].ShipViaCode = strShipVia;
                    ds.ShipHead[0].Weight = dTotalWeight;
                    ds.ShipHead[0].TrackingNumber = strConsignment;
                    ds.ShipHead[0].UserDefinedColumns["FS_PickListNo_c"] = strPickList;
                    ds.ShipHead[0].UserDefinedColumns["FS_PickingListNum_c"] = strPickList;
                    ds.ShipHead[0].UserDefinedColumns["SD_StationId_c"] = strStationId;
                    ds.ShipHead[0].UserDefinedColumns["SD_CartonNum_c"] = int.Parse( dTotalBox.ToString());
                    ds.ShipHead[0].ShipPerson = strEpicUser;
                    ds.ShipHead[0].EntryPerson = strEpicUser;
                    //ds.ShipHead[0].ShipDate = UD101.Date01;

                    custShipClient.Update(ref ds);

                    iPackNum = (int)ds.ShipHead[0].PackNum;


                    // create custship details
                    int iRow = 1;
                    custShipClient1.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));
                    foreach (var OrdItem in oOrderList)
                    {
                        
                        CustShipTableset ds1 = new CustShipTableset();
                        //ds1 = custShipClient1.GetByID(iPackNum);
                        custShipClient1.GetNewShipDtl(ref ds1, iPackNum);

                        var newShipLine = ds1.ShipDtl.Where(n => n.RowMod.Equals("A", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

                        if (newShipLine != null)
                        {
                            newShipLine.CustNum = int.Parse( OrdItem.CustNum);
                            newShipLine.ShipToNum = OrdItem.ShipToNum;
                            newShipLine.ShipToCustNum = int.Parse(OrdItem.CustNum);
                            newShipLine.OrderNum = OrdItem.OrderNum;
                            newShipLine.LineType = "PART";
                            newShipLine.OrderLine = OrdItem.OrderLine;
                            newShipLine.OrderRelNum = OrdItem.OrderRel;

                            newShipLine.PartNum = OrdItem.PartNum;
                            newShipLine.LineDesc = OrdItem.PartDescription;
                            newShipLine.IUM = OrdItem.IUM;
                            newShipLine.WarehouseCode = OrdItem.Warehouse;
                            newShipLine.BinNum = OrdItem.Bin;
                            newShipLine.LotNum = OrdItem.Lot;
                            newShipLine.Plant = strPlant;
                            newShipLine.SellingInventoryShipQty = OrdItem.OrderQty; ;
                            newShipLine.SalesUM = OrdItem.SalesUM;
                            newShipLine.OurInventoryShipQty = OrdItem.OrderQty;
                            newShipLine.InventoryShipUOM = OrdItem.SalesUM;
                            newShipLine.OurStockShippedQty = OrdItem.OrderQty;
                            newShipLine.WUM = OrdItem.NetWeightUOM;
                            newShipLine.SellingFactor = 1;
                            newShipLine.UserDefinedColumns["SD_IsComplete_c"] = (iRow == oOrderList.Count ? true : false);
                            newShipLine.RowMod = "A";

                            custShipClient1.GetQtyInfo(ref ds1, 0, OrdItem.OrderQty, 0);
                            custShipClient1.Update(ref ds1);
                        }

                        iRow++;

                    }

                    //
                    //ticked shipped
                    //CustShipTableset ds2 = new CustShipTableset();
                    //ds2 = custShipClient.GetByID(iPackNum);
                    //ds2.ShipHead[0].ReadyToInvoice= true;
                    //ds2.ShipHead[0].RowMod= "U";
                    //custShipClient.Update(ref ds);
                    //
                    //UpdExtCustShipTableset dsExt = new UpdExtCustShipTableset();
                    //ShipHeadRow bkpShipHeadRow = new ShipHeadRow();
                    //BufferCopy.Copy(ds2.ShipHead[0], ref bkpShipHeadRow);
                    //dsExt.ShipHead.Add(bkpShipHeadRow);

                    //custShipClient.UpdateExt(ref dsExt, true, true, out bErrorOccur);
                    


                }
                catch (Exception ex)
                {
                    IsEpicTrxSuccess = false;
                    strMessage = ex.Message.ToString();
                }

                IsEpicTrxSuccess = (strMessage.Length > 0 ? false : true);


            }
            else
            {
                strMessage = "Unable to connect into Epicor";
                IsEpicTrxSuccess = false;
            }




            if (sessionId != Guid.Empty)
            {
                IsEpicLoginOK = true;
                sessionModClient.Logout();
            }
            else
            {
                IsEpicLoginOK = false;
                strMessage = "Unable to connect into Epicor";
            }

            return (IsEpicLoginOK && IsEpicTrxSuccess);
        }


        //public bool __PrintBAQReport(ref EpicEnv oEpicEnv, out string strMessage,string strUID = "", string strPass = "", string strCompany = "", string strPlant = "", string strBAQID = "", string strBAQReport = "", int iReportStyle = 0, string strOutputFormat="", string strParm01 = "")
        //{
        //    bool IsEpicLoginOK = false;
        //    bool IsEpicTrxSuccess = false;
        /*
        string scheme = "http";
        string strEpicSessionModSvc = oEpicEnv.Env_AppEpicor + "/" + strSessionModSvcPath;
        string strBAQReportSvc = oEpicEnv.Env_AppEpicor + "/" + strBAQReportSvcPath;
        string strDynamicReportSvc = oEpicEnv.Env_AppEpicor + "/" + strDynamicReportSvcPath;
        string strReportMonitorSvc = oEpicEnv.Env_AppEpicor + "/" + strReportMonitorSvcPath;
        string strEpicUser = (oEpicEnv.Env_UsedEpicLogin ? strUID : oEpicEnv.Env_AppUserId);
        string strEpicPwd = (oEpicEnv.Env_UsedEpicLogin ? strPass : oEpicEnv.Env_AppPassKey);

        System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => { return true; };
        EndpointBindingType bindingType = EndpointBindingType.BasicHttp;

        if (bindingType == EndpointBindingType.BasicHttp)
        {
            scheme = "https";
        }

        UriBuilder builder = new UriBuilder(scheme, oEpicEnv.Env_AppServer);

        builder.Path = strEpicSessionModSvc;
        SessionModSvcContractClient sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

        builder.Path = strBAQReportSvc;
        BAQReportSvcContractClient baqReportClient = GetClient<BAQReportSvcContractClient, BAQReportSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

        builder.Path = strDynamicReportSvc;
        DynamicReportSvcContractClient dynamicReportClient = GetClient<DynamicReportSvcContractClient, DynamicReportSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

        builder.Path = strReportMonitorSvc;
        ReportMonitorSvcContractClient reportMonitorClient = GetClient<ReportMonitorSvcContractClient, ReportMonitorSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);


        Guid sessionId = Guid.Empty;
        string workstationID = "";

        try
        {
            sessionId = sessionModClient.Login();

            string companyName;
            string plantID;
            string plantName;

            string workstationDescription;
            string employeeID;
            string countryGroupCode;
            string countryCode;

            builder.Path = strEpicSessionModSvc;
            sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            sessionModClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));
            sessionModClient.SetTaskClientID(strEpicUser);
            sessionModClient.SetCompany(strCompany, out companyName, out plantID, out plantName, out workstationID, out workstationDescription,
                        out employeeID, out countryGroupCode, out countryCode);

            if (strPlant != "")
            {
                sessionModClient.SetPlant(strPlant);
            }
            IsEpicLoginOK = true;
            strMessage = "";

        }
        catch (Exception ex)
        {
            IsEpicLoginOK = false;
            strMessage = ex.Message.ToString();
        }

        if (IsEpicLoginOK == true)
        {

            DynamicReportTableset rptDs = dynamicReportClient.GetByID(strBAQReport);
            var baqRptDS = baqReportClient.GetNewBAQReportParam(strBAQReport);

            baqRptDS.BAQReportParam[0].AutoAction = "SSRSPrint";
            baqRptDS.BAQReportParam[0].WorkstationID = workstationID;
            baqRptDS.BAQReportParam[0].SSRSRenderFormat = strOutputFormat;
            baqRptDS.BAQReportParam[0].AttachmentType = strOutputFormat;
            baqRptDS.BAQReportParam[0].BAQRptID = strBAQReport;
            baqRptDS.BAQReportParam[0].ReportID = strBAQReport;
            baqRptDS.BAQReportParam[0].Summary = false;
            baqRptDS.BAQReportParam[0].ReportStyleNum = iReportStyle;
            baqRptDS.BAQReportParam[0].BAQID = strBAQID;
            baqRptDS.BAQReportParam[0].UserID = strUID;
            baqRptDS.BAQReportParam[0].SSRSEnableRouting = true;



            rptDs.BAQRptOptionFld[0].FieldValue = strParm01;
            //rptDs.AcceptChanges();
            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(rptDs.GetType());

            //baqRptDS.BAQReportParam[0].Filter1 = rptDs.;


        }
        else
        {
            IsEpicLoginOK = false;
            strMessage = "Unable to connect into Epicor";
        }



        if (sessionId != Guid.Empty)
        {
            IsEpicLoginOK = true;
            sessionModClient.Logout();
        }
        else
        {
            IsEpicLoginOK = false;
            strMessage = "Unable to connect into Epicor";
        }
        */
        //return (IsEpicLoginOK && IsEpicTrxSuccess);

        //}


        public bool _EpicCreateCustomer(ref EpicEnv oEpicEnv, ref Customer oCustomer,  out string strMessage, string strUID = "", string strPass = "")
        {

            bool IsEpicLoginOK = false;
            bool IsEpicTrxSuccess = false;

            string scheme = "http";
            string strEpicSessionModSvc = oEpicEnv.Env_AppEpicor + "/" + strSessionModSvcPath;
            string strCustomerSvc = oEpicEnv.Env_AppEpicor + "/" + strCustomerSvcPath;
            //string partTran = "";

            string strEpicUser = (oEpicEnv.Env_UsedEpicLogin ? strUID : oEpicEnv.Env_AppUserId);
            string strEpicPwd = (oEpicEnv.Env_UsedEpicLogin ? strPass : oEpicEnv.Env_AppPassKey);


            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => { return true; };
            EndpointBindingType bindingType = EndpointBindingType.BasicHttp;

            if (bindingType == EndpointBindingType.BasicHttp)
            {
                scheme = "https";
            }

            UriBuilder builder = new UriBuilder(scheme, oEpicEnv.Env_AppServer);

            builder.Path = strEpicSessionModSvc;
            SessionModSvcContractClient sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            builder.Path = strCustomerSvc;
            CustomerSvcContractClient customerClient = GetClient<CustomerSvcContractClient, CustomerSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            Guid sessionId = Guid.Empty;

            try
            {
                sessionId = sessionModClient.Login();

                string companyName;
                string plantID;
                string plantName;
                string workstationID;
                string workstationDescription;
                string employeeID;
                string countryGroupCode;
                string countryCode;


                builder.Path = strEpicSessionModSvc;
                sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

                sessionModClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                sessionModClient.SetCompany(oCustomer.Company, out companyName, out plantID, out plantName, out workstationID, out workstationDescription,
                                            out employeeID, out countryGroupCode, out countryCode);


                IsEpicLoginOK = true;
                strMessage = "";

            }
            catch (Exception ex)
            {
                IsEpicLoginOK = false;
                strMessage = ex.Message.ToString();
            }


            if (IsEpicLoginOK == true)
            {
                var ts = new  CustomerTableset();
                customerClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                try
                {
                    customerClient.GetNewCustomer(ref ts);

                    var newRow = ts.Customer[0];

                    if (newRow != null)
                    {
                        newRow.Company = oCustomer.Company;
                        newRow.CustID = oCustomer.CustID;
                        newRow.Name = oCustomer.CustName;
                        
                        newRow.Address1 = oCustomer.Address1;
                        newRow.Address2 = oCustomer.Address2;
                        newRow.Address3 = oCustomer.Address3;
                        newRow.City = oCustomer.City;
                        newRow.State = oCustomer.State;
                        newRow.Zip = oCustomer.Zip;
                        newRow.CountryNum = oCustomer.CountryNum;
                        newRow.PhoneNum = oCustomer.PhoneNum;
                        newRow.FaxNum = oCustomer.FaxNum;
                        newRow.EMailAddress = oCustomer.EMailAddress;
                        newRow.CustURL = oCustomer.CustURL;

                        newRow.TerritoryID = oCustomer.TerritoryID;
                        newRow.SalesRepCode = oCustomer.SalesRepCode;
                        newRow.TermsCode = oCustomer.TermsCode;
                        newRow.GroupCode = oCustomer.GroupCode;
                        newRow.BusinessCatList = oCustomer.BusinessCatList;

                        newRow.UserDefinedColumns["FS_APCLicense_c"] = oCustomer.FS_APCLicense_c;
                        newRow.UserDefinedColumns["FS_MMAMembership_c"] = oCustomer.FS_MMAMembership_c;
                        newRow.UserDefinedColumns["FS_GVOTForm24_c"] = oCustomer.FS_GVOTForm24_c;
                        newRow.UserDefinedColumns["FS_GVOTForm49_c"] = oCustomer.FS_GVOTForm49_c;
                        newRow.UserDefinedColumns["FS_BussReg_c"] = oCustomer.FS_BussReg_c;
                        newRow.UserDefinedColumns["FS_TradingLicense_c"] = oCustomer.FS_TradingLicense_c;
                        newRow.UserDefinedColumns["FS_IC_c"] = oCustomer.FS_IC_c;

                        newRow.RowMod = "A";


                        customerClient.CheckCreditHold(ts);
                        //customerClient.CheckDupCustomer(oCustomer.CustName, oCustomer.CustID, Guid.NewGuid(), "", "");
                        customerClient.Update(ref ts);
                    }



                    IsEpicTrxSuccess = true;
                }
                catch (Exception ex)
                {
                    IsEpicTrxSuccess = false;
                    strMessage = ex.Message.ToString();
                }


            }


            if (sessionId != Guid.Empty)
            {
                IsEpicLoginOK = true;
                sessionModClient.Logout();
            }
            else
            {
                IsEpicLoginOK = false;
                strMessage = "Unable to connect into Epicor";
            }

            return (IsEpicLoginOK && IsEpicTrxSuccess);
        }

        public bool _ReprintInvoice(ref EpicEnv oEpicEnv, string strCurCompany, string strCurPlant, string strInvoiceNum, string strLegalNumber, string strPickListNum, out string strMessage, string strUID = "", string strPass = "")
        {
            bool IsEpicLoginOK = false;
            bool IsEpicTrxSuccess = false;

            string scheme = "http";
            string strEpicSessionModSvc = oEpicEnv.Env_AppEpicor + "/" + strSessionModSvcPath;
            //string strEpicUD14Svc = oEpicEnv.Env_AppEpicor + "/" + strUD14SvcPath;

            string strEpicUser = (oEpicEnv.Env_UsedEpicLogin ? strUID : oEpicEnv.Env_AppUserId);
            string strEpicPwd = (oEpicEnv.Env_UsedEpicLogin ? strPass : oEpicEnv.Env_AppPassKey);

            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => { return true; };
            EndpointBindingType bindingType = EndpointBindingType.BasicHttp;

            if (bindingType == EndpointBindingType.BasicHttp)
            {
                scheme = "https";
            }

            UriBuilder builder = new UriBuilder(scheme, oEpicEnv.Env_AppServer);

            builder.Path = strEpicSessionModSvc;
            SessionModSvcContractClient sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

           // builder.Path = strEpicUD14Svc;
            //UD14SvcContractClient ud14Client = GetClient<UD14SvcContractClient, UD14SvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            Guid sessionId = Guid.Empty;

            try
            {
                sessionId = sessionModClient.Login();

                string companyName;
                string plantID;
                string plantName;
                string workstationID;
                string workstationDescription;
                string employeeID;
                string countryGroupCode;
                string countryCode;


                builder.Path = strEpicSessionModSvc;
                sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

                sessionModClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                sessionModClient.SetCompany(strCurCompany, out companyName, out plantID, out plantName, out workstationID, out workstationDescription,
                                            out employeeID, out countryGroupCode, out countryCode);

                if (strCurPlant != "")
                {
                    sessionModClient.SetPlant(strCurPlant);
                }

                IsEpicLoginOK = true;
                strMessage = "";

            }
            catch (Exception ex)
            {
                IsEpicLoginOK = false;
                strMessage = ex.Message.ToString();
            }

            if (IsEpicLoginOK == true)
            {
                try
                {
                    



                }
                catch (Exception ex)
                {
                    IsEpicTrxSuccess = false;
                    strMessage = ex.Message.ToString();
                }

                IsEpicTrxSuccess = (strMessage.Length > 0 ? false : true);

            }
            else
            {
                strMessage = "Login into Epicor failed. ";
                IsEpicTrxSuccess = false;
            }




            if (sessionId != Guid.Empty)
            {
                IsEpicLoginOK = true;
                sessionModClient.Logout();
            }
            else
            {
                IsEpicLoginOK = false;
                strMessage = "Unable to connect into Epicor";
            }

            return (IsEpicLoginOK && IsEpicTrxSuccess);


        }

        public bool _ShipHead(ref EpicEnv oEpicEnv, out string strMessage, out int iPackNum, int iOrderNum, string strPickList, string strUID = "", string strPass = "", string strCompany = "", string strPlant = "")
        {
            bool IsEpicLoginOK = false;
            bool IsEpicTrxSuccess = false;

            string scheme = "http";
            string strEpicSessionModSvc = oEpicEnv.Env_AppEpicor + "/" + strSessionModSvcPath;
            string strEpicCustShipSvc = oEpicEnv.Env_AppEpicor + "/" + strCustShipSvcPath;

            string strEpicUser = (oEpicEnv.Env_UsedEpicLogin ? strUID : oEpicEnv.Env_AppUserId);
            string strEpicPwd = (oEpicEnv.Env_UsedEpicLogin ? strPass : oEpicEnv.Env_AppPassKey);

            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => { return true; };
            EndpointBindingType bindingType = EndpointBindingType.BasicHttp;

            if (bindingType == EndpointBindingType.BasicHttp)
            {
                scheme = "https";
            }

            UriBuilder builder = new UriBuilder(scheme, oEpicEnv.Env_AppServer);

            builder.Path = strEpicSessionModSvc;
            SessionModSvcContractClient sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            builder.Path = strEpicCustShipSvc;
            CustShipSvcContractClient custShipClient = GetClient<CustShipSvcContractClient, CustShipSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            Guid sessionId = Guid.Empty;

            try
            {
                sessionId = sessionModClient.Login();

                string companyName;
                string plantID;
                string plantName;
                string workstationID;
                string workstationDescription;
                string employeeID;
                string countryGroupCode;
                string countryCode;

                builder.Path = strEpicSessionModSvc;
                sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

                sessionModClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));
                sessionModClient.SetTaskClientID(strEpicUser);
                sessionModClient.SetCompany(strCompany, out companyName, out plantID, out plantName, out workstationID, out workstationDescription,
                            out employeeID, out countryGroupCode, out countryCode);

                if (strPlant != "")
                {
                    sessionModClient.SetPlant(strPlant);
                }
                IsEpicLoginOK = true;
                strMessage = "";

            }
            catch (Exception ex)
            {
                IsEpicLoginOK = false;
                strMessage = ex.Message.ToString();
            }


            if (IsEpicLoginOK == true)
            {
                try
                {
                    custShipClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));
                    CustShipTableset ds = new CustShipTableset();

                    // create custshipment header
                    custShipClient.GetNewShipHead(ref ds);
                    custShipClient.GetHeadOrderInfo(iOrderNum, ref ds);

                    ds.ShipHead[0].Plant = strPlant;
                    ds.ShipHead[0].ReadyToInvoice = false;
                    //ds.ShipHead[0].ShipViaCode = strShipVia;
                    //ds.ShipHead[0].Weight = dTotalWeight;
                    //ds.ShipHead[0].TrackingNumber = strConsignment;
                    ds.ShipHead[0].UserDefinedColumns["SD_Picker_c"] = strUID;
                    ds.ShipHead[0].UserDefinedColumns["FS_PickListNo_c"] = strPickList;
                    ds.ShipHead[0].UserDefinedColumns["SD_StartPicked_c"] = DateTime.Today;
                    //ds.ShipHead[0].UserDefinedColumns["SD_CartonNum_c"] = int.Parse(dTotalBox.ToString());
                    ds.ShipHead[0].ShipPerson = strEpicUser;
                    ds.ShipHead[0].EntryPerson = strEpicUser;
                    ds.ShipHead[0].ShipDate = DateTime.Today;

                    custShipClient.Update(ref ds);

                    iPackNum = (int)ds.ShipHead[0].PackNum;

                    IsEpicTrxSuccess = true;

                }
                catch (Exception ex)
                {
                    IsEpicTrxSuccess = false;
                    strMessage = ex.Message.ToString();
                    iPackNum = 0;
                }

                //IsEpicTrxSuccess = (strMessage.Length > 0 ? false : true);


            }
            else
            {
                strMessage = "Unable to connect into Epicor";
                IsEpicTrxSuccess = false;
                iPackNum = 0;
            }




            if (sessionId != Guid.Empty)
            {
                IsEpicLoginOK = true;
                sessionModClient.Logout();
            }
            else
            {
                IsEpicLoginOK = false;
                strMessage = "Unable to connect into Epicor";
            }

            return (IsEpicLoginOK && IsEpicTrxSuccess);
        }

        public bool _ShipDetail(ref EpicEnv oEpicEnv, out string strMessage, int iPackNum, ref Order oOrdRel, string strUID = "", string strPass = "", string strCompany = "", string strPlant = "")
        {
            bool IsEpicLoginOK = false;
            bool IsEpicTrxSuccess = false;

            string scheme = "http";
            string strEpicSessionModSvc = oEpicEnv.Env_AppEpicor + "/" + strSessionModSvcPath;
            string strEpicCustShipSvc = oEpicEnv.Env_AppEpicor + "/" + strCustShipSvcPath;

            string strEpicUser = (oEpicEnv.Env_UsedEpicLogin ? strUID : oEpicEnv.Env_AppUserId);
            string strEpicPwd = (oEpicEnv.Env_UsedEpicLogin ? strPass : oEpicEnv.Env_AppPassKey);

            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => { return true; };
            EndpointBindingType bindingType = EndpointBindingType.BasicHttp;

            if (bindingType == EndpointBindingType.BasicHttp)
            {
                scheme = "https";
            }

            UriBuilder builder = new UriBuilder(scheme, oEpicEnv.Env_AppServer);

            builder.Path = strEpicSessionModSvc;
            SessionModSvcContractClient sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            builder.Path = strEpicCustShipSvc;
            CustShipSvcContractClient custShipClient = GetClient<CustShipSvcContractClient, CustShipSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            Guid sessionId = Guid.Empty;

            try
            {
                sessionId = sessionModClient.Login();

                string companyName;
                string plantID;
                string plantName;
                string workstationID;
                string workstationDescription;
                string employeeID;
                string countryGroupCode;
                string countryCode;

                builder.Path = strEpicSessionModSvc;
                sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

                sessionModClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));
                sessionModClient.SetTaskClientID(strEpicUser);
                sessionModClient.SetCompany(strCompany, out companyName, out plantID, out plantName, out workstationID, out workstationDescription,
                            out employeeID, out countryGroupCode, out countryCode);

                if (strPlant != "")
                {
                    sessionModClient.SetPlant(strPlant);
                }
                IsEpicLoginOK = true;
                strMessage = "";

            }
            catch (Exception ex)
            {
                IsEpicLoginOK = false;
                strMessage = ex.Message.ToString();
            }

            if (IsEpicLoginOK == true)
            {
                try
                {
                    custShipClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));
                    CustShipTableset ds = new CustShipTableset();

                    custShipClient.GetNewShipDtl(ref ds, iPackNum);
                    var newShipLine = ds.ShipDtl.Where(n => n.RowMod.Equals("A", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

                    if (newShipLine != null)
                    {
                        newShipLine.CustNum = int.Parse(oOrdRel.CustNum);
                        newShipLine.ShipToNum = oOrdRel.ShipToNum;
                        newShipLine.ShipToCustNum = int.Parse(oOrdRel.CustNum);
                        newShipLine.OrderNum = oOrdRel.OrderNum;
                        newShipLine.LineType = "PART";
                        newShipLine.OrderLine = oOrdRel.OrderLine;
                        newShipLine.OrderRelNum = oOrdRel.OrderRel;

                        newShipLine.PartNum = oOrdRel.PartNum;
                        newShipLine.LineDesc = oOrdRel.PartDescription;
                        newShipLine.IUM = oOrdRel.IUM;
                        newShipLine.WarehouseCode = oOrdRel.Warehouse;
                        newShipLine.BinNum = oOrdRel.Bin;
                        newShipLine.LotNum = oOrdRel.Lot;
                        newShipLine.Plant = strPlant;
                        newShipLine.SellingInventoryShipQty = oOrdRel.OrderQty; ;
                        newShipLine.SalesUM = oOrdRel.SalesUM;
                        newShipLine.OurInventoryShipQty = oOrdRel.OrderQty;
                        newShipLine.InventoryShipUOM = oOrdRel.SalesUM;
                        newShipLine.OurStockShippedQty = oOrdRel.OrderQty;
                        newShipLine.WUM = oOrdRel.NetWeightUOM;
                        newShipLine.SellingFactor = 1;
                        //newShipLine.UserDefinedColumns["SD_IsComplete_c"] = (iRow == oOrderList.Count ? true : false);
                        newShipLine.RowMod = "A";

                        custShipClient.GetQtyInfo(ref ds, 0, oOrdRel.OrderQty, 0);
                        custShipClient.Update(ref ds);

                        IsEpicTrxSuccess = true;
                    }


                }
                catch (Exception ex)
                {
                    IsEpicTrxSuccess = false;
                    strMessage = ex.Message.ToString();
                }

                //IsEpicTrxSuccess = (strMessage.Length > 0 ? false : true);

            }
            else
            {
                strMessage = "Unable to connect into Epicor";
                IsEpicTrxSuccess = false;
            }



            if (sessionId != Guid.Empty)
            {
                IsEpicLoginOK = true;
                sessionModClient.Logout();
            }
            else
            {
                IsEpicLoginOK = false;
                strMessage = "Unable to connect into Epicor";
            }

            return (IsEpicLoginOK && IsEpicTrxSuccess);


        }

        public bool _ShipDetailUpd(ref EpicEnv oEpicEnv, out string strMessage, int iPackNum, int iPackLine, string strUID = "", string strPass = "", string strCompany = "", string strPlant = "", string strPicker = "", string strPacker = "", string strTagNum = "", string strPallet = "")
        {
            bool IsEpicLoginOK = false;
            bool IsEpicTrxSuccess = false;

            string scheme = "http";
            string strEpicSessionModSvc = oEpicEnv.Env_AppEpicor + "/" + strSessionModSvcPath;
            string strEpicCustShipSvc = oEpicEnv.Env_AppEpicor + "/" + strCustShipSvcPath;

            string strEpicUser = (oEpicEnv.Env_UsedEpicLogin ? strUID : oEpicEnv.Env_AppUserId);
            string strEpicPwd = (oEpicEnv.Env_UsedEpicLogin ? strPass : oEpicEnv.Env_AppPassKey);

            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => { return true; };
            EndpointBindingType bindingType = EndpointBindingType.BasicHttp;

            if (bindingType == EndpointBindingType.BasicHttp)
            {
                scheme = "https";
            }

            UriBuilder builder = new UriBuilder(scheme, oEpicEnv.Env_AppServer);

            builder.Path = strEpicSessionModSvc;
            SessionModSvcContractClient sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            builder.Path = strEpicCustShipSvc;
            CustShipSvcContractClient custShipClient = GetClient<CustShipSvcContractClient, CustShipSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            Guid sessionId = Guid.Empty;

            try
            {
                sessionId = sessionModClient.Login();

                string companyName;
                string plantID;
                string plantName;
                string workstationID;
                string workstationDescription;
                string employeeID;
                string countryGroupCode;
                string countryCode;

                builder.Path = strEpicSessionModSvc;
                sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

                sessionModClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));
                sessionModClient.SetTaskClientID(strEpicUser);
                sessionModClient.SetCompany(strCompany, out companyName, out plantID, out plantName, out workstationID, out workstationDescription,
                            out employeeID, out countryGroupCode, out countryCode);

                if (strPlant != "")
                {
                    sessionModClient.SetPlant(strPlant);
                }
                IsEpicLoginOK = true;
                strMessage = "";

            }
            catch (Exception ex)
            {
                IsEpicLoginOK = false;
                strMessage = ex.Message.ToString();
            }

            if (IsEpicLoginOK == true)
            {
                try
                {
                    custShipClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));
                    CustShipTableset ds = new CustShipTableset();

                    ds =custShipClient.GetByID( iPackNum);

                    var newShipLine = ds.ShipDtl.Where(n => n.PackLine == iPackLine).FirstOrDefault();

                    if (newShipLine != null)
                    {

                        if (strPicker != "")
                        {
                            newShipLine.UserDefinedColumns["SD_PickedBy_c"] = strPicker;
                            newShipLine.UserDefinedColumns["SD_IsPicked_c"] = true;
                            newShipLine.UserDefinedColumns["SD_PickedDate_c"] = DateTime.Now;
                        }

                        if (strPacker != "")
                        {
                            newShipLine.UserDefinedColumns["SD_PackedBy_c"] = strPacker;
                            newShipLine.UserDefinedColumns["SD_IsPacked_c"] = true;
                            newShipLine.UserDefinedColumns["SD_PackedDate_c"] = DateTime.Now;
                        }

                        newShipLine.UserDefinedColumns["SD_TagNum_c"] = strTagNum;
                        newShipLine.RowMod = "U";

                        //custShipClient.GetQtyInfo(ref ds, 0, oOrdRel.OrderQty, 0);
                        custShipClient.Update(ref ds);

                        SQLServerBO oSQL = new SQLServerBO();

                        string _strSQLCon = oSQL._retSQLConnectionString();
                        _strSQLCon = string.Format(_strSQLCon, oEpicEnv.Env_SQLServer, oEpicEnv.Env_SQLDB, oEpicEnv.Env_SQLUserId, oEpicEnv.Env_SQLPassKey);

                        string strSQL;
                        DataSet _dts;
                        int rowstoscan = 0;

                        strSQL = "select isnull(count(1),0) as rowstoscan from shipdtl ";
                        strSQL += "where company = '"  + strCompany + "' and packnum = '" + iPackNum.ToString() + "' and SD_IsPicked_c = 0 ";

                        _dts = oSQL._MSSQLDataSetResult(strSQL, _strSQLCon);

                        if (_dts.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in _dts.Tables[0].Rows)
                            {
                                rowstoscan = Convert.ToInt16( row["rowstoscan"]);
                            }
                        }

                        if (rowstoscan == 0)
                        {
                            strSQL = "update shiphead set SD_IsPicked_c = 1, SD_EndPicked_c = getdate(), SD_PalletNum_c = '" + strPallet + "' ";
                            strSQL += "where company = '" + strCompany + "' and packnum = '" + iPackNum.ToString() + "' ";

                        }
                        else
                        {
                            strSQL = "update shiphead set SD_PalletNum_c = '" + strPallet + "' ";
                            strSQL += "where company = '" + strCompany + "' and packnum = '" + iPackNum.ToString() + "' ";

                        }

                        oSQL._exeSQLCommand(strSQL, _strSQLCon);

                    }


                }
                catch (Exception ex)
                {
                    IsEpicTrxSuccess = false;
                    strMessage = ex.Message.ToString();
                }

                IsEpicTrxSuccess = (strMessage.Length > 0 ? false : true);

            }
            else
            {
                strMessage = "Unable to connect into Epicor";
                IsEpicTrxSuccess = false;
            }



            if (sessionId != Guid.Empty)
            {
                IsEpicLoginOK = true;
                sessionModClient.Logout();
            }
            else
            {
                IsEpicLoginOK = false;
                strMessage = "Unable to connect into Epicor";
            }

            return (IsEpicLoginOK && IsEpicTrxSuccess);


        }



        public void Dispose(bool v)
        {
            throw new NotImplementedException();
        }


    }

}