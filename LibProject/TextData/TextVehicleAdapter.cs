namespace BTM.TextData
{
    public abstract class TextVehicleAdapter : IVehicle
    {
        public abstract int Id { get; }
        public abstract object GetValueByName(string name);
    }
}