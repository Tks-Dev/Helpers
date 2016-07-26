using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using ZetaLongPaths;

namespace CloneKiller.Business
{
    public class RedundantFile
    {
        public string FileName { get; set; }
        public List<ZlpFileInfo> Files  { get; set; }
        public long Weight { get; private set; }
        public ConcurrentQueue<ZlpFileInfo> FilesAsync { get; set; }

        public void AddFile(ZlpFileInfo fileInfo)
        {
            if (Files == null)
            {
                Files = new List<ZlpFileInfo>();
                Weight = fileInfo.Length;
            }
            Files.Add(fileInfo);
        }

        public string[] ToStringArray()
        {
            if (Files == null)
                Files = new List<ZlpFileInfo>();
            if (FilesAsync !=null)
                foreach (var zlpFileInfo in FilesAsync.Where(zlpFileInfo => !Files.Contains(zlpFileInfo)))
                {
                    Files.Add(zlpFileInfo);
                }

            var totLength = Files.Sum(zlpFileInfo => zlpFileInfo.Length);
            return Files.Count == 0 ? new string[] {FileName, "0", "0", "0", "0"} : new string[] {FileName, Files.Count.ToString(), Files.First()?.Length.ToString(), totLength.ToString()};
        }

        public void AddFileAsync(ZlpFileInfo fileInfo)
        {
            if (FilesAsync == null)
                FilesAsync = new ConcurrentQueue<ZlpFileInfo>();
            FilesAsync.Enqueue(fileInfo);
        }
    }
}
