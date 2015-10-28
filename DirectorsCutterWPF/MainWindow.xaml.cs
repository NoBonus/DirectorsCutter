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
using WF = System.Windows.Forms;

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
            //mePlayer.Position=
        }

        private void cutRange_UpperThumbDragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            tbTimeRange.Text = "From: " + cutRange.LowerValue.ToString() + " To: " + cutRange.UpperValue.ToString();
        }

        private void cutRange_UpperThumbDragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {

        }

        private void openVid_Click(object sender, RoutedEventArgs e)
        {
            WF.OpenFileDialog openFileDialog1 = new WF.OpenFileDialog();
            string formats = "All Videos Files |*.dat; *.wmv; *.3g2; *.3gp; *.3gp2; *.3gpp; *.amv; *.asf;  *.avi; *.bin; *.cue; *.divx; *.dv; *.flv; *.gxf; *.iso; *.m1v; *.m2v; *.m2t; *.m2ts; *.m4v; " +
                  " *.mkv; *.mov; *.mp2; *.mp2v; *.mp4; *.mp4v; *.mpa; *.mpe; *.mpeg; *.mpeg1; *.mpeg2; *.mpeg4; *.mpg; *.mpv2; *.mts; *.nsv; *.nuv; *.ogg; *.ogm; *.ogv; *.ogx; *.ps; *.rec; *.rm; *.rmvb; *.tod; *.ts; *.tts; *.vob; *.vro; *.webm";

            //openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = formats;

            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == WF.DialogResult.OK)
            {
                mePlayer.Source = new Uri(openFileDialog1.FileName);
                tbVidPath.Text = openFileDialog1.FileName;
            }
        }

        private void mePlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            cutRange.Minimum = 0;
            cutRange.Maximum = mePlayer.NaturalDuration.TimeSpan.TotalSeconds;
            cutRange.LowerValue = cutRange.Minimum;
            cutRange.UpperValue = cutRange.Maximum;
            tbTimeRange.Text = "From: " + cutRange.LowerValue.ToString() + " To: " + cutRange.UpperValue.ToString();
        }
    }
}
