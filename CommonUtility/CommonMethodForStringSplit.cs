using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtility
{
    public static class CommonMethodForStringSplit
    {
        public static string[] SplitString(string FullString, char SplitChar)
        {
            string[] OutputArray = null;
            try
            {
                OutputArray = FullString.Trim().Split(new char[] { SplitChar });
            }
            catch (Exception ex)
            {
            }


            return OutputArray;
        }

    }
}
