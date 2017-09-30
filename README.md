# Alexandria #

**Alexandria** is a C# library designed to provide a universal API for interacting with the various popular fanfiction websites available. Major fanfiction websites such as
[Archive of Our Own (AO3)](http://archiveofourown.org) or [FanFiction.net (FF.net)](http://fanfiction.net) store a seemingly limitless wealth of fan-created content, but there are
no official APIs at the moment to interact with the data provided there. Furthermore, even if there were, these APIs would be site-specific. Alexandria seeks to create a uniform
API for interacting with the major fanfiction websites.

The project operates by providing a programmer-friendly interface that is then translated into HTTP calls and executed, downloading the HTML pages from the websites and decoding them
back into interfaces and classes useful for programmers. The complexities of this--namely, making HTTP calls or working with HTML--is completely hidden from the programmer.

The end goal of Alexandria is to support many different websites as part of the project. Following the current release pattern, one website will be added in every major revision,
in order to not prevent the project from ever releasing; the order of these can be seen below under **Release Map**. Listed in no particular order are the websites we'd currently like
to provide support for:

* [Archive of Our Own (AO3)](http://archiveofourown.org)
* [FanFiction.net (FF.net)](https://www.fanfiction.net)
* [LiveJournal](https://www.livejournal.com) (if possible)

However, additional fanfiction websites beyond these are always welcome to be proposed.

### Release Map ###

The following are the goals of each release, as planned out thus far. These are the minimum goals, though additional features may be included in each version.

#### Release 1.0 ####

* Setup of reusable public framework that is extensible but still allows for specific features that are present on one website to be utilised.
* Establishing the internal backbone for the project
* Implementing the API for interacting with AO3 in a readonly manner
    * Validating of data (such as all input parameters) in a way that the user can publicly access as well, in order to provide end user protection against exceptions being thrown
    * All major endpoints should be present for accessing data
* Setting up of CI for the project
* Establishing all necessary unit tests related to the above points
    * Replacing any tight coupling with Ctor dependency injections
* Rewriting the original code to make it adhere to maintainability standards
* Full, comprehensive XMLDOC documentation on all public-facing functions
* Submitting package to NuGet

#### Release 2.0 ####

* Implement the API for interacting with FF.net in a readonly manner

### Requirements ###

* [Visual Studio 2017](https://www.visualstudio.com/downloads/) (or higher)
* [Microsoft .NET 4.5.2](https://www.microsoft.com/en-us/download/details.aspx?id=42637)

Alexandria makes use of the following NuGet packages:

* [HtmlAgilityPack](http://html-agility-pack.net/)
* [System.IO.Abstractions](https://github.com/tathamoddie/System.IO.Abstractions)

Alexandria.Tests makes use of all of the above NuGet packages, but also uses [NUnit](https://github.com/nunit/nunit) as its unit test framework.

Additional to these NuGet packages, Alexandria also uses [StyleCop.Analyzers](https://github.com/DotNetAnalyzers/StyleCopAnalyzers) and [ReSharper](https://www.jetbrains.com/resharper/) to ensure code consistency.
