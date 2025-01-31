using BrawlmartTest.Models;
using System;

namespace BrawlmartTest
{
    internal class CreateTrinkets
    {
        public static void Run()
        {
            using var context = new Models.MyDbContext();

            var trinket1 = new Product
            {
                Name = "One Ring",
                Color = "Gold",
                Material = "Gold",
                Level = 100,
                Size = null,
                Rarity = "Legendary",
                Details = "A powerful ring that grants invisibility and control over other rings of power.",
                Stock = 1,
                Price = 5000000.0f,
                CategoryId = 3,
                FrontId = null
            };
            context.Add<Product>(trinket1);

            var trinket2 = new Product
            {
                Name = "Arc Reactor",
                Color = "Blue",
                Material = "Palladium",
                Level = 100,
                Size = null,
                Rarity = "Legendary",
                Details = "A powerful energy source created by Tony Stark, capable of powering the Iron Man suit.",
                Stock = 1,
                Price = 6000000.0f,
                CategoryId = 3,
                FrontId = null
            };
            context.Add<Product>(trinket2);

            var trinket3 = new Product
            {
                Name = "Philosopher's Stone",
                Color = "Red",
                Material = "Stone",
                Level = 100,
                Size = null,
                Rarity = "Legendary",
                Details = "A legendary alchemical substance capable of turning base metals into gold and granting immortality.",
                Stock = 1,
                Price = 6000000.0f,
                CategoryId = 3,
                FrontId = null
            };
            context.Add<Product>(trinket3);

            var trinket4 = new Product
            {
                Name = "Amulet of Mara",
                Color = "Gold",
                Material = "Gold",
                Level = 20,
                Size = null,
                Rarity = "Legendary",
                Details = "An amulet that symbolizes the goddess Mara, often used in marriage ceremonies.",
                Stock = 1,
                Price = 1000000.0f,
                CategoryId = 3,
                FrontId = null
            };
            context.Add<Product>(trinket4);

            var trinket5 = new Product
            {
                Name = "Tesseract",
                Color = "Blue",
                Material = "Unknown",
                Level = 100,
                Size = null,
                Rarity = "Legendary",
                Details = "A powerful energy source capable of opening portals to other dimensions.",
                Stock = 1,
                Price = 7000000.0f,
                CategoryId = 3,
                FrontId = null
            };
            context.Add<Product>(trinket5);

            var trinket6 = new Product
            {
                Name = "Invisibility Cloak",
                Color = "Silver",
                Material = "Unknown",
                Level = 100,
                Size = null,
                Rarity = "Legendary",
                Details = "A cloak that renders the wearer completely invisible.",
                Stock = 1,
                Price = 4500000.0f,
                CategoryId = 3,
                FrontId = null
            };
            context.Add<Product>(trinket6);

            var trinket7 = new Product
            {
                Name = "Heart of TARDIS",
                Color = "Blue",
                Material = "Unknown",
                Level = 100,
                Size = null,
                Rarity = "Legendary",
                Details = "A powerful artifact that grants the user control over time and space.",
                Stock = 1,
                Price = 8000000.0f,
                CategoryId = 3,
                FrontId = null
            };
            context.Add<Product>(trinket7);

            var trinket8 = new Product
            {
                Name = "Ethereal Crystal",
                Color = "Purple",
                Material = "Crystal",
                Level = 60,
                Size = null,
                Rarity = "Legendary",
                Details = "A crystal that holds immense magical power, often used by sorcerers to enhance their spells.",
                Stock = 1,
                Price = 3000000.0f,
                CategoryId = 3,
                FrontId = null
            };
            context.Add<Product>(trinket8);

            var trinket9 = new Product
            {
                Name = "Time-Turner",
                Color = "Gold",
                Material = "Gold",
                Level = 80,
                Size = null,
                Rarity = "Legendary",
                Details = "A magical device used to travel back in time for short periods.",
                Stock = 1,
                Price = 4000000.0f,
                CategoryId = 3,
                FrontId = null
            };
            context.Add<Product>(trinket9);

            var trinket10 = new Product
            {
                Name = "Horcrux",
                Color = "Varies",
                Material = "Varies",
                Level = 100,
                Size = null,
                Rarity = "Legendary",
                Details = "An object in which a dark wizard or witch has hidden a fragment of their soul for the purpose of attaining immortality.",
                Stock = 1,
                Price = 5000000.0f,
                CategoryId = 3,
                FrontId = null
            };
            context.Add<Product>(trinket10);

            context.SaveChanges();
        }
    }
}
