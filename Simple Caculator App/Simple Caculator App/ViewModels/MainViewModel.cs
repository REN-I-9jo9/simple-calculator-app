using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Simple_Caculator_App.Models;
using System.Collections.Generic;
using System.Linq;

namespace Simple_Caculator_App.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            TokenList.CollectionChanged += TokenList_CollectionChanged;
        }

        private void TokenList_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(TokenList));
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ResultCommand))]
        [NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
        private ObservableCollectionEx _tokenList = new();

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
            if (!TokenList.Any())
            {
                TokenList.Add(input);
            }
            else
            {
                switch (input)
                {
                    case "+":
                    case "-":
                    case "*":
                    case "/":
                        TokenList.Add(input);
                        break;
                    default:
                        var lastToken = TokenList[TokenList.Count - 1];
                        if (lastToken.Text.Contains('+') || lastToken.Text.Contains('-') || lastToken.Text.Contains('*') || lastToken.Text.Contains('/'))
                            TokenList.Add(input);
                        else
                        {
                            lastToken += input;
                            TokenList[TokenList.Count - 1] = lastToken;
                        }
                        break;
                }
            }
        }

        private bool VaildEquation()
        {
            return true;
        }

        private bool TextNotEmpty() => true;

        [RelayCommand(CanExecute = nameof(TextNotEmpty))]
        private void Delete()
        {
            var lastToken = TokenList[TokenList.Count - 1];
            if (lastToken.Length > 1)
                TokenList[TokenList.Count - 1] = lastToken.Substring(0, lastToken.Length - 1);
            else
                TokenList.RemoveAt(TokenList.Count - 1);
        }

        [RelayCommand(CanExecute = nameof(VaildEquation))]
        private void Result()
        {
            //infix to postfix
            var tokens = TokenList.ToList();
            var stack = new Stack<string>();
            var postfix = new List<string>();
            for (var i = 0; i < tokens.Count; i++)
            {
                switch (tokens[i].Text)
                {
                    case "+":
                    case "-":
                        while (stack.Any())
                            postfix.Add(stack.Pop());
                        stack.Push(tokens[i].Text);
                        break;
                    case "*":
                    case "/":
                        stack.Push(tokens[i].Text);
                        break;
                    default:
                        postfix.Add(tokens[i].Text);
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
            TokenList.Clear();
            TokenList.Add($"{result}");
        }
    }
}
