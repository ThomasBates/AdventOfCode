using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.SegmentList;
using AoC.Grammar;
using AoC.Puzzles2018.Properties;

namespace AoC.Puzzles2018;

[Export(typeof(IPuzzle))]
public class Day04 : IPuzzle
{
	#region IPuzzle Properties

	public int Year => 2018;

	public int Day => 4;

	public string Name => $"Day {Day:00}";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day04Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	public Day04()
	{
		Solvers.Add("Solve Part 1", SolvePart1);
		Solvers.Add("Solve Part 2", SolvePart2);
	}

	#endregion Constructors

	private enum GuardEventType
	{
		None,
		BeginShift,
		FallAsleep,
		WakeUp
	}

	private class GuardEvent
	{
		public int Year;
		public int Month;
		public int Day;
		public int Hour;
		public int Minute;
		public DateTime TimeStamp;
		public int GuardID;
		public GuardEventType EventType;

		public override string ToString()
		{
			return $"[{Year}-{Month}-{Day} {Hour}:{Minute} {GuardID} {EventType}";
		}
	}

	private GuardEvent _guardEvent;
	private readonly List<GuardEvent> _guardEvents = new();
	private readonly Stack<string> _valueStack = new();

	public string SolvePart1(string input)
	{
		_guardEvents.Clear();
		_valueStack.Clear();

		var grammarReader = new L2GrammarReader();
		var _grammar = grammarReader.ReadGrammarDefinition(Resources.Day03Grammar);
		var _parser = new GrammarParser(_grammar);

		_parser.OnValueEmitted += Parser_ValueEmitted;
		_parser.OnTokenEmitted += Parser_TokenEmitted;

		InputHelper.TraverseInputLines(input, line =>
		{
			if (!String.IsNullOrWhiteSpace(line))
			{
				_guardEvent = new GuardEvent();
				_parser.ParseInput(line);
				_guardEvents.Add(_guardEvent);
			}
		});

		_guardEvents.Sort((x, y) => DateTime.Compare(x.TimeStamp, y.TimeStamp));

		var guardLists = new Dictionary<int, ISegmentList>();
		int guardID = -1;
		ISegmentList guardList = null;

		int minMeasure = -1;
		int maxMeasure = -1;

		foreach (var guardEvent in _guardEvents)
		{
			switch (guardEvent.EventType)
			{
				default:
					throw new InvalidOperationException($"Invalid Event Type: {guardEvent.EventType}");
				case GuardEventType.BeginShift:
					guardID = guardEvent.GuardID;
					if (!guardLists.TryGetValue(guardID, out guardList))
					{
						guardList = new AccumulatingSegmentList();
						guardLists.Add(guardID, guardList);
						guardList.AddSegment(0, 60, 0);
					}
					break;
				case GuardEventType.FallAsleep:
					if (guardID < 0)
					{
						throw new InvalidOperationException("Guard ID not set.");
					}
					minMeasure = guardEvent.Minute;
					break;
				case GuardEventType.WakeUp:
					if (guardID < 0)
					{
						throw new InvalidOperationException("Guard ID not set.");
					}
					maxMeasure = guardEvent.Minute;
					guardList.AddSegment(minMeasure, maxMeasure, 1);
					break;
			}
		}

		int maxGuardID = -1;
		ISegmentList maxGuardList = null;
		double maxMinutes = 0;
		foreach (var entry in guardLists)
		{
			guardID = entry.Key;
			guardList = entry.Value;

			double totalMinutes = 0;

			for (int i = 0; i < guardList.Count; i++)
			{
				ISegmentListItem segment = guardList[i];

				totalMinutes += (segment.MaxMeasure - segment.MinMeasure) * segment.Value;
			}

			if (totalMinutes > maxMinutes)
			{
				maxMinutes = totalMinutes;
				maxGuardID = guardID;
				maxGuardList = guardList;
			}
		}

		double maxValue = 0;
		ISegmentListItem maxSegment = null;

		for (int i = 0; i < maxGuardList.Count; i++)
		{
			ISegmentListItem segment = maxGuardList[i];

			if (segment.Value > maxValue)
			{
				maxValue = segment.Value;
				maxSegment = segment;
			}
		}

		return $"Guard {maxGuardID} from 00:{maxSegment.MinMeasure} to 00:{maxSegment.MaxMeasure}.  Answer is {maxGuardID * maxSegment.MinMeasure}.";
	}

