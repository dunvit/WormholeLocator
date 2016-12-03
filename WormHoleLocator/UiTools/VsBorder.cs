using System.Drawing;

namespace WHLocator.UiTools
{
    public static class VsBorder
    {
        public static void DrawBorderToolTip(Graphics fGraph, int Left, int Top, int Width, int Heigth)
        {
            Rectangle mSimpleRect = new Rectangle(Left, Top, Width, Heigth);

            Pen mPen = new System.Drawing.Pen(Color.FromArgb(225, 225, 225), 1);

            fGraph.DrawRectangle(mPen, mSimpleRect);
        }

        public static void DrawBorder(Graphics fGraph, int Left, int Top, int Width, int Heigth)
        {
            Rectangle mSimpleRect = new Rectangle(Left, Top, Width, Heigth);

            Pen mPen = new System.Drawing.Pen(Color.FromArgb(89, 78, 66), 1);

            fGraph.DrawRectangle(mPen, mSimpleRect);


            mSimpleRect = new Rectangle(Left + 1, Top + +1, Width - 2, Heigth - 2);

            mPen = new System.Drawing.Pen(Color.FromArgb(150, 140, 131), 1);
            fGraph.DrawRectangle(mPen, mSimpleRect);
        }

        public static void DrawBorderButton(Graphics fGraph, int Left, int Top, int Width, int Heigth)
        {
            Rectangle mSimpleRect = new Rectangle(Left, Top, Width, Heigth);

            Pen mPen = new System.Drawing.Pen(Color.FromArgb(89, 78, 66), 1);

            fGraph.DrawRectangle(mPen, mSimpleRect);


            mSimpleRect = new Rectangle(Left + 1, Top + +1, Width - 2, Heigth - 2);

            mPen = new System.Drawing.Pen(Color.FromArgb(150, 140, 131), 1);
            fGraph.DrawRectangle(mPen, mSimpleRect);

            mSimpleRect = new Rectangle(Left + 2, Top + 2, Width - 4, Heigth - 4);

            mPen = new System.Drawing.Pen(Color.FromArgb(44, 37, 29), 1);
            fGraph.DrawRectangle(mPen, mSimpleRect);

            mSimpleRect = new Rectangle(Left + 3, Top + 3, Width - 6, Heigth - 6);

            mPen = new System.Drawing.Pen(Color.FromArgb(111, 100, 89), 1);
            fGraph.DrawRectangle(mPen, mSimpleRect);
        }

        public static void DrawBorderFrame(Graphics fGraph, int Left, int Top, int Width, int Heigth)
        {
            var mSimpleRect = new Rectangle(Left, Top, Width, Heigth);

            var mPen = new Pen(Color.FromArgb(89, 78, 66), 1);

            fGraph.DrawRectangle(mPen, mSimpleRect);


            mSimpleRect = new Rectangle(Left + 1, Top + +1, Width - 2, Heigth - 2);

            mPen = new Pen(Color.FromArgb(150, 140, 131), 1);
            fGraph.DrawRectangle(mPen, mSimpleRect);

            mSimpleRect = new Rectangle(Left + 2, Top + 2, Width - 4, Heigth - 4);

            mPen = new Pen(Color.FromArgb(44, 37, 29), 1);
            fGraph.DrawRectangle(mPen, mSimpleRect);

        }

        public static void DrawBorderSmallWindow(Graphics fGraph, int Left, int Top, int Width, int Heigth)
        {
            var mSimpleRect = new Rectangle(Left, Top, Width, Heigth);

            var mPen = new Pen(Color.FromArgb(100, Color.Black), 1);

            fGraph.DrawRectangle(mPen, mSimpleRect);


            mSimpleRect = new Rectangle(Left + 1, Top + +1, Width - 2, Heigth - 2);

            mPen = new Pen(Color.FromArgb(100, Color.FromArgb(80, 80, 80)), 1);
            fGraph.DrawRectangle(mPen, mSimpleRect);

            mSimpleRect = new Rectangle(Left + 2, Top + 2, Width - 4, Heigth - 4);

            mPen = new Pen(Color.FromArgb(100, Color.FromArgb(56, 56, 56)), 1);
            fGraph.DrawRectangle(mPen, mSimpleRect);

        }

        public static void DrawBorderButtonInFocus(Graphics fGraph, int Left, int Top, int Width, int Heigth)
        {
            Rectangle mSimpleRect = new Rectangle(Left, Top, Width, Heigth);

            Pen mPen = new System.Drawing.Pen(Color.FromArgb(89, 78, 66), 1);

            fGraph.DrawRectangle(mPen, mSimpleRect);


            mSimpleRect = new Rectangle(Left + 1, Top + +1, Width - 2, Heigth - 2);

            mPen = new System.Drawing.Pen(Color.FromArgb(200, 200, 75), 1);
            fGraph.DrawRectangle(mPen, mSimpleRect);

            mSimpleRect = new Rectangle(Left + 2, Top + 2, Width - 4, Heigth - 4);

            mPen = new System.Drawing.Pen(Color.FromArgb(44, 37, 29), 1);
            fGraph.DrawRectangle(mPen, mSimpleRect);

            mSimpleRect = new Rectangle(Left + 3, Top + 3, Width - 6, Heigth - 6);

            mPen = new System.Drawing.Pen(Color.FromArgb(111, 100, 89), 1);
            fGraph.DrawRectangle(mPen, mSimpleRect);
        }


        public static void DrawBorderWindow(Graphics fGraph, int Left, int Top, int Width, int Heigth)
        {
            Rectangle mSimpleRect = new Rectangle(Left + 1, Top + 1, Width - 3, Heigth - 3);

            Pen mPen = new System.Drawing.Pen(Color.FromArgb(44, 37, 29), 3);

            fGraph.DrawRectangle(mPen, mSimpleRect);



            mSimpleRect = new Rectangle(Left + 4, Top + 4, Width - 8, Heigth - 8);

            mPen = new System.Drawing.Pen(Color.FromArgb(111, 100, 89), 2);
            fGraph.DrawRectangle(mPen, mSimpleRect);


            mSimpleRect = new Rectangle(Left + 5, Top + 5, Width - 11, Heigth - 11);

            mPen = new System.Drawing.Pen(Color.FromArgb(44, 37, 29), 1);
            fGraph.DrawRectangle(mPen, mSimpleRect);

        }
    }
}
