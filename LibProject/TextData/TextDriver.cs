namespace BTM.TextData
{
    public class TextDriver : TextRepresentation
    {
        public override string TextRepr { get; }

        public TextDriver(string name, string surname, int seniority, params int[] vehicles)
        {
            TextRepr = $"{name} {surname}({seniority})@{string.Join(',', vehicles)}";
        }
    }
}