using System.ComponentModel;
using Dane;

namespace Model;

public class BallModel : INotifyPropertyChanged
{
    private readonly Ball _ball;
    
    private readonly double _scale = 8.0;
    
    public BallModel(Ball ball)
    {
        _ball = ball;
        _ball.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(Ball.X)) OnPropertyChanged(nameof(X));
            if (e.PropertyName == nameof(Ball.Y)) OnPropertyChanged(nameof(Y));
            if (e.PropertyName == nameof(Ball.Radius)) OnPropertyChanged(nameof(Diameter));
        };
    }
    
    public double X => (_ball.X - _ball.Radius) * _scale;
    
    public double Y => (_ball.Y - _ball.Radius) * _scale;
    
    public double Diameter => (_ball.Radius * 2) * _scale;

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}