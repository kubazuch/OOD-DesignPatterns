using BTM;
using BTM.Collections;
//using BTM.TextData;
using System;
using System.Collections.Generic;
using BTM.BaseData;
using BTM.TupleStackData;

namespace ConsoleProject
{
    public class DataManager
    {
        public readonly DataVault Vault = new();

        public Dictionary<string, ICollection> Mapping { get; }

        public Vector<Line> Lines { get; } = new();
        public Vector<Stop> Stops { get; } = new();
        public Vector<Bytebus> Bytebuses { get; } = new();
        public Vector<Tram> Trams { get; } = new();
        public Vector<Driver> Drivers { get; } = new();

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
                case DataRepresentation.Base:
                    PrepareBaseData();
                    break;
                //case DataRepresentation.Text:
                //    PrepareTextData();
                //    break;
                case DataRepresentation.TupleStack:
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
                new BaseLine("10", 16, "SIMD"),
                new BaseLine("17", 23, "Isengard-Mordor"),
                new BaseLine("E", 14, "Museum of Plant")
            };
            lines.ForEach(Lines.Add);
            lines.ForEach(Vault.Register);

            List<BaseStop> stops = new()
            {
                new BaseStop(1, "SPIR-V", "bus", lines[0]),
                new BaseStop(2, "GLSL", "tram", lines[0]),
                new BaseStop(3, "HLSL", "other", lines[0]),
                new BaseStop(4, "Dol Guldur", "bus", lines[1]),
                new BaseStop(5, "Amon Hen", "bus", lines[1]),
                new BaseStop(6, "Gondolin", "bus", lines[1]),
                new BaseStop(7, "Bitazon", "tram", lines[1], lines[2]),
                new BaseStop(8, "Bytecroft", "bus", lines[0], lines[2]),
                new BaseStop(9, "Maple", "other", lines[2])
            };
            stops.ForEach(Stops.Add);
            stops.ForEach(Vault.Register);

            List<BaseBytebus> bytebuses = new()
            {
                new BaseBytebus(11, "Byte5", lines[0], lines[1]),
                new BaseBytebus(12, "bisel20", lines[0]),
                new BaseBytebus(13, "bisel20", lines[0]),
                new BaseBytebus(14, "gibgaz", lines[1], lines[2]),
                new BaseBytebus(15, "gibgaz", lines[1])
            };
            bytebuses.ForEach(Bytebuses.Add);
            bytebuses.ForEach(Vault.Register);

            List<BaseTram> trams = new()
            {
                new BaseTram(21, 1, lines[2]),
                new BaseTram(22, 2, lines[2]),
                new BaseTram(23, 6, lines[2])
            };
            trams.ForEach(Trams.Add);
            trams.ForEach(Vault.Register);

