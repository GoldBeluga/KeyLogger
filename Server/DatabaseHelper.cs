using MongoDB.Driver;

namespace Server;

public static class DatabaseHelper
{
    public static IMongoCollection<T> ConnectToMongo<T>(string Collection, string ConnectionString)
    {
        return new MongoClient(ConnectionString)
            .GetDatabase("KeyLoggerDB")
            .GetCollection<T>(Collection);
    }

    public static async Task ReplaceData<TDocument, TVariable>(this IMongoCollection<TDocument> Collection, string IdentificationVariableName, TVariable VariableData, TDocument NewData)
    {
        await Collection
           .ReplaceOneAsync(Builders<TDocument>
           .Filter.Eq(IdentificationVariableName, VariableData)
           , NewData, new ReplaceOptions { IsUpsert = true });
    }
}
