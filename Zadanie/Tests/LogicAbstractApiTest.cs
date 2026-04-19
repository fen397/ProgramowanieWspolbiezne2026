using System.Linq;
using JetBrains.Annotations;
using Logika;
using Dane;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests;

[TestClass]
[TestSubject(typeof(LogicAbstractApi))]
public class LogicAbstractApiTest
{

    [TestMethod]
    public void BallsWithinBoardBoundariesTest()
    {
        LogicAbstractApi logicApi = LogicAbstractApi.CreateApi();
        int ballCount = 10;
        
        logicApi.CreateBalls(ballCount);
        var balls = logicApi.GetBalls().ToList();

        foreach (var ball in balls )
        {
            Assert.IsTrue(ball.X >= 0);
            Assert.IsTrue(ball.Y >= 0);
            Assert.IsTrue(ball.X <= 800 - ball.Radius * 2);
            Assert.IsTrue(ball.Y <= 800 - ball.Radius * 2);
            
        }
    }
}