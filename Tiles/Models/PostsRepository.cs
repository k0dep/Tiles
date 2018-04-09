using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;
using Tiles.Models.Data;

namespace Tiles.Models
{
    public class PostsRepository
    {
        public IMongoDatabase Database { get; }

        public PostsRepository(IMongoDatabase database) => 
            Database = database;

        private IMongoCollection<Post> Posts => 
            Database.GetCollection<Post>("Posts");

        public async Task<Post> GetPost(string id) => 
            await Posts.Find(new BsonDocument("_id", new ObjectId(id))).FirstOrDefaultAsync();

        public async Task SavePost(Post post) => 
            await Posts.InsertOneAsync(post);

        public async Task UpdatePost(Post post) =>
            await Posts.ReplaceOneAsync(t => t.Id == post.Id, post);

        public IEnumerable<Post> AllPosts => 
            Posts.Find(_ => true).ToList();
    }
}
