using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace Octocad_2D
{
    interface Primitive
    {
        /// <summary>
        /// Draws the primitive
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pen"></param>
        void Draw(Graphics g, Pen pen);

        /// <summary>
        /// Draws the primitive as it is for export. 
        /// That is, it translates it so that the upper-left of the screen is at 0,0
        /// (and not the center), and draws the object with a one-pixel brush in B&W.
        /// </summary>
        /// <param name="bitmapData"></param>
        void DrawForExport(ref BitmapData bitmapData);

        void UpdatePrimitive();
        void DrawEndpoints(Graphics g, Pen pen);
        void DrawSelectedEndpoint(int endpoint, Graphics g, Pen pen);

        List<PointF> GetEndpoints();


        /// <summary>
        /// Deep-copies the primitive itself and returns it.
        /// TODO determine if I can completely remove this function if ref counting works correctly.
        /// </summary>
        /// <returns></returns>
        Primitive Copy();
    }
}
