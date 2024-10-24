namespace MenuSystem;

public class Menu
{
    private string MenuHeader { get; set; }
    private static string _menuDivider = "=================";
    public List<MenuItem> MenuItems { get; set; }

    private MenuItem _menuItemExit = new MenuItem()
    {
        Shortcut = "E",
        Title = "Exit"
    };
    
    private MenuItem _menuItemReturn = new MenuItem()
    {
        Shortcut = "R",
        Title = "Return"
    };
    private MenuItem _menuItemReturnMain = new MenuItem()
    {
        Shortcut = "M",
        Title = "return to Main menu"
    };

    private EMenuLevel _menuLevel { get; set; }

    private bool _isCustomMenu { get; set; }
    public void SetMenuItemAction(string shortCut, Func<string> action)
    {
        var menuItem = MenuItems.Single(m => m.Shortcut == shortCut);
        menuItem.MenuItemAction = action;
    }

    public Menu(EMenuLevel menuLevel, string menuHeader, List<MenuItem> menuItems, bool isCustomMenu = false)
    {
        if (string.IsNullOrWhiteSpace(menuHeader))
        {
            throw new ApplicationException("Menu header cannot be empty.");
        }

        MenuHeader = menuHeader;

        // If menuItems is null or empty, add a default "Return" item
        if (menuItems == null || menuItems.Count == 0)
        {
            menuItems = new List<MenuItem>
            {
                new MenuItem
                {
                    Shortcut = "R",
                    Title = "Return",
                    MenuItemAction = () => "R"
                }
            };
        }

        MenuItems = menuItems;
        _isCustomMenu = isCustomMenu;
        _menuLevel = menuLevel;
    
        // Add return and exit items based on menu level
        if (_menuLevel != EMenuLevel.Main)
        {
            MenuItems.Add(_menuItemReturn);
        }
    
        if (_menuLevel == EMenuLevel.Deep)
        {
            MenuItems.Add(_menuItemReturnMain);
        }

        MenuItems.Add(_menuItemExit);
    }


    public string Run()
    {
        
        Console.Clear();
        do
        {
            var menuItem = DisplayMenuGetUserChoice();
            var menuReturnValue = "";

            if (menuItem.MenuItemAction != null)
            {
                menuReturnValue = menuItem.MenuItemAction();
                if (_isCustomMenu) return menuReturnValue;
            }

            if (menuItem.Shortcut == _menuItemReturn.Shortcut)
            {
                return menuItem.Shortcut;
            }
            
            if (menuItem.Shortcut == _menuItemExit.Shortcut || menuReturnValue == _menuItemExit.Shortcut)
            {
                return _menuItemExit.Shortcut;
            }

            if ((menuItem.Shortcut == _menuItemReturnMain.Shortcut || menuReturnValue == _menuItemReturnMain.Shortcut) && _menuLevel != EMenuLevel.Main)
            {
                return _menuItemReturnMain.Shortcut;
            }

        } while (true);
    }

    private MenuItem DisplayMenuGetUserChoice()
    {
        var userInput = "";

        do
        {
            DrawMenu();

            userInput = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(userInput))
            {
                Console.WriteLine("Please try to select one of the available options.");
                Console.WriteLine();
            }
            else
            {
                userInput = userInput.ToUpper();
                
                foreach (var menuItem in MenuItems)
                {
                    if (menuItem.Shortcut.ToUpper() != userInput) continue;
                    return menuItem;
                }

                Console.WriteLine("Try to choose something from the existing options.... please....");
                Console.WriteLine();
            }
        } while (true);
    }

    private void DrawMenu()
    {
        Console.WriteLine(MenuHeader);
        Console.WriteLine(_menuDivider);

        foreach (var t in MenuItems)
        {
            Console.WriteLine(t);
        }
        
        Console.WriteLine();

        Console.Write(">");
    }
}
