using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;
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
            helper.Events.Input.ButtonPressed += this.ShowExpanded;
        }


        /*********
        ** Private methods
        *********/
        // Class wide variables
        // Collections
        private readonly List<string> _info = new();
        private readonly List<string> _itemInfoShort = new();
        private readonly Dictionary<string, string> _itemInfo = new();
        // Constants
        private const string HeaderName = "Items";
        private const string HeaderValue = "Value";
        // Primitives
        private int _totalSalePrice;
        private int _totalItems;
        private bool _largeStack;
        private bool _largeValue;

        // Naming of the rarities
        private readonly Dictionary<int, string> _qualityMap = new()
        {
            { 0, "Normal" },
            { 1, "Silver" },
            { 2, "Gold" },
            { 4, "Iridium" }
        }; 

        /// <summary>Calculates and creates the list "info" to be used in other functions</summary>
        private void Calculations()
        {
            // Check if the world is loaded and for null references regarding the player or their items
            if (!Context.IsWorldReady || Game1.player == null || Game1.player?.Items == null)
                return;

            // Standard excluded items and categories
            string[] excludedItems = { "Torch", "Wood", "Stone", "Salad" };
            int[] excludedCategories = { -7, -74 }; // Cooking, Seeds/Saplings
            
            // Set starting information
            _info.Clear();
            _itemInfo.Clear();
            _itemInfoShort.Clear();
            _totalSalePrice = 0;
            _totalItems = 0;

            // Get all items in the player inventory
            foreach (var item in Game1.player.Items)
            {
                // Check if the item is a valid object 
                if (item is not StardewValley.Object obj || excludedItems.Contains(item.DisplayName) 
                    || excludedCategories.Contains(item.Category) || obj.sellToStorePrice() == 0) continue;

                // Information about the items
                var value = obj.sellToStorePrice() * obj.Stack;
                _totalItems += obj.Stack;
                _totalSalePrice += value;
    
                // Format each item line
                string quality = qualityName;
                var itemName = $"{obj.Stack}x {obj.DisplayName} ({quality})";
                var itemValue = $"{value} ({obj.sellToStorePrice()} p.p.)";
                _itemInfoShort.Add($"{obj.DisplayName} ({quality}) [{value}]");
                
                // Special cases for formatting
                _largeStack = (obj.Stack >= 10);
                _largeValue = (obj.Stack >= 10 && obj.sellToStorePrice() >= 100);
                
                _itemInfo[itemName] = itemValue;
            }//foreach
        } // Calculations
        
        /// <summary> Creates the output for if no items are found </summary>
        private void NoneFound()
        {
            _info.Clear(); _info.Add("No sellable items in your inventory");
            Game1.drawObjectDialogue(_info.First());
        } // NoneFound

        /// <summary> Creates the format for the short output </summary>
        private void SummaryFormat()
        {
            // Format and add the items to _info
            _info.Add($"{HeaderName,-15}{HeaderValue,26}");
            foreach (var item in _itemInfo)
            {
                var itemName = item.Key;
                var itemValue = item.Value;
                
                // Special cases for formatting
                itemName = itemName.PadRight(33);
                itemValue = (_largeStack) ? item.Key.PadLeft(15) : item.Key.PadLeft(16);
                itemValue = (_largeValue) ? item.Value.PadLeft(17) : item.Value.PadLeft(16);
                _info.Add($"{itemName} {itemValue}"); // Without the per piece and amount info
                
            }//foreach
            _info.Add($"Total sale price: {_totalSalePrice} ({_totalItems} items)");
        } // SummaryFormat
        
        //TODO: Fix the formatting
        /// <summary> Creates the format for the expanded output </summary>
        private void ExpandedFormat()
        {
            // For the console log output
            _info.Add($"{HeaderName,-15}{HeaderValue,26}");
            foreach (var item in _itemInfo) _info.Add($"{item.Key,-33}{item.Value,16}");
            _info.Add($"Total sale price: {_totalSalePrice} ({_totalItems} items)");
            
            // For the in-game dialogue output
            var output = $"Total sale price: {_totalSalePrice} ({_totalItems} items). We  checked these items: ";
            foreach (var item in _itemInfoShort)
            {
                output += $"{item}";
                if (item != _itemInfoShort.Last()) output += ", ";
            }
            Game1.drawObjectDialogue(output);
        } // ExpandedFormat
        
        /// <summary>Shows the short version of the calculated sale prices</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowSummary(object sender, ButtonPressedEventArgs e)
        {
            // Basic checks and initialisations
            if (e.Button != SButton.H) return;
            Calculations();
            this.Monitor.Log($"{Game1.player.Name} pressed {e.Button}.", LogLevel.Debug);
            
            // If we do not find any items we return an appropriate prompt
            if (_totalSalePrice == 0) NoneFound();
            else SummaryFormat(); Game1.drawObjectDialogue(_info.Last());

            // Print out all lines in the info array
            foreach (var t in _info) Monitor.Log(t, LogLevel.Info);
        } // ShowSummary
        
        /// <summary>Shows the expanded version of the calculated sale prices</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowExpanded(object sender, ButtonPressedEventArgs e)
        {
            // Basic checks and initialisations
            if (e.Button != SButton.V) return;
            Calculations();
            this.Monitor.Log($"{Game1.player.Name} pressed {e.Button}.", LogLevel.Debug);
            
            // If we do not find any items we return an appropriate prompt
            if (_totalSalePrice == 0) NoneFound();
            else ExpandedFormat();
            
            // Print out all lines in the info array
            foreach (var t in _info) Monitor.Log(t, LogLevel.Info);
        } // ShowExpanded
    }
}
