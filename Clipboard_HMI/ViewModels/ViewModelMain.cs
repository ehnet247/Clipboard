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
using Clipboard_HMI.Models;
using Clipboard_HMI.Views;
using System.Diagnostics;

namespace Clipboard_HMI.ViewModels
{
    public class ViewModelMain : ObservableRecipient
    {
        public ObservableCollection<string> ExpressionsNames { get; set; }
        public ObservableCollection<string> ExpressionsContent { get; set; }
        public ICommand CommandOpenConfigView { get; }
        public ICommand CommandExit { get; }
        private ViewMain ViewInstance;
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
                SelectionChanged();
            }
        }

        public ViewModelMain(ViewMain _ViewInstance)
        {
            ViewInstance = _ViewInstance;
            ExpressionsNames = new ObservableCollection<string>();
            ExpressionsContent = new ObservableCollection<string>();
            // Fill-in the expression names list
            ReadExpressionsNames();
            // Fill-in the expression content list
            ReadExpressionsContent();
            CommandOpenConfigView = new RelayCommand(OpenConfigViewMethod);
            CommandExit = new RelayCommand(ExitMethod);
        }

        private void ReadExpressionsNames()
        {
            ExpressionsNames.Clear();
            foreach (string? exprName in UserSettings.Default.ExpressionsNames)
            {
                if (exprName != null)
                    ExpressionsNames.Add(exprName);
            }
        }

        private void ReadExpressionsContent()
        {
            ExpressionsContent.Clear();
            foreach (string? exprContent in UserSettings.Default.Expressions)
            {
                if (exprContent != null)
                {
                    ExpressionsContent.Add(exprContent);
                }
            }
        }

        private void OpenConfigViewMethod()
        {
            ViewConfig viewConfig;
            bool? result = null;
            try
            {
                viewConfig = new ViewConfig();
                ViewModelConfig viewModel = new ViewModelConfig(viewConfig);
                viewConfig.DataContext = viewModel;
                viewConfig.ShowDialog();
                result = viewConfig.DialogResult;
            }
            catch (Exception ex)
            {
               Debug.WriteLine(ex.Message);
            }
            if ((result != null) && (result == true))
            {
                // OK button has been clicked
                UserSettings.Default.Save();
                RefreshDisplay();
            }

        }

        private void ExitMethod()
        {
            ViewInstance.Close();
        }

        private void SelectionChanged()
        {
            // Get the related expression
            string expressionToCopy = string.Empty;
            for (int exprIndex = 0; exprIndex < ExpressionsNames.Count; exprIndex++)
            {
                if (ExpressionsNames[exprIndex] == SelectedExpressionName)
                {
                    CopyToClipboard(ExpressionsContent[exprIndex]);
                    ViewInstance.Close();
                }
            }
        }

        private void CopyToClipboard(string expressionToCopy)
        {
            System.Windows.Clipboard.SetText(expressionToCopy);
        }

        public void RefreshDisplay()
        {
            // Fill-in the expression names list
            ReadExpressionsNames();
            // Fill-in the expression content list
            ReadExpressionsContent();
            OnPropertyChanged(nameof(ExpressionsNames));
            OnPropertyChanged(nameof(ExpressionsContent));
        }
    }
}
