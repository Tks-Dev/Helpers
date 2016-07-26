namespace TksHelpers
{
    public static class StringExtension
    {
        /// <summary>
        /// SQL "Like" instruction
        /// </summary>
        /// <param name="src">this to test</param>
        /// <param name="toCompare">string to search in this</param>
        /// <returns>True if searched string is null or is in this</returns>
        public static bool ContainsOrNull(this string src, string toCompare)
        {
            return string.IsNullOrEmpty(toCompare) || src.ToLower().Contains(toCompare.ToLower());
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
}
