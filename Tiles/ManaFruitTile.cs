using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ManaFruit.Tiles {
    public class ManaFruitTile : ModTile {
        public override void SetStaticDefaults() {
            Main.tileOreFinderPriority[Type] = 810;
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
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.RandomStyleRange = 3;
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(68, 35, 113), this.GetLocalization("MapEntry"));
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY) {
            Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 32, ModContent.ItemType<Items.ManaFruit>());
        }
    }
}
