// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using Alexandria.Documents;
using Alexandria.Exceptions.Parsing;
using Alexandria.Model;
using HtmlAgilityPack;

namespace Alexandria.AO3.Model
{
    internal sealed class AO3Character : AO3TagBase, ICharacter
    {
        AO3Character( AO3Source source, Uri url, HtmlNode mainDiv )
            : base( source, url, mainDiv )
        {
        }

        public static AO3Character Parse( AO3Source source, HtmlCacheableDocument document )
        {
            HtmlNode mainDiv = GetMainDiv( document );

            TagType type = ParseTagType( mainDiv );
            if ( type != TagType.Character )
            {
                string name = ParseTagText( mainDiv );
                throw new InvalidTagTypeAlexandriaException( TagType.Character, type, name );
            }

            return new AO3Character( source, document.Url, mainDiv );
        }
    }
}
