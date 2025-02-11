using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FreeCourse.Services.Catelog.Models
{   // mongodb nın veritabanda ki karsılıkları
    public class Course
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public int Id { get; set; }
        public int Name { get; set; }
        public string Description { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; set; }
        public string UserId { get; set; }
        public string Picture { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedTime { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime UpdateTime { get; set; }

        public Feature Feature { get; set; }
        public string CategoryId { get; set; }

        [BsonIgnore] // veritabanda karsılıgı yok, kodelamada kullanıyoruz
        public Category Category { get; set; }
    }
}
