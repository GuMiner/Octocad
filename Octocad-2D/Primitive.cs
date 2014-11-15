using System;
using System.Collections.Generic;
using System.Drawing;

namespace Octocad_2D
{
    interface Primitive
    {
        /// <summary>
        /// Draws the primitive
        /// </summary>
        /// <param name="g"></param>
        /// <param name="p"></param>
        void Draw(Graphics g, Pen p);

        /// <summary>
        /// Deep-copies the primitive itself and returns it.
        /// TODO determine if I can completely remove this function if ref counting works correctly.
        /// </summary>
        /// <returns></returns>
        Primitive Copy();
    }
}
