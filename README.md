# Disclaimer
The information and malware code provided on this [website/blog/publication] is for **educational and research purposes only**. The intent of this content is to aid cybersecurity professionals, developers, and researchers in understanding malware behavior, improving defense mechanisms, and furthering security knowledge.

The use of this malware, or any information provided herein, for illegal or unethical purposes, including but not limited to hacking, unauthorized access, or exploitation, is strictly prohibited. The publisher does not endorse, encourage, or condone any such activities.

The publisher is **not responsible for any misuse, harm, or damage** that may result from the use or application of the information or tools provided. It is the responsibility of the reader to ensure their actions comply with all applicable laws and regulations.

By accessing or using this content, you agree to use it responsibly and at your own risk.
## Welcome to my Key Logger
This Key Logger is written in C# [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0), and consists a client (the key logger itself), register (to register the target) and the server. Some partial of codes are from [here](https://medium.com/@davho/c-keyloggers-using-windows-api-d53eafcd48b). This key logger will only work on Windows.
## Prerequisites

 1. [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) and [docker desktop](https://www.docker.com/products/docker-desktop/) installed.
 2. You have full access to your target machines.
 
## How to use it?
After installing .NET 8 and docker desktop, navigate to the `Server` directory, run these commands to deploy server.

 1. `dotnet build`
 2. `dotnet publish -c Release`
 3. `docker compose up --build`
 
In default,  port `5001` will be exposed as the endpoint of the server. Additionaly, You may also setup cloudflare tunnel OR NGINX to expose it to the public.

Navigate to the `Key Logger` and the `Register` directory and run this command respectively.

    dotnet publish -c Release -r win-x64 --self-contained
Run the `register.exe` to register your target with your server. Enter the `target id` (integer), then press `enter`. In the second line, enter the `password` to obtain access to upload data.
 
 After registering your target with your server, you can run the `KeyLogger.exe`.
 
 ### This key logger consists three components
 **Key Logger**, the key logger itself, to get confidental data.
 **Register**, to register the target with the server, and to obtain an `auth token`.
 **Server**, to listen to all posted information.
 

