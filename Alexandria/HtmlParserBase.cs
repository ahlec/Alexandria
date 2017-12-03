// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System.Text;
using System.Web;
using HtmlAgilityPack;

namespace Alexandria
{
    /// <summary>
    /// A base class for any class that interacts with parsing HTML. This provides utility functions that make
    /// parsing HTML much easier.
    /// </summary>
    internal abstract class HtmlParserBase
    {
        /// <summary>
        /// Like <seealso cref="HtmlNode.InnerText"/>, it will return all of the visible text in this node, without any
        /// HTML markup. However, unlike <seealso cref="HtmlNode.InnerText"/>, some HTML nodes will be interpretted before
        /// they are stripped out (for example, &lt;br /&gt; will be removed and replaced with a linebreak). This will produce
        /// text that has only visible characters and no HTML nodes, but which will also more closely visually resemble what
        /// it looks like when the text is rendered.
        /// </summary>
        /// <param name="node">The HTML node whose text should be retrieved.</param>
        /// <returns>If the node is null, or the node has no text, then this will return null. Otherwise, this will return a string
        /// which contains all of the text with the inner HTML nodes stripped out of it.</returns>
        protected static string GetReadableInnerText( HtmlNode node )
        {
            if ( node == null )
            {
                return null;
            }

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
