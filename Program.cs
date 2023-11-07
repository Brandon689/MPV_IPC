using System;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;

class MpvIpcExample
{
    private static async Task SendCommandAsync(NamedPipeClientStream pipeStream, string command)
    {
        // Ensure the command is in JSON format and terminated with a newline
        byte[] buffer = Encoding.UTF8.GetBytes(command + "\n");
        await pipeStream.WriteAsync(buffer, 0, buffer.Length);
        await pipeStream.FlushAsync();

        Console.WriteLine("Command sent: " + command);
    }

    public static async Task Main(string[] args)
    {
        try
        {
            using (NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "mpv-pipe", PipeDirection.Out))
            {
                Console.WriteLine("Connecting to MPV IPC...");
                pipeClient.Connect(); // Connect to the pipe
                Console.WriteLine("Connected to MPV IPC.");

                // Example command to set the volume
                string setVolumeCommand = "{\"command\": [\"set_property\", \"volume\", 75]}";
                await SendCommandAsync(pipeClient, setVolumeCommand);

                Console.WriteLine("Commands executed. Exiting...");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
