using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ITunEsTooL
{
    class ImageOperation
    {

        /// <summary>
        /// リソースをセット
        /// </summary>
        /// <param name="BitmapName"></param>
        /// <returns></returns>
        public Bitmap SetResources(string BitmapName)
        {
            try
            {
                System.Reflection.Assembly assembly;

                assembly = System.Reflection.Assembly.GetExecutingAssembly();

                System.Resources.ResourceManager rm = new System.Resources.ResourceManager
                ("itunesTool.Properties.Resources", assembly);

                Bitmap bmp = (Bitmap)rm.GetObject(BitmapName);
                return bmp;
            }
            catch (System.Exception)
            {
                return null;
            }
        }
    }
}
