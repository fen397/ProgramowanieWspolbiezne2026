using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dane;
using System.Linq;
using System.Threading;

namespace Tests;

[TestClass]
public class DataAbstractApiTest
{
    private DataAbstractApi _dataApi;
    [TestInitialize]
    public void Setup()
    {
        _dataApi = DataAbstractApi.CreateApi();
    }

    [TestMethod]
    public void CreateBalls_ShouldGenerateCorrectNumberOfBalls()
    {
        _dataApi.CreateBalls(5);
        var balls = _dataApi.GetBalls().ToList();
        Assert.AreEqual(5, balls.Count);
    }

    [TestMethod]
    public void CreateBalls_BallsShouldHaveValidParameters()
    {
        _dataApi.CreateBalls(1);
        var ball = _dataApi.GetBalls().First();
        
        Assert.IsTrue(ball.Mass > 0, "Masa powinna być większa od zera.");
        Assert.IsTrue(ball.Radius > 0, "Promień powinien być większy od zera.");
        Assert.IsTrue(ball.VX != 0 || ball.VY != 0, "Kula nie może stać w miejscu na starcie.");
    }

    [TestMethod]
    public void StartSimulation_ShouldMoveBallsAsynchronously()
    {
        _dataApi.CreateBalls(1);
        var ball = _dataApi.GetBalls().First();
        
        double initialX = ball.X;
        double initialY = ball.Y;
        
        ball.VX = 2.0;
        ball.VY = 2.0;
        
        _dataApi.StartSimulation();
        
        Thread.Sleep(100); 
        
        _dataApi.StopSimulation();
        
        Assert.AreNotEqual(initialX, ball.X, "Kula powinna zmienić pozycję X.");
        Assert.AreNotEqual(initialY, ball.Y, "Kula powinna zmienić pozycję Y.");
    }
}