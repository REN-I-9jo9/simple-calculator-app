using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Xml.Linq;

namespace Simple_Caculator_App.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ResultCommand))]
        [NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
        private string _text = "";

        [ObservableProperty]
        private string _number0Content = "0";

        [ObservableProperty]
        private string _number1Content = "1";

        [ObservableProperty]
        private string _number2Content = "2";

        [ObservableProperty]
        private string _number3Content = "3";

        [ObservableProperty]
        private string _number4Content = "4";

        [ObservableProperty]
        private string _number5Content = "5";

        [ObservableProperty]
        private string _number6Content = "6";

        [ObservableProperty]
        private string _number7Content = "7";

        [ObservableProperty]
        private string _number8Content = "8";

        [ObservableProperty]
        private string _number9Content = "9";

        [ObservableProperty]
        private string _dotContent = ".";

        [ObservableProperty]
        private string _operatorPlusContent = "+";

        [ObservableProperty]
        private string _operatorSubtractContent = "-";

        [ObservableProperty]
        private string _operatorMultiplyContent = "*";

        [ObservableProperty]
        private string _operatorDivideContent = "/";

        private bool CanUseButton() => true;

        [RelayCommand(CanExecute = nameof(CanUseButton))]
        private void Input(string input)
        {
            Text += input;
        }

        private bool VaildEquation()
        {
            var strs = Text.Split(['+', '-', '*', '/']);
            if (strs.Any(str =>
            {
                return !decimal.TryParse(str, out _);
            }))
                return false;
            return true;
        }

        private bool TextNotEmpty() => !string.IsNullOrEmpty(Text);

        [RelayCommand(CanExecute = nameof(TextNotEmpty))]
        private void Delete()
        {
            Text = Text.Substring(0, Text.Length - 1);
        }

        [RelayCommand(CanExecute = nameof(VaildEquation))]
        private void Result()
        {
            //infix to postfix
            var tokens = new List<string>();
            var text = Text;
            while (text.Length > 0)
            {
                var operatorIndex = text.IndexOfAny(['+', '-', '*', '/']);
                switch (operatorIndex)
                {
                    case 0:
                        tokens.Add($"{text[0]}");
                        text = text.Substring(1);
                        break;
                    case -1:
                        tokens.Add(text);
                        text = "";
                        break;
                    default:
                        tokens.Add(text.Substring(0, operatorIndex));
                        text = text.Substring(operatorIndex);
                        break;
                }
            }
            var stack = new Stack<string>();
            var postfix = new List<string>();
            for (var i = 0; i < tokens.Count; i++)
            {
                switch (tokens[i])
                {
                    case "+":
                    case "-":
                        while (stack.Any())
                            postfix.Add(stack.Pop());
                        stack.Push(tokens[i]);
                        break;
                    case "*":
                    case "/":
                        stack.Push(tokens[i]);
                        break;
                    default:
                        postfix.Add(tokens[i]);
                        break;
                }
            }
            while (stack.Any())
                postfix.Add(stack.Pop());

            var resultstack = new Stack<decimal>();
            for(var i = 0; i < postfix.Count; i++)
            {
                if (decimal.TryParse(postfix[i], out var number))
                    resultstack.Push(number);
                else
                {
                    var number2 = resultstack.Pop();
                    var number1 = resultstack.Pop();
                    switch (postfix[i])
                    {
                        case "+":
                            resultstack.Push(number1 + number2);
                            break;
                        case "-":
                            resultstack.Push(number1 - number2);
                            break;
                        case "*":
                            resultstack.Push(number1 * number2);
                            break;
                        case "/":
                            resultstack.Push(number1 / number2);
                            break;
                    }
                }
            }
            var result = resultstack.Pop();
            Text = $"{result}";
        }
    }
}
