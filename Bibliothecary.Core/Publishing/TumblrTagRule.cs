using System;
using System.Collections.Generic;

namespace Bibliothecary.Core.Publishing
{
	public sealed class TumblrTagRuleContentEqualityComparer : EqualityComparer<TumblrTagRule>
	{
		/// <inheritdoc />
		public override Boolean Equals( TumblrTagRule x, TumblrTagRule y )
		{
			if ( ( x == null ) != ( y == null ) )
			{
				return false;
			}

			if ( x == null )
			{
				return true; // Both null
			}

			return x.ContentEquals( y );
		}

		/// <inheritdoc />
		public override Int32 GetHashCode( TumblrTagRule obj )
		{
			return obj?.GetContentHashCode() ?? 0;
		}
	}

	public sealed class TumblrTagRule : IEquatable<TumblrTagRule>
	{
		public TumblrTagRule( String text ) : this( Guid.NewGuid() )
		{
			if ( String.IsNullOrWhiteSpace( text ) )
			{
				throw new ArgumentNullException( nameof( text ) );
			}

			Tag = text;
		}

		TumblrTagRule( Guid guid )
		{
			_guid = guid;
		}

		public String Tag { get; internal set; }

		public TumblrTagRule Clone()
		{
			return new TumblrTagRule( _guid )
			{
				Tag = Tag
			};
		}

		public Boolean ContentEquals( TumblrTagRule other )
		{
			if ( other == null )
			{
				return false;
			}

			if ( !String.Equals( Tag, other.Tag, StringComparison.CurrentCultureIgnoreCase ) )
			{
				return false;
			}

			return true;
		}

		public Int32 GetContentHashCode()
		{
			Int32 hashCode = 17;
			hashCode += 31 * ( Tag?.GetHashCode() ?? 0 );
			return hashCode;
		}

		public Boolean Equals( TumblrTagRule other )
		{
			if ( other == null )
			{
				return false;
			}

			return ( _guid.Equals( other._guid ) );
		}

		/// <inheritdoc />
		public override Boolean Equals( Object obj )
		{
			TumblrTagRule other = obj as TumblrTagRule;
			if ( other == null )
			{
				return false;
			}

			if ( ReferenceEquals( obj, this ) )
			{
				return true;
			}

			return Equals( other );
		}

		/// <inheritdoc />
		public override Int32 GetHashCode()
		{
			return _guid.GetHashCode();
		}

		readonly Guid _guid;
	}
}
