// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using Alexandria.Searching;
using NUnit.Framework;

namespace Alexandria.Tests.Searching
{
    [TestFixture]
    public class NumberSearchCriteriaTests
    {
        [Test]
        public void NumberSearchCriteria_Clone_ReturnsNewInstance()
        {
            NumberSearchCriteria original = new NumberSearchCriteria();
            NumberSearchCriteria clone = original.Clone();

            Assert.That( clone, Is.Not.SameAs( original ) );
        }

        [Test]
        public void NumberSearchCriteria_Clone_CopiesAllFields()
        {
            NumberSearchCriteria original = new NumberSearchCriteria
            {
                Number1 = 10,
                Number2 = 100,
                Type = NumberSearchCriteriaType.Range
            };

            NumberSearchCriteria clone = original.Clone();

            Assert.That( clone, Is.EqualTo( original ) );
        }

        [Test]
        public void NumberSearchCriteria_Equals_NotEqualNull()
        {
            NumberSearchCriteria original = new NumberSearchCriteria();

            Assert.That( original, Is.Not.EqualTo( null ) );
        }

        [Test]
        public void NumberSearchCriteria_Equals_ConsidersType()
        {
            NumberSearchCriteria original = new NumberSearchCriteria
            {
                Number1 = 15,
                Number2 = 20,
                Type = NumberSearchCriteriaType.GreaterThan
            };
            NumberSearchCriteria clone = original.Clone();
            Assert.That( clone, Is.EqualTo( original ) );

            clone.Type = NumberSearchCriteriaType.LessThan;
            Assert.That( clone, Is.Not.EqualTo( original ) );
        }

        [Test]
        public void NumberSearchCriteria_Equals_ConsidersNumber1()
        {
            NumberSearchCriteria original = new NumberSearchCriteria
            {
                Number1 = 15,
                Number2 = 20,
                Type = NumberSearchCriteriaType.GreaterThan
            };
            NumberSearchCriteria clone = original.Clone();
            Assert.That( clone, Is.EqualTo( original ) );

            clone.Number1 = 1;
            Assert.That( clone, Is.Not.EqualTo( original ) );
        }

        [Test]
        public void NumberSearchCriteria_Equals_IgnoresNumber2IfNotRange()
        {
            NumberSearchCriteria original = new NumberSearchCriteria
            {
                Number1 = 15,
                Number2 = 20,
                Type = NumberSearchCriteriaType.GreaterThan
            };
            NumberSearchCriteria clone = original.Clone();
            Assert.That( clone, Is.EqualTo( original ) );

            clone.Number2 = 1;
            Assert.That( clone, Is.EqualTo( original ) );

            clone.Type = NumberSearchCriteriaType.Range;
            Assert.That( clone, Is.Not.EqualTo( original ) );

            clone.Type = original.Type;
            Assert.That( clone, Is.EqualTo( original ) );
        }

        [Test]
        public void NumberSearchCriteria_Parsing_NullString()
        {
            Assert.Throws<ArgumentNullException>( () => NumberSearchCriteria.Parse( null ) );

            Assert.That( NumberSearchCriteria.TryParse( null, out NumberSearchCriteria tryParseOutput ), Is.False );
            Assert.That( tryParseOutput, Is.Null );
        }

        [Test]
        public void NumberSearchCriteria_Parsing_FailsForInvalidStringInput()
        {
            Assert.Throws<ArgumentException>( () => NumberSearchCriteria.Parse( "? something" ) );
            Assert.Throws<ArgumentException>( () => NumberSearchCriteria.Parse( string.Empty ) );
            Assert.Throws<ArgumentException>( () => NumberSearchCriteria.Parse( "            " ) );
            Assert.Throws<ArgumentException>( () => NumberSearchCriteria.Parse( "hello world" ) );

            Assert.That( NumberSearchCriteria.TryParse( "? something", out NumberSearchCriteria tryParseOutput ), Is.False );
            Assert.That( tryParseOutput, Is.Null );
            Assert.That( NumberSearchCriteria.TryParse( string.Empty, out tryParseOutput ), Is.False );
            Assert.That( tryParseOutput, Is.Null );
            Assert.That( NumberSearchCriteria.TryParse( "           ", out tryParseOutput ), Is.False );
            Assert.That( tryParseOutput, Is.Null );
            Assert.That( NumberSearchCriteria.TryParse( "hello world", out tryParseOutput ), Is.False );
            Assert.That( tryParseOutput, Is.Null );
        }

        [Test]
        public void NumberSearchCriteria_Parsing_ExactMatch()
        {
            NumberSearchCriteria original = new NumberSearchCriteria
            {
                Number1 = 12,
                Type = NumberSearchCriteriaType.ExactMatch
            };
            Assert.That( NumberSearchCriteria.Parse( "12" ), Is.EqualTo( original ) );

            original.Number1 = 1000;
            Assert.That( NumberSearchCriteria.Parse( "     1000  " ), Is.EqualTo( original ) );

            original.Number1 = 99;
            Assert.That( NumberSearchCriteria.TryParse( "99", out NumberSearchCriteria tryParseResult ), Is.True );
            Assert.That( tryParseResult, Is.EqualTo( original ) );

            original.Number1 = 65535;
            Assert.That( NumberSearchCriteria.TryParse( "65535", out tryParseResult ), Is.True );
            Assert.That( tryParseResult, Is.EqualTo( original ) );
        }
    }
}
