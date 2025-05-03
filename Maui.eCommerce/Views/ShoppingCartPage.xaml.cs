// Maui.eCommerce/Views/ShoppingCartPage.xaml.cs
using Maui.eCommerce.ViewModels;
using Microsoft.Maui.Controls;

namespace Maui.eCommerce.Views
{
    public partial class ShoppingCartPage : ContentPage
    {
        ShoppingCartViewModel ViewModel => BindingContext as ShoppingCartViewModel;

        public ShoppingCartPage()
        {
            InitializeComponent();

            // 1) Set the VM as our BindingContext
            BindingContext = new ShoppingCartViewModel();
        }

        // 2) When the page comes into view, refresh the cart
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // This triggers your RefreshCommand → LoadCart()
            ViewModel.RefreshCommand.Execute(null);
        }
    }
}
