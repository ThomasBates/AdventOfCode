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
using AoC.Puzzles2022;

namespace AoC.Main
{
	class MainViewModel : ViewModel
	{
		private TextViewModel _selectedPuzzleViewModel;
		private IPuzzle _selectedPuzzle;
		private TextViewModel _selectedInputsViewModel;
		private TextViewModel _selectedSolverViewModel;

		private string _inputText;
		private string _outputText;

		private Dictionary<TextViewModel, IPuzzle> _puzzleMap = new Dictionary<TextViewModel, IPuzzle>();

		[ImportMany(typeof(IPuzzle))]
		private IEnumerable<IPuzzle> _puzzles;

		public MainViewModel()
		{
			Puzzles = new ObservableCollection<TextViewModel>();
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
			CompositionContainer container = new CompositionContainer(catalog);

			//Fill the imports of this object
			container.ComposeParts(this);

			List<IPuzzle> puzzleList = new List<IPuzzle>(_puzzles);
			puzzleList.Sort((a, b) => String.Compare(b.Name, a.Name));
			foreach (var puzzle in puzzleList)
			{
				var viewModel = new TextViewModel(puzzle.Name);
				Puzzles.Add(viewModel);
				_puzzleMap.Add(viewModel, puzzle);

				if (SelectedPuzzle == null)
				{
					SelectedPuzzle = viewModel;
				}
			}
		}

		public ObservableCollection<TextViewModel> Puzzles
		{
			get;
		}

		public TextViewModel SelectedPuzzle
		{
			get
			{
				return _selectedPuzzleViewModel;
			}
			set
			{
				if (_selectedPuzzleViewModel == value)
				{
					return;
				}
				_selectedPuzzleViewModel = value;
				_puzzleMap.TryGetValue(_selectedPuzzleViewModel, out _selectedPuzzle);

				NotifyPropertyChanged(nameof(SelectedPuzzle));
				NotifyPropertyChanged(nameof(PuzzleName));

				SelectedInputs = null;
				Inputs.Clear();
				foreach (var key in _selectedPuzzle.Inputs.Keys.OrderBy(k => k))
				{
					Inputs.Add(new TextViewModel(key));
				}

				SelectedSolver = null;
				Solvers.Clear();
				foreach (var key in _selectedPuzzle.Solvers.Keys.OrderBy(k => k))
				{
					Solvers.Add(new TextViewModel(key));
				}

				InputText = string.Empty;
				OutputText = string.Empty;
			}
		}

		public string PuzzleName
		{
			get
			{
				if (_selectedPuzzle == null)
				{
					return String.Empty;
				}
				return _selectedPuzzle.Name;
			}
		}

		public ObservableCollection<TextViewModel> Inputs
		{
			get;
		}

		public TextViewModel SelectedInputs
		{
			get
			{
				return _selectedInputsViewModel;
			}
			set
			{
				if (_selectedInputsViewModel == value)
				{
					return;
				}
				_selectedInputsViewModel = value;

				NotifyPropertyChanged(nameof(SelectedInputs));

				if (value != null)
				{
					InputText = _selectedPuzzle.Inputs[value.Text];
				}
			}
		}

		public ObservableCollection<TextViewModel> Solvers
		{
			get;
		}

		public TextViewModel SelectedSolver
		{
			get
			{
				return _selectedSolverViewModel;
			}
			set
			{
				if (_selectedSolverViewModel == value)
				{
					return;
				}
				_selectedSolverViewModel = value;

				NotifyPropertyChanged(nameof(SelectedSolver));

				if (value != null)
				{
					OutputText = _selectedPuzzle.Solvers[value.Text](InputText);
				}

				SelectedSolver = null;
			}
		}

		public string InputText
		{
			get
			{
				return _inputText;
			}
			set
			{
				if (String.Equals(value, _inputText))
				{
					return;
				}
				_inputText = value;
				NotifyPropertyChanged(nameof(InputText));
			}
		}

		public string OutputText
		{
			get
			{
				return _outputText;
			}
			set
			{
				if (String.Equals(value, _outputText))
				{
					return;
				}
				_outputText = value;
				NotifyPropertyChanged(nameof(OutputText));
			}
		}
	}
}
