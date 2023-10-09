using Terraria;
using Terraria.ModLoader;
using ManaFruit.Tiles;
using Terraria.ID;

namespace ManaFruit {
    public class ManaFruitWorld : ModSystem {
        public override void PreUpdateWorld() {
            if (!Main.hardMode || !NPC.downedMechBossAny) {
                return;
            }

            float genChance = Main.expertMode ? 0.0095f : 0.006f;
            int iterations = (int)(Main.maxTilesX * Main.maxTilesY * 1.5E-05f * Main.desiredWorldTilesUpdateRate);
            for (int iterator = 0; iterator < iterations; iterator++) {
                int possibleX = WorldGen.genRand.Next(10, Main.maxTilesX - 10);
                int possibleY = WorldGen.genRand.Next((int)Main.worldSurface - 1, Main.maxTilesY - 20);
                if (!WorldGen.InWorld(possibleX, possibleY, 12)) {
                    continue;
                }

                int possibleTop = possibleY - 1;
                Tile possibleTile = Framing.GetTileSafely(possibleX, possibleY);
                if (possibleTile.TileType != TileID.JungleGrass || possibleTile.LiquidAmount > 32 || !possibleTile.HasUnactuatedTile || WorldGen.genRand.NextFloat() >= genChance) {
                    continue;
                }

                Tile possibleTileTop = Framing.GetTileSafely(possibleX, possibleTop);
                if (possibleTileTop.LiquidAmount != 0) {
                    continue;
                }

                bool canGenerateFr00t = true;
                int checkRadius = Main.expertMode ? 60 : 50;
                int fruitScanXBound = possibleX + checkRadius;
                int fruitScanYBound = possibleY + checkRadius;
                for (int fruitScanX = possibleX - checkRadius; fruitScanX < fruitScanXBound; fruitScanX += 2) {
                    for (int fuirtScanY = possibleY - checkRadius; fuirtScanY < fruitScanYBound; fuirtScanY += 2) {
                        if (!WorldGen.InWorld(fruitScanX, fuirtScanY, 2)) {
                            break;
                        }

                        Tile possibleTileTopLeft = Framing.GetTileSafely(fruitScanX, fuirtScanY);

                        if (possibleTileTopLeft.TileType == ModContent.TileType<ManaFruitTile>() && possibleTileTopLeft.HasTile) {
                            canGenerateFr00t = false;
                            break;
                        }
                    }
                }

                if (canGenerateFr00t) {
                    WorldGen.PlaceObject(possibleX, possibleTop, ModContent.TileType<ManaFruitTile>(), true, WorldGen.genRand.Next(3));
                }
            }
        }
    }
}
