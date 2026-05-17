using System.Collections.Specialized;
using System.ComponentModel;

namespace Dane;

public class Ball : INotifyPropertyChanged
{
    private double _x;
    private double _y;
    private double _radius;
    private double _mass;
    
    
    public double VX { get; set; }
    public double VY { get; set; }
    
    

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

    public double Mass
    {
        get => _mass;
        set {;
            if (value != _mass)
                {
                _mass = value;
                OnPropertyChanged();
                }
        }
    }

    //Mechanizm współbierzności 
    private Task? _moveTask;
    public bool _stopRequested;

    
    //Metoda uruchamiająca niezależny ruch kuli
    public void StartMovement()
    {
        _stopRequested = false;
        // Taks.Run uruchamia nowy wątek, który będzie wykonywał ruch kuli
        _moveTask = Task.Run(async () =>
        {
            while (!_stopRequested)
            {
                X += VX;
                Y += VY;
                // Odpoczynek zadania na 16 milisekund (zapewnia płynność ok. 60 klatek na sekundę) i pozwala innym wątkom na dostęp do procesora
                await Task.Delay(16);
            }

        });
    }
    
    public void StopMovement()
    {
        _stopRequested = true;
    }
    
    
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
}