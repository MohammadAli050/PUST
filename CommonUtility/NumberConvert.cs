using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtility
{
    public class NumberConvert
    {
        //public static String ConvertNumber(decimal number)
        //{
        //    double tempdbl = Convert.ToDouble(number);
        //    string amount = AmountInWords(tempdbl);
        //    return amount;
        //}
  
        public static string AmountInWords(decimal amount)
        {
            var n = (int)amount;

            if (n == 0)
                return "";
            else if (n > 0 && n <= 99)
            {
                var arr = new string[] { "এক", "দুই", "তিন", "চার", "পাঁচ", "ছয়", "সাত", "আট", "নয়", "দশ", "এগার", "বারো", "তের", "চৌদ্দ", "পনের", "ষোল", "সতের", "আঠার", "ঊনিশ", "বিশ", "একুশ", "বাইশ", "তেইশ", "চব্বিশ", "পঁচিশ", "ছাব্বিশ", "সাতাশ", "আটাশ", "ঊনত্রিশ", "ত্রিশ", "একত্রিশ", "বত্রিশ", "তেত্রিশ", "চৌত্রিশ", "পঁয়ত্রিশ", "ছত্রিশ", "সাঁইত্রিশ", "আটত্রিশ", "ঊনচল্লিশ", "চল্লিশ", "একচল্লিশ", "বিয়াল্লিশ", "তেতাল্লিশ", "চুয়াল্লিশ", "পঁয়তাল্লিশ", "ছেচল্লিশ", "সাতচল্লিশ", "আটচল্লিশ", "ঊনপঞ্চাশ", "পঞ্চাশ", "একান্ন", "বায়ান্ন", "তিপ্পান্ন", "চুয়ান্ন", "পঞ্চান্ন", "ছাপ্পান্ন", "সাতান্ন", "আটান্ন", "ঊনষাট", "ষাট", "একষট্টি", "বাষট্টি", "তেষট্টি", "চৌষট্টি", "পঁয়ষট্টি", "ছেষট্টি", " সাতষট্টি", "আটষট্টি", "ঊনসত্তর ", "সত্তর", "একাত্তর ", "বাহাত্তর", "তেহাত্তর", "চুয়াত্তর", "পঁচাত্তর", "ছিয়াত্তর", "সাতাত্তর", "আটাত্তর", "ঊনআশি", "আশি", "একাশি", "বিরাশি", "তিরাশি", "চুরাশি", "পঁচাশি", "ছিয়াশি", "সাতাশি", "আটাশি", "ঊননব্বই", "নব্বই", "একানব্বই", "বিরানব্বই", "তিরানব্বই", "চুরানব্বই", "পঁচানব্বই ", "ছিয়ানব্বই ", "সাতানব্বই", "আটানব্বই", "নিরানব্বই" };
                return arr[n - 1];
            }
            //else if (n >= 100 && n <= 199)
            //{
            //    return AmountInWords(n / 100) + "একশত " + AmountInWords(n % 100);
            //}

            else if (n >= 100 && n <= 999)
            {
                return AmountInWords(n / 100) + "শত " + AmountInWords(n % 100);
            }
            //else if (n >= 1000 && n <= 1999)
            //{
            //    return "এক হাজার " + AmountInWords(n % 1000);
            //}
            else if (n >= 1000 && n <= 99999)
            {
                return AmountInWords(n / 1000) + " হাজার " + AmountInWords(n % 1000);
            }
            //else if (n >= 100000 && n <= 199999)
            //{
            //    return "এক লাখ " + AmountInWords(n % 100000);
            //}
            else if (n >= 100000 && n <= 9999999)
            {
                return AmountInWords(n / 100000) + " লক্ষ " + AmountInWords(n % 100000);
            }
            //else if (n >= 10000000 && n <= 19999999)
            //{
            //    return "এক কোটি " + AmountInWords(n % 10000000);
            //}
            else
            {
                return AmountInWords(n / 10000000) + " কোটি " + AmountInWords(n % 10000000);
            }
        }

        public static string BanglaToEnglish(string bangla)
        {
            string english = "";
            if (!string.IsNullOrEmpty(bangla))
            {
                english = bangla.Replace("০", "0").Replace("১", "1").Replace("২", "2").Replace("৩", "3").Replace("৪", "4").Replace("৫", "5").Replace("৬", "6").Replace("৭", "7").Replace("৮", "8").Replace("৯", "9");
            }
            return english;
        }

        public static string EnglishToBangla(string english)
        {
            string bangla = "";
            if (!string.IsNullOrEmpty(english))
            {
                bangla = english.Replace("0", "০").Replace("1", "১").Replace("2", "২").Replace("3", "৩").Replace("4", "৪").Replace("5", "৫").Replace("6", "৬").Replace("7", "৭").Replace("8", "৮").Replace("9", "৯");
            }
            return bangla;
        }
    }


}
