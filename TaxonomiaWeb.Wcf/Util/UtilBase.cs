using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxonomiaWeb.Wcf.Util
{
    public class UtilBase
    {
        public static string tabSpaces(int numberTab)
        {
            string tabs = string.Empty;
            while (numberTab > 0)
            {
                tabs += "\t";
                numberTab += -1;
            }
            return tabs;
        }

    }
}