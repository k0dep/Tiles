using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Tiles.Models.Data
{
    public class Post
    {
        public Post()
        {
        }

        public Post(string title, string content, DateTime publishDate)
        {
            Title = title;
            Content = content;
            PublishDate = publishDate;
        }

        

        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required(ErrorMessage = "Требуется заголовок")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Содержимое обязательно")]
        public string Content { get; set; }

        public string ShortDescription { get; set; }

        public DateTime PublishDate { get; set; }

        public List<string> Tags { get; set; }
    }
}