            List<BaseDriver> drivers = new()
            {
                new BaseDriver(1, "Tomas", "Chairman", 20, bytebuses[0], trams[0], bytebuses[4]),
                new BaseDriver(2, "Tomas", "Thetank", 4, bytebuses[1], bytebuses[2], bytebuses[3]),
                new BaseDriver(3, "Oru", "Bii", 55, trams[1], trams[2])
            };
            drivers.ForEach(Drivers.Add);
            drivers.ForEach(Vault.Register);
        }

        private void PrepareTextData()
        {
            //List<TextLine> lines = new()
            //{
            //    new TextLine("10", 16, "SIMD", new[] { 1, 2, 3, 8 }, new[] { 11, 12, 13 }),
            //    new TextLine("17", 23, "Isengard-Mordor", new[] { 4, 5, 6, 7 }, new[] { 11, 14, 15 }),
            //    new TextLine("E", 14, "Museum of Plant", new[] { 7, 8, 9 }, new[] { 14, 21, 22, 23 })
            //};
            //lines.ForEach(x => Lines.Add(new TextLineAdapter(x)));

            //List<TextStop> stops = new()
            //{
            //    new TextStop(1, "SPIR-V", "bus", 16),
            //    new TextStop(2, "GLSL", "tram", 16),
            //    new TextStop(3, "HLSL", "other", 16),
            //    new TextStop(4, "Dol Guldur", "bus", 23),
            //    new TextStop(5, "Amon Hen", "bus", 23),
            //    new TextStop(6, "Gondolin", "bus", 23),
            //    new TextStop(7, "Bitazon", "tram", 23, 14),
            //    new TextStop(8, "Bytecroft", "bus", 16, 14),
            //    new TextStop(9, "Maple", "other", 14)
            //};
            //stops.ForEach(x => Stops.Add(new TextStopAdapter(x)));

            //List<TextBytebus> bytebuses = new()
            //{
            //    new TextBytebus(11, "Byte5", 16, 23),
            //    new TextBytebus(12, "bisel20", 16),
            //    new TextBytebus(13, "bisel20", 16),
            //    new TextBytebus(14, "gibgaz", 23, 14),
            //    new TextBytebus(15, "gibgaz", 23)
            //};
            //bytebuses.ForEach(x => Bytebuses.Add(new TextBytebusAdapter(x)));

            //List<TextTram> trams = new()
            //{
            //    new TextTram(21, 1, 14),
            //    new TextTram(22, 2, 14),
            //    new TextTram(23, 6, 14)
            //};
            //trams.ForEach(x => Trams.Add(new TextTramAdapter(x)));

            //List<TextDriver> drivers = new()
            //{
            //    new TextDriver("Tomas", "Chairman", 20, 11, 21, 15),
            //    new TextDriver("Tomas", "Thetank", 4, 12, 13, 14),
            //    new TextDriver("Oru", "Bii", 55, 22, 23)
            //};
            //drivers.ForEach(x => Drivers.Add(new TextDriverAdapter(x)));
        }

        private void PrepareTupleStackData()
        {
            List<Line> lines = new()
            {
                new TupleStackLineAdapter(new TupleStackLine("10", 16, "SIMD", new[] { 1, 2, 3, 8 }, new[] { 11, 12, 13 })),
                new TupleStackLineAdapter(new TupleStackLine("17", 23, "Isengard-Mordor", new[] { 4, 5, 6, 7 }, new[] { 11, 14, 15 })),
                new TupleStackLineAdapter(new TupleStackLine("E", 14, "Museum of Plant", new[] { 7, 8, 9 }, new[] { 14, 21, 22, 23 }))
            };
            lines.ForEach(Lines.Add);
            lines.ForEach(Vault.Register);

            List<Stop> stops = new()
            {
                new TupleStackStopAdapter(new TupleStackStop(1, "SPIR-V", "bus", 16)),
                new TupleStackStopAdapter(new TupleStackStop(2, "GLSL", "tram", 16)),
                new TupleStackStopAdapter(new TupleStackStop(3, "HLSL", "other", 16)),
                new TupleStackStopAdapter(new TupleStackStop(4, "Dol Guldur", "bus", 23)),
                new TupleStackStopAdapter(new TupleStackStop(5, "Amon Hen", "bus", 23)),
                new TupleStackStopAdapter(new TupleStackStop(6, "Gondolin", "bus", 23)),
                new TupleStackStopAdapter(new TupleStackStop(7, "Bitazon", "tram", 23, 14)),
                new TupleStackStopAdapter(new TupleStackStop(8, "Bytecroft", "bus", 16, 14)),
                new TupleStackStopAdapter(new TupleStackStop(9, "Maple", "other", 14))
            };
            stops.ForEach(Stops.Add);
            stops.ForEach(Vault.Register);

            List<Bytebus> bytebuses = new()
            {
                new TupleStackBytebusAdapter(new TupleStackBytebus(11, "Byte5", 16, 23)),
                new TupleStackBytebusAdapter(new TupleStackBytebus(12, "bisel20", 16)),
                new TupleStackBytebusAdapter(new TupleStackBytebus(13, "bisel20", 16)),
                new TupleStackBytebusAdapter(new TupleStackBytebus(14, "gibgaz", 23, 14)),
                new TupleStackBytebusAdapter(new TupleStackBytebus(15, "gibgaz", 23))
            };
            bytebuses.ForEach(Bytebuses.Add);
            bytebuses.ForEach(Vault.Register);

            List<Tram> trams = new()
            {
                new TupleStackTramAdapter(new TupleStackTram(21, 1, 14)),
                new TupleStackTramAdapter(new TupleStackTram(22, 2, 14)),
                new TupleStackTramAdapter(new TupleStackTram(23, 6, 14))
            };
            trams.ForEach(Trams.Add);
            trams.ForEach(Vault.Register);

            List<Driver> drivers = new()
            {
                new TupleStackDriverAdapter(new TupleStackDriver(1, "Tomas", "Chairman", 20, 11, 21, 15)),
                new TupleStackDriverAdapter(new TupleStackDriver(2, "Tomas", "Thetank", 4, 12, 13, 14)),
                new TupleStackDriverAdapter(new TupleStackDriver(3, "Oru", "Bii", 55, 22, 23))
            };
            drivers.ForEach(Drivers.Add);
            drivers.ForEach(Vault.Register);
        }
    }
    public enum DataRepresentation
    {
        Base,
        //Text,
        TupleStack
    }
}
