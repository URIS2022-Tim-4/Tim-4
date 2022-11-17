using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using Emby.Naming.Common;

namespace Emby.Naming.AudioBook
{
    /// <summary>
    /// Parser class to extract part and/or chapter number from audiobook filename.
    /// </summary>
    public class AudioBookFilePathParser
    {
        private readonly NamingOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioBookFilePathParser"/> class.
        /// </summary>
        /// <param name="options">Naming options containing AudioBookPartsExpressions.</param>
        public AudioBookFilePathParser(NamingOptions options)
        {
            _options = options;
        }

        /// <summary>
        /// Based on regex determines if filename includes part/chapter number.
        /// </summary>
        /// <param name="path">Path to audiobook file.</param>
        /// <returns>Returns <see cref="AudioBookFilePathParser"/> object.</returns>
        public AudioBookFilePathParserResult Parse(string path)
        {
            AudioBookFilePathParserResult result = default;
            var fileName = Path.GetFileNameWithoutExtension(path);
            foreach (var expression in _options.AudioBookPartsExpressions)
            {
                var match = new Regex(expression, RegexOptions.IgnoreCase).Match(fileName);
                if (match.Success && (!result.ChapterNumber.HasValue) && (!result.PartNumber.HasValue))
                {
                        var valueC = match.Groups["chapter"];
                        if (valueC.Success && int.TryParse(valueC.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intValueC))
                        {
                            result.ChapterNumber = intValueC;
                        }

                        var valueP = match.Groups["part"];
                        if (valueP.Success && int.TryParse(valueP.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intValueP))
                        {
                            result.PartNumber = intValueP;
                        }
                }
            }

            return result;
        }
    }
}
