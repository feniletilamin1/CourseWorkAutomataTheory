using System;
using System.Collections.Generic;

namespace CourwWorkAutomataTheory
{
    class ExpressionAnalyzer
    {
        int counter = 0;
        readonly LexemeAnalyzer lexemeAnalyzer = new LexemeAnalyzer();
        private readonly List<string> infixExpr;
        //	Хранит постфиксное выражение
        List<string> postfixExprList;

        //	Список и приоритет операторов
        private readonly Dictionary<string, int> operationPriority = new Dictionary<string, int>()
        {
            { "(", 0 },
            { "+", 1 },
            { "-", 1 },
            { "*", 2 },
            { "/", 2 },
            { "^", 3 }
        };

        public ExpressionAnalyzer(List<string> expression, LexemeAnalyzer lexeme)
        {
            //	Инициализируем поля
            infixExpr = expression;
            lexemeAnalyzer = lexeme;
        }

        private List<string> ToPostfix(List<string> infixExpr)
        {
            //	Выходная строка, содержащая постфиксную запись
            //	Инициализация стека, содержащий операторы в виде символов
            Stack<string> stack = new Stack<string>();

            if(infixExpr.Count > 1)
            {
                for (int i = 0; i < infixExpr.Count-1; i++)
                {
                    if((infixExpr[i] == "+" && infixExpr[i+1] == "+") || (infixExpr[i] == "-" && infixExpr[i + 1] == "-") || 
                        (infixExpr[i] == "*" && infixExpr[i + 1] == "*") || (infixExpr[i] == "/" && infixExpr[i + 1] == "/") || 
                        (lexemeAnalyzer.indentificators.Contains(infixExpr[i]) && lexemeAnalyzer.indentificators.Contains(infixExpr[i+1])
                        || (lexemeAnalyzer.literals.Contains(infixExpr[i]) && lexemeAnalyzer.literals.Contains(infixExpr[i + 1])) ||
                        (lexemeAnalyzer.indentificators.Contains(infixExpr[i]) && lexemeAnalyzer.literals.Contains(infixExpr[i + 1])) ||
                        (lexemeAnalyzer.literals.Contains(infixExpr[i]) && lexemeAnalyzer.indentificators.Contains(infixExpr[i + 1]))) ||
                        infixExpr[i + 1] == "(" && (lexemeAnalyzer.literals.Contains(infixExpr[i]) || lexemeAnalyzer.indentificators.Contains(infixExpr[i]))) {
                        throw new Exception("Ошибка в арифметичекском выражении!");
                    }
                    
                }
            }

            List<string> postFixExprList = new List<string>();

            //	Перебираем строку
            for (int i = 0; i < infixExpr.Count; i++)
            {
                //	Текущий символ
                string c = infixExpr[i];

                //	Если симовол - цифра
                if (lexemeAnalyzer.indentificators.Contains(c) || lexemeAnalyzer.literals.Contains(c))
                {
                    //	Парсии его, передав строку и текущую позицию, и заносим в выходную строку
                    postFixExprList.Add(c);
                }
                //	Если открывающаяся скобка 
                else if (c == "(")
                {
                    //	Заносим её в стек
                    stack.Push(c);
                }
                //	Если закрывающая скобка
                else if (c == ")")
                {
                    //	Заносим в выходную строку из стека всё вплоть до открывающей скобки
                    while (stack.Count > 0 && stack.Peek() != "(")
                        postFixExprList.Add(stack.Pop());
                    //	Удаляем открывающуюся скобку из стека
                    try
                    {
                        stack.Pop();
                    }

                    catch
                    {
                        throw new Exception("Отсутвует открывающая скобка в арифметическом выражении!");
                    }
                }
                //	Проверяем, содержится ли символ в списке операторов
                else if (operationPriority.ContainsKey(c))
                {
                    //	Если да, то сначала проверяем
                    string op = c;

                    //	Заносим в выходную строку все операторы из стека, имеющие более высокий приоритет
                    while (stack.Count > 0 && (operationPriority[stack.Peek()] >= operationPriority[op]))
                        postFixExprList.Add(stack.Pop());
                    //	Заносим в стек оператор
                    stack.Push(op);
                }
                else 
                {
                    throw new Exception("Ошибка в арифметическом выражении!");
                }
            }

            //	Заносим все оставшиеся операторы из стека в выходную строку
            foreach (string op in stack)
            {
                if(stack.Peek() == infixExpr[infixExpr.Count-1]) {
                    throw new Exception("Ошибка в арифметическом выражении");
                }
                postFixExprList.Add(op);
            }
                
            //	Возвращаем выражение в постфиксной записи
            return postFixExprList; 
        }

        public string Analyze()
        {
            string log = "";
            //	Стек для хранения чисел
            Stack<string> locals = new Stack<string>();

            postfixExprList = ToPostfix(infixExpr);

            //	Проходим по строке
            foreach (var item in postfixExprList)
            {

                //	Текущий символ
                string c = item;

                //	Если символ число
                if (lexemeAnalyzer.indentificators.Contains(c) || lexemeAnalyzer.literals.Contains(c))
                {
                    //	Парсим
                    string number = c;
                    //	Заносим в стек, преобразовав из String в Double-тип
                    locals.Push(number);
                }
                //	Если символ есть в списке операторов
                else if (operationPriority.ContainsKey(c))
                {
                    //	Прибавляем значение счётчику
                    counter += 1;

                    //	Получаем значения из стека в обратном порядке
                    string second = locals.Pop();
                    string first;
                    if(locals.Count != 0)
                    {
                        first = locals.Pop();
                    }
                    else
                    {
                        throw new Exception("Отсутсвует закрывающая скобка в арифметическом выражении!");
                    }
                    locals.Push("OP"+ counter);

                    //	Отчитываемся пользователю о проделанной работе
                    log += $"{locals.Peek()} =  {first} {c} {second}\n";
                }
            }
            return log;
        }
    }
}
