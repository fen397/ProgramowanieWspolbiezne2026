using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ViewModel;
using Model;
using Dane;

public class MainViewModel : System.ComponentModel.INotifyPropertyChanged
{
    private readonly ModelAbstractApi _modelApi;
    private int _ballCount;
    
    public ObservableCollection<BallModel> Balls { get; } = new();
    public ICommand StartCommand { get; }

    public MainViewModel()
    {
        _modelApi = ModelAbstractApi.CreateApi();
        StartCommand = new RelayCommand(Start);
    }

    public int BallCount
    {
        get => _ballCount;
        set 
        {
            _ballCount = value;
            OnPropertyChanged();
        }
    }

    public void Start()
    {
        _modelApi.Start(BallCount);
        Balls.Clear();
        foreach (var ballModel in _modelApi.GetBalls())
        {
            Balls.Add(ballModel);
        }
     
        
    }
    public event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string name = null!) =>
        PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(name));
}