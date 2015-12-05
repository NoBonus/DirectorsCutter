using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;
using System.IO;

namespace DirectorsCutterWPF
{
    public class VideoCut
    {
        public int index { get; set; }
        public string cutName { get; set; }
        public TimeSpan startTime { get; set; }
        public TimeSpan endTime { get; set; }
        public TimeSpan duration
        {
            get
            {
                return endTime - startTime;
            }
        }
        public string sourcePath { get; set; }
        public string thumbPath { get; set; }
        public byte[] imgThumb { get; set; }
        public VideoCut()
        {

        }

        public void genThumb()
        {
            thumbPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), cutName + index.ToString() + ".jpg");
            var inputFile = new MediaFile { Filename = sourcePath };
            var outputFile = new MediaFile { Filename = thumbPath };
            using (var engine = new Engine())
            {
                engine.GetMetadata(inputFile);
                var options = new ConversionOptions { Seek = startTime };
                engine.GetThumbnail(inputFile, outputFile, options);
            }
            var img = System.Drawing.Image.FromFile(thumbPath);
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            imgThumb = ms.ToArray();
        }
    }

}
