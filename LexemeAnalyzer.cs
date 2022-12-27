using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CourwWorkAutomataTheory
{
    public class LexemeAnalyzer
    {
        readonly string pattern = @"\p{IsCyrillic}";

        public readonly List<string> literals = new List<string>();
        public readonly List<string> indentificators = new List<string>();
        public readonly List<string> limiters = new List<string>() {">=", "<=", "=", "<", ">", "+", "-", "*", "/", "\n", "(",")"};
        public readonly List<string> keyWords = new List<string>() { "Dim", "as", "integer", "char", "single", "string", "if", "then", "else", "end", "expr" };
        public readonly List<Tuple<string, string>> lexemes = new List<Tuple<string, string>>();
        private enum TypeOfLexem
        {
            I,
            L,
            R
        }

        public List<Tuple<string, string>> GetLexeme(string text)
        {
            lexemes.Clear();

            TypeOfLexem folowLexemeType = TypeOfLexem.R;
            string buffer = "";

            if (Regex.Matches(text, pattern).Count > 0)
            {
                throw new Exception("Русские буквы недопустимы");
            }

            for (int i = 0; i < text.Length; i++)
            {
                char symbol = text[i];

                switch (CheckLexeme(symbol))
                {
                    case "Letter":

                        if (buffer == "")
                        {
                            folowLexemeType = TypeOfLexem.I;
                            buffer += symbol;
                            break;
                        }

                        switch (folowLexemeType)
                        {
                            case TypeOfLexem.I:
                                buffer += symbol;
                                break;

                            case TypeOfLexem.L:
                                throw new Exception("Идентификатор не может начинаться с цифр");

                            case TypeOfLexem.R:
                                lexemes.Add(new Tuple<string, string>(buffer, "Разделитель"));
                                buffer = symbol.ToString();
                                folowLexemeType = TypeOfLexem.I;
                                break;
                        }

                        break;

                    case "Digit":

                        if (buffer == "")
                        {
                            folowLexemeType = TypeOfLexem.L;
                            buffer += symbol;
                            break;
                        }

                        switch (folowLexemeType)
                        {
                            case TypeOfLexem.I:
                                buffer += symbol;
                                break;

                            case TypeOfLexem.L:
                                buffer += symbol;
                                break;

                            case TypeOfLexem.R:
                                lexemes.Add(new Tuple<string, string>(buffer, "Разделитель"));
                                buffer = symbol.ToString();
                                folowLexemeType = TypeOfLexem.L;
                                break;
                        }

                        break;

                    case "Lemiter":

                        if (symbol == '\n')
                        {

                            string type = "";

                            switch (folowLexemeType)
                            {
                                case TypeOfLexem.I: type = "Идентификатор"; break;
                                case TypeOfLexem.L: type = "Литерал"; break;
                                case TypeOfLexem.R: type = "Разделитель"; break;
                            }

                            if (text[i-1] != ' ' && text[i - 1] != '\n')
                            {
                                lexemes.Add(new Tuple<string, string>(buffer, type));
                                buffer = "";
                            }

                            if (text[i-1] != '\n' && lexemes[lexemes.Count-1].Item1 != "\n")
                            {
                                lexemes.Add(new Tuple<string, string>("\n", "Разделитель"));
                                buffer = "";
                            }


                            break;
                        }

                        if (buffer == "")
                        {
                            folowLexemeType = TypeOfLexem.R;
                            buffer += symbol;
                            break;
                        }

                        switch (folowLexemeType)
                        {
                            case TypeOfLexem.I:
                                lexemes.Add(new Tuple<string, string>(buffer, "Идентификатор"));
                                buffer = symbol.ToString();
                                folowLexemeType = TypeOfLexem.R;
                                break;

                            case TypeOfLexem.L:
                                lexemes.Add(new Tuple<string, string>(buffer, "Литерал"));
                                buffer = symbol.ToString();
                                folowLexemeType = TypeOfLexem.R;
                                break;

                            case TypeOfLexem.R:
                                if (buffer.Length > 2)
                                    throw new Exception("Разделитель не может иметь более двух символов");
                                string str = buffer + symbol;
                                if(limiters.Contains(str))
                                {
                                    lexemes.Add(new Tuple<string, string>(str, "Разделитель"));
                                    buffer = "";
                                    break;
                                }
                                else
                                {
                                    lexemes.Add(new Tuple<string, string>(buffer, "Разделитель"));
                                    buffer = "";
                                }

                                buffer += symbol;

                                folowLexemeType = TypeOfLexem.R;
                                break;
                        }

                        break;

                    case "Space":

                        if (buffer == "")
                        {
                            break;
                        }

                        switch (folowLexemeType)
                        {
                            case TypeOfLexem.I:
                                lexemes.Add(new Tuple<string, string>(buffer, "Идентификатор"));
                                break;

                            case TypeOfLexem.L:
                                lexemes.Add(new Tuple<string, string>(buffer, "Литерал"));
                                break;

                            case TypeOfLexem.R:
                                lexemes.Add(new Tuple<string, string>(buffer, "Разделитель"));
                                break;
                        }
                        buffer = "";
                        break;
                }
            }

            if(buffer != "")
            {
                switch (folowLexemeType)
                {
                    case TypeOfLexem.I:
                        lexemes.Add(new Tuple<string, string>(buffer, "Идентификатор"));
                        break;

                    case TypeOfLexem.L:
                        lexemes.Add(new Tuple<string, string>(buffer, "Литерал"));
                        break;

                    case TypeOfLexem.R:
                        lexemes.Add(new Tuple<string, string>(buffer, "Разделитель"));
                        break;
                }
            }

            if (lexemes[lexemes.Count-1].Item1 != "\n")
            {
                lexemes.Add(new Tuple<string, string>("\n", "Разделитель"));
            }

            return lexemes;
        }

        private string CheckLexeme(char sym)
        {
            string symType;

            if (char.IsDigit(sym))
            {
                symType = "Digit";
            }

            else if (char.IsLetter(sym))
            {
                symType = "Letter";
            }

            else if (sym == ' ')
            {
                symType = "Space";
            }

            else if (limiters.Contains(sym.ToString()))
            {
                symType = "Lemiter";
            }

            else
            {
                throw new Exception("Запрещенный символ!");
            }
            return symType;
        }

        public List<Tuple<string, int>> GetTccTable()
        {
            List<Tuple<string, int>> resultList = new List<Tuple<string, int>>();
            foreach (var item in lexemes)
            {
                string value = item.Item1;
                string type = item.Item2;


                switch (type)
                {
                    case "Идентификатор":
                        if (keyWords.Contains(value))
                        {
                            resultList.Add(new Tuple<string, int>("K", keyWords.IndexOf(value)));
                        }
                        else if (!keyWords.Contains(value))
                        {
                            if (value.Length <= 6)
                            {
                                if (!indentificators.Contains(value))
                                {
                                    indentificators.Add(value);
                                    resultList.Add(new Tuple<string, int>("I", indentificators.IndexOf(value)));
                                }
                                else
                                {
                                    resultList.Add(new Tuple<string, int>("I", indentificators.IndexOf(value)));
                                }
                            }

                            else
                            {
                                throw new Exception("Имя переменной не должна превышать длину 6-ти символов");
                            }
                        }
                        break;
                    case "Литерал":
                        if (!literals.Contains(value))
                        {
                            literals.Add(value);
                            resultList.Add(new Tuple<string, int>("L", literals.IndexOf(value)));
                        }
                        else
                        {
                            resultList.Add(new Tuple<string, int>("L", literals.IndexOf(value)));
                        }
                        break;
                    case "Разделитель":
                        if (value.Length <= 2)
                        {
                            resultList.Add(new Tuple<string, int>("R", limiters.IndexOf(value)));
                        }
                        else
                        {
                            throw new Exception("Длина разделителя не превышает 2-ух симоволов");
                        }

                        break;
                }
            }
            return resultList;
        }
    }

}
