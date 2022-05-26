using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Clipboard.ViewModels;
using Clipboard.Models;

namespace Clipboard.ViewModels
{
    public class ViewModelConfig : ObservableRecipient
    {
        public ObservableCollection<string> ExpressionsNames { get; set; }
        public ObservableCollection<string> ExpressionsContent { get; set; }
        //public ICommand CommandExit { get; }
        private string selectedExpressionName;
        public string SelectedExpressionName
        {
            get
            {
                return selectedExpressionName;
            }
            set
            {
                selectedExpressionName = value;
                SetExpressionContent();
            }
        }

        private string selectedExpressionContent;
        public string SelectedExpressionContent
        {
            get
            {
                return selectedExpressionContent;
            }
            set
            {
                selectedExpressionContent = value;
            }
        }

        private void SetExpressionContent()
        {
            for (int idx = 0; idx < ExpressionsNames.Count; idx++)
            {
                if (ExpressionsNames[idx] == SelectedExpressionName)
                {
                    SelectedExpressionContent = ExpressionsContent[idx];
                }
            }
        }

        public ViewModelConfig()
        {
            ExpressionsNames = new ObservableCollection<string>();
            ExpressionsContent = new ObservableCollection<string>();
            // Fill-in the expression names list
            foreach (string exprName in UserSettings.Default.ExpressionsNames)
            {
                if (exprName != null)
                    ExpressionsNames.Add(exprName);
            }
            // Fill-in the expression content list
            foreach (string exprContent in UserSettings.Default.Expressions)
            {

                if (exprContent != null)
                    ExpressionsContent.Add(exprContent);
            }
        }
    }
}
