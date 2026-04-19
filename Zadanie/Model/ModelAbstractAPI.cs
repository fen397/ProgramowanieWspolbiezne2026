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

    public ModelApi(LogicAbstractApi logicApi)
    {
        _logicApi = logicApi;
    }

    public override void Start(int ballCount)
    {
        _logicApi.CreateBalls(ballCount);
        _logicApi.Start();
    }
    
    public override void Stop() => _logicApi.Stop();
    public override IEnumerable<BallModel> GetBalls()
    {
        return _logicApi.GetBalls().Select(b => new BallModel(b)).ToList();
    }
}

