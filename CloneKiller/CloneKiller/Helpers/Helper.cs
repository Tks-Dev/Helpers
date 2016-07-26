using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace CloneKiller.Helpers
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
            return (date.Year * 100 + date.Month) * 100 + date.Day; 
        }

        /// <summary>
        /// Convertit la date en miliseconds (de manière approximative : 1 mois = 30.4375 jours (moyenne sur 4 ans (>à la réalité)) ((365+365+365+366)/48)) 
        /// sans verification d'overflow
        /// </summary>
        /// <param name="dateTime">La date this que l'on veut en millisecondes</param>
        /// <returns>this en millisedondes</returns>
        public static int ToMillis(this DateTime dateTime)
        {
            return (int)((((((((dateTime.Year) * 12 + dateTime.Month - 1) * 30.4375 + dateTime.Day) * 24 + dateTime.Hour) * 60 + dateTime.Minute) * 60 + dateTime.Second) * 1000 + dateTime.Millisecond) % Int32.MaxValue);
        }
    }
}
