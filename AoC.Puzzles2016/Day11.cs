using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

		Solvers.Add("Solve Part 1 (linear)", input => SolvePart1Linear(LoadData(input)).ToString());
		Solvers.Add("Solve Part 2 (linear)", input => SolvePart2Linear(LoadData(input)).ToString());

		Solvers.Add("Solve Part 1 (parallel)", input => SolvePart1Parallel(LoadData(input)).ToString());
		Solvers.Add("Solve Part 2 (parallel)", input => SolvePart2Parallel(LoadData(input)).ToString());
	}

	#endregion Constructors

	#region Helpers

	void LoggerSendDebug(string message)
	{
		lock (logger)
			logger.SendDebug(nameof(Day11), message);
	}

	void LoggerSendVerbose(string message)
	{
		lock (logger)
			logger.SendVerbose(nameof(Day11), message);
	}

	#endregion Helpers

	const string generator = nameof(generator);
	const string microchip = nameof(microchip);

	private class Component
	{
		public string Element { get; set; }
		public string ComponentType { get; set; }
		public string Name => $"{Element} {ComponentType}";
		public string ShortName => $"{Element[0]}{ComponentType[0]}".ToUpper();
		public override string ToString() => Name;
	}

	private class State
	{
		public State Parent;
		public long ID = 0;
		public int Steps = 0;
		public int ElevatorFloor = 1;
		public List<(Component component, int floor)> Components = new();

		// "THE MOST IMPORTANT, ABSOLUTELY ESSENTIAL:
		// ALL PAIRS ARE INTERCHANGEABLE - The following two states are EQUIVALENT:
		// (HGen@floor0, HChip@floor1, LGen@floor2, LChip@floor2)
		// (LGen@floor0, LChip@floor1, HGen@floor2, HChip@floor2)
		// prune any state EQUIVALENT TO (not just exactly equal to) a state you have already seen!"
		// https://www.reddit.com/r/adventofcode/comments/5hoia9/comment/db1v1ws/?utm_source=share&utm_medium=web2x&context=3
		long? longHashCode = null;
		public long LongHashCode
		{
			get
			{
				if (longHashCode == null)
				{
					var pairs = new List<(int genFloor, int chipFloor)>();
					foreach (var element in Components.Select(c => c.component.Element).Distinct())
					{
						var genFloor = Components.FirstOrDefault(c => c.component.Element == element && c.component.ComponentType == generator).floor;
						var chipFloor = Components.FirstOrDefault(c => c.component.Element == element && c.component.ComponentType == microchip).floor;
						pairs.Add((genFloor, chipFloor));
					}
					var ordered = pairs.OrderBy(p => p.genFloor).ThenBy(p => p.chipFloor);

					longHashCode = ElevatorFloor - 1;
					foreach (var (genFloor, chipFloor) in ordered)
					{
						longHashCode = longHashCode * 4 + genFloor - 1;
						longHashCode = longHashCode * 4 + chipFloor - 1;
					}
				}
				return longHashCode.Value;
			}
		}

		public override string ToString()
		{
			//  display state like https://adventofcode.com/2016/day/11
			var builder = new StringBuilder();
			var ordered = Components.OrderBy(c => c.component.Name).ToList();
			builder.AppendLine($"[{ID}] Step {Steps}:");
			for (int floor = 4; floor >= 1; floor--)
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

		LoggerSendDebug($"Initial state: {initialState}");

		return initialState;
	}

	private List<State> finalStates;

	private int SolvePart1Linear(State initialState)
	{
		FindFinalStatesLinear(initialState);

		LoggerSendDebug("");
		LoggerSendDebug("Final Solution:");
		
		foreach (var finalState in finalStates)
			VisualizeState(finalState);	

		return finalStates.FirstOrDefault()?.Steps ?? 0;
	}

	private int SolvePart1Parallel(State initialState)
	{
		FindFinalStatesParallel(initialState);

		LoggerSendDebug("");
		LoggerSendDebug("Final Solution:");

		foreach (var finalState in finalStates)
			VisualizeState(finalState);

		return finalStates.FirstOrDefault()?.Steps ?? 0;
	}

	private int SolvePart2Linear(State initialState)
	{
		AddExtraComponents(initialState);
		return SolvePart1Linear(initialState);
	}

	private int SolvePart2Parallel(State initialState)
	{
		AddExtraComponents(initialState);
		return SolvePart1Parallel(initialState);
	}

	private void AddExtraComponents(State initialState)
	{
		initialState.Components.Add((new Component { Element = "elerium", ComponentType = generator }, 1));
		initialState.Components.Add((new Component { Element = "elerium", ComponentType = microchip }, 1));
		initialState.Components.Add((new Component { Element = "dilithium", ComponentType = generator }, 1));
		initialState.Components.Add((new Component { Element = "dilithium", ComponentType = microchip }, 1));
		initialState.Components = initialState.Components.OrderBy(c => c.component.Name).ToList();

		LoggerSendDebug($"Real Initial state: {initialState}");
	}

	private Queue<State> linearQueue;
	private BlockingCollection<State> parallelQueue;
	private HashSet<long> seen;
	private long nextID;

	private int lastStep = 0;
	private int stepStateCount = 0;
	private readonly object locker = new();

	private void FindFinalStatesLinear(State initialState)
	{
		linearQueue = new();
		linearQueue.Enqueue(initialState);

		seen = new() { initialState.LongHashCode };

		finalStates = new();
		nextID = 1;

		lastStep = 0;
		stepStateCount = 0;

		while (linearQueue.Count > 0)
		{
			var state = linearQueue.Dequeue();

			if (state.Steps > lastStep)
			{
				lastStep = state.Steps;
				LoggerSendDebug($"Step {lastStep} - {stepStateCount} processed, queue size = {linearQueue.Count + 1}");
				stepStateCount = 0;
			}
			stepStateCount++;

			ProcessState(state);

			lock (finalStates)
				if (finalStates.Count > 0)
					break;
		}
		linearQueue = null;
	}

	private void FindFinalStatesParallel(State initialState)
	{
		parallelQueue = new() { initialState };

		seen = new() { initialState.LongHashCode };

		finalStates = new();
		nextID = 1;

		lastStep = 0;
		stepStateCount = 0;

		using var source = new CancellationTokenSource();

		var threads = new List<Thread>();

		var thread = new Thread(() => MonitorStateQueueWorker(source)) { Name = "Monitor" };
		threads.Add(thread);
		thread.Start();

		for (int i = 0; i < Environment.ProcessorCount; i++)
		{
			thread = new Thread(() => ProcessStateQueueWorker(source.Token)) { Name = $"{i + 1}" };
			threads.Add(thread);
			thread.Start();
		}

		WaitHandle.WaitAll(new WaitHandle[] { source.Token.WaitHandle });

		while (threads.Any(t => t.ThreadState == ThreadState.Running))
			Thread.Sleep(10);

		parallelQueue = null;
	}

	private void MonitorStateQueueWorker(CancellationTokenSource source)
	{
		while (true)
		{
			lock (finalStates)
			{
				if (finalStates.Count > 0)
				{
					source.Cancel();
					return;
				}
			}

			Thread.Sleep(100);
		}
	}

	public void ProcessStateQueueWorker(CancellationToken cancellationToken)
	{
		while (!cancellationToken.IsCancellationRequested)
		{
			try
			{
				parallelQueue.TryTake(out var state, -1, cancellationToken);

				lock (locker)
				{
					if (state.Steps > lastStep)
					{
						lastStep = state.Steps;
						LoggerSendDebug($"Thread {Thread.CurrentThread.Name} - Step {lastStep} - {stepStateCount} processed, queue size = {parallelQueue.Count + 1}");
						stepStateCount = 0;
					}
					stepStateCount++;
				}

				ProcessState(state);
			}
			catch (OperationCanceledException)
			{
				break;
			}
		}
	}

	private void ProcessState(State state)
	{
		var verbose = logger.Severity == SeverityLevel.Verbose ? new StringBuilder() : null;

		verbose?.Append($"Processing state: {state}");

		var candidates = state.Components.Where(c => c.floor == state.ElevatorFloor);
		var pairs = new List<(Component first, Component second)>();

		var singleUp = new List<State>();
		var doubleUp = new List<State>();
		var singleDown = new List<State>();
		var doubleDown = new List<State>();

		foreach (var first in candidates)
		{
			if (state.ElevatorFloor < 4)
			{
				verbose?.AppendLine($"\nConsidering moving {first.component.ShortName} up");
				var newState = CreateNewState(state, new[] { first.component }, 1);
				CheckState(newState, singleUp, verbose);
			}
			//  "minor: If floor 0 is empty and you are on floor 1,
			//  don't bother moving things back down to floor 0.
			//  Similarly, if floors 0 and 1 are both empty and you are on floor 2,
			//  don't bother moving things back down to floor 1."
			//  https://www.reddit.com/r/adventofcode/comments/5hoia9/comment/db1v1ws/?utm_source=share&utm_medium=web2x&context=3
			if (state.Components.Any(c => c.floor < state.ElevatorFloor))
			{
				verbose?.AppendLine($"\nConsidering moving {first.component.ShortName} down");
				var newState = CreateNewState(state, new[] { first.component }, -1);
				CheckState(newState, singleDown, verbose);
			}

			foreach (var second in candidates)
			{
				if (second == first)
				{
					verbose?.AppendLine($"\nConsidering moving {first.component.ShortName} and {second.component.ShortName}");
					verbose?.AppendLine("    Self.");
					continue;
				}

				if (pairs.Contains((first.component, second.component)) ||
					pairs.Contains((second.component, first.component)))
				{
					verbose?.AppendLine($"\nConsidering moving {first.component.ShortName} and {second.component.ShortName}");
					verbose?.AppendLine("    Already Done.");
					continue;
				}

				pairs.Add((first.component, second.component));

				if (state.ElevatorFloor < 4)
				{
					verbose?.AppendLine($"\nConsidering moving {first.component.ShortName} and {second.component.ShortName} up");
					var newState = CreateNewState(state, new[] { first.component, second.component }, 1);
					CheckState(newState, doubleUp, verbose);
				}
				//  "minor: If floor 0 is empty and you are on floor 1,
				//  don't bother moving things back down to floor 0.
				//  Similarly, if floors 0 and 1 are both empty and you are on floor 2,
				//  don't bother moving things back down to floor 1."
				//  https://www.reddit.com/r/adventofcode/comments/5hoia9/comment/db1v1ws/?utm_source=share&utm_medium=web2x&context=3
				if (state.Components.Any(c => c.floor < state.ElevatorFloor))
				{
					verbose?.AppendLine($"\nConsidering moving {first.component.ShortName} and {second.component.ShortName} down");
					var newState = CreateNewState(state, new[] { first.component, second.component }, -1);
					CheckState(newState, doubleDown, verbose);
				}
			}
		}

		//  "Kind of important:
		//  If you can move two items upstairs, don't bother bringing one item upstairs.
		//  If you can move one item downstairs, don't bother bringing two items downstairs."
		//  https://www.reddit.com/r/adventofcode/comments/5hoia9/comment/db1v1ws/?utm_source=share&utm_medium=web2x&context=3
		if (doubleUp.Count > 0)
		{
			verbose?.AppendLine($"Adding [{string.Join("], [", doubleUp.Select(s => s.ID))}]");
			AddNewStates(doubleUp);
		}
		else if (singleUp.Count > 0)
		{
			verbose?.AppendLine($"Adding [{string.Join("], [", singleUp.Select(s => s.ID))}]");
			AddNewStates(singleUp);
		}

		if (singleDown.Count > 0)
		{
			verbose?.AppendLine($"Adding [{string.Join("], [", singleDown.Select(s => s.ID))}]");
			AddNewStates(singleDown);
		}
		else if (singleDown.Count > 0)
		{
			verbose?.AppendLine($"Adding [{string.Join("], [", doubleDown.Select(s => s.ID))}]");
			AddNewStates(doubleDown);
		}

		if (verbose != null)
			LoggerSendVerbose(verbose.ToString());
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
				newState.Components.Add((component.component, component.floor));
			else
				newState.Components.Add((component.component, component.floor + direction));
		}

		_ = newState.LongHashCode;

		return newState;
	}

	private void CheckState(State state, List<State> states, StringBuilder verbose)
	{
		verbose?.Append($"Potential state: {state}");
		bool isSeen;

		lock(seen)
		{
			isSeen = seen.Contains(state.LongHashCode);
			if (!isSeen)
				seen.Add(state.LongHashCode);
		}

		if (isSeen)
		{
			verbose?.AppendLine("    Already seen.");
			return;
		}

		if (!IsValidState(state))
		{
			verbose?.AppendLine("    Unsafe.");
			return;
		}

		if (IsFinalState(state))
		{
			verbose?.AppendLine("    Final.");
			lock (finalStates)
				finalStates.Add(state);
			return;
		}

		verbose?.AppendLine("    Safe.");
		states.Add(state);
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

	private void AddNewStates(List<State> newStates)
	{
		foreach (var newState in newStates)
		{
			if (linearQueue != null)
				linearQueue.Enqueue(newState);
			else
				while (!parallelQueue.TryAdd(newState, -1)) ;
		}
	}

	private void VisualizeState(State state)
	{
		if (state?.Parent != null)
			VisualizeState(state.Parent);

		LoggerSendDebug($"{state?.ToString() ?? "NULL"}");
	}
}
