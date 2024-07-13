<h1>Mod to list sellable items in Stardew Valley game inventory</h1>

<p>This code creates a mod for the Stardew Valley game, which lists all sellable items in the player's inventory and their 
corresponding sale price. It can be run using SMAPI. The code is written in C#. </p>
<h3>Requirements</h3>

    SMAPI 3.0 or later
    Stardew Valley 1.5 or later

<h2>How it works</h2>

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
