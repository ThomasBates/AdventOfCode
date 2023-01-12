using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.Serialization;
using System.Text;
using AoC.Common;
using AoC.Common.Helpers;
using AoC.Common.Logger;
using AoC.Puzzles2016.Properties;

namespace AoC.Puzzles2016;

[Export(typeof(IPuzzle))]
public class Day11 : IPuzzle
{
	#region Private Members

	private readonly ILogger logger;

	#endregion Private Members

	#region IPuzzle Properties

	public int Year => 2016;

	public int Day => 11;

	public string Name => $"Day 11";

	public Dictionary<string, string> Inputs { get; } = new()
	{
		{"Example Inputs", Resources.Day11Inputs},
		{"Puzzle Inputs",  ""}
	};

	public Dictionary<string, Func<string, string>> Solvers { get; } = new();

	#endregion IPuzzle Properties

	#region Constructors

	[ImportingConstructor]
	public Day11(ILogger logger)
	{
		this.logger = logger;

		Solvers.Add("Solve Part 1", input => SolvePart1(LoadData(input)).ToString());
		Solvers.Add("Solve Part 2", input => SolvePart2(LoadData(input)).ToString());
	}

	#endregion Constructors

	const string generator = nameof(generator);
	const string microchip = nameof(microchip);

	long nextID = 1;

	private class Component
	{
		public string Element { get; set; }
		public string ComponentType { get; set; }
		public string Name => $"{Element} {ComponentType}";
		public string ShortName => $"{Element[0]}{ComponentType[0]}".ToUpper();

		public override string ToString()
		{
			return $"{Element} {ComponentType}";
		}
	}

	private class State : IEquatable<State>
	{
		public State Parent;
		public long ID = 0;
		public int Steps = 0;
		public int ElevatorFloor = 1;
		public List<(Component component, int floor)> Components = new();

		private List<(int genFloor, int chipFloor)> pairs;

		public List<(int genFloor, int chipFloor)> Pairs
		{
			get
			{
				if (pairs == null)
				{
					pairs = new List<(int genFloor, int chipFloor)>();
					foreach (var element in Components.Select(c => c.component.Element).Distinct())
					{
						var genFloor = Components.FirstOrDefault(c => c.component.Element == element && c.component.ComponentType == generator).floor;
						var chipFloor = Components.FirstOrDefault(c => c.component.Element == element && c.component.ComponentType == microchip).floor;
						pairs.Add((genFloor, chipFloor));
					}
				}
				return pairs;
			}
		}

		// "THE MOST IMPORTANT, ABSOLUTELY ESSENTIAL:
		// ALL PAIRS ARE INTERCHANGEABLE - The following two states are EQUIVALENT:
		// (HGen@floor0, HChip@floor1, LGen@floor2, LChip@floor2)
		// (LGen@floor0, LChip@floor1, HGen@floor2, HChip@floor2)
		// prune any state EQUIVALENT TO (not just exactly equal to) a state you have already seen!"
		// https://www.reddit.com/r/adventofcode/comments/5hoia9/comment/db1v1ws/?utm_source=share&utm_medium=web2x&context=3
		public bool Equals(State other)
		{
			if (ElevatorFloor != other.ElevatorFloor)
				return false;

			// check for "equivalent".
			var thisPairs = new List<(int genFloor, int chipFloor)>(Pairs);
			var otherPairs= new List<(int genFloor, int chipFloor)>(other.Pairs);

			while(thisPairs.Count>0)
			{
				var pair = thisPairs[0];
				thisPairs.RemoveAt(0);
				var index = otherPairs.IndexOf(pair);
				if (index < 0)
					return false;
				otherPairs.RemoveAt(index);
			}
			return true;
		}

		public override string ToString()
		{
			//  display state like https://adventofcode.com/2016/day/11
			var builder = new StringBuilder();
			var ordered = Components.OrderBy(c => c.component.Name).ToList();
			builder.AppendLine($"[{ID}] Step {Steps}:");
			for(int floor = 4;floor >= 1; floor--)
			{
				builder.Append($"F{floor}");
				if (ElevatorFloor == floor)
					builder.Append(" E ");
				else
					builder.Append(" . ");

				foreach (var component in ordered)
				{
					if (component.floor == floor)
						builder.Append($" {component.component.ShortName,-2}");
					else
						builder.Append($" . ");
				}
				builder.AppendLine();
			}
			return builder.ToString();
		}
	}

