using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AoC.IO;
using AoC.Parser;
using AoC.Puzzle;
using AoC.Puzzles2022.Properties;

namespace AoC.Puzzles2022
{
	[Export(typeof(IPuzzle))]
	public class Day07 : IPuzzle
	{
		#region IPuzzle Properties

		public string Name => "Day 07";

		public Dictionary<string, string> Inputs { get; } = new Dictionary<string, string>()
		{
			{"Example Inputs", Resources.Day07ExampleInputs},
			{"Puzzle Inputs",  Resources.Day07PuzzleInputs}
		};

		public Dictionary<string, Func<string, string>> Solvers { get; } = new Dictionary<string, Func<string, string>>()
		{
			{ "Part 1", SolvePart1 },
			{ "Part 1 (with parser)", SolvePart1WithParser },
			{ "Part 2", SolvePart2 }
		};

		#endregion IPuzzle Properties

		private class File
		{
			public string Name;
			public int Size;
		}

		private class Directory
		{
			public string Name;
			public List<File> Files = new List<File>();
			public Directory Parent;
			public List<Directory> Directories = new List<Directory>();
			public int Size => Files.Sum(file => file.Size) + Directories.Sum(dir => dir.Size);
			public string Path => (Parent == null ? "" : Parent.Path) + Name + "/";
		}

		#region Solvers

		private static string SolvePart1(string input)
		{
			StringBuilder output = new StringBuilder();

			var root = new Directory { Name = "" };

			ReadDisk(input, root, output);

			AnalyzeDiskPart1(root, output);

			return output.ToString();
		}

		private static string SolvePart1WithParser(string input)
		{
			StringBuilder output = new StringBuilder();

			var root = new Directory { Name = "" };

			ReadDiskWithParser(input, root, output);

			AnalyzeDiskPart1(root, output);

			return output.ToString();
		}

		private static string SolvePart2(string input)
		{
			StringBuilder output = new StringBuilder();

			var root = new Directory { Name = "" };

			ReadDisk(input, root, output);

			AnalyzeDiskPart2(root, output);

			return output.ToString();
		}

		#endregion Solvers

		#region Private Methods

		private static void ReadDisk(string input, Directory root, StringBuilder output)
		{
			var currentDirectory = root;
			var currentCommand = "";

			Helper.TraverseInputLines(input, line =>
			{
				var tokens = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

				if (tokens.Length == 0)
				{
					output.AppendLine("Empty line.");
					return;
				}

				if (tokens[0] == "$")
				{
					if (tokens.Length == 1)
					{
						output.AppendLine("No Command on line.");
						return;
					}

					switch (tokens[1])
					{
						case "cd":
							currentCommand = "cd";
							if (tokens.Length == 2)
							{
								output.AppendLine("No argument for cd.");
								return;
							}

							var directoryName = tokens[2];

							switch (directoryName)
							{
								case "/":
									currentDirectory = root;
									break;
								case "..":
									if (currentDirectory.Parent == null)
									{
										output.AppendLine($"Directory {currentDirectory.Path} does not have a parent directory.");
										return;
									}
									currentDirectory = currentDirectory.Parent;
									break;
								default:
									var find = currentDirectory.Directories.FirstOrDefault(dir => dir.Name == directoryName);
									if (find is null)
									{
										output.AppendLine($"Directory {directoryName} not found in {currentDirectory.Path}.");
										return;
									}
									currentDirectory = find;
									break;
							}
							output.AppendLine($"Current directory is: {currentDirectory.Path}");
							break;
						case "ls":
							currentCommand = "ls";
							output.AppendLine($"Listing contents of: {currentDirectory.Path}");
							break;
					}
				}
				else if (currentCommand == "ls")
				{
					if (tokens[0] == "dir")
					{
						var directoryName = tokens[1];
						var find = currentDirectory.Directories.FirstOrDefault(dir => dir.Name == directoryName);
						if (find == null)
						{
							output.AppendLine($"Adding directory {directoryName} to {currentDirectory.Path}.");
							currentDirectory.Directories.Add(new Directory { Name = directoryName, Parent = currentDirectory });
						}
						else
						{
							output.AppendLine($"Directory {directoryName} already exists in {currentDirectory.Path}.");
						}
					}
					else
					{
						if (!int.TryParse(tokens[0], out int fileSize))
						{
							output.AppendLine("File size is invalid.");
							return;
						}
						var fileName = tokens[1];
						var find = currentDirectory.Files.FirstOrDefault(file => file.Name == fileName);
						if (find == null)
						{
							output.AppendLine($"Adding file {fileName} to {currentDirectory.Path}.");
							currentDirectory.Files.Add(new File { Name = fileName, Size = fileSize });
						}
						else
						{
							output.AppendLine($"File {fileName} already exists in {currentDirectory.Path}.");
						}
					}
				}
				else
				{
					output.AppendLine("Current Command is not 'ls'.");
					return;
				}

			});
		}

