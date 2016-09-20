using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leaves_FAT_Management.Core
{
    class FATCount
    {
        /// <summary>
        /// Set or get grand FAT total
        /// </summary>
        public double GrandTotal {set; get;}
        public double TotalMaladie { set; get; }
        public double TotalCongesPayes { set; get; }
        public double TotalCongesExceptionnels { set; get; }
        public double TotalFerie { set; get; }
        // ABO 22/03/2016
        public double TotalFormation { set; get; }
    }
}
