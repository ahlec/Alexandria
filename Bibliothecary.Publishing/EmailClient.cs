using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Alexandria.Model;

namespace Bibliothecary.Publishing
{
	public class EmailClient
	{
		public EmailClient()
		{
			_client = new SmtpClient
			{
				Port = 25
			};
		}

		public Boolean EnableSsl
		{
			get => _client.EnableSsl;
			set => _client.EnableSsl = value;
		}

		public void SetCredentials( String username, String password )
		{
			_client.UseDefaultCredentials = false;
			_client.Credentials = new NetworkCredential( username, password );
		}

		public String Host
		{
			get => _client.Host;
			set => _client.Host = value;
		}

		public Int32 Port
		{
			get => _client.Port;
			set => _client.Port = value;
		}

		public String FromEmail { get; set; }

		public String ToEmail { get; set; }

		public void SendMail( IFanfic fanfic )
		{
			if ( String.IsNullOrWhiteSpace( FromEmail ) )
			{
				throw new InvalidOperationException( $"{nameof( FromEmail )} must not be null, empty, or only whitespace!" );
			}
			if ( String.IsNullOrWhiteSpace( ToEmail ) )
			{
				throw new InvalidOperationException( $"{nameof( ToEmail )} must not be null, empty, or only whitespace!" );
			}

			using ( MailMessage message = new MailMessage( FromEmail, ToEmail ) )
			{
				message.IsBodyHtml = true;

				message.Subject = "Hello world!";

				message.Body = "<strong>Hello</strong>, world! Hi alec!";

				_client.Send( message );
			}
		}

		readonly SmtpClient _client;
	}
}
