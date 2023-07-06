namespace FwaEu.MediCare.GenericSession
{
    public interface IManageGenericDbService
    {
        public string GetGenericDb();
        public int GetGenericDbId();
        public void OnChangeGenericDb(int selectedDbId, string selectedDbName);
    }
}
