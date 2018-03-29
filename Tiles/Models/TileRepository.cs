using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Tiles.Models.Data;

namespace Tiles.Models
{
    public class TileRepository
    {
        public IMongoDatabase Database { get; private set; }

        public TileRepository(IMongoDatabase database)
        {
            Database = database;
        }

        public IMongoCollection<Tile> Tiles => Database.GetCollection<Tile>("Tiles");

        public async Task<IEnumerable<Tile>> AllTiles() => await Tiles.Find(_ => true).ToListAsync();

        public async Task SaveTile(Tile item) =>
            await Tiles.InsertOneAsync(item);

        public async Task DeleteTile(string id) =>
            await Tiles.DeleteOneAsync(t => t.Id == id);

        public async Task<Tile> FindTile(string id) =>
            await Tiles.Find(t => t.Id == id).FirstOrDefaultAsync();

        public async Task EditTile(Tile item) =>
            await Tiles.ReplaceOneAsync(t => t.Id == item.Id, item);
    }
}
