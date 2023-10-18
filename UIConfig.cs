using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace ManaFruit;

public class UIConfig : ModConfig {
    public override ConfigScope Mode => ConfigScope.ClientSide;

    [DefaultValue(true)]
    public bool DrawAllPurple;
}
