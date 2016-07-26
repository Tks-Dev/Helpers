using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using WMPLib;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TksHelpers
{
    public class TksPlaylist : IList<Mp3Music>
    {
        private List<Mp3Music> _items;
        private WindowsMediaPlayer _player;
        private WindowsMediaPlayer _adder;
        private IWMPPlaylistCollection playlistCollection;
        private IWMPPlaylistArray playlistArray;
        private IWMPPlaylist playlist;

        public TksPlaylist()
        {
            _player = new WindowsMediaPlayer();
            _player.settings.autoStart = false;
            _adder = new WindowsMediaPlayer();
            _adder.settings.autoStart = false;
            playlist = _player.currentPlaylist;
        }

        public IEnumerator<Mp3Music> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(Mp3Music item)
        {
            _items.Add(item);
            _adder.URL = item.Path;
            playlist.appendItem(_adder.currentMedia);
        }

        public void Add(string path)
        {
            _items.Add(new Mp3Music(TagLib.File.Create(path)));
            _adder.URL = path;
            playlist.appendItem(_adder.currentMedia);
        }

        public void Clear()
        {
            _items.Clear();
            playlist.clear();
        }

        public bool Contains(Mp3Music item)
        {
            return _items.Contains(item);
        }

        public void CopyTo(Mp3Music[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public bool Remove(Mp3Music item)
        {
            for (var index = 0; index < playlist.count; index++)
                if(playlist.Item[index].sourceURL == item.Path)
                    playlist.removeItem(playlist.Item[index]);

            return _items.Remove(item);
        }

        public int Count => _items.Count;
        public bool IsReadOnly { get; }

        public int IndexOf(Mp3Music item)
        {
            return _items.IndexOf(item);
        }

        public void Insert(int index, Mp3Music item)
        {
            _items.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _items.RemoveAt(index);
            playlist.clear();
            foreach (var mp3Music in _items)
            {
                _adder.URL = mp3Music.Path;
                playlist.appendItem(_adder.currentMedia);
            }
        }

        public Mp3Music this[int index]
        {
            get { return _items[index]; }
            set { _items[index] = value; }
        }

        public void PlayingMode(PlayingMode mode)
        {
            switch (mode)
            {
                case TksHelpers.PlayingMode.Normal:
                    _player.settings.setMode("shuffle", false);
                    break;
                case TksHelpers.PlayingMode.Random:
                    _player.settings.setMode("shuffle", true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        public void LoopMode(LoopMode mode)
        {
            switch (mode)
            {
                case TksHelpers.LoopMode.Normal:
                    break;
                case TksHelpers.LoopMode.One:
                    break;
                case TksHelpers.LoopMode.All:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        public void Next()
        {
            _player.controls.next();
        }

        public void Pause()
        {
            _player.controls.pause();
        }

        public void Stop()
        {
            _player.controls.stop();
        }

        public void Previous()
        {
            _player.controls.previous();
        }

        public void FastForward()
        {
            _player.controls.fastForward();
        }

        public void FastReverse()
        {
            _player.controls.fastReverse();
        }

        public string[] LoadPlaylistFromFile(string path)
        {
            if(!path.EndsWith(".tksp")) throw new ArgumentException("The playlist must be a tksp file");

            var errors = new List<string>();
            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    var file = reader.ReadLine();
                    try
                    {
                        Add(file);
                    }
                    catch (Exception e)
                    {
                        errors.Add(file + " => " + e.Message);
                    }
                }
            }

            return errors.ToArray();
        }

        public void Save(string path)
        {
            var output = path.EndsWith(".tksp") ? path : path + ".tksp";
            using (var file = File.CreateText(output))
                foreach (var mp3Music in _items)
                {
                    file.WriteLine(mp3Music.Path);
                }
        }

        public void SaveWithUI(string title, string initialDirectory)
        {
            Save(ExplorerHelper.SaveFileWindow(title, initialDirectory, ".tksp"));
        }
    }

    public enum PlayingMode
    {
        Normal,
        Random
    }

    public enum LoopMode
    {
        Normal,
        One,
        All
    }
}
