using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

namespace CrossPlatformLab8
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("1. Task 1");
                Console.WriteLine("2. Task 2");
                Console.WriteLine("3. Task 3");
                Console.WriteLine("4. Task 4");
                Console.WriteLine("5. Task 5");
                Console.WriteLine("0. Exit");
                Console.Write("\nSelect a task (0-5): ");

                string choice = Console.ReadLine();
                Console.Clear();

                switch (choice)
                {
                    case "1":
                        Task1_IpAddressProcessor.Run();
                        break;
                    case "2":
                        Task2_WordLengthFilter.Run();
                        break;
                    case "3":
                        Task3_RemoveLongestWords.Run();
                        break;
                    case "4":
                        Task4_BinaryFileFractions.Run();
                        break;
                    case "5":
                        Task5_FileSystemLab.Run();
                        break;
                    case "0":
                        Console.WriteLine("Exiting the program...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a number from 0 to 5.");
                        break;
                }

                Console.WriteLine("Press any key to return to the menu...");
                Console.ReadKey();
            }
        }
    }

    class Task1_IpAddressProcessor
    {
        public static void Run()
        {
            string inputFile = @"D:\University\Ізгої\2 курс\2 семестр 2026\Крос-платформенне програмування\Lab8\Lab8\input.txt";
            string extractedIpsFile = @"D:\University\Ізгої\2 курс\2 семестр 2026\Крос-платформенне програмування\Lab8\Lab8\extracted_ips.txt";
            string modifiedFile = @"D:\University\Ізгої\2 курс\2 семестр 2026\Крос-платформенне програмування\Lab8\Lab8\modified_text.txt";

            CreateSampleFileIfNotExists(inputFile);

            Console.WriteLine($"Reading a file: {inputFile}");
            string text = File.ReadAllText(inputFile);
            Console.WriteLine(text + "\n");

            string ipPattern = @"\b(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b";
            Regex ipRegex = new Regex(ipPattern);

            MatchCollection matches = ipRegex.Matches(text);

            Console.WriteLine($"Found IP addresses: {matches.Count}");

            var ipList = matches.Cast<Match>().Select(m => m.Value).Distinct().ToList();
            File.WriteAllLines(extractedIpsFile, ipList);
            Console.WriteLine($"Unique IP addresses saved to file: {extractedIpsFile}\n");
            string modifiedText = text;

            Console.Write("Enter the IP address to REMOVE from the text: ");
            string ipToRemove = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(ipToRemove))
            {
                modifiedText = Regex.Replace(modifiedText, $@"\b{Regex.Escape(ipToRemove)}\b", "");
                Console.WriteLine($"IP address {ipToRemove} removed.\n");
            }

            Console.Write("Enter the IP address to REPLACE: ");
            string ipToReplace = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(ipToReplace))
            {
                Console.Write("Enter the replacement text: ");
                string replacement = Console.ReadLine();

                modifiedText = Regex.Replace(modifiedText, $@"\b{Regex.Escape(ipToReplace)}\b", replacement);
                Console.WriteLine($"IP address {ipToReplace} replaced with '{replacement}'.\n");
            }

            File.WriteAllText(modifiedFile, modifiedText);
            Console.WriteLine($"\nModified text saved to file: {modifiedFile}");
            Console.WriteLine("Result:");
            Console.WriteLine(modifiedText);
        }

        static void CreateSampleFileIfNotExists(string filePath)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            if (!File.Exists(filePath))
            {
                string sampleText = "The database server is located at 192.168.0.15. " +
                                    "We also have a backup server: 10.0.0.1. " +
                                    "Invalid address 256.100.50.10 should not be recognized, " +
                                    "and 127.0.0.1 is the localhost. ";
                File.WriteAllText(filePath, sampleText);
            }
        }
    }

    class Task2_WordLengthFilter
    {
        public static void Run()
        {
            string inputFile = @"D:\University\Ізгої\2 курс\2 семестр 2026\Крос-платформенне програмування\Lab8\Lab8\input2.txt";
            string outputFile = @"D:\University\Ізгої\2 курс\2 семестр 2026\Крос-платформенне програмування\Lab8\Lab8\result2.txt";

            Directory.CreateDirectory(Path.GetDirectoryName(inputFile));

            if (!File.Exists(inputFile))
            {
                string sampleText = "This is a simple test file to check the program. " +
                                    "It contains various words: university, programming, " +
                                    "computer, code, database, data.";
                File.WriteAllText(inputFile, sampleText);
            }

            Console.WriteLine($"Reading file: {inputFile}");
            Console.WriteLine(File.ReadAllText(inputFile) + "\n");

            Console.Write("Enter desired word length: ");

            if (!int.TryParse(Console.ReadLine(), out int targetLength) || targetLength <= 0)
            {
                Console.WriteLine("Invalid value. Please enter a positive integer.");
                return;
            }

            string text = File.ReadAllText(inputFile);
            string wordPattern = @"\b[A-Za-z]+\b";

            var words = Regex.Matches(text, wordPattern)
                             .Cast<Match>()
                             .Select(m => m.Value)
                             .Where(w => w.Length == targetLength)
                             .ToList();

            if (words.Count > 0)
            {
                File.WriteAllLines(outputFile, words);
                Console.WriteLine($"\nFound words with {targetLength} characters: {words.Count}");
                foreach (var word in words)
                {
                    Console.WriteLine($"- {word}");
                }
                Console.WriteLine($"\nResult successfully saved to: {outputFile}");
            }
            else
            {
                File.WriteAllText(outputFile, "No words of the specified length found.");
                Console.WriteLine($"\nNo words of {targetLength} characters found. This info is saved to {outputFile}.");
            }
        }
    }

    class Task3_RemoveLongestWords
    {
        public static void Run()
        {
            string inputFile = @"D:\University\Ізгої\2 курс\2 семестр 2026\Крос-платформенне програмування\Lab8\Lab8\input3.txt";
            string outputFile = @"D:\University\Ізгої\2 курс\2 семестр 2026\Крос-платформенне програмування\Lab8\Lab8\result_no_longest3.txt";

            Directory.CreateDirectory(Path.GetDirectoryName(inputFile));

            if (!File.Exists(inputFile))
            {
                string sampleText = "This is a simple test. Programming is fascinating, but occasionally challenging! " +
                                    "We are finding the absolutely longest words in this document. Architecture.";
                File.WriteAllText(inputFile, sampleText);
            }

            string text = File.ReadAllText(inputFile);
            Console.WriteLine($"Reading file: {inputFile}");
            Console.WriteLine(text + "\n");

            string wordPattern = @"\b[A-Za-z]+\b";
            var matches = Regex.Matches(text, wordPattern);

            if (matches.Count == 0)
            {
                Console.WriteLine("No words found in the text.");
                return;
            }

            int maxLength = matches.Cast<Match>().Max(m => m.Length);

            var longestWords = matches.Cast<Match>()
                                      .Where(m => m.Length == maxLength)
                                      .Select(m => m.Value)
                                      .Distinct()
                                      .ToList();

            Console.WriteLine($"Maximum word length: {maxLength}");
            Console.WriteLine($"Longest words found: {string.Join(", ", longestWords)}\n");

            string modifiedText = Regex.Replace(text, wordPattern, match =>
            {
                return match.Length == maxLength ? "" : match.Value;
            });

            modifiedText = Regex.Replace(modifiedText, @" {2,}", " ");

            Console.WriteLine("Text after removing the longest words");
            Console.WriteLine(modifiedText.Trim());

            File.WriteAllText(outputFile, modifiedText.Trim());
            Console.WriteLine($"\nResult successfully saved to: {outputFile}");
        }
    }

    class Task4_BinaryFileFractions
    {
        public static void Run()
        {
            string fileName = @"D:\University\Ізгої\2 курс\2 семестр 2026\Крос-платформенне програмування\Lab8\Lab8\fractions.dat";
            Directory.CreateDirectory(Path.GetDirectoryName(fileName));

            Console.Write("Enter the number of elements (n): ");
            if (!int.TryParse(Console.ReadLine(), out int n) || n <= 0)
            {
                Console.WriteLine("Invalid input. Please enter a positive integer.");
                return;
            }

            using (BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.Create)))
            {
                for (int i = 1; i <= n; i++)
                {
                    double value = 1.0 / i;
                    writer.Write(value);
                }
            }

            Console.WriteLine($"\nSuccessfully wrote {n} numbers to the binary file '{fileName}'.");
            Console.WriteLine("Note: If you open this file in Notepad, it will look like unreadable symbols.\n");

            Console.WriteLine("Elements with ordinal numbers that are multiples of 3:");

            using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                int position = 1;

                while (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    double currentValue = reader.ReadDouble();

                    if (position % 3 == 0)
                    {
                        Console.WriteLine($"Element #{position}: {currentValue:F4}");
                    }

                    position++;
                }
            }
        }
    }

    class Task5_FileSystemLab
    {
        public static void Run()
        {
            string baseDir = @"d:\temp";
            string folder1 = Path.Combine(baseDir, "Vatamaniuk1");
            string folder2 = Path.Combine(baseDir, "Vatamaniuk2");
            string folderAll = Path.Combine(baseDir, "ALL");

            if (Directory.Exists(folder1)) Directory.Delete(folder1, true);
            if (Directory.Exists(folder2)) Directory.Delete(folder2, true);
            if (Directory.Exists(folderAll)) Directory.Delete(folderAll, true);
            if (!Directory.Exists(baseDir)) Directory.CreateDirectory(baseDir);

            Directory.CreateDirectory(folder1);
            Directory.CreateDirectory(folder2);
            Console.WriteLine("1. The following folders have been created: Vatamaniuk1 and Vatamaniuk2.");

            string fileT1 = Path.Combine(folder1, "t1.txt");
            string textT1 = "Шевченко Степан Іванович, 2001 року народження, місце проживання м. Суми";
            File.WriteAllText(fileT1, textT1);

            string fileT2 = Path.Combine(folder1, "t2.txt");
            string textT2 = "Комар Сергій Федорович, 2000 року народження, місце проживання м. Київ";
            File.WriteAllText(fileT2, textT2);
            Console.WriteLine("2. The files t1.txt and t2.txt have been created in the Vatamaniuk1 folder.");

            string fileT3 = Path.Combine(folder2, "t3.txt");
            string contentT1 = File.ReadAllText(fileT1);
            string contentT2 = File.ReadAllText(fileT2);
            File.WriteAllText(fileT3, contentT1 + "\r\n\r\n" + contentT2);
            Console.WriteLine("3. A file named t3.txt has been created in the Vatamaniuk2 folder (combining the text from t1 and t2).");

            Console.WriteLine("\n4. Detailed information about the created files:");
            PrintFileInfo(fileT1);
            PrintFileInfo(fileT2);
            PrintFileInfo(fileT3);

            string newFileT2 = Path.Combine(folder2, "t2.txt");
            File.Move(fileT2, newFileT2);
            Console.WriteLine("\n5. The file t2.txt has been successfully moved to the Vatamaniuk2 folder.");

            string newFileT1 = Path.Combine(folder2, "t1.txt");
            File.Copy(fileT1, newFileT1);
            Console.WriteLine("6. The file t1.txt has been successfully copied to the Vatamaniuk2 folder.");

            Directory.Move(folder2, folderAll);
            Directory.Delete(folder1, true);
            Console.WriteLine("7. The “Vatamaniuk2” folder has been renamed to “ALL,” and the “Vatamaniuk1” folder has been completely deleted.");

            Console.WriteLine("\n8. Complete information about the files in the current folder ALL:");
            DirectoryInfo dirAll = new DirectoryInfo(folderAll);
            foreach (FileInfo f in dirAll.GetFiles())
            {
                PrintFileInfo(f.FullName);
            }
        }

        static void PrintFileInfo(string filePath)
        {
            FileInfo fi = new FileInfo(filePath);
            if (fi.Exists)
            {
                Console.WriteLine($@"
   - File name: {fi.Name}
     Full path: {fi.FullName}
     Size:      {fi.Length} bytes
     Created:   {fi.CreationTime}");
            }
        }
    }
}