namespace FwaEu.MediCare.GenericSession
{
    public interface IManageGenericDbService
    {
        public string GetGenericDb();
        public void OnChangeGenericDb(string selectedDbName);
    }
}
