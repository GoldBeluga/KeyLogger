using System.Net.Http.Json;
using SharedDataTypes;
using Microsoft.Win32;

namespace Install_Verification;

public class Verify
{
    private static readonly HttpClient SharedTarget = new() { BaseAddress = new Uri("http://localhost:5001"), }; // OR insert your desired address.
    private const string SubKey = @"Software\KeyLogger\Auth"; // change the name accordingly.

    public static async Task Main()
    {
        try
        {
            int EnteredID = int.Parse(Console.ReadLine() ?? "");
            string EnteredPassword = Console.ReadLine() ?? "";

            RequestRegister Body = new(EnteredID, EnteredPassword);
            HttpResponseMessage Response = await SharedTarget.PostAsJsonAsync("/register", Body);
            ResponseRegister Data = await Response.Content.ReadFromJsonAsync<ResponseRegister>() ?? throw new Exception();
            Response.EnsureSuccessStatusCode();

            SaveData("AuthToken", Data.AuthToken);
            SaveData("ID", EnteredID.ToString());

            Console.WriteLine("Succeed");
        }
        catch
        {
            Console.WriteLine("Fail");
        }
        finally
        {
            Console.ReadKey();
        }

    }
    public static void SaveData(string ValueName, string Value)
    {
        try
        {
            using RegistryKey Key = Registry.CurrentUser.CreateSubKey(SubKey);
            if (Key is not null)
            {
                Key.SetValue(ValueName, Value, RegistryValueKind.String);
            }
        }
        finally { }
    }
}