namespace Logika;

public class Delta
{
    private double _a;
    private double _b;
    private double _c;
    
    public Delta(double a, double b, double c)
    {
        this._a = a;
        this._b = b;
        this._c = c;
    }
    
    

    public double A => _a;

    public double B => _b;

    public double C => _c;

    public double A1
    {
        get => _a;
        set => _a = value;
    }

    public double B1
    {
        get => _b;
        set => _b = value;
    }

    public double C1
    {
        get => _c;
        set => _c = value;
    }
    
    
    public double CalculateDelta()
    {
        return Math.Pow(_b, 2) - 4 * _a * _c;
    }
    
    
    public double CalculateX1()
    {
        double delta = CalculateDelta();
        if (delta < 0)
        {
            throw new InvalidOperationException("Delta jest ujemna, brak pierwiastków rzeczywistych.");
        }
        return (-_b - Math.Sqrt(delta)) / (2 * _a);
    }
    
    public double CalculateX2()
    {
        double delta = CalculateDelta();
        if (delta < 0)
        {
            throw new InvalidOperationException("Delta jest ujemna, brak pierwiastków rzeczywistych.");
        }
        return (-_b + Math.Sqrt(delta)) / (2 * _a);
    }
    
}