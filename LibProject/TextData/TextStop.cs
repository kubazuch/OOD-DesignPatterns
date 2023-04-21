namespace BTM.TextData
{
    public class TextStop : TextRepresentation
    {
        public override string TextRepr { get; }

        public TextStop(int id, string name, string type, params int[] lines)
        {
            TextRepr = $"#{id}({string.Join(',', lines)}){name}/{type}";
        }
    }
}