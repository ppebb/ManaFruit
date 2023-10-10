using ManaFruit.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ManaFruit.Items {
    public class ManaFruit : ModItem {
        public override void SetDefaults() {
            Item.width = 26;
            Item.height = 26;
            Item.maxStack = 99;
            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 30;
            Item.UseSound = SoundID.Item4;
            Item.useAnimation = 30;
            Item.rare = ItemRarityID.Lime;
            Item.value = Item.sellPrice(gold: 2);
        }

        public override bool CanUseItem(Player player) {
            return player.statManaMax == 200 && player.GetModPlayer<FruitPlayer>().manaFruits < FruitPlayer.maxManaFruits;
        }

        public override bool? UseItem(Player player) {
            player.statManaMax2 += 10;
            player.statMana += 10;

            if (Main.myPlayer == player.whoAmI)
                player.ManaEffect(10);

            player.GetModPlayer<FruitPlayer>().manaFruits += 1;
            return true;
        }
    }
}
