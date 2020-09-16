using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ManaFruit.Tiles;
using static Terraria.ModLoader.ModContent;
using System;

namespace ManaFruit
{
	public class ManaFruitWorld : ModWorld
	{
		public override void PreUpdate()
		{
			int maxValue = 20;
			if (Main.expertMode)
				maxValue = 30;
			float num3 = 1.5E-05f * Main.worldRate;
			for (int num62 = 0; num62 < (Main.maxTilesX * Main.maxTilesY) * num3; num62++)
			{
				int num63 = WorldGen.genRand.Next(10, Main.maxTilesX - 10);
				int num64 = WorldGen.genRand.Next((int)Main.worldSurface - 1, Main.maxTilesY - 20);
				int num65 = num63 - 1;
				int num66 = num63 + 2;
				int num67 = num64 - 1;
				int num68 = num64 + 2;
				if (num65 < 10)
					num65 = 10;
				if (num66 > Main.maxTilesX - 10)
					num66 = Main.maxTilesX - 10;

				if (num67 < 10)
					num67 = 10;

				if (num68 > Main.maxTilesY - 10)
					num68 = Main.maxTilesY - 10;

				if (Main.tile[num63, num64] == null)
					continue;

				if (Main.tile[num63, num64].liquid <= 32)
				{
					if (Main.tile[num63, num64].nactive())
					{

						if (Main.tile[num63, num64].type == 23 && !Main.tile[num63, num67].active() && WorldGen.genRand.Next(1) == 0)
						{
							if (Main.netMode == NetmodeID.Server && Main.tile[num63, num67].active())
								NetMessage.SendTileSquare(-1, num63, num67, 1);
						}

						if (Main.tile[num63, num64].type == 32 && WorldGen.genRand.Next(3) == 0)
						{
							int num69 = num63;
							int num70 = num64;
							int num71 = 0;
							if (Main.tile[num69 + 1, num70].active() && Main.tile[num69 + 1, num70].type == 32)
								num71++;

							if (Main.tile[num69 - 1, num70].active() && Main.tile[num69 - 1, num70].type == 32)
								num71++;

							if (Main.tile[num69, num70 + 1].active() && Main.tile[num69, num70 + 1].type == 32)
								num71++;

							if (Main.tile[num69, num70 - 1].active() && Main.tile[num69, num70 - 1].type == 32)
								num71++;

							if (num71 < 3 || Main.tile[num63, num64].type == 23)
							{
								switch (WorldGen.genRand.Next(4))
								{
									case 0:
										num70--;
										break;
									case 1:
										num70++;
										break;
									case 2:
										num69--;
										break;
									case 3:
										num69++;
										break;
								}

								if (!Main.tile[num69, num70].active())
								{
									num71 = 0;
									if (Main.tile[num69 + 1, num70].active() && Main.tile[num69 + 1, num70].type == 32)
										num71++;

									if (Main.tile[num69 - 1, num70].active() && Main.tile[num69 - 1, num70].type == 32)
										num71++;

									if (Main.tile[num69, num70 + 1].active() && Main.tile[num69, num70 + 1].type == 32)
										num71++;

									if (Main.tile[num69, num70 - 1].active() && Main.tile[num69, num70 - 1].type == 32)
										num71++;

									if (num71 < 2)
									{
										int num72 = 7;
										int num73 = num69 - num72;
										int num74 = num69 + num72;
										int num75 = num70 - num72;
										int num76 = num70 + num72;
										bool flag16 = false;
										for (int num77 = num73; num77 < num74; num77++)
										{
											for (int num78 = num75; num78 < num76; num78++)
											{
												if (Math.Abs(num77 - num69) * 2 + Math.Abs(num78 - num70) < 9 && Main.tile[num77, num78].active() && Main.tile[num77, num78].type == 23 && Main.tile[num77, num78 - 1].active() && Main.tile[num77, num78 - 1].type == 32 && Main.tile[num77, num78 - 1].liquid == 0)
												{
													flag16 = true;
													break;
												}
											}
										}

										if (flag16)
										{
											Main.tile[num69, num70].type = 32;
											Main.tile[num69, num70].active(active: true);

											if (Main.netMode == NetmodeID.Server)
												NetMessage.SendTileSquare(-1, num69, num70, 3);
										}
									}
								}
							}
						}


						if (Main.tile[num63, num64].type == 199)
						{
							int type6 = Main.tile[num63, num64].type;
							bool flag17 = false;
							for (int num79 = num65; num79 < num66; num79++)
							{
								for (int num80 = num67; num80 < num68; num80++)
								{
									if ((num63 != num79 || num64 != num80) && Main.tile[num79, num80].active() && Main.tile[num79, num80].type == 0)
									{
										if (Main.tile[num79, num80].type == type6)
										{
											flag17 = true;
										}
									}
								}
							}

							if (Main.netMode == NetmodeID.Server && flag17)
								NetMessage.SendTileSquare(-1, num63, num64, 3);
						}

						if (Main.tile[num63, num64].type == 60)
						{
							if (!Main.tile[num63, num67].active() && WorldGen.genRand.Next(10) == 0)
							{
								if (Main.netMode == NetmodeID.Server && Main.tile[num63, num67].active())
									NetMessage.SendTileSquare(-1, num63, num67, 1);
							}
							else if (WorldGen.genRand.Next(25) == 0 && Main.tile[num63, num67].liquid == 0)
							{
								if (Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 && WorldGen.genRand.Next(60) == 0)
								{
									bool flag18 = true;
									int num81 = 150;
									for (int num82 = num63 - num81; num82 < num63 + num81; num82 += 2)
									{
										for (int num83 = num64 - num81; num83 < num64 + num81; num83 += 2)
										{
											if (num82 > 1 && num82 < Main.maxTilesX - 2 && num83 > 1 && num83 < Main.maxTilesY - 2 && Main.tile[num82, num83].active() && Main.tile[num82, num83].type == 238)
											{
												flag18 = false;
												break;
											}
										}
									}

									if (flag18)
									{
										if (Main.tile[num63, num67].type == 238 && Main.netMode == NetmodeID.Server)
											NetMessage.SendTileSquare(-1, num63, num67, 4);
									}
								}

								if (Main.hardMode && NPC.downedMechBossAny && WorldGen.genRand.Next(maxValue) == 0)
								{
									bool flag19 = true;
									int num84 = 60;
									if (Main.expertMode)
										num84 -= 10;

									for (int num85 = num63 - num84; num85 < num63 + num84; num85 += 2)
									{
										for (int num86 = num64 - num84; num86 < num64 + num84; num86 += 2)
										{
											if (num85 > 1 && num85 < Main.maxTilesX - 2 && num86 > 1 && num86 < Main.maxTilesY - 2 && Main.tile[num85, num86].active() && Main.tile[num85, num86].type == 236)
											{
												flag19 = false;
												break;
											}
										}
									}

									if (flag19)
									{
										PlaceLifeFruits(num63, num67, (ushort)TileType<ManaFruitTile>(), WorldGen.genRand.Next(3), 0);
									}
								}
							}
						}
					}
				}
			}
		}

