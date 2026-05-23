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
    // Kolejka bezpieczna dla wątków (bufor na wypadek wolnego dysku)
    private readonly ConcurrentQueue<string> _logQueue;
    private readonly Task _loggingTask;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly string _filePath;

    public Logger()
    {
        _logQueue = new ConcurrentQueue<string>();
        _cancellationTokenSource = new CancellationTokenSource();
        
        // 1. Definiujemy ścieżkę do folderu "Log" wewnątrz katalogu uruchomieniowego aplikacji
        string logFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
        
        // 2. Automatycznie tworzymy folder, jeśli jeszcze nie istnieje
        Directory.CreateDirectory(logFolderPath);
        
        // 3. Łączymy ścieżkę folderu z nazwą pliku logu
        _filePath = Path.Combine(logFolderPath, "diagnostics.log");
        
        // Startujemy niezależny wątek zapisu w tle
        _loggingTask = Task.Run(WriteToFileLoop);
    }

    // Metoda dla Producenta (wywoływana przez kule lub w sekcji krytycznej)
    public void LogBallState(Ball ball)
    {
        // Robimy "zdjęcie" (snapshot) danych w danym momencie.
        // Nie przekazujemy samej referencji do kuli, bo jej stan może ulec 
        // zmianie przez inny wątek, zanim Logger zdąży ją zapisać.
        var snapshot = new
        {
            Timestamp = DateTime.Now.ToString("O"), // Czas z milisekundami
            BallId = ball.GetHashCode(), // Unikalny identyfikator instancji
            X = ball.X,
            Y = ball.Y,
            VX = ball.VX,
            VY = ball.VY,
            Radius = ball.Radius,
            Mass = ball.Mass
        };

        // Serializacja do tekstu (JSON)
        string json = JsonSerializer.Serialize(snapshot);
        
        // Wrzucamy do kolejki i natychmiast wychodzimy - nie blokujemy ruchu kul!
        _logQueue.Enqueue(json);
    }

    // Metoda dla Konsumenta (działa w pętli w osobnym Tasku)
    private async Task WriteToFileLoop()
    {
        // Kluczowe: wymuszamy zapis w ASCII zgodnie z poleceniem
        using StreamWriter writer = new StreamWriter(_filePath, append: true, Encoding.ASCII);
        
        while (!_cancellationTokenSource.Token.IsCancellationRequested)
        {
            if (_logQueue.TryDequeue(out string? logEntry))
            {
                await writer.WriteLineAsync(logEntry);
            }
            else
            {
                // Jeśli kolejka jest pusta, wątek "odpoczywa", żeby nie zarzynać procesora
                await Task.Delay(10); 
            }
        }
        
        // Po zakończeniu symulacji, dopisujemy to, co ewentualnie zostało w buforze
        while (_logQueue.TryDequeue(out string? remainingLog))
        {
            await writer.WriteLineAsync(remainingLog);
        }
    }

    // Sprzątanie po zatrzymaniu symulacji
    public void Dispose()
    {
        if (!_cancellationTokenSource.IsCancellationRequested)
        {
            _cancellationTokenSource.Cancel();
            // Czekamy, aż wątek zapisujący bezpiecznie skończy pracę
            _loggingTask.Wait(); 
        }
        _cancellationTokenSource.Dispose();
    }
}