	private State LoadData(string input)
	{
		var data = new object();

		int currentFloor = -1;
		var initialState = new State();

		var ok = GrammarHelper.ParseInput(logger, input, Resources.Day11Grammar,
			scopeControllerAction: (token, valueStack) =>
			{
				switch (token)
				{
					case "s_floor":
						currentFloor = int.Parse(valueStack.Pop());
						break;
					default:
						logger.SendError("Parser", $"Unknown token: {token}");
						break;
				}
			},
			typeCheckerAction: (token, valueStack) =>
			{
				switch (token)
				{
					case "t_first":
						valueStack.Push("1");
						break;
					case "t_second":
						valueStack.Push("2");
						break;
					case "t_third":
						valueStack.Push("3");
						break;
					case "t_fourth":
						valueStack.Push("4");
						break;
					case "t_element":
						break;
					default:
						logger.SendError("Parser", $"Unknown token: {token}");
						break;
				}
			},
			codeGeneratorAction: (token, valueStack) =>
			{
				switch (token)
				{
					case "c_nothing":
						break;
					case "c_generator":
						initialState.Components.Add((new Component
						{
							Element = valueStack.Pop(),
							ComponentType = generator
						}, currentFloor));
						break;
					case "c_microchip":
						initialState.Components.Add((new Component
						{
							Element = valueStack.Pop(),
							ComponentType = microchip
						}, currentFloor));
						break;
					default:
						logger.SendError("Parser", $"Unknown token: {token}");
						break;
				}
			});

		if (!ok)
			return null;

		initialState.Components = initialState.Components.OrderBy(c => c.component.Name).ToList();

		logger.SendDebug(nameof(Day11), $"Initial state: {initialState}");

		return initialState;
	}

	private int SolvePart1(State initialState)
	{
		var finalState = FindFinalState(initialState);

		logger.SendDebug(nameof(Day11), "");
		logger.SendDebug(nameof(Day11), "Final Solution:");
		VisualizeState(finalState);

		return finalState.Steps;
	}

	private int SolvePart2(State initialState)
	{
		initialState.Components.Add((new Component { Element = "elerium", ComponentType = generator }, 1));
		initialState.Components.Add((new Component { Element = "elerium", ComponentType = microchip }, 1));
		initialState.Components.Add((new Component { Element = "dilithium", ComponentType = generator }, 1));
		initialState.Components.Add((new Component { Element = "dilithium", ComponentType = microchip }, 1));
		initialState.Components = initialState.Components.OrderBy(c => c.component.Name).ToList();

		logger.SendDebug(nameof(Day11), $"Real Initial state: {initialState}");

		var finalState = FindFinalState(initialState);

		logger.SendDebug(nameof(Day11), "");
		logger.SendDebug(nameof(Day11), "Final Solution:");
		VisualizeState(finalState);

		return finalState.Steps;
	}

	private State FindFinalState(State initialState)
	{
		var states = new Queue<State>();
		states.Enqueue(initialState);
		var seen = new List<State> { initialState };

		var lastStep = 0;
		var nextLog = DateTime.Now.AddSeconds(10);
		var stepStateCount = 0;
		var timeStateCount = 0;
		while (states.Count > 0)
		{
			if (states.Peek().Steps > lastStep)
			{
				lastStep = states.Peek().Steps;
				logger.SendDebug(nameof(Day11), $"Step {lastStep} - {stepStateCount} processed");
				nextLog = DateTime.Now.AddSeconds(-10);
				stepStateCount = 0;
			}
			if (DateTime.Now > nextLog)
			{
				logger.SendDebug(nameof(Day11), $"states size = {states.Count} - {timeStateCount} processed");
				nextLog = DateTime.Now.AddSeconds(10);
				timeStateCount = 0;
			}

			var state = states.Dequeue();
			stepStateCount++;
			timeStateCount++;

			State finalState = ProcessState(states, seen, state);
			if (finalState != null)
				return finalState;
		}

		return null;
	}

