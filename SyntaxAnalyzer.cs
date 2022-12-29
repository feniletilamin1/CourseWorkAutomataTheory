using System;
using System.Collections.Generic;

namespace CourwWorkAutomataTheory
{
    public class SyntaxAnalyzer
    {
        int exprCounter = 1;
        int i;
        int state = 0;
        int rowNum = 1;
        public string log;

        List<Tuple<string, int>> list;

        LexemeAnalyzer lexeme;

        bool isEnd = false;
        readonly Stack<string> stack = new Stack<string>();
        readonly Stack<int> stateStack = new Stack<int>();

        public void CheckSyntax(List<Tuple<string, int>> list, LexemeAnalyzer lexeme)
        {
            this.list = list;
            this.lexeme = lexeme;

            GoState(0);

            while (!isEnd)
            {
                switch (state)
                {
                    case 0:
                        State0();
                        break;
                    case 1:
                        State1();
                        break;
                    case 2:
                        State2();
                        break;
                    case 3:
                        State3();
                        break;
                    case 4:
                        State4();
                        break;
                    case 5:
                        State5();
                        break;
                    case 6:
                        State6();
                        break;
                    case 7:
                        State7();
                        break;
                    case 8:
                        State8();
                        break;
                    case 9:
                        State9();
                        break;
                    case 10:
                        State10();
                        break;
                    case 11:
                        State11();
                        break;
                    case 12:
                        State12();
                        break;
                    case 13:
                        State13();
                        break;
                    case 14:
                        State14();
                        break;
                    case 15:
                        State15();
                        break;
                    case 16:
                        State16();
                        break;
                    case 17:
                        State17();
                        break;
                    case 18:
                        State18();
                        break;
                    case 19:
                        State19();
                        break;
                    case 20:
                        State20();
                        break;
                    case 21:
                        State21();
                        break;
                    case 22:
                        State22();
                        break;
                    case 23:
                        State23();
                        break;
                    case 24:
                        State24();
                        break;
                    case 25:
                        State25();
                        break;
                    case 26:
                        State26();
                        break;
                    case 27:
                        State27();
                        break;
                    case 28:
                        State28();
                        break;
                    case 29:
                        State29();
                        break;
                    case 30:
                        State30();
                        break;
                    case 31:
                        State31();
                        break;
                    case 32:
                        State32();
                        break;
                    case 33:
                        State33();
                        break;
                    case 34:
                        State34();
                        break;
                    case 35:
                        State35();
                        break;
                    case 36:
                        State36();
                        break;
                    case 37:
                        State37();
                        break;
                    case 38:
                        State38();
                        break;
                    case 39:
                        State39();
                        break;
                    case 40:
                        State40();
                        break;
                    case 41:
                        State41();
                        break;
                    case 42:
                        State42();
                        break;
                    case 43:
                        State43();
                        break;
                }
            }

            isEnd = false;
            rowNum = 1;

            if(stack.Count > 1)
            {
                Reset();
                throw new Exception("Встретилось излишнее ключевое слово!");
            }

        }

        public void Reset()
        {
            i = 0;
            rowNum = 1;
            stack.Clear();
            state = 0;
            stateStack.Clear();
            log = "";
            exprCounter = 1;
        }

        private void Shift()
        {
            stack.Push(lexeme.lexemes[i].Item1);
            i++;
        }

        private void GoState(int state)
        {
            stateStack.Push(state);
            this.state = state;
        }

        private void Convolution(int count, string NotATerminal) 
        {
            for (int i = 0; i < count; i++)
            {
                stack.Pop();
                state = stateStack.Pop();
            }

            state = stateStack.Peek();
            stack.Push(NotATerminal);
        }

        private void Expr()
        {
            int count = 0;
            List<string> expr = new List<string>();
            while(lexeme.lexemes[i].Item1 != "\n" && i-1 <= lexeme.lexemes.Count)
            {
                expr.Add(lexeme.lexemes[i].Item1);
                Shift();
                stateStack.Push(100);
                count++;

            }
            ExpressionAnalyzer expression = new ExpressionAnalyzer(expr, lexeme);
            string outStr = expression.Analyze();
            if(outStr != "")
            {
                log += $"Выражение №{exprCounter}:\n" + outStr + "\n";
                exprCounter++;
            }
               
            Convolution(count, "expr");
        }

        private void Eror(string text)
        {

            string[] words = text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string meet;
            string expected= "";

            if (lexeme.lexemes[i - 1].Item1 == "\n")
                meet = "\\n";
            else
                meet = lexeme.lexemes[i - 1].Item1;

            for (int i = 0; i < words.Length; i++)
            {
                if (i != words.Length-1)
                {
                    expected += words[i] + " или ";
                }

                else
                {
                    expected += words[i];
                }
                
            }

            stack.Clear();
            stateStack.Clear();
            i = 0;

            throw new Exception($"Ошибка на строке {rowNum}\nВстретилось: {meet}\n\nОжидалось: \n{expected}.");
        }

