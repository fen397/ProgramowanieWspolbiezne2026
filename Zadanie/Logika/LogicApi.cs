using Dane;
using System;
using System.Collections.Generic;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Logika;

internal class LogicApi : LogicAbstractApi
{
    private readonly DataAbstractApi _dataApi;
    private readonly Timer _timer;
    private readonly Random _random = new Random();
    private readonly object _lock = new object();

    public LogicApi(DataAbstractApi dataApi)
    {
        _dataApi = dataApi;
        _timer = new Timer(20);
        _timer.Elapsed += OnTimerElapsed;
    }

    public override void CreateBalls(int count)
    {
        lock (_lock)
        {
            _dataApi.CreateBalls(count);
            Board board = _dataApi.GetBoard();
            
            foreach (var ball in _dataApi.GetBalls())
            {
                int maxX = Math.Max(0, (int)board.Width - (int)(ball.Radius * 2));
                int maxY = Math.Max(0, (int)board.Height - (int)(ball.Radius *2));
                ball.X = _random.Next(0, maxX );
                ball.Y = _random.Next(0, maxY);
                
                ball.VX = _random.NextDouble() * 1.0 - 0.5;
                ball.VY = _random.NextDouble() * 1.0 - 0.5;
            }
        }
    }
    public override void Start() => _timer.Start();
    public override void Stop() => _timer.Stop();

    public override IEnumerable<Ball> GetBalls() => _dataApi.GetBalls();

    private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        lock (_lock)
        {
            Board board = _dataApi.GetBoard();
            foreach (var ball in _dataApi.GetBalls())
            {
                MoveBall(ball, board);
            }
        }
    }

    private void MoveBall(Ball ball, Board board)
    {
        double newX = ball.X + ball.VX;
        double newY = ball.Y + ball.VY;

        if (newX <= 0 || newX >= board.Width - ball.Radius * 2)
        {
            ball.VX *= -1;
        }

        if (newY <= 0 || newY >= board.Height - ball.Radius * 2)
        {
            ball.VY *= -1;
        }
        ball.X += ball.VX;
        ball.Y += ball.VY;

    }
}