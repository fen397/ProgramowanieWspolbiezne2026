namespace Dane;

public abstract class DataAbstractApi
{
    public abstract Board GetBoard();
    
    public abstract void CreateBalls(int count);
    public abstract IEnumerable<Ball> GetBalls();

    public abstract void StartSimulation();
    
    public abstract void StopSimulation();
    
    public static DataAbstractApi CreateApi()
    {
        return new DataApi();
    }

}

internal class DataApi : DataAbstractApi
{
    private readonly List<Ball> _balls = new List<Ball>();
    private readonly Board _board;
    
    private readonly Random _random = new Random();

    public DataApi()
    {
        _board = new Board(100, 100);
    }

    public override Board GetBoard()
    {
        return _board;
    }
    
    public override void CreateBalls(int count)
    {
        _balls.Clear();
        double radius = 10.0;
        for (int i = 0; i < count; i++)
        {
            //Obliczamy granice aby kula nie pojawiłą sie w ścianie
            int minX = (int)radius;
            int maxX = _board.Width - (int)radius;
            int minY = (int)radius;
            int maxY = _board.Height - (int)radius;

            Ball newBall = new Ball
            {
                X = _random.Next(minX, maxX),
                Y = _random.Next(minY, maxY),
                Radius = radius,

                //Losowa masa od 1.0 do 5.0
                Mass = _random.NextDouble() * 4.0 + 1.0,

                VX = (_random.NextDouble() * 4.0) - 2.0, // Prędkość od -2.0 do 2.0
                VY = (_random.NextDouble() * 4.0) - 2.0

            };

            if (Math.Abs(newBall.VX) < 0.5) newBall.VX = 1.0;
            if (Math.Abs(newBall.VY) < 0.5) newBall.VY = 1.0;
            
            _balls.Add(newBall);
        }
    }
    
    
    public override IEnumerable<Ball> GetBalls()
         {
             return _balls;
         }
    
    public override void StartSimulation()
    {
        foreach (var ball in _balls)
        {
            ball.StartMovement();
        }
    }
    
    public override void StopSimulation()
    {
        foreach (var ball in _balls)
        {
            ball.StopMovement();
        }
    }
    
}