        private void State0()
        {
            if (stack.Count == 0)
            {
                Shift();
                return;
            }

            else if (lexeme.indentificators.Contains(stack.Peek()))
            {
                GoState(8);
                return;
            }

            switch (stack.Peek())
            {
                case "<программа>": isEnd = true; break;
                case "<список_операторов>": GoState(1); break;
                case "<оператор>": GoState(2); break;
                case "<условие>": GoState(3); break;
                case "<присвоение>": GoState(4); break;
                case "<объявление>": GoState(5); break;
                case "if": GoState(6); break;
                case "Dim": GoState(7); break;
                default:
                    Eror("if,Dim,Индентификатор");
                    break;
            }
        }

        private void State1()
        {
            if (lexeme.indentificators.Contains(stack.Peek()))
            {
                GoState(8);
                return;
            }

            switch (stack.Peek())
            {
                case "<список_операторов>": if (i < lexeme.lexemes.Count-1) { Shift(); } else { Convolution(1, "<программа>"); } break;
                case "<оператор>": GoState(9); break;
                case "<условие>": GoState(3); break;
                case "<присвоение>": GoState(4); break;
                case "<объявление>": GoState(5); break; 
                case "if": GoState(6); break;
                case "Dim": GoState(7); break;
                default:
                    Eror("if,Dim, Индентификатор");
                    break;
            }
        }

        private void State2()
        {
            switch (stack.Peek())
            {
                case "<оператор>": Shift(); break;
                case "\n": GoState(10); rowNum++; break;
                default:
                    Eror("\\n");
                    break;
            }
        }

        private void State3()
        {
            Convolution(1, "<оператор>");
        }

        private void State4()
        {
             Convolution(1, "<оператор>");
        }
        private void State5()
        {
            Convolution(1, "<оператор>");
        }

        private void State6()
        {
            if (lexeme.indentificators.Contains(stack.Peek()))
            {
                GoState(13);
                return;
            }

            else if (lexeme.literals.Contains(stack.Peek()))
            {
                GoState(14);
                return;
            }

            switch (stack.Peek())
            {
                case "if": Shift(); break;
                case "<логическое_условие>": GoState(11); break;
                case "<операнд>": GoState(12); break;
                default:
                    Eror("if,Литерал,Идентификатор");
                    break;

            }
        }

        private void State7()
        {
            if (lexeme.indentificators.Contains(stack.Peek()))
            {
                GoState(16);
                return;
            }   

            switch (stack.Peek())
            {
                case "Dim": Shift(); break;
                case "<список_переменных>": GoState(15); break;
                default:
                    Eror("Dim, Индентификатор");
                    break;

            }
        }

        private void State8()
        {
            if (lexeme.indentificators.Contains(stack.Peek()))
            {
                Shift();
                return;
            }

            if (stack.Peek() == "=")
            {
                GoState(17);
            }

            else 
            {
                Eror("=");
            }
        }
        private void State9()
        {
            switch (stack.Peek())
            {
                case "<оператор>": Shift(); break;
                case "\n": GoState(18); rowNum++; break;
                default:
                    Eror("\\n");
                    break;
            }
        }

        private void State10()
        {
            Convolution(2, "<список_операторов>");
        }

        private void State11()
        {
            switch (stack.Peek())
            {
                case "<логическое_условие>": Shift(); break;
                case "then": GoState(19); break;
                default:
                    Eror("then");
                    break;
            }
        }

        private void State12()
        {
            switch (stack.Peek())
            {
                case "<операнд>": Shift(); break;
                case "<логический_знак>": GoState(20); break;
                case ">": GoState(21); break;
                case "<": GoState(22); break;
                case "=": GoState(23); break;
                case ">=": GoState(24); break;
                case "<=": GoState(25); break;
                default:
                    Eror(">,<,=,>=,<=");
                    break;
            }
        }

        private void State13()
        {
            Convolution(1, "<операнд>");
        }

        private void State14()
        {
            Convolution(1, "<операнд>");
        }

        private void State15()
        {
            switch (stack.Peek())
            {
                case "<список_переменных>": Shift(); break;
                case "as": GoState(26); break;
                case ",": GoState(27); break;
                default:
                    Eror("as,,");
                    break;
            }
        }

        private void State16()
        {
            Convolution(1, "<список_переменных>");
        }

        private void State17()
        {
            switch (stack.Peek())
            {
                case "=": Expr(); break;
                case "expr": GoState(28); break;
                default:
                    Eror("=,expr");
                    break;
            }
        }

        private void State18()
        {
            Convolution(3, "<список_операторов>");
        }

