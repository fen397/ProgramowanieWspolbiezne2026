using Dane;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Timers;
using Timer = System.Timers.Timer;

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

        // Gdy kula przesunie się w swoim własnym Tasku, wywoła się OnBallPropertyChanged.
        foreach (var ball in _dataApi.GetBalls())
        {
            ball.PropertyChanged += OnBallPropertyChanged;
        }
    }

    public override void Start()
    {
        _dataApi.StartSimulation();
    }
    public override void Stop()
    {
        _dataApi.StopSimulation();
    }

    public override IEnumerable<Ball> GetBalls() => _dataApi.GetBalls();

    private void OnBallPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is Ball currentBall && (e.PropertyName == "X" || e.PropertyName == "Y"))
        {
            // Otwieramy SEKCJĘ KRYTYCZNĄ. 
            // Kule działają w różnych wątkach. Ten lock zapobiega sytuacji, w której
            // dwie kule naraz próbują zmienić swoje pozycje i prędkości, co mogłoby zepsuć fizykę.

            lock (_lock)
            {
                Board board = _dataApi.GetBoard();
                
                // 1. Najpierw sprawdzamy, czy kula nie uderzyła w ścianę stołu
                CheckWallCollision(currentBall, board);
                
                // 2. Następnie sprawdzamy, czy kula nie uderzyła w inne kule
                CheckBallCollision(currentBall);
            }
        }
    }



    private void CheckWallCollision(Ball currentBall, Board board)
    {
        double diameter = currentBall.Radius * 2;

        // Odbicie od lewej lub prawej ściany
        if (currentBall.X <= 0 && currentBall.VX < 0)
        {
            currentBall.VX *= -1;
        }
        else if (currentBall.X + diameter >= board.Width && currentBall.VX > 0)
        {
            currentBall.VX *= -1;
        }
        // Odbicie od górnej lub dolnej ściany
        if (currentBall.Y <= 0 && currentBall.VY < 0)
        {
            currentBall.VY *= -1;
        }
        else if (currentBall.Y + diameter >= board.Height && currentBall.VY > 0)
        {
            currentBall.VY *= -1;
        }


    }
    
    private void CheckBallCollision(Ball currentBall)
    {
        foreach (var otherBall in _dataApi.GetBalls())
        {
            if (currentBall == otherBall) continue;
            
            // Obliczamy środki obu kul (X i Y to lewy górny róg)
            double center1X = currentBall.X + currentBall.Radius;
            double center1Y = currentBall.Y + currentBall.Radius;
            
            double center2X = otherBall.X + otherBall.Radius;
            double center2Y = otherBall.Y + otherBall.Radius;
            
            
            // Różnica pozycji w osiach X i Y
            double dx = center1X - center2X;
            double dy = center1Y - center2Y;
            
            // Odległość między środkami (Twierdzenie Pitagorasa)
            double distance = Math.Sqrt(dx * dx + dy * dy);
            
            // Sprawdzamy czy doszło do kontaktu fizycznego
            if (distance <= currentBall.Radius + otherBall.Radius)
            {
                // Zabezpieczenie przed "zlepianiem się" kul (sprawdzamy kierunek pędu)
                double velocityDifferenceX = currentBall.VX - otherBall.VX;
                double velocityDifferenceY = currentBall.VY - otherBall.VY;
                
                // Iloczyn skalarny różnicy prędkości i różnicy pozycji
                double dotProduct = velocityDifferenceX * dx + velocityDifferenceY * dy;

                // Jeśli dotProduct < 0, oznacza to, że kule zbliżają się do siebie. 
                // Zmieniamy prędkości tylko wtedy, żeby kule, które się nakładają ale oddalają, mogły się gładko rozdzielić.
                if (dotProduct < 0)
                {
                    // WZÓR NA ZDERZENIE SPRĘŻYSTE 2D (ELASTIC COLLISION)
                    double massSum = currentBall.Mass + otherBall.Mass;
                    double distanceSquared = distance * distance;

                    // Współczynniki masowe
                    double massCoef1 = (2 * otherBall.Mass) / massSum;
                    double massCoef2 = (2 * currentBall.Mass) / massSum;

                    // Nowe wektory prędkości
                    currentBall.VX -= massCoef1 * (dotProduct / distanceSquared) * dx;
                    currentBall.VY -= massCoef1 * (dotProduct / distanceSquared) * dy;

                    // Dla drugiej kuli wektor odległości jest odwrotny (-dx, -dy)
                    otherBall.VX -= massCoef2 * (dotProduct / distanceSquared) * (-dx);
                    otherBall.VY -= massCoef2 * (dotProduct / distanceSquared) * (-dy);
                }
            }

        }
    }
}