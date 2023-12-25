namespace SpracheBinary
{
    /// <summary>
    /// Represents a customizable comment parser.
    /// </summary>
    [System.Obsolete("Don't need this anymore")]
    public interface IComment
    {
        ///<summary>
        /// Single-line comment header.
        ///</summary>
        [System.Obsolete("Don't need this anymore")]
        string Single { get; set; }

        ///<summary>
        /// Newline character preference.
        ///</summary>
        [System.Obsolete("Don't need this anymore")]
        string NewLine { get; set; }

        ///<summary>
        /// Multi-line comment opener.
        ///</summary>
        [System.Obsolete("Don't need this anymore")]
        string MultiOpen { get; set; }

        ///<summary>
        /// Multi-line comment closer.
        ///</summary>
        [System.Obsolete("Don't need this anymore")]
        string MultiClose { get; set; }

        ///<summary>
        /// Parse a single-line comment.
        ///</summary>
        [System.Obsolete("Don't need this anymore")]
        Parser<string> SingleLineComment { get; }

        ///<summary>
        /// Parse a multi-line comment.
        ///</summary>
        [System.Obsolete("Don't need this anymore")]
        Parser<string> MultiLineComment { get; }

        ///<summary>
        /// Parse a comment.
        ///</summary>
        [System.Obsolete("Don't need this anymore")]
        Parser<string> AnyComment { get; }
    }
}
