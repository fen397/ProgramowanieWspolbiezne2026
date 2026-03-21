using JetBrains.Annotations;
using Logika;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
namespace Tests;

[TestClass]
[TestSubject(typeof(Delta))]
public class DeltaTest
{

    [TestMethod]
    public void TestWhenZeroIsGreaterThanDelta()
    {
        Delta delta = new Delta(1, 2, 3);
        double result = delta.CalculateDelta();
        Assert.IsTrue(result < 0);
        var exception = Assert.Throws<InvalidOperationException>(() => delta.CalculateX1());
        Assert.AreEqual("Delta jest ujemna, brak pierwiastków rzeczywistych.", exception.Message);
        var exception1 = Assert.Throws<InvalidOperationException>(() => delta.CalculateX2());
        Assert.AreEqual("Delta jest ujemna, brak pierwiastków rzeczywistych.", exception1.Message); 

    }

    [TestMethod]
    public void TestWhenZeroIsEqualToDelta()
    {
        Delta delta = new Delta(-2, 4, -2);
        double result = delta.CalculateDelta();
        Assert.IsTrue(result == 0);
        
        double x1 = delta.CalculateX1();
        double x2 = delta.CalculateX2();
        Assert.AreEqual(x1, x2);
    }
    
    [TestMethod]
    public void TestWhenZeroIsLessThanDelta()
    {
        Delta delta = new Delta(1, -3, 2);
        double result = delta.CalculateDelta();
        Assert.IsTrue(result > 0);
        
        double x1 = delta.CalculateX1();
        double x2 = delta.CalculateX2();
        Assert.AreEqual(1, x1);
        Assert.AreEqual(2, x2);
    }

    [TestMethod]
    public void TestGetters()
    {
        Delta delta = new Delta(1, -3, 2);
        Assert.AreEqual(1, delta.A);
        Assert.AreEqual(-3, delta.B);
        Assert.AreEqual(2, delta.C);
    }
    
    [TestMethod]
    public void TestSetters()
    {
        Delta delta = new Delta(1, -3, 2);
        Assert.AreEqual(1, delta.A);
        Assert.AreEqual(-3, delta.B);
        Assert.AreEqual(2, delta.C);
        delta.A1 = 2;
        delta.B1 = -4;
        delta.C1 = 2;
        Assert.AreEqual(2, delta.A1);
        Assert.AreEqual(-4, delta.B1);
        Assert.AreEqual(2, delta.C1);
        
    }
}