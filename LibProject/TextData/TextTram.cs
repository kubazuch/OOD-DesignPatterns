namespace BTM.TextData
{
    public class TextTram : TextVehicle
    {
        public override string TextRepr { get; }

        public TextTram(int id, int carsNumber, int line)
        {
            TextRepr = $"#{id}({carsNumber}){line}";
        }
    }
}