using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVHplatform.Models
{
    public class AudioFileInfo
    {
        public string SourceType { get; set; }      // 录音/导入
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public TimeSpan Duration { get; set; }
        public int SampleRate { get; set; }
        public int Channels { get; set; }
        public int BitRate { get; set; }
        public string Format { get; set; }
    }

}
