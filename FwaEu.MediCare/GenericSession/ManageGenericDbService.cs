using FwaEu.Fwamework.Users;
using FwaEu.MediCare.GenericRepositorySession;
using FwaEu.MediCare.Orders.Services;
using FwaEu.MediCare.Users;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace FwaEu.MediCare.GenericSession
{
    public class ManageGenericDbService : IManageGenericDbService
    {
        public string SelectedDbName = null;


        public string GetGenericDb()
        {
            return SelectedDbName;
        }

        public void OnChangeGenericDb(string selectedDbName)
        {
            SelectedDbName = selectedDbName;
        }
    }
}