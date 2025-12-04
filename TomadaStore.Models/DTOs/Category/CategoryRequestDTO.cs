using MongoDB.Bson.Serialization.Attributes;

namespace TomadaStore.Models.DTOs.Category
{
    public class CategoryRequestDTO
    {
        [BsonElement("name")]
        public string Name { get; init; }
        [BsonElement("description")]
        public string Description { get; init; }
    }
}
