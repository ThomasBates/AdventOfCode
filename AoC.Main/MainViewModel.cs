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
		private int selectedPuzzleYear;
		private IPuzzle selectedPuzzle;
		private string selectedInputs;
		private string selectedSolver;

		private string outputText;
		private string inputText;

		//private readonly Dictionary<string, int> puzzleYearMap = new();
		///private readonly Dictionary<string, IPuzzle> puzzleDayMap = new();

		private readonly Dictionary<int, List<IPuzzle>> yearPuzzles = new();

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value null
#pragma warning disable IDE0044 // Add readonly modifier
		[ImportMany(typeof(IPuzzle))]
		private IEnumerable<IPuzzle> importedPuzzles;
#pragma warning restore IDE0044 // Add readonly modifier
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
			catalog.Catalogs.Add(new DirectoryCatalog(
				Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)));

			//Create the CompositionContainer with the parts in the catalog
			CompositionContainer container = new(catalog);
			container.ComposeExportedValue(logger);

			//Fill the imports of this object
			container.ComposeParts(this);

			foreach (var puzzle in importedPuzzles.OrderByDescending(p => p.Year).ThenByDescending(p=>p.Name).ToList())
			{
				if (!PuzzleYears.Contains(puzzle.Year))
				{
					PuzzleYears.Add(puzzle.Year);
					yearPuzzles[puzzle.Year] = new List<IPuzzle>();
				}

				yearPuzzles[puzzle.Year].Add(puzzle);
			}

			SelectedPuzzleYear = 2015; // PuzzleYears[0];
		}

		public ObservableCollection<int> PuzzleYears { get; } = new();

		public ObservableCollection<IPuzzle> Puzzles { get; } = new();

		public ObservableCollection<string> Inputs { get; } = new();

		public ObservableCollection<string> Solvers { get; } = new();

		public ObservableCollection<string> MessageLog { get; } = new();

		public int SelectedPuzzleYear
		{
			get => selectedPuzzleYear;
			set
			{
				if (selectedPuzzleYear == value)
					return;

				selectedPuzzleYear = value;
				NotifyPropertyChanged();

				Puzzles.Clear();

				foreach (var puzzle in yearPuzzles[selectedPuzzleYear].ToList())
				{
					Puzzles.Add(puzzle);

					SelectedPuzzle ??= puzzle;
				}
			}
		}

		public IPuzzle SelectedPuzzle
		{
			get => selectedPuzzle;
			set
			{
				if (selectedPuzzle == value)
					return;

				selectedPuzzle = value;

				NotifyPropertyChanged();
				NotifyPropertyChanged(nameof(PuzzleName));

				InputText = string.Empty;
				OutputText = string.Empty;

				SelectedInputs = null;
				Inputs.Clear();

				SelectedSolver = null;
				Solvers.Clear();

				if (selectedPuzzle == null)
					return;

				foreach (var key in selectedPuzzle.Inputs.Keys.OrderBy(k => k))
					Inputs.Add(key);

				foreach (var key in selectedPuzzle.Solvers.Keys.OrderBy(k => k))
					Solvers.Add(key);
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

		public string SelectedInputs
		{
			get => selectedInputs;
			set
			{
				if (selectedInputs == value)
					return;

				selectedInputs = value;

				NotifyPropertyChanged();

				if (value != null)
				{
					InputText = GetInputText(selectedPuzzleYear, selectedPuzzle, value);
				}
			}
		}

		private string GetInputText(int selectedPuzzleYear, IPuzzle selectedPuzzle, string key)
		{
			var inputText = this.selectedPuzzle.Inputs[key];

			if (string.IsNullOrEmpty(inputText))
			{
				inputText = Helper.GetInputText(selectedPuzzleYear, selectedPuzzle.Day);
				if (!string.IsNullOrEmpty(inputText))
					this.selectedPuzzle.Inputs[key] = inputText;
			}

			return inputText;
		}

		public string SelectedSolver
		{
			get => selectedSolver;
			set
			{
				if (selectedSolver == value)
					return;

				selectedSolver = value;

				NotifyPropertyChanged();

				if (value != null)
				{
					OutputText = "";
					MessageLog.Clear();

					new Thread(() =>
					{
						Thread.CurrentThread.IsBackground = true;
						var begin = DateTime.Now;
						logger.Send(SeverityLevel.Info, "Core", $"Begin");
						OutputText = selectedPuzzle.Solvers[value](inputText);
						var end = DateTime.Now;
						logger.Send(SeverityLevel.Info, "Core", $"End: {end - begin}");
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
