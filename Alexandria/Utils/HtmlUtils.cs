using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using HtmlAgilityPack;

namespace Alexandria.Utils
{
	public static class HtmlUtils
	{
		public const String OptionsHtmlTag = "my_option";

		public static HtmlDocument GetWebPage( String endpoint )
		{
			return GetWebPage( endpoint, out Uri _ );
		}

		public static HtmlDocument GetWebPage( String endpoint, out Uri responseUri )
		{
			HttpWebRequest request = (HttpWebRequest) WebRequest.Create( endpoint );
			request.Method = "GET";
			HttpWebResponse response = (HttpWebResponse) request.GetResponse();
			responseUri = response.ResponseUri;

			if ( response.StatusCode != HttpStatusCode.OK )
			{
				throw new ApplicationException( $"The page {endpoint} resulted in a {response.StatusCode} error code." );
			}

			String text;
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
			const String OptionsOpenTagReplacement = "<" + OptionsHtmlTag + " ";
			const String OptionsCloseTagReplacement = OptionsHtmlTag + ">";
			text = text.Replace( "<option ", OptionsOpenTagReplacement ).Replace( "option>", OptionsCloseTagReplacement );

			HtmlDocument document = new HtmlDocument();
			Byte[] bytes = Encoding.UTF8.GetBytes( text );
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
	}
}
