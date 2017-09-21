// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://bitbucket.org/ahlec/alexandria/).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using HtmlAgilityPack;

namespace Alexandria.Utils
{
    public static class HtmlUtils
    {
        public const string OptionsHtmlTag = "my_option";

        public static HtmlDocument GetWebPage( string endpoint )
        {
            return GetWebPage( endpoint, out Uri _ );
        }

        public static HtmlDocument GetWebPage( string endpoint, out Uri responseUri )
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create( endpoint );
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            responseUri = response.ResponseUri;

            if ( response.StatusCode != HttpStatusCode.OK )
            {
                throw new ApplicationException( $"The page {endpoint} resulted in a {response.StatusCode} error code." );
            }

            string text;
            using ( Stream responseStream = response.GetResponseStream() )
            {
                if ( responseStream == null )
                {
                    throw new ApplicationException( "Response stream from the web request was null!" );
                }

                using ( StreamReader reader = new StreamReader( responseStream ) )
                {
                    text = reader.ReadToEnd();
                }
            }

            // There's some super-bizzaro thing with HtmlAgilityPack where it doesn't recognise </option>. I know that sounds
            // like I'm making bullshit up to cover my arse because "I'm not using it right" but here
            // http://stackoverflow.com/questions/293342/htmlagilitypack-drops-option-end-tags
            // Just replacing the tag name altogether to something else, it doesn't matter, we're not going to pay attention
            // to it right now.
            const string OptionsOpenTagReplacement = "<" + OptionsHtmlTag + " ";
            const string OptionsCloseTagReplacement = OptionsHtmlTag + ">";
            text = text.Replace( "<option ", OptionsOpenTagReplacement ).Replace( "option>", OptionsCloseTagReplacement );

            HtmlDocument document = new HtmlDocument();
            byte[] bytes = Encoding.UTF8.GetBytes( text );
            using ( Stream textStream = new MemoryStream( bytes ) )
            {
                document.Load( textStream, Encoding.UTF8 );
            }

            if ( document.ParseErrors.Any() )
            {
                StringBuilder exceptionMessage = new StringBuilder( $"The page {endpoint} resulted in {document.ParseErrors.Count()} error(s)" );
                exceptionMessage.AppendLine();
                foreach ( HtmlParseError error in document.ParseErrors )
                {
                    exceptionMessage.AppendLine( $"- Line {error.Line} Col {error.LinePosition}: {error.Reason} [{error.Code}]" );
                }

                throw new ApplicationException( exceptionMessage.ToString() );
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

            builder.Trim();

            string text = HttpUtility.HtmlDecode( builder.ToString() );
            return text;
        }
    }
}
