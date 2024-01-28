using System;
using System.Threading.Tasks;

namespace FwaEu.ElderAid.GenericSession
{
    public class ManageGenericDbService : IManageGenericDbService
    {
        public string SelectedDbName = null;
        public int SelectedDbId = 0;

        public string GetGenericDb()
        {
            return SelectedDbName;
        }

        public int GetGenericDbId()
        {
            return SelectedDbId;
        }

        public void OnChangeGenericDb(int selectDbId, string selectedDbName)
        {
            SelectedDbId = selectDbId;
            SelectedDbName = selectedDbName;
        }
    }
}