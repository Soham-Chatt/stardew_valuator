using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace ClassLibrary1
{
    /// <summary>The mod entry point.</summary>
    internal sealed class ModEntry : Mod
    {
        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            helper.Events.Input.ButtonPressed += this.OnButtonPressed;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            // ignore if player hasn't loaded a save yet or doesnt press the H button
            if (!Context.IsWorldReady)
                return;
            if (e.Button != SButton.H)
                return;
            
            // check for null references
            if (Game1.player == null || Game1.player?.Items == null)
                return;
            
            // Get all items in the player inventory
            
            int totalSalePrice = 0;
            int totalItems = 0;
            string[] excludeditems = { "Torch", "Wood", "Stone", "Salad" };
            string HeaderName = "Items".PadRight(15);
            string HeaderValue = "Value".PadLeft(22);
            string info = $"\n{HeaderName}{HeaderValue}\n";
            foreach (var item in Game1.player.Items)
            {
                if (item is StardewValley.Object obj)
                {
                    // Exclude some common items
                    if (excludeditems.Contains(item.DisplayName))
                        continue;
                    
                    // Information about the items
                    totalSalePrice += item.salePrice();
                    totalItems += obj.Stack;
                    var value = obj.sellToStorePrice() * obj.Stack;
                    Dictionary<int, string> qualityMap = new Dictionary<int, string>()
                    {
                        { 0, "Normal" },
                        { 1, "Silver" },
                        { 2, "Gold" },
                        { 3, "Iridium" }
                    }; 
                    
                    // Formatting for the items
                    string itemValue = (obj.Stack > 1)  ? $"{value} ({obj.sellToStorePrice()} pp)" : $"{value}";
                    int sellPriceLength = obj.sellToStorePrice().ToString().Length;
                    int paddingLength = 18 - sellPriceLength;
                    string itemName = $"{obj.Stack}x {obj.DisplayName} ({qualityMap[obj.Quality]})";
                    itemName = (obj.Stack > 1) ? itemName.PadRight(33) : itemName.PadRight(25);
                    itemValue = (obj.Stack >= 10) ? itemValue.PadLeft(15) : itemValue.PadLeft(16);
                    info += $"{itemName}{itemValue}\n";
                }
            }
            info += $"\nTotal sale price: {totalSalePrice} ({totalItems} items)";
            if (totalSalePrice == 0)
            {
                info = "No sellable items in your inventory";
                Game1.drawObjectDialogue(info);
            }
            else
            {
                Game1.drawObjectDialogue($"Total sale price items: {totalSalePrice} ({totalItems} items)");
            }
            Monitor.Log(info, LogLevel.Info);

            // print button presses to the console window
            this.Monitor.Log($"{Game1.player.Name} pressed {e.Button}.", LogLevel.Debug);
        }
    }
}