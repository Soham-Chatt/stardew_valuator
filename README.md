<h1>Mod to list sellable items in Stardew Valley game inventory</h1>

<p>This code creates a mod for the Stardew Valley game, which lists all sellable items in the player's inventory and their 
corresponding sale price. It can be run using SMAPI. The code is written in C#. </p>
<h3>Requirements</h3>

    SMAPI 3.0 or later
    Stardew Valley 1.5 or later

<h2>How it works</h2><hr style="height: 3px;">

When the mod is loaded, it listens to the ButtonPressed event. When the player presses the button bound to the mod, 
the mod creates a list of all sellable items in the player's inventory and their sale price. The mod then formats this 
information in a short output.

The short output lists the total sale value and the amount of items it counted. All items in the categories cooking and 
seeds are excluded in the calculation.
<h3>Installation</h3>
<ol>
    <li> Install SMAPI.</li>
    <li> Extract the contents of the downloaded archive to the Stardew Valley/Mods folder. </li>
    <li> Put the .dll and manifest of the mod in one folder </li>
    <li> Launch the game using SMAPI. </li>
</ol>

You can also check out the <a href="https://stardewvalleywiki.com/Modding:Player_Guide/Getting_Started#Install_mods">
Stardew Valley Wiki</a> if you are having trouble installing.

<h3>Usage</h3>
The mod is triggered by pressing a button bound to it. By default, this button is set to H.

<h2>Code overview</h2><hr style="height: 3px;">
<h3>ModEntry class</h3>

    Entry method: This is the mod's entry point, where it sets up the event listeners and binds the mod to a button press event.
    Calculations method: This method calculates the information to be displayed by the mod.
    NoneFound method: This method creates the output for when no sellable items are found in the player's inventory.
    SummaryFormat method: This method formats the output for the short list of sellable items.
    WIP: ExpandedFormat method: This method formats the output for the expanded list of sellable items.

<h3>Variables</h3>

    _info: A list of strings that stores the information to be displayed.
    _itemInfoShort: A list of strings that stores the names of the items to be displayed in the expanded format.
    _itemInfo: A dictionary that stores the information about each sellable item, such as its name, quality, and sale price.
    _totalSalePrice: An integer that stores the total sale price of all sellable items.
    _totalItems: An integer that stores the total number of sellable items.
    _largeStack: A boolean that is true if the number of items the player has is greater than or equal to 10.
    _largeValue: A boolean that is true if the sale price per item is greater than or equal to 100.
    _qualityMap: A dictionary that maps the quality of an item to its corresponding string representation.

<h2>Future updates</h2><hr style="height: 3px;">
<h3 style="font-weight:normal;">
    <ul>
        <li> Expanded output format in-game in the form of a list (comparable to the day recap box) </li>
        <li> Ability to exclude items/categories via in-game buttons on the go </li>
        <li> Ability to select only certain items in the inventory </li>
        <li> Ability to choose an amount after which the mod will calculate the most efficient and best way to
              reach that amount</li>
    </ul>
</h3>