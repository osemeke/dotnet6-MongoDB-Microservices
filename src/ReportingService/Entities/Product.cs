using MongoDB.Bson.Serialization.Attributes;

namespace ReportingService.Entities
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Photo { get; set; }
        public int CategoryId { get; set; }
    }
}
