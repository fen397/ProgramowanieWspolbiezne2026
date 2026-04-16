namespace Dane;

public abstract class DataAbstractApi
{
    public abstract Board GetBoard();
    
    public abstract void CreateBalls(int count);
    public abstract IEnumerable<Ball> GetBalls();

    public static DataAbstractApi CreateApi()
    {
        return new DataApi();
    }

}

internal class DataApi : DataAbstractApi
{
    private readonly List<Ball> _balls = new List<Ball>();
    private readonly Board _board;

    public DataApi()
    {
        _board = new Board(800, 600);
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
            _balls.Add(new Ball{X = 0, Y = 0, Radius = 10});
        }
    }
    public override IEnumerable<Ball> GetBalls()
    {
        return _balls;
    }
}