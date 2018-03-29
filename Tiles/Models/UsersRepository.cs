using System.Threading.Tasks;
using MongoDB.Driver;
using Tiles.Models.Data;

namespace Tiles.Models
{
    public class UsersRepository
    {
        public IMongoDatabase Database { get; private set; }

        public UsersRepository(IMongoDatabase database)
        {
            Database = database;
        }

        public IMongoCollection<User> Users => Database.GetCollection<User>("Users");

        public async Task<User> GetUser(string login) => await Users.Find(t => t.Login == login).FirstOrDefaultAsync();

        public async Task SaveUser(User user) => await Users.InsertOneAsync(user);
    }
}
