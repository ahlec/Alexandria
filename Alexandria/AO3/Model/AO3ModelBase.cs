// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Alexandria.AO3.RequestHandles;
using Alexandria.RequestHandles;
using HtmlAgilityPack;

namespace Alexandria.AO3.Model
{
    /// <summary>
    /// A base class for all concrete classes of <seealso cref="IRequestable"/>. This contains shared
    /// code and share interface implementations that will allow for getting a model up without boilerplate.
    /// </summary>
    /// <typeparam name="TSelf">The child class itself. We use this for working with other generic functions to
    /// pass in `this` (so to speak) in order to prevent boxing internally.</typeparam>
    internal abstract class AO3ModelBase<TSelf> : HtmlParserBase, IRequestable
        where TSelf : AO3ModelBase<TSelf>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AO3ModelBase{TSelf}"/> class.
        /// </summary>
        /// <param name="source">The configured concrete class that can be used to make requests of
        /// the particular website.</param>
        /// <param name="url">The URL for this tag page on the website.</param>
        protected AO3ModelBase( AO3Source source, Uri url )
        {
            Source = source;
            Url = url;
        }

        /// <summary>
        /// A mutator function that can be used with <seealso cref="ParseDlTable"/> in order to parse
        /// any of AO3's &lt;dl&gt; tables efficiently.
        /// </summary>
        /// <param name="self">The instance of the concrete class that should be mutated based on what the
        /// value of the &lt;dd&gt; cell in the &lt;dl&gt; table was.</param>
        /// <param name="value">The value of the &lt;dd&gt; cell in the &lt;dl&gt; table.</param>
        protected delegate void TableFieldMutator( AO3Source source, TSelf self, HtmlNode value );

        /// <summary>
        /// A delegate that can be used to create a new instance of a specific request handle.
        /// </summary>
        /// <typeparam name="TModel">The interface of the model that is returned by the request handle.</typeparam>
        /// <typeparam name="TRequestHandle">The interface of the specific request handle itself.</typeparam>
        /// <param name="self">The instance of the <seealso cref="AO3ModelBase{TSelf}"/>.</param>
        /// <param name="text">The tag that the request handle should request data on.</param>
        /// <returns>A fully constructed and valid request handle that can be used to retrieve the data as desired.</returns>
        protected delegate TRequestHandle RequestHandleCreatorFunc<TModel, out TRequestHandle>( TSelf self, string text )
            where TModel : IRequestable
            where TRequestHandle : IRequestHandle<TModel>;

        /// <summary>
        /// Possibles choices for how &lt;dt&gt; nodes encountered in a &lt;dl&gt; table on AO3 should be interpretted
        /// by <seealso cref="ParseDlTable"/>.
        /// </summary>
        protected enum DlFieldSource
        {
            /// <summary>
            /// The @class attribute of the &lt;dt&gt; should be retrieved.
            /// </summary>
            DtClass,

            /// <summary>
            /// The human-readable inner text of the &lt;dt&gt; should be used (with any following colons - ':' - removed).
            /// </summary>
            DtText
        }

        /// <inheritdoc />
        public Website Website => Source.Website;

        /// <inheritdoc />
        public Uri Url { get; }

        /// <summary>
        /// Gets the configured concrete class that can be used to construct further request handles of
        /// the particular website.
        /// </summary>
        protected AO3Source Source { get; }

        /// <summary>
        /// Parses through an AO3 &lt;dl&gt; table and reads in the values expected from there, applying the values in various ways.
        /// The way that this function works is by providing a dictionary of mutators where the key of the dictionary is the name of
        /// the &lt;dl&gt; field, and the accompanying value is a mutator function that should be fed the value of that row. In this
        /// way, it's easy for us to skip all of the HTML parsing necessary to interpret the table and focus exclusively on what each
        /// value in the table is supposed to mean, which field it should be going to, and how that particular data value should be
        /// parsed.
        /// </summary>
        /// <param name="self">The instance of the concrete class that should be mutated.</param>
        /// <param name="dl">The HTML table node that the children should be iterated over.</param>
        /// <param name="mutators">A dictionary where the key is the name of a field in the table and the value is a function that
        /// should be invoked that will mutate the appropriate field on <paramref name="self"/> using the value of the row.</param>
        /// <param name="fieldSource">Indicates how each &lt;dt&gt; should be processed in order to get a key for the
        /// <paramref name="mutators"/> dictionary.</param>
        protected static void ParseDlTable( AO3Source source, TSelf self, HtmlNode dl, IReadOnlyDictionary<string, TableFieldMutator> mutators, DlFieldSource fieldSource )
        {
            foreach ( Tuple<string, HtmlNode> row in EnumerateDlTable( dl, fieldSource ) )
            {
                if ( !mutators.TryGetValue( row.Item1, out TableFieldMutator mutator ) )
                {
                    continue;
                }

                mutator( source, self, row.Item2 );
            }
        }

