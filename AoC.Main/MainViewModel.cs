using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;

namespace AoC.Main;

class MainViewModel : ViewModel
{
	#region Private Members

	private readonly ILogger messengerLogger;
	private readonly ILogger fileLogger;
	private readonly ILogger logger;

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value null
#pragma warning disable IDE0044 // Add readonly modifier
	[ImportMany(typeof(IPuzzle))]
	private IEnumerable<IPuzzle> importedPuzzles;
#pragma warning restore IDE0044 // Add readonly modifier
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value null

	private readonly Dictionary<int, List<IPuzzle>> yearPuzzles = new();

	private int selectedPuzzleYear;
	private IPuzzle selectedPuzzle;
	private string selectedInputs;
	private string selectedSolver;

	private string outputText;
	private string inputText;
	private SeverityLevel selectedSeverityLevel;

	#endregion Private Members

	#region Constructors

	public MainViewModel()
	{
		var logMessenger = new Messenger();
		messengerLogger = new MessengerLogger(logMessenger, SeverityLevel.Debug);
		fileLogger = new FileLogger(SeverityLevel.Verbose);
		logger = new AggregateLogger(new ILogger[] { messengerLogger, fileLogger });

		logMessenger.OnMessageSent += LogMessenger_OnMessageSent;

		CopyOutputCommand = new RelayCommand(CopyOutput, CanCopyOutput);
		CopyLogCommand = new RelayCommand(CopyLog, CanCopyLog);

		//  Use MEF to compose the list of puzzles.
		var catalog = new AggregateCatalog();
		catalog.Catalogs.Add(new DirectoryCatalog(
			Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)));

		CompositionContainer container = new(catalog);
		container.ComposeExportedValue(logger);

		container.ComposeParts(this);

		foreach (var puzzle in importedPuzzles.OrderByDescending(p => p.Year).ThenByDescending(p => p.Name).ToList())
		{
			if (!PuzzleYears.Contains(puzzle.Year))
			{
				PuzzleYears.Add(puzzle.Year);
				yearPuzzles[puzzle.Year] = new List<IPuzzle>();
			}

			yearPuzzles[puzzle.Year].Add(puzzle);
		}

		SelectedPuzzleYear = 2016; // PuzzleYears[0];
		//SelectedPuzzle = yearPuzzles[2022].FirstOrDefault(p => p.Day == 11);

		foreach (SeverityLevel level in Enum.GetValues(typeof(SeverityLevel)))
			SeverityLevels.Add(level);
		SelectedSeverityLevel = messengerLogger.Severity;
	}

	#endregion Constructors

	#region Properties

	public ObservableCollection<int> PuzzleYears { get; } = new();

	public ObservableCollection<IPuzzle> Puzzles { get; } = new();

	public ObservableCollection<string> Inputs { get; } = new();

	public ObservableCollection<string> Solvers { get; } = new();

	public ObservableCollection<SeverityLevel> SeverityLevels { get; } = new();

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

				if (string.IsNullOrEmpty(InputText)) 
				{
					logger.SendError("Core", "No inputs selected.");
					return;
				}

				new Thread(() =>
				{
					Thread.CurrentThread.IsBackground = true;
					var begin = DateTime.Now;
					logger.SendInfo("Core", $"Begin");
					OutputText = selectedPuzzle.Solvers[value](InputText);
					var end = DateTime.Now;
					logger.SendInfo("Core", $"End: {end - begin}");
				}).Start();
			}

			SelectedSolver = null;
		}
	}

	public SeverityLevel SelectedSeverityLevel
	{
		get => selectedSeverityLevel;
		set
		{
			if (selectedSeverityLevel == value) 
				return;

			selectedSeverityLevel = value;
			NotifyPropertyChanged();

			messengerLogger.Severity = value;
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

	#endregion Properties

	#region Commands

	public ICommand CopyOutputCommand { get; }

	private bool CanCopyOutput(object obj)
	{
		return !string.IsNullOrEmpty(OutputText);
	}

	private void CopyOutput(object obj)
	{
		Clipboard.SetDataObject(OutputText);
	}

	public ICommand CopyLogCommand { get; }

	private bool CanCopyLog(object obj)
	{
		return MessageLog.Count > 0;
	}

	private void CopyLog(object obj)
	{
		var log = new StringBuilder();
		foreach (var message in MessageLog)
			log.AppendLine(message);
		Clipboard.SetDataObject(log.ToString());
	}

	#endregion Commands

	#region Private Methods

	private void LogMessenger_OnMessageSent(object sender, string message)
	{
		App.Current.Dispatcher.Invoke(() =>
		{
			MessageLog.Add(message);
		});
	}

	private string GetInputText(int selectedPuzzleYear, IPuzzle selectedPuzzle, string key)
	{
		var inputText = this.selectedPuzzle.Inputs[key];

		if (string.IsNullOrEmpty(inputText))
		{
			inputText = InputHelper.GetInputText(selectedPuzzleYear, selectedPuzzle.Day);
			if (!string.IsNullOrEmpty(inputText))
				this.selectedPuzzle.Inputs[key] = inputText;
		}

		return inputText;
	}

	#endregion Private Methods
}
