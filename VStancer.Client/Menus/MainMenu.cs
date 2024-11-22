﻿using System;
using CitizenFX.Core;
using static CitizenFX.FiveM.Native.Natives;
using static VStancer.Client.Menus.MenuUtilities;
using MenuAPI;
using VStancer.Client.Scripts;

namespace VStancer.Client.Menus
{
    internal class MainMenu : Menu
    {
        private readonly MainScript _script;

        private WheelMenu WheelMenu { get; set; }
        private WheelModMenu WheelModMenu { get; set; }
        private SuspensionMenu SuspensionMenu { get; set; }
        private ClientPresetsMenu ClientPresetsMenu { get; set; }
        private ClientSettingsMenu ClientSettingsMenu { get; set; }

        private MenuItem WheelMenuMenuItem { get; set; }
        private MenuItem WheelModMenuMenuItem { get; set; }
        private MenuItem SuspensionMenuMenuItem { get; set; }
        private MenuItem ClientPresetsMenuMenuItem { get; set; }
        private MenuItem ClientSettingsMenuMenuItem { get; set; }


        internal MainMenu(MainScript script, string name = Globals.ScriptName, string subtitle = "Main Menu") : base(name, subtitle)
        {
            _script = script;

            _script.ToggleMenuVisibility += new System.EventHandler((sender, args) =>
            {
                var currentMenu = MenuController.MainMenu;

                if (currentMenu == null)
                    return;

                currentMenu.Visible = !currentMenu.Visible;
            });

            MenuController.MenuAlignment = MenuController.MenuAlignmentOption.Right;
            MenuController.MenuToggleKey = (CitizenFX.FiveM.Control)_script.Config.ToggleMenuControl;
            MenuController.EnableMenuToggleKeyOnController = false;
            MenuController.DontOpenAnyMenu = true;
            MenuController.MainMenu = this;

            if (_script.WheelScript != null)
                WheelMenu = _script.WheelScript.Menu;

            if (_script.WheelModScript != null)
            {
                WheelModMenu = _script.WheelModScript.Menu;
                WheelModMenu.PropertyChanged += (sender, args) => UpdateWheelModMenuMenuItem();
            }

            if (_script.SuspensionScript != null)
                SuspensionMenu = _script.SuspensionScript.Menu;

            if (_script.ClientPresetsScript != null)
                ClientPresetsMenu = _script.ClientPresetsScript.Menu;

            if (_script.ClientSettingsScript != null)
                ClientSettingsMenu = _script.ClientSettingsScript.Menu;

            Update();
        }

        internal void Update()
        {
            ClearMenuItems();

            MenuController.Menus.Clear();
            MenuController.AddMenu(this);

            if (WheelMenu != null)
            {
                WheelMenuMenuItem = new MenuItem("Wheel Menu", "The menu to edit main properties.")
                {
                    Label = "→→→"
                };

                AddMenuItem(WheelMenuMenuItem);

                MenuController.AddSubmenu(this, WheelMenu);
                MenuController.BindMenuItem(this, WheelMenu, WheelMenuMenuItem);
            }

            if (WheelModMenu != null)
            {
                WheelModMenuMenuItem = new MenuItem("Wheel Mod Menu")
                {
                    Label = "→→→"
                };
                UpdateWheelModMenuMenuItem();

                AddMenuItem(WheelModMenuMenuItem);

                MenuController.AddSubmenu(this, WheelModMenu);
                MenuController.BindMenuItem(this, WheelModMenu, WheelModMenuMenuItem);
            }

            if (SuspensionMenu != null)
            {
                SuspensionMenuMenuItem = new MenuItem("Suspension Menu", "The menu to suspension properties.")
                {
                    Label = "→→→"
                };
                AddMenuItem(SuspensionMenuMenuItem);

                MenuController.AddSubmenu(this, SuspensionMenu);
                MenuController.BindMenuItem(this, SuspensionMenu, SuspensionMenuMenuItem);
            }

            if (ClientPresetsMenu != null)
            {
                ClientPresetsMenuMenuItem = new MenuItem("Client Presets Menu", "The menu to manage the presets saved by you.")
                {
                    Label = "→→→"
                };

                AddMenuItem(ClientPresetsMenuMenuItem);

                MenuController.AddSubmenu(this, ClientPresetsMenu);
                MenuController.BindMenuItem(this, ClientPresetsMenu, ClientPresetsMenuMenuItem);
            }

            if (ClientSettingsMenu != null)
            {
                ClientSettingsMenuMenuItem = new MenuItem("Client Settings Menu", "The menu to manage your own settings.")
                {
                    Label = "→→→"
                };

                AddMenuItem(ClientSettingsMenuMenuItem);

                MenuController.AddSubmenu(this, ClientSettingsMenu);
                MenuController.BindMenuItem(this, ClientSettingsMenu, ClientSettingsMenuMenuItem);
            }
        }

        internal bool HideMenu
        {
            get => MenuController.DontOpenAnyMenu;
            set
            {
                MenuController.DontOpenAnyMenu = value;
            }
        }

        internal bool LeftAlignment
        {
            get => MenuController.MenuAlignment == MenuController.MenuAlignmentOption.Left;
            set
            {
                MenuController.MenuAlignment = value ? MenuController.MenuAlignmentOption.Left : MenuController.MenuAlignmentOption.Right;
            }
        }

        private void UpdateWheelModMenuMenuItem()
        {
            if (WheelModMenuMenuItem == null)
                return;

            var enabled = WheelModMenu != null ? WheelModMenu.Enabled : false;

            WheelModMenuMenuItem.Enabled = enabled;
            WheelModMenuMenuItem.RightIcon = enabled ? MenuItem.Icon.NONE : MenuItem.Icon.LOCK;
            WheelModMenuMenuItem.Label = enabled ? "→→→" : string.Empty;
            WheelModMenuMenuItem.Description = enabled ? "The menu to edit custom wheel properties." : "Install a custom wheel to access to this menu.";
        }
    }
}
