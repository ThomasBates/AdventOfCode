using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

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

		public int Year => 2022;

		public int Day => 7;

		public string Name => $"Day {Day:00}";

		public Dictionary<string, string> Inputs { get; } = new()
		{
			{"Example Inputs", Resources.Day07Inputs},
			{"Puzzle Inputs",  ""}
		};

		public Dictionary<string, Func<string, string>> Solvers { get; } = new()
		{
			{ "Part 1", SolvePart1 },
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
			public List<File> Files = new();
			public Directory Parent;
			public List<Directory> Directories = new();
			public int Size => Files.Sum(file => file.Size) + Directories.Sum(dir => dir.Size);
			public string Path => (Parent == null ? "" : Parent.Path) + Name + "/";
		}

		#region Solvers

		private static string SolvePart1(string input)
		{
			var output = new StringBuilder();

			var root = new Directory { Name = "" };

			LoadDataFromInput(input, root, output);

			AnalyzeDiskPart1(root, output);

			return output.ToString();
		}

		private static string SolvePart2(string input)
		{
			var output = new StringBuilder();

			var root = new Directory { Name = "" };

			LoadDataFromInput(input, root, output);

			AnalyzeDiskPart2(root, output);

			return output.ToString();
		}

		#endregion Solvers

		#region Private Methods

		private static void LoadDataFromInput(string input, Directory root, StringBuilder output)
		{
			var currentDirectory = root;

			Helper.RunParser(input, Resources.Day07Grammar,
				null,
				null,
				(token, valueStack) =>
				{
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
						default:
							output.AppendLine($"Unknown parser token: {token}");
							break;
					}
				},
				(severity, category, message) =>
				{
					output.AppendLine($"[{severity,-7}] - [{category,-15}] - {message}");
				});
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
