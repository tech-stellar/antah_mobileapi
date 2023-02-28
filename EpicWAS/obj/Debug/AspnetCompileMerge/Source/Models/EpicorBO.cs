using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ServiceModel;
using System.ServiceModel.Channels;
using EpicWAS.Epicor.SessionModSvc;
using EpicWAS.Epicor.IssueReturnSvc;
using EpicWAS.Epicor.InvTransferSvc;
using EpicWAS.Epicor.ReceiptsFromMfgSvc;
using EpicWAS.Epicor.LotSelectUpdateSvc;

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

        public bool _EpicIssueMaterialToJob( ref EpicEnv oEpicEnv, ref IssueMtlToJob oIssueMtlToJob, out string strMessage)
        {

            bool IsEpicLoginOK = false;
            bool IsEpicTrxSuccess = false;

            string scheme = "http";
            string strEpicSessionModSvc = oEpicEnv.Env_AppEpicor + "/" + strSessionModSvcPath;
            string strEpicIssueMaterialSvc = oEpicEnv.Env_AppEpicor + "/" + strIssueReturnSvcPath;
            string partTran = "";
            string strEpicUser = oEpicEnv.Env_AppUserId;
            string strEpicPwd = oEpicEnv.Env_AppPassKey;

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

                builder.Path = strEpicSessionModSvc;
                sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

                sessionModClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

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
                issueReturnClient.GetNewIssueReturnToJob(oIssueMtlToJob.JobNum, oIssueMtlToJob.AssemblySeq, "STK-MTL", Guid.NewGuid(), ref ts);

                var newRow = ts.IssueReturn.Where(n => n.RowMod.Equals("A", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

                if (newRow != null)
                {
                    newRow.Company = oIssueMtlToJob.Company;
                    newRow.PartNum = oIssueMtlToJob.PartNum;
                    newRow.PartIUM = oIssueMtlToJob.IUM;
                    newRow.TranQty = oIssueMtlToJob.TranQty;
                    newRow.DimCode = oIssueMtlToJob.IUM;
                    newRow.DUM = oIssueMtlToJob.IUM; 
                    newRow.UM = oIssueMtlToJob.IUM;
                    newRow.LotNum = oIssueMtlToJob.LotNum;

                    newRow.FromWarehouseCode = oIssueMtlToJob.FromWarehouseCode;
                    newRow.FromBinNum = oIssueMtlToJob.FromBinNum;
                    

                    newRow.ToWarehouseCode = oIssueMtlToJob.ToWarehouseCode;
                    newRow.ToBinNum = oIssueMtlToJob.ToBinNum;
                    newRow.ToJobNum = oIssueMtlToJob.ToJobNum;

                    newRow.ToJobPartNum = oIssueMtlToJob.PartNum;
                    newRow.ToJobSeqPartNum = oIssueMtlToJob.PartNum;
                    newRow.ToJobSeq = oIssueMtlToJob.ToJobSeq;

                    newRow.RowMod = "U";

                    issueReturnClient.PrePerformMaterialMovement(ref ts);
                    issueReturnClient.PerformMaterialMovement(true, ref ts, out partTran);

                    IsEpicTrxSuccess = (partTran.Length > 0 ? false : true);

                    strMessage = partTran;
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


        public bool _EpicReceiptsFromMfg(string strEpicSvrName, string strEpicEnv, string strEpicUser, string strEpicPwd, string strJobNum, int intAssemblySeq, out string strMessage)
        {
            bool IsEpicLoginOK = false;
            bool IsEpicTrxSuccess = false;

            string scheme = "http";
            string strEpicSessionModSvc = strEpicEnv + "/" + strSessionModSvcPath;
            string strEpicReceiptsFromMfgSvc = strEpicEnv + "/" + strReceiptsFromMfgSvcPath;
            string partTran = "";

            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => { return true; };
            EndpointBindingType bindingType = EndpointBindingType.BasicHttp;

            if (bindingType == EndpointBindingType.BasicHttp)
            {
                scheme = "https";
            }

            UriBuilder builder = new UriBuilder(scheme, strEpicSvrName);

            builder.Path = strEpicSessionModSvc;
            SessionModSvcContractClient sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            builder.Path = strEpicReceiptsFromMfgSvc;
            ReceiptsFromMfgSvcContractClient receiptsFromMfgClient = GetClient<ReceiptsFromMfgSvcContractClient, ReceiptsFromMfgSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            Guid sessionId = Guid.Empty;

            try
            {
                sessionId = sessionModClient.Login();

                builder.Path = strEpicSessionModSvc;
                sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

                sessionModClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

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

                receiptsFromMfgClient.GetNewReceiptsFromMfgJobAsm(strJobNum, intAssemblySeq, "MFG-STK", "RcptToInvEntry", ref ts);

                var newRow = ts.PartTran.Where(n => n.RowMod.Equals("A", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

                if (newRow != null)
                {
                    newRow.Company = "EPIC05";
                    newRow.ActTranQty = 999;
                    newRow.TranQty = 999;
                    newRow.ThisTranQty = 999;
                    newRow.InventoryTrans = true;

                    newRow.RowMod = "U";

                    receiptsFromMfgClient.PreUpdate(ref ts);
                    receiptsFromMfgClient.ReceiveMfgPartToInventory(ref ts, 0, true, "RcptToInvEntry", out partTran);

                    IsEpicTrxSuccess = (partTran.Length > 0 ? false : true);

                    strMessage = partTran;
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


        public bool _EpicInvTransfer(ref EpicEnv oEpicEnv, ref MoveInventory oMoveInventory, out string strMessage)
        {
            bool IsEpicLoginOK = false;
            bool IsEpicTrxSuccess = false;

            string scheme = "http";
            string strEpicSessionModSvc = oEpicEnv.Env_AppEpicor + "/" + strSessionModSvcPath;
            string strEpicInvTransferSvc = oEpicEnv.Env_AppEpicor + "/" + strInvTransferSvcPath;
            string partTran = "";
            string strEpicUser = oEpicEnv.Env_AppUserId;
            string strEpicPwd = oEpicEnv.Env_AppPassKey;

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

                builder.Path = strEpicSessionModSvc;
                sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

                sessionModClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

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
                invTransferClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

                var ts = new InvTransferTableset();

                //ts = invTransferClient.GetTransferRecord(strPartNum, strUOM);

                var newRow = ts.InvTrans.Where(n => n.RowMod.Equals("A", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

                if (newRow != null)
                {
                    newRow.Company = oMoveInventory.Company;
                    newRow.TransferQty = oMoveInventory.TranQty;
                    newRow.TrackingQty = oMoveInventory.TranQty;

                    newRow.FromWarehouseCode = oMoveInventory.FromWarehouseCode;
                    newRow.FromBinNum = oMoveInventory.FromBinNum;
                    newRow.FromLotNumber = oMoveInventory.FromLotNum;
                    //newRow.FromPlant = "";

                    newRow.ToWarehouseCode = oMoveInventory.ToWarehouseCode;
                    invTransferClient.ChangeToWhse(ref ts);

                    newRow.ToBinNum = oMoveInventory.ToBinNum;
                    newRow.ToLotNumber = oMoveInventory.ToLotNum;
                    //newRow.ToPlant = "";
                    newRow.TranReference = oMoveInventory.Reference;
                    newRow.RowMod = "U";

                    invTransferClient.PreCommitTransfer(ref ts);
                    invTransferClient.CommitTransfer(ref ts, out partTran);

                    IsEpicTrxSuccess = (partTran.Length > 0 ? false : true);

                    strMessage = partTran;
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


        public bool _EpicJobReceiptsToSalvage(string strEpicSvrName, string strEpicEnv, string strEpicUser, string strEpicPwd, string strJobNum, int intAssemblySeq, out string strMessage)
        {

            bool IsEpicLoginOK = false;
            bool IsEpicTrxSuccess = false;

            string scheme = "http";
            string strEpicSessionModSvc = strEpicEnv + "/" + strSessionModSvcPath;
            string strEpicReceiptsFromMfgSvc = strEpicEnv + "/" + strReceiptsFromMfgSvcPath;
            string partTran = "";

            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => { return true; };
            EndpointBindingType bindingType = EndpointBindingType.BasicHttp;

            if (bindingType == EndpointBindingType.BasicHttp)
            {
                scheme = "https";
            }

            UriBuilder builder = new UriBuilder(scheme, strEpicSvrName);

            builder.Path = strEpicSessionModSvc;
            SessionModSvcContractClient sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            builder.Path = strEpicReceiptsFromMfgSvc;
            ReceiptsFromMfgSvcContractClient receiptsFromMfgClient = GetClient<ReceiptsFromMfgSvcContractClient, ReceiptsFromMfgSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

            Guid sessionId = Guid.Empty;

            try
            {
                sessionId = sessionModClient.Login();

                builder.Path = strEpicSessionModSvc;
                sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

                sessionModClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

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

                var ts = new SelectedReceiptsJobAsmblTableset();

                //ts.SelectedJobAsmbl

                receiptsFromMfgClient.GetNewJobAsmblMultiple(ref ts, "SVG-STK", "RcptToSalEntry");




                //var newRow = ts.PartTran.Where(n => n.RowMod.Equals("A", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

                //if (newRow != null)
                //{
                //    newRow.Company = "EPIC05";
                //    newRow.JobNum = "";
                //    newRow.AssemblySeq = 0;

                //    newRow.RowMod = "U";

                //    receiptsFromMfgClient.ReceiveSalvagedPartToInventory( ref ts, "RcptToSalEntry", out partTran);

                //    IsEpicTrxSuccess = (partTran.Length > 0 ? false : true);

                //    strMessage = partTran;
                //}
                //else
                //{
                //    strMessage = "Unable to perform job receipt to inventory.";
                //    IsEpicTrxSuccess = false;
                //}

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


        public bool _EpicLotSelectUpdate(ref EpicEnv oEpicEnv, ref PartLot oPartLot, out string strMessage)
        {
            bool IsEpicLoginOK = false;
            bool IsEpicTrxSuccess = false;

            string scheme = "http";
            string strEpicSessionModSvc = oEpicEnv.Env_AppEpicor + "/" + strSessionModSvcPath;
            string strEpicLotSelectUpdateSvc = oEpicEnv.Env_AppEpicor + "/" + strLotSelectUpdateSvcPath;
            string strEpicUser = oEpicEnv.Env_AppUserId;
            string strEpicPwd = oEpicEnv.Env_AppPassKey;

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

                builder.Path = strEpicSessionModSvc;
                sessionModClient = GetClient<SessionModSvcContractClient, SessionModSvcContract>(builder.Uri.ToString(), strEpicUser, strEpicPwd, bindingType);

                sessionModClient.Endpoint.EndpointBehaviors.Add(new HookServiceBehavior(sessionId, strEpicUser));

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
                    newRow.Company = oPartLot.Company;
                    newRow.PartNum = oPartLot.PartNum;
                    newRow.LotNum = oPartLot.LotNum;
                    newRow.PartLotDescription = oPartLot.PartLotDescription;
                    newRow.Batch = oPartLot.Batch;
                    newRow.MfgBatch = oPartLot.MfgBatch;
                    newRow.MfgLot = oPartLot.MfgLot;
                    newRow.HeatNum = oPartLot.HeatNum;
                    newRow.FirmWare = oPartLot.FirmWare;
                    newRow.BestBeforeDt = oPartLot.BestBeforeDt;
                    newRow.MfgDt = oPartLot.MfgDt;
                    newRow.CureDt = oPartLot.CureDt;
                    newRow.ExpireDt = oPartLot.ExpireDt;

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








        public void Dispose(bool v)
        {
            throw new NotImplementedException();
        }

    }
}