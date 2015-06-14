// Loader for NSBMD files.
// Code adapted from kiwi.ds' NSBMD Model Viewer.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LibNDSFormats.NSBMD
{
	/// <summary>
	/// Loader for NSBMD data.
	/// </summary>
    public static class NsbmdLoader
    {
    	/// <summary>
    	/// Load NSBMD from stream.
    	/// </summary>
    	/// <param name="stream">Stream with NSBMD data.</param>
    	/// <returns>NSBMD object.</returns>
        public static Nsbmd LoadNsbmd(Stream stream)
        {
            return Nsbmd.FromStream(stream);   
        }
    }
}
