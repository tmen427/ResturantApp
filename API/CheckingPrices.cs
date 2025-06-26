namespace API;

public static class CheckingPrices
{
    public static decimal CheckMenuItemPricesFromStatic(string itemName)
    {
        switch (itemName)
        {
            case "Egg Roll Platter":
                return 14.95m;
            case "Papaya Salad":
                return 8.95m;
            case "Tofu":
                return 12.95m;
            case "Caesar Salad":
                return  8.95m;
            case "Chopped Beef":
                return 9.95m;
            case "Veggie Platter":
                return  8.95m;
                
            default: throw new Exception("That is not a valid menu item- " + itemName);
        }
    }
}