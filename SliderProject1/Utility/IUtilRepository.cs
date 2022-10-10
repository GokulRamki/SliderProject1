using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.IO;

namespace SliderProject1.Utility
{
    public interface IUtilRepository : IDisposable
    {
        Bitmap ResizeImage(Stream streamImage, int resizeWidth, int resizeHeight);
    }
}