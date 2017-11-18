// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Alexandria.AO3.RequestHandles;
using Alexandria.Documents;
using Alexandria.Exceptions.Parsing;
using Alexandria.Model;
using Alexandria.RequestHandles;
using HtmlAgilityPack;

namespace Alexandria.AO3.Model
{
    /// <summary>
    /// A concrete class for parsing a tag from AO3.
    /// </summary>
    internal sealed class AO3Character : AO3TagBase, ICharacter
    {
        AO3Character( AO3Source source, Uri url, HtmlNode mainDiv )
            : base( source, url, mainDiv )
        {
            Fandoms = ParseParentTags( mainDiv );
            AlternateNames = ParseAlternateNames( mainDiv );
            Relationships = ParseChildRelationshipTags( mainDiv );
        }

        /// <inheritdoc />
        public IReadOnlyList<ITagRequestHandle> Fandoms { get; }

        /// <inheritdoc />
        public IReadOnlyList<ICharacterRequestHandle> AlternateNames { get; }

        /// <inheritdoc />
        public IReadOnlyList<IShipRequestHandle> Relationships { get; }

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

        IReadOnlyList<ICharacterRequestHandle> ParseAlternateNames( HtmlNode mainDiv )
        {
            List<ICharacterRequestHandle> alternateNames = new List<ICharacterRequestHandle>();

            foreach ( ITagRequestHandle requestHandle in ParseSynonymousTags( mainDiv ) )
            {
                alternateNames.Add( new AO3CharacterRequestHandle( Source, requestHandle.Text ) );
            }

            return alternateNames;
        }
    }
}
