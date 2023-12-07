namespace NLayer.Core.UnitOfWorks
{
    public interface IUnitOfWork
    {
        Task CommitAsync(); //implemente ettiğimizde  DBContextin saveChangesAsync(); metoduna karşılık gelecek.

        void Commit(); //implemente ettiğimizde  DBContextin saveChanges(); metoduna karşılık gelecek
    }
}