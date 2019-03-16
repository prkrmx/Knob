using KnobUC.Control;
using KnobUI.Helpers;
using MahApps.Metro;
using System.Windows;

namespace KnobUI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Fields
        private Accent _SelectedAccent = ThemeManager.GetAccent("Lime");
        private bool _SelectedDark = true;
        private double _Value;

        #endregion

        #region Properties


        public double Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                OnPropertyChanged("Value");
            }
        }
        public Accent SelectedAccent
        {
            get { return _SelectedAccent; }
            set
            {
                _SelectedAccent = value;
                var theme = ThemeManager.DetectAppStyle(Application.Current);
                ThemeManager.ChangeAppStyle(Application.Current, _SelectedAccent, theme.Item1);
                Application.Current.MainWindow.Activate();
                OnPropertyChanged("SelectedAccent");
            }
        }
        public bool SelectedDark
        {
            get { return _SelectedDark; }
            set
            {
                _SelectedDark = value;
                var theme = ThemeManager.DetectAppStyle(Application.Current);

                if (_SelectedDark)
                    ThemeManager.ChangeAppStyle(Application.Current, theme.Item2, ThemeManager.GetAppTheme("DarkTheme"));
                else
                    ThemeManager.ChangeAppStyle(Application.Current, theme.Item2, ThemeManager.GetAppTheme("LightTheme"));
                OnPropertyChanged("SelectedDark");
            }
        }
        public RelayCommand Loaded { get; set; }

        #endregion

        #region Constructor
        public MainViewModel()
        {
            Loaded = new RelayCommand(OnLoaded);

        }

        #endregion

        #region Commands Methods
        private void OnLoaded()
        {

        }
        #endregion
    }
}