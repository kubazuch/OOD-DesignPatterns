using System;
using System.Collections.Generic;
using BTM;
using BTM.Collections;
using BTM.Data;
using BTM.TextData;
using BTM.TupleStackData;

namespace ConsoleProject
{
    public class DataManager
    {
        public Dictionary<string, ICollection> Mapping { get; }

        public Vector<ILine> Lines { get; } = new();
        public Vector<IStop> Stops { get; } = new();
        public Vector<IBytebus> Bytebuses { get; } = new();
        public Vector<ITram> Trams { get; } = new();
        public Vector<IDriver> Drivers { get; } = new();

        public DataManager(DataRepresentation representation)
        {
            Mapping = new Dictionary<string, ICollection>
            {
                ["line"] = Lines,
                ["stop"] = Stops,
                ["bytebus"] = Bytebuses,
                ["tram"] = Trams,
                ["driver"] = Drivers
            };

            switch (representation)
            {
                case DataRepresentation.BASE:
                    PrepareBaseData();
                    break;
                case DataRepresentation.TEXT:
                    PrepareTextData();
                    break;
                case DataRepresentation.TUPLE_STACK:
                    PrepareTupleStackData();
                    break;
            }
        }

        public void PrintData()
        {
            Console.WriteLine("Line - number(hex + decimal), commonName, stops, vehicles");
            Algorithms.ForEach(Lines.GetForwardIterator(), Console.WriteLine);

            Console.WriteLine("\nStop - id, name, type, lines(decimal)");
            Algorithms.ForEach(Stops.GetForwardIterator(), Console.WriteLine);

            Console.WriteLine("\nBytebus - id, lines, engineClass");
            Algorithms.ForEach(Bytebuses.GetForwardIterator(), Console.WriteLine);

            Console.WriteLine("\nTram - id, carsNumber, line");
            Algorithms.ForEach(Trams.GetForwardIterator(), Console.WriteLine);

            Console.WriteLine("\nDriver - name, surname, seniority, vehicles");
            Algorithms.ForEach(Drivers.GetForwardIterator(), Console.WriteLine);
        }

        private void PrepareBaseData()
        {
            List<Line> lines = new()
            {
                new Line("10", 16, "SIMD"),
                new Line("17", 23, "Isengard-Mordor"),
                new Line("E", 14, "Museum of Plant")
            };
            lines.ForEach(Lines.Add);

            List<Stop> stops = new()
            {
                new Stop(1, "SPIR-V", "bus", lines[0]),
                new Stop(2, "GLSL", "tram", lines[0]),
                new Stop(3, "HLSL", "other", lines[0]),
                new Stop(4, "Dol Guldur", "bus", lines[1]),
                new Stop(5, "Amon Hen", "bus", lines[1]),
                new Stop(6, "Gondolin", "bus", lines[1]),
                new Stop(7, "Bitazon", "tram", lines[1], lines[2]),
                new Stop(8, "Bytecroft", "bus", lines[0], lines[2]),
                new Stop(9, "Maple", "other", lines[2])
            };
            stops.ForEach(Stops.Add);

            List<Bytebus> bytebuses = new ()
            {
                new Bytebus(11, "Byte5", lines[0], lines[1]),
                new Bytebus(12, "bisel20", lines[0]),
                new Bytebus(13, "bisel20", lines[0]),
                new Bytebus(14, "gibgaz", lines[1], lines[2]),
                new Bytebus(15, "gibgaz", lines[1])
            };
            bytebuses.ForEach(Bytebuses.Add);

            List<Tram> trams = new()
            {
                new Tram(21, 1, lines[2]),
                new Tram(22, 2, lines[2]),
                new Tram(23, 6, lines[2])
            };
            trams.ForEach(Trams.Add);

            List<Driver> drivers = new()
            {
                new Driver("Tomas", "Chairman", 20, bytebuses[0], trams[0], bytebuses[4]),
                new Driver("Tomas", "Thetank", 4, bytebuses[1], bytebuses[2], bytebuses[3]),
                new Driver("Oru", "Bii", 55, trams[1], trams[2])
            };
            drivers.ForEach(Drivers.Add);
        }

