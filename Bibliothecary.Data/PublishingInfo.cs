using System;

namespace Bibliothecary.Data
{
	public sealed class PublishingInfo : IEquatable<PublishingInfo>
	{
		internal PublishingInfo( Int32 projectId )
		{
			_projectId = projectId;
		}

		public Boolean UsesEmail { get; internal set; }

		public String SenderEmail { get; internal set; }

		public String SenderHost { get; internal set; }

		public Int32 SenderPort { get; internal set; }

		public Boolean DoesSenderRequireCredentials { get; internal set; }

		public String SenderUsername { get; internal set; }

		public String SenderPassword { get; internal set; }

		public String RecipientEmail { get; internal set; }

		public Boolean SetUsesEmail( Boolean value )
		{
			if ( value == UsesEmail )
			{
				return false;
			}

			UsesEmail = value;
			return true;
		}

		public Boolean SetSenderEmail( String email )
		{
			if ( !UsesEmail )
			{
				throw new InvalidOperationException( $"Cannot set {nameof( SenderEmail )} when {nameof( UsesEmail )} is false." );
			}

			if ( String.Equals( SenderEmail, email, StringComparison.InvariantCultureIgnoreCase ) )
			{
				return false;
			}

			SenderEmail = email;
			return true;
		}

		public Boolean SetSenderHost( String host )
		{
			if ( !UsesEmail )
			{
				throw new InvalidOperationException( $"Cannot set {nameof( SenderHost )} when {nameof( UsesEmail )} is false." );
			}

			if ( String.Equals( SenderHost, host, StringComparison.InvariantCultureIgnoreCase ) )
			{
				return false;
			}

			SenderHost = host;
			return true;
		}

		public Boolean SetSenderPort( Int32 port )
		{
			if ( !UsesEmail )
			{
				throw new InvalidOperationException( $"Cannot set {nameof( SenderPort )} when {nameof( UsesEmail )} is false." );
			}

			if ( port == SenderPort )
			{
				return false;
			}

			SenderPort = port;
			return true;
		}

		public Boolean SetDoesSenderRequireCredentials( Boolean value )
		{
			if ( !UsesEmail )
			{
				throw new InvalidOperationException( $"Cannot set {nameof( SenderUsername )} when {nameof( UsesEmail )} is false." );
			}

			if ( value == DoesSenderRequireCredentials )
			{
				return false;
			}

			DoesSenderRequireCredentials = value;
			return true;
		}

		public Boolean SetSenderUsername( String username )
		{
			if ( !UsesEmail )
			{
				throw new InvalidOperationException( $"Cannot set {nameof( SenderUsername )} when {nameof( UsesEmail )} is false." );
			}

			if ( String.Equals( SenderUsername, username, StringComparison.InvariantCulture ) )
			{
				return false;
			}

			SenderUsername = username;
			return true;
		}

		public Boolean SetSenderPassword( String password )
		{
			if ( !UsesEmail )
			{
				throw new InvalidOperationException( $"Cannot set {nameof( SenderPassword )} when {nameof( UsesEmail )} is false." );
			}

			if ( String.Equals( SenderPassword, password, StringComparison.InvariantCulture ) )
			{
				return false;
			}

			SenderPassword = password;
			return true;
		}

		public Boolean SetRecipientEmail( String email )
		{
			if ( !UsesEmail )
			{
				throw new InvalidOperationException( $"Cannot set {nameof( RecipientEmail )} when {nameof( UsesEmail )} is false." );
			}

			if ( String.Equals( RecipientEmail, email, StringComparison.InvariantCultureIgnoreCase ) )
			{
				return false;
			}

			RecipientEmail = email;
			return true;
		}

		public PublishingInfo Clone()
		{
			return new PublishingInfo( _projectId )
			{
				UsesEmail = UsesEmail,
				SenderEmail = SenderEmail,
				SenderHost = SenderHost,
				SenderPort = SenderPort,
				DoesSenderRequireCredentials = DoesSenderRequireCredentials,
				SenderUsername = SenderUsername,
				SenderPassword = SenderPassword,
				RecipientEmail = RecipientEmail
			};
		}

		public Boolean Equals( PublishingInfo other )
		{
			if ( other == null || other._projectId != _projectId )
			{
				return false;
			}

			if ( UsesEmail != other.UsesEmail )
			{
				return false;
			}

			if ( UsesEmail )
			{
				if ( !String.Equals( SenderEmail, other.SenderEmail, StringComparison.InvariantCultureIgnoreCase ) )
				{
					return false;
				}

				if ( !String.Equals( SenderHost, other.SenderHost, StringComparison.InvariantCultureIgnoreCase ) )
				{
					return false;
				}

				if ( SenderPort != other.SenderPort )
				{
					return false;
				}

				if ( DoesSenderRequireCredentials != other.DoesSenderRequireCredentials )
				{
					return false;
				}

				if ( DoesSenderRequireCredentials )
				{
					if ( !String.Equals( SenderUsername, other.SenderUsername, StringComparison.InvariantCulture ) )
					{
						return false;
					}

					if ( !String.Equals( SenderPassword, other.SenderPassword, StringComparison.InvariantCulture ) )
					{
						return false;
					}
				}

				if ( !String.Equals( RecipientEmail, other.RecipientEmail, StringComparison.InvariantCultureIgnoreCase ) )
				{
					return false;
				}
			}

			return true;
		}

		/// <inheritdoc />
		public override Boolean Equals( Object obj )
		{
			if ( ReferenceEquals( obj, null ) )
			{
				return false;
			}

			if ( ReferenceEquals( obj, this ) )
			{
				return true;
			}

			PublishingInfo other = obj as PublishingInfo;
			return Equals( other );
		}

		/// <inheritdoc />
		public override Int32 GetHashCode()
		{
			return _projectId;
		}

		readonly Int32 _projectId;
	}
}
