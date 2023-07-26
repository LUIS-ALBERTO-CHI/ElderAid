using FwaEu.Fwamework.Users;
using FwaEu.MediCare.GenericRepositorySession;
using FwaEu.MediCare.Users;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Temporal;
using FwaEu.MediCare.GenericSession;
using System.Net;
using System.Net.Sockets;
using FwaEu.MediCare.Orders;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace FwaEu.MediCare.Protections.Services
{
    public class ProtectionService : IProtectionService
    {
        private readonly GenericSessionContext _genericsessionContext;
        private readonly ICurrentUserService _currentUserService;
        public ProtectionService(GenericSessionContext genericSessionContext,
                                    ICurrentUserService currentUserService)
        {
            _genericsessionContext = genericSessionContext;
            _currentUserService = currentUserService;
        }

        public async Task CreateProtectionAsync(CreateProtectionModel model)
        {
            var query = "exec SP_MDC_CreateProtection :PatientId, :ArticleId, :StartDate, :StopDate, :PosologyExpression, :PosologyJSONDetails, :UserLogin, :UserIp";

            var stockedProcedure = _genericsessionContext.NhibernateSession.CreateSQLQuery(query);

            var currentUserLogin = ((IApplicationPartEntityPropertiesAccessor)this._currentUserService.User.Entity).Login;
            var currentUserIp = GetCurrentIpAddress();

            string posologyExpression = BuildStringFromDictionary(model.ArticleUnit, model.ProtectionDosages);

            var posologyJSONArray = model.ProtectionDosages.Select(kvp => new { Hour = kvp.Key.Hours, Quantity = kvp.Value }).ToList();
            string posologyJSONString = JsonConvert.SerializeObject(posologyJSONArray);

            stockedProcedure.SetParameter("PatientId", model.PatientId);
            stockedProcedure.SetParameter("ArticleId", model.ArticleId);
            stockedProcedure.SetParameter("StartDate", model.StartDate);
            stockedProcedure.SetParameter("StopDate", model.StopDate);
            stockedProcedure.SetParameter("PosologyExpression", posologyExpression);
            stockedProcedure.SetParameter("PosologyJSONDetails", posologyJSONString);
            stockedProcedure.SetParameter("UserLogin", currentUserLogin);
            stockedProcedure.SetParameter("UserIp", currentUserIp);

            await stockedProcedure.ExecuteUpdateAsync();
        }

        public async Task UpdateProtectionAsync(UpdateProtectionModel model)
        {
            var query = "exec SP_MDC_UpdateProtection :PrescriptionId, :StartDate, :StopDate, :PosologyExpression, :PosologyJSONDetails, :UserLogin, :UserIp";

            var stockedProcedure = _genericsessionContext.NhibernateSession.CreateSQLQuery(query);

            var currentUserLogin = ((IApplicationPartEntityPropertiesAccessor)this._currentUserService.User.Entity).Login;
            var currentUserIp = GetCurrentIpAddress();

            string posologyExpression = BuildStringFromDictionary(model.ArticleUnit ,model.ProtectionDosages);

            var posologyJSONArray = model.ProtectionDosages.Select(kvp => new { Hour = kvp.Key.Hours, Quantity = kvp.Value }).ToList();
            string posologyJSONString= JsonConvert.SerializeObject(posologyJSONArray);

            stockedProcedure.SetParameter("PrescriptionId", model.ProtectionId);
            stockedProcedure.SetParameter("StartDate", model.StartDate);
            stockedProcedure.SetParameter("StopDate", model.StopDate);
            stockedProcedure.SetParameter("PosologyExpression", posologyExpression);
            stockedProcedure.SetParameter("PosologyJSONDetails", posologyJSONString);
            stockedProcedure.SetParameter("UserLogin", currentUserLogin);
            stockedProcedure.SetParameter("UserIp", currentUserIp);

            await stockedProcedure.ExecuteUpdateAsync();
        }

        public async Task StopProtectionAsync(StopProtectionModel model)
        {
            var query = "exec SP_MDC_StopProtection :PrescriptionId, :StopDate, :UserLogin, :UserIp";
            var stockedProcedure = _genericsessionContext.NhibernateSession.CreateSQLQuery(query);

            var currentUserLogin = ((IApplicationPartEntityPropertiesAccessor)this._currentUserService.User.Entity).Login;
            var currentUserIp = GetCurrentIpAddress();

            stockedProcedure.SetParameter("PrescriptionId", model.ProtectionId);
            stockedProcedure.SetParameter("StopDate", model.StopDate);
            stockedProcedure.SetParameter("UserLogin", currentUserLogin);
            stockedProcedure.SetParameter("UserIp", currentUserIp);
            
            await stockedProcedure.ExecuteUpdateAsync();
        }

        static string BuildStringFromDictionary(string articleUnit, Dictionary<TimeSpan, int> protectionDosages)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(articleUnit);
            foreach (var protectionDosage in protectionDosages)
            {
                sb.Append('/');
                sb.Append(protectionDosage.Value);
                sb.Append(':');
                sb.Append(protectionDosage.Key.ToString(@"h\h"));
            }

            return sb.ToString();
        }

        public static string GetCurrentIpAddress()
        {
            IPAddress[] localIPAddresses = Dns.GetHostAddresses(Dns.GetHostName());

            IPAddress localIPAddress = localIPAddresses.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(ip));

            if (localIPAddress == null)
                localIPAddress = localIPAddresses.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);

            if (localIPAddress != null)
                return localIPAddress.ToString();
            else
                return "";
        }
    }
}