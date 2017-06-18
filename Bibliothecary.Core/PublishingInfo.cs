using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using Bibliothecary.Core.Publishing;

namespace Bibliothecary.Core
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

		public Boolean DoesSenderUseSsl { get; internal set; }

		public Boolean DoesSenderRequireCredentials { get; internal set; }

		public String SenderUsername { get; internal set; }

		public SecureString SenderPassword { get; internal set; }

		public String RecipientEmail { get; internal set; }

		public Boolean UsesTumblr { get; internal set; }

		public String TumblrConsumerKey { get; internal set; }

		public String TumblrConsumerSecret { get; internal set; }

		public String TumblrOauthToken { get; internal set; }

		public String TumblrOauthSecret { get; internal set; }

		public String TumblrBlogName { get; internal set; }

		public Boolean AreTumblrPostsQueued { get; internal set; }

		public IReadOnlyList<TumblrTagRule> TumblrTags => ConcreteTumblrTags;
		internal List<TumblrTagRule> ConcreteTumblrTags { get; } = new List<TumblrTagRule>();

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

		public Boolean SetDoesSenderUseSsl( Boolean value )
		{
			if ( !UsesEmail )
			{
				throw new InvalidOperationException( $"Cannot set {nameof( DoesSenderUseSsl )} when {nameof( UsesEmail )} is false." );
			}

			if ( value == DoesSenderUseSsl )
			{
				return false;
			}

			DoesSenderUseSsl = value;
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

		public Boolean SetSenderPassword( SecureString password )
		{
			if ( !UsesEmail )
			{
				throw new InvalidOperationException( $"Cannot set {nameof( SenderPassword )} when {nameof( UsesEmail )} is false." );
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

		public Boolean SetUsesTumblr( Boolean value )
		{
			if ( value == UsesTumblr )
			{
				return false;
			}

			UsesTumblr = value;
			return true;
		}

		public Boolean SetTumblrConsumerKey( String consumerKey )
		{
			if ( !UsesTumblr )
			{
				throw new InvalidOperationException( $"Cannot set {nameof( TumblrConsumerKey )} when {nameof( UsesTumblr )} is false." );
			}

			if ( String.Equals( TumblrConsumerKey, consumerKey, StringComparison.InvariantCulture ) )
			{
				return false;
			}

			TumblrConsumerKey = consumerKey;
			return true;
		}

		public Boolean SetTumblrConsumerSecret( String consumerSecret )
		{
			if ( !UsesTumblr )
			{
				throw new InvalidOperationException( $"Cannot set {nameof( TumblrConsumerKey )} when {nameof( UsesTumblr )} is false." );
			}

			if ( String.Equals( TumblrConsumerSecret, consumerSecret, StringComparison.InvariantCulture ) )
			{
				return false;
			}

			TumblrConsumerSecret = consumerSecret;
			return true;
		}

		public Boolean SetTumblrOauthToken( String oauthToken )
		{
			if ( !UsesTumblr )
			{
				throw new InvalidOperationException( $"Cannot set {nameof( TumblrOauthToken )} when {nameof( UsesTumblr )} is false." );
			}

			if ( String.Equals( TumblrOauthToken, oauthToken, StringComparison.InvariantCulture ) )
			{
				return false;
			}

			TumblrOauthToken = oauthToken;
			return true;
		}

		public Boolean SetTumblrOauthSecret( String oauthSecret )
		{
			if ( !UsesTumblr )
			{
				throw new InvalidOperationException( $"Cannot set {nameof( TumblrOauthSecret )} when {nameof( UsesTumblr )} is false." );
			}

			if ( String.Equals( TumblrOauthSecret, oauthSecret, StringComparison.InvariantCulture ) )
			{
				return false;
			}

			TumblrOauthSecret = oauthSecret;
			return true;
		}

		public Boolean SetTumblrBlogName( String blogName )
		{
			if ( !UsesTumblr )
			{
				throw new InvalidOperationException( $"Cannot set {nameof( TumblrBlogName )} when {nameof( UsesTumblr )} is false." );
			}

			if ( String.Equals( TumblrBlogName, blogName, StringComparison.InvariantCultureIgnoreCase ) )
			{
				return false;
			}

			TumblrBlogName = blogName;
			return true;
		}

		public Boolean SetAreTumblrPostsQueued( Boolean value )
		{
			if ( !UsesTumblr )
			{
				throw new InvalidOperationException( $"Cannot set {nameof( TumblrBlogName )} when {nameof( UsesTumblr )} is false." );
			}

			if ( value == AreTumblrPostsQueued )
			{
				return false;
			}

			AreTumblrPostsQueued = value;
			return true;
		}

		public Boolean AddTumblrTag( TumblrTagRule tag, Int32 index = -1 )
		{
			if ( !UsesTumblr )
			{
				throw new InvalidOperationException( $"Cannot set {nameof( TumblrBlogName )} when {nameof( UsesTumblr )} is false." );
			}

			if ( tag == null )
			{
				throw new ArgumentNullException( nameof( tag ) );
			}

			if ( TumblrTags.Any( tag.Equals ) )
			{
				return false; // Basically expecting this to never happen, because GUID equality
			}

			if ( index < 0 || index >= ConcreteTumblrTags.Count )
			{
				ConcreteTumblrTags.Add( tag );
			}
			else
			{
				ConcreteTumblrTags.Insert( index, tag );
			}
			return true;
		}

		public Boolean RemoveTumblrTag( TumblrTagRule tag, out Int32 oldIndex )
		{
			if ( !UsesTumblr )
			{
				throw new InvalidOperationException( $"Cannot set {nameof( TumblrBlogName )} when {nameof( UsesTumblr )} is false." );
			}

			if ( tag == null )
			{
				throw new ArgumentNullException( nameof( tag ) );
			}

			oldIndex = ConcreteTumblrTags.IndexOf( tag );
			if ( oldIndex < 0 )
			{
				return false;
			}
			ConcreteTumblrTags.RemoveAt( oldIndex );
			return true;
		}

		public EmailClient CreateEmailClient()
		{
			if ( !UsesEmail )
			{
				throw new InvalidOperationException( $"Cannot create a new {nameof( EmailClient )} when {nameof( UsesEmail )} is false." );
			}

			EmailClient client = new EmailClient
			{
				FromEmail = SenderEmail,
				Host = SenderHost,
				Port = SenderPort,
				EnableSsl = DoesSenderUseSsl,
				ToEmail = RecipientEmail
			};

			if ( DoesSenderRequireCredentials )
			{
				client.SetCredentials( SenderUsername, SenderPassword );
			}

			return client;
		}

		public TumblrClient CreateTumblrClient()
		{
			if ( !UsesTumblr )
			{
				throw new InvalidOperationException( $"Cannot create a new {nameof( TumblrClient )} when {nameof( UsesTumblr )} is false." );
			}

			return new TumblrClient
			{
				ConsumerKey = TumblrConsumerKey,
				ConsumerSecret = TumblrConsumerSecret,
				OauthToken = TumblrOauthToken,
				OauthTokenSecret = TumblrOauthSecret,
				BlogName = TumblrBlogName,
				ArePostsQueued = AreTumblrPostsQueued,
				TagRules = TumblrTags
			};
		}

		public PublishingInfo Clone()
		{
			PublishingInfo clone = new PublishingInfo( _projectId )
			{
				UsesEmail = UsesEmail,
				SenderEmail = SenderEmail,
				SenderHost = SenderHost,
				SenderPort = SenderPort,
				DoesSenderUseSsl = DoesSenderUseSsl,
				DoesSenderRequireCredentials = DoesSenderRequireCredentials,
				SenderUsername = SenderUsername,
				SenderPassword = SenderPassword,
				RecipientEmail = RecipientEmail,
				UsesTumblr = UsesTumblr,
				TumblrConsumerKey = TumblrConsumerKey,
				TumblrConsumerSecret = TumblrConsumerSecret,
				TumblrOauthToken = TumblrOauthToken,
				TumblrOauthSecret = TumblrOauthSecret,
				TumblrBlogName = TumblrBlogName,
				AreTumblrPostsQueued = AreTumblrPostsQueued
			};
			clone.ConcreteTumblrTags.AddRange( TumblrTags.Select( tag => tag.Clone() ) );
			return clone;
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

				if ( DoesSenderUseSsl != other.DoesSenderUseSsl )
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
				}

				if ( !String.Equals( RecipientEmail, other.RecipientEmail, StringComparison.InvariantCultureIgnoreCase ) )
				{
					return false;
				}
			}

			if ( UsesTumblr != other.UsesTumblr )
			{
				return false;
			}

			if ( UsesTumblr )
			{
				if ( !String.Equals( TumblrConsumerKey, other.TumblrConsumerKey, StringComparison.InvariantCulture ) )
				{
					return false;
				}

				if ( !String.Equals( TumblrOauthToken, other.TumblrOauthToken, StringComparison.InvariantCulture ) )
				{
					return false;
				}

				if ( !String.Equals( TumblrBlogName, other.TumblrBlogName, StringComparison.InvariantCultureIgnoreCase ) )
				{
					return false;
				}

				if ( AreTumblrPostsQueued != other.AreTumblrPostsQueued )
				{
					return false;
				}

				if ( TumblrTags.Count != other.TumblrTags.Count )
				{
					return false;
				}

				if ( TumblrTags.Count > 0 && TumblrTags.Where( ( tag, index ) => !tag.ContentEquals( other.TumblrTags[index] ) ).Any() )
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
