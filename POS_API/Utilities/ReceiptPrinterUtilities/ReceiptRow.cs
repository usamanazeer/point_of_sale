using System.Collections.Generic;
using System.Drawing;


namespace POS_API.Utilities.ReceiptPrinterUtilities
{
    public class ReceiptRow
    {
        public Image Image { get; set; }
        public int? Width { get; }
        public int? Height { get; }
        public int PaddingTop { get; }
        public int PaddingBottum { get; }
        public int PaddingLeft { get; }
        public int PaddingRight { get; }
        private bool? alignmentBIT { get; set; }
        public bool? AlignmentBIT =>  alignmentBIT;
        public Brush Brush { get; }
        public Font Font { get; set; }


        public List<string> RowTexts;
        public List<int?> RowTextLeftPaddings;



        private string text;
        public string Text => text;




        //private string text1;
        //public string Text1 => text1;
        //private string text2;
        //public string Text2 => text2;
        public bool IsMultiTextRow => (RowTexts != null);

        
        public bool IsImageRow;

        public ReceiptRow(Image image,  int? width = null, int? height = null, bool? alignmentBit = false, int paddingTop = 0, int paddingBottum = 0, int paddingLeft = 0, int paddingRight = 0) {
            Image = image;
            Width = width;
            Height = height;
            PaddingTop = paddingTop;
            PaddingBottum = paddingBottum;
            PaddingLeft = paddingLeft;
            PaddingRight = paddingRight;
            alignmentBIT = alignmentBit;
            IsImageRow = true;
        }
        public ReceiptRow(string _text, int? width = null, int? height = null, bool? _alignmentBIT = null, Font font = null, int paddingTop = 0, int paddingBottum = 0, int paddingLeft = 0, int paddingRight = 0)
        {
            text = _text;
            alignmentBIT = _alignmentBIT;
            //Brush = brush;
            Font = font;
            PaddingTop = paddingTop;
            PaddingBottum = paddingBottum;
            PaddingLeft = paddingLeft;
            PaddingRight = paddingRight;
        }
        //public ReceiptRow(string _text1, string _text2, int? width = null, int? height = null, bool? _alignmentBIT = null,  Font font = null, int paddingTop = 0, int paddingBottum = 0, int paddingLeft = 0, int paddingRight = 0)
        //{
        //    text1 = _text1;
        //    text2 = _text2;
        //    alignmentBIT = _alignmentBIT;
        //    //Brush = brush;
        //    Font = font;
        //    PaddingTop = paddingTop;
        //    PaddingBottum = paddingBottum;
        //    PaddingLeft = paddingLeft;
        //    PaddingRight = paddingRight;
        //}
        public ReceiptRow(List<string> texts, List<int?> textsLeftSpacings, int? width = null, int? height = null, bool? _alignmentBIT = null, Font font = null, int paddingTop = 0, int paddingBottum = 0)
        {

            RowTexts = texts;
            RowTextLeftPaddings = textsLeftSpacings;
            alignmentBIT = _alignmentBIT;
            //Brush = brush;
            Font = font;
            PaddingTop = paddingTop;
            PaddingBottum = paddingBottum;
        }
        public ReceiptRow AlignCentre() 
        {
            alignmentBIT = null;
            return this;
        }
        public ReceiptRow AlignLeft()
        {
            alignmentBIT = false;
            return this;
        }
        public ReceiptRow AlignRight()
        {
            alignmentBIT = true;
            return this;
        }
        public bool? GetAlignmentBit() {
            return alignmentBIT;
        }
    }
}