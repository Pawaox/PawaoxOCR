using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawaoxOCR
{
    public class ProcessResult
    {
        public bool Success { get; set; }

        public string RawParsedText { get; set; } = "";
        public float MeanConfidence { get; set; }

        public List<List<string>> Blocks { get; set; }

        public ProcessResult()
        {
            Blocks = new List<List<string>>();
        }

        public List<string> GetAllLines()
        {
            List<string> lines = new List<string>();

            if (Blocks != null)
            {
                foreach (var lst in Blocks)
                {
                    foreach (string line in lst)
                    {
                        string str = line?.Trim() ?? "";

                        if (!string.IsNullOrEmpty(str))
                            lines.Add(str);
                    }
                }
            }

            return lines;
        }
    }
}
