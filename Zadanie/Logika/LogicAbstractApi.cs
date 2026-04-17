using Dane;
using System.Collections.Generic;

namespace Logika;

public abstract class LogicAbstractApi
{
    public abstract void CreateBalls(int count);
    public abstract void Start();
    public abstract void Stop();
    public abstract IEnumerable<Ball> GetBalls();

    public static LogicAbstractApi CreateApi(DataAbstractApi data = null)
    {
        return new LogicApi(data ?? DataAbstractApi.CreateApi());
    }


}