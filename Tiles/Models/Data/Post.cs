using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Tiles.Models.Data
{
    public class Post
    {
        public Post()
        {
        }

        public Post(string title, string content, DateTime publishDate, List<string> tags)
        {
            Title = title;
            Content = content;
            PublishDate = publishDate;
            Tags = tags;
        }

        

        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime PublishDate { get; set; }

        public List<string> Tags { get; set; }
    }
}
