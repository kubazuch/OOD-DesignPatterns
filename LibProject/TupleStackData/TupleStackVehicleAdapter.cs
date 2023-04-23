namespace BTM.TupleStackData
{
    public abstract class TupleStackVehicleAdapter : IVehicle
    {
        public abstract int Id { get; }
        public abstract object GetValueByName(string name);
    }
}