
namespace Sprache
{
    /// <summary>
    /// Constructs customizable comment parsers.
    /// </summary>
    [System.Obsolete("Don't need this anymore")]
    public class CommentParser : IComment
    {
        ///<summary>
        ///Single-line comment header.
        ///</summary>
        [System.Obsolete("Don't need this anymore")]
        public string Single { get; set; }

        ///<summary>
        ///Newline character preference.
        ///</summary>
        [System.Obsolete("Don't need this anymore")]
        public string NewLine { get; set; }

        ///<summary>
        ///Multi-line comment opener.
        ///</summary>
        [System.Obsolete("Don't need this anymore")]
        public string MultiOpen { get; set; }

        ///<summary>
        ///Multi-line comment closer.
        ///</summary>
        [System.Obsolete("Don't need this anymore")]
        public string MultiClose { get; set; }

        /// <summary>
        /// Initializes a Comment with C-style headers and Windows newlines.
        /// </summary>
        [System.Obsolete("Don't need this anymore")]
        public CommentParser()
        {
            Single = "//";
            MultiOpen = "/*";
            MultiClose = "*/";
            NewLine = "\n";
        }

        /// <summary>
        /// Initializes a Comment with custom multi-line headers and newline characters.
        /// Single-line headers are made null, it is assumed they would not be used.
        /// </summary>
        /// <param name="multiOpen"></param>
        /// <param name="multiClose"></param>
        /// <param name="newLine"></param>
        [System.Obsolete("Don't need this anymore")]
        public CommentParser(string multiOpen, string multiClose, string newLine)
        {
            Single = null;
            MultiOpen = multiOpen;
            MultiClose = multiClose;
            NewLine = newLine;
        }

        ///<summary>
        ///Parse a single-line comment.
        ///</summary>
        [System.Obsolete("Don't need this anymore")]
        public Parser<string> SingleLineComment
        {
            get
            {
                throw new System.NotImplementedException();
            }
            private set { }
        }

        ///<summary>
        ///Parse a multi-line comment.
        ///</summary>
        [System.Obsolete("Don't need this anymore")]
        public Parser<string> MultiLineComment
        {
            get
            {
                throw new System.NotImplementedException();
            }
            private set { }
        }

        ///<summary>
        ///Parse a comment.
        ///</summary>
        [System.Obsolete("Don't need this anymore")]
        public Parser<string> AnyComment
        {
            get
            {
                throw new System.NotImplementedException();
            }
            private set { }
        }
    }
}
