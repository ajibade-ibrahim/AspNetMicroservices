using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.API.Entities
{
    public class Product
    {
        public string Category { get; set; }
        public string Description { get; set; }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string ImageFile { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Summary { get; set; }
    }
}