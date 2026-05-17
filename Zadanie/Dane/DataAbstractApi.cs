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
        _board = new Board(100, 75);
    }

    public override Board GetBoard()
    {
        return _board;
    }
    
    public override void CreateBalls(int count)
    {
        _balls.Clear();
        for (int i = 0; i < count; i++)
        {
            
            double radius = _random.NextDouble() * (3.5 - 1.5) + 1.5;  // Promień od 1.5 do 3.5
            //Obliczamy granice aby kula nie pojawiłą sie w ścianie
            double minX = radius;
            double maxX = _board.Width - radius;
            double minY = radius;
            double maxY = _board.Height - radius;
            
            double x = _random.NextDouble() * (maxX - minX) + minX;
            double y = _random.NextDouble() * (maxY - minY) + minY;
            
            double vx = (_random.NextDouble() * 1.0) - 0.5;
            double vy = (_random.NextDouble() * 1.0) - 0.5;
            
            if (Math.Abs(vx) < 0.1) vx = 0.2;
            if (Math.Abs(vy) < 0.1) vy = 0.2;
            
            double mass = radius * radius;

            _balls.Add(new Ball
            {
                X = x,
                Y = y,
                Radius = radius,
                Mass = mass,
                VX = vx,
                VY = vy
            });
            
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