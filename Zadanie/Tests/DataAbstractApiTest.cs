using System.Collections.Generic;
using Dane;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests;

[TestClass]
[TestSubject(typeof(DataAbstractApi))]
public class DataAbstractApiTest
{

    [TestMethod]
    public void CreateBallsTest()
    {
        DataAbstractApi api = DataAbstractApi.CreateApi();
        
        api.CreateBalls(5);
        var balls = api.GetBalls();
        
        Assert.AreEqual(5, ((List<Ball>)balls).Count);

    }
    
    [TestMethod]
    public void BoardDimensionTest()
    {
        DataAbstractApi api = DataAbstractApi.CreateApi();

        var board = api.GetBoard();
        
        Assert.AreEqual(800, board.Width);
        Assert.AreEqual(600, board.Height);

    }
}