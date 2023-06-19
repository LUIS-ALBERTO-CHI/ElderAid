using FwaEu.Fwamework.Data.Database.Sessions;

namespace FwaEu.MediCare.GenericRepositorySession
{
    public interface IGenericRepositorySessionService
    {
        public RepositorySession<IStatefulSessionAdapter> GetRepositorySession();
    }
}