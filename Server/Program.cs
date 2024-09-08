using MongoDB.Driver;
using Server.Model;
using Server.Model.Admin;
using SharedDataTypes;
using System.Security.Cryptography;
using System.Text;

namespace Server;

public class Program
{
    public async static Task Main()
    {
        var builder = WebApplication.CreateBuilder();

        var app = builder.Build();

        string ConnectionString = "mongodb://mongo:27017";

        var Admin = DatabaseHelper.ConnectToMongo<AdminCrediantial>("Admin", ConnectionString);
        var User = DatabaseHelper.ConnectToMongo<UserCrediantial>("User", ConnectionString);

        if ((await Admin.FindAsync(x => true)).ToList().FirstOrDefault() is null)
        {
            string TheSalt = GenerateToken();
            var AdminBody = new AdminCrediantial()
            {
                Salt = TheSalt,
                HashedPassword = Hash("Password", TheSalt)
            };
            await Admin.InsertOneAsync(AdminBody);
        }

        app.MapGet("/", () => "Connected!");

        app.MapPost("/register", async (RequestRegister Body) =>
        {
            if (Body is not null)
            {
                var x = (await Admin.FindAsync(x => true)).ToList().FirstOrDefault() ?? new();
                if (x.HashedPassword == Hash(Body.Password, x.Salt))
                {
                    var y = (await User.FindAsync(x => x.TargetId == Body.Id)).ToList().FirstOrDefault();

                    if (y is null)
                    {
                        string PlainToken = GenerateToken();
                        string Salt = GenerateToken();

                        await User.InsertOneAsync(new UserCrediantial()
                        {
                            Salt = Salt,
                            HashedToken = Hash(PlainToken, Salt),
                            TargetId = Body.Id
                        });
                        return Results.Accepted(value: new ResponseRegister(PlainToken));
                    }
                }
            }
            return Results.BadRequest();
        });

        app.MapPost("/send", async (ReceivedData Body) =>
        {
            if (Body is not null)
            {
                var x = (await User.FindAsync(x => x.TargetId == Body.TargetId)).ToList().FirstOrDefault() ?? new();

                if (x.HashedToken == Hash(Body.AuthToken, x.Salt))
                {
                    var Data = DatabaseHelper.ConnectToMongo<DataModel>(Body.TargetId.ToString(), ConnectionString);
                    var Value = new DataModel()
                    {
                        TargetId = Body.TargetId,
                        Data = Body.PureData,
                        RecordedTime = Body.StartedTime
                    };
                    await Data.InsertOneAsync(Value);
                    return Results.Accepted();
                }
            }
            return Results.BadRequest();
        });

        app.Run();
    }

    public static string GenerateToken(int ByteValue = 32)
    {
        using RandomNumberGenerator rng = RandomNumberGenerator.Create();
        byte[] data = new byte[ByteValue];
        rng.GetBytes(data);
        return Convert.ToBase64String(data);
    }

    public static string Hash(string Plain, string Salt) => Convert.ToBase64String(SHA256.HashData(Encoding.ASCII.GetBytes(Plain + Salt)));
}