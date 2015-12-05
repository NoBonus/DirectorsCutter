using MahApps.Metro.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using System.Windows.Threading;
using WF = System.Windows.Forms;
using WPF.JoshSmith.ServiceProviders.UI;
using System.Text.RegularExpressions;

namespace DirectorsCutterWPF
{
    public static class Command
    {

        public static readonly RoutedUICommand AddCutCommand = new RoutedUICommand("Add Cut", "AddCut", typeof(MainWindow));
        public static readonly RoutedUICommand StepForwardCommand = new RoutedUICommand("Step Forward", "StepForward", typeof(MainWindow));
        public static readonly RoutedUICommand StepBackwardCommand = new RoutedUICommand("Step Backward", "StepBackward", typeof(MainWindow));
    }
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private bool mediaPlayerIsPlaying=false;
        public ObservableCollection<VideoCut> lstCuts;
        ListViewDragDropManager<VideoCut> dragMgr;
        private CutProcess cprocess;
        public MainWindow()
        {
            InitializeComponent();
            CommandBindings.Add(new CommandBinding(MediaCommands.Play, MediaElement_Play, MediaElement_CanPlay));
            CommandBindings.Add(new CommandBinding(MediaCommands.Stop, MediaElement_Stop, MediaElement_IfPlaying));
            CommandBindings.Add(new CommandBinding(MediaCommands.Pause, MediaElement_Pause, MediaElement_IfPlaying));
            CommandBindings.Add(new CommandBinding(MediaCommands.FastForward, MediaElement_FastForward, MediaElement_IfPlaying));
            CommandBindings.Add(new CommandBinding(MediaCommands.Rewind, MediaElement_Rewind, MediaElement_IfPlaying));
            CommandBindings.Add(new CommandBinding(Command.AddCutCommand, AddCut, MediaElement_CanPlay));
            CommandBindings.Add(new CommandBinding(Command.StepForwardCommand, StepForward, MediaElement_CanPlay));
            CommandBindings.Add(new CommandBinding(Command.StepBackwardCommand, StepBackward, MediaElement_CanPlay));

            lstCuts = new ObservableCollection<VideoCut>();
            lvCuts.ItemsSource = lstCuts;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
            cprocess = new CutProcess();
        }

       

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            dragMgr = new ListViewDragDropManager<VideoCut>(lvCuts);
            lvCuts.DragEnter += OnlvCutsDragEnter;
            lvCuts.Drop += OnlvCutsDrop;

        }
        private void StepForward(object sender, ExecutedRoutedEventArgs e)
        {
            mePlayer.Pause();
            var btn =(Button) e.Source;
            if (btn.Name == "btnStartStepForward")
            {
                cutRange.LowerValue += cutRange.SmallChange;
                mePlayer.Position = TimeSpan.FromSeconds(cutRange.LowerValue);
            }
            else
            {
                cutRange.UpperValue += cutRange.SmallChange;
                mePlayer.Position = TimeSpan.FromSeconds(cutRange.UpperValue);
            }
            setRangeLabel();
        }
        private void StepBackward(object sender, ExecutedRoutedEventArgs e)
        {
            mePlayer.Pause();
            var btn = (Button)e.Source;
            if (btn.Name == "btnStartStepBackward")
            {
                cutRange.LowerValue -= cutRange.SmallChange;
                mePlayer.Position = TimeSpan.FromSeconds(cutRange.LowerValue);
            }
            else
            {
                cutRange.UpperValue -= cutRange.SmallChange;
                mePlayer.Position = TimeSpan.FromSeconds(cutRange.UpperValue);
            }
            setRangeLabel();
        }


        private void OnlvCutsDrop(object sender, DragEventArgs e)
        {
            if (e.Effects == DragDropEffects.None)
                return;

        }

        private void OnlvCutsDragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;

        }
        private void AddCut(object sender, ExecutedRoutedEventArgs e)
        {
            VideoCut vc = new VideoCut();
            vc.cutName = "Cut_" + lstCuts.Count.ToString();
            vc.index = lstCuts.Count;
            vc.sourcePath = tbVidPath.Text;
            vc.startTime = TimeSpan.FromSeconds(cutRange.LowerValue);
            vc.endTime = TimeSpan.FromSeconds(cutRange.UpperValue);
            vc.genThumb();
            lstCuts.Add(vc);
            lvCuts.Items.Refresh();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (!mediaPlayerIsPlaying) return;
            if(mePlayer.Position.TotalSeconds>= cutRange.UpperValue)mePlayer.Pause();
            
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
            mePlayer.Pause();
        }

        private void MediaElement_IfPlaying(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = mediaPlayerIsPlaying;
        }

        private void MediaElement_Stop(object sender, ExecutedRoutedEventArgs e)
        {
            mePlayer.Stop();
            mediaPlayerIsPlaying = false;
        }

        private void MediaElement_CanPlay(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (mePlayer != null) && (mePlayer.Source != null);
        }

        private void MediaElement_Play(object sender, ExecutedRoutedEventArgs e)
        {
            mePlayer.Position = TimeSpan.FromSeconds(cutRange.LowerValue);
            mePlayer.Play();
            mediaPlayerIsPlaying = true;
        }

        private void cutRange_LowerThumbDragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {

        }

        private void cutRange_LowerThumbDragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            setRangeLabel();
            //mePlayer.Position=

        }

        private void cutRange_UpperThumbDragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            setRangeLabel();
        }

        private void cutRange_UpperThumbDragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {

        }
        private void setRangeLabel()
        {
            tbTimeRange.Text = "From: " + TimeSpan.FromSeconds(cutRange.LowerValue).ToString() + " To: " + TimeSpan.FromSeconds(cutRange.UpperValue).ToString() + " , duration: " + TimeSpan.FromSeconds(cutRange.UpperValue - cutRange.LowerValue).ToString();
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
            mePlayer.Pause();
        }

        private void mePlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            cutRange.Minimum = 0;
            cutRange.Maximum = mePlayer.NaturalDuration.TimeSpan.TotalSeconds;
            cutRange.LowerValue = cutRange.Minimum;
            cutRange.UpperValue = cutRange.Maximum;
            //tbTimeRange.Text = "From: " + TimeSpan.FromSeconds(cutRange.LowerValue).ToString() + " To: " + TimeSpan.FromSeconds(cutRange.UpperValue).ToString();
            setRangeLabel();

        }

        private void cutRange_LowerThumbDragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            Console.WriteLine("cutRange.LowerValue:{0}", cutRange.LowerValue);
            mePlayer.Position = TimeSpan.FromSeconds(cutRange.LowerValue);
        }

        private void cutRange_UpperThumbDragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            Console.WriteLine("cutRange.UpperValue:{0}", cutRange.UpperValue);
            mePlayer.Pause();
            mePlayer.Position = TimeSpan.FromSeconds(cutRange.UpperValue);
            mePlayer.Play();
            mePlayer.Pause();
        }

        private void mePlayer_BufferingStarted(object sender, RoutedEventArgs e)
        {
            if(!mediaPlayerIsPlaying) mePlayer.Play();
        }

        private void cutRange_CentralThumbDragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            setRangeLabel();
        }

        private void cutRange_CentralThumbDragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            Console.WriteLine("cutRange.UpperValue:{0}", cutRange.UpperValue);
            mePlayer.Pause();
            mePlayer.Position = TimeSpan.FromSeconds(cutRange.UpperValue);
            mePlayer.Play();
            mePlayer.Pause();
        }

        private void txtStepVal_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }

        private void btnStepValUp_Click(object sender, RoutedEventArgs e)
        {
            float val = float.Parse(txtStepVal.Text);
            val += 0.1f;
            cutRange.SmallChange = val;
        }

        private void btnStepValDown_Click(object sender, RoutedEventArgs e)
        {
            float val = float.Parse(txtStepVal.Text);
            val -= 0.1f;
            cutRange.SmallChange = val;
        }

        private void btnremoveCut_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            ListViewItem lvi = FindParent<ListViewItem>(btn);
            VideoCut vc = (VideoCut)lvi.DataContext;
            lstCuts.Remove(vc);
        }
        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            //get parent item
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            //we've reached the end of the tree
            if (parentObject == null) return null;

            //check if the parent matches the type we're looking for
            T parent = parentObject as T;
            if (parent != null)
                return parent;
            else
                return FindParent<T>(parentObject);
        }

        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {
            btnProcess.IsEnabled = false;
            lvCuts.IsEnabled = false;
            cprocess.Process(lstCuts, false,"");
            btnProcess.IsEnabled = true;
            lvCuts.IsEnabled = true;
        }

        private void btnOutputFolder_Click(object sender, RoutedEventArgs e)
        {
            WF.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.ShowNewFolderButton = false;
            var result = fbd.ShowDialog();
            if (result == WF.DialogResult.OK)
            {
                txtOutputPath.Text = fbd.SelectedPath;
            }

        }
    }
    
   
}
