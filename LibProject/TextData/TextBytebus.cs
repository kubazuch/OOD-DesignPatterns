namespace BTM.TextData
{
    public class TextBytebus : TextVehicle
    {
        public override string TextRepr { get; }

        public TextBytebus(int id, string engineClass, params int[] lines)
        {
            TextRepr = $"#{id}^{engineClass}*{string.Join(',', lines)}";
        }
    }
}