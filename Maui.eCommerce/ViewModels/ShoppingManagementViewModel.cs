// Maui.eCommerce/ViewModels/ShoppingManagementViewModel.cs
using Library.eCommerce.Models;
using Library.eCommerce.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Maui.eCommerce.ViewModels
{
    public class ShoppingManagementViewModel : INotifyPropertyChanged
    {
        private readonly ProductServiceProxy _invSvc = ProductServiceProxy.Current;
        private readonly ShoppingCartService _cartSvc = ShoppingCartService.Current;

        public ItemViewModel? SelectedItem { get; set; }
        public ItemViewModel? SelectedCartItem { get; set; }

        // 4) Cart search text
        private string _cartQuery = string.Empty;
        public string CartQuery
        {
            get => _cartQuery;
            set
            {
                if (_cartQuery != value)
                {
                    _cartQuery = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(ShoppingCart));
                    NotifyPropertyChanged(nameof(CartTotal));
                }
            }
        }

        // 1) Inventory list (unchanged)
        public ObservableCollection<ItemViewModel> Inventory =>
            new ObservableCollection<ItemViewModel>(
                _invSvc.Products
                    .Where(i => i?.Quantity > 0)
                    .Select(m => new ItemViewModel(m)!));

        // 1) Shopping cart list, now filtered by CartQuery
        public ObservableCollection<ItemViewModel> ShoppingCart
        {
            get
            {
                var items = _cartSvc.CartItems
                    .Where(i => i?.Quantity > 0);

                if (!string.IsNullOrWhiteSpace(CartQuery))
                {
                    items = items.Where(i =>
                        i?.Product?.Name
                          .Contains(CartQuery, StringComparison.OrdinalIgnoreCase) == true);
                }

                return new ObservableCollection<ItemViewModel>(
                    items.Select(m => new ItemViewModel(m)!));
            }
        }

        // 3) Combined total
        public decimal CartTotal => _cartSvc.CartItems
            .Where(i => i?.Quantity > 0)
            .Sum(i => (i.Price) * (i.Quantity ?? 0));

        public event PropertyChangedEventHandler? PropertyChanged;
        void NotifyPropertyChanged([CallerMemberName] string prop = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        // Called after Add or Remove to refresh both lists and total
        public void RefreshUX()
        {
            NotifyPropertyChanged(nameof(Inventory));
            NotifyPropertyChanged(nameof(ShoppingCart));
            NotifyPropertyChanged(nameof(CartTotal));
        }

        // 2) Purchase: decrement inventory, add to cart
        public void PurchaseItem()
        {
            if (SelectedItem != null)
            {
                var shouldRefresh = SelectedItem.Model.Quantity >= 1;
                var updated = _cartSvc.AddOrUpdate(SelectedItem.Model); // existing code

                if (updated != null && shouldRefresh)
                    RefreshUX();
            }
        }

        // 3) Return from cart
        public void ReturnItem()
        {
            if (SelectedCartItem != null)
            {
                var shouldRefresh = SelectedCartItem.Model.Quantity >= 1;
                var updated = _cartSvc.ReturnItem(SelectedCartItem.Model); // existing code

                if (updated != null && shouldRefresh)
                    RefreshUX();
            }
        }

        // 4) Search cart simply re-binds ShoppingCart & total
        public void SearchCart()
        {
            NotifyPropertyChanged(nameof(ShoppingCart));
            NotifyPropertyChanged(nameof(CartTotal));
        }
    }
}
