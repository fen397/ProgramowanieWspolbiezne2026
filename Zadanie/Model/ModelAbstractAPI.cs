namespace Model;
using Logika;
using Dane;
using System.Linq;

public abstract class ModelAbstractApi
{
    public abstract void Start(int ballCount);
    public abstract void Stop();

    public abstract IEnumerable<BallModel> GetBalls();

    public static ModelAbstractApi CreateApi(LogicAbstractApi? logicApi = null)
    {
        return new ModelApi(logicApi ?? LogicAbstractApi.CreateApi());
    }
    
    
}

internal class ModelApi : ModelAbstractApi
{
    private readonly LogicAbstractApi _logicApi;

    private readonly List<BallModel> _ballModels = new List<BallModel>();

    public ModelApi(LogicAbstractApi logicApi)
    {
        _logicApi = logicApi;
    }

    public override void Start(int ballCount)
    {
        _logicApi.Stop();
        
        _logicApi.CreateBalls(ballCount);
        
        _ballModels.Clear();

        foreach (var ball in _logicApi.GetBalls())
        {
            _ballModels.Add(new BallModel(ball));
        }
        _logicApi.Start();
    }
    
    public override void Stop() => _logicApi.Stop();
    public override IEnumerable<BallModel> GetBalls() => _ballModels;
}