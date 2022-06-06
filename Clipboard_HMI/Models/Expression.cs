using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Diagnostics;

namespace Clipboard_HMI.Models
{
    public class Expression : ObservableObject
    {
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (value != null)
                {
                    name = value;
                    UserSettings.Default.ExpressionsNames[Index] = value;
                }
            }
        }
        private string content;
        public string Content
        {
            get
            {
                return content;
            }
            set
            {
                if (value != null)
                {
                    content = value;
                    UserSettings.Default.Expressions[Index] = value;
                }
            }
        }
        public int Index { get; private set; }

        public Expression(string name, string content, int index)
        {
            Name = name;
            Content = content;
            Index = Index;
        }

        public Expression()
        {
            Name = String.Empty;
            Content = String.Empty;
        }
    }

    public class Expressions : ObservableObject
    {
        private ObservableCollection<Expression> expressionsList;
        public ObservableCollection<Expression> ExpressionsList
        {
            get
            {
                return expressionsList;
            }
            private set
            {
                expressionsList = value;
                OnPropertyChanged(nameof(ExpressionsList));
            }
        }
        public ObservableCollection<string> ExpressionsNameList { get; set; }

        public Expressions()
        {
            ExpressionsList = new ObservableCollection<Expression>();
            ExpressionsNameList = new ObservableCollection<string>();
            // Create the expressions list from the user settings
            BuildExpressionsNameList();
            
        }

        private void BuildExpressionsNameList()
        {
            for (int idx = 0; idx < UserSettings.Default.Expressions.Count; idx++)
            {
                string name;
                string content = UserSettings.Default.Expressions[idx];
                if (UserSettings.Default.ExpressionsNames.Count > idx)
                {
                    name = UserSettings.Default.ExpressionsNames[idx];
                }
                else
                {
                    name = "UnNamed expr " + (idx + 1).ToString();
                }
                Expression expression = new Expression(name, content, idx);
                ExpressionsList.Add(expression);
                ExpressionsNameList.Add(name);

            }
        }

        public void Save()
        {
            // Copy expressions from buffer to user settings
            for (int idx = 0; idx < ExpressionsList.Count; idx++)
            {
                string name = ExpressionsList[idx].Name;
                string content = ExpressionsList[idx].Content;
                if (UserSettings.Default.ExpressionsNames.Count < idx)
                {
                    UserSettings.Default.ExpressionsNames.Add(name);
                }
                else
                {
                    UserSettings.Default.ExpressionsNames[idx] = name;
                }

                if (UserSettings.Default.Expressions.Count < idx)
                {
                    UserSettings.Default.Expressions.Add(content);
                }
                else
                {
                    UserSettings.Default.Expressions[idx] = content;
                }

            }
            // Now save the user settings
            try
            {
                UserSettings.Default.Save();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error encountered while saving the expressions:");
                Debug.WriteLine(ex.Message);
            }
        }

        public void Reset()
        {
            try
            {
                UserSettings.Default.Reset();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error encountered while reseting the expressions:");
                Debug.WriteLine(ex.Message);
            }
        }

        internal void SetExpressionName(int currentExpressionIndex, string newName)
        {
            ExpressionsList[currentExpressionIndex].Name = newName;
            BuildExpressionsNameList();
        }
    }
}