	public string SolvePart2(string input)
	{
		_guardEvents.Clear();
		_valueStack.Clear();

		var grammarReader = new L2GrammarReader();
		var _grammar = grammarReader.ReadGrammarDefinition(Resources.Day03Grammar);
		var _parser = new GrammarParser(_grammar);

		_parser.OnValueEmitted += Parser_ValueEmitted;
		_parser.OnTokenEmitted += Parser_TokenEmitted;

		InputHelper.TraverseInputLines(input, line =>
		{
			if (!String.IsNullOrWhiteSpace(line))
			{
				_guardEvent = new GuardEvent();
				_parser.ParseInput(line);
				_guardEvents.Add(_guardEvent);
			}
		});

		_guardEvents.Sort((x, y) => DateTime.Compare(x.TimeStamp, y.TimeStamp));

		var guardLists = new Dictionary<int, ISegmentList>();
		int guardID = -1;
		ISegmentList guardList = null;

		int minMeasure = -1;
		int maxMeasure = -1;

		foreach (var guardEvent in _guardEvents)
		{
			switch (guardEvent.EventType)
			{
				default:
					throw new InvalidOperationException($"Invalid Event Type: {guardEvent.EventType}");
				case GuardEventType.BeginShift:
					guardID = guardEvent.GuardID;
					if (!guardLists.TryGetValue(guardID, out guardList))
					{
						guardList = new AccumulatingSegmentList();
						guardLists.Add(guardID, guardList);
						guardList.AddSegment(0, 60, 0);
					}
					break;
				case GuardEventType.FallAsleep:
					if (guardID < 0)
					{
						throw new InvalidOperationException("Guard ID not set.");
					}
					minMeasure = guardEvent.Minute;
					break;
				case GuardEventType.WakeUp:
					if (guardID < 0)
					{
						throw new InvalidOperationException("Guard ID not set.");
					}
					maxMeasure = guardEvent.Minute;
					guardList.AddSegment(minMeasure, maxMeasure, 1);
					break;
			}
		}

		int maxGuardID = -1;
		ISegmentListItem maxSegment = null;
		double maxValue = 0;
		foreach (var entry in guardLists)
		{
			guardID = entry.Key;
			guardList = entry.Value;

			for (int i = 0; i < guardList.Count; i++)
			{
				ISegmentListItem segment = guardList[i];

				if (segment.Value > maxValue)
				{
					maxGuardID = guardID;
					maxSegment = segment;
					maxValue = segment.Value;
				}
			}
		}

		return $"Guard {maxGuardID} from 00:{maxSegment.MinMeasure} to 00:{maxSegment.MaxMeasure} ({maxSegment.Value} times).  Answer is {maxGuardID * maxSegment.MinMeasure}.";
	}

	#region Event Handler Methods

	/// <summary>
	/// Handles the ValueEmitted event of the Parser control.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="Dynamic.Parser.ParserEventArgs"/> instance containing the event data.</param>
	void Parser_ValueEmitted(object sender, GrammarEmitEventArgs e)
	{
		_valueStack.Push(e.Value);
	}

	/// <summary>
	/// Handles the TokenEmitted event of the Parser control.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="Dynamic.Parser.ParserEventArgs"/> instance containing the event data.</param>
	void Parser_TokenEmitted(object sender, GrammarEmitEventArgs e)
	{
		//DebugPrint("Filter", e.Token);

		if (string.IsNullOrEmpty(e.Token))
			return;
		switch (e.Token[0])
		{
			case 's':
				ScopeController(e.Token);
				break;
			case 't':
				TypeChecker(e.Token);
				break;
			case 'c':
				CodeGenerator(e.Token);
				break;
		}
	}

	#endregion
	#region Local Support Methods

	/// <summary>
	/// Controls the scope.
	/// </summary>
	/// <param name="token">The scope token.</param>
	protected virtual void ScopeController(string token)
	{
	}

	/// <summary>
	/// checks the type.
	/// </summary>
	/// <param name="token">The type token.</param>
	protected virtual void TypeChecker(string token)
	{
		switch (token)
		{
			case "t_timestamp":
				_guardEvent.TimeStamp = new DateTime(
					_guardEvent.Year, 
					_guardEvent.Month, 
					_guardEvent.Day, 
					_guardEvent.Hour, 
					_guardEvent.Minute, 
					0);
				break;

			case "t_beginsShift":
				_guardEvent.EventType = GuardEventType.BeginShift;
				break;

			case "t_fallsAsleep":
				_guardEvent.EventType = GuardEventType.FallAsleep;
				break;

			case "t_wakesUp":
				_guardEvent.EventType = GuardEventType.WakeUp;
				break;
		}
	}

	/// <summary>
	/// Generates the code.
	/// </summary>
	/// <param name="token">The code token.</param>
	protected virtual void CodeGenerator(string token)
	{
		string value;

		switch (token)
		{
			case "c_year":
				value = _valueStack.Pop();
				_guardEvent.Year = int.Parse(value);
				break;
			case "c_month":
				value = _valueStack.Pop();
				_guardEvent.Month = int.Parse(value);
				break;
			case "c_day":
				value = _valueStack.Pop();
				_guardEvent.Day = int.Parse(value);
				break;
			case "c_hour":
				value = _valueStack.Pop();
				_guardEvent.Hour = int.Parse(value);
				break;
			case "c_minute":
				value = _valueStack.Pop();
				_guardEvent.Minute = int.Parse(value);
				break;

			case "c_guardId":
				value = _valueStack.Pop();
				_guardEvent.GuardID = int.Parse(value);
				break;
		}
	}

	#endregion
}
