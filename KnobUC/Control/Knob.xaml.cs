using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace KnobUC.Control
{
    /// <summary>
    /// Interaction logic for Knob.xaml
    /// </summary>
    public partial class Knob : UserControl, INotifyPropertyChanged
    {

        #region Fields
        private bool isMouseDown;
        private Point previousMousePosition;
        private double mouseMoveThreshold = 20;

        private double _PointerStartAngle;
        private double _PointerEndAngle;
        private double _LevelEndAngle;

        #endregion

        #region Properties

        public double PointerStartAngle
        {
            get { return _PointerStartAngle; }
            set
            {
                _PointerStartAngle = value;
                OnPropertyChanged("PointerStartAngle");
            }
        }

        public double PointerEndAngle
        {
            get { return _PointerEndAngle; }
            set
            {
                _PointerEndAngle = value;
                OnPropertyChanged("PointerEndAngle");
            }
        }

        public double LevelEndAngle
        {
            get { return _LevelEndAngle; }
            set
            {
                _LevelEndAngle = value;
                OnPropertyChanged("LevelEndAngle");
            }
        }

        private static readonly DependencyProperty TitleFontDP = DependencyProperty.Register(
            nameof(TitleFont), typeof(FontFamily), typeof(Knob));
        [Description("Gets or sets a title font."), Category("Knob")]
        public FontFamily TitleFont
        {
            get { return (FontFamily)GetValue(TitleFontDP); }
            set { SetValue(TitleFontDP, value); }
        }

        private static readonly DependencyProperty TitleFontSizeDP = DependencyProperty.Register(
            nameof(TitleFontSize), typeof(double), typeof(Knob), new PropertyMetadata(default(double)));
        [Description("Gets or sets a title font size."), Category("Knob")]
        public double TitleFontSize
        {
            get { return (double)GetValue(TitleFontSizeDP); }
            set { SetValue(TitleFontSizeDP, value); }
        }

        private static readonly DependencyProperty TitleForegroundDP = DependencyProperty.Register(
            nameof(TitleForeground), typeof(Brush), typeof(Knob));
        [Description("Gets or sets a title foreground."), Category("Knob")]
        public Brush TitleForeground
        {
            get { return (Brush)GetValue(TitleForegroundDP); }
            set { SetValue(TitleForegroundDP, value); }
        }


        private static readonly DependencyProperty TitleStyleDP = DependencyProperty.Register(
            nameof(TitleStyle), typeof(Style), typeof(Knob));
        [Description("Gets or sets a title style."), Category("Knob")]
        public Style TitleStyle
        {
            get { return (Style)GetValue(TitleStyleDP); }
            set { SetValue(TitleStyleDP, value); }
        }


        private static readonly DependencyProperty TitleVisibilityDP = DependencyProperty.Register(
            nameof(TitleVisibility), typeof(Visibility), typeof(Knob));
        [Description("Gets or sets a title visibility."), Category("Knob")]
        public Visibility TitleVisibility
        {
            get { return (Visibility)GetValue(TitleVisibilityDP); }
            set { SetValue(TitleVisibilityDP, value); }
        }


        private static readonly DependencyProperty ValueDP = DependencyProperty.Register(
            nameof(Value), typeof(double), typeof(Knob), new PropertyMetadata(default(double)));
        [Description("Gets or sets a value."), Category("Knob")]
        public double Value
        {
            get { return (double)GetValue(ValueDP); }
            set
            {
                SetValue(ValueDP, Math.Max(Math.Min(value, Maximum), Minimum));
                UpdateUI();
            }
        }

        private static readonly DependencyProperty DefaultDP = DependencyProperty.Register(
            nameof(Default), typeof(double), typeof(Knob), new PropertyMetadata(default(double)));
        [Description("Gets or sets a default value."), Category("Knob")]
        public double Default
        {
            get { return (double)GetValue(DefaultDP); }
            set { SetValue(DefaultDP, value); }
        }

        private static readonly DependencyProperty MinimumDP = DependencyProperty.Register(
            nameof(Minimum), typeof(double), typeof(Knob), new PropertyMetadata(default(double)));
        [Description("Gets or sets the minimum."), Category("Knob")]
        public double Minimum
        {
            get { return (double)GetValue(MinimumDP); }
            set { SetValue(MinimumDP, value); }
        }

        private static readonly DependencyProperty MaximumDP = DependencyProperty.Register(
            nameof(Maximum), typeof(double), typeof(Knob), new PropertyMetadata(default(double)));
        [Description("Gets or sets the maximum."), Category("Knob")]
        public double Maximum
        {
            get { return (double)GetValue(MaximumDP); }
            set { SetValue(MaximumDP, value); }
        }

        private static readonly DependencyProperty IntervalDP = DependencyProperty.Register(
            nameof(Interval), typeof(double), typeof(Knob), new PropertyMetadata(default(double)));
        [Description("Gets or sets an interval."), Category("Knob")]
        public double Interval
        {
            get { return (double)GetValue(IntervalDP); }
            set { SetValue(IntervalDP, value); }
        }

        private static readonly DependencyProperty StartAngleDP = DependencyProperty.Register(
            nameof(StartAngle), typeof(double), typeof(Knob), new PropertyMetadata(default(double)));
        [Description("Gets or sets a start angle [degree]."), Category("Knob")]
        public double StartAngle
        {
            get { return (double)GetValue(StartAngleDP); }
            set { SetValue(StartAngleDP, value); }
        }

        private static readonly DependencyProperty EndAngleDP = DependencyProperty.Register(
            nameof(EndAngle), typeof(double), typeof(Knob), new PropertyMetadata(default(double)));
        [Description("Gets or sets the end angle [degree]."), Category("Knob")]
        public double EndAngle
        {
            get { return (double)GetValue(EndAngleDP); }
            set { SetValue(EndAngleDP, value); }
        }

        private static readonly DependencyProperty AccentDP = DependencyProperty.Register(
            nameof(Accent), typeof(Brush), typeof(Knob));
        [Description("Gets or sets a accent color brush."), Category("Knob")]
        public Brush Accent
        {
            get { return (Brush)GetValue(AccentDP); }
            set { SetValue(AccentDP, value); }
        }

        private static readonly DependencyProperty BackgroundBrushDP = DependencyProperty.Register(
            nameof(BackgroundBrush), typeof(Brush), typeof(Knob));
        [Description("Gets or sets a control color brush."), Category("Knob")]
        public Brush BackgroundBrush
        {
            get { return (Brush)GetValue(BackgroundBrushDP); }
            set { SetValue(BackgroundBrushDP, value); }
        }

        #endregion

        #region Constructor
        public Knob()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods
        private void UpdateUI()
        {
            double newAngle = (EndAngle - StartAngle) / (Maximum - Minimum) * (Value - Minimum) + StartAngle;
            LevelEndAngle = newAngle;
            PointerStartAngle = newAngle - 3;
            PointerEndAngle = newAngle + 3;
        }
        #endregion

        #region Events

        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            double d = e.Delta / 120; // Mouse wheel 1 click (120 delta) = 1 step
            Value += d * Interval;
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = true;
            (sender as Ellipse).CaptureMouse();
            previousMousePosition = e.GetPosition((Ellipse)sender);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                Point newMousePosition = e.GetPosition((Ellipse)sender);
                double dY = (previousMousePosition.Y - newMousePosition.Y);
                if (Math.Abs(dY) > mouseMoveThreshold)
                {
                    Value += Math.Sign(dY) * Interval;
                    previousMousePosition = newMousePosition;
                }
            }
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = false;
            (sender as Ellipse).ReleaseMouseCapture();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            UpdateUI();
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
                Value = Default;
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Protected Methods

        /// <summary>
        /// Fires the PropertyChanged Event.
        /// </summary>
        /// <param name="propertyName">The name of the changed property.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
