using System;
using System.Windows.Input;
using Bibliothecary.Core;
using HeadLibrarian.WPF;
using PubSub;

namespace HeadLibrarian.ViewModels
{
	public sealed partial class ProjectViewModel : BaseViewModel
	{
		public ProjectViewModel( Database database, Project project )
		{
			UndoStack = new UndoRedoStack();
			UndoStack.UndoPerformed += OnUndoRedo;
			UndoStack.RedoPerformed += OnUndoRedo;
			UndoCommand = new Command( null, CommandUndo );
			RedoCommand = new Command( null, CommandRedo );

			Project = project;
			SearchQuery = new LibrarySearchViewModel( this, project.SearchQuery );
			PublishingInfo = new PublishingInfoViewModel( this, project.PublishingInfo );

			SaveCommand = new Command( null, CommandSave );
		}

		#region UndoStack

		public UndoRedoStack UndoStack { get; }

		public Boolean CanUndo
		{
			get => _canUndo;
			private set => SetProperty( ref _canUndo, value );
		}

		public Boolean CanRedo
		{
			get => _canRedo;
			private set => SetProperty( ref _canRedo, value );
		}

		void OnUndoRedo( Int32 undoStackSize, Int32 redoStackSize )
		{
			CanUndo = ( undoStackSize > 0 );
			CanRedo = ( redoStackSize > 0 );
		}

		public ICommand UndoCommand { get; }

		void CommandUndo( Object o )
		{
			UndoStack.Undo();
		}

		public ICommand RedoCommand { get; }

		void CommandRedo( Object o )
		{
			UndoStack.Redo();
		}

		#endregion

		public Project Project { get; }

		public Int32 ProjectId => Project.ProjectId;

		public Boolean HasUnsavedChanges => Project.HasUnsavedChanges || HasChangedPublishingSenderEmailCredentials;

		public Boolean HasChangedPublishingSenderEmailCredentials { get; set; }

		public String Name
		{
			get => Project.Name;
			set
			{
				String oldName = Name;
				if ( Project.SetName( value ) )
				{
					UndoStack.Push( new SetNameUndoAction( this, oldName, Name ) );
					InvokeNameChanged();
				}
			}
		}

		void InvokeNameChanged()
		{
			OnPropertyChanged( nameof( Name ) );
			OnPropertyChanged( nameof( HasUnsavedChanges ) );
		}

		public TimeSpan UpdateFrequency
		{
			get => Project.UpdateFrequency;
			set
			{
				if ( Project.SetUpdateFrequency( (Int32) value.TotalMinutes ) )
				{
					OnPropertyChanged( nameof( UpdateFrequency ) );
					OnPropertyChanged( nameof( HasUnsavedChanges ) );
				}
			}
		}

		public Int32 MaxResultsPerSearch
		{
			get => Project.MaxResultsPerSearch;
			set
			{
				Int32 oldValue = MaxResultsPerSearch;
				if ( Project.SetMaxResultsPerSearch( value ) )
				{
					UndoStack.Push( new SetMaxResultsPerSearchUndoAction( this, oldValue, value ) );
					InvokeMaxResultsPerSearchChanged();
				}
			}
		}

		void InvokeMaxResultsPerSearchChanged()
		{
			OnPropertyChanged( nameof( MaxResultsPerSearch ) );
			OnPropertyChanged( nameof( HasUnsavedChanges ) );
		}

		public Boolean SearchAO3
		{
			get => Project.SearchAO3;
			set
			{
				Boolean oldValue = SearchAO3;
				if ( Project.SetSearchAO3( value ) )
				{
					UndoStack.Push( new SetSearchAO3UndoAction( this, oldValue, value ) );
					InvokeSearchAO3Changed();
				}
			}
		}

		void InvokeSearchAO3Changed()
		{
			OnPropertyChanged( nameof( SearchAO3 ) );
			OnPropertyChanged( nameof( HasUnsavedChanges ) );
		}

		public LibrarySearchViewModel SearchQuery { get; }

		public PublishingInfoViewModel PublishingInfo { get; }

		public ICommand SaveCommand { get; }

		void CommandSave( Object o )
		{
			Project.Save();
			HasChangedPublishingSenderEmailCredentials = false;
			OnPropertyChanged( nameof( HasUnsavedChanges ) );
		}

		public void RefreshHasSavedChanged()
		{
			OnPropertyChanged( nameof( HasUnsavedChanges ) );
		}

		/// <inheritdoc />
		protected override void OnPropertyChanged( String propertyName )
		{
			base.OnPropertyChanged( propertyName );

			if ( propertyName.Equals( nameof( HasUnsavedChanges ) ) )
			{
				if ( _previousHasUnsavedChanges != HasUnsavedChanges )
				{
					this.Publish<ProjectHasUnsavedChangesChanged>();
				}
				_previousHasUnsavedChanges = HasUnsavedChanges;
			}
		}

		public Boolean Delete()
		{
			return Project.Delete();
		}

		Boolean _canUndo;
		Boolean _canRedo;
		Boolean _previousHasUnsavedChanges;
	}
}
