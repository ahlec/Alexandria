// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using HtmlAgilityPack;

namespace Alexandria.Utils
{
    internal static class HtmlUtils
    {
        public const string OptionsHtmlTag = "my_option";

        public static HtmlDocument ParseHtmlDocument( string html )
        {
            // There's some super-bizzaro thing with HtmlAgilityPack where it doesn't recognise </option>.
            // http://stackoverflow.com/questions/293342/htmlagilitypack-drops-option-end-tags
            // Just replacing the tag name altogether to something else, it doesn't matter, we're not going to pay attention
            // to it right now.
            const string OptionsOpenTagReplacement = "<" + OptionsHtmlTag + " ";
            const string OptionsCloseTagReplacement = OptionsHtmlTag + ">";
            html = html.Replace( "<option ", OptionsOpenTagReplacement ).Replace( "option>", OptionsCloseTagReplacement );

            HtmlDocument document = new HtmlDocument();
            byte[] bytes = Encoding.UTF8.GetBytes( html );
            using ( Stream textStream = new MemoryStream( bytes ) )
            {
                document.Load( textStream, Encoding.UTF8 );
            }

            if ( document.ParseErrors.Any() )
            {
                throw new ApplicationException( GetExceptionMessageFromParseErrors( document ) );
            }

            return document;
        }

        public static string ReadableInnerText( this HtmlNode node )
        {
            // Strip out all of the HTML tags, EXCEPT for <br> and <br /> and <p>, which should be transformed into newline characters
            string innerHtml = node.InnerHtml;
            StringBuilder builder = new StringBuilder( innerHtml.Length );
            bool currentlyInsideTag = false;
            int currentTagIndex = 0;
            bool wasLinebreakTag = false;
            char firstCharacterInTag = '\0';
            foreach ( char character in innerHtml )
            {
                if ( character == '<' )
                {
                    currentlyInsideTag = true;
                    currentTagIndex = 0;
                    wasLinebreakTag = false;
                    continue;
                }

                if ( character == '>' )
                {
                    currentlyInsideTag = false;
                    if ( wasLinebreakTag )
                    {
                        builder.AppendLine();
                    }

                    wasLinebreakTag = false;
                    continue;
                }

                if ( !currentlyInsideTag )
                {
                    builder.Append( character );
                }
                else
                {
                    switch ( currentTagIndex )
                    {
                        case 0:
                        {
                            wasLinebreakTag = ( character == 'b' || character == 'B' || character == 'p' || character == 'P' );
                            firstCharacterInTag = character;
                            break;
                        }

                        case 1:
                        {
                            if ( wasLinebreakTag )
                            {
                                if ( firstCharacterInTag == 'b' || firstCharacterInTag == 'B' )
                                {
                                    wasLinebreakTag = ( character == 'r' || character == 'R' );
                                }
                                else
                                {
                                    wasLinebreakTag = char.IsWhiteSpace( character );
                                }
                            }

                            break;
                        }

                        default:
                        {
                            wasLinebreakTag = wasLinebreakTag && char.IsWhiteSpace( character );
                            break;
                        }
                    }

                    currentTagIndex++;
                }
            }

            string text = HttpUtility.HtmlDecode( builder.ToString() );
            text = text.Trim();

            if ( string.IsNullOrEmpty( text ) )
            {
                return null;
            }

            return text;
        }

        static string GetExceptionMessageFromParseErrors( HtmlDocument document )
        {
            StringBuilder exceptionMessage = new StringBuilder( $"There were {document.ParseErrors.Count()} parsing errors:" );
            exceptionMessage.AppendLine();
            foreach ( HtmlParseError error in document.ParseErrors )
            {
                exceptionMessage.AppendLine( $"- Line {error.Line} Col {error.LinePosition}: {error.Reason} [{error.Code}]" );
            }

            return exceptionMessage.ToString();
        }
    }
}
