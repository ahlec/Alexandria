using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Alexandria.Model;
using Alexandria.Searching;

namespace Bibliothecary.Data.Utils
{
	// NOTE!! Don't use nameof() or default() here, in case any of those fields change! We don't want to break serialization to an external database!!

	public static class LibrarySearchUtils
	{
		public static IEnumerable<KeyValuePair<String, String>> GetProjectSearchFields( LibrarySearch search )
		{
			if ( !String.IsNullOrWhiteSpace( search.Title ) )
			{
				yield return new KeyValuePair<String, String>( FieldNameTitle, search.Title );
			}

			if ( !String.IsNullOrWhiteSpace( search.Author ) )
			{
				yield return new KeyValuePair<String, String>( FieldNameAuthor, search.Author );
			}

			if ( search.Date != null )
			{
				yield return new KeyValuePair<String, String>( FieldNameDate, search.Date.ToString() );
			}

			if ( search.OnlyIncludeCompleteFanfics )
			{
				yield return new KeyValuePair<String, String>( FieldNameOnlyComplete, "true" );
			}

			if ( search.OnlyIncludeSingleChapterFanfics )
			{
				yield return new KeyValuePair<String, String>( FieldNameOnlySingleChapters, "true" );
			}

			if ( search.WordCount != null )
			{
				yield return new KeyValuePair<String, String>( FieldNameWordCount, search.WordCount.ToString() );
			}

			if ( search.Language != null )
			{
				yield return new KeyValuePair<String, String>( FieldNameLanguage, search.Language.ToString() );
			}

			if ( search.Fandoms?.Count > 0 )
			{
				foreach ( String fandom in search.Fandoms )
				{
					yield return new KeyValuePair<String, String>( FieldNameFandom, fandom );
				}
			}

			if ( search.Rating != null )
			{
				yield return new KeyValuePair<String, String>( FieldNameMaturityRating, search.Rating.ToString() );
			}

			if ( search.ContentWarnings != null && search.ContentWarnings != ContentWarnings.None )
			{
				foreach ( ContentWarnings warning in Enum.GetValues( typeof( ContentWarnings ) ) )
				{
					if ( warning == ContentWarnings.None )
					{
						continue;
					}

					if ( search.ContentWarnings.Value.HasFlag( warning ) )
					{
						yield return new KeyValuePair<String, String>( FieldNameContentWarning, warning.ToString() );
					}
				}
			}

			if ( search.CharacterNames?.Count > 0 )
			{
				foreach ( String characterName in search.CharacterNames )
				{
					yield return new KeyValuePair<String, String>( FieldNameCharacterName, characterName );
				}
			}

			if ( search.Ships?.Count > 0 )
			{
				foreach ( String ship in search.Ships )
				{
					yield return new KeyValuePair<String, String>( FieldNameShip, ship );
				}
			}

			if ( search.Tags?.Count > 0 )
			{
				foreach ( String tag in search.Tags )
				{
					yield return new KeyValuePair<String, String>( FieldNameTag, tag );
				}
			}

			if ( search.NumberLikes != null )
			{
				yield return new KeyValuePair<String, String>( FieldNameNumberLikes, search.NumberLikes.ToString() );
			}

			if ( search.NumberComments != null )
			{
				yield return new KeyValuePair<String, String>( FieldNameNumberComments, search.NumberComments.ToString() );
			}

			yield return new KeyValuePair<String, String>( FieldNameSortField, search.SortField.ToString() );
			yield return new KeyValuePair<String, String>( FieldNameSortDirection, search.SortDirection.ToString() );
		}

		public static LibrarySearch ReadFromDatabase( SQLiteConnection connection, Int32 projectId )
		{
			SQLiteUtils.ValidateConnection( connection );

			using ( SQLiteCommand selectCommand = new SQLiteCommand( "SELECT field_name, field_value FROM project_search_fields WHERE project_id = @projectId", connection ) )
			{
				selectCommand.Parameters.AddWithValue( "@projectId", projectId );
				using ( SQLiteDataReader reader = selectCommand.ExecuteReader() )
				{
					LibrarySearch search = new LibrarySearch();
					while ( reader.Read() )
					{
						String fieldValue = reader.GetString( 1 );
						switch ( reader.GetString( 0 ) )
						{
							case FieldNameTitle:
								{
									search.Title = fieldValue;
									break;
								}
							case FieldNameAuthor:
								{
									search.Author = fieldValue;
									break;
								}
							case FieldNameDate:
								{
									throw new NotImplementedException();
								}
							case FieldNameOnlyComplete:
								{
									search.OnlyIncludeCompleteFanfics = Boolean.Parse( fieldValue );
									break;
								}
							case FieldNameOnlySingleChapters:
								{
									search.OnlyIncludeSingleChapterFanfics = Boolean.Parse( fieldValue );
									break;
								}
							case FieldNameWordCount:
								{
									throw new NotImplementedException();
								}
							case FieldNameLanguage:
								{
									if ( Enum.TryParse( fieldValue, out Language parsedLanguage ) ) // Languages can be removed by AO3!!
									{
										search.Language = parsedLanguage;
									}
									break;
								}
							case FieldNameFandom:
								{
									search.Fandoms.Add( fieldValue );
									break;
								}
							case FieldNameMaturityRating:
								{
									search.Rating = (MaturityRating) Enum.Parse( typeof( MaturityRating ), fieldValue );
									break;
								}
							case FieldNameContentWarning:
								{
									ContentWarnings warningFlag = (ContentWarnings) Enum.Parse( typeof( ContentWarnings ), fieldValue );
									search.ContentWarnings |= warningFlag;
									break;
								}
							case FieldNameCharacterName:
								{
									search.CharacterNames.Add( fieldValue );
									break;
								}
							case FieldNameShip:
								{
									search.Ships.Add( fieldValue );
									break;
								}
							case FieldNameTag:
								{
									search.Tags.Add( fieldValue );
									break;
								}
							case FieldNameNumberLikes:
								{
									throw new NotImplementedException();
								}
							case FieldNameNumberComments:
								{
									throw new NotImplementedException();
								}
							case FieldNameSortField:
								{
									search.SortField = (SearchField) Enum.Parse( typeof( SearchField ), fieldValue );
									break;
								}
							case FieldNameSortDirection:
								{
									search.SortDirection = (SortDirection) Enum.Parse( typeof( SortDirection ), fieldValue );
									break;
								}
							default:
								throw new NotImplementedException();
						}
					}

					return search;
				}
			}
		}

		const String FieldNameTitle = "title";
		const String FieldNameAuthor = "author";
		const String FieldNameDate = "date";
		const String FieldNameOnlyComplete = "complete";
		const String FieldNameOnlySingleChapters = "single_chapters";
		const String FieldNameWordCount = "word_count";
		const String FieldNameLanguage = "language";
		const String FieldNameFandom = "fandom";
		const String FieldNameMaturityRating = "maturity_rating";
		const String FieldNameContentWarning = "content_warning";
		const String FieldNameCharacterName = "character_name";
		const String FieldNameShip = "ship";
		const String FieldNameTag = "tag";
		const String FieldNameNumberLikes = "number_likes";
		const String FieldNameNumberComments = "number_comments";
		const String FieldNameSortField = "sort_field";
		const String FieldNameSortDirection = "sort_direction";
	}
}
