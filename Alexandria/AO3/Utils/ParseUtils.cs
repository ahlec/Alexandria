// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace Alexandria.AO3.Utils
{
    internal static class ParseUtils
    {
        public static IEnumerable<Tuple<string, HtmlNode>> EnumerateDlTable( this HtmlNode dl )
        {
            string lastDtText = null;
            foreach ( HtmlNode child in dl.ChildNodes )
            {
                if ( child.Name.Equals( "dt" ) )
                {
                    lastDtText = child.InnerText.Trim().TrimEnd( ':' );
                    continue;
                }

                if ( !child.Name.Equals( "dd" ) )
                {
                    continue;
                }

                yield return new Tuple<string, HtmlNode>( lastDtText, child );
                lastDtText = null;
            }

            if ( lastDtText != null )
            {
                yield return new Tuple<string, HtmlNode>( lastDtText, null );
            }
        }
    }
}
