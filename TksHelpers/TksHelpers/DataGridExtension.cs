using System.Windows.Controls;

namespace TksHelpers
{
    public static class DataGridExtension
    {
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
    }
}
