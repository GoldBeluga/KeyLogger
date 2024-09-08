using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Server.Model;

public record DataModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; init; } = "";
    public int TargetId { get; init; } = 0;
    public DateTime RecordedTime { get; init; }
    public string Data { get; init; } = "";
}
