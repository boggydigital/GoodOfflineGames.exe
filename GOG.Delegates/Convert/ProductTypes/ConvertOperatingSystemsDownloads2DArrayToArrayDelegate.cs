﻿using System;
using System.Collections.Generic;
using System.Text;

using Interfaces.Delegates.Convert;

using GOG.Models;

namespace GOG.Delegates.Convert.ProductTypes
{
    public class ConvertOperatingSystemsDownloads2DArrayToArrayDelegate : 
        IConvertDelegate<OperatingSystemsDownloads[][], OperatingSystemsDownloads[]> 
    {
        public OperatingSystemsDownloads[] Convert(OperatingSystemsDownloads[][] data)
        {
            if (data == null) return null;
            var projection = new List<OperatingSystemsDownloads>();

            for (var xx=0; xx<data.Length; xx++)
            {
                if (data[xx] == null) continue;
                for (var yy=0;yy<data[xx].Length; yy++)
                {
                    if (data[xx][yy] == null) continue;
                    projection.Add(data[xx][yy]);
                }
            }
            return projection.ToArray();
        }
    }
}
