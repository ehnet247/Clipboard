﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Clipboard.Views
{
    /// <summary>
    /// Interaction logic for ViewMain.xaml
    /// </summary>
    public partial class ViewMain : Window
    {
        public ViewMain(ViewModels.ViewModelMain viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        public ViewMain()
        {
            InitializeComponent();
        }
    }
}
