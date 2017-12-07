# Maintainability #

Maintainability is something that I really want to focus on for writing this application, but also for any open-source project. However,
because that can be a little open-ended, this documentation exists to explain the concrete rules for what that means for this project and
what programming rules should be followed in able to participate or submit code into the project.

Most of these (all of them?) come from other resources I've found. In particular, I'm a *huge* fan of [Building Maintainable Software
(C# Edition)](https://www.amazon.com/dp/1491954523/_encoding=UTF8?coliid=I88HOLC70H5D&colid=2G8WSACWRS6UK&psc=0) by Joost Visser et al.
The book was really what opened me up to the idea that my formatting and composition was just as important as the code itself that I was
writing, and sparked a new wave for me.

### 1. Functions should be ~15 lines long. ###

More than anything, this really forces you to write functions that only attempt to accomplish one thing. The limit of 15 is a bit loose with
Alexandria, and there will be a number of functions that go over it. The general idea, however, is to limit your functions to right around
15 lines (or less), which definitely keeps you from trying to do too many things with a single function.

Also, it keeps the code way easier to read. I loathe reading long functions because the indentation gets really confusing and I don't remember
what variables are what when they're declared offscreen.

### 2. Functions should take ~4 parameters or less. ###

Again, something that for Alexandria is a bit loose, but the general idea is to keep the number of parameters low. Granted, when you have
15 lines of code or less to write your code, it's already pretty difficult to *use* more than four parameters. However, the idea behind limiting
the parameters is that it forces you to evaluate if you *need* all of the parameters, and if you're passing them around in the most ideal form.
For example, if you're passing in as the last four parameters: the author's username; the author's URL; the author's profile picture URL; and
the number of works that the author has published, that looks like you could simplify all of that to just passing in a new data model that contains
all of those.

I was dubious at first whether this was actually possible, and some cases it's *way* more difficult, but with some refactoring I've not found cases
where I've really needed to go above 4.

### 3. No utility classes. ###

This is one that I've tried to really stomp out in Alexandria. I used to be REALLY keen on utility classes, but after it was pointed out that it's
usually the sign of poor class structure and that a better design would to create base classes with that shared functionality, it's really shown me
that very thing. At the time of writing this, there's one (1) pesky utility function left that I can't seem to refactor, but as for the rest of them:
they've all been turned into base classes, and the code looks WAY better for it.

### 4. Braces for everything. ###

Curly braces are not optional. There's a plethora of reasons for this (consistency, preventing fall-through code, makes it easier to add more than
one statement in a block, etc). However, the reason I'm going to use as justification for this project is simple: I like it that way. It looks good.
It makes things easier to read, spaces out lines, and it makes reading through a function that might have four indentation levels less awful.

Additionally, they should always be on the next line. No Java same-line curly brace bullshit. For the same reason. I think that's awful to look at.
And C# agrees with me.

### 5. A `switch` statement with 4 or more `case` statements should not be a `switch` statement. ###

This is another one of those rules that I was wary of but has paid off in spades. `switch` statements are SUPER useful, but they're also really easily
abused, and I would have functions (particularly for the first incarnation or two of the Language enum code for Alexandria) that was just a long,
60+ `case` statement `switch` block. It was suggested that `switch` statements that long should be refactored to an encapsulated class inside of a
lookup/dictionary, and once I did that, I noticed that suddenly, TONS of classes were easier to write, and all of my code was now consolidated in one
location. Really useful technology, but too easily abused, `switch` statements.

### 6. Obey all of the ReSharper and StyleCop rules and warnings. ###

These are configured to be your friend, and they help ensure that *my* code will look like *your* code in all the tiny little ways, and that *our* code
will look like *Derek's* code, and so on. It really just keeps everything consolidated, not just across our project, but across all of C#. Or, at least,
it will try to. And that's about as best as you'll ever be able to do.

# Conclusion #

Anyways, these are the major points that I'm considering when it comes to "maintainability." This list can and probably will change in the future as
I learn new things or encounter new paradigms or ways people have written things.