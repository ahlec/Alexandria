// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://bitbucket.org/ahlec/alexandria/).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using Alexandria.Documents;
using Alexandria.Model;

namespace Alexandria.AO3.Model
{
    internal sealed class AO3Character : ICharacter
    {
        AO3Character( Uri uri )
        {
            Url = uri;
        }

        public Uri Url { get; }

        public static AO3Character Parse( HtmlCacheableDocument document )
        {
            throw new NotImplementedException();
        }
    }
}
