using DevExpress.XamarinForms.DataGrid;
using Xamarin.Forms;

namespace XpoPrism.Views
{
    public partial class ContactList : ContentPage
    {
        public ContactList()
        {
            InitializeComponent();
        }
        private void Grid_Tap(object sender, DataGridGestureEventArgs e)
        {
            if (e.Item != null)
            {
                var editForm = new EditFormPage(grid, grid.GetRow(e.RowHandle).Item);
                Navigation.PushAsync(editForm);
            }
        }
    }
}