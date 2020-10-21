using ReLogic.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using MonoMod.Cil;
using System;

namespace ManaFruit
{
	public class ManaFruitMod : Mod
	{
        public static SpriteBatch spriteBatch = Main.spriteBatch;
        public int UI_ScreenAnchorX => Main.screenWidth - 800;
        public override void Load()
		{
			IL.Terraria.Player.LoadPlayer += Player_LoadPlayer;
			IL.Terraria.Player.Update += Player_Update;
			On.Terraria.Main.DrawInterface_Resources_Mana += ManaFruitUI;
		}

		private void Player_Update(ILContext il)
		{
            ILCursor c = new ILCursor(il);

            if (!c.TryGotoNext(MoveType.Before, i => i.MatchLdfld("Terraria.Player", "statManaMax2"), i => i.MatchLdcI4(400)))
			{
                Logger.Fatal("Instruction not found");
                return;
			}

            c.Next.Next.Operand = 500;
		}

		private void Player_LoadPlayer(ILContext il)
		{
            ILCursor c = new ILCursor(il);

            if (!c.TryGotoNext(MoveType.Before, i => i.MatchLdfld("Terraria.Player", "statMana"), i => i.MatchLdcI4(400)))
            {
                Logger.Fatal("Instruction not found");
                return;
			}

            c.Next.Next.Operand = 500;
		}

		private void ManaFruitUI(On.Terraria.Main.orig_DrawInterface_Resources_Mana orig)
        {
            Player player = Main.player[Main.myPlayer];
            int fruits = player.GetModPlayer<FruitPlayer>().manaFruits;
            int UIDisplay_ManaPerStar;
            if (fruits < 10)
                UIDisplay_ManaPerStar = 20 + fruits;
            else
                UIDisplay_ManaPerStar = player.statManaMax2 / 10;
            if (player.ghost || player.statManaMax2 <= 0)
                return;
            Vector2 vector = Main.fontMouseText.MeasureString(Language.GetTextValue("LegacyInterface.2"));
            int num = 50;
            if (vector.X >= 45f)
                num = (int)vector.X + 5;
            spriteBatch.DrawString(Main.fontMouseText, Language.GetTextValue("LegacyInterface.2"), new Vector2(800 - num + UI_ScreenAnchorX, 6f), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, default, 1f, SpriteEffects.None, 0f);
            for (int i = 1; i < player.statManaMax2 / UIDisplay_ManaPerStar + 1; i++)
            {
                int num2;
                bool flag = false;
                float num3 = 1f;
                if (player.statMana >= i * UIDisplay_ManaPerStar)
                {
                    num2 = 255;
                    if (player.statMana == i * UIDisplay_ManaPerStar)
                        flag = true;
                }
                else
                {
                    float num4 = (player.statMana - (i - 1) * UIDisplay_ManaPerStar) / (float)UIDisplay_ManaPerStar;
                    num2 = (int)(30f + 225f * num4);
                    if (num2 < 30)
                        num2 = 30;
                    num3 = num4 / 4f + 0.75f;
                    if (num3 < 0.75)
                        num3 = 0.75f;
                    if (num4 > 0f)
                        flag = true;
                }
                if (flag)
                    num3 += Main.cursorScale - 1f;
                int a = (int)((float)num2 * 0.9);
                if (i > fruits)
                    spriteBatch.Draw(Main.manaTexture, new Vector2(775 + UI_ScreenAnchorX, (30 + Main.manaTexture.Height / 2) + (Main.manaTexture.Height - Main.manaTexture.Height * num3) / 2f + (28 * (i - 1))), new Rectangle(0, 0, Main.manaTexture.Width, Main.manaTexture.Height), new Color(num2, num2, num2, a), 0f, new Vector2(Main.manaTexture.Width / 2, Main.manaTexture.Height / 2), num3, SpriteEffects.None, 0f);
                else
                    spriteBatch.Draw(ModContent.GetTexture("ManaFruit/UI/ManaFruitUI"), new Vector2(775 + UI_ScreenAnchorX, (30 + Main.manaTexture.Height / 2) + (Main.manaTexture.Height - Main.manaTexture.Height * num3) / 2f + (28 * (i - 1))), new Rectangle(0, 0, Main.manaTexture.Width, Main.manaTexture.Height), new Color(num2, num2, num2, a), 0f, new Vector2(Main.manaTexture.Width / 2, Main.manaTexture.Height / 2), num3, SpriteEffects.None, 0f);
            }
        }
    }
}