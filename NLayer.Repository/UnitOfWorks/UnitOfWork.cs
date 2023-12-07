using NLayer.Core.UnitOfWorks;

namespace NLayer.Repository.UnitOfWorks
{
    //bu sınıfı core katmanındaki interfaceden implemente ettik bu design patterni kullanım amacı savechanges metodlarına erişim kısıtlaması getirmek.
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}