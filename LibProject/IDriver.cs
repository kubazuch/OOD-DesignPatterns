﻿using System.Collections.Generic;

namespace BTM
{
    public interface IDriver
    {
        public List<IVehicle> Vehicles { get; }
        public string Name { get; }
        public string Surname { get; }
        public int Seniority { get; }
    }
}