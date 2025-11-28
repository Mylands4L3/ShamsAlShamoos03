

namespace ShamsAlShamoos01.Infrastructure.Persistence.Repositories
{
    public interface IEntityDataBaseTransaction:IDisposable
    {
        void Commit();
        void RollBack();
    }
}
