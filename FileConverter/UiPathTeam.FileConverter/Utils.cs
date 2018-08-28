namespace UiPathTeam.FileConverter
{
    public static partial class Utils
    {


        /// <summary>
        ///  if the file is inside our current directory, we remove the absolute part of the URL
        /// </summary> 
        public static string TrimFilePath(string initialPath, string absolutePath)
        {
            if (initialPath.StartsWith(absolutePath))
            {
                return initialPath.Remove(0, absolutePath.Length).TrimStart('\\');
            }

            return initialPath;
        }

    }
}