	private State ProcessState(Queue<State> states, List<State> seen, State state)
	{
		logger.SendVerbose(nameof(Day11), $"Processing state: {state}");

		var candidates = state.Components.Where(c => c.floor == state.ElevatorFloor);
		var pairs = new List<(Component first, Component second)>();

		var singleUp = new List<State>();
		var doubleUp = new List<State>();
		var singleDown = new List<State>();
		var doubleDown = new List<State>();

		foreach (var first in candidates)
		{
			logger.SendVerbose(nameof(Day11), $"    Considering moving {first.component.ShortName}");
			if (state.ElevatorFloor < 4)
			{
				var newState = CreateNewState(state, new[] { first.component }, 1);
				bool isFinal = CheckState(newState, singleUp, seen);
				if (isFinal)
					return newState;
			}
			//  "minor: If floor 0 is empty and you are on floor 1,
			//  don't bother moving things back down to floor 0.
			//  Similarly, if floors 0 and 1 are both empty and you are on floor 2,
			//  don't bother moving things back down to floor 1."
			if (state.Components.Any(c => c.floor < state.ElevatorFloor))
			{
				var newState = CreateNewState(state, new[] { first.component }, -1);
				bool isFinal = CheckState(newState, singleDown, seen);
				if (isFinal)
					return newState;
			}

			foreach (var second in candidates)
			{
				logger.SendVerbose(nameof(Day11), $"    Considering moving {first.component.ShortName} and {second.component.ShortName}");
				if (second == first)
				{
					logger.SendVerbose(nameof(Day11), "        Self.");
					continue;
				}

				if (pairs.Contains((first.component, second.component)) ||
					pairs.Contains((second.component, first.component)))
				{
					logger.SendVerbose(nameof(Day11), "        Done.");
					continue;
				}

				pairs.Add((first.component, second.component));

				if (state.ElevatorFloor < 4)
				{
					var newState = CreateNewState(state, new[] { first.component, second.component }, 1);
					bool isFinal = CheckState(newState, doubleUp, seen);
					if (isFinal)
						return newState;
				}
				//  "minor: If floor 0 is empty and you are on floor 1,
				//  don't bother moving things back down to floor 0.
				//  Similarly, if floors 0 and 1 are both empty and you are on floor 2,
				//  don't bother moving things back down to floor 1."
				if (state.Components.Any(c => c.floor < state.ElevatorFloor))
				{
					var newState = CreateNewState(state, new[] { first.component, second.component }, -1);
					bool isFinal = CheckState(newState, doubleDown, seen);
					if (isFinal)
						return newState;
				}
			}
		}

		// "Kind of important:
		// If you can move two items upstairs, don't bother bringing one item upstairs.
		// If you can move one item downstairs, don't bother bringing two items downstairs."
		// https://www.reddit.com/r/adventofcode/comments/5hoia9/comment/db1v1ws/?utm_source=share&utm_medium=web2x&context=3
		if (doubleUp.Count > 0)
		{
			logger.SendVerbose(nameof(Day11), $"    Adding {string.Join(", ", doubleUp.Select(s => s.ID))}");
			foreach (var newState in doubleUp)
				states.Enqueue(newState);
		}
		else if (singleUp.Count > 0)
		{
			logger.SendVerbose(nameof(Day11), $"    Adding {string.Join(", ", singleUp.Select(s => s.ID))}");
			foreach (var newState in singleUp)
				states.Enqueue(newState);
		}

		if (singleDown.Count > 0)
		{
			logger.SendVerbose(nameof(Day11), $"    Adding {string.Join(", ", singleDown.Select(s => s.ID))}");
			foreach (var newState in singleDown)
				states.Enqueue(newState);
		}
		else if (singleDown.Count > 0)
		{
			logger.SendVerbose(nameof(Day11), $"    Adding {string.Join(", ", doubleDown.Select(s => s.ID))}");
			foreach (var newState in doubleDown)
				states.Enqueue(newState);
		}

		return null;
	}

	private State CreateNewState(State state, Component[] components, int direction)
	{
		var newState = new State()
		{
			Parent = state,
			ID = nextID++,
			Steps = state.Steps + 1,
			ElevatorFloor = state.ElevatorFloor + direction
		};

		var componentList = components.ToList();
		foreach(var component in state.Components)
		{
			if (!componentList.Contains(component.component))
			{
				newState.Components.Add((component.component, component.floor));
			}
			else
			{
				newState.Components.Add((component.component, component.floor + direction));
			}
		}

		return newState;
	}

	private bool CheckState(State newState, List<State> states, List<State> seen)
	{
		logger.SendVerbose(nameof(Day11), $"    Potential state: {newState}");
		if (seen.Contains(newState))
		{
			logger.SendVerbose(nameof(Day11), "        Already seen.");
		}
		else
		{
			seen.Add(newState);

			if (IsValidState(newState))
			{
				if (IsFinalState(newState))
				{
					logger.SendVerbose(nameof(Day11), "        Final.");
					return true;
				}

				states.Add(newState);
				logger.SendVerbose(nameof(Day11), "        Safe.");
			}
			else
			{
				logger.SendVerbose(nameof(Day11), "        Not safe.");
			}
		}
		return false;
	}

	private bool IsValidState(State state)
	{
		for (int floor = 1; floor <= 4; floor++)
		{
			var components = state.Components.Where(s => s.floor == floor).Select(s => s.component);

			var generators = components.Where(c => c.ComponentType == generator);
			if (!generators.Any())
				continue;

			var microchips = components.Where(c => c.ComponentType == microchip);

			foreach (var microchip in microchips)
			{
				var isProtected = generators.Any(g => g.Element == microchip.Element);
				if (isProtected) continue;
				if (generators.Any(g => g.Element != microchip.Element))
					return false;
			}
		}

		return true;
	}

	private bool IsFinalState(State state) => state.Components.All(c => c.floor == 4);

	private void VisualizeState(State state)
	{
		if (state.Parent != null)
			VisualizeState(state.Parent);
		logger.SendDebug(nameof(Day11), $"{state}");
	}
}
