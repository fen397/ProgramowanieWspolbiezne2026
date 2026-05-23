using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Dane;

internal class Logger : IDisposable
{
    private readonly ConcurrentQueue<string> _logQueue;
    private readonly Task _loggingTask;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly string _filePath;

    public Logger()
    {
        _logQueue = new ConcurrentQueue<string>();
        _cancellationTokenSource = new CancellationTokenSource();
        
        string logFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
        
        Directory.CreateDirectory(logFolderPath);
        
        _filePath = Path.Combine(logFolderPath, "diagnostics.log");
        _loggingTask = Task.Run(WriteToFileLoop);
    }
    
    public void LogBallState(Ball ball)
    {
        var snapshot = new
        {
            Timestamp = DateTime.Now.ToString("O"),
            BallId = ball.GetHashCode(),
            X = ball.X,
            Y = ball.Y,
            VX = ball.VX,
            VY = ball.VY,
            Radius = ball.Radius,
            Mass = ball.Mass
        };

        // Serializacja do tekstu
        string json = JsonSerializer.Serialize(snapshot);
        
        _logQueue.Enqueue(json);
    }
    
    private async Task WriteToFileLoop()
    {
        using StreamWriter writer = new StreamWriter(_filePath, append: true, Encoding.ASCII);
        
        while (!_cancellationTokenSource.Token.IsCancellationRequested)
        {
            if (_logQueue.TryDequeue(out string? logEntry))
            {
                await writer.WriteLineAsync(logEntry);
            }
            else
            {
                await Task.Delay(10); 
            }
        }
        
        while (_logQueue.TryDequeue(out string? remainingLog))
        {
            await writer.WriteLineAsync(remainingLog);
        }
    }
    public void Dispose()
    {
        if (!_cancellationTokenSource.IsCancellationRequested)
        {
            _cancellationTokenSource.Cancel();
            _loggingTask.Wait(); 
        }
        _cancellationTokenSource.Dispose();
    }
}