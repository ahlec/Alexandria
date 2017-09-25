// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://bitbucket.org/ahlec/alexandria/).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Alexandria.Documents;
using Alexandria.Model;
using Alexandria.RequestHandles;
using HtmlAgilityPack;

namespace Alexandria.AO3.Model
{
    internal sealed class AO3Ship : IShip
    {
        AO3Ship( Uri url )
        {
            Url = url;
        }

        public Uri Url { get; }

        public string Name { get; private set; }

        public ShipType Type { get; private set; }

        public IReadOnlyList<ICharacterRequestHandle> Characters { get; private set; }

        internal static AO3Ship Parse( HtmlCacheableDocument document )
        {
            throw new NotImplementedException();
        }
    }
}
