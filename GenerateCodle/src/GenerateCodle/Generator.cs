using System;
using System.Collections.Generic;
using System.Text;

namespace GenerateCodle
{
    class Generator
    {
        static Random r;
        static int sign, num1, num2, nOrB;
        public static char[] GenerateAnswer()
        {
            r = new Random();
            nOrB = r.Next(1, 4);
            bool evaluation = r.Next(0, 2) == 1;
            string half1 = Term(evaluation, nOrB);
            nOrB = r.Next(1, 4);

            return RunComplicator(half1 + LatterTerm(evaluation, nOrB), ref r);
        }

        static string Term(bool eval, int d)
        {
            string term;
            if (d < 3)
            {
                do
                {
                    sign = r.Next(1, 7);
                    num1 = r.Next(0, 9);
                    num2 = r.Next(0, num1);

                } while ((num1 == num2 && (sign == 2 || sign == 4 || sign == 6) && eval == false) || (eval == true && num1 != num2 && sign == 2));

                if (num1 == num2)
                {
                    switch (r.Next(1, 4))
                    {
                        case 1:
                            sign = 2;
                            break;
                        case 2:
                            sign = 4;
                            break;
                        case 3:
                            sign = 6;
                            break;
                    }
                }
            }
            else
                sign = r.Next(1, 3);

            switch (sign)
            {
                case 1:
                    if (d == 3)
                        if (r.Next(0, 1) == 1)
                            term = eval ? "T≠F" : "T≠T";
                        else
                            term = eval ? "F≠T" : "F≠F";
                    else
                        term = DoOrder(eval, num1, num2, "\u2260");
                    break;
                case 2:
                    if (d == 3)
                        if (r.Next(0, 1) == 1)
                            term = eval ? "T=T" : "T=F";
                        else
                            term = eval ? "F=F" : "F=T";
                    else
                        term = DoOrder(eval, num1, num2, "=");
                    break;
                case 3:
                    term = DoOrder(eval, num1, num2, ">");
                    break;
                case 4:
                    term = DoOrder(eval, num1, num2, "≥");
                    break;
                case 5:
                    term = DoOrder(!eval, num1, num2, "<");
                    break;
                case 6:
                    term = DoOrder(!eval, num1, num2, "≤");
                    break;
                default:
                    Console.WriteLine("Something went wrong.");
                    term = "";
                    break;
            }

            return term;


        }

        static string DoOrder(bool eval, int num1, int num2, string symbol)
        {
            return eval ? $"{num1}{symbol}{num2}" : $"{num2}{symbol}{num1}";
        }

        static string LatterTerm(bool eval, int d)
        {
            string term;
            int sign;
            do
            {
                sign = r.Next(1, 5);
            } while (!eval && sign == 2);

            switch (sign)
            {
                case 1:
                    term = "|";
                    if (!eval)
                    {
                        string additionToTerm = Term(true, d);
                        term += additionToTerm;
                    }
                    else
                    {
                        string additionToTerm = r.Next(0, 2) == 1 ? Term(true, d) : Term(false, d);
                        term += additionToTerm;
                    }
                    break;
                case 2:
                    term = "&" + Term(true, d);
                    break;
                case 3:
                    term = "=" + Term(eval, d);
                    break;
                case 4:
                    term = "\u2260" + Term(!eval, d); //not equals
                    break;
                default:
                    term = "";
                    break;
            }
            return term;
        }

        static char[] RunComplicator(string str, ref Random r)
        {
            while (str.Length < 8)
            {
                if (str.Contains("T"))
                    str = ReplaceFirst(str, "T", "!F");
                else if (str.Contains("F"))
                    str = ReplaceFirst(str, "F", "!T");
                else
                {
                    string tbr = FindBefore(str, "≥");
                    if (tbr != null)
                        str = ReplaceFirst(str, tbr, tbr + r.Next(0, 10));
                    else
                    {
                        tbr = FindBefore(str, ">");
                        if (tbr != null)
                            str = ReplaceFirst(str, tbr, tbr + r.Next(0, 10));
                        else
                        {
                            tbr = FindBefore(str, "\u2260");
                            if (tbr != null)
                                str = ReplaceFirst(str, tbr, tbr + r.Next(0, 10));
                            else
                            {
                                string highest = FindHighestNum(str);
                                str = ReplaceFirst(str, highest, highest + r.Next(0, 10));
                                if (str.Contains("="))
                                {
                                    string sub = str.Substring(str.IndexOf("=") + 1);
                                    if (sub.Contains("="))
                                    {
                                        if (sub.Contains("&"))
                                            str = ReplaceFirst(str, "&", "|");
                                        else
                                            str = ReplaceFirst(str, "=", "\u2260");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return str.ToCharArray();
        }

        static string ReplaceFirst(string str, string search, string replace)
        {
            int pos = str.IndexOf(search);
            return str.Substring(0, pos) + replace + str.Substring(pos + search.Length);
        }
        static string FindHighestNum(string s)
        {
            int highest = 0, current = 0;
            foreach (char c in s.ToCharArray())
            {
                if (c >= 48 && c <= 57)
                    current = int.Parse(c.ToString());

                if (current > highest)
                    highest = current;
            }
            return highest.ToString();
        }

        static string FindBefore(string s, string search)
        {
            int index = s.IndexOf(search);
            if (index > 0)
                return s.Substring(index - 1, 1);
            return null;
        }
    }
}
