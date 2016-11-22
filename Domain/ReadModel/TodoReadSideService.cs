using Domain.Core.Persistance;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ReadModel
{
    public interface ITodoReadSideService
    {
        IEnumerable<TodoDetail> GetAll();
        IEnumerable<TodoDetail> GetAllActive();
        IEnumerable<TodoDetail> GetAllInactive();
        TodoDetail GetDetails(Guid id);
    }

    public class TodoReadSideService : ITodoReadSideService
    {
        private readonly IMongoConnection _db;

        public TodoReadSideService(IMongoConnection db)
        {
            _db = db;
        }
        public IEnumerable<TodoDetail> GetAll()
        {
            return _db.Database.GetCollection<TodoDetail>("todos").AsQueryable();
        }

        public IEnumerable<TodoDetail> GetAllActive()
        {
            return _db.Database.GetCollection<TodoDetail>("todos").AsQueryable().Where(x => x.IsActive == true);
        }

        public IEnumerable<TodoDetail> GetAllInactive()
        {
            return _db.Database.GetCollection<TodoDetail>("todos").AsQueryable().Where(x => x.IsActive == false);
        }

        public TodoDetail GetDetails(Guid id)
        {
            return _db.Database.GetCollection<TodoDetail>("todos").AsQueryable().First(x => x.UUID == id);
        }
    }
}
