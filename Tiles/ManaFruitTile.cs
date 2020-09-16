using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ManaFruit.Tiles
{
    public class ManaFruitTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileValue[Type] = 810;
            Main.tileShine[Type] = 600;
            Main.tileShine2[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileCut[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileSolid[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.RandomStyleRange = 3;
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Mana Fruit");
            AddMapEntry(new Color(68, 35, 113), name);
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 16, 32, ModContent.ItemType<Items.ManaFruit>());
        }
    }
}