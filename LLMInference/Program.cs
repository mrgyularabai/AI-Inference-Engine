using System;
using System.Collections.Generic;
using System.IO;
using Ozeki; 

namespace LLMInference
{
    class Program
    {
        private static string _selectedDirectory = @"C:\AIModels";

        private static List<string> _ggufFiles = new List<string>();

        static void Main(string[] args)
        {
            if (Directory.Exists(_selectedDirectory))
            {
                _ggufFiles = FindGGUFFiles(_selectedDirectory);
            }

            while (true)
            {
                ShowMainMenu();
            }
        }

        private static void ShowMainMenu()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                LLM Inference - Main Menu                 ║");
            Console.WriteLine("╠══════════════════════════════════════════════════════════╣");
            Console.WriteLine($"║  Selected Directory: {_selectedDirectory,-56}║".Substring(0, 58) + " ║");
            Console.WriteLine($"║  GGUF Files Found: {_ggufFiles.Count,-56}║".Substring(0, 58) + " ║");
            Console.WriteLine("╠══════════════════════════════════════════════════════════╣");
            Console.WriteLine("║                                                          ║");
            Console.WriteLine("║  [1] Select Directory                                    ║");
            Console.WriteLine("║  [2] View GGUF Files                                     ║");
            Console.WriteLine("║  [3] Load Model & Generate Text                          ║");
            Console.WriteLine("║                                                          ║");
            Console.WriteLine("║  [0] Exit                                                ║");
            Console.WriteLine("║                                                          ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════╝");
            Console.Write("\n  Enter your choice: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    SelectDirectory();
                    break;
                case "2":
                    ViewGGUFFiles();
                    break;
                case "3":
                    LoadAndGenerate();
                    break;
                case "0":
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }

        private static void SelectDirectory()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                     Select Directory                     ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("  Enter the full path to a directory:");
            Console.Write("  > ");

            var path = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(path))
            {
                Console.WriteLine("\n  No path entered. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            if (!Directory.Exists(path))
            {
                Console.WriteLine($"\n  ERROR: Directory does not exist: {path}");
                Console.WriteLine("  Press any key to continue...");
                Console.ReadKey();
                return;
            }

            _selectedDirectory = path;
            _ggufFiles = FindGGUFFiles(path);

            Console.WriteLine($"\n  Found {_ggufFiles.Count} GGUF file(s) in subdirectories.");
            Console.WriteLine("  Press any key to continue...");
            Console.ReadKey();
        }

        private static List<string> FindGGUFFiles(string directory)
        {
            var files = new List<string>();
            try
            {
                foreach (var file in Directory.GetFiles(directory, "*.gguf", SearchOption.AllDirectories))
                {
                    files.Add(file);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  Error scanning directory: {ex.Message}");
            }
            return files;
        }

        private static void ViewGGUFFiles()
        {
            if (string.IsNullOrEmpty(_selectedDirectory))
            {
                Console.Clear();
                Console.WriteLine("╔══════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                      GGUF Files                          ║");
                Console.WriteLine("╚══════════════════════════════════════════════════════════╝");
                Console.WriteLine();
                Console.WriteLine("  No directory selected. Please select a directory first.");
                Console.WriteLine("\n  Press any key to continue...");
                Console.ReadKey();
                return;
            }

            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                      GGUF Files                          ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine($"  Directory: {_selectedDirectory}");
            Console.WriteLine();

            if (_ggufFiles.Count == 0)
            {
                Console.WriteLine("  No GGUF files found.");
            }
            else
            {
                for (int i = 0; i < _ggufFiles.Count; i++)
                {
                    var file = _ggufFiles[i];
                    var relativePath = file.Replace(_selectedDirectory, "").TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                    Console.WriteLine($"  [{i + 1,3}] {relativePath}");
                }
            }

            Console.WriteLine();
            Console.WriteLine("  Press any key to continue...");
            Console.ReadKey();
        }

        private static void LoadAndGenerate()
        {
            if (_ggufFiles.Count == 0)
            {
                Console.WriteLine("\n  No GGUF files available. Please add models to the selected directory.");
                Console.WriteLine("  Press any key to continue...");
                Console.ReadKey();
                return;
            }

            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                Load Model & Generate Text                ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("  Available GGUF files:");
            Console.WriteLine();

            for (int i = 0; i < _ggufFiles.Count; i++)
            {
                var file = _ggufFiles[i];
                var relativePath = file.Replace(_selectedDirectory, "").TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                Console.WriteLine($"  [{i + 1,3}] {relativePath}");
            }

            Console.WriteLine();
            Console.WriteLine("  [0] Cancel");
            Console.Write("\n  Enter the number of the file to load: ");

            var input = Console.ReadLine();

            if (input == "0")
                return;

            if (!int.TryParse(input, out int selection) || selection < 1 || selection > _ggufFiles.Count)
            {
                Console.WriteLine("\n  Invalid selection. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            var selectedFile = _ggufFiles[selection - 1];

            var model = new OzAIModel();
            model.modelPath = selectedFile;
            if (!model.PerformStart(out string startError))
            {
                Console.WriteLine($"\n  Model start failed: {startError}");
                Console.WriteLine("  Press any key to continue...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\n  Model started successfully!");
            Console.Write("  Enter initial text prompt:");

            var currentText = Console.ReadLine();
            Console.Write(currentText);
            while (true)
            {
                model.GetNextWord(currentText, out string outputWord, out bool responseComplete, out string errorMessage);
                Console.Write(outputWord);
                currentText += outputWord;
            }
        }
    }
}
