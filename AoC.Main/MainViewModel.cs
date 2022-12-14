using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using AoC.IO;
using AoC.Puzzle;

namespace AoC.Main
{
	class MainViewModel : ViewModel
	{
		private TextViewModel selectedPuzzleYearViewModel;
		private TextViewModel selectedPuzzleDayViewModel;
		private int selectedPuzzleYear;
		private IPuzzle selectedPuzzleDay;
		private TextViewModel selectedInputsViewModel;
		private TextViewModel selectedSolverViewModel;

		private string inputText;
		private string outputText;

		private readonly Dictionary<TextViewModel, int> puzzleYearMap = new();
		private readonly Dictionary<TextViewModel, IPuzzle> puzzleDayMap = new();

		private readonly Dictionary<int, List<TextViewModel>> puzzleYearDays = new();

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value null
		[ImportMany(typeof(IPuzzle))]
		private IEnumerable<IPuzzle> puzzleDays;
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value null

		public MainViewModel()
		{
			PuzzleYears = new ObservableCollection<TextViewModel>();
			PuzzleDays = new ObservableCollection<TextViewModel>();
			Inputs = new ObservableCollection<TextViewModel>();
			Solvers = new ObservableCollection<TextViewModel>();

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

			//Fill the imports of this object
			container.ComposeParts(this);

			foreach (var year in puzzleDays.Select(p => p.Year).Distinct().OrderByDescending(p => p).ToList())
			{
				puzzleYearDays[year] = new List<TextViewModel>();

				var yearViewModel = new TextViewModel(year.ToString());
				puzzleYearMap.Add(yearViewModel, year);
				PuzzleYears.Add(yearViewModel);
			}

			foreach (var puzzleDay in puzzleDays.OrderByDescending(p => p.Name).ToList())
			{
				var dayViewModel = new TextViewModel(puzzleDay.Name);
				puzzleDayMap.Add(dayViewModel, puzzleDay);
				puzzleYearDays[puzzleDay.Year].Add(dayViewModel);
			}

			SelectedPuzzleYear = PuzzleYears[0];
		}

		public ObservableCollection<TextViewModel> PuzzleYears { get; }

		public ObservableCollection<TextViewModel> PuzzleDays { get; }

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

				if (puzzleDayMap.TryGetValue(selectedPuzzleDayViewModel, out selectedPuzzleDay))
				{
					foreach (var key in selectedPuzzleDay.Inputs.Keys.OrderBy(k => k))
						Inputs.Add(new TextViewModel(key));

					foreach (var key in selectedPuzzleDay.Solvers.Keys.OrderBy(k => k))
						Solvers.Add(new TextViewModel(key));
				}
			}
		}

		public string PuzzleName
		{
			get
			{
				if (selectedPuzzleDay == null)
					return String.Empty;

				return selectedPuzzleDay.Name;
			}
		}

		public ObservableCollection<TextViewModel> Inputs
		{
			get;
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
					InputText = GetInputText(selectedPuzzleYear, selectedPuzzleDay, value.Text);
				}
			}
		}

		private string GetInputText(int selectedPuzzleYear, IPuzzle selectedPuzzleDay, string key)
		{
			var inputText = this.selectedPuzzleDay.Inputs[key];

			if (string.IsNullOrEmpty(inputText))
			{
				inputText = Helper.GetInputText(selectedPuzzleYear, selectedPuzzleDay.Day);
				if (!string.IsNullOrEmpty(inputText))
					this.selectedPuzzleDay.Inputs[key] = inputText;
			}

			return inputText;
		}

		public ObservableCollection<TextViewModel> Solvers
		{
			get;
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
					OutputText = selectedPuzzleDay.Solvers[value.Text](InputText);
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
	}
}
