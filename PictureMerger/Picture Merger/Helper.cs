using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.VisualStyles;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace YuGiOhCardCreator.Helpers
{
    class Helper
    {
        /// <summary>
        /// Convertit une date sous en un entier sous la forme YYYYMMDD
        /// </summary>
        /// <param name="toConvert">La date a convertir</param>
        /// <returns>L'entier correspondant a la date</returns>
        public static int ToInt(DateTime toConvert)
        {
            return toConvert.Year * 10000 + toConvert.Month * 10 + toConvert.Day;
        }

        /// <summary>
        /// Convertit un entier de la forme YYYYMMDD en date
        /// </summary>
        /// <param name="toConvert">L'entier a convertir</param>
        /// <returns>La date correspondante a l'entier</returns>
        public static DateTime ToDate(int toConvert)
        {
            var year = toConvert / 10000;
            var month = toConvert % 10000 / 100;
            var day = toConvert % 100;
            return new DateTime(year, month, day);
        }

        public static DateTime StringFrToDate(string date)
        {
            if (date.Trim() == string.Empty)
                return DateTime.MaxValue;
            return new DateTime(
                        Convert.ToInt32(date.Split('/')[2]),
                        Convert.ToInt32(date.Split('/')[1]),
                        Convert.ToInt32(date.Split('/')[0])
                            );
        }

        public static ImageSource ImageSourceFromPath(string path)
        {
            var imgsrc = new BitmapImage();
            imgsrc.BeginInit();
            imgsrc.UriSource = new Uri(path, UriKind.Relative);
            imgsrc.CacheOption = BitmapCacheOption.OnLoad;
            imgsrc.EndInit();
            return imgsrc;
        }

        public static int DateTimeToMillis(DateTime dateTime)
        {
            return (int)((((((((dateTime.Year) * 12 + dateTime.Month - 1) * 30.41 + dateTime.Day) * 24 + dateTime.Hour) * 60 + dateTime.Minute) * 60 + dateTime.Second)*1000 + dateTime.Millisecond) % Int32.MaxValue);
        }

        public static void ShowError(Exception e)
        {
            MessageBox.Show(e.Message + "\n======================\n" + e.StackTrace, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static string AdaptStringToSql(string toAdapt)
        {
            for (var i = 0; i < toAdapt.Length; i++)
                if (toAdapt[i] == '\'')
                {
                    toAdapt = toAdapt.Insert(i, "'");
                    i++;
                }
            return toAdapt;
        }

        /// <summary>
        /// Show a message box.
        /// Title : Informations
        /// Text Shown : The parameter
        /// </summary>
        /// <param name="p">The text shown in the message box</param>
        public static void Show(string p)
        {
            MessageBox.Show(p, "Informations");
        }

        /// <summary>
        /// Open a folder browser dialog to get the path to a folder
        /// </summary>
        /// <param name="windowTitle">The title of the FolderBrowserDialog</param>
        /// <returns>The path of the selected folder</returns>
        public static string FolderFromBrowser(string windowTitle)
        {
            var f = new System.Windows.Forms.FolderBrowserDialog
            {
                Description = windowTitle
            };
            f.ShowDialog();
            return f.SelectedPath;
        }

        /// <summary>
        /// Open a file browser dialog to get the path to a folder
        /// </summary>
        /// <param name="windowTitle">The title of the FileBrowserDialog</param>
        /// <param name="initialDirectory">Path of the start directory</param>
        /// <returns>The path to the selected file</returns>
        public static string FileFromBrowser(string windowTitle, string initialDirectory)
        {
            var f = new System.Windows.Forms.OpenFileDialog
            {
                InitialDirectory = initialDirectory,
                Title = windowTitle
            };
            f.ShowDialog();
            return f.FileName;
        }

        public static string GetFileNameFromPath(string p)
        {
            return p.Split('\\')[p.Split('\\').Length - 1];
        }
    }

    public static class ExtensionsMethods
    {
        /// <summary>
        /// Convertit un DateTime en une chaine de caractere utilisable en SQLite pour inserer une date
        /// </summary>
        /// <param name="toAdapt">La date a convertir</param>
        /// <returns></returns>
        public static string ToSqLiteModel(this DateTime toAdapt)
        {
            return toAdapt.Year + "-" + (toAdapt.Month > 9 ? string.Empty + toAdapt.Month : "0" + toAdapt.Month) + "-" + (toAdapt.Day > 9 ? string.Empty + toAdapt.Day : "0" + toAdapt.Day);
        }

        /// <summary>
        /// Permet de récuperer la valeur d'une cellule d'un DataGrid dans la ligne selectionnée.
        /// ATTENTION : La valeur de la colonne est absolue : si le DataGrid est réorganisé par l'utilisateur, il faut changer l'index.
        /// </summary>
        /// <param name="dGrid">Le DataGrid this où l'on fait la récupération</param>
        /// <param name="columnIndex">L'index de la valeur dans la ligne selectionnée</param>
        /// <returns>La valeur contenue dans la ligne selectionnée ou une chaine vide si aucune ligne n'est selectionnée</returns>
        public static string GetValueAt(this DataGrid dGrid, int columnIndex)
        {
            if (dGrid.SelectedItem == null) // si rien n'est selectionné, on renvoit une chaine vide
                return string.Empty;
            var str = dGrid.SelectedItem.ToString(); // Recupère la ligne selectionnee
            str = str.Replace("}", string.Empty).Trim().Replace("{", string.Empty).Trim(); // Enlève les caracteres superflu
            if (columnIndex < 0 || columnIndex >= str.Split(',').Length) // Cas où l'index donné n'est pas dans l'ensemble des index utilisables
                return string.Empty;
            str = str.Split(',')[columnIndex].Trim();
            str = str.Split('=')[1].Trim();
            return str;
        }

        /// <summary>
        /// Permet de récupérer la valeur d'une cellule d'un DataGrid dans la ligne sélectionnée.
        /// </summary>
        /// <param name="dGrid">Le DataGrid où l'on fait la récupération.</param>
        /// <param name="columnName">Le nom de la colonne de la valeur recherchée. ATTENTION, le paramètre doit être le même que celui AFFICHE.</param>
        /// <returns>La valeur contenue dans la ligne selectionnée ou une chaîne de caractères vide si aucune ligne n'est selectionnée ou si la colonne n'existe pas.</returns>
        public static string GetValueAt(this DataGrid dGrid, string columnName)
        {
            if (dGrid.SelectedItem == null)
                return string.Empty;
            for (var i = 0; i < columnName.Length; i++)
                if (columnName[i] == '_')
                {
                    columnName = columnName.Insert(i, "_"); // Formalise le nom de la colonne (il faut 2 '_' pour qu'il n'y en ai qu'un d'affiche)
                    i++;
                }
            var str = dGrid.SelectedItem.ToString(); // Recupère la ligne selectionnée
            str = str.Replace("}", string.Empty).Trim().Replace("{", string.Empty).Trim(); // Enlève les caractères superflus
            for (var i = 0; i < str.Split(',').Length; i++)
                if (str.Split(',')[i].Trim().Split('=')[0].Trim() == columnName) // Vérifie la correspondance entre la colonne demandée et celles présentes dans le DataGrid
                    return str.Split(',')[i].Trim().Split('=')[1].Trim();
            return str;
        }

        private static readonly Action EmptyDelegate = delegate() { };

        /// <summary>
        /// Permet de rafraichir un élement graphique
        /// </summary>
        /// <param name="uiElement">L'élement graphique à rafraichir</param>
        public static void Refresh(this UIElement uiElement)
        {
            uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }

        /// <summary>
        /// Effectue un fondu en utilisant une DoubleAnimation
        /// </summary>
        /// <param name="uiElement">L'element this qui subira la transformation</param>
        /// <param name="opacityValue">La valeur d'opacite à atteindre</param>
        /// <param name="startTime">Le delai avant le commencement de l'animation</param>
        /// <param name="duration">La lapse de temps durant lequel l'animation se déroule</param>
        public static void Fade(this UIElement uiElement, double opacityValue, double startTime, double duration)
        {
            var animation = new DoubleAnimation
            {
                To = opacityValue,
                BeginTime = TimeSpan.FromSeconds(startTime),
                Duration = TimeSpan.FromSeconds(duration),
                FillBehavior = FillBehavior.Stop
            };
            animation.Completed += (s, a) => uiElement.Opacity = opacityValue;
            uiElement.BeginAnimation(UIElement.OpacityProperty, animation);
        }

        /// <summary>
        /// Change cette image avec une transition en fondu
        /// </summary>
        /// <param name="image">L'image this à modifier</param>
        /// <param name="source">La source de la prochaine image</param>
        /// <param name="beginTime">Le délai en seconde après lequel la transition commence</param>
        /// <param name="fadeTime">Le durée de la transition</param>
        public static void ChangeSource(this Image image, ImageSource source, double beginTime, double fadeTime)
        {
            var fadeInAnimation = new DoubleAnimation
            {
                To = 1,
                BeginTime = TimeSpan.FromSeconds(fadeTime),
                Duration = TimeSpan.FromSeconds(fadeTime)
            };

            if (image.Source != null)
            {
                var fadeOutAnimation = new DoubleAnimation
                {
                    To = 0,
                    BeginTime = TimeSpan.FromSeconds(beginTime),
                    Duration = TimeSpan.FromSeconds(fadeTime)
                };

                fadeOutAnimation.Completed += (o, e) =>
                {
                    image.Source = source;
                    image.BeginAnimation(UIElement.OpacityProperty, fadeInAnimation);
                };
                image.BeginAnimation(UIElement.OpacityProperty, fadeOutAnimation);
            }
            else
            {
                image.Opacity = 0d;
                image.Source = source;
                fadeInAnimation.BeginTime = TimeSpan.FromSeconds(beginTime);
                image.BeginAnimation(UIElement.OpacityProperty, fadeInAnimation);
            }
        }

        public static void ChangeSource(this Image image, string path)
        {
            image.ChangeSource(path, 0,0);
        }

        public static void ChangeSource(this Image image, string path, double beginTime, double fadeTime)
        {
            if (!File.Exists(path))
            {
                MessageBox.Show("Picture at : \"" + path + "\" does not exist.", "No image", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            var imgsrc = new BitmapImage();
            imgsrc.BeginInit();
            imgsrc.UriSource = new Uri(path,UriKind.Relative);
            imgsrc.CacheOption = BitmapCacheOption.OnLoad;
            imgsrc.EndInit();
            image.ChangeSource(imgsrc, beginTime, fadeTime);
        }

        public static Dictionary<Coordonates, double> GetSourceCoordonates(this Image image)
        {
            var src = image.Source;
            if (src == null)
                return new Dictionary<Coordonates, double>(4) {{Coordonates.Top, 0.0}, {Coordonates.Bottom, 0.0}, {Coordonates.Left, 0.0}, {Coordonates.Right, 0.0}};
            var srcW = src.Width;
            var srcH = src.Height;
            switch (image.Stretch)
            {
                case Stretch.Uniform:
                {
                    double realW, realH;
                    var ratio = srcH/srcW;
                    if (image.ActualHeight > image.ActualWidth)
                    {
                        realW = image.ActualWidth;
                        realH = realW*ratio;
                    }
                    else
                    {
                        realH = image.ActualHeight;
                        realW = realH/ratio;
                    }
                    
                    //var l = (image.Margin.Left + image.Margin.Right)/2 - realW/2;
                    //var r = (image.Margin.Left + image.Margin.Right)/2 + realW/2;
                    //var t = (image.Margin.Top + image.Margin.Bottom)/2 + realH/2;
                    //var b = image.Margin.Left + image.Margin.Right/2 - realH/2;
                    var marginVer = (image.ActualHeight - realH)/2;
                    var marginHor = (image.ActualWidth - realW)/2;
                    return new Dictionary<Coordonates, double>()
                    {
                        {Coordonates.Bottom, marginVer},
                        {Coordonates.Left, marginHor},
                        {Coordonates.Right, marginHor},
                        {Coordonates.Top, marginVer}
                    };
                        
                }
                case Stretch.Fill:
                    return new Dictionary<Coordonates, double>()
                    {
                        {Coordonates.Top, image.Margin.Top},
                        {Coordonates.Bottom, image.Margin.Bottom},
                        {Coordonates.Left, image.Margin.Left},
                        {Coordonates.Right, image.Margin.Right}
                    };
                case Stretch.UniformToFill:
                {
                    double realW, realH;
                
                    if (image.Height < image.Width)
                    {
                        realW = image.Width;
                        realH = realW*srcH/srcW;
                    }
                    else
                    {
                        realH = image.Height;
                        realW = realH*srcW/srcH;
                    }

                    var l = (image.Margin.Left + image.Margin.Right)/2 - realW/2;
                    var r = (image.Margin.Left + image.Margin.Right)/2 + realW/2;
                    var t = (image.Margin.Top + image.Margin.Bottom)/2 + realH/2;
                    var b = (image.Margin.Left + image.Margin.Right)/2 - realH/2;
                    return new Dictionary<Coordonates, double>()
                    {
                        {Coordonates.Bottom, b},
                        {Coordonates.Left, l},
                        {Coordonates.Right, r},
                        {Coordonates.Top, t}
                    };
            }
                case Stretch.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            throw new NotImplementedException();
        }

        public static Thickness AsThickness(this Dictionary<Coordonates, double> coordonates)
        {
            return new Thickness(coordonates[Coordonates.Left], coordonates[Coordonates.Top],
                coordonates[Coordonates.Right], coordonates[Coordonates.Bottom]);
        }

        public static void SaveAsJpg(this ImageSource img, string fullpath, int quality = 100)
        {
            var bmp = img as BitmapSource;
            var encoder = new JpegBitmapEncoder();
            var outputFrame = BitmapFrame.Create(bmp);
            encoder.Frames.Add(outputFrame);
            encoder.QualityLevel = quality;

            using (var file = File.OpenWrite(fullpath + ".jpg"))
            {
                encoder.Save(file);
            }
        }

        public static string SaveAsPng(this ImageSource img, string fullpath)
        {
            var bmp = img as BitmapSource;
            var encoder = new PngBitmapEncoder();
            var outputFrame = BitmapFrame.Create(bmp);
            encoder.Frames.Add(outputFrame);
            
            using (var file = File.OpenWrite(fullpath + ".png"))
            {
                encoder.Save(file);
            }

            return fullpath + ".png";
        }
    }

    public static class ByteExtension
    {
        public static Bits AsBits(this byte value)
        {
            var bits = new Bits();
            bits.FillFromByte(value);
            return bits;
        }
    }

    public static class UShortExtension
    {
        public static Bits AsBits(this ushort value)
        {
            var bits = new Bits();
            bits.FillFromUShort(value);
            return bits;
        }
    }

    public class Bits
    {
        public const byte BYTE_BITS = 8;
        public const byte INT16_BITS = 16;
        public const byte INT32_BITS = 32;
        public const byte INT64_BITS = 64;

        private bool[] _bitArray;

        public bool[] BitArray
        {
            get { return _bitArray.Clone() as bool[]; }
            private set { _bitArray = value; }
        }

        public byte AsByte()
        {
            if (BitArray.Length > 8)
                throw new InvalidCastException("Too Many bits");
            return BitArray.Aggregate<bool, byte>(0, (current, b1) => Convert.ToByte(current*2 + (b1 ? 1 : 0)));
        }

        public short AsUInt16()
        {
            if (BitArray.Length > 16)
                throw new InvalidCastException("Too Many bits");
            return BitArray.Aggregate<bool, short>(0, (current, b1) => Convert.ToInt16(current * 2 + (b1 ? 1 : 0)));
        }

        public int AsInt32()
        {
            if (BitArray.Length > 32)
                throw new InvalidCastException("Too Many bits");
            return BitArray.Aggregate(0, (current, b1) => Convert.ToInt32(current * 2 + (b1 ? 1 : 0)));
        }

        public long AsInt64()
        {
            if (BitArray.Length > 64)
                throw new InvalidCastException("Too Many bits");
            return BitArray.Aggregate<bool, long>(0, (current, b1) => Convert.ToInt64(current * 2 + (b1 ? 1 : 0)));
        }

        public void FillFromList(IEnumerable<bool> values)
        {
            _bitArray = values.ToArray();
        }

        public void FillFromList(IEnumerable<int> values)
        {
            if(values.Any(i => i>1 || i<0))
                throw new InvalidCastException("At least 1 element isn't 0 or 1");
            _bitArray = values.Select(i => i == 1).ToArray();
        }

        public void FillWithTrue(int length)
        {
            _bitArray = new bool[length];
            for (var i = 0; i < length; i++)
                _bitArray[i] = true;
        }

        public void FillFromByte(byte value)
        {
            var b = value;
            BitArray = new bool[8];
            for (var i = BitArray.Length ; i > 0 ; i--)
            {
                _bitArray[i-1] = b%2 != 0;
                b = Convert.ToByte(b/2);
            }
        }

        public void FillFromUShort(ushort value)
        {
            var b = value;
            BitArray = new bool[16];
            for (var i = BitArray.Length; i > 0; i--)
            {
                _bitArray[i - 1] = b % 2 != 0;
                b = Convert.ToUInt16(b / 2);
            }
        }

        public void FillFromString(string bitsString)
        {
            if (bitsString.Any(c=> c != '0' && c!='1'))
                throw new InvalidCastException("The string contains invalid at least 1 incorrect char");
            BitArray = new bool[bitsString.Length];
            for (var i = 0; i < BitArray.Length; i++)
            {
                _bitArray[i] = bitsString[i] == '1';
            }
        }

        public void CreateFromBits(params Bits[] bits)
        {
            CreateFromBitsList(bits);
        }

        public void CreateFromBitsList(IEnumerable<Bits> bits)
        {
            _bitArray = bits.Select(b => b._bitArray).Aggregate((bools, bools1) => bools.Concat(bools1).ToArray());
        }

        public Bits GetMSB()
        {
            if (_bitArray.Length < 8)
                throw new InvalidDataException("Not Enough bits to make a byte");
            var b = new Bits();
            b.FillFromList(_bitArray.Take(8));
            return b;
        }

        public Bits GetLSB()
        {
            if (_bitArray.Length < 8)
                throw new InvalidDataException("Not Enough bits to make a byte");
            var b = new Bits();
            b.FillFromList(_bitArray.Skip(_bitArray.Length-8));
            return b;
        }

        public Bits GetMSQ()
        {
            if (_bitArray.Length < 4)
                throw new InvalidDataException("Not Enough bits to make a byte");
            var b = new Bits();
            b.FillFromList(_bitArray.Take(4));
            return b;
        }

        public Bits GetLSQ()
        {
            if (_bitArray.Length < 4)
                throw new InvalidDataException("Not Enough bits to make a byte");
            var b = new Bits();
            b.FillFromList(_bitArray.Skip(_bitArray.Length - 4));
            return b;
        }

        public Bits GetMSBit()
        {
            return GetFirstBits(1);
        }

        public Bits GetLSBit()
        {
            return GetLastBits(1);
        }

        public Bits GetFirstBits(int bitsCount)
        {
            if (_bitArray.Length < bitsCount)
                throw new InvalidDataException("Not Enough bits to take");
            var b = new Bits();
            b.FillFromList(_bitArray.Take(bitsCount));
            return b;
        }

        public Bits GetLastBits(int bitsCount)
        {
            if (_bitArray.Length < bitsCount)
                throw new InvalidDataException("Not Enough bits to take");
            var b = new Bits();
            b.FillFromList(_bitArray.Skip(_bitArray.Length - bitsCount));
            return b;
        }

        public Bits GetInverted()
        {
            var b = new Bits {_bitArray = new bool[_bitArray.Length]};
            for (var i = 0; i < _bitArray.Length; i++)
            {
                b._bitArray[_bitArray.Length - 1 - i] = _bitArray[i];
            }
            return b;
        }

        public void ClearStart()
        {
            var b = _bitArray.ToList();
            while (!b[0])
                b.RemoveAt(0);
        }

        

        public static Bits operator &(Bits b1, Bits b2)
        {
            var bitsRes = new Bits {_bitArray = new bool[Math.Min(b1._bitArray.Length, b2._bitArray.Length)]};
            for (var i = bitsRes._bitArray.Length; i > 0; i--)
                bitsRes._bitArray[bitsRes.BitArray.Length - i] = b1.BitArray[b1.BitArray.Length - i] & b2.BitArray[b2.BitArray.Length - i];
            
            return bitsRes;
        }

        public static bool operator ==(Bits b1, Bits b2)
        {
            if (b1._bitArray == null)
                return b2._bitArray == null;
            if (b1._bitArray.Length != b2?._bitArray.Length)
                return false;
            return !b1._bitArray.Where((t, i) => t != b2._bitArray[i]).Any();
        }

        public static bool operator !=(Bits b1, Bits b2)
        {
            return !(b1 == b2);
        }

        public static implicit operator byte (Bits x)
        {
            if(x.BitArray.Length > 8)
                throw new InvalidCastException("Too Many bits");
            return x.BitArray.Aggregate<bool, byte>(0, (current, b1) => Convert.ToByte(current * 2 + (b1 ? 1 : 0)));
        }

        public static implicit operator string(Bits x)
        {
            return x.BitArray.Aggregate(string.Empty, (current, b) => current + (b ? "1" : "0"));
        }

    }

    public class Coordonates
    {
        private const string TOP = "Top";
        private const string BOTTOM = "Bottom";
        private const string LEFT = "Left";
        private const string RIGHT = "Right";

        private string _coordonate;
        public static readonly Coordonates Top = new Coordonates() {_coordonate = TOP};
        public static readonly Coordonates Bottom = new Coordonates() {_coordonate = BOTTOM};
        public static readonly Coordonates Left = new Coordonates() {_coordonate = LEFT};
        public static readonly Coordonates Right = new Coordonates() {_coordonate = RIGHT};

        private Coordonates()
        {
        }

        public override string ToString()
        {
            return _coordonate;
        }
    }

    /// <summary>
    /// Class d'extension pour les chaines de caractères
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Fonction similaire au "LIKE '%toCompare%'" en sql
        /// </summary>
        /// <param name="src">La chaine this à tester</param>
        /// <param name="toCompare">La chaine que l'on veut trouver dans this</param>
        /// <returns>True si la chaine à comparer est contenue (au sens large) dans this, False sinon</returns>
        public static bool ContainsOrNull(this string src, string toCompare)
        {
            if (string.IsNullOrEmpty(toCompare))
                return true;
            return src.ToLower().IndexOf(toCompare.ToLower(), StringComparison.Ordinal) >= 0;
        }
		
		/// <summary>
        /// Move the text to the right
        /// </summary>
        /// <param name="s">this string to indent</param>
        /// <param name="level">The level of indentation</param>
        /// <param name="indentString">The string used to indent (\t by default)</param>
        /// <returns>This indented string</returns>
        public static string Indent(this string s, int level, string indentString = null)
        {
            var ret = string.Empty;
            for (var i = 0; i < level; i++)
                ret += !string.IsNullOrEmpty(indentString) ? indentString : "\t";
            return ret + s;
        }
    }

    /// <summary>
    /// Class d'extension pour les objet DateTime
    /// </summary>
    public static class DateExtensions
    {
        /// <summary>
        /// Transforme this en entier
        /// </summary>
        /// <param name="date">La date yhis que l'on veut sous forme d'entier</param>
        /// <returns>this sous la forme YYYYMMDD</returns>
        public static int ToInt(this DateTime date)
        {
            return (date.Year*100 + date.Month)*100 + date.Day;
        }

        /// <summary>
        /// Convertit la date en miliseconds (de manière approximative : 1 mois = 30.4375 jours (moyenne sur 4 ans (>à la réalité)) ((365+365+365+366)/48)) 
        /// sans verification d'overflow
        /// </summary>
        /// <param name="dateTime">La date this que l'on veut en millisecondes</param>
        /// <returns>this en millisedondes</returns>
        public static int ToMillis(this DateTime dateTime)
        {
            return (int) ((((((((dateTime.Year)*12 + dateTime.Month - 1)*30.4375 + dateTime.Day)*24 + dateTime.Hour)*60 + dateTime.Minute)*60 + dateTime.Second)*1000 + dateTime.Millisecond)%Int32.MaxValue);
        }
    }
	
	public class TreeNode : TreeViewItem
    {
        public int Level { get; set; }
        public Brush ForeGround { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public void AddToLast(TreeNode node)
        {
            if (node.Level == 0 || node.Level - 1 == Level)
            {
                Items.Add(node);
                return;
            }

            Items.Cast<TreeNode>().Last().AddToLast(node);
        }

        public override string ToString()
        {
            var str = Header.ToString().Indent(Level) + "\n";
            return Items.Cast<object>().Aggregate(str, (current, item) => current + item.ToString());
        }

        public TreeViewItem AsTreeViewItem()
        {
                var t = new TreeViewItem { Header = new Label() { Content = Header, Foreground = ForeGround ?? Brushes.Black, Padding = new Thickness(0, 0, 0, 0) } };
                if (Items.IsEmpty)
                    return t;
                var it = (from object item in Items select (item as TreeNode).AsTreeViewItem()).ToList();
                
                foreach (var treeViewItem in it)
                {
                    if (!t.Items.Contains(treeViewItem))
                        t.Items.Add(treeViewItem);
                }
                return t;
        }
    }

    public static class TreeViewExtension
    {
        public static void GenerateTree(this TreeView treeView, TreeNode tree)
        {
            var items = tree.AsTreeViewItem();
            treeView.Items.Add(items);
        }
    }
}
