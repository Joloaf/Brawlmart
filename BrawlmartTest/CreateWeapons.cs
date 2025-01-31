using BrawlmartTest.Models;
using System;

namespace BrawlmartTest
{
    internal class CreateWeapons
    {
        public static void Run()
        {
            using var context = new Models.MyDbContext();

            var weapon1 = new Product
            {
                Name = "The Sword of a Thousand Truths",
                Color = "Gold",
                Material = "Divine Steel",
                Level = 40,
                Size = "Human",
                Rarity = "Legendary",
                Details = "A sword of unparalleled power, capable of delivering devastating blows and dealing massive holy damage. It's said to be a weapon of unlimited truth, one that only the most dedicated adventurers can wield.",
                Stock = 1,
                Price = 1000000.0f,
                CategoryId = 1,
                FrontId = 1
            };
            context.Add<Product>(weapon1);

            var weapon2 = new Product
            {
                Name = "Excalibur",
                Color = "Silver",
                Material = "Mithril",
                Level = 60,
                Size = "Human",
                Rarity = "Legendary",
                Details = "A magically imbued sword that grants the wielder unparalleled strength and the ability to cut through anything. It's said to be unbreakable and shines with divine light.",
                Stock = 1,
                Price = 2000000.0f,
                CategoryId = 1,
                FrontId = 2
            };
            context.Add<Product>(weapon2);

            var weapon3 = new Product
            {
                Name = "Mjölnir",
                Color = "Silver",
                Material = "Uru metal",
                Level = 50,
                Size = "Elf",
                Rarity = "Legendary",
                Details = "A mighty warhammer, capable of summoning storms, delivering devastating blows, and returning to the wielder. Only those deemed worthy can wield its full power.",
                Stock = 1,
                Price = 1500000.0f,
                CategoryId = 1,
                FrontId = 3
            };
            context.Add<Product>(weapon3);

            var weapon4 = new Product
            {
                Name = "Andúril",
                Color = "Silver",
                Material = "Steel",
                Level = 45,
                Size = "Human",
                Rarity = "Legendary",
                Details = "The reforged sword of Narsil, wielded by Aragorn. It glows with a bright flame and is a symbol of hope and leadership.",
                Stock = 1,
                Price = 1200000.0f,
                CategoryId = 1,
                FrontId = null
            };
            context.Add<Product>(weapon4);

            var weapon5 = new Product
            {
                Name = "Glamdring",
                Color = "Blue",
                Material = "Elven Steel",
                Level = 35,
                Size = "Human",
                Rarity = "Legendary",
                Details = "The sword of Gandalf, known as the Foe-hammer. It glows blue in the presence of orcs and is a powerful weapon against evil.",
                Stock = 1,
                Price = 1100000.0f,
                CategoryId = 1,
                FrontId = null
            };
            context.Add<Product>(weapon5);

            var weapon6 = new Product
            {
                Name = "Sting",
                Color = "Blue",
                Material = "Elven Steel",
                Level = 20,
                Size = "Hobbit",
                Rarity = "Legendary",
                Details = "A small sword wielded by Frodo Baggins. It glows blue in the presence of orcs and is exceptionally sharp.",
                Stock = 1,
                Price = 900000.0f,
                CategoryId = 1,
                FrontId = null
            };
            context.Add<Product>(weapon6);

            var weapon7 = new Product
            {
                Name = "Masamune",
                Color = "Black",
                Material = "Unknown",
                Level = 55,
                Size = "Human",
                Rarity = "Legendary",
                Details = "A long katana wielded by Sephiroth. It is known for its incredible length and sharpness, capable of cutting through almost anything.",
                Stock = 1,
                Price = 1800000.0f,
                CategoryId = 1,
                FrontId = null
            };
            context.Add<Product>(weapon7);

            var weapon8 = new Product
            {
                Name = "Buster Sword",
                Color = "Steel",
                Material = "Steel",
                Level = 50,
                Size = "Human",
                Rarity = "Legendary",
                Details = "A massive sword wielded by Cloud Strife. It is known for its size and power, capable of delivering devastating blows.",
                Stock = 1,
                Price = 1600000.0f,
                CategoryId = 1,
                FrontId = null
            };
            context.Add<Product>(weapon8);

            var weapon9 = new Product
            {
                Name = "Dragon Slayer",
                Color = "Black",
                Material = "Iron",
                Level = 70,
                Size = "Human",
                Rarity = "Legendary",
                Details = "A massive sword wielded by Guts. It is known for its immense size and weight, capable of slaying dragons and other large creatures.",
                Stock = 1,
                Price = 2500000.0f,
                CategoryId = 1,
                FrontId = null
            };
            context.Add<Product>(weapon9);

            var weapon10 = new Product
            {
                Name = "Lightsaber",
                Color = "Varies",
                Material = "Plasma",
                Level = 60,
                Size = "Human",
                Rarity = "Legendary",
                Details = "A weapon used by Jedi and Sith. It is a blade of pure plasma, capable of cutting through almost anything and deflecting blaster bolts.",
                Stock = 1,
                Price = 2200000.0f,
                CategoryId = 1,
                FrontId = null
            };
            context.Add<Product>(weapon10);

            context.SaveChanges();
        }
    }
}
