using Dane;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Logika;

internal class LogicApi : LogicAbstractApi
{
    private readonly DataAbstractApi _dataApi;
    private readonly object _lock = new object();

    public LogicApi(DataAbstractApi dataApi)
    {
        _dataApi = dataApi;
    }

    public override void CreateBalls(int count)
    {
        _dataApi.CreateBalls(count);
        foreach (var ball in _dataApi.GetBalls())
        {
            ball.PropertyChanged += OnBallPropertyChanged;
        }
    }

    public override void Start() => _dataApi.StartSimulation();
    public override void Stop() => _dataApi.StopSimulation();
    public override IEnumerable<Ball> GetBalls() => _dataApi.GetBalls();

    private void OnBallPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is Ball currentBall && (e.PropertyName == "X" || e.PropertyName == "Y"))
        {
            lock (_lock)
            {
                // Wszystkie obliczenia wykonujemy na wymiarach logicznych planszy (100 x 75)
                Board board = _dataApi.GetBoard();
                CheckWallCollision(currentBall, board);
                CheckBallCollision(currentBall);
            }
        }
    }

    private void CheckWallCollision(Ball ball, Board board)
    {
        // Zakładamy, że X i Y w klasie Ball określają ŚRODEK kuli (najwygodniejsze do fizyki i skalowania)
        
        // Odbicie od lewej lub prawej ściany logicznej
        if (ball.X - ball.Radius <= 0 && ball.VX < 0)
        {
            ball.VX *= -1;
        }
        else if (ball.X + ball.Radius >= board.Width && ball.VX > 0)
        {
            ball.VX *= -1;
        }

        // Odbicie od górnej lub dolnej ściany logicznej
        if (ball.Y - ball.Radius <= 0 && ball.VY < 0)
        {
            ball.VY *= -1;
        }
        else if (ball.Y + ball.Radius >= board.Height && ball.VY > 0)
        {
            ball.VY *= -1;
        }
    }

    private void CheckBallCollision(Ball currentBall)
    {
        foreach (var otherBall in _dataApi.GetBalls())
        {
            if (currentBall == otherBall) continue;

            // Odległość między środkami kul w układzie logicznym
            double dx = currentBall.X - otherBall.X;
            double dy = currentBall.Y - otherBall.Y;
            double distance = Math.Sqrt(dx * dx + dy * dy);

            // Warunek zderzenia: suma promieni logicznych kul
            if (distance <= currentBall.Radius + otherBall.Radius)
            {
                double velocityDifferenceX = currentBall.VX - otherBall.VX;
                double velocityDifferenceY = currentBall.VY - otherBall.VY;
                double dotProduct = velocityDifferenceX * dx + velocityDifferenceY * dy;

                // Kule zbliżają się do siebie
                if (dotProduct < 0)
                {
                    double massSum = currentBall.Mass + otherBall.Mass;
                    double distanceSquared = distance * distance;

                    double massCoef1 = (2 * otherBall.Mass) / massSum;
                    double massCoef2 = (2 * currentBall.Mass) / massSum;

                    // Obliczenie nowych wektorów prędkości po zderzeniu sprężystym
                    currentBall.VX -= massCoef1 * (dotProduct / distanceSquared) * dx;
                    currentBall.VY -= massCoef1 * (dotProduct / distanceSquared) * dy;

                    otherBall.VX -= massCoef2 * (dotProduct / distanceSquared) * (-dx);
                    otherBall.VY -= massCoef2 * (dotProduct / distanceSquared) * (-dy);
                }
            }
        }
    }
}