using System;
using System.Drawing.Text;
using System.Runtime.InteropServices;

namespace WHLocator
{
    public static class Global
    {
        public static PrivateFontCollection Fonts = new PrivateFontCollection();

        static Global()
        {
            InitCustomLabelFont();
        }

        private static void InitCustomLabelFont()
        {
            //Select your font from the resources.
            int fontLength = Properties.Resources.eve.Length;

            // create a buffer to read in to
            byte[] fontdata = Properties.Resources.eve;

            // create an unsafe memory block for the font data
            IntPtr data = Marshal.AllocCoTaskMem(fontLength);

            // copy the bytes to the unsafe memory block
            Marshal.Copy(fontdata, 0, data, fontLength);

            // pass the font to the font collection
            Fonts.AddMemoryFont(data, fontLength);

            // free up the unsafe memory
            Marshal.FreeCoTaskMem(data);
        }
    }
}
