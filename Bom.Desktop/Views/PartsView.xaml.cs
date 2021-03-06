﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Bom.Desktop.ViewModels;
using Core.Common.UI.Core;

namespace Bom.Desktop.Views
{
    /// <summary>
    /// Interaction logic for PartsView.xaml
    /// </summary>
    public partial class PartsView : UserControlViewBase
    {
        public PartsView()
        {
            InitializeComponent();
        }

        protected override void OnUnwireViewModelEvents(ViewModelBase viewModel)
        {
            PartsViewModel vm = viewModel as PartsViewModel;
            if (vm != null)
            {
                vm.ConfirmDelete -= OnConfirmDelete;
                vm.ErrorOccured -= OnErrorOccured;
                vm.OpenEditPartWindow -= OnOpenEditPartWindow;
            }
        }

        protected override void OnWireViewModelEvents(ViewModelBase viewModel)
        {
            PartsViewModel vm = viewModel as PartsViewModel;
            if (vm != null)
            {
                vm.ConfirmDelete += OnConfirmDelete;
                vm.ErrorOccured += OnErrorOccured;
                vm.OpenEditPartWindow += OnOpenEditPartWindow;
            }
        }

        void OnConfirmDelete(object sender, CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this part?", "Confirm Delete",
                                                      MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
                e.Cancel = true;
        }

        void OnErrorOccured(object sender, Core.Common.ErrorMessageEventArgs e)
        {
            MessageBox.Show(e.ErrorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        void OnOpenEditPartWindow(object sender, EditPartViewModel e)
        {
            EditPartWindow editPartWindow = new EditPartWindow(e);
            editPartWindow.Show();
        }
    }
}
