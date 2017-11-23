// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

namespace Alexandria.Exceptions.Input
{
    /// <summary>
    /// An exception that indicates that the input provided to the function in the form of a series
    /// handle was invalid. On a per-website basis, there are validation functions that will verify
    /// that the input to functions such as this one are in the correct format for a series handle.
    /// </summary>
    public sealed class InvalidSeriesAlexandriaException : AlexandriaException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidSeriesAlexandriaException"/> class.
        /// </summary>
        /// <param name="website">The website that this series handle was invalid for.</param>
        /// <param name="value">The value that was provided to the original function, which turned
        /// out to be invalid in composition to be used as a series handle.</param>
        /// <param name="paramName">The name of the parameter that was supposed to be used as an
        /// invalid value.</param>
        internal InvalidSeriesAlexandriaException( Website website, string value, string paramName )
        {
            Website = website;
            InvalidValue = value;
            ParamName = paramName;
        }

        /// <summary>
        /// Gets the website that this series handle was invalid for.
        /// </summary>
        public Website Website { get; }

        /// <summary>
        /// Gets the invalid value that was provided as a series handle.
        /// </summary>
        public string InvalidValue { get; }

        /// <summary>
        /// Gets the name of the parameter that was supposed to be used as an invalid value.
        /// </summary>
        public string ParamName { get; }
    }
}
