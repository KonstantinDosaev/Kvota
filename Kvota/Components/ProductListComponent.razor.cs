using BlazorBootstrap;
using Kvota.Constants;
using Kvota.Models;
using Kvota.Models.Content;
using Kvota.Models.Products;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Kvota.Components
{
    partial class ProductListComponent
    {
        [Parameter] public IEnumerable<Product>? ProductList { get; set; }
        [Parameter] public Product? Product { get; set; }
        [Parameter] public List<ProductInOrder>? ProductInOrderList { get; set; }
        [Parameter] public EventCallback ReloadListCallback { get; set; }
        private Modal? _modalProductCard;
        private Guid IdCurrent { get; set; }
        private ContactsModel Contacts { get; set; } = null!;

        private int _quantity { get; set; }
        private bool _visibleProductCard;
        protected override void OnInitialized()
        {
           
        }

        protected override void OnParametersSet()
        {
            if (ProductInOrderList == null || !ProductInOrderList.Any()) return;
            var idList = ProductInOrderList.Select(w => w.Id);
            if (idList != null && !idList.Contains(Product!.Id) || idList == null) return;

            _quantity = ProductInOrderList.FirstOrDefault(w => w.Id == Product!.Id)!.Quantity;
        }

        private void GetModalCard(Guid id)
        {
            IdCurrent = id;
            _visibleProductCard = true;

        }
    
        private async void OnDropdownShowingAsync()
        {
            if (Contacts != null) return;
            Contacts = await ContactSerialize.DeSerialize($"{Links.RootPath}/{Links.ContactsJson}");
            await InvokeAsync(StateHasChanged);

        }
        private async Task AddProductToOrder(Guid productId,int quantity)
        {
            ProductInOrderList ??= new List<ProductInOrder>();
            if (!ProductInOrderList.Select(s => s.Id).Contains(productId))
            {
                if (quantity == 0)
                    return;
                
                ProductInOrderList.Add(new ProductInOrder() { Id = productId, Quantity = quantity });
            }
               
            else
            {
                if (quantity == 0)
                {
                    await DeleteProductFromOrder(productId);
                    return;
                }
                ProductInOrderList[ProductInOrderList.
                    IndexOf(ProductInOrderList.FirstOrDefault(f=>f.Id==productId)!)].Quantity=quantity;
            }
            await LocalStorage.SetAsync("OrderList", ProductInOrderList);
        }
        private async Task DeleteProductFromOrder(Guid productId)
        {
            _quantity = 0;
            if (ProductInOrderList != null)
            {
                ProductInOrderList.Remove(ProductInOrderList.FirstOrDefault(f => f.Id == productId)!);
               await LocalStorage.SetAsync("OrderList", ProductInOrderList);

               await ReloadListCallback.InvokeAsync();
            }
        }
        [Inject] private IDialogService? DialogService { get; set; }
        private async Task NotAuthClick()
        {
            var options = new DialogOptions() { ClassBackground = "my-custom-class", };
            var result = await DialogService!.ShowMessageBox(
                "Внимание",
                "Для того, чтобы составлять заявки. Войдите или зарегистрируйтесь!",
                yesText: "Войти!  ", cancelText: "  Закрыть", options: options);

            if (result != null && (bool)result)
            {
                ImpNavigationManager.NavigateTo("/login",true);
            }
        }
    }
}
