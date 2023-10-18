using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.ResourceSets;
using Terraria.ModLoader;

namespace ManaFruit {
    public class ManaFruitMod : Mod {
        public override void Load() {
            IL_PlayerStatsSnapshot.ctor += ctor;
        }

        public delegate void SnapshotDelegate(ref PlayerStatsSnapshot snapshot);

        private void ctor(ILContext il) {
            ILCursor c = new(il);

            if (!c.TryGotoNext(MoveType.Before, i => i.MatchRet())) {
                Logger.Fatal("Unable to apply IL edit to Terraria.GameContent.UI.ResourceSets.PlayerStatsSnapshot.ctor");
                return;
            }

            c.Emit(OpCodes.Ldarg, 0);
            c.EmitDelegate<SnapshotDelegate>((ref PlayerStatsSnapshot snapshot) => {
                int numFruits = Main.LocalPlayer.GetModPlayer<FruitPlayer>().manaFruits;
                int nonFruitMana = (snapshot.ManaMax - (30 * numFruits));
                snapshot.AmountOfManaStars = numFruits + (nonFruitMana / 20);
            });
        }
    }

    public class ManaFruitOverlay : ModResourceOverlay {
        // This field is used to cache vanilla assets used in the CompareAssets helper method further down in this file
        private Dictionary<string, Asset<Texture2D>> vanillaAssetCache = new();

        // These fields are used to cache the result of ModContent.Request<Texture2D>()
        private Asset<Texture2D> starTexture, fancyPanelTexture, barsFillingTexture, barsPanelTexture;

        // Unlike VanillaLifeOverlay, every star is drawn over by this hook.
        public override void PostDrawResource(ResourceOverlayDrawContext context) {
            Asset<Texture2D> asset = context.texture;

            string fancyFolder = "Images/UI/PlayerResourceSets/FancyClassic/";
            string barsFolder = "Images/UI/PlayerResourceSets/HorizontalBars/";

            if (context.resourceNumber >= Main.LocalPlayer.GetModPlayer<FruitPlayer>().manaFruits && !(ModContent.GetInstance<UIConfig>().DrawAllPurple && Main.LocalPlayer.GetModPlayer<FruitPlayer>().manaFruits == 10))
                return;

            // NOTE: CompareAssets is defined below this method's body
            if (asset == TextureAssets.Mana) {
                // Draw over the Classic stars
                DrawClassicFancyOverlay(context);
            }
            else if (CompareAssets(asset, fancyFolder + "Star_Fill")) {
                // Draw over the Fancy stars
                DrawClassicFancyOverlay(context);
            }
            else if (CompareAssets(asset, barsFolder + "MP_Fill")) {
                // Draw over the Bars mana bars
                DrawBarsOverlay(context);
            }
            else if (CompareAssets(asset, fancyFolder + "Star_A") || CompareAssets(asset, fancyFolder + "Star_B") || CompareAssets(asset, fancyFolder + "Star_C") || CompareAssets(asset, fancyFolder + "Star_Single")) {
                // Draw over the Fancy star panels
                DrawFancyPanelOverlay(context);
            }
            else if (CompareAssets(asset, barsFolder + "MP_Panel_Middle")) {
                // Draw over the Bars middle mana panels
                DrawBarsPanelOverlay(context);
            }
        }

        private bool CompareAssets(Asset<Texture2D> existingAsset, string compareAssetPath) {
            // This is a helper method for checking if a certain vanilla asset was drawn
            if (!vanillaAssetCache.TryGetValue(compareAssetPath, out var asset))
                asset = vanillaAssetCache[compareAssetPath] = Main.Assets.Request<Texture2D>(compareAssetPath);

            return existingAsset == asset;
        }

        private void DrawClassicFancyOverlay(ResourceOverlayDrawContext context) {
            // Draw over the Classic / Mana stars
            // "context" contains information used to draw the resource
            // If you want to draw directly on top of the vanilla stars, just replace the texture and have the context draw the new texture
            context.texture = starTexture ??= ModContent.Request<Texture2D>("ManaFruit/UI/OverlayClassic");
            context.Draw();
        }

        // Drawing over the panel backgrounds is not required.
        // This example just showcases changing the "inner" part of the star panels to more closely resemble the example life fruit.
        private void DrawFancyPanelOverlay(ResourceOverlayDrawContext context) {
            // Draw over the Fancy star panels
            string fancyFolder = "Images/UI/PlayerResourceSets/FancyClassic/";

            // The original position refers to the entire panel slice.
            // However, since this overlay only modifies the "inner" portion of the slice (aka the part behind the star),
            // the position should be modified to compensate for the sprite size difference
            Vector2 positionOffset = default;

            if (context.resourceNumber != context.snapshot.AmountOfManaStars && !CompareAssets(context.texture, fancyFolder + "Star_A")) {
                // Any panel that has a panel above AND below it
                // Vanilla texture is "Star_B"
                positionOffset = new Vector2(0, -4);
            }

            // "context" contains information used to draw the resource
            // If you want to draw directly on top of the vanilla stars, just replace the texture and have the context draw the new texture
            context.texture = fancyPanelTexture ??= ModContent.Request<Texture2D>("ManaFruit/UI/OverlayClassicPanel");
            // Due to the replacement texture and the vanilla texture having different dimensions, the source needs to also be modified
            context.source = context.texture.Frame();
            context.position += positionOffset;
            context.Draw();
        }

        private void DrawBarsOverlay(ResourceOverlayDrawContext context) {
            // Draw over the Bars mana bars
            // "context" contains information used to draw the resource
            // If you want to draw directly on top of the vanilla bars, just replace the texture and have the context draw the new texture
            context.texture = barsFillingTexture ??= ModContent.Request<Texture2D>("ManaFruit/UI/OverlayFill");
            context.Draw();
        }

        // Drawing over the panel backgrounds is not required.
        // This example just showcases changing the "inner" part of the bar panels to more closely resemble the example life fruit.
        private void DrawBarsPanelOverlay(ResourceOverlayDrawContext context) {
            // Draw over the Bars middle life panels
            // "context" contains information used to draw the resource
            // If you want to draw directly on top of the vanilla bar panels, just replace the texture and have the context draw the new texture
            context.texture = barsPanelTexture ??= ModContent.Request<Texture2D>("ManaFruit/UI/OverlayFillPanel");
            // Due to the replacement texture and the vanilla texture having different heights, the source needs to also be modified
            context.source = context.texture.Frame();
            // The original position refers to the entire panel slice.
            // However, since this overlay only modifies the "inner" portion of the slice (aka the part behind the bar filling),
            // the position should be modified to compensate for the sprite size difference
            /* context.position.Y += 6; */
            context.Draw();
        }
    }
}
