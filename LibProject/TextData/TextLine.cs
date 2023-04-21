using System.Collections.Generic;

namespace BTM.TextData
{
    public class TextLine : TextRepresentation
    {
        public override string TextRepr { get; }

        public TextLine(string numberHex, int numberDec, string commonName, int[] stops, int[] vehicles)
        {
            TextRepr = $"{numberHex}({numberDec})`{commonName}`@{string.Join(',', stops)}!{string.Join(',', vehicles)}";
        }
    }
}