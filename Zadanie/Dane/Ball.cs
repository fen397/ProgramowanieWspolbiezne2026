using System.Collections.Specialized;
using System.ComponentModel;

namespace Dane;

public class Ball : INotifyPropertyChanged
{
    private double _x;
    private double _y;
    private double _radius;

    public double X
    {
        get => _x;
        set
        {
            if (value != _x)
            {
                _x = value;
                OnPropertyChanged();
            }
        }
    }
    
    public double Y
    {
        get => _y;
        set
        {
            if (value != _y)
                {
                _y = value;
                OnPropertyChanged();
                }
        }
    }
    public double Radius
    {
        get => _radius;
        set
        {
            if (value != _radius)
                {
                _radius = value;
                OnPropertyChanged();
                }
                
        }
    }
    
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
}