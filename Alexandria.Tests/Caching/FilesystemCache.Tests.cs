// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Text;
using Alexandria.Caching;
using Alexandria.Net;
using HtmlAgilityPack;
using NUnit.Framework;

namespace Alexandria.Tests.Caching
{
    [TestFixture]
    public sealed class FilesystemCacheTests
    {
        const string Directory = @"c:\cache\";

        [Test]
        public void FilesystemCache_Contains_ReturnsFalseIfNoFile()
        {
            IFileSystem filesystem = CreateFileSystem();
            FilesystemCache cache = new FilesystemCache( filesystem, Directory );
            Assert.That( cache.Contains( "does-not-exist" ), Is.False );
        }

        [Test]
        public void FilesystemCache_Contains_ReturnsTrueIfFileExists()
        {
            const string Handle = "my-file";
            IFileSystem filesystem = CreateFileSystem();
            FilesystemCache cache = new FilesystemCache( filesystem, Directory );
            string filename = cache.ConstructFileName( Handle );
            filesystem.File.Create( filename );
            Assert.That( cache.Contains( Handle ), Is.True );
        }

        [Test]
        public void FilesystemCache_RemoveItem_NoErrorsOnNonPresentFile()
        {
            IFileSystem filesystem = CreateFileSystem();
            FilesystemCache cache = new FilesystemCache( filesystem, Directory );
            Assert.DoesNotThrow( () => cache.RemoveItem( "does-not-exist" ) );
        }

        [Test]
        public void FilesystemCache_RemoveItem_RemovesFileWhenPresent()
        {
            const string Handle = "my-file";
            IFileSystem filesystem = CreateFileSystem();
            FilesystemCache cache = new FilesystemCache( filesystem, Directory );

            string filename = cache.ConstructFileName( Handle );
            filesystem.File.Create( filename );

            Assert.DoesNotThrow( () => cache.RemoveItem( Handle ) );

            Assert.That( filesystem.File.Exists( filename ), Is.False );
        }

        [Test]
        public void FilesystemCache_TryReadFromCache_ReturnsFalseIfNotPresent()
        {
            IFileSystem filesystem = CreateFileSystem();
            FilesystemCache cache = new FilesystemCache( filesystem, Directory );
            Assert.That( cache.TryReadFromCache( "does-not-exist", out Document document ), Is.False );
            Assert.That( document, Is.Null );
        }

        [Test]
        public void FilesystemCache_TryReadFromCache_ReturnsTrueIfPresent()
        {
            const string Handle = "mock-document";
            IFileSystem filesystem = CreateFileSystem();
            FilesystemCache cache = new FilesystemCache( filesystem, Directory );

            Document mockDocument = CreateMockFile( Handle );
            cache.WriteToCache( mockDocument );

            Assert.That( cache.TryReadFromCache( Handle, out Document document ), Is.True );
            Assert.That( document, Is.Not.Null );
            Assert.That( document.Url, Is.EqualTo( mockDocument.Url ) );
        }

        [Test]
        public void FilesystemCache_WriteToCache_CreatesFileForNewEntry()
        {
            const string Handle = "mock-document";
            IFileSystem filesystem = CreateFileSystem();
            FilesystemCache cache = new FilesystemCache( filesystem, Directory );

            Document mockDocument = CreateMockFile( Handle );
            string filename = cache.ConstructFileName( Handle );

            Assert.That( filesystem.File.Exists( filename ), Is.False );
            Assert.DoesNotThrow( () => cache.WriteToCache( mockDocument ) );
            Assert.That( filesystem.File.Exists( filename ), Is.True );
        }

        [Test]
        public void FilesystemCache_WriteToCache_OverwritesFileForExistingEntries()
        {
            const string Handle = "mock-document";
            IFileSystem filesystem = CreateFileSystem();
            FilesystemCache cache = new FilesystemCache( filesystem, Directory );

            string filename = cache.ConstructFileName( Handle );
            filesystem.File.Create( filename );

            Document mockDocument = CreateMockFile( Handle );

            Assert.That( filesystem.File.Exists( filename ), Is.True );
            Assert.DoesNotThrow( () => cache.WriteToCache( mockDocument ) );
        }

        [Test]
        public void FilesystemCache_GetCachedDocument_ReturnsSuccessfully()
        {
            const string Handle = "mock-document";
            IFileSystem filesystem = CreateFileSystem();
            FilesystemCache cache = new FilesystemCache( filesystem, Directory );

            Document input = CreateMockFile( Handle );
            cache.WriteToCache( input );

            CachedDocument cached = cache.GetCachedDocument( Handle );
            Assert.That( cached, Is.Not.Null );

            Document output = Document.ReadFromStream( Handle, cached.Url, cached.Stream );
            Assert.That( output.Url, Is.EqualTo( input.Url ) );
            Assert.That( output.Html.OuterHtml, Is.EqualTo( input.Html.OuterHtml ) );
        }

        static IFileSystem CreateFileSystem()
        {
            IFileSystem filesystem = new MockFileSystem();
            filesystem.Directory.CreateDirectory( Directory );
            return filesystem;
        }

        static Document CreateMockFile( string handle )
        {
            byte[] textBytes = Encoding.UTF8.GetBytes( "<html></html>" );
            using ( MemoryStream webResponseStream = new MemoryStream( textBytes ) )
            {
                WebResult mockWebResult = new WebResult( new Uri( "http://github.com/ahlec/" ), webResponseStream );
                Document document = Document.ParseFromWebResult( Website.AO3, handle, mockWebResult );
                return document;
            }
        }
    }
}
