using Kvota.Interfaces;
using Kvota.Models.Products;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace Kvota.Components.Admin
{
    partial class PaginationComponent
    {
        private int _totalPg;
        private int _quantityInPage = 2;
        private int _currentPageNumber=1;
        private static IEnumerable<Product>? _pagedList;
        [Parameter]
        public IEnumerable<Product> Products { get; set; } = null!;
        [Parameter]
        public EventCallback<IEnumerable<Product>> ProductPgListCallback { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            OnPageChanged(_currentPageNumber);


            //SearchString 

        }
        //protected override async Task OnParametersSetAsync()
        protected override void OnAfterRender(bool firstRender)
        {

            OnPageChanged(_currentPageNumber);


            //SearchString 

        }
        private async void OnPageChanged(int newPageNumber)
        {
            _currentPageNumber = newPageNumber;
            _pagedList = Products.Skip((newPageNumber - 1) * _quantityInPage).Take(_quantityInPage);
            _totalPg = (int)Math.Ceiling((double)Products.Count() / _quantityInPage);
            await ProductPgListCallback.InvokeAsync(_pagedList);

        }
    }
}
