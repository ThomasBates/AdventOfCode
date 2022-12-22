using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using AoC.Common;

namespace AoC.Main
{
	class MainViewModel : ViewModel
	{
		private TextViewModel selectedPuzzleYearViewModel;
		private TextViewModel selectedPuzzleDayViewModel;
		private int selectedPuzzleYear;
		private IPuzzle selectedPuzzle;
		private TextViewModel selectedInputsViewModel;
		private TextViewModel selectedSolverViewModel;

		private string inputText;
		private string outputText;

		private readonly Dictionary<TextViewModel, int> puzzleYearMap = new();
		private readonly Dictionary<TextViewModel, IPuzzle> puzzleDayMap = new();

		private readonly Dictionary<int, List<TextViewModel>> puzzleYearDays = new();

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value null
		[ImportMany(typeof(IPuzzle))]
		private IEnumerable<IPuzzle> puzzles;
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value null

		private readonly ILogger logger = new Logger(SeverityLevel.Debug);

		public MainViewModel()
		{
			logger.OnMessageSent += Logger_OnMessageSent;

			ImportPuzzles();
		}

		public void ImportPuzzles()
		{
			//An aggregate catalog that combines multiple catalogs
			var catalog = new AggregateCatalog();
			//Adds all the parts found in all assemblies in 
			//the same directory as the executing program
			catalog.Catalogs.Add(
			 new DirectoryCatalog(
			  Path.GetDirectoryName(
			   Assembly.GetExecutingAssembly().Location)));

			//Create the CompositionContainer with the parts in the catalog
			CompositionContainer container = new(catalog);
			container.ComposeExportedValue<ILogger>(logger);

			//Fill the imports of this object
			container.ComposeParts(this);

			foreach (var year in puzzles.Select(p => p.Year).Distinct().OrderByDescending(p => p).ToList())
			{
				puzzleYearDays[year] = new List<TextViewModel>();

				var yearViewModel = new TextViewModel(year.ToString());
				puzzleYearMap.Add(yearViewModel, year);
				PuzzleYears.Add(yearViewModel);
			}

			foreach (var puzzleDay in puzzles.OrderByDescending(p => p.Name).ToList())
			{
				var dayViewModel = new TextViewModel(puzzleDay.Name);
				puzzleDayMap.Add(dayViewModel, puzzleDay);
				puzzleYearDays[puzzleDay.Year].Add(dayViewModel);
			}

			SelectedPuzzleYear = PuzzleYears[0];
		}

		public ObservableCollection<TextViewModel> PuzzleYears { get; } = new();

		public ObservableCollection<TextViewModel> PuzzleDays { get; } = new();

		public ObservableCollection<TextViewModel> Inputs { get; } = new();

		public ObservableCollection<TextViewModel> Solvers { get; } = new();

		public ObservableCollection<string> MessageLog { get; } = new();

		public TextViewModel SelectedPuzzleYear
		{
			get => selectedPuzzleYearViewModel;
			set
			{
				if (selectedPuzzleYearViewModel == value)
					return;

				selectedPuzzleYearViewModel = value;
				puzzleYearMap.TryGetValue(selectedPuzzleYearViewModel, out selectedPuzzleYear);

				NotifyPropertyChanged();

				PuzzleDays.Clear();

				foreach (var dayViewModel in puzzleYearDays[selectedPuzzleYear].OrderByDescending(p => p.Text).ToList())
				{
					PuzzleDays.Add(dayViewModel);

					SelectedPuzzleDay ??= dayViewModel;
				}
			}
		}

		public TextViewModel SelectedPuzzleDay
		{
			get => selectedPuzzleDayViewModel;
			set
			{
				if (selectedPuzzleDayViewModel == value)
					return;

				selectedPuzzleDayViewModel = value;

				NotifyPropertyChanged();
				NotifyPropertyChanged(nameof(PuzzleName));

				InputText = string.Empty;
				OutputText = string.Empty;

				SelectedInputs = null;
				Inputs.Clear();

				SelectedSolver = null;
				Solvers.Clear();

				if (selectedPuzzleDayViewModel == null)
					return;

				if (puzzleDayMap.TryGetValue(selectedPuzzleDayViewModel, out selectedPuzzle))
				{
					foreach (var key in selectedPuzzle.Inputs.Keys.OrderBy(k => k))
						Inputs.Add(new TextViewModel(key));

					foreach (var key in selectedPuzzle.Solvers.Keys.OrderBy(k => k))
						Solvers.Add(new TextViewModel(key));
				}
			}
		}

		public string PuzzleName
		{
			get
			{
				if (selectedPuzzle == null)
					return String.Empty;

				return selectedPuzzle.Name;
			}
		}

		public TextViewModel SelectedInputs
		{
			get => selectedInputsViewModel;
			set
			{
				if (selectedInputsViewModel == value)
					return;

				selectedInputsViewModel = value;

				NotifyPropertyChanged();

				if (value != null)
				{
					InputText = GetInputText(selectedPuzzleYear, selectedPuzzle, value.Text);
				}
			}
		}

		private string GetInputText(int selectedPuzzleYear, IPuzzle selectedPuzzleDay, string key)
		{
			var inputText = this.selectedPuzzle.Inputs[key];

			if (string.IsNullOrEmpty(inputText))
			{
				inputText = Helper.GetInputText(selectedPuzzleYear, selectedPuzzleDay.Day);
				if (!string.IsNullOrEmpty(inputText))
					this.selectedPuzzle.Inputs[key] = inputText;
			}

			return inputText;
		}

		public TextViewModel SelectedSolver
		{
			get => selectedSolverViewModel;
			set
			{
				if (selectedSolverViewModel == value)
					return;

				selectedSolverViewModel = value;

				NotifyPropertyChanged();

				if (value != null)
				{
					OutputText = "";

					new Thread(() =>
					{
						Thread.CurrentThread.IsBackground = true;
						OutputText = selectedPuzzle.Solvers[value.Text](inputText);
					}).Start();
				}

				SelectedSolver = null;
			}
		}

		public string InputText
		{
			get => inputText;
			set
			{
				if (String.Equals(value, inputText))
					return;

				inputText = value;
				NotifyPropertyChanged();
			}
		}

		public string OutputText
		{
			get => outputText;
			set
			{
				if (String.Equals(value, outputText))
					return;

				outputText = value;
				NotifyPropertyChanged();
			}
		}

		private void Logger_OnMessageSent(object sender, string message)
		{
			App.Current.Dispatcher.Invoke(() => 
			{
				MessageLog.Add(message);
			});
		}
	}
}
