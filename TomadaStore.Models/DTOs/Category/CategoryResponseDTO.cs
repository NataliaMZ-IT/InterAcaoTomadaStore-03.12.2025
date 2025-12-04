using MongoDB.Bson.Serialization.Attributes;

namespace TomadaStore.Models.DTOs.Category
{
    public class CategoryResponseDTO
    {
        [BsonElement("id")]
        public string Id { get; init; }
        [BsonElement("name")]
        public string Name { get; init; }
        [BsonElement("description")]
        public string Description { get; init; }
    }
}
