using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ZetaLongPaths;

namespace CloneKiller.Business
{
    public class RedundantFiles : List<RedundantFile>
    {
        public int Percent { get; set; }
        public bool Error { get; set; }
        public ConcurrentQueue<RedundantFile> ConcurrentQueue { get; private set; }
        public MainWindow MainWindow { get; set; }
        public MainWindow.ChangeCountDelegate _chn { get; set; }


        public Task AddFile(string path)
        {
           return Task.Run(() =>
            {
                var fileInfo = new ZlpFileInfo(path);
                MainWindow.ChangeFileName(path);
                //if (this.Any(rf => rf.FileName == fileInfo.Name))
                //{
                //var file = this.First(rf => rf.FileName == fileInfo.Name);
                var fileName = string.Empty;
                long weight;
                int margin;
                RedundantFile file;
                do
                {
                    if (fileName == string.Empty)
                        fileName = fileInfo.Name;
                    else
                        fileName = "--" + fileName;

                    var name = fileName;
                    file = ConcurrentQueue.FirstOrDefault(rf => rf.FileName == name);
                    if (file == null)
                        break;
                    try
                    {
                        weight = file.Weight;
                    }
                    catch
                    {
                        weight = 0;
                    }
                    margin = (int) (weight*Percent/100);
                } while (fileInfo.Length > weight + margin || fileInfo.Length < weight - margin);
                if (file == null)
                {
                    file = new RedundantFile() {FileName = fileName};
                    ConcurrentQueue.Enqueue(file);
                }
                ConcurrentQueue.First(rf => rf.FileName == fileName).AddFileAsync(fileInfo);
                _chn(ConcurrentQueue.Count.ToString());
            });
            //}
            //else
            //{
            //    var file = new RedundantFile() {FileName = fileInfo.Name};
            //    file.AddFile(path, fileInfo.Length);
            //    Add(file);
            //}

        }

        public Task FillFromFolder(string v)
        {
            
            var files = GetFiles(v);
            ConcurrentQueue = new ConcurrentQueue<RedundantFile>();
            var tasks = files.Select(AddFile).ToList();
            var t = Task.WhenAll(tasks);
            AddRange(ConcurrentQueue);
            return t;
        }

        public List<string[]> ToStringArrayList()
        {
            if (Count == 0 && ConcurrentQueue != null)
                AddRange(ConcurrentQueue);
            return this.Select(red => red.ToStringArray()).ToList();
        }

        public List<string> GetFiles(string directory)
        {
            if (directory == null || !Directory.Exists(directory))
                return new List<string>();
            try
            {
                var files = Directory.EnumerateFiles(directory).ToList();
                var directories = Directory.EnumerateDirectories(directory);
                foreach (var directory1 in directories)
                {
                    files.AddRange(GetFiles(directory1));
                }
                return files;
            }
            catch (UnauthorizedAccessException)
            {
                Error = true;
                return new List<string>();
            }
        }

    }

}
