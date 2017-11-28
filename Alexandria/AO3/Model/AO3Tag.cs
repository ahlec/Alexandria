// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Alexandria.Model;
using Alexandria.RequestHandles;
using HtmlAgilityPack;

namespace Alexandria.AO3.Model
{
    /// <summary>
    /// A concrete class for parsing a tag from AO3.
    /// <para />
    /// This class can parse a ship or a character tag (because we don't always know what
    /// a tag is ahead of time) whereas <seealso cref="AO3Ship"/> and <seealso cref="AO3Character"/>
    /// cannot parse something that isn't exactly the type that they are.
    /// </summary>
    internal sealed class AO3Tag : AO3TagBase<AO3Tag>, ITag
    {
        AO3Tag( AO3Source source, Uri url, HtmlNode mainDiv )
            : base( source, url, mainDiv )
        {
            Type = ParseTagType( mainDiv, source.Website, url );
            ParentTags = ParseParentTags( this, mainDiv );
            SynonymousTags = ParseSynonymousTags( this, mainDiv );
        }

        /// <summary>
        /// Gets the type of the tag.
        /// </summary>
        public TagType Type { get; }

        /// <summary>
        /// Gets any parent tags that this tag might have (tags which conceptually would
        /// encompass this tag alongside other tags, if this website supports that).
        /// </summary>
        public IReadOnlyList<ITagRequestHandle> ParentTags { get; }

        /// <summary>
        /// Gets any other tags which have the same meaning as this tag but which are perhaps
        /// written a different way (for instance, a ship tag AAAA/BBBB might have a synonymous
        /// tag of BBBB/AAAA).
        /// </summary>
        public IReadOnlyList<ITagRequestHandle> SynonymousTags { get; }

        /// <summary>
        /// Parses an HTML page into an instance of an <seealso cref="AO3Tag"/>.
        /// </summary>
        /// <param name="source">The source that the HTML page came from, which is then stored for
        /// querying fanfics and also passed along to any nested request handles for them to parse
        /// data with as well.</param>
        /// <param name="document">The document that came from the website itself.</param>
        /// <returns>An instance of <seealso cref="AO3Tag"/> that was parsed and configured using
        /// the information provided.</returns>
        public static AO3Tag Parse( AO3Source source, Document document )
        {
            HtmlNode mainDiv = GetMainDiv( document );
            return new AO3Tag( source, document.Url, mainDiv );
        }
    }
}
