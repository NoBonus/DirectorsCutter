using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectorsCutterWPF
{
    public delegate void ProcessProgressEventHandler(object sender, EventArgs e);
    public delegate void ProcessCompleteEventHandler(object sender, EventArgs e);
    
    public class CutProcess
    {
        private VideoCut currentCut;
        public event ProcessProgressEventHandler ProcessProgress;
        public event ProcessCompleteEventHandler ProcessComplete;
        public void Process( ObservableCollection<VideoCut> vclist, bool join, string joinfname)
        {
            

            foreach(VideoCut vc in vclist)
            {
                string outpath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), vc.cutName + System.IO.Path.GetExtension(vc.sourcePath));
                var inputFile = new MediaFile { Filename = vc.sourcePath };
                var outputFile = new MediaFile { Filename = outpath };

                using (var engine = new Engine())
                {
                    engine.ConvertProgressEvent += ConvertProgressEvent;
                    engine.ConversionCompleteEvent += engine_ConversionCompleteEvent;
                    engine.GetMetadata(inputFile);

                    var options = new ConversionOptions();
                    
                    options.CutMedia(vc.startTime, vc.duration);

                    engine.Convert(inputFile, outputFile, options);
                }
            }
        }

        private void engine_ConversionCompleteEvent(object sender, ConversionCompleteEventArgs e)
        {
            Console.WriteLine("\n------------\nConversion complete!\n------------");
            Console.WriteLine("Bitrate: {0}", e.Bitrate);
            Console.WriteLine("Fps: {0}", e.Fps);
            Console.WriteLine("Frame: {0}", e.Frame);
            Console.WriteLine("ProcessedDuration: {0}", e.ProcessedDuration);
            Console.WriteLine("SizeKb: {0}", e.SizeKb);
            Console.WriteLine("TotalDuration: {0}\n", e.TotalDuration);
        }

        private void ConvertProgressEvent(object sender, ConvertProgressEventArgs e)
        {
            Console.WriteLine("\n------------\nConverting...\n------------");
            Console.WriteLine("Bitrate: {0}", e.Bitrate);
            Console.WriteLine("Fps: {0}", e.Fps);
            Console.WriteLine("Frame: {0}", e.Frame);
            Console.WriteLine("ProcessedDuration: {0}", e.ProcessedDuration);
            Console.WriteLine("SizeKb: {0}", e.SizeKb);
            Console.WriteLine("TotalDuration: {0}\n", e.TotalDuration);
        }
        private class ProcessMedia
        {
            public MediaFile inputMedia{ get { return inputMedia; } set { inputMedia = value; getInputMetadata(); } }
            public MediaFile outputMedia { get; set; }
            public ProcessMedia(MediaFile input, MediaFile output)
            {
                inputMedia = input;
                outputMedia = output;
                getInputMetadata();
            }
            private void getInputMetadata()
            {
                using (var engine = new Engine())
                {
                    engine.GetMetadata(inputMedia);
                }
            }
            public ProcessMedia(string inputpath, string outputpath)
            {
                inputMedia= new MediaFile { Filename = inputpath };
                outputMedia = new MediaFile { Filename = outputpath };
                getInputMetadata();
            }
        }
    }
}
