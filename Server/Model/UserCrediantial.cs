using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Server.Model;

public class UserCrediantial
{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; init; } = "";
    public string HashedToken { get; init; } = "";
    public string Salt { get; init; } = "";
    public int TargetId { get; init; } = 0;
}
