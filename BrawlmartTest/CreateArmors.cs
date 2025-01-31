using BrawlmartTest.Models;
using System;

namespace BrawlmartTest
{
    internal class CreateArmors
    {
        public static void Run()
        {
            using var context = new Models.MyDbContext();

            var armor1 = new Product
            {
                Name = "Mithril Armor",
                Color = "Silver",
                Material = "Mithril",
                Level = 50,
                Size = "Human",
                Rarity = "Legendary",
                Details = "A lightweight and incredibly strong armor, providing excellent protection without hindering movement.",
                Stock = 1,
                Price = 2000000.0f,
                CategoryId = 2,
                FrontId = null
            };
            context.Add<Product>(armor1);

            var armor2 = new Product
            {
                Name = "Dragon Scale Armor",
                Color = "Green",
                Material = "Dragon Scales",
                Level = 60,
                Size = "Human",
                Rarity = "Legendary",
                Details = "Armor made from the scales of a dragon, offering unparalleled protection and resistance to fire.",
                Stock = 1,
                Price = 2500000.0f,
                CategoryId = 2,
                FrontId = null
            };
            context.Add<Product>(armor2);

            var armor3 = new Product
            {
                Name = "Elven Armor",
                Color = "Green",
                Material = "Elven Steel",
                Level = 40,
                Size = "Elf",
                Rarity = "Legendary",
                Details = "A beautifully crafted armor, providing excellent protection and enhanced agility.",
                Stock = 1,
                Price = 1800000.0f,
                CategoryId = 2,
                FrontId = null
            };
            context.Add<Product>(armor3);

            var armor4 = new Product
            {
                Name = "Knight's Armor",
                Color = "Silver",
                Material = "Steel",
                Level = 55,
                Size = "Human",
                Rarity = "Legendary",
                Details = "A heavy and durable armor, providing exceptional protection and durability.",
                Stock = 1,
                Price = 2200000.0f,
                CategoryId = 2,
                FrontId = null
            };
            context.Add<Product>(armor4);

            var armor5 = new Product
            {
                Name = "Phoenix Feather Robe",
                Color = "Red",
                Material = "Phoenix Feathers",
                Level = 45,
                Size = "Elf",
                Rarity = "Legendary",
                Details = "A robe made from phoenix feathers, providing magical protection and the ability to regenerate health.",
                Stock = 1,
                Price = 2400000.0f,
                CategoryId = 2,
                FrontId = null
            };
            context.Add<Product>(armor5);

            var armor6 = new Product
            {
                Name = "Celestial Armor",
                Color = "White",
                Material = "Celestial Steel",
                Level = 70,
                Size = "Human",
                Rarity = "Legendary",
                Details = "A divine armor, providing unmatched protection and resistance to all forms of damage.",
                Stock = 1,
                Price = 3000000.0f,
                CategoryId = 2,
                FrontId = null
            };
            context.Add<Product>(armor6);

            var armor7 = new Product
            {
                Name = "Shadow Armor",
                Color = "Black",
                Material = "Shadow Silk",
                Level = 50,
                Size = "Human",
                Rarity = "Legendary",
                Details = "An armor that blends with the shadows, providing excellent stealth and protection.",
                Stock = 1,
                Price = 2000000.0f,
                CategoryId = 2,
                FrontId = null
            };
            context.Add<Product>(armor7);

            var armor8 = new Product
            {
                Name = "Titanium Armor",
                Color = "Gray",
                Material = "Titanium",
                Level = 65,
                Size = "Gnome",
                Rarity = "Legendary",
                Details = "A highly durable and strong armor, providing excellent protection against physical attacks.",
                Stock = 1,
                Price = 2600000.0f,
                CategoryId = 2,
                FrontId = null
            };
            context.Add<Product>(armor8);

            var armor9 = new Product
            {
                Name = "Vampire Armor",
                Color = "Black",
                Material = "Vampire Leather",
                Level = 55,
                Size = "Gnome",
                Rarity = "Legendary",
                Details = "An armor that grants the wearer enhanced strength and the ability to drain life from enemies.",
                Stock = 1,
                Price = 2300000.0f,
                CategoryId = 2,
                FrontId = null
            };
            context.Add<Product>(armor9);

            var armor10 = new Product
            {
                Name = "Necromancer Robe",
                Color = "Purple",
                Material = "Dark Silk",
                Level = 60,
                Size = "Human",
                Rarity = "Legendary",
                Details = "A robe that enhances the wearer's necromantic powers and provides protection against magical attacks.",
                Stock = 1,
                Price = 2500000.0f,
                CategoryId = 2,
                FrontId = null
            };
            context.Add<Product>(armor10);

            context.SaveChanges();
        }
    }
}
