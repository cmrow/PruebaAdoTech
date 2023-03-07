using Microsoft.EntityFrameworkCore;
using PruebaAdoTech.Data;
using PruebaAdoTech.Modelos;
using PruebaAdoTech.Repositorio.IRepositorio;

namespace PruebaAdoTech.Repositorio
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _db;
        private DbSet<TEntity> _DbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            _DbSet = _db.Set<TEntity>();
        }

        public IEnumerable<TEntity> Get() => _DbSet.ToList();


        public TEntity Get(int id) => _DbSet.Find(id);


        public void Add(TEntity data) => _DbSet.Add(data);


        public void Delete(int id)
        {
            var dataDelete = _DbSet.Find(id);
            _DbSet.Remove(dataDelete);
        }

        public void Update(TEntity data)
        {
            _DbSet.Attach(data);
            _db.Entry(data).State = EntityState.Modified;
        }


        public void Save() => _db.SaveChanges();

        
    }
}
