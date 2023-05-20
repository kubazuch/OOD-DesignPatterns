using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using ConsoleProject.CLI.Arguments;
using ConsoleProject.CLI.Exception;

namespace ConsoleProject.CLI.Commands
{
    public static class QueueCommand
    {
        //private static readonly EnumArgument SubcommandArg = new(new List<string> { "print", "export", "commit", "dismiss", "load" }, "subcommand", true);
        //private static readonly PathArgument PathArg = new(true);
        //private static readonly EnumArgument FormatArg = new(new List<string> { "XML", "plaintext" }, "subcommand", false);

        public static void PrintCall(CommandDispatcher dispatcher)
        {
            foreach (var cmd in dispatcher.CommandQueue)
            {
                Log.WriteLine(cmd.ToString());
                Log.WriteLine();
            }
        }

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
                var serializer = new XmlSerializer(typeof(CommandQueue));
                serializer.Serialize(writer, dispatcher.CommandQueue);
                Log.WriteLine($"§aCommand queue exported to `§l{path}§a` as XML");
            }

            void SerializePlain()
            {
                using var writer = File.CreateText(path);
                foreach (var cmd in dispatcher.CommandQueue)
                {
                    Log.WriteLine(writer, cmd.ToCommandline());
                    Log.WriteLine(writer);
                }
                Log.WriteLine($"§aCommand queue exported to `§l{path}§a` as plaintext");
            }
        }

        public static void DismissCall(CommandDispatcher dispatcher)
        {
            dispatcher.CommandQueue.Clear();
            Log.WriteLine($"§aCommand queue cleared");
        }

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
                    throw new ArgumentException($"Unknown extension: `§l{ext}§l`");
            }

            void DeserializeXML()
            {
                using var reader = XmlReader.Create(path);
                var serializer = new XmlSerializer(typeof(CommandQueue));
                CommandQueue qu = (CommandQueue)serializer.Deserialize(reader);

                while (qu.Count > 0)
                    dispatcher.CommandQueue.Enqueue(qu.Dequeue());

                Log.WriteLine($"§aCommand queue imported from `§l{path}§l` as XML");
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

                Log.WriteLine($"§aCommand queue imported from `§l{path}§l` as plaintext");
            }
        }
    }
}
