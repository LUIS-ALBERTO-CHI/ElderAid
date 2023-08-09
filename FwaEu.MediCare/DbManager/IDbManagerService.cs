using FwaEu.Fwamework.Data.Database.Sessions;
using System.Threading.Tasks;

namespace FwaEu.MediCare.DbManager
{
    public interface IDbManagerService
    {
        public Task SetDataSession();
    }
}
