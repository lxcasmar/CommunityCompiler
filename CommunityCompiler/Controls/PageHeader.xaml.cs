using CommunityCompiler.Models;
using Microsoft.Maui.Controls;

namespace CommunityCompiler.Controls
{
    public partial class PageHeader : ContentView
    {
        public string CurUserName
        {
            get
            {
                return UserState._CurUserName;
            }
            set
            {
                UserState._CurUserName = value;
            }
        }

        public PageHeader()
        {
            InitializeComponent();
            BindingContext = this;
        }

    }
}