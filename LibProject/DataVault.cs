using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTM
{
    public class DataVault
    {
        internal readonly Dictionary<int, Line?> Lines = new() { [-1] = null };
        internal readonly Dictionary<int, Stop?> Stops = new() { [-1] = null };
        internal readonly Dictionary<int, Bytebus?> Bytebuses = new() { [-1] = null };
        internal readonly Dictionary<int, Tram?> Trams = new() { [-1] = null };
        internal readonly Dictionary<int, Vehicle?> Vehicles = new() { [-1] = null };
        internal readonly Dictionary<int, Driver?> Drivers = new() { [-1] = null };

        public void Register(Line line)
        {
            if (Lines.ContainsKey(line.NumberDec))
                throw new ArgumentException($"DataVault already contains Line with id {line.NumberDec}");

            Lines[line.NumberDec] = line;
            line.Vault = this;
            line.LineDeleted += Remove;
        }

        public void Remove(Line line)
        {
            if (!Lines.ContainsKey(line.NumberDec))
                return;

            Lines.Remove(line.NumberDec);
            line.LineDeleted -= Remove;
        }

        public void Register(Stop stop)
        {
            if (Stops.ContainsKey(stop.Id))
                throw new ArgumentException($"DataVault already contains Stop with id {stop.Id}");

            Stops[stop.Id] = stop;
            stop.Vault = this;
            stop.StopDeleted += Remove;
        }

        public void Remove(Stop stop)
        {
            if (!Stops.ContainsKey(stop.Id))
                return;

            Stops.Remove(stop.Id);
            stop.StopDeleted -= Remove;
        }

        public void Register(Bytebus bus)
        {
            if (Bytebuses.ContainsKey(bus.Id))
                throw new ArgumentException($"DataVault already contains Bytebus with id {bus.Id}");
            if (Vehicles.ContainsKey(bus.Id))
                throw new ArgumentException($"DataVault already contains Vehicle with id {bus.Id}");

            Bytebuses[bus.Id] = bus;
            Vehicles[bus.Id] = bus;
            bus.Vault = this;
            bus.BytebusDeleted += Remove;
        }

        public void Remove(Bytebus bus)
        {
            if (!Bytebuses.ContainsKey(bus.Id))
                return;

            Bytebuses.Remove(bus.Id);
            Vehicles.Remove(bus.Id);
            bus.BytebusDeleted -= Remove;
        }

        public void Register(Tram tram)
        {
            if (Trams.ContainsKey(tram.Id))
                throw new ArgumentException($"DataVault already contains Tram with id {tram.Id}");
            if (Vehicles.ContainsKey(tram.Id))
                throw new ArgumentException($"DataVault already contains Vehicle with id {tram.Id}");

            Trams[tram.Id] = tram;
            Vehicles[tram.Id] = tram;
            tram.Vault = this;
            tram.TramDeleted += Remove;
        }

        public void Remove(Tram tram)
        {
            if (!Trams.ContainsKey(tram.Id))
                return;

            Trams.Remove(tram.Id);
            Vehicles.Remove(tram.Id);
            tram.TramDeleted -= Remove;
        }

        public void Register(Driver driver)
        {
            if (Drivers.ContainsKey(driver.Id))
                throw new ArgumentException($"DataVault already contains Driver with id {driver.Id}");

            Drivers[driver.Id] = driver;
            driver.Vault = this;
            driver.DriverDeleted += Remove;
        }

        public void Remove(Driver driver)
        {
            if (!Drivers.ContainsKey(driver.Id))
                return;

            Drivers.Remove(driver.Id);
            driver.DriverDeleted -= Remove;
        }
    }
}
