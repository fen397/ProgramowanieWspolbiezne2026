using System.ComponentModel;
using Dane;

namespace Model;

public class BallModel : INotifyPropertyChanged
{
    private readonly Ball _ball;
    private readonly double _scaleX;
    private readonly double _scaleY;
    
    public BallModel(Ball ball, double scaleX = 8.0, double scaleY = 6.0)
    {
        _ball = ball;
        _scaleX = scaleX;
        _scaleY = scaleY;
        
        _ball.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(Ball.X)) OnPropertyChanged(nameof(X));
            if (e.PropertyName == nameof(Ball.Y)) OnPropertyChanged(nameof(Y));
            if (e.PropertyName == nameof(Ball.Radius)) OnPropertyChanged(nameof(Diameter));
        };
    }
    
    public double X => _ball.X * _scaleX;
    public double Y => _ball.Y * _scaleY;
    
    public double Diameter => 20;
    

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}