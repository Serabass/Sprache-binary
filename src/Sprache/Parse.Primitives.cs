namespace SpracheBinary
{
    partial class Parse
    {
        /// <summary>
        /// Parser for identifier starting with <paramref name="firstLetterParser"/> and continuing with <paramref name="tailLetterParser"/>
        /// </summary>
        public static Parser<string> Identifier(Parser<char> firstLetterParser, Parser<char> tailLetterParser)
        {
            return
                from firstLetter in firstLetterParser
                from tail in tailLetterParser.Many().Text()
                select firstLetter + tail;
        }
    }
}