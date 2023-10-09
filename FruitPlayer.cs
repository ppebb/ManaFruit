using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ManaFruit {
    public class FruitPlayer : ModPlayer {
        public int manaFruits;
        public const int maxManaFruits = 10;

        public override void ResetEffects() {
            Player.statManaMax2 += manaFruits * 10;
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer) {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)Player.whoAmI);
            packet.Write(manaFruits);
            packet.Send(toWho, fromWho);
        }

        public override void SaveData(TagCompound tag) {
            tag["manaFruits"] = manaFruits;
        }

        public override void LoadData(TagCompound tag) {
            manaFruits = tag.GetInt("manaFruits");
        }
    }
}
