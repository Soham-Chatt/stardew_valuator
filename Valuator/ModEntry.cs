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
            helper.Events.Input.ButtonPressed += this.ShowSummary;
            helper.Events.Input.ButtonPressed += this.ShowFull;
        }


        /*********
        ** Private methods
        *********/
        // Class wide variables
        private List<string> info = new();
        private int totalSalePrice;
        private int totalItems;
        // Definition of the rarities
        Dictionary<int, string> qualityMap = new()
        {
            { 0, "Normal" },
            { 1, "Silver" },
            { 2, "Gold" },
            { 3, "Iridium" }
        }; 

        
        /// <summary>Calculates and creates the string info to be used in later functions</summary>
        private void Calculations()
        {
            // Check if the world is loaded and for null references
            if (!Context.IsWorldReady || Game1.player == null || Game1.player?.Items == null)
                return;

            // Standard excluded items
            string[] excludedItems = { "Torch", "Wood", "Stone", "Salad" };
            
            // Set starting information
            info.Clear();
            totalSalePrice = 0;
            totalItems = 0;

            // Headers
            var headerName = "Items".PadRight(15);
            var headerValue = "Value".PadLeft(27);
            info.Add($"{headerName}{headerValue}");
            
            // Get all items in the player inventory
            foreach (var item in Game1.player.Items)
            {
                // Check if the item is a valid object 
                if (item is not StardewValley.Object obj || excludedItems.Contains(item.DisplayName)) continue;

                // Information about the items
                totalSalePrice += item.salePrice();
                totalItems += obj.Stack;
                var value = obj.sellToStorePrice() * obj.Stack;

                // Format each item line
                var itemValue = (obj.Stack > 1)  ? $"{value} ({obj.sellToStorePrice()} p.p.)" : $"{value}";
                var itemName = $"{obj.Stack}x {obj.DisplayName} ({qualityMap[obj.Quality]})";
                
                // Special cases for formatting
                itemName = (obj.Stack > 1) ? itemName.PadRight(33) : itemName.PadRight(25);
                itemValue = (obj.Stack >= 10) ? itemValue.PadLeft(15) : itemValue.PadLeft(16);
                itemValue = (obj.Stack >= 10 && obj.sellToStorePrice() >= 100) ? itemValue.PadLeft(17) : itemValue.PadLeft(16);
                
                // Add the string into the array
                info.Add($"{itemName}{itemValue}");
            }

            info.Add($"Total sale price: {totalSalePrice} ({totalItems} items)");
        }

        /// <summary>Shows the short version of the calculated sale prices</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowSummary(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button != SButton.H) return;
            Calculations();
            this.Monitor.Log($"{Game1.player.Name} pressed {e.Button}.", LogLevel.Debug);
            
            Game1.drawObjectDialogue(totalSalePrice == 0
                ? "No sellable items in your inventory"
                : $"Total sale price: {totalSalePrice} ({totalItems} items & {info.Count}) objects");
            
            // Print out all lines in the array
            foreach (var t in info)
            {
                Monitor.Log(t, LogLevel.Info);
            }
        }

        private void ShowFull(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button != SButton.F) return;
            Calculations();
            this.Monitor.Log($"{Game1.player.Name} pressed {e.Button}.", LogLevel.Debug);
            if (totalSalePrice == 0)
            {
                Game1.drawObjectDialogue("No sellable items in your inventory");
            }
            else
            {
                var output = "";
                foreach (var t1 in info)
                {
                    output += t1 + "\n";
                }
                Game1.drawObjectDialogue(output);
            }
        }
    }
}