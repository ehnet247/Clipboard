using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Diagnostics;
using Clipboard_HMI.ViewModels;
using Clipboard_HMI.Models;

namespace Clipboard_HMI.ViewModels
{
    public class ViewModelConfig : ObservableRecipient
    {
        public Expressions Expressions { get; set; }
        public ICommand CommandNew { get; }
        public ICommand CommandOk { get; }
        public ICommand CommandCancel { get; }
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
                OnPropertyChanged("SelectedExpressionName");
            }
        }

        private string editingExpressionName;
        public string EditingExpressionName
        {
            get
            {
                return editingExpressionName;
            }
            set
            {
                editingExpressionName = value;
                Expressions.ExpressionsList[CurrentExpressionIndex].Name = value;
                //Expressions.SetExpressionName(CurrentExpressionIndex, value);
                OnPropertyChanged(nameof(EditingExpressionName));
            }
        }

        private string editingExpressionContent;
        public string EditingExpressionContent
        {
            get
            {
                return editingExpressionContent;
            }
            set
            {
                editingExpressionContent = value;
                Expressions.ExpressionsList[CurrentExpressionIndex].Content = value;
                OnPropertyChanged(nameof(EditingExpressionContent));
            }
        }

        private int CurrentExpressionIndex;

        private Window ConfigView;

        private void SetExpressionContent()
        {
            for (int idx = 0; idx < Expressions.ExpressionsList.Count; idx++)
            {
                if (Expressions.ExpressionsList[idx].Name == SelectedExpressionName)
                {
                    EditingExpressionContent = Expressions.ExpressionsList[idx].Content;
                    EditingExpressionName = Expressions.ExpressionsList[idx].Name;
                    CurrentExpressionIndex = idx;
                }
            }
        }

        public ViewModelConfig(Window configWindow)
        {
            ConfigView = configWindow;
            Expressions = new Expressions();
            // Initialize the commands
            CommandNew = new RelayCommand(MethodNew);
            CommandOk = new RelayCommand(MethodSave);
            CommandCancel = new RelayCommand(MethodCancel);
        }

        private void MethodNew()
        {
            var newExpression = new Clipboard_HMI.Models.Expression()
            {
                Name = "New Name",
                Content = "New Content"
            };
            if (newExpression != null)
            {
                Expressions.ExpressionsList.Add(newExpression);
                Expressions.ExpressionsNameList.Add(newExpression.Name);
                OnPropertyChanged(nameof(Expressions.ExpressionsNameList));
            }

        }

        private void MethodSave()
        {
            Expressions.Save();
            try
            {
                ConfigView.Close();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error encountered while closing the Config Window:\r\n"+ex.Message);

            }
        }

        private void MethodCancel()
        {
            try
            {
                ConfigView.Close();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error encountered while closing the Config Window:\r\n" + ex.Message);
            }
        }
    }
}