        /// <summary>
        /// Parses through a &lt;ul&gt; list of tags and parses every tag encountered into the correct type of request handle.
        /// <para />
        /// NOTE: This isn't meant for all &lt;ul&gt; containers; this is for a very specific, albeit rather common, use of the
        /// &lt;ul&gt; on AO3: a &lt;ul&gt; that contains &lt;li&gt; elements which therein contain &lt;a&gt; elements that have
        /// a @class attribute of "tag". These elements are links to tags (of various type, ie ship or character, etc).
        /// </summary>
        /// <typeparam name="TModel">The interface of the model that the request handles should resolve to.</typeparam>
        /// <typeparam name="TRequestHandle">The interface of the request handle that should be retrieved.</typeparam>
        /// <param name="self">The instance of the model that data is being parsed into.</param>
        /// <param name="ul">The &lt;ul&gt; element container that should be iterated over.</param>
        /// <param name="requestHandleCreator">A delegate that should be used to create an instance of the desired request handle type.</param>
        /// <returns>A list that contains valid request handles of the desired type. If &lt;ul&gt; was null or empty, this will return
        /// a list with no entries in it. (This function never returns null).</returns>
        protected static IReadOnlyList<TRequestHandle> ParseTagsUl<TModel, TRequestHandle>( TSelf self, HtmlNode ul, RequestHandleCreatorFunc<TModel, TRequestHandle> requestHandleCreator )
            where TModel : IRequestable
            where TRequestHandle : IRequestHandle<TModel>
        {
            if ( ul == null )
            {
                return new List<TRequestHandle>( 0 );
            }

            List<TRequestHandle> results = new List<TRequestHandle>();

            foreach ( HtmlNode li in ul.Elements( "li" ) )
            {
                HtmlNode tagA = li.Element( "a" );
                string aClass = tagA.GetAttributeValue( "class", string.Empty ) ?? string.Empty;
                if ( !aClass.Equals( "tag" ) )
                {
                    continue;
                }

                string tag = GetReadableInnerText( tagA ).Trim();
                TRequestHandle requestHandle = requestHandleCreator( self, tag );
                results.Add( requestHandle );
            }

            return results;
        }

        /// <summary>
        /// An easy-to-use version of <seealso cref="RequestHandleCreatorFunc{TModel,TRequestHandle}"/> that
        /// will create a <seealso cref="ICharacterRequestHandle"/>.
        /// </summary>
        /// <param name="self">The model that is being parsed into.</param>
        /// <param name="tag">The tag that is being parsed.</param>
        /// <returns>A valid, constructed character request handle.</returns>
        protected static ICharacterRequestHandle CharacterRequestHandleCreator( TSelf self, string tag )
        {
            return new AO3CharacterRequestHandle( self.Source, tag );
        }

        /// <summary>
        /// An easy-to-use version of <seealso cref="RequestHandleCreatorFunc{TModel,TRequestHandle}"/> that
        /// will create a <seealso cref="IShipRequestHandle"/>.
        /// </summary>
        /// <param name="self">The model that is being parsed into.</param>
        /// <param name="tag">The tag that is being parsed.</param>
        /// <returns>A valid, constructed ship request handle.</returns>
        protected static IShipRequestHandle ShipRequestHandleCreator( TSelf self, string tag )
        {
            return new AO3ShipRequestHandle( self.Source, tag );
        }

        /// <summary>
        /// An easy-to-use version of <seealso cref="RequestHandleCreatorFunc{TModel,TRequestHandle}"/> that
        /// will create a <seealso cref="ITagRequestHandle"/>.
        /// </summary>
        /// <param name="self">The model that is being parsed into.</param>
        /// <param name="tag">The tag that is being parsed.</param>
        /// <returns>A valid, constructed tag request handle.</returns>
        protected static ITagRequestHandle TagRequestHandleCreator( TSelf self, string tag )
        {
            return new AO3TagRequestHandle( self.Source, tag );
        }

        /// <summary>
        /// Parses through a container (of any type, AO3 uses a number of different containers here) that contain
        /// &lt;a&gt; tags with @rel = "author" to create author request handles.
        /// </summary>
        /// <param name="source">The configured concrete class that can be used to make requests of
        /// the particular website.</param>
        /// <param name="container">The HTML container element that contains the authors (if any) that should be
        /// parsed.</param>
        /// <returns>A list, never null, of all of the authors that were mentioned in the HTML container.</returns>
        protected static IReadOnlyList<IAuthorRequestHandle> ParseAuthorsList( AO3Source source, HtmlNode container )
        {
            if ( container == null )
            {
                return new List<IAuthorRequestHandle>( 0 );
            }

            List<IAuthorRequestHandle> authors = new List<IAuthorRequestHandle>();

            foreach ( HtmlNode authorA in container.Elements( "a" ) )
            {
                string aRel = authorA.GetAttributeValue( "rel", string.Empty ) ?? string.Empty;
                if ( !aRel.Equals( "author" ) )
                {
                    continue;
                }

                IAuthorRequestHandle requestHandle = AO3AuthorRequestHandle.Parse( source, authorA );
                authors.Add( requestHandle );
            }

            return authors;
        }

        static IEnumerable<Tuple<string, HtmlNode>> EnumerateDlTable( HtmlNode dl, DlFieldSource fieldSource )
        {
            string lastDtText = null;
            foreach ( HtmlNode child in dl.ChildNodes )
            {
                if ( child.Name.Equals( "dt" ) )
                {
                    lastDtText = GetDtField( child, fieldSource );
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

        static string GetDtField( HtmlNode dt, DlFieldSource fieldSource )
        {
            switch ( fieldSource )
            {
                case DlFieldSource.DtClass:
                    return dt.GetAttributeValue( "class", null );
                case DlFieldSource.DtText:
                    return dt.InnerText.Trim().TrimEnd( ':' );
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