		public static void PlaceLifeFruits(int X2, int Y2, ushort type, int styleX, int styleY)
		{
			if (styleY > 0 || type == TileType<ManaFruitTile>() || type == 238)
			{
				int num = Y2;
				if (type == 95 || type == 126)
					num++;

				if (X2 < 5 || X2 > Main.maxTilesX - 5 || num < 5 || num > Main.maxTilesY - 5)
					return;

				bool flag = true;
				for (int i = X2 - 1; i < X2 + 1; i++)
				{
					for (int j = num - 1; j < num + 1; j++)
					{
						if (Main.tile[i, j] == null)
							Main.tile[i, j] = new Tile();

						if (Main.tile[i, j].active() && Main.tile[i, j].type != 61 && Main.tile[i, j].type != 62 && Main.tile[i, j].type != 69 && Main.tile[i, j].type != 74 && (type != 236 || Main.tile[i, j].type != 233) && (type != 238 || Main.tile[i, j].type != 233) && (Main.tile[i, j].type != 185 || Main.tile[i, j].frameY != 0))
							flag = false;

						if (type == 98 && Main.tile[i, j].liquid > 0)
							flag = false;
					}

					if (Main.tile[i, num + 1] == null)
						Main.tile[i, num + 1] = new Tile();

					if (!WorldGen.SolidTile(i, num + 1) || Main.tile[i, num + 1].type != 60)
						flag = false;
				}

				if (flag)
				{
					short num2 = 36;
					if (type == TileType<ManaFruitTile>() || type == 238)
						num2 = 0;

					short num3 = (short)(36 * styleX);
					Main.tile[X2 - 1, num - 1].active(active: true);
					Main.tile[X2 - 1, num - 1].frameY = num2;
					Main.tile[X2 - 1, num - 1].frameX = num3;
					Main.tile[X2 - 1, num - 1].type = type;
					Main.tile[X2, num - 1].active(active: true);
					Main.tile[X2, num - 1].frameY = num2;
					Main.tile[X2, num - 1].frameX = (short)(18 + num3);
					Main.tile[X2, num - 1].type = type;
					Main.tile[X2 - 1, num].active(active: true);
					Main.tile[X2 - 1, num].frameY = (short)(num2 + 18);
					Main.tile[X2 - 1, num].frameX = num3;
					Main.tile[X2 - 1, num].type = type;
					Main.tile[X2, num].active(active: true);
					Main.tile[X2, num].frameY = (short)(num2 + 18);
					Main.tile[X2, num].frameX = (short)(18 + num3);
					Main.tile[X2, num].type = type;
				}
			}
			else
			{
				if (X2 < 5 || X2 > Main.maxTilesX - 5 || Y2 < 5 || Y2 > Main.maxTilesY - 5)
					return;

				bool flag2 = true;
				for (int k = X2 - 1; k < X2 + 2; k++)
				{
					for (int l = Y2 - 1; l < Y2 + 1; l++)
					{
						if (Main.tile[k, l] == null)
							Main.tile[k, l] = new Tile();

						if (Main.tile[k, l].active() && Main.tile[k, l].type != 61 && Main.tile[k, l].type != 62 && Main.tile[k, l].type != 69 && Main.tile[k, l].type != 74 && (Main.tile[k, l].type != 185 || Main.tile[k, l].frameY != 0))
							flag2 = false;
					}

					if (Main.tile[k, Y2 + 1] == null)
						Main.tile[k, Y2 + 1] = new Tile();

					if (!WorldGen.SolidTile(k, Y2 + 1) || Main.tile[k, Y2 + 1].type != 60)
						flag2 = false;
				}

				if (flag2)
				{
					short num4 = (short)(54 * styleX);
					Main.tile[X2 - 1, Y2 - 1].active(active: true);
					Main.tile[X2 - 1, Y2 - 1].frameY = 0;
					Main.tile[X2 - 1, Y2 - 1].frameX = num4;
					Main.tile[X2 - 1, Y2 - 1].type = type;
					Main.tile[X2, Y2 - 1].active(active: true);
					Main.tile[X2, Y2 - 1].frameY = 0;
					Main.tile[X2, Y2 - 1].frameX = (short)(num4 + 18);
					Main.tile[X2, Y2 - 1].type = type;
					Main.tile[X2 + 1, Y2 - 1].active(active: true);
					Main.tile[X2 + 1, Y2 - 1].frameY = 0;
					Main.tile[X2 + 1, Y2 - 1].frameX = (short)(num4 + 36);
					Main.tile[X2 + 1, Y2 - 1].type = type;
					Main.tile[X2 - 1, Y2].active(active: true);
					Main.tile[X2 - 1, Y2].frameY = 18;
					Main.tile[X2 - 1, Y2].frameX = num4;
					Main.tile[X2 - 1, Y2].type = type;
					Main.tile[X2, Y2].active(active: true);
					Main.tile[X2, Y2].frameY = 18;
					Main.tile[X2, Y2].frameX = (short)(num4 + 18);
					Main.tile[X2, Y2].type = type;
					Main.tile[X2 + 1, Y2].active(active: true);
					Main.tile[X2 + 1, Y2].frameY = 18;
					Main.tile[X2 + 1, Y2].frameX = (short)(num4 + 36);
					Main.tile[X2 + 1, Y2].type = type;
				}
			}
		}
	}
}