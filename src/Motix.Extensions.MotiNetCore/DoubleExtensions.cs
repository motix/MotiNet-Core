using System;
using System.Globalization;

namespace MotiNet
{
    public static class DoubleExtensions
    {
        public static string InWords(this double number)
        {
            switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
            {
                case "vi":
                    return InWordsVi(number);
                default:
                    return number.ToString();
            }
        }

        private static string InWordsVi(double number)
        {
            string[] numbers = new string[] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín", "mười" };
            string amountString = Convert.ToInt64(number < 0 ? (number * -1) : number).ToString();
            string ret = string.Empty;
            for (int i = 0; i < amountString.Length; i++)
            {
                int digit = int.Parse(amountString[i].ToString());
                if (digit == 0)
                {
                    if ((amountString.Length - i) % 3 == 0)							// 0 trăm
                    {
                        if (int.Parse(amountString[i + 1].ToString()) == 0 &&
                            int.Parse(amountString[i + 2].ToString()) == 0)
                        {
                            continue;
                        }
                        else
                        {
                            ret += numbers[digit] + " ";
                        }
                    }
                    else if ((amountString.Length - i) % 3 == 2)					// 0 chục
                    {
                        if (int.Parse(amountString[i + 1].ToString()) == 0)
                        {
                            continue;
                        }
                    }
                    else if ((amountString.Length - i) % 3 == 1)					// 0 đơn vị
                    {
                        if (i == 0)
                        {
                            ret += numbers[digit] + " ";
                        }
                        else if (int.Parse(amountString[i - 1].ToString()) == 0 &&
                            int.Parse(amountString[i - 2].ToString()) == 0)
                        {
                            continue;
                        }
                    }
                }
                else if (digit == 1)
                {
                    if ((amountString.Length - i) % 3 == 0)							// 1 trăm
                    {
                        ret += numbers[digit] + " ";
                    }
                    else if ((amountString.Length - i) % 3 == 2)					// 1 chục
                    {
                        ret += "mười ";
                        continue;
                    }
                    else if ((amountString.Length - i) % 3 == 1)					// 1 đơn vị
                    {
                        if (i > 0 && int.Parse(amountString[i - 1].ToString()) != 1 &&
                            int.Parse(amountString[i - 1].ToString()) != 0)
                        {
                            ret += "mốt ";
                        }
                        else
                        {
                            ret += numbers[digit] + " ";
                        }
                    }
                }
                else if (digit == 5)
                {
                    if ((amountString.Length - i) % 3 == 0)							// 5 trăm
                    {
                        ret += numbers[digit] + " ";
                    }
                    else if ((amountString.Length - i) % 3 == 2)					// 5 chục
                    {
                        ret += numbers[digit] + " ";
                    }
                    else if ((amountString.Length - i) % 3 == 1)					// 5 đơn vị
                    {
                        if (i > 0 && int.Parse(amountString[i - 1].ToString()) != 0)
                        {
                            ret += "lăm ";
                        }
                        else
                        {
                            ret += numbers[digit] + " ";
                        }
                    }
                }
                else
                {
                    ret += numbers[digit] + " ";
                }
                if (amountString.Length - i - 1 == 0)
                {
                }
                else if ((amountString.Length - i - 1) % 9 == 0)
                {
                    ret += "tỉ ";
                }
                else if ((amountString.Length - i - 1) % 9 == 8)
                {
                    ret += "trăm ";
                }
                else if ((amountString.Length - i - 1) % 9 == 7)
                {
                    if (digit == 0)
                    {
                        ret += "lẻ ";
                    }
                    else
                    {
                        ret += "mươi ";
                    }
                }
                else if ((amountString.Length - i - 1) % 9 == 6)
                {
                    ret += "triệu ";
                }
                else if ((amountString.Length - i - 1) % 9 == 5)
                {
                    ret += "trăm ";
                }
                else if ((amountString.Length - i - 1) % 9 == 4)
                {
                    if (digit == 0)
                    {
                        ret += "lẻ ";
                    }
                    else
                    {
                        ret += "mươi ";
                    }
                }
                else if ((amountString.Length - i - 1) % 9 == 3)
                {
                    ret += "nghìn ";
                }
                else if ((amountString.Length - i - 1) % 9 == 2)
                {
                    ret += "trăm ";
                }
                else if ((amountString.Length - i - 1) % 9 == 1)
                {
                    if (digit == 0)
                    {
                        ret += "lẻ ";
                    }
                    else
                    {
                        ret += "mươi ";
                    }
                }
            }

            ret = ret.Substring(0, ret.Length - 1);

            if (number < 0)
            {
                ret = "âm " + ret;
            }

            ret = ret[0].ToString().ToUpper() + ret.Substring(1);

            return ret;
        }
    }
}
