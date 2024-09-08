using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Server.Model.Admin;

public record AdminCrediantial
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; init; } = "";
    public string HashedPassword { get; init; } = "";
    public string Salt { get; init; } = "";
}