		private static void ReadDiskWithParser(string input, Directory root, StringBuilder output)
		{
			var currentDirectory = root;

			var valueStack = new Stack<string>();
			valueStack.Clear();

			using (var _grammar = new L2Grammar())
			using (var _parser = new L2Parser(_grammar))
			{
				try
				{
					_grammar.ReadGrammar(Resources.Day07Grammar);
				}
				catch (GrammarException ex)
				{
					output.AppendLine($"{ex.Message}");
				}
				_parser.ValueEmitted += Parser_ValueEmitted;
				_parser.TokenEmitted += Parser_TokenEmitted;

				Helper.TraverseInputLines(input, (Action<string>)(line =>
				{
					output.AppendLine(line);
					try
					{
						_parser.Parse(line);
					}
					catch (ParserException ex)
					{
						output.AppendLine($"{ex.Message}");
					}
				}));
			}

			void Parser_ValueEmitted(object sender, ParserEventArgs e)
			{
				output.AppendLine($"value emitted: {e.Value}");
				valueStack.Push(e.Value);
			}
			void Parser_TokenEmitted(object sender, ParserEventArgs e)
			{
				output.AppendLine($"token emitted: {e.Token}");

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
			void ScopeController(string token)
			{
			}
			void TypeChecker(string token)
			{
			}
			void CodeGenerator(string token)
			{
				string value;

				switch (token)
				{
					case "c_dir":   // add child directory to current directory
						{
							var directoryName = valueStack.Pop();
							var find = currentDirectory.Directories.FirstOrDefault(dir => dir.Name == directoryName);
							if (find == null)
							{
								output.AppendLine($"Adding directory {directoryName} to {currentDirectory.Path}.");
								currentDirectory.Directories.Add(new Directory { Name = directoryName, Parent = currentDirectory });
							}
							else
							{
								output.AppendLine($"Directory {directoryName} already exists in {currentDirectory.Path}.");
							}
						}
						break;
					case "c_file":  //	add file to current directory
						{
							var fileName = valueStack.Pop();
							var fileSizeString = valueStack.Pop();

							if (!int.TryParse(fileSizeString, out int fileSize))
							{
								output.AppendLine("File size is invalid.");
								return;
							}

							var find = currentDirectory.Files.FirstOrDefault(file => file.Name == fileName);
							if (find == null)
							{
								output.AppendLine($"Adding file {fileName} to {currentDirectory.Path}.");
								currentDirectory.Files.Add(new File { Name = fileName, Size = fileSize });
							}
							else
							{
								output.AppendLine($"File {fileName} already exists in {currentDirectory.Path}.");
							}
						}
						break;
					case "c_ls":    //	command to list files in directory
						break;
					case "c_extension": //	add extension to file name
						{
							var extension = valueStack.Pop();
							var fileName = valueStack.Pop();
							valueStack.Push($"{fileName}.{extension}");
						}
						break;
					case "c_cdRoot":
						currentDirectory = root;
						break;
					case "c_cdParent":
						if (currentDirectory.Parent == null)
						{
							output.AppendLine($"Directory {currentDirectory.Path} does not have a parent directory.");
							return;
						}
						currentDirectory = currentDirectory.Parent;
						break;
					case "c_cdChild":
						{
							var directoryName = valueStack.Pop();
							var find = currentDirectory.Directories.FirstOrDefault(dir => dir.Name == directoryName);
							if (find is null)
							{
								output.AppendLine($"Directory {directoryName} not found in {currentDirectory.Path}.");
								return;
							}
							currentDirectory = find;
						}
						break;
				}
			}
		}

		private static void AnalyzeDiskPart1(Directory root, StringBuilder output)
		{
			var directories = AllDescendantDirectories(root);

			var total = 0;
			foreach (var directory in directories)
			{
				var directorySize = directory.Size;
				output.AppendLine($"{directory.Path} = {directorySize}");
				if (directorySize <= 100000)
					total += directorySize;
			}
			output.AppendLine($"total = {total}");
		}

		private static void AnalyzeDiskPart2(Directory root, StringBuilder output)
		{
			var rootSize = root.Size;
			var minSize = rootSize - 40000000;
			output.AppendLine($"total size = {rootSize}. Need {minSize}");

			var directories = AllDescendantDirectories(root);

			var bestSize = 70000000;
			foreach (var directory in directories)
			{
				var directorySize = directory.Size;
				output.AppendLine($"{directory.Path} = {directorySize}");

				if (directorySize >= minSize && directorySize < bestSize)
				{
					output.AppendLine($":::::::::::::::::::    {directory.Path} = {directorySize}");
					bestSize = directorySize;
				}
			}
			output.AppendLine($"best size = {bestSize}");
		}

		private static List<Directory> AllDescendantDirectories(Directory root)
		{
			var result = new List<Directory>(root.Directories);
			foreach (var directory in root.Directories)
			{
				var directories = AllDescendantDirectories(directory);
				result.AddRange(directories);
			}
			return result;
		}

		#endregion Private Methods

	}
}