        private void State19()
        {

            switch (stack.Peek())
            {
                case "then": Shift(); break;
                case "\n": GoState(29); rowNum++;  break;
                default:
                    Eror("\\n");
                    break;
            }
        }

        private void State20()
        {

            if (lexeme.indentificators.Contains(stack.Peek()))
            {
                GoState(13);
                return;
            }

            else if (lexeme.literals.Contains(stack.Peek()))
            {
                GoState(14);
                return;
            }

            switch (stack.Peek())
            {
                case "<логический_знак>": Shift(); break;
                case "<операнд>": GoState(30); break;
                default:
                    Eror("Литерал,Идентификатор");
                    break;
            }
        }

        private void State21()
        {
            Convolution(1, "<логический_знак>");
        }

        private void State22()
        {
            Convolution(1, "<логический_знак>");
        }

        private void State23()
        {
            Convolution(1, "<логический_знак>");
        }

        private void State24()
        {
            Convolution(1, "<логический_знак>");
        }

        private void State25()
        {
            Convolution(1, "<логический_знак>");
        }

        private void State26()
        {
            switch (stack.Peek())
            {
                case "as": Shift(); break;
                case "<тип>": GoState(31); break;
                case "integer": GoState(32); break;
                case "double": GoState(33); break;
                case "decimal": GoState(34); break;
                default:
                    Eror("integer,double,decimal");
                    break;
            }
        }

        private void State27()
        {
            if(stack.Peek() == ",")
            {
                Shift();
            }

            else if (lexeme.indentificators.Contains(stack.Peek()))
            {
                GoState(35);
                return;
            }

            else
            {
                Eror("");
            }
        }

        private void State28()
        {
            Convolution(3, "<присвоение>");
        }

        private void State29()
        {
            if (lexeme.indentificators.Contains(stack.Peek()))
            {
                GoState(8);
                return;
            }

            switch (stack.Peek())
            {
                case "\n": Shift(); break;
                case "<список_операторов>": GoState(36); break;
                case "<оператор>": GoState(2); break;
                case "<условие>": GoState(3); break;
                case "<присвоение>": GoState(4); break;
                case "<объявление>": GoState(5); break;
                case "if": GoState(6); break;
                case "Dim": GoState(7); break;
                default:
                    Eror("if,Dim,Идентификатор");
                    break;
            }
        }

        private void State30()
        {
            Convolution(3, "<логическое_условие>");
        }

        private void State31()
        {
            Convolution(4, "<объявление>");
        }

        private void State32()
        {
            Convolution(1, "<тип>");
        }

        private void State33()
        {
            Convolution(1, "<тип>");
        }

        private void State34()
        {
            Convolution(1, "<тип>");
        }

        private void State35()
        {
            Convolution(3, "<список_переменных>");
        }

        private void State36()
        {
            if (lexeme.indentificators.Contains(stack.Peek()))
            {
                GoState(8);
                return;
            }

            switch (stack.Peek())
            {
                case "<список_операторов>": Shift(); break;
                case "end": GoState(37); break;
                case "else": GoState(38); break;
                case "<оператор>": GoState(9); break;
                case "<условие>": GoState(3); break;
                case "<присвоение>": GoState(4); break;
                case "<объявление>": GoState(5); break;
                case "if": GoState(6); break;
                case "Dim": GoState(7); break;
                default:
                    Eror("end,else,if,Dim");
                    break;
            }
        }

        private void State37()
        {
            switch (stack.Peek())
            {
                case "end": Shift(); break;
                case "if": GoState(39); break;
                default:
                    Eror("if");
                    break;
            }
        }

        private void State38()
        {
            switch (stack.Peek())
            {
                case "else": Shift(); break;
                case "\n": GoState(40); rowNum++; break;
                default:
                    Eror("\\n");
                    break;
            }
        }

        private void State39()
        {
            Convolution(7, "<условие>");
        }

        private void State40()
        {
            if (lexeme.indentificators.Contains(stack.Peek()))
            {
                GoState(8);
                return;
            }

            switch (stack.Peek())
            {
                case "\n": Shift(); break;
                case "<список_операторов>": GoState(41); break;
                case "<оператор>": GoState(2); break;
                case "<условие>": GoState(3); break;
                case "<присвоение>": GoState(4); break;
                case "<объявление>": GoState(5); break;
                case "if": GoState(6); break;
                case "Dim": GoState(7); break;
                default:
                    Eror("if,Dim");
                    break;
            }
        }

        private void State41()
        {
            switch (stack.Peek())
            {
                case "<список_операторов>": Shift(); break;
                case "end": GoState(42); break;
                default:
                    Eror("end");
                    break;
            }
        }

        private void State42()
        {
            switch (stack.Peek())
            {
                case "end": Shift(); break;
                case "if": GoState(43); break;
                default:
                    Eror("if");
                    break;
            }
        }

        private void State43()
        {
            Convolution(10, "<условие>");
        }
    }
}
