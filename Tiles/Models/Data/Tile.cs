using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Tiles.Models.Data
{
    public class Tile
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required]
        public int Width { get; set; }

        [Required]
        public int Height { get; set; }

        [Required]
        public string PhotoUri { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string MiniDesc { get; set; }

        [Required]
        public string Uri { get; set; }
    }
}