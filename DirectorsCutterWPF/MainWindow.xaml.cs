using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DirectorsCutterWPF
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            CommandBindings.Add(new CommandBinding(MediaCommands.Play, MediaElement_Play, MediaElement_CanPlay));
            CommandBindings.Add(new CommandBinding(MediaCommands.Stop, MediaElement_Stop, MediaElement_IfPlaying));
            CommandBindings.Add(new CommandBinding(MediaCommands.Pause, MediaElement_Pause, MediaElement_IfPlaying));
            CommandBindings.Add(new CommandBinding(MediaCommands.FastForward, MediaElement_FastForward, MediaElement_IfPlaying));
            CommandBindings.Add(new CommandBinding(MediaCommands.Rewind, MediaElement_Rewind, MediaElement_IfPlaying));
        }

        private void MediaElement_Rewind(object sender, ExecutedRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MediaElement_FastForward(object sender, ExecutedRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MediaElement_Pause(object sender, ExecutedRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MediaElement_IfPlaying(object sender, CanExecuteRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MediaElement_Stop(object sender, ExecutedRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MediaElement_CanPlay(object sender, CanExecuteRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MediaElement_Play(object sender, ExecutedRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void cutRange_LowerThumbDragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {

        }

        private void cutRange_LowerThumbDragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            tbTimeRange.Text = "From: "+cutRange.LowerValue.ToString()+" To: "+cutRange.UpperValue.ToString();
        }

        private void cutRange_UpperThumbDragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            tbTimeRange.Text = "From: " + cutRange.LowerValue.ToString() + " To: " + cutRange.UpperValue.ToString();
        }

        private void cutRange_UpperThumbDragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {

        }
    }
}
