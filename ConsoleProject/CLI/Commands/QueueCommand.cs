using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace ConsoleProject.CLI.Commands
{
    public static class QueueCommand
    {

        public static void PrintCall(CommandDispatcher dispatcher)
        {
#if !HISTORY
            foreach (var cmd in dispatcher.CommandQueue)
#else
            foreach (var cmd in dispatcher.CommandHistory)
#endif
            {
                Log.WriteLine(cmd.ToString());
                Log.WriteLine();
            }
        }

#if !HISTORY
        public static void CommitCall(CommandDispatcher dispatcher)
        {
            var i = 1;
            while (dispatcher.CommandQueue.TryDequeue(out var cmd))
            {
                try
                {
                    Log.WriteLine($"§5Command #{i++} ({cmd.Name}):");
                    cmd.Execute();
                }
                catch (ArgumentException ex)
                {
                    Log.HandleException(ex);
                }

                Log.WriteLine();
            }
        }

#endif
        public static void ExportCall(CommandDispatcher dispatcher, List<object?> args)
        {
            var path = (string) args[0]!;
            var format = (string?) args[1] ?? "XML";

            switch (format)
            {
                case "XML":
                    SerializeXML();
                    break;
                case "plaintext":
                    SerializePlain();
                    break;
            }

            void SerializeXML()
            {
                using var writer = XmlWriter.Create(path, new XmlWriterSettings { Indent = true, IndentChars = "    " });
#if !HISTORY
                var serializer = new XmlSerializer(typeof(CommandQueue));
                serializer.Serialize(writer, dispatcher.CommandQueue);
#else
                var serializer = new XmlSerializer(typeof(CommandHistory));
                serializer.Serialize(writer, dispatcher.CommandHistory);
#endif
                Log.WriteLine($"§aCommand queue exported to `{path}` as XML");
            }

            void SerializePlain()
            {
                using var writer = File.CreateText(path);
#if !HISTORY
                foreach (var cmd in dispatcher.CommandQueue)
#else
                foreach (var cmd in dispatcher.CommandHistory.Reverse())
#endif
                {
                    Log.WriteLine(writer, cmd.ToCommandline());
                    Log.WriteLine(writer);
                }
                Log.WriteLine($"§aCommand queue exported to `{path}` as plaintext");
            }
        }

#if !HISTORY
        public static void DismissCall(CommandDispatcher dispatcher)
        {
            dispatcher.CommandQueue.Clear();
            Log.WriteLine($"§aCommand queue cleared");
        }

#endif
        public static void ImportCall(CommandDispatcher dispatcher, List<object?> args)
        {
            var path = (string)args[0]!;
            var ext = Path.GetExtension(path).ToLower();

            switch (ext)
            {
                case ".xml":
                    DeserializeXML();
                    break;
                case ".txt":
                    DeserializePlain();
                    break;
                default:
                    throw new ArgumentException($"Unknown extension: `{ext}`");
            }

            void DeserializeXML()
            {
                using var reader = XmlReader.Create(path);
#if !HISTORY
                var serializer = new XmlSerializer(typeof(CommandQueue));
                var qu = (CommandQueue)serializer.Deserialize(reader);

                while (qu.Count > 0)
                    dispatcher.CommandQueue.Enqueue(qu.Dequeue());
#else
                var serializer = new XmlSerializer(typeof(CommandHistory));
                var qu = (CommandHistory)serializer.Deserialize(reader);

                qu.Reverse().ToList().ForEach(x => dispatcher.CommandHistory.Push(x));
#endif

                Log.WriteLine($"§aCommand queue imported from `{path}` as XML");
            }

            void DeserializePlain()
            {
                using var reader = File.OpenText(path);
                string input;

                while ((input = reader.ReadLine()) != null)
                {
                    try
                    {
                        dispatcher.Parse(input.Trim(), reader, false);
                    }
                    catch (InvalidOperationException ex)
                    {
                        Log.HandleException(ex);
                    }
                    catch (ArgumentException ex)
                    {
                        Log.HandleException(ex);
                    }
                }

                Log.WriteLine($"§aCommand queue imported from `{path}` as plaintext");
            }
        }
    }
}
