// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System.Text;
using System.Web;
using HtmlAgilityPack;

namespace Alexandria.Utils
{
    internal static class HtmlUtils
    {
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
    }
}
