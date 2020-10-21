using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ManaFruit
{
	public class FruitPlayer : ModPlayer
	{
		public int manaFruits;
		public const int maxManaFruits = 10;

        public override void ResetEffects()
        {
            player.statManaMax2 += manaFruits * 10;
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = mod.GetPacket();
            packet.Write((byte)player.whoAmI);
            packet.Write(manaFruits);
            packet.Send(toWho, fromWho);
        }

        public override TagCompound Save()
        {
            return new TagCompound
            {
                {"manaFruits", manaFruits}
            };
        }
        public override void Load(TagCompound tag)
        {
            manaFruits = tag.GetInt("manaFruits");
        }
    }
}