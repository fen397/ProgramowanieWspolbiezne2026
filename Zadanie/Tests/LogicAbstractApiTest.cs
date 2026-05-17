using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logika;
using Dane;
using System.Collections.Generic;
using System.Linq;

namespace Tests;

internal class FakeDataApi : DataAbstractApi
{
    public Board FakeBoard { get; } = new Board(100, 75);
    public List<Ball> FakeBalls { get; set; } = new List<Ball>();

    public override Board GetBoard() => FakeBoard;
    public override IEnumerable<Ball> GetBalls() => FakeBalls;

    public override void CreateBalls(int count)
    {
    }

    public override void StartSimulation() { }
    public override void StopSimulation() { }
}

[TestClass]
public class LogicAbstractApiTest
{
    private FakeDataApi _fakeDataApi;
    private LogicAbstractApi _logicApi;

    [TestInitialize]
    public void Setup()
    {
        _fakeDataApi = new FakeDataApi();
        _logicApi = LogicAbstractApi.CreateApi(_fakeDataApi);
    }

    [TestMethod]
    public void WallCollision_ShouldReverseVelocityX_WhenHittingRightWall()
    {
        var ball = new Ball { X = 95, Y = 50, Radius = 5, VX = 2, VY = 0, Mass = 1 };
        _fakeDataApi.FakeBalls.Add(ball);
        
        _logicApi.CreateBalls(1); 
        
        ball.X = 101; 
        
        Assert.IsTrue(ball.VX < 0, "Prędkość VX powinna zostać odwrócona przy uderzeniu w prawą ścianę.");
    }

    [TestMethod]
    public void WallCollision_ShouldReverseVelocityY_WhenHittingTopWall()
    {
        var ball = new Ball { X = 50, Y = 6, Radius = 5, VX = 0, VY = -2, Mass = 1 };
        _fakeDataApi.FakeBalls.Add(ball);
        _logicApi.CreateBalls(1);
        
        ball.Y = -1;
        
        Assert.IsTrue(ball.VY > 0, "Prędkość VY powinna zostać odwrócona przy uderzeniu w górną ścianę.");
    }

    [TestMethod]
    public void BallCollision_ElasticCollision_ShouldChangeVelocities()
    {
        var ball1 = new Ball { X = 40, Y = 40, Radius = 5, VX = 2, VY = 0, Mass = 2 };
        var ball2 = new Ball { X = 60, Y = 40, Radius = 5, VX = -2, VY = 0, Mass = 2 };
        
        _fakeDataApi.FakeBalls.Add(ball1);
        _fakeDataApi.FakeBalls.Add(ball2);
        
        _logicApi.CreateBalls(2);
        
        ball1.X = 49; 
        ball2.X = 51;
        
        Assert.IsTrue(ball1.VX < 0, "Kula 1 powinna odbić się w lewo (ujemne VX).");
        Assert.IsTrue(ball2.VX > 0, "Kula 2 powinna odbić się w prawo (dodatnie VX).");
    }
}