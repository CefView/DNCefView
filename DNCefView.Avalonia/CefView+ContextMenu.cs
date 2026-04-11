using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives.PopupPositioning;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DNCefView.Avalonia;

public partial class CefView
{
    public static readonly StyledProperty<bool> IsContextMenuEnabledProperty =
    AvaloniaProperty.Register<CefView, bool>(
        nameof(IsContextMenuEnabled), defaultValue: true);

    public bool IsContextMenuEnabled
    {
        get => GetValue(IsContextMenuEnabledProperty);
        set => SetValue(IsContextMenuEnabledProperty, value);
    }

    private bool _isShowingContextMenu;
    private ContextMenu? _contextMenu;
    private List<MenuItemData>? _contextMenuData;
    private bool _contextMenuCommandExecuted;

    static void ClassInitializeContextMenu()
    {
    }

    void InitializeContextMenu()
    {
    }

    bool UI_OnCefBeforeContextMenu(string menuData)
    {
        bool allow = false;

        RunInUIThread(
            () =>
            {
                if (IsContextMenuEnabled)
                {
                    _contextMenuData = ParseMenuData(menuData);
                    _contextMenu = BuildContextMenu(_contextMenuData);
                    allow = true;
                }
                else
                {
                    allow = false;
                }
            },
            block: true);

        return allow;
    }

    void UI_OnCefRunContextMenu(int x, int y)
    {
        RunInUIThread(
            () =>
            {
                if (_contextMenu == null)
                {
                    return;
                }

                _contextMenu.PlacementTarget = this;
                _contextMenu.PlacementRect = new Rect(x, y, 1, 1);
                _contextMenu.Placement = PlacementMode.AnchorAndGravity;
                _contextMenu.PlacementAnchor = PopupAnchor.TopLeft;
                _contextMenu.PlacementGravity = PopupGravity.BottomRight;
                _contextMenu.Open(this);
                _isShowingContextMenu = true;
            },
            block: false);
    }

    void UI_OnCefContextMenuDismissed()
    {
        RunInUIThread(
            () =>
            {
                _contextMenu?.Close();
                _isShowingContextMenu = false;
            },
            block: false);
    }

    private List<MenuItemData> ParseMenuData(string menuData)
    {
        if (string.IsNullOrWhiteSpace(menuData))
        {
            return new List<MenuItemData>();
        }

        try
        {
            return JsonSerializer.Deserialize<List<MenuItemData>>(menuData) ?? new List<MenuItemData>();
        }
        catch
        {
            return new List<MenuItemData>();
        }
    }

    private ContextMenu BuildContextMenu(List<MenuItemData> data)
    {
        _contextMenuCommandExecuted = false;
        var menu = new ContextMenu
        {
            ItemsSource = BuildMenuItems(data)
        };

        menu.Closed += (_, _) =>
        {
            if (!_contextMenuCommandExecuted)
            {
                _cefBrowser?.CancelContextMenu();
            }
        };

        return menu;
    }

    private List<object> BuildMenuItems(List<MenuItemData> data)
    {
        var items = new List<object>();

        foreach (var item in data)
        {
            if (!item.Visible)
            {
                continue;
            }

            switch (item.Type)
            {
                case MenuItemType.Separator:
                    items.Add(new Separator());
                    break;
                case MenuItemType.SubMenu:
                    items.Add(BuildSubMenu(item));
                    break;
                case MenuItemType.Command:
                case MenuItemType.Check:
                case MenuItemType.Radio:
                    items.Add(BuildCommandMenuItem(item));
                    break;
                default:
                    break;
            }
        }

        return items;
    }

    private MenuItem BuildSubMenu(MenuItemData item)
    {
        return new MenuItem
        {
            Header = item.Label,
            IsEnabled = item.Enable,
            ItemsSource = BuildMenuItems(item.SubMenuData)
        };
    }

    private MenuItem BuildCommandMenuItem(MenuItemData item)
    {
        var menuItem = new MenuItem
        {
            Header = item.Label,
            IsEnabled = item.Enable,
            Tag = item.CommandId
        };

        var icon = CreateMenuItemIcon(item);
        if (icon != null)
        {
            menuItem.Icon = icon;
        }

        menuItem.Click += (_, _) =>
        {
            _contextMenuCommandExecuted = true;
            _cefBrowser?.ExecuteContextMenuCommand(item.CommandId);
            _contextMenu?.Close();
        };

        return menuItem;
    }

    private Control? CreateMenuItemIcon(MenuItemData item)
    {
        return item.Type switch
        {
            MenuItemType.Check => new CheckBox
            {
                IsChecked = item.Checked,
                IsHitTestVisible = false
            },
            MenuItemType.Radio => new RadioButton
            {
                IsChecked = item.Checked,
                IsHitTestVisible = false
            },
            _ => null
        };
    }

    private enum MenuItemType
    {
        None = 0,
        Command = 1,
        Check = 2,
        Radio = 3,
        Separator = 4,
        SubMenu = 5
    }

    private sealed class MenuItemData
    {
        [JsonPropertyName("type")]
        public MenuItemType Type { get; set; } = MenuItemType.None;

        [JsonPropertyName("label")]
        public string Label { get; set; } = string.Empty;

        [JsonPropertyName("commandId")]
        public int CommandId { get; set; }

        [JsonPropertyName("enable")]
        public bool Enable { get; set; }

        [JsonPropertyName("visible")]
        public bool Visible { get; set; }

        [JsonPropertyName("checked")]
        public bool Checked { get; set; }

        [JsonPropertyName("groupId")]
        public int GroupId { get; set; } = -1;

        [JsonPropertyName("accelerator")]
        public int Accelerator { get; set; } = -1;

        [JsonPropertyName("subMenuData")]
        public List<MenuItemData> SubMenuData { get; set; } = new();
    }
}
