using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2017.Properties;

namespace AoC.Puzzles2017;

[Export(typeof(IPuzzle))]
public class Day25 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2017;

	public int Day => 25;

	public string Name => $"Day 25";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day25Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day25(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", input => SolvePart1(LoadData(input)).ToString());
	}

	#endregion Constructors

	#region Helpers

	void SendDebug(string message = "") => logger.SendDebug(nameof(Day25), message);
	void SendVerbose(string message = "") => logger.SendVerbose(nameof(Day25), message);

	#endregion Helpers

	private class StateAction
	{
		public int WriteValue;
		public int MoveDirection;
		public string NextState;
	}

	private class State
	{
		public string Name;
		public StateAction action0 = new();
		public StateAction action1 = new();
	}

	private class Data
	{
		public string InitialState;
		public int Iterations;
		public Dictionary<string, State> States = new();
		public Dictionary<int, int> Tape = new();
		public State CurrentState;
		public int CurrentSlot;
	}

	private Data LoadData(string input)
	{
		var data = new Data();

		State state = null;
		StateAction action = null;

		GrammarHelper.ParseInput(logger, input, Resources.Day25Grammar,
			scopeControllerAction: (token, valueStack) =>
			{
				switch (token)
				{
					case "s_newState":
						state = new State() { Name = valueStack.Pop() };
						data.States.Add(state.Name, state);
						action = null;
						break;
					case "s_currentValue":
						action = valueStack.Pop() == "0" ? state.action0 : state.action1;
						break;
				}
			},
			typeCheckerAction: null,
			codeGeneratorAction: (token, valueStack) =>
			{
				switch (token)
				{
					case "c_initialState":
						data.InitialState = valueStack.Pop();
						break;
					case "c_iterations":
						data.Iterations = int.Parse(valueStack.Pop());
						break;
					case "c_writeValue":
						action.WriteValue = int.Parse(valueStack.Pop());
						break;
					case "c_moveLeft":
						action.MoveDirection = -1;
						break;
					case "c_moveRight":
						action.MoveDirection = 1;
						break;
					case "c_nextState":
						action.NextState = valueStack.Pop();
						break;
				}
			});

		data.CurrentState = data.States[data.InitialState];

		return data;
	}

	private object SolvePart1(Data data)
	{
		for (var i = 0; i < data.Iterations; i++)
			ClockStates(data);

		var checksum = data.Tape.Sum(slot => slot.Value);

		return checksum;
	}

	private void ClockStates(Data data)
	{
		if (!data.Tape.TryGetValue(data.CurrentSlot, out var value))
			value = 0;
		var action = value == 0 ? data.CurrentState.action0 : data.CurrentState.action1;

		data.Tape[data.CurrentSlot] = action.WriteValue;
		data.CurrentSlot += action.MoveDirection;
		data.CurrentState = data.States[action.NextState];
	}
}
