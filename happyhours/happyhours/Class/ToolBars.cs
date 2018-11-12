using happyhours.Vues;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace happyhours.Class
{
    public class ToolBars
    {
        public void MenuCarte(CartePage thisLocal, INavigation Navigation)
        {
            ToolbarItem MenuAjouter = new ToolbarItem
            {
                Text = "Ajouter",
                Priority = 0,
                Order = ToolbarItemOrder.Primary
            };
            MenuAjouter.Clicked += (sender, EventArgs) => { Navigation.PushAsync(new AjouterElement()); };
            thisLocal.ToolbarItems.Add(MenuAjouter);

            ToolbarItem MenuNavInformation = new ToolbarItem
            {
                Text = "Informations",
                Priority = 0,
                Order = ToolbarItemOrder.Primary
            };
            MenuNavInformation.Clicked += (sender, EventArgs) => { Navigation.PushAsync(new About()); };

            thisLocal.ToolbarItems.Add(MenuNavInformation);
            
            // return MenuNavHistorique;
        }

        public void MenuMainPage(MainPage thisLocal, INavigation Navigation)
        {
            ToolbarItem MenuNavCarte = new ToolbarItem
            {
                Text = "Carte",
                Priority = 0,
                Order = ToolbarItemOrder.Primary
            };
            MenuNavCarte.Clicked += (sender, EventArgs) => { Navigation.PushAsync(new CartePage()); };
            thisLocal.ToolbarItems.Add(MenuNavCarte);

            ToolbarItem MenuAjouter = new ToolbarItem
            {
                Text = "Ajouter",
                Priority = 0,
                Order = ToolbarItemOrder.Primary
            };
            MenuAjouter.Clicked += (sender, EventArgs) => { Navigation.PushAsync(new AjouterElement()); };
            thisLocal.ToolbarItems.Add(MenuAjouter);


            ToolbarItem MenuNavInformation = new ToolbarItem
            {
                Text = "Informations",
                Priority = 0,
                Order = ToolbarItemOrder.Primary
            };
            MenuNavInformation.Clicked += (sender, EventArgs) => { Navigation.PushAsync(new About()); };

            thisLocal.ToolbarItems.Add(MenuNavInformation);

            // return MenuNavHistorique;
        }
    }
}