        private void PrepareTextData()
        {
            List<TextLine> lines = new()
            {
                new TextLine("10", 16, "SIMD", new[] { 1, 2, 3, 8 }, new[] { 11, 12, 13 }),
                new TextLine("17", 23, "Isengard-Mordor", new[] { 4, 5, 6, 7 }, new[] { 11, 14, 15 }),
                new TextLine("E", 14, "Museum of Plant", new[] { 7, 8, 9 }, new[] { 14, 21, 22, 23 })
            };
            lines.ForEach(x => Lines.Add(new TextLineAdapter(x)));

            List<TextStop> stops = new()
            {
                new TextStop(1, "SPIR-V", "bus", 16),
                new TextStop(2, "GLSL", "tram", 16),
                new TextStop(3, "HLSL", "other", 16),
                new TextStop(4, "Dol Guldur", "bus", 23),
                new TextStop(5, "Amon Hen", "bus", 23),
                new TextStop(6, "Gondolin", "bus", 23),
                new TextStop(7, "Bitazon", "tram", 23, 14),
                new TextStop(8, "Bytecroft", "bus", 16, 14),
                new TextStop(9, "Maple", "other", 14)
            };
            stops.ForEach(x => Stops.Add(new TextStopAdapter(x)));

            List<TextBytebus> bytebuses = new()
            {
                new TextBytebus(11, "Byte5", 16, 23),
                new TextBytebus(12, "bisel20", 16),
                new TextBytebus(13, "bisel20", 16),
                new TextBytebus(14, "gibgaz", 23, 14),
                new TextBytebus(15, "gibgaz", 23)
            };
            bytebuses.ForEach(x => Bytebuses.Add(new TextBytebusAdapter(x)));

            List<TextTram> trams = new()
            {
                new TextTram(21, 1, 14),
                new TextTram(22, 2, 14),
                new TextTram(23, 6, 14)
            };
            trams.ForEach(x => Trams.Add(new TextTramAdapter(x)));

            List<TextDriver> drivers = new()
            {
                new TextDriver("Tomas", "Chairman", 20, 11, 21, 15),
                new TextDriver("Tomas", "Thetank", 4, 12, 13, 14),
                new TextDriver("Oru", "Bii", 55, 22, 23)
            };
            drivers.ForEach(x => Drivers.Add(new TextDriverAdapter(x)));
        }

        private void PrepareTupleStackData()
        {
            List<TupleStackLine> lines = new()
            {
                new TupleStackLine("10", 16, "SIMD", new [] { 1, 2, 3, 8 }, new [] { 11, 12, 13 }),
                new TupleStackLine("17", 23, "Isengard-Mordor", new [] { 4, 5, 6, 7 }, new [] { 11, 14, 15 }),
                new TupleStackLine("E", 14, "Museum of Plant", new [] { 7, 8, 9 }, new [] { 14, 21, 22, 23 })
            };
            lines.ForEach(x => Lines.Add(new TupleStackLineAdapter(x)));

            List<TupleStackStop> stops = new()
            {
                new TupleStackStop(1, "SPIR-V", "bus", 16),
                new TupleStackStop(2, "GLSL", "tram", 16),
                new TupleStackStop(3, "HLSL", "other", 16),
                new TupleStackStop(4, "Dol Guldur", "bus", 23),
                new TupleStackStop(5, "Amon Hen", "bus", 23),
                new TupleStackStop(6, "Gondolin", "bus", 23),
                new TupleStackStop(7, "Bitazon", "tram", 23, 14),
                new TupleStackStop(8, "Bytecroft", "bus", 16, 14),
                new TupleStackStop(9, "Maple", "other", 14)
            };
            stops.ForEach(x => Stops.Add(new TupleStackStopAdapter(x)));

            List<TupleStackBytebus> bytebuses = new()
            {
                new TupleStackBytebus(11, "Byte5", 16, 23),
                new TupleStackBytebus(12, "bisel20", 16),
                new TupleStackBytebus(13, "bisel20", 16),
                new TupleStackBytebus(14, "gibgaz", 23, 14),
                new TupleStackBytebus(15, "gibgaz", 23)
            };
            bytebuses.ForEach(x => Bytebuses.Add(new TupleStackBytebusAdapter(x)));

            List<TupleStackTram> trams = new()
            {
                new TupleStackTram(21, 1, 14),
                new TupleStackTram(22, 2, 14),
                new TupleStackTram(23, 6, 14)
            };
            trams.ForEach(x => Trams.Add(new TupleStackTramAdapter(x)));

            List<TupleStackDriver> drivers = new()
            {
                new TupleStackDriver("Tomas", "Chairman", 20, 11, 21, 15),
                new TupleStackDriver("Tomas", "Thetank", 4, 12, 13, 14),
                new TupleStackDriver("Oru", "Bii", 55, 22, 23)
            };
            drivers.ForEach(x => Drivers.Add(new TupleStackDriverAdapter(x)));
        }
    }
    public enum DataRepresentation
    {
        BASE,
        TEXT,
        TUPLE_STACK
    }
}
