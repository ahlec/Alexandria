# Alexandria #

**Alexandria** is a suite of libraries and applications written in C# that are meant to facilitate interaction with popular fanfiction websites. Major fanfiction websites such as
[Archive of Our Own (AO3)](http://archiveofourown.org) or [FanFiction.net (FF.net)](http://fanfiction.net) store a seemingly limitless wealth of fan-created content, but there are
no official APIs at the moment to interact with the data provided there. Furthermore, even if there were, these APIs would be site-specific. Alexandria seeks to create a uniform
API for interacting with the major fanfiction websites, and also provides a number of applications that will consume this in meaningful ways.

### Components ###

#### Alexandria ####

The eponymous project is the library which facilitates the backbone of the project. *Alexandria* provides interfaces, enumerations, and classes for interacting with fanfiction websites
in a generic yet meaningful way. The project operates by providing a programmer-friendly interface which is then translated into HTTP calls, which then downloads the HTML from the websites
and is decoded back into the interfaces and classes to allow for programmatic consumption. *Alexandria* does not require the consumer to have any knowledge of what is happening behind the scenes
and the interface is designed to, in fact, hide away what is really happening and focus on providing the data in a way that fits the consumer's needs.

At present, only AO3 (Archive of Our Own) is supported. However, future bindings for FF.net (as well as others after that) are intended as time permits.

#### Bibliothecary ####

A Windows service, *Bibliothecary* is designed to continually poll the fanfiction websites for specific search queries and then report new finds to the user as new fanfics or updates are
made public. These search queries are customisable without any need for a knowledge of programming or interacting with the code to create a special build, and numerous actions are provided built-in
to allow for customising what can be done when new data is found. *Bibliothecary* grew out of a desire to create a new, dedicated replacement for the popular ao3feed Tumblr blogs (eg,
[hijack-ao3feed](http://ao3feed-hijack.tumblr.com/) that are currently set up using [IFTTT](https://ifttt.com). While IFTTT is powerful and easy to use, the technology doesn't provide a large degree
of customization for the blog runner, either in terms of what queries they can be set up with, in how the posts are formatted when posted to Tumblr, or even in how many blogs can be configured
per account. Additionally, these are traditionally limited to only being able to post AO3-based fanfics, and so other fanfic websites are omitted, leaving readers to miss many otherwise fantastic
works. Addressing all of these problems while also allowing for the creation and management of these blogs by non-programmers is the goal of *Bibliothecary*.

#### HeadLibrarian ####

*HeadLibrarian* is the Windows desktop application that is used to congfigure *Bibliothecary*. Through this panel, new search queries (called "Projects") can be set up and configured, as well as the
various actions that *Bibliothecary* should undertake when a new fanfic is found. All configuration happens at the project-level. Through the shared design of *HeadLibrarian* and *Bibliothecary*, a
single instance of *Bibliothecary* is able to process hundreds of different projects at a time, polling each of them at different intervals and performing different actions for each project. Authentication
with Tumblr, configuration of email addresses, search query management, and more all happen within *HeadLibrarian*. Additionally, it provides an easy, visual way to be able to check on the status of
*Bibliothecary*, to start it, and to force various operations to happen.

### Requirements ###

* [Visual Studio 2017](https://www.visualstudio.com/downloads/) (or higher)
* [Microsoft .NET 4.7](https://www.microsoft.com/en-us/download/details.aspx?id=55168) (C# 7)

All libraries used within the suite are obtained via NuGet packages. These are presently:

* [HtmlAgilityPack](http://html-agility-pack.net/)
* [Newtonsoft.Json](http://newtonsoft.com/json)
* [NewTumblrSharp](https://github.com/piedoom/TumblrSharp)
    * [BouncyCastle-PCL](https://github.com/onovotny/BouncyCastle-PCL)
    * [Microsoft.Bcl](http://go.microsoft.com/fwlink/?LinkID=280057)
    * [Microsoft.Bcl.Build](http://go.microsoft.com/fwlink/?LinkID=296436)
* [System.Data.SQLite](http://system.data.sqlite.org/)
* [NLog](http://nlog-project.org/)
* [PubSub](http://github.com/upta/pubsub)

Not all projects will use all packages.

### Building ###

[MORE COMING SHORTLY]