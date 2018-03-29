using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;
using Tiles.Models.Data;

namespace Tiles.Models
{
    public class PostsRepository
    {
        public IMongoDatabase Database { get; private set; }

        public PostsRepository(IMongoDatabase database)
        {
            Database = database;
        }

        private IMongoCollection<Post> Posts => Database.GetCollection<Post>("Posts");

        public async Task<Post> GetPost(string id)
        {
            return await Posts.Find(new BsonDocument("_id", new ObjectId(id))).FirstOrDefaultAsync();
        }

        public void SavePost(Post post)
        {
            Posts.InsertOne(post);
        }

        public IEnumerable<Post> AllPosts => Posts.Find(_ => true).ToList();
    }
